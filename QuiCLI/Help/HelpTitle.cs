namespace QuiCLI.Help;

internal class HelpTitle(string text)
{
    public string Text { get; } = text;

    public static implicit operator HelpTitle(string text)
    {
        return new HelpTitle(text);
    }

    public static implicit operator string(HelpTitle title) => title.ToString();
    public override string ToString()
    {
        if (Console.IsOutputRedirected)
        {
            return Text;
        }
        else
        {
            return "\x1b[1m" + Text + "\x1b[0m";
        }
    }
}
