using Micrograd.Types;

namespace MicrogradTests;

[TestClass]
public class SanityTests
{
    [TestMethod]
    public void SanityCheck()
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

        var o = n.Tanh("o");

        o.Grad = 1;
        o.Backward();

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
}