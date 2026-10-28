namespace Duckie.Shared.Utils;

public static class ReflectUtils
{
    public static IInterface[] Get<IInterface>()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => x.GetName().Name is "Duckie" or "Duckie.Shared" or "Duckie.Windows")
            .SelectMany(x => x.GetTypes())
            .Where(x => x.IsClass && !x.IsAbstract && !x.ContainsGenericParameters
                && typeof(IInterface).IsAssignableFrom(x)
                && x.GetConstructor(Type.EmptyTypes) != null)
            .Select(Activator.CreateInstance)
            .OfType<IInterface>()
            .ToArray();
    }
}
