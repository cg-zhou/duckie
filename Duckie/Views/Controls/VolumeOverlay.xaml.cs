using Duckie.Services.Volume;
using Duckie.Views.Common;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Duckie.Views.Controls;

public partial class VolumeOverlay : UserControl
{
    private DispatcherTimer _hideTimer;
    private Storyboard _fadeOutAnimation;

    public event EventHandler OnHidden;

    public VolumeOverlay()
    {
        InitializeComponent();
        InitializeAnimations();
        InitializeTimer();
    }

    private void InitializeAnimations()
    {
        _fadeOutAnimation = (Storyboard)Resources["FadeOutAnimation"];
    }

    private void InitializeTimer()
    {
        _hideTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1.5)
        };
        _hideTimer.Tick += HideTimer_Tick;
    }

    /// <summary>
    /// 显示音量悬浮窗
    /// </summary>
    public void ShowVolume()
    {
        // 停止所有正在进行的动画
        _fadeOutAnimation.Stop(this);

        // 停止计时器
        _hideTimer.Stop();

        UpdateVolumeDisplay();

        // 确保窗口可见并重置透明度
        Opacity = 1;

        // 重新开始计时器
        _hideTimer.Start();
    }

    /// <summary>
    /// 更新音量显示
    /// </summary>
    private void UpdateVolumeDisplay()
    {
        var volumePercent = VolumeUtils.GetVolumePercent();
        var isMuted = VolumeUtils.IsMuted();

        // 更新文本
        VolumePercentText.Text = $"{volumePercent}%";

        // 更新图标
        if (isMuted)
        {
            volumeIcon.IconType = IconType.SpeakerX;
        }
        else if (volumePercent > 50)
        {
            volumeIcon.IconType = IconType.SpeakerHigh;
        }
        else if (volumePercent > 0)
        {
            volumeIcon.IconType = IconType.SpeakerLow;
        }
        else
        {
            volumeIcon.IconType = IconType.SpeakerNone;
        }

        // 更新进度条
        volumeBar.Width = volumePercent;
    }

    /// <summary>
    /// 计时器到期，开始淡出
    /// </summary>
    private void HideTimer_Tick(object sender, EventArgs e)
    {
        _hideTimer.Stop();

        _fadeOutAnimation.Begin(this);
    }

    /// <summary>
    /// 淡出动画完成
    /// </summary>
    private void FadeOutAnimation_Completed(object sender, EventArgs e)
    {
        OnHidden?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// 清理资源
    /// </summary>
    public void Dispose()
    {
        _hideTimer?.Stop();
        _hideTimer = null;
    }
}
