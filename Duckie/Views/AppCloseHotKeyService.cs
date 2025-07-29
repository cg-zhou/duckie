using Duckie.Utils.HotKeys;
using Duckie.Utils.Ui;
using System.Windows.Forms;

namespace Duckie
{
    public class AppCloseHotKeyService : IHotKeyService
    {
        public string Name => "Exit Duckie";

        public KeyModifiers Modifiers => KeyModifiers.Alt | KeyModifiers.Shift;

        public Keys Keys => Keys.Q;

        public void Run()
        {
            UiUtils.BeginInvoke(() =>
            {
                App.MainWindow.Close();
            });
        }
    }
}