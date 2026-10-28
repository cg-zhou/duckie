namespace Duckie.Shared.Utils;

public static class ReflectUtils
{
    public static IInterface[] Get<IInterface>(params System.Reflection.Assembly[] assemblies)
    {
        if (assemblies.Length == 0)
        {
            assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.GetName().Name is "Duckie" or "Duckie.Shared" or "Duckie.Windows")
                .ToArray();
        }

        return assemblies.Distinct()
            .SelectMany(x => x.GetTypes())
            .Where(x => x.IsClass && !x.IsAbstract && !x.ContainsGenericParameters
                && typeof(IInterface).IsAssignableFrom(x)
                && x.GetConstructor(Type.EmptyTypes) != null)
            .Select(Activator.CreateInstance)
            .OfType<IInterface>()
            .ToArray();
    }
}
