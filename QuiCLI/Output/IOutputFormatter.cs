using System.Diagnostics.CodeAnalysis;

namespace QuiCLI.Output;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
public interface IOutputFormatter
{
    string Format(object? value);
}
