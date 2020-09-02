using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace BenchmarkingSolution
{
    public interface IPolicy
    {
        int Calculate();
    }
    public struct ConcretePolicy : IPolicy
    {
        static readonly Random r = new Random();
        [MethodImpl(MethodImplOptions.NoInlining)]
        public int Calculate()
        {
            return 34;
        }
    }

    [SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.NetCoreApp30)]
    [MemoryDiagnoser]
    public class PolicyStructCost
    {
        public int ApplyPolicy<T>() where T : struct, IPolicy
        {
            var t = new T();
            return t.Calculate();
        }

        [Benchmark]
        public int AllocatePolicy()
        {
            return ApplyPolicy<ConcretePolicy>();
        }
    }
}
