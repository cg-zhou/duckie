using Duckie.Utils.HotKeys;
using System.Windows.Forms;

namespace Duckie.Services.Clipboard;

public class ClipboardHotKeyService : IHotKeyService
{
    public IEnumerable<HotKeyAction> Register()
    {
        yield return new HotKeyAction("���������", KeyModifiers.ALtShift, Keys.PageUp, CloudClipboard.UploadClipboardContent);
    }
}
