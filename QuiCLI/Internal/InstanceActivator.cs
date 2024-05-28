using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace QuiCLI.Internal
{
    internal static class InstanceActivator
    {
        public static object CreateInstance(IServiceProvider provider, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type, object[] args)
        {
            return ActivatorUtilities.CreateInstance(provider, type, args);
        }

        public static object GetServiceOrCreateInstance(IServiceProvider provider, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type)
        {
            return ActivatorUtilities.GetServiceOrCreateInstance(provider, type);
        }
    }
}
