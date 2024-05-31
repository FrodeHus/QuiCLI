using QuiCLI.Command;

namespace QuiCLI.Internal
{
    internal static class Dispatcher
    {
        public static async Task<object?> InvokeAsync(object instance, ParsedCommand command)
        {
            object?[]? parameters = GetParameters(command.Arguments, command.Definition.Arguments.Where(a => !a.IsGlobal).ToList());
            var result = command.Definition.Method!.Invoke(instance, parameters);
            if (result is Task<object> taskObject)
            {
                return await taskObject.ConfigureAwait(false);
            }

            if (result is Task task)
            {
                await task.ConfigureAwait(false);
            }
            return result;
        }

        private static object?[]? GetParameters(List<ArgumentValue> values, List<ArgumentDefinition> parameters)
        {
            if (parameters.Count == 0)
            {
                return null;
            }

            var result = new object?[parameters.Count];
            for (int i = 0; i < result.Length; i++)
            {
                var parameter = parameters[i];
                var value = values.Find(v => v.Name == parameter.Name);
                if (value is not null)
                {
                    result[i] = Convert.ChangeType(value.Value, parameter.ValueType);
                }
                else if (parameter.DefaultValue is not null)
                {
                    result[i] = Convert.ChangeType(parameter.DefaultValue, parameter.ValueType);
                }
                else if (!parameter.IsRequired)
                {
                    result[i] = null;
                }
            }
            return result;
        }
    }
}
