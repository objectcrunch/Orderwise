using System.Collections;
using System.Reflection.Emit;

namespace CoreApplication;

public class OperationProvider : IOperationProvider
{
    private readonly IDictionary<string, OperationContainer> _operations;

    private readonly List<string> _keys;

    public IReadOnlyCollection<string> Keys { get; private set; }

    public OperationContainer? this[string key]
    {
        get
        {
            if (_operations.TryGetValue(key, out var builder))
                return builder;
            return null;
        }
        set
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            _operations.Add(key, value);
            _keys.Add(key);
        }
    }

    public OperationProvider()
    {
        _operations = new Dictionary<string, OperationContainer>(
            StringComparer.OrdinalIgnoreCase)
        {
            ["+"] = new OperationContainer(Add, 2),
            ["-"] = new OperationContainer(Subtract, 2),
            ["x"] = new OperationContainer(Multiply, 2),
            ["÷"] = new OperationContainer(Divide, 2),
            ["x²"] = new OperationContainer(Square, 1),
            ["√"] = new OperationContainer(SquareRoot, 1),
            ["1/x"] = new OperationContainer(Inverse, 1),
            ["±"] = new OperationContainer(Negate, 1),
            ["%"] = new OperationContainer(Percentage, 1)
        };
        _keys = new List<string>(_operations.Keys);
        Keys = _keys.AsReadOnly();
    }

    public OperationResultContainer? Calculate(string operationType, string left, string? right = null)
    {
        var operationContainer = this[operationType];

        if (operationContainer == null)
        {
            return null;
            // TODO: provide a better handling
        }

        var result = operationContainer.Operation(left, right);

        return result;
    }

    public int GetRequiredOperandsCount(string operationType)
    {
        var operationContainer = this[operationType];

        if (operationContainer == null)
        {
            return 0;

        }

        return operationContainer.ExpectedNumberOfOperands;
    }

    private OperationResultContainer? Add(string left, string? right)
    {
        double leftOperand = ToOperand(left);
        double righOperand = ToOperand(right!);

        var result = leftOperand + righOperand;

        return new OperationResultContainer
        {
            Result = result,
            OperationDetail = $"{left} + {righOperand} ="
        };
    }

    private OperationResultContainer? Subtract(string left, string? right)
    {
        double leftOperand = ToOperand(left);
        double righOperand = ToOperand(right!);

        var result = leftOperand - righOperand;

        return new OperationResultContainer
        {
            Result = result,
            OperationDetail = $"{left} - {right} ="
        };
    }

    private OperationResultContainer? Multiply(string left, string? right)
    {
        double leftOperand = ToOperand(left);
        double righOperand = ToOperand(right!);

        var result = leftOperand * righOperand;

        return new OperationResultContainer
        {
            Result = result,
            OperationDetail = $"{left} x {right} ="
        };
    }

    private OperationResultContainer? Divide(string left, string? right)
    {
        double leftOperand = ToOperand(left);
        double righOperand = ToOperand(right!);

        if (righOperand == 0)
        {
            //TODO: Provide a better handling
            return null;
        }

        var result = leftOperand / righOperand;

        return new OperationResultContainer
        {
            Result = result,
            OperationDetail = $"{left} ÷ {right} ="
        };
    }

    private OperationResultContainer Square(string left, string? right = null)
    {
        double leftOperand = ToOperand(left);

        var result = leftOperand * leftOperand;

        return new OperationResultContainer
        {
            Result = result,
            OperationDetail = $"({left})² ="
        };
    }

    private OperationResultContainer? SquareRoot(string left, string? right = null)
    {
        double leftOperand = ToOperand(left);

        if (leftOperand < 0)
        {
            // TODO: Provide a better handling
            return null;
        }

        var result = Math.Sqrt(leftOperand);

        return new OperationResultContainer
        {
            Result = result,
            OperationDetail = $"√({left}) ="
        };
    }

    private OperationResultContainer Inverse(string left, string? right = null)
    {
        double leftOperand = ToOperand(left);

        var result = (1 / leftOperand);

        return new OperationResultContainer
        {
            Result = result,
            OperationDetail = $"1/({left}) ="
        };
    }
    private OperationResultContainer Negate(string left, string? right = null)
    {
        double leftOperand = ToOperand(left);
        var result = -leftOperand;

        return new OperationResultContainer
        {
            Result = result,
            OperationDetail = $"-({left}) ="
        };
    }

    private OperationResultContainer Percentage(string left, string? right = null)
    {
        double leftOperand = ToOperand(left);
        var result = leftOperand / 100;

        return new OperationResultContainer
        {
            Result = result,
            OperationDetail = $"{left}% ="
        };
    }

    private static double ToOperand(string value)
    {
        if (!double.TryParse(value, out var result))
        {
            return default;
        }

        return result;
    }

    public IEnumerator<KeyValuePair<string, OperationContainer>> GetEnumerator()
    {
        return _operations.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}