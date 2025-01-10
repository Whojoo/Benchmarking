using BenchmarkDotNet.Attributes;

namespace BitwiseAdd;

public class BitwiseBencher
{
    [Params(1, 23, 50)]
    public int Num { get; set; }
    
    [Params(5, 36)]
    public int T  { get; set; }
    
    
    [Benchmark]
    public int Normal() => Num + 2 * T;

    [Benchmark]
    public int Bitwise()
    {
        var toAdd = T << 1;

        const int maxIterations = 10000;
        var iteration = 0;

        var carry = Num & toAdd;
        var result = Num ^ toAdd;

        while (carry != 0 && iteration < maxIterations)
        {
            iteration++;

            var shiftedCarry = carry << 1;
            carry = result & shiftedCarry;
            result ^= shiftedCarry;
        }

        return result;
    }
}