using Microsoft.Extensions.Options;
using Moq;

namespace CoreApplication;

public class OperandProviderTest
{
    [Fact]
    public void CanPushEntry()
    {
        // Arrange
        var optionsMock = new Mock<IOptions<Options>>();

        optionsMock.SetupGet(m => m.Value).Returns(new Options
        {
            OperandSizeLimit = 2,
        });

        var provider = new OperandProvider(optionsMock.Object);

        var input = "7";

        // Act
        provider.PushEntry(input);
        var result = provider.GetOperand();

        // Assert
        Assert.Equal(input, result);
    }

    [Fact]
    public void CanClearLastEntry()
    {
        // Arrange
        var optionsMock = new Mock<IOptions<Options>>();

        optionsMock.SetupGet(m => m.Value).Returns(new Options
        {
            OperandSizeLimit = 2,
        });

        var provider = new OperandProvider(optionsMock.Object);

        var input = "7";
        var lastEntry = "9";

        // Act
        provider.PushEntry(input);
        provider.PushEntry(lastEntry);

        if (provider.TryClearLastEntry(out var result)) { };

        var operand = provider.GetOperand();

        // Assert
        Assert.Equal(lastEntry, result);
        Assert.Equal(input, operand);
    }

    [Fact]
    public void CanEnforceLimit()
    {
        // Arrange
        var optionsMock = new Mock<IOptions<Options>>();

        optionsMock.SetupGet(m => m.Value).Returns(new Options
        {
            OperandSizeLimit = 2,
        });

        var provider = new OperandProvider(optionsMock.Object);

        var input = "7";
        var lastEntry = "9";
        var discard = "2";

        // Act
        provider.PushEntry(input);
        provider.PushEntry(lastEntry);
        provider.PushEntry(discard);

        var operand = provider.GetOperand();

        // Assert
        Assert.Equal($"{input}{lastEntry}", operand);
    }
}
