using CoreApplication;
using System.Windows;
using System.Windows.Controls;

namespace Calculator;

public partial class MainWindow : Window
{
    public readonly ViewModel Model;

    private string? _leftOperand;
    private string? _rightOperand;
    private string? _operationType;
    private bool _resultCaptured = false;

    private readonly IMemoryProvider<string> _memory;
    private readonly IOperandProvider _operandProvider;
    private readonly IOperationProvider _operationProvider;

    private static readonly string _dot = ".";
    private static readonly string _zero = "0";
        
    public MainWindow(
        IMemoryProvider<string> memory,
        IOperandProvider operandProvider,
        IOperationProvider operationProvider)
    {
        InitializeComponent();

        Model = new ViewModel();
        DataContext = Model;

        _memory = memory;
        _operandProvider = operandProvider;
        _operationProvider = operationProvider;
    }

    private void Store(object sender, RoutedEventArgs e)
    {
        if (Model.Result != null)
        {
            _memory.Store(Model.Result);
            Model.MemoryClearEnabled = true;
            Model.MemoryRecallEnabled = true;
        }
    }

    private void ClearMemory(object sender, RoutedEventArgs e)
    {
        _memory.Clear();
        Model.MemoryClearEnabled = false;
        Model.MemoryRecallEnabled = false;
    }

    private void RecallMemory(object sender, RoutedEventArgs e)
    {
        var result = _memory.Get();

        if (result != null)
        {
            ResetCalculator();

            _operandProvider.PushEntry(result!);
            Model.Result = result;
        }
    }

    private void AddToMemory(object sender, RoutedEventArgs e)
    {
        if (Model.Result != null)
        {
            var stored = _memory.Get() ?? _zero;

            if (stored != null)
            {
                var content = GetButtonContent(e);
                _operationType = content!.Substring(content.Length - 1);

                var result = _operationProvider.Calculate(_operationType, stored, Model.Result);

                if (result != null)
                {
                    Model.Result = $"{result.Result}";
                    _memory.Store(Model.Result);
                }
            }
        }
    }

    private void SubtractFromMemory(object sender, RoutedEventArgs e)
    {
        if (Model.Result != null)
        {
            var stored = _memory.Get() ?? _zero;

            if (stored != null)
            {
                var content = GetButtonContent(e);
                _operationType = content!.Substring(content.Length - 1);

                var result = _operationProvider.Calculate(_operationType, stored, Model.Result);

                if (result != null)
                {
                    Model.Result = $"{result.Result}";
                    _memory.Store(Model.Result);
                }
            }
        }
    }

    private void GetOperand(object sender, RoutedEventArgs e)
    {
        if (_resultCaptured)
        {
            ResetCalculator();
            _resultCaptured = false;
        }

        var content = GetButtonContent(e);

        _operandProvider.PushEntry(content!);
        var result = _operandProvider.GetOperand();
        Model.Result = result;
    }

    private void GetDecimal(object sender, RoutedEventArgs e)
    {
        if (_resultCaptured)
        {
            ResetCalculator();
            _operandProvider.PushEntry(_zero);
            _resultCaptured = false;
            Model.OperationDetail = null;
        }

        if (Model.Result?.IndexOf(_dot) != -1)
        {
            return;
        }

        var content = GetButtonContent(e);

        _operandProvider.PushEntry(content);
        var result = _operandProvider.GetOperand();
        Model.Result = result;
    }
    
    private void ClearLastInputItem(object sender, RoutedEventArgs e)
    {
        if (_resultCaptured)
        {
            Model.OperationDetail = null;
        }
        else
        {
            _operandProvider.TryClearLastEntry(out var _);

            var result = _operandProvider.GetOperand();
            Model.Result = result;
        }
    }

    private void ClearInput(object sender, RoutedEventArgs e)
    {
        _operandProvider.ClearOperand();

        if (Model.Result != null)
        {
            Model.Result = null;
        }
        else
        {
            Model.OperationDetail = null;
        }
    }

    private void GetTwoOperandsOperationType(object sender, RoutedEventArgs e)
    {
        _leftOperand ??= _operandProvider.GetOperand();

        if (_leftOperand == null)
        {
            return;
        }

        _operandProvider.ClearOperand();
        Model.Result = _leftOperand;
        _operationType = GetButtonContent(e);

        Model.OperationDetail = $"{_leftOperand} {_operationType}";
    }
    
    private void PerformSingleOperandOperation(object sender, RoutedEventArgs e)
    {
        _leftOperand ??= _operandProvider.GetOperand();

        if (_leftOperand == null)
        {
            return;
        }

        _leftOperand = CloseADot(_leftOperand);

        _operandProvider.ClearOperand();

        _operationType = GetButtonContent(e);

        var result = _operationProvider.Calculate(_operationType, _leftOperand!);

        if (result != null)
        {
            Model.Result = $"{result.Result}";
            Model.OperationDetail = $"{result.OperationDetail}";
            _resultCaptured = true;
        }
    }
       
    private void NegateEntry(object sender, RoutedEventArgs e)
    {
        if (Model.Result != null)
        {
            _operationType = (e.Source as Button)!.Content.ToString();

            var operand = CloseADot(Model.Result);

            var result = _operationProvider.Calculate(_operationType!, operand!);

            if (result != null)
            {
                Model.Result = $"{result.Result}";
                Model.OperationDetail = $"{result.OperationDetail}";
            }
        }
    }
       
    private void PerformTwoOperandsOperation(object sender, RoutedEventArgs e)
    {
        if (_operationType == null)
        {
            return;
        }

        if (_leftOperand == null)
        {
            return;
        }

        _rightOperand = _operandProvider.GetOperand();

        _operandProvider.ClearOperand();

        var getRequiredOperandsCount = _operationProvider.GetRequiredOperandsCount(_operationType);

        if (getRequiredOperandsCount == 1)
        {
            _leftOperand = CloseADot(_leftOperand);
            var result = _operationProvider.Calculate(_operationType, _leftOperand!);

            if (result != null)
            {
                Model.Result = $"{result.Result}";
                Model.OperationDetail = $"{result.OperationDetail}";
                _resultCaptured = true;
            }
        }
        else if (getRequiredOperandsCount == 2 && _rightOperand != null)
        {
            _leftOperand = CloseADot(_leftOperand);
            _rightOperand = CloseADot(_rightOperand);

            var result = _operationProvider.Calculate(_operationType, _leftOperand!, _rightOperand);

            if (result != null)
            {
                Model.Result = $"{result.Result}";
                Model.OperationDetail = $"{result.OperationDetail}";
                _resultCaptured = true;
            }
        }
    }
    
    private static string GetButtonContent(RoutedEventArgs e) => (e.Source as Button)!.Content!.ToString()!;
    
    private static string? CloseADot(string value)
    {
        if (value?.EndsWith(".") ?? false)
        {
            value += _zero;
        }

        return value;
    }
    
    private void ClearOperation(object sender, RoutedEventArgs e)
    {
        Model.Result = null;
        _leftOperand = null;
        _rightOperand = null;
        _operationType = null;
        _operandProvider.ClearOperand();
    }
    
    private void ResetCalculator()
    {
        Model.Result = null;
        _leftOperand = null;
        _rightOperand = null;
        _operationType = null;
        _operandProvider.ClearOperand();
    }
}
