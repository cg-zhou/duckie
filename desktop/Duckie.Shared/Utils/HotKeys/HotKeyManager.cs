using Duckie.Shared.Utils;

namespace Duckie.Utils.HotKeys;

public static partial class HotKeyManager
{
    private static readonly List<HotKeyRegistration> registrations = new();
    private static readonly object sync = new();
    private static readonly HotKeyForm form;
    private static int nextId;

    static HotKeyManager()
    {
        form = new HotKeyForm();
        form.KeyPressed += (_, id) =>
        {
            HotKeyAction action;
            lock (sync) action = registrations.FirstOrDefault(x => x.Id == id)?.Action;
            action?.Action();
        };
    }

    public static IHotKeyService[] GetHotKeyServices() => ReflectUtils.Get<IHotKeyService>();

    public static void RegisterServices()
    {
        foreach (var action in GetHotKeyServices().SelectMany(x => x.Register()))
        {
            try { Register(action, HotKeyBindings.Get(action)); }
            catch (Exception ex) { Duckie.Shared.Utils.Ui.UiUtils.Warning(ex.Message, "快捷键注册失败"); }
        }
        HotKeyBindings.NotifyChanged();
    }

    public static int Register(HotKeyAction action, HotKeyBinding binding)
    {
        lock (sync)
        {
            var current = registrations.FirstOrDefault(x => x.Action.Id == action.Id);
            if (current != null && current.Binding.Equals(binding)) return current.Id;
            if (binding.IsEmpty)
            {
                Unregister(action);
                return 0;
            }
            var conflict = registrations.FirstOrDefault(x => x.Action.Id != action.Id && x.Binding.Equals(binding));
            if (conflict != null) throw new InvalidOperationException($"{binding} 已用于“{conflict.Action.Name}”。");

            // Reserve the new combination before releasing the current registration.
            // Application hotkey IDs must stay in 0x0000–0xBFFF.
            int id;
            do { nextId = nextId % 0xBFFF + 1; id = nextId; }
            while (registrations.Any(x => x.Id == id));
            var modifiers = action.NoRepeat ? binding.Modifiers | Interop.ModNoRepeat : binding.Modifiers;
            if (!Interop.RegisterHotKey(form.Handle, id, modifiers, binding.Key))
                throw new System.ComponentModel.Win32Exception(
                    System.Runtime.InteropServices.Marshal.GetLastWin32Error(),
                    $"无法注册 {binding}，可能已被其他程序占用。");

            Unregister(action);
            registrations.Add(new HotKeyRegistration
            {
                Id = id, Action = action, Binding = new HotKeyBinding(binding.Modifiers, binding.Key)
            });
            return id;
        }
    }

    public static void Unregister(HotKeyAction action)
    {
        lock (sync)
        {
            foreach (var item in registrations.Where(x => x.Action.Id == action.Id).ToArray())
            {
                Interop.UnregisterHotKey(form.Handle, item.Id);
                registrations.Remove(item);
            }
        }
    }

    public static HotKeyBinding GetRegisteredBinding(string actionId)
    {
        lock (sync)
        {
            var binding = registrations.FirstOrDefault(x => x.Action.Id == actionId)?.Binding;
            return binding == null ? null : new HotKeyBinding(binding.Modifiers, binding.Key);
        }
    }

    public static bool IsRegistered(string actionId) => GetRegisteredBinding(actionId) != null;
}
