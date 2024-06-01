namespace QuiCLI.Common;

public record Error(ErrorCode ErrorCode, string ErrorMessage, Exception? InnerException = null)
{
    public override string ToString()
    {
        return $"Error: {ErrorCode} - {ErrorMessage}";
    }
}