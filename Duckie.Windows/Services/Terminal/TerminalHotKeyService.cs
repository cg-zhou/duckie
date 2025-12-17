using Duckie.Utils.HotKeys;
using System.Windows.Forms;

namespace Duckie.Windows.Services.Terminal;

/// <summary>
/// 终端相关的热键服务
/// </summary>
public class TerminalHotKeyService : IHotKeyService
{
    public IEnumerable<HotKeyAction> Register()
    {
        yield return new HotKeyAction("在当前文件夹打开终端", KeyModifiers.ALtShift, Keys.Oemtilde, OpenTerminalHere);
    }

    /// <summary>
    /// 在当前文件夹打开终端
    /// </summary>
    private void OpenTerminalHere()
    {
        try
        {
            var currentPath = WindowsUtils.GetCurrentExplorerPath();
            TerminalUtils.OpenAt(currentPath);
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"打开终端失败: {ex.Message}");
        }
    }
}
