using Duckie.Shared.Utils.Localization;
using Duckie.Utils.HotKeys;
using System.Windows.Forms;

namespace Duckie.Services.Volume;

public class VolumeHotKeyService : IHotKeyService
{
    public IEnumerable<HotKeyAction> Register()
    {
        yield return new HotKeyAction("volume.down", LocKey.HotKey_VolumeDown, KeyModifiers.ALtShift, Keys.D8, VolumeUtils.VolumeDown);
        yield return new HotKeyAction("volume.up", LocKey.HotKey_VolumeUp, KeyModifiers.ALtShift, Keys.D9, VolumeUtils.VolumeUp);
        yield return new HotKeyAction("volume.mute", LocKey.HotKey_ToggleMute, KeyModifiers.ALtShift, Keys.D0, VolumeUtils.ToggleMute);
    }
}
