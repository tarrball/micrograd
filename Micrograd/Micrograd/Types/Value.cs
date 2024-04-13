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

    public double Grad { get; set; }

    public string? Label { get; set; } = label;

    public OperationType Operation { get; } = operation;

    private readonly HashSet<Value> _children = children is not null
        ? children.Value.B is not null
            ? [children.Value.A, children.Value.B]
            : [children.Value.A]
        : [];

    private Action _backward = () => { };

    public static implicit operator Value(double data) => new(data);
    public static implicit operator double(Value value) => value.Data;

    public static Value operator +(Value a, Value b) => Add(a, b);
    public static Value operator +(Value a, double b) => Add(a, new Value(b));
    public static Value operator +(double a, Value b) => Add(new Value(a), b);

    public static Value operator *(Value a, Value b) => Multiply(a, b);
    public static Value operator *(Value a, double b) => Multiply(a, new Value(b));
    public static Value operator *(double a, Value b) => Multiply(new Value(a), b);

    private static Value Add(Value a, Value b)
    {
        var newValue = new Value(a.Data + b.Data, operation: OperationType.Add, children: (a, b));

        newValue._backward = () =>
        {
            a.Grad += newValue.Grad;
            b.Grad += newValue.Grad;
        };

        return newValue;
    }

    private static Value Multiply(Value a, Value b)
    {
        var newValue = new Value(a.Data * b.Data, operation: OperationType.Multiply, children: (a, b));

        newValue._backward = () =>
        {
            a.Grad += b.Data * newValue.Grad;
            b.Grad += a.Data * newValue.Grad;
        };

        return newValue;
    }

    public void Backward()
    {
        var topo = new List<Value>();
        var visited = new HashSet<Value>();

        BuildTopo(this);

        Grad = 1;
        topo.Reverse();

        foreach (var vertex in topo)
        {
            vertex._backward();
        }

        return;

        void BuildTopo(Value value)
        {
            if (visited.Add(value))
            {
                foreach (var child in value._children)
                {
                    BuildTopo(child);
                }

                topo.Add(value);
            }
        }
    }

    public Value Tanh(string label)
    {
        var newValue = new Value(Math.Tanh(Data), operation: OperationType.Tanh, children: (this, null));

        newValue._backward = () =>
        {
            //
            Grad += (1 - Math.Pow(newValue.Data, 2)) * newValue.Grad;
        };

        newValue.Label = label;

        return newValue;
    }

    public override string ToString()
    {
        return $"{Label} | data: {Data} | grad: {Grad}";
    }
}