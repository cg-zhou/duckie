using Duckie.Shared.Utils;
using System.Windows.Forms;

namespace Duckie.Utils.HotKeys;

public static partial class HotKeyManager
{
    public static IHotKeyService[] GetHotKeyServices()
    {
        return ReflectUtils.Get<IHotKeyService>();
    }

    public static void RegisterServices()
    {
        var hotKeyServices = GetHotKeyServices();
        foreach (var hotKeyService in hotKeyServices)
        {
            foreach (var hotKeyAction in hotKeyService.Register())
            {
                Register(hotKeyAction, hotKeyAction.Modifiers, hotKeyAction.Keys);
            }
        }
    }

    static HotKeyManager()
    {
        form = new HotKeyForm();
        form.KeyPressed += Form_KeyPressed;
    }

    private static void Form_KeyPressed(object sender, int id)
    {
        foreach (var item in items.Where(x => x.Id == id))
        {
            item.HotKeyAction.Action();
        }
    }

    private static int id = 9527;

    private static List<HotKeyItem> items { get; set; } = new List<HotKeyItem>();
    private static HotKeyForm form = new HotKeyForm();
    private static object lockObj = new object();

    public static int Register(HotKeyAction hotKeyAction, KeyModifiers modifiers, Keys keys)
    {
        Unregister(hotKeyAction);

        lock (lockObj)
        {
            ++id;

            var item = new HotKeyItem
            {
                Id = id,
                HotKeyAction = hotKeyAction
            };

            items.Add(item);

            Interop.RegisterHotKey(form.Handle, item.Id, modifiers, keys);

            return id;
        }
    }

    public static void Unregister(HotKeyAction hotKeyAction)
    {
        lock (lockObj)
        {
            var removedIds = new List<int>();
            foreach (var item in items)
            {
                if (item.HotKeyAction.Name == hotKeyAction.Name)
                {
                    removedIds.Add(item.Id);
                    Interop.UnregisterHotKey(form.Handle, item.Id);
                }
            }

            items.RemoveAll(x => removedIds.Contains(x.Id));
        }
    }
}
