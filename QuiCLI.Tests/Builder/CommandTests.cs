using QuiCLI.Builder;

namespace QuiCLI.Tests.Builder
{
    public class CommandTests
    {
        [Fact]
        public void AddCommand_ShouldAddCommand()
        {
            // Arrange
            var builder = new QuicAppBuilder();


            // Act
            builder.AddCommand(sp => new TestCommand());
            var app = builder.Build();


            // Assert
            Assert.Single(app.Commands);
        }
    }
}
