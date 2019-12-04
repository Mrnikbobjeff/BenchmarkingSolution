using BenchmarkDotNet.Attributes;
using System;
using System.Runtime.CompilerServices;

namespace BenchmarkingSolution
{
    [MemoryDiagnoser]
    [SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.NetCoreApp30)]
    public class MethodGroupConversions
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        long TestMethod(long i) => i + 2;

        long Applicator(Func<long, long> applicator, long value) => applicator(value);

        [Benchmark]
        public long MultWithMethodGroup()
        {
            long r = 0;
            for (var i = 0; i < 1000; i++)
                r += Applicator(TestMethod, i);
            return r;
        }

        [Benchmark]
        public long MultWithFunc()
        {
            long r = 0;
            for (var i = 0; i < 1000; i++)
                r += Applicator((x) => x + 2, i);
            return r;
        }
    }
}
