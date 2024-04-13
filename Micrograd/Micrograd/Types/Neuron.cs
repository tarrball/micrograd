using System.Diagnostics;
using Micrograd.Utilities;

namespace Micrograd.Types;

public class Neuron
{
    /// <summary>
    /// Bias
    /// </summary>
    public Value B { get; }

    /// <summary>
    /// Weights
    /// </summary>
    public Value[] W { get; }

    /// <param name="nin">Number of inputs</param>
    public Neuron(int nin)
    {
        W = Enumerable
            .Range(0, nin)
            .Select(_ => RandomUtility.NextValue(-1, 1))
            .ToArray();

        B = RandomUtility.NextValue(-1, 1);
    }

    public double Calculate(Value[] values)
    {
        Debug.Assert(values.Length == W.Length, "Number of inputs must match number of weights");

        var sum = W.Zip(values, (w, x) => w * x).Aggregate(B, (a, b) => a + b);

        return sum.Tanh();
    }
}