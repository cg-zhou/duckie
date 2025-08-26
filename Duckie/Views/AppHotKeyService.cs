using Duckie.Shared;
using Duckie.Shared.Utils.Ui;
using Duckie.Shared.Views;
using Duckie.Utils.HotKeys;
using System.Windows.Forms;

namespace Duckie.Views;

public class AppHotKeyService : IHotKeyService
{
    public IEnumerable<HotKeyAction> Register()
    {
        yield return new HotKeyAction("Show/Hide Duckie", KeyModifiers.ALtShift, Keys.E, TogglApp);
        yield return new HotKeyAction("Exit Duckie", KeyModifiers.ALtShift, Keys.Q, ExitApp);
    }

    public void TogglApp()
    {
        UiUtils.BeginInvoke((AppEnv.MainWindow as IMainWindow).ToggleWindow);
    }

    public void ExitApp()
    {
        UiUtils.BeginInvoke(AppEnv.MainWindow.Close);
    }
}