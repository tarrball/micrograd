using Micrograd.Types;

namespace Micrograd.Utilities;

internal static class RandomUtility
{
    public static Value NextValue(int min, int max) => new Value(new Random().NextDouble() * (max - min) + min);
}