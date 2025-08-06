using Duckie.Utils.Monitor;
using System.Windows;

namespace Duckie.Views;

public partial class VolumeOverlayWindow : Window
{
    private static VolumeOverlayWindow _instance;

    public VolumeOverlayWindow()
    {
        InitializeComponent();
        VolumeOverlayControl.OnHidden += OnOverlayHidden;

        // 设置窗口位置到屏幕中央偏上
        PositionWindow();
    }

    /// <summary>
    /// 获取单例实例
    /// </summary>
    public static VolumeOverlayWindow Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new VolumeOverlayWindow();
            }
            return _instance;
        }
    }

    /// <summary>
    /// 显示音量悬浮窗
    /// </summary>
    public static void ShowVolumeOverlay()
    {
        var instance = Instance;

        // 重新定位窗口（防止屏幕分辨率变化）
        instance.PositionWindow();

        // 显示窗口和控件
        instance.Show();
        instance.VolumeOverlayControl.ShowVolume();
    }

    /// <summary>
    /// 设置窗口位置
    /// </summary>
    private void PositionWindow()
    {
        var displayInfo = MonitorUtils.GetPrimaryDisplayInfo();

        Left = displayInfo.LogicalWorkArea.Width - Width - 20;
        Top = displayInfo.LogicalWorkArea.Height - Height - 20;
    }

    /// <summary>
    /// 悬浮窗隐藏时的回调
    /// </summary>
    private void OnOverlayHidden(object sender, EventArgs e)
    {
        // 隐藏窗口但不关闭，以便重复使用
        Hide();
    }

    /// <summary>
    /// 窗口关闭时的处理
    /// </summary>
    protected override void OnClosed(EventArgs e)
    {
        VolumeOverlayControl?.Dispose();
        _instance = null;
        base.OnClosed(e);
    }

    /// <summary>
    /// 防止窗口获得焦点
    /// </summary>
    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);

        // 设置窗口样式，防止获得焦点
        var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
        var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
        SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_NOACTIVATE);
    }

    #region Win32 API
    private const int GWL_EXSTYLE = -20;
    private const int WS_EX_NOACTIVATE = 0x08000000;

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
    #endregion
}
