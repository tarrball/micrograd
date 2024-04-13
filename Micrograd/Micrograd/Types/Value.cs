namespace Micrograd.Types;

/// <summary>
/// Store a single scalar value and its gradient.
/// </summary>
public class Value(
    double data,
    OperationType operationType = OperationType.None,
    (Value Left, Value Right)? children = null
)
{
    public double Data { get; } = data;

    public double Grad { get; set; } = 0;

    public OperationType OperationType { get; } = operationType;

    private Action _backward = () => { };

    private (Value Left, Value Right)? _children = children;

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
        var newValue = new Value(left.Data + right.Data, OperationType.Add, (left, right));

        newValue._backward = () =>
        {
            left.Grad += newValue.Grad;
            right.Grad += newValue.Grad;
        };

        return newValue;
    }

    private static Value Multiply(Value left, Value right)
    {
        var newValue = new Value(left.Data * right.Data, OperationType.Multiply, (left, right));

        newValue._backward = () =>
        {
            left.Grad += right.Data * newValue.Grad;
            right.Grad += left.Data * newValue.Grad;
        };

        return newValue;
    }
}