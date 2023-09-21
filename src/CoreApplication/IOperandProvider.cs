namespace CoreApplication
{
    public interface IOperandProvider
    {
        int Count { get; }
        bool PushEntry(string value);
        string? GetOperand();
        bool TryClearLastEntry(out string? lastEntry);
        void ClearOperand();
    }
}