using Duckie.Utils.HotKeys;
using System.Linq;
using System.Windows.Forms;

namespace Duckie.Services.PacManager
{
    public class PacManagerHotKeyService : IHotKeyService
    {
        public string Name => "�л� PAC";

        KeyModifiers IHotKeyService.Modifiers => KeyModifiers.Alt | KeyModifiers.Shift;

        public Keys Keys => Keys.D1;

        public void Run()
        {
            var pacConfigs = PacManagerService.GetAllPacConfigs();

            var currentPacConfig = PacManagerService.GetCurrentPacConfig();
            var lastIndex = pacConfigs.ToList().FindLastIndex(x => x.Uri == currentPacConfig?.Uri);
            if (lastIndex < 0 && pacConfigs.Any())
            {
                PacManagerService.ApplyPacConfig(pacConfigs[0]);
            }
            else if (lastIndex < pacConfigs.Length - 1)
            {
                PacManagerService.ApplyPacConfig(pacConfigs[lastIndex + 1]);
            }
            else if (lastIndex == pacConfigs.Length - 1)
            {
                PacManagerService.ApplyPacConfig(null);
            }
        }
    }
}
