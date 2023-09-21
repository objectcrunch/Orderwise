namespace CoreApplication;

public class OperationProviderTest
{
    [Theory]
    [InlineData("2", "3", 5, "2 + 3 =")]
    [InlineData("2", "-3", -1, "2 + -3 =")]
    [InlineData("-2", "3", 1, "-2 + 3 =")]
    public void CalculateCanPerformAdd(string left, string right, double sum, string detail)
    {
        // Arrange
        var operationType = "+";
        var provider = new OperationProvider();

        // Act
        var result = provider.Calculate(operationType, left, right);

        // Assert
        Assert.Equal(sum, result!.Result);
        Assert.Equal(detail, result.OperationDetail);
    }

    [Theory]
    [InlineData("2", "3", -1, "2 - 3 =")]
    [InlineData("2", "-3", 5, "2 - -3 =")]
    [InlineData("-2", "3", -5, "-2 - 3 =")]
    public void CalculateCanPerformSubtract(string left, string right, double sum, string detail)
    {
        // Arrange
        var operationType = "-";
        var provider = new OperationProvider();

        // Act
        var result = provider.Calculate(operationType, left, right);

        // Assert
        Assert.Equal(sum, result!.Result);
        Assert.Equal(detail, result.OperationDetail);
    }

    [Theory]
    [InlineData("2", "3", 6, "2 X 3 =")]
    [InlineData("2", "-3", -6, "2 X -3 =")]
    [InlineData("-2", "3", -6, "-2 X 3 =")]
    public void CalculateCanPerformMultiply(string left, string right, double sum, string detail)
    {
        // Arrange
        var operationType = "X";
        var provider = new OperationProvider();

        // Act
        var result = provider.Calculate(operationType, left, right);

        // Assert
        Assert.Equal(sum, result!.Result);
        Assert.Equal(detail, result.OperationDetail);
    }

    [Theory]
    [InlineData("2", "3", 0.66666666666666663, "2 ÷ 3 =")]
    [InlineData("2", "-3", -0.66666666666666663, "2 ÷ -3 =")]
    [InlineData("-2", "3", -0.66666666666666663, "-2 ÷ 3 =")]
    public void CalculateCanPerformDivide(string left, string right, double sum, string detail)
    {
        // Arrange
        var operationType = "÷";
        var provider = new OperationProvider();

        // Act
        var result = provider.Calculate(operationType, left, right);

        // Assert
        Assert.Equal(sum, result!.Result);
        Assert.Equal(detail, result.OperationDetail);
    }

    [Theory]
    [InlineData("2", "3", 4, "(2)² =")]
    [InlineData("-3", "-3", 9, "(-3)² =")]
    [InlineData("4", "3", 16, "(4)² =")]
    public void CalculateCanPerformSquare(string left, string right, double sum, string detail)
    {
        // Arrange
        var operationType = "x²";
        var provider = new OperationProvider();

        // Act
        var result = provider.Calculate(operationType, left, right);

        // Assert
        Assert.Equal(sum, result!.Result);
        Assert.Equal(detail, result.OperationDetail);
    }

    [Theory]
    [InlineData("2", "3", 1.4142135623730951, "√(2) =")]
    [InlineData("3", "-3", 1.7320508075688772, "√(3) =")]
    [InlineData("4", "3", 2, "√(4) =")]
    public void CalculateCanPerformSquareRoot(string left, string right, double sum, string detail)
    {
        // Arrange
        var operationType = "√";
        var provider = new OperationProvider();

        // Act
        var result = provider.Calculate(operationType, left, right);

        // Assert
        Assert.Equal(sum, result!.Result);
        Assert.Equal(detail, result.OperationDetail);
    }

    [Theory]
    [InlineData("2", "3", 0.5, "1/(2) =")]
    [InlineData("-3", "-3", -0.33333333333333331, "1/(-3) =")]
    [InlineData("4", "3", 0.25, "1/(4) =")]
    public void CalculateCanPerformInverse(string left, string right, double sum, string detail)
    {
        // Arrange
        var operationType = "1/x";
        var provider = new OperationProvider();

        // Act
        var result = provider.Calculate(operationType, left, right);

        // Assert
        Assert.Equal(sum, result!.Result);
        Assert.Equal(detail, result.OperationDetail);
    }

    [Theory]
    [InlineData("2", "3", -2, "-(2) =")]
    [InlineData("-3", "-3", 3, "-(-3) =")]
    [InlineData("4", "3", -4, "-(4) =")]
    public void CalculateCanPerformNegate(string left, string right, double sum, string detail)
    {
        // Arrange
        var operationType = "±";
        var provider = new OperationProvider();

        // Act
        var result = provider.Calculate(operationType, left, right);

        // Assert
        Assert.Equal(sum, result!.Result);
        Assert.Equal(detail, result.OperationDetail);
    }

    [Theory]
    [InlineData("2", "3", 0.02, "2% =")]
    [InlineData("30", "-3", 0.3, "30% =")]
    [InlineData("89", "3", 0.89, "89% =")]
    public void CalculateCanPerformPercentage(string left, string right, double sum, string detail)
    {
        // Arrange
        var operationType = "%";
        var provider = new OperationProvider();

        // Act
        var result = provider.Calculate(operationType, left, right);

        // Assert
        Assert.Equal(sum, result!.Result);
        Assert.Equal(detail, result.OperationDetail);
    }
}
