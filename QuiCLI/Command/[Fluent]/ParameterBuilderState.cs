namespace QuiCLI.Command;

internal sealed class ParameterBuilderState : IBuilderState, IParameterBuilder
{
    private bool _isRequired;
    private bool _isFlag;
    private object? _defaultValue;
    private string? _description;

    internal ParameterBuilderState()
    {
        
    }
    object IBuilderState.Build()
    {
        return new ParameterDefinition
        {
            Name = "",
            InternalName = "",
            IsFlag = _isFlag,
            IsRequired = _isRequired,
            DefaultValue = _defaultValue,
            Description = _description
        };
    }

    IParameterBuilder IParameterBuilder.IsFlag()
    {
        _isFlag = true;
        return this;
    }

    IParameterBuilder IParameterBuilder.IsRequired()
    {
        _isRequired = true;
        return this;
    }

    IParameterBuilder IParameterBuilder.WithDefaultValue(object value)
    {
        _defaultValue = value;
        return this;
    }

    IParameterBuilder IParameterBuilder.WithDescription(string description)
    {
        _description = description;
        return this;
    }
}
