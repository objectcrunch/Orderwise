namespace CoreApplication
{
    public class MemoryProvider<T> : IMemoryProvider<T>
    {
        private readonly T[] _values;
        public int Count => _values.Length;

        public MemoryProvider()
        {
            _values = new T[1];
        }

        public void Clear()
        {
            if (Count > 0)
            {
                Array.Clear(_values, 0, _values.Length);
            }
        }

        public T? Get()
        {
            if (Count > 0)
            {
                return _values[0];
            }
            return default;
        }

        public void Store(T value)
        {
            _values[0] = value;
        }
    }
}
