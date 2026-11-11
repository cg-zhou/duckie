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
                try { Register(hotKeyAction, hotKeyAction.Modifiers, hotKeyAction.Keys); }
                catch (Exception ex) { Duckie.Shared.Utils.Ui.UiUtils.Warning(ex.Message, "快捷键注册失败"); }
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

            var nativeModifiers = hotKeyAction.NoRepeat ? modifiers | Interop.ModNoRepeat : modifiers;
            if (!Interop.RegisterHotKey(form.Handle, item.Id, nativeModifiers, keys))
                throw new System.ComponentModel.Win32Exception(
                    System.Runtime.InteropServices.Marshal.GetLastWin32Error(),
                    $"无法注册快捷键：{hotKeyAction.Name}。请检查其他程序是否占用了此按键。");

            items.Add(item);

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

    public static bool IsRegistered(string name)
    {
        lock (lockObj) return items.Any(item => item.HotKeyAction.Name == name);
    }
}
