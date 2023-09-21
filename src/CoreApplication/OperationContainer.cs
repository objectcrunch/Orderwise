namespace CoreApplication;

public class OperationContainer
{
    public int ExpectedNumberOfOperands {  get; }  
    public Func<string, string?, OperationResultContainer?> Operation {  get; }

    public OperationContainer(Func<string, string?, OperationResultContainer?> operation, int expectedNumberOfOperands) 
    {
        Operation = operation;
        ExpectedNumberOfOperands = expectedNumberOfOperands;
    }
}
