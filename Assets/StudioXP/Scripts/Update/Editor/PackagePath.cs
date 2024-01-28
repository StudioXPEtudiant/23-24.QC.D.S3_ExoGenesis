using System.IO;
using UnityEngine;
using VisualDesignCafe.Packages;

namespace StudioXP.Scripts.Update.Editor
{
    public static class PackagePath
    {
        public const int Version = 1000;
        
        public const string AssetsFolder = "Assets";
        public const string PackageFolder = "Packages";
        public const string ProjectSettingsFolder = "ProjectSettings";
        public const string UpdaterFolder = "PackageUpdater";
        
        private const string PackageHashFile = "package_hash";
        private const string PackageHashFileLocalExt = ".local";
        private const string PackageHashFileRemoteExt = ".remote";
        private const string PackageHashFileVersionExt = ".version";

        private const string RepositoryRootURL = "https://git.studioxp.ca/Unity";
        private const string PackageName = "SurvivalRPG";
        //private const string PackageBranch = "master";
        private const string PackageBranch = "Concepteur-000";
        //private const string PackageBranch = "Graphiste-000";
        //private const string PackageBranch = "Programmeur-000";
        
        public static string PackageRootURL => $"{RepositoryRootURL}/{PackageName}/raw/branch/{PackageBranch}";
        public static string ProjectRoot => Directory.GetParent(Application.dataPath)?.FullName;
        public static string PackageHashFileLocal => $"{PackageHashFile}{PackageHashFileLocalExt}";
        public static string PackageHashFileRemote => $"{PackageHashFile}{PackageHashFileRemoteExt}";
        public static string PackageHashFileVersion => $"{PackageHashFile}{PackageHashFileVersionExt}";
        public static string PackageUpdaterLogFile => $"{ProjectRoot}/Logs/package_updater.log";

        public static string GetProjectRelativePath(string path)
        {
            return ToStandardURL(Path.GetRelativePath(ProjectRoot, path));
        }

        public static string ToStandardURL(string path)
        {
            return path.Replace(@"\", "/");
        }
    }
}
