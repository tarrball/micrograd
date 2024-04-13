using Micrograd.Types;
using Shouldly;

namespace MicrogradTests;

[TestClass]
public class SanityTests
{
    [TestMethod]
    public void TestValue()
    {
        var a = new Value(3.0);
        var b = new Value(2.0);
        var c = a + b;
        c.Data.ShouldBe(5.0);
        c.Grad.ShouldBe(0.0);
    }
}