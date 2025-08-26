using Duckie.Utils.HotKeys;
using System.Windows.Forms;

namespace Duckie.Services.PacManager;

public class PacManagerHotKeyService : IHotKeyService
{
    public IEnumerable<HotKeyAction> Register()
    {
        yield return new HotKeyAction("ÇÐ»» PAC", KeyModifiers.ALtShift, Keys.D1, Run);
    }

    private void Run()
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
