using Duckie.Shared.Services.UserConfigs;

namespace Duckie.Utils.HotKeys;

public static class HotKeyBindings
{
    public static event EventHandler Changed;

    public static HotKeyBinding Get(HotKeyAction action)
    {
        var binding = (UserConfigService.Get().HotKeys ?? [])
            .FirstOrDefault(x => x.ActionId == action.Id)?.Binding ?? action.DefaultBinding;
        return new HotKeyBinding(binding.Modifiers, binding.Key);
    }

    public static void Save(HotKeyAction action, HotKeyBinding binding)
    {
        var previous = HotKeyManager.GetRegisteredBinding(action.Id);
        try
        {
            HotKeyManager.Register(action, binding);
            var config = UserConfigService.Get();
            config.HotKeys = (config.HotKeys ?? []).Where(x => x.ActionId != action.Id)
                .Concat(new[] { new HotKeyConfig { ActionId = action.Id, Binding = binding } }).ToArray();
            UserConfigService.Set(config);
        }
        catch (Exception error)
        {
            try
            {
                if (previous == null) HotKeyManager.Unregister(action);
                else HotKeyManager.Register(action, previous);
            }
            catch (Exception rollbackError)
            {
                throw new AggregateException("保存失败，原快捷键也未能恢复。", error, rollbackError);
            }
            throw;
        }
        NotifyChanged();
    }

    public static void NotifyChanged() => Changed?.Invoke(null, EventArgs.Empty);
}
