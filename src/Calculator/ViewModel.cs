using System.ComponentModel;

namespace Calculator;

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

    private bool _memoryRecallEnabled;
    public bool MemoryRecallEnabled
    {
        get
        {
            return _memoryRecallEnabled;
        }
        set
        {
            _memoryRecallEnabled = value;
            NotifyPropertyChanged(nameof(MemoryRecallEnabled));
        }
    }

    private bool _memoryClearEnabled;
    public bool MemoryClearEnabled
    {
        get
        {
            return _memoryClearEnabled;
        }
        set
        {
            _memoryClearEnabled = value;
            NotifyPropertyChanged(nameof(MemoryClearEnabled));
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
