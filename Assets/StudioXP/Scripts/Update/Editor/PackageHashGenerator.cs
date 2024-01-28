//#define SKIP_LOCAL_HASH

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using Application = UnityEngine.Device.Application;

namespace StudioXP.Scripts.Update.Editor
{
    public static class PackageHashGenerator
    {
        private static List<string> _files;
        private static int _fileIter;

        private static bool _calculationInProgress;
        private static PackageHash _localPackageHash;
        private static Action<PackageHash> _operationDone;
        private static bool _saveVersion;

        [MenuItem("Tools/Studio XP/Generate Package Version")]
        public static void GeneratePackageVersion()
        {
            GeneratePackageHashStart(true);
        }

        public static void GetLocalFilesHash(Action<PackageHash> operationDone)
        {
            if (_calculationInProgress)
            {
                _operationDone += operationDone;
                return;
            }

            _operationDone = operationDone;
#if SKIP_LOCAL_HASH
            var packageHash =
 ReadFilesHash(new StreamReader($"{PackagePath.ProjectRoot}/{PackagePath.UpdaterFolder}/{PackagePath.PackageHashFileLocal}"));
            _operationDone.Invoke(packageHash);
#else
            GeneratePackageHashStart(false);
#endif
        }

        public static PackageHash ReadFilesHash(StreamReader reader)
        {
            var version = Convert.ToInt32(reader.ReadLine());
            if (version == 1000)
            {
                var packageHash = new PackageHash
                {
                    Time = Convert.ToInt64(reader.ReadLine()),
                    Hash = new Dictionary<string, string>()
                };

                while (reader.Peek() >= 0)
                    packageHash.Hash.Add(
                        reader.ReadLine() ??
                        throw new InvalidOperationException("The package file is invalid. Error 2."),
                        reader.ReadLine());

                reader.Close();
                return packageHash;
            }

            throw new InvalidDataException("The package file is invalid. Error 1.");
        }

        private static void GeneratePackageHashStart(bool saveVersion)
        {
            AssetDatabase.DisallowAutoRefresh();
            _files = Directory.EnumerateFiles($"{PackagePath.ProjectRoot}/{PackagePath.AssetsFolder}", "*.*",
                SearchOption.AllDirectories).ToList();
            _files.AddRange(Directory.EnumerateFiles($"{PackagePath.ProjectRoot}/{PackagePath.PackageFolder}", "*.*",
                SearchOption.AllDirectories));
            _files.AddRange(Directory.EnumerateFiles($"{PackagePath.ProjectRoot}/{PackagePath.ProjectSettingsFolder}",
                "*.*", SearchOption.AllDirectories));
            _localPackageHash = new PackageHash {Hash = new Dictionary<string, string>()};
            _fileIter = 0;
            _calculationInProgress = true;
            _saveVersion = saveVersion;

            EditorApplication.update += GeneratePackageHashUpdate;
        }

        private static void GeneratePackageHashUpdate()
        {
            if (!_calculationInProgress) return;

            if (_fileIter >= _files.Count)
            {
                GeneratePackageHashEnd();
                return;
            }

            var file = PackagePath.ToStandardURL(_files[_fileIter++]);
            EditorUtility.DisplayProgressBar("Analysing local package's changes", file,
                (float) _fileIter / _files.Count);
            GeneratePackageHash(file);
        }

        private static void GeneratePackageHashEnd()
        {
            EditorApplication.update -= GeneratePackageHashUpdate;

            _localPackageHash.Time = DateTime.Now.ToFileTimeUtc();

            WritePackage($"{PackagePath.ProjectRoot}/{PackagePath.UpdaterFolder}/{PackagePath.PackageHashFileLocal}");
            if (_saveVersion)
            {
                WritePackage($"{Application.dataPath}/{PackagePath.UpdaterFolder}/{PackagePath.PackageHashFileVersion}");
                WritePackage($"{PackagePath.ProjectRoot}/{PackagePath.UpdaterFolder}/{PackagePath.PackageHashFileVersion}");
            }
            
            EditorUtility.ClearProgressBar();

            _calculationInProgress = false;
            _operationDone?.Invoke(_localPackageHash);
            AssetDatabase.AllowAutoRefresh();
        }

        private static void WritePackage(string path)
        {
            string directory = Path.GetDirectoryName(path);
            string filename = Path.GetFileName(path);
            
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            /*using var writeStream =
                new StreamWriter(
                    );*/
            
            using var writeStream = new StreamWriter(path);

            writeStream.WriteLine(PackagePath.Version.ToString());
            writeStream.WriteLine(_localPackageHash.Time.ToString());
            foreach (var key in _localPackageHash.Hash.Keys)
            {
                writeStream.WriteLine(key);
                writeStream.WriteLine(_localPackageHash.Hash[key]);
            }
        }

        private static void GeneratePackageHash(string file)
        {
            using var sha512 = SHA512.Create();
            using var readStream = File.OpenRead(file);
            _localPackageHash.Hash.Add(PackagePath.GetProjectRelativePath(file), ToHex(sha512.ComputeHash(readStream)));
        }
        
        private static string ToHex(IReadOnlyCollection<byte> bytes)
        {
            var result = new StringBuilder(bytes.Count*2);

            foreach (var t in bytes)
                result.Append(t.ToString("X2"));

            return result.ToString();
        }
    }
}
