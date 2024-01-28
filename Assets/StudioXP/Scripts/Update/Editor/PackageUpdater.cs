using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using UnityEditor;
using UnityEngine;
using Application = UnityEngine.Device.Application;

namespace StudioXP.Scripts.Update.Editor
{
    public static class PackageUpdater
    {
        private static bool _isUpdating;

        private static PackageHash _localPackageHash;
        private static PackageHash _remotePackageHash;
        private static PackageHash _versionPackageHash;

        private static WebClient _client;
        private static List<PackageUpdateOperation> _operations;
        private static int _operationsIter;

        private static int _addCount;
        private static int _deleteCount;
        private static int _replaceCount;
        private static List<string> _log;

        [MenuItem("Studio XP/Update Package")]
        public static void UpdatePackage()
        {
            if (_isUpdating) return;

            _isUpdating = true;
            _localPackageHash = null;
            _remotePackageHash = null;
            _versionPackageHash = null;

            var packageUpdaterFolder = $"{PackagePath.ProjectRoot}/{PackagePath.UpdaterFolder}";
            if (!Directory.Exists(packageUpdaterFolder))
                Directory.CreateDirectory(packageUpdaterFolder);

            var versionPackageHashFile = $"{Application.dataPath}/{PackagePath.UpdaterFolder}/{PackagePath.PackageHashFileVersion}";
            if (!File.Exists(versionPackageHashFile))
            {
                Debug.LogError("Missing version file. Error 3.");
                _isUpdating = false;
                return;
            }

            _versionPackageHash = PackageHashGenerator.ReadFilesHash(new StreamReader(versionPackageHashFile));
            if (_versionPackageHash == null)
            {
                Debug.LogError("Invalid version hash file. Error 4.");
                _isUpdating = false;
                return;
            }

            _client ??= new WebClient();

            var destination = $"{packageUpdaterFolder}/{PackagePath.PackageHashFileRemote}";
            if (File.Exists(destination))
            {
                try
                {
                    File.Delete(destination);
                }
                catch
                {
                    _isUpdating = false;
                    Debug.LogError("Error while updating. Wait then retry. Error 5.");
                    return;
                }
            }

            _client.DownloadFileCompleted += DownloadPackageHashCompleted;
            _client.DownloadFileAsync(
                new Uri($"{PackagePath.PackageRootURL}/{PackagePath.UpdaterFolder}/{PackagePath.PackageHashFileLocal}"),
                destination);
        }

        private static void SetLocalPackageHash(PackageHash packageHash)
        {
            _localPackageHash = packageHash;
            UpdatePackageStart();
        }

        private static void SetRemotePackageHash(PackageHash packageHash)
        {
            _remotePackageHash = packageHash;
            if (_remotePackageHash.Time <= _versionPackageHash.Time)
            {
                Debug.Log("Package is already at the latest version.");
                _isUpdating = false;
                return;
            }

            PackageHashGenerator.GetLocalFilesHash(SetLocalPackageHash);
        }

        private static void DownloadPackageHashCompleted(object sender, AsyncCompletedEventArgs args)
        {
            _client.DownloadFileCompleted -= DownloadPackageHashCompleted;

            if (args.Cancelled)
            {
                Debug.LogError("PackageHash file download cancelled.");
                _isUpdating = false;
                return;
            }

            if (args.Error != null)
            {
                Debug.LogError(args.Error.ToString());
                _isUpdating = false;
                return;
            }

            var remotePackageHashFile =
                $"{PackagePath.ProjectRoot}/{PackagePath.UpdaterFolder}/{PackagePath.PackageHashFileRemote}";
            SetRemotePackageHash(PackageHashGenerator.ReadFilesHash(new StreamReader(remotePackageHashFile)));
        }

        private static void UpdatePackageStart()
        {
            AssetDatabase.DisallowAutoRefresh();
            GenerateOperations();

            _operationsIter = 0;
            _addCount = 0;
            _replaceCount = 0;
            _deleteCount = 0;
            _log = new List<string>();

            UpdatePackageUpdate();
        }

        private static void UpdatePackageUpdate()
        {
            if (_operationsIter >= _operations.Count)
            {
                UpdatePackageEnd();
                return;
            }

            var operation = _operations[_operationsIter++];
            EditorUtility.DisplayProgressBar("Downloading update", operation.RelativePath,
                (float) _operationsIter / _operations.Count);

            switch (operation.Type)
            {
                case PackageUpdateOperation.OperationType.Create:
                    _addCount++;
                    break;
                case PackageUpdateOperation.OperationType.Delete:
                    _deleteCount++;
                    break;
                case PackageUpdateOperation.OperationType.Replace:
                    _replaceCount++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _log.Add($"{operation.Type} : {operation.RelativePath}");
            operation.Process(UpdatePackageUpdate);
        }

        private static void UpdatePackageEnd()
        {
            var updaterFolder = $"{PackagePath.ProjectRoot}/{PackagePath.UpdaterFolder}";
            var versionFile =
                $"{Application.dataPath}/{PackagePath.UpdaterFolder}/{PackagePath.PackageHashFileVersion}";
            
            if(File.Exists(versionFile))
                File.Delete(versionFile);
            
            File.Move($"{updaterFolder}/{PackagePath.PackageHashFileRemote}",
                versionFile);

            using var writer = new StreamWriter(PackagePath.PackageUpdaterLogFile);
            foreach (var log in _log)
                writer.WriteLine(log);

            Debug.Log(
                $"{_addCount} new files created.\n{_deleteCount} files deleted.\n{_replaceCount} files replaced.\n" +
                $"All deleted and replaced files were renamed with a .bak# extension.\nSee Logs/package_updater.log for details.");

            EditorUtility.ClearProgressBar();
            AssetDatabase.AllowAutoRefresh();
            AssetDatabase.Refresh();
            _isUpdating = false;
        }

        private static void GenerateOperations()
        {
            var localHash = _localPackageHash.Hash;
            var remoteHash = _remotePackageHash.Hash;
            var remoteHashBak = _versionPackageHash.Hash;

            _operations = (from file in remoteHashBak.Keys
                where !remoteHash.ContainsKey(file) && localHash.ContainsKey(file)
                select new PackageUpdateOperation()
                    {RelativePath = file, Type = PackageUpdateOperation.OperationType.Delete}).ToList();

            foreach (var file in remoteHash.Keys)
            {
                if (remoteHashBak.ContainsKey(file))
                {
                    if (GetHash(remoteHash, file) != GetHash(remoteHashBak, file) && GetHash(remoteHash, file) != GetHash(localHash, file))
                    {
                        _operations.Add(new PackageUpdateOperation()
                            {RelativePath = file, Type = PackageUpdateOperation.OperationType.Replace});
                    }
                }
                else
                {
                    if (localHash.ContainsKey(file))
                    {
                        if (GetHash(remoteHash, file) != GetHash(localHash, file))
                        {
                            _operations.Add(new PackageUpdateOperation()
                                {RelativePath = file, Type = PackageUpdateOperation.OperationType.Replace});
                        }
                    }
                    else
                    {
                        _operations.Add(new PackageUpdateOperation()
                            {RelativePath = file, Type = PackageUpdateOperation.OperationType.Create});
                    }
                }
            }
        }

        private static string GetHash(Dictionary<string, string> hashDict, string key)
        {
            return hashDict.ContainsKey(key) ? hashDict[key] : null;
        }
    }
}
