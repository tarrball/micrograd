using Micrograd.Types;
using Shouldly;

namespace MicrogradTests;

[TestClass]
public class SanityTests
{
    [TestMethod]
    public void SanityCheck_DemoVideoExpressionGraph()
    {
        var x1 = new Value(2, "x1");
        var w1 = new Value(-3, "w1");

        var x2 = new Value(0, "x2");
        var w2 = new Value(1, "w2");

        var x1w1 = x1 * w1;
        x1w1.Label = "x1*w1";

        var x2w2 = x2 * w2;
        x2w2.Label = "x2*w2";

        var x1w1px2w2 = x1w1 + x2w2;
        x1w1px2w2.Label = "x1w1+x2w2";

        var b = new Value(6.8814, "b");
        var n = b + x1w1px2w2;
        n.Label = "n";

        var o = n.Tanh();
        o.Label = "o";

        o.Grad = 1;
        o.Backward();

        o.Data.ShouldBe(0.7071, .0001);
        o.Grad.ShouldBe(1);

        n.Data.ShouldBe(0.8814, .0001);
        n.Grad.ShouldBe(0.5, .0001);

        b.Data.ShouldBe(6.8814, .0001);
        b.Grad.ShouldBe(0.5, .0001);

        x1w1px2w2.Data.ShouldBe(-6);
        x1w1px2w2.Grad.ShouldBe(0.5, .0001);

        x1w1.Data.ShouldBe(-6);
        x1w1.Grad.ShouldBe(0.5, .0001);
        x2w2.Data.ShouldBe(0);
        x2w2.Grad.ShouldBe(0.5, .0001);

        w1.Data.ShouldBe(-3);
        w1.Grad.ShouldBe(1, .0001);
        w2.Data.ShouldBe(1);
        w2.Grad.ShouldBe(0);
        x1.Data.ShouldBe(2);
        x1.Grad.ShouldBe(-1.5, .0001);
        x2.Data.ShouldBe(0);
        x2.Grad.ShouldBe(0.5, .0001);

        Console.WriteLine(o.ToString());
        Console.WriteLine(n.ToString());
        Console.WriteLine(b.ToString());
        Console.WriteLine(x1w1px2w2.ToString());
        Console.WriteLine(x1w1.ToString());
        Console.WriteLine(x2w2.ToString());
        Console.WriteLine(w1.ToString());
        Console.WriteLine(w2.ToString());
        Console.WriteLine(x1.ToString());
        Console.WriteLine(x2.ToString());
    }

    [TestMethod]
    public void SanityCheck_Mlp()
    {
        var x = new[] { new Value(2), new Value(3) };
        var n = new Neuron(2);

        Console.WriteLine(n.Calculate(x));
    }
}