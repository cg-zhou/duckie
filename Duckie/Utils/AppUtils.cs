using System;
using System.IO;
using System.Reflection;

namespace Duckie.Utils
{
    internal static class AppUtils
    {
        static AppUtils()
        {
            EntryAssembly = Assembly.GetEntryAssembly();
            AppName = EntryAssembly.GetName().Name;

            var userAppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            AppDataFolder = Path.Combine(userAppDataFolder, AppName);

            EntryAssemblyPath = EntryAssembly.Location;
            EntryAssemblyName = Path.GetFileName(EntryAssemblyPath);
            EntryAssemblyNameWithoutExtension = Path.GetFileNameWithoutExtension(EntryAssemblyPath);
        }

        public static string AppName { get; }
        public static string AppDataFolder { get; }

        public static Assembly EntryAssembly { get; }
        public static string EntryAssemblyPath { get; }
        public static string EntryAssemblyName { get; }
        public static string EntryAssemblyNameWithoutExtension { get; }

        public static bool TryGetResourceStream(string fileName, out Stream stream)
        {
            var resourceName = $"{EntryAssemblyNameWithoutExtension}.resources.{fileName}";
            stream = EntryAssembly.GetManifestResourceStream(resourceName);
            return stream != null;
        }
    }
}
