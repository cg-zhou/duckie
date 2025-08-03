using Duckie.Utils.HotKeys;
using Duckie.Utils.Ui;
using System.Windows.Forms;

namespace Duckie;

public class AppHotKeyService : IHotKeyService
{
    public HotKeyAction[] Register()
    {
        return [
            new HotKeyAction("Show/Hide Duckie", KeyModifiers.ALtShift, Keys.E, TogglApp),
            new HotKeyAction("Exit Duckie", KeyModifiers.ALtShift, Keys.Q, ExitApp)
            ];
    }

    public void TogglApp()
    {
        UiUtils.BeginInvoke(() =>
        {
            App.MainWindow.Toggle();
        });
    }

    public void ExitApp()
    {
        UiUtils.BeginInvoke(() =>
        {
            App.MainWindow.Close();
        });
    }
}