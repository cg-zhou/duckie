using Duckie.Utils.HotKeys;
using System.Diagnostics;
using System.Windows.Forms;

namespace Duckie.Windows.Services.Terminal;

public class TerminalHotKeyService : IHotKeyService
{
    public IEnumerable<HotKeyAction> Register()
    {
        yield return new HotKeyAction("在当前文件夹打开终端", KeyModifiers.Win, Keys.Oemtilde, OpenTerminalHere);
    }

    private void OpenTerminalHere()
    {
        try
        {
            var currentPath = WindowsUtils.GetActiveExplorerPath();
            TerminalUtils.OpenAt(currentPath);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"打开终端失败: {ex.Message}");
        }
    }
}
