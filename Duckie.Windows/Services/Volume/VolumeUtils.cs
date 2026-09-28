using System.Runtime.InteropServices;

namespace Duckie.Services.Volume;

public static class VolumeUtils
{
    private const uint WM_APPCOMMAND = 0x0319;
    private const int APPCOMMAND_VOLUME_MUTE = 8;
    private const int APPCOMMAND_VOLUME_DOWN = 9;
    private const int APPCOMMAND_VOLUME_UP = 10;

    private static IAudioEndpointVolume _audioEndpointVolume = GetAudioEndpointVolume();

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(
        IntPtr hWnd,
        uint message,
        IntPtr wParam,
        IntPtr lParam);

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
        SendVolumeCommand(APPCOMMAND_VOLUME_UP);
    }

    public static void VolumeDown()
    {
        SendVolumeCommand(APPCOMMAND_VOLUME_DOWN);
    }

    public static void ToggleMute()
    {
        SendVolumeCommand(APPCOMMAND_VOLUME_MUTE);
    }

    private static void SendVolumeCommand(int command)
    {
        var target = GetForegroundWindow();
        if (target == IntPtr.Zero)
        {
            return;
        }

        SendMessage(target, WM_APPCOMMAND, IntPtr.Zero, (IntPtr)(command << 16));
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
