namespace Micrograd.Types;

/// <summary>
/// Store a single scalar value and its gradient.
/// </summary>
public class Value(
    double data,
    string? label = null,
    OperationType operation = OperationType.None,
    (Value A, Value? B)? children = null
)
{
    public double Data { get; } = data;

    public double Grad { get; set; } = 0;

    public string? Label { get; set; } = label;

    public OperationType Operation { get; } = operation;

    public Action Backward { get; private set; } = () => { };

    private (Value A, Value? B)? _children = children;

    public static implicit operator Value(double data) => new(data);
    public static implicit operator double(Value value) => value.Data;

    public static Value operator +(Value a, Value b) => Add(a, b);
    public static Value operator +(Value a, double b) => Add(a, new Value(b));
    public static Value operator +(double a, Value b) => Add(new Value(a), b);

    public static Value operator *(Value a, Value b) => Multiply(a, b);
    public static Value operator *(Value a, double b) => Multiply(a, new Value(b));
    public static Value operator *(double a, Value b) => Multiply(new Value(a), b);

    private static Value Add(Value left, Value right)
    {
        var newValue = new Value(left.Data + right.Data, operation: OperationType.Add, children: (left, right));

        newValue.Backward = () =>
        {
            left.Grad += newValue.Grad;
            right.Grad += newValue.Grad;
        };

        return newValue;
    }

    private static Value Multiply(Value left, Value right)
    {
        var newValue = new Value(left.Data * right.Data, operation: OperationType.Multiply, children: (left, right));

        newValue.Backward = () =>
        {
            left.Grad += right.Data * newValue.Grad;
            right.Grad += left.Data * newValue.Grad;
        };

        return newValue;
    }

    public Value Tanh()
    {
        var newValue = new Value(Math.Tanh(Data), operation: OperationType.Tanh, children: (this, null));

        newValue.Backward = () => { Grad += (1 - Math.Pow(newValue.Data, 2)) * newValue.Grad; };

        return newValue;
    }
}