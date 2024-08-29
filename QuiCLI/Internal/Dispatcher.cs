using QuiCLI.Command;

namespace QuiCLI.Internal
{
    internal static class Dispatcher
    {
        public static async Task<object?> InvokeAsync(object instance, ParsedCommand command)
        {
            object?[]? parameters = GetParameters(command.Arguments, command.Definition.Parameters.Where(a => !a.IsGlobal).ToList());
            object? result;
            if (command.Definition.Method!.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                
                result = await GetSupportedAsyncResult(instance, command, parameters);
                return result;
            }

            result = command.Definition.Method!.Invoke(instance, parameters);

            if (result is Task task)
            {
                await task.ConfigureAwait(false);
            }
            return result;
        }

        private static async Task<object?> GetSupportedAsyncResult(object instance, ParsedCommand command, object?[]? parameters)
        {
            var returnType = command.Definition.Method!.ReturnType;
            if (returnType == typeof(Task))
            {
                await (Task)command.Definition.Method!.Invoke(instance, parameters)!;
                return null;
            }
            return command.Definition.Method!.ReturnType switch
            {
                Type t when t == typeof(Task<string>) => await (Task<string>)command.Definition.Method!.Invoke(instance, parameters)!,
                Type t when t == typeof(Task<object>) => await (Task<object>)command.Definition.Method!.Invoke(instance, parameters)!,
                _ => throw new ArgumentException("Method must return either Task, Task<string> or Task<object>")
            };
        }

        private static object?[]? GetParameters(List<ParameterValue> values, List<ParameterDefinition> parameters)
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
