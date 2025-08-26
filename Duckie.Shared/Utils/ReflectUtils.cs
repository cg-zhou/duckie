namespace Duckie.Shared.Utils;

public static class ReflectUtils
{
    public static IInterface[] Get<IInterface>()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => x.GetInterfaces().Contains(typeof(IInterface)))
            .Select(Activator.CreateInstance)
            .OfType<IInterface>()
            .ToArray();
    }
}
