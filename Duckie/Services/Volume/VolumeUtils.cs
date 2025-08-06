using Duckie.Views;
using System.Runtime.InteropServices;

namespace Duckie.Services.Volume;

public static class VolumeUtils
{
    private static IAudioEndpointVolume _audioEndpointVolume = GetAudioEndpointVolume();
    private static Guid _eventContext = Guid.Empty; // Use an empty GUID for the event context

    private static IAudioEndpointVolume GetAudioEndpointVolume()
    {
        IMMDeviceEnumerator deviceEnumerator = null;
        IMMDevice audioDevice = null;
        IAudioEndpointVolume audioEndpointVolume = null;

        try
        {
            // Create the device enumerator
            deviceEnumerator = (IMMDeviceEnumerator)new MMDeviceEnumeratorComObject();

            // Get the default audio endpoint for rendering
            deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eConsole, out audioDevice);

            // Activate the IAudioEndpointVolume interface
            Guid iid = typeof(IAudioEndpointVolume).GUID;
            audioDevice.Activate(ref iid, (int)CLSCTX.CLSCTX_ALL, IntPtr.Zero, out object o);
            audioEndpointVolume = (IAudioEndpointVolume)o;
        }
        finally
        {
            // Release COM objects
            if (audioDevice != null)
            {
                Marshal.ReleaseComObject(audioDevice);
            }
            if (deviceEnumerator != null)
            {
                Marshal.ReleaseComObject(deviceEnumerator);
            }
        }

        return audioEndpointVolume;
    }

    public static void VolumeUp()
    {
        if (_audioEndpointVolume == null)
        {
            return;
        }

        _audioEndpointVolume.GetMasterVolumeLevelScalar(out var currentVolume);
        var newVolume = Math.Min(1.0f, currentVolume + 0.05f); // Increase by 5%, max 100%
        _audioEndpointVolume.SetMasterVolumeLevelScalar(newVolume, ref _eventContext);

        VolumeOverlayWindow.ShowVolumeOverlay();
    }

    public static void VolumeDown()
    {
        _audioEndpointVolume.GetMasterVolumeLevelScalar(out var currentVolume);
        var newVolume = Math.Max(0.0f, currentVolume - 0.05f); // Decrease by 5%, min 0%
        _audioEndpointVolume.SetMasterVolumeLevelScalar(newVolume, ref _eventContext);

        VolumeOverlayWindow.ShowVolumeOverlay();
    }

    public static void ToggleMute()
    {
        _audioEndpointVolume.GetMute(out var currentMute);
        _audioEndpointVolume.SetMute(!currentMute, ref _eventContext);

        VolumeOverlayWindow.ShowVolumeOverlay();
    }

    /// <summary>
    /// 获取当前音量百分比 (0-100)
    /// </summary>
    public static int GetVolumePercent()
    {
        if (_audioEndpointVolume == null)
        {
            return 0;
        }

        _audioEndpointVolume.GetMasterVolumeLevelScalar(out var volume);
        return (int)(volume * 100);
    }

    /// <summary>
    /// 获取当前静音状态
    /// </summary>
    public static bool IsMuted()
    {
        if (_audioEndpointVolume == null)
        {
            return false;
        }

        _audioEndpointVolume.GetMute(out var isMuted);
        return isMuted;
    }
}
