using System.Windows.Forms;

namespace Duckie.Utils.HotKeys
{
    public interface IHotKeyService
    {
        string Name { get; }
        KeyModifiers Modifiers { get; }
        Keys Keys { get; }
        void Run();
    }
}
