using Microsoft.Extensions.Options;

namespace CoreApplication
{
    public class OperandProvider : IOperandProvider
    {
        
        private string? _operand;
        private DataNode<string>? _head;
        private readonly Options _options;

        public int Count { get; private set; }

        public OperandProvider(IOptions<Options> optionsAccessor)
        {
            _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
        }

        public bool PushEntry(string value)
        {
            if (Count >= _options.OperandSizeLimit)
            {
                return false;
            }

            Push(value);
            _operand ??= "";
            _operand += value;
            return true;
        }

        public string? GetOperand()
        {
            return _operand;
        }

        public bool TryClearLastEntry(out string? lastEntry)
        {
            if (Count <= 0)
            {
                lastEntry = null;
                return false;
            }

            lastEntry = Pop();

            _operand = ReplaceLastOccurrence(_operand!, lastEntry, "");

            return true;
        }

        public void ClearOperand()
        {
            _head = null;
            _operand = null;
            Count = 0;
        }

        private void Push(string data)
        {

            if (Count == 0)
            {
                _head = new DataNode<string>(data);
                Count++;
            }
            else
            {
                var newHead = new DataNode<string>(data);
                newHead.Next = _head;
                _head = newHead;
                Count++;
            }
        }

        private string Pop()
        {
            if (Count <= 0)
            {
                throw new System.InvalidOperationException("stack is empty");
            }

            var data = _head!.Data;
            _head = _head.Next;
            Count--;

            return data;
        }

        private static string? ReplaceLastOccurrence(
            string source,
            string find,
            string replace)
        {
            if (source == null)
            {
                return source;
            }

            int place = source.LastIndexOf(find);

            if (place == -1)
            {
                return source;
            }

            return source.Remove(place, find.Length).Insert(place, replace);
        }
    }
}
