using System.Windows;

namespace Duckie.Utils.Monitor;

public class MonitorInfo
{
    /// <summary>
    /// Scale factor
    /// </summary>
    public double ScaleFactor { get; set; }

    /// <summary>
    /// Dots Per Inch
    /// </summary>
    public uint Dpi { get; set; }

    /// <summary>
    /// 物理像素工作区
    /// </summary>
    public Rect PhysicalWorkArea { get; set; }

    /// <summary>
    /// 逻辑像素工作区 (WPF单位)
    /// </summary>
    public Rect LogicalWorkArea { get; set; }
}
