using Duckie.Services.Volume;
using Duckie.Shared.Utils.Ui;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Duckie.Views;

public partial class VolumeManagerView : UserControl
{
    private readonly DispatcherTimer _volumeUpdateTimer;

    public VolumeManagerView()
    {
        InitializeComponent();
        
        // Initialize volume update timer
        _volumeUpdateTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(500)
        };
        _volumeUpdateTimer.Tick += VolumeUpdateTimer_Tick;
        _volumeUpdateTimer.Start();
        
        // Initial volume update
        UpdateVolumeDisplay();
    }

    private void VolumeUpdateTimer_Tick(object sender, EventArgs e)
    {
        UpdateVolumeDisplay();
    }

    private void UpdateVolumeDisplay()
    {
        try
        {
            var volume = VolumeUtils.GetVolumePercent();
            var isMuted = VolumeUtils.IsMuted();
            
            UiUtils.BeginInvoke(() =>
            {
                VolumeProgressBar.Value = volume;
                VolumePercentText.Text = $"{volume:F0}%";
                
                // Update mute button text
                var muteButton = FindMuteButton();
                if (muteButton != null)
                {
                    muteButton.Content = isMuted ? "取消静音" : "静音";
                }
            });
        }
        catch (Exception ex)
        {
            // Handle volume access errors silently
            System.Diagnostics.Debug.WriteLine($"Volume update error: {ex.Message}");
        }
    }

    private Button FindMuteButton()
    {
        // Find the mute button in the visual tree
        return FindVisualChild<Button>(this, btn => btn.Content?.ToString()?.Contains("静音") == true);
    }

    private static T FindVisualChild<T>(DependencyObject parent, Func<T, bool> predicate = null) where T : DependencyObject
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            if (child is T typedChild && (predicate == null || predicate(typedChild)))
            {
                return typedChild;
            }

            var result = FindVisualChild<T>(child, predicate);
            if (result != null)
                return result;
        }
        return null;
    }

    private void VolumeDown_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            VolumeUtils.VolumeDown();
            UpdateVolumeDisplay();
        }
        catch (Exception ex)
        {
            UiUtils.Warning($"音量调节失败: {ex.Message}", "音量控制");
        }
    }

    private void VolumeUp_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            VolumeUtils.VolumeUp();
            UpdateVolumeDisplay();
        }
        catch (Exception ex)
        {
            UiUtils.Warning($"音量调节失败: {ex.Message}", "音量控制");
        }
    }

    private void ToggleMute_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            VolumeUtils.ToggleMute();
            UpdateVolumeDisplay();
        }
        catch (Exception ex)
        {
            UiUtils.Warning($"静音切换失败: {ex.Message}", "音量控制");
        }
    }

    private void TestOverlay_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // Test the volume overlay by showing current volume
            VolumeUtils.ShowVolumeOverlay();
        }
        catch (Exception ex)
        {
            UiUtils.Warning($"悬浮提示测试失败: {ex.Message}", "音量控制");
        }
    }
}
