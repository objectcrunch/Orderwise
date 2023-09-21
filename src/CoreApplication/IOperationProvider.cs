namespace CoreApplication
{
    public interface IOperationProvider : IEnumerable<KeyValuePair<string, OperationContainer>>
    {
        OperationResultContainer? Calculate(string operationType, string left, string? right = null);
        int GetRequiredOperandsCount(string operationType);
    }
}