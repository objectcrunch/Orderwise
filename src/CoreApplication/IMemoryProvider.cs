namespace CoreApplication
{
    public interface IMemoryProvider<T>
    {
        int Count { get; }

        void Clear();
        T? Get();
        void Store(T value);
    }
}