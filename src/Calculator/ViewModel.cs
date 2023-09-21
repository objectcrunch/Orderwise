using System.ComponentModel;

namespace Calculator
{
    public class ViewModel : INotifyPropertyChanged
    {
        private string? _result;
        public string? Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
                NotifyPropertyChanged(nameof(Result));
            }
        }

        private string? _operation;
        public string? Operation
        {
            get
            {
                return _operation;
            }
            set
            {
                _operation = value;
                NotifyPropertyChanged(nameof(Operation));
            }
        }

        private bool _memoryRecallDisabled;
        public bool MemoryRecallDisabled
        {
            get
            {
                return _memoryRecallDisabled;
            }
            set
            {
                _memoryRecallDisabled = value;
                NotifyPropertyChanged(nameof(MemoryRecallDisabled));
            }
        }

        private bool _memoryClearDisabled;
        public bool MemoryClearDisabled
        {
            get
            {
                return _memoryClearDisabled;
            }
            set
            {
                _memoryClearDisabled = value;
                NotifyPropertyChanged(nameof(MemoryClearDisabled));
            }
        }

        private string? _operationDetail;
        public string? OperationDetail
        {
            get
            {
                return _operationDetail;
            }
            set
            {
                _operationDetail = value;
                NotifyPropertyChanged(nameof(OperationDetail));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
