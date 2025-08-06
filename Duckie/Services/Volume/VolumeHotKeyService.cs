using Duckie.Utils.HotKeys;
using System.Windows.Forms;

namespace Duckie.Services.Volume;

public class VolumeHotKeyService : IHotKeyService
{
    public IEnumerable<HotKeyAction> Register()
    {
        yield return new HotKeyAction("调小音量", KeyModifiers.Alt | KeyModifiers.Shift, Keys.D8, VolumeUtils.VolumeDown);
        yield return new HotKeyAction("调大音量", KeyModifiers.Alt | KeyModifiers.Shift, Keys.D9, VolumeUtils.VolumeUp);
        yield return new HotKeyAction("静音/取消静音", KeyModifiers.Alt | KeyModifiers.Shift, Keys.D0, VolumeUtils.ToggleMute);
    }
}
