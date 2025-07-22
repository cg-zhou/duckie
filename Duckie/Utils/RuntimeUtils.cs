using System.Runtime.InteropServices;
using System.Text;

namespace Duckie.Utils
{
    public static class MsixPackageUtils
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetCurrentPackageFullName(ref int packageFullNameLength, StringBuilder packageFullName);

        private const int MaxPackageNameLength = 128;

        private const int ERROR_SUCCESS = 0;

        public static bool TryGetMsixPackageName(out string name)
        {
            name = string.Empty;

            try
            {
                var length = MaxPackageNameLength;
                var packageNameBuffer = new StringBuilder(length);

                var result = GetCurrentPackageFullName(ref length, packageNameBuffer);
                if (result != ERROR_SUCCESS)
                {
                    return false;
                }

                name = packageNameBuffer.ToString();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
