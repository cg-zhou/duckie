namespace Duckie.Utils.Registry
{
    internal static partial class RegistryUtils
    {
        public static class StartUp
        {
            private const string runKey = @"Software\Microsoft\Windows\CurrentVersion\Run";

            public static bool Get()
            {
                var exePath = AppUtils.EntryAssemblyPath;
                var name = AppUtils.EntryAssemblyNameWithoutExtension;

                var isStartup = !string.IsNullOrWhiteSpace(GetCurrentUserValue(runKey, name));
                return isStartup;
            }

            public static void Set(bool setAsStartup)
            {
                var exePath = AppUtils.EntryAssemblyPath;
                var name = AppUtils.EntryAssemblyNameWithoutExtension;

                var value = setAsStartup ? exePath : string.Empty;
                SetCurrentUserValue(runKey, name, value);
            }
        }
    }
}
