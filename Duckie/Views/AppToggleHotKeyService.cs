using Duckie.Utils.HotKeys;
using Duckie.Utils.Ui;
using System.Windows.Forms;

namespace Duckie
{
    public class AppToggleHotKeyService : IHotKeyService
    {
        public string Name => "Show/Hide Duckie";

        public KeyModifiers Modifiers => KeyModifiers.Alt | KeyModifiers.Shift;

        public Keys Keys => Keys.E;

        public void Run()
        {
            UiUtils.BeginInvoke(() =>
            {
                App.MainWindow.Toggle();
            });
        }
    }
}