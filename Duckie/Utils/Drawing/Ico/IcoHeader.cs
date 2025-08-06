using System.Runtime.InteropServices;

namespace Duckie.Utils.Drawing.Ico;

[StructLayout(LayoutKind.Sequential)]
internal struct IcoHeader
{
    public ushort Reserved { get; set; }
    public ImageType ImageType { get; set; }
    public ushort Numbers { get; set; }
}
