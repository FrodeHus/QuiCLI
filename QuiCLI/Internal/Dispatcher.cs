using System.Diagnostics.CodeAnalysis;

namespace QuiCLI.Internal
{
    internal static class Dispatcher
    {
        public static async Task<object?> InvokeAsync(object instance, string methodName)
        {
            var method = instance.GetType().GetMethod(methodName);
            if (method == null)
            {
                throw new InvalidOperationException($"Method '{methodName}' not found on type '{instance.GetType().FullName}'");
            }

            var result = method.Invoke(instance, null);
            if (result is Task task)
            {
                await task.ConfigureAwait(false);
                var property = GetProperty("Result", task);
                return property?.GetValue(task);
            }
            return result;
        }

        private static System.Reflection.PropertyInfo? GetProperty<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(string propertyName, T type)
        {
            return typeof(T).GetProperty(propertyName);
        }
    }
}
