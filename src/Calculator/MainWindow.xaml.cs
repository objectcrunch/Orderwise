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

    private void ClearMemory(object sender, RoutedEventArgs e)
    {
        _memory.Clear();
    }

    private void RecallMemory(object sender, RoutedEventArgs e)
    {
        Model.Result = _memory.Get();
    }

    private void AddToMemory(object sender, RoutedEventArgs e)
    {
        if (Model.Result != null)
        {
            var stored = _memory.Get();

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
            var stored = _memory.Get();

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

    private void Store(object sender, RoutedEventArgs e)
    {
        if (Model.Result != null)
        {
            _memory.Store(Model.Result);
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

    private void GetBiOperation(object sender, RoutedEventArgs e)
    {
        if (_leftOperand == null)
        {
            _leftOperand = _operandProvider.GetOperand();
        }

        if (_leftOperand == null)
        {
            return;
        }

        _operandProvider.ClearOperand();
        Model.Result = _leftOperand;
        _operationType = GetButtonContent(e);
    }

    private void PerformSingleOperandOperation(object sender, RoutedEventArgs e)
    {
        _leftOperand ??= _operandProvider.GetOperand();

        if (_leftOperand == null)
        {
            return;
        }

        _operandProvider.ClearOperand();

        _operationType = GetButtonContent(e);

        var result = _operationProvider.Calculate(_operationType, _leftOperand);

        if (result != null)
        {
            Model.Result = $"{result.Result}";
            Model.OperationDetail = $"{result.OperationDetail}";
            _resultCaptured = true;
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

    private void NegateEntry(object sender, RoutedEventArgs e)
    {
        if (Model.Result != null)
        {
            _operationType = (e.Source as Button)!.Content.ToString();

            var result = _operationProvider.Calculate(_operationType!, Model.Result);

            if (result != null)
            {
                Model.Result = $"{result.Result}";
                Model.OperationDetail = $"{result.OperationDetail}";
            }
        }
    }

    private void GetDecimal(object sender, RoutedEventArgs e)
    {
        if (_resultCaptured)
        {
            ResetCalculator();
        }

        var content = GetButtonContent(e);
        _resultCaptured = true;
        _operandProvider.PushEntry(content);
    }

    private void PerformOperation(object sender, RoutedEventArgs e)
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
            var result = _operationProvider.Calculate(_operationType, _leftOperand);

            if (result != null)
            {
                Model.Result = $"{result.Result}";
                Model.OperationDetail = $"{result.OperationDetail}";
                _resultCaptured = true;
            }
        }
        else if (getRequiredOperandsCount == 2 && _rightOperand != null)
        {
            var result = _operationProvider.Calculate(_operationType, _leftOperand, _rightOperand);

            if (result != null)
            {
                Model.Result = $"{result.Result}";
                Model.OperationDetail = $"{result.OperationDetail}";
                _resultCaptured = true;
            }
        }
    }

    private static string GetButtonContent(RoutedEventArgs e) => (e.Source as Button)!.Content!.ToString()!;

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
