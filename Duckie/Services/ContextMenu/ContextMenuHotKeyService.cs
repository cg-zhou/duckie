using Duckie.Utils.HotKeys;
using System.Windows.Forms;

namespace Duckie.Services.ContextMenu;

public class ContextMenuHotKeyService : IHotKeyService
{
    public IEnumerable<HotKeyAction> Register()
    {
        yield return new HotKeyAction("�л��˵����", KeyModifiers.ALtShift, Keys.D5, ContextMenuUtils.ToggleContextMenuStyle);
    }
}
