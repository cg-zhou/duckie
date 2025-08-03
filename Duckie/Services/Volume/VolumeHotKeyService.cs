using Duckie.Utils.HotKeys;
using System.Windows.Forms;

namespace Duckie.Services.Volume;

public class VolumeHotKeyService : IHotKeyService
{
    public HotKeyAction[] Register()
    {
        return [
            new HotKeyAction("��С����", KeyModifiers.Alt | KeyModifiers.Shift, Keys.D8, VolumeUtils.VolumeDown),
            new HotKeyAction("��������", KeyModifiers.Alt | KeyModifiers.Shift, Keys.D9, VolumeUtils.VolumeUp),
            new HotKeyAction("����/ȡ������", KeyModifiers.Alt | KeyModifiers.Shift, Keys.D0, VolumeUtils.ToggleMute),
            ];
    }
}
