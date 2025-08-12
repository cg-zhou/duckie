using System.Runtime.InteropServices;

namespace Duckie.Utils.Registry;

static partial class RegistryUtils
{
    public static class Pac
    {
        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);

        private const int INTERNET_OPTION_REFRESH = 37;
        private const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        private const int INTERNET_OPTION_PROXY = 38;

        private const string pacKey = @"Software\Microsoft\Windows\CurrentVersion\Internet Settings";
        private const string name = "AutoConfigURL";
        public static string Get()
        {
            return GetCurrentUserValue(pacKey, name);
        }

        public static void Set(string pacFileUrl)
        {
            SetCurrentUserValue(pacKey, name, pacFileUrl);

            // Notify the system that the proxy settings have changed
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_PROXY, IntPtr.Zero, 0);
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
        }
    }
}
