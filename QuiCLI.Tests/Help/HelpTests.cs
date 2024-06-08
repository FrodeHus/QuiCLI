using QuiCLI.Command;
using QuiCLI.Help;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuiCLI.Tests.Help;

public class HelpTests
{
    [Fact]
    public void GenerateHelpItemFromCommand()
    {
        var method = typeof(TestCommand).GetMethod("Test6");
        var definition = CommandDefinition.FromMethod(method!).Value;
        var helpItem = HelpBuilder.GenerateHelp(definition) as HelpCommand;
        Assert.NotNull(helpItem);
        Assert.Equal("test6", helpItem!.Name);
        Assert.Equal(2, helpItem.Arguments!.Count);
        Assert.Equal("parameter", helpItem.Arguments[0].Name);
        Assert.True(helpItem.Arguments[0].IsRequired);
        Assert.False(helpItem.Arguments[0].IsFlag);
    }
}
