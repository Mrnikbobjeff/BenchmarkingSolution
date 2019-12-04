using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace BenchmarkingSolution
{
    [MemoryDiagnoser]
    [SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.NetCoreApp30)]
    public class NonValueTypeEnumerator
    {
        public static Lazy<IList<int>> InterfacedList { get; } = new Lazy<IList<int>>(() => GetList());
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static IList<int> GetList()
        {
            return (IList<int>) new List<int>(Enumerable.Range(0, 20));
        }

        static NonValueTypeEnumerator() => InterfacedList.Value.GetHashCode();

        [Benchmark]
        public int EnumerateThroughIEnumerator()
        {
            int x = 0;
            var l = InterfacedList.Value;
            foreach (var item in l)
                x += item;
            return x;
        }

        [Benchmark]
        public int EnumerateThroughStructEnumerator()
        {
            int x = 0;
            var l = (List<int>)InterfacedList.Value;
            foreach (var item in l)
                x += item;
            return x;
        }

        [Benchmark]
        public int EnumerateViaIndexer()
        {
            int x = 0;
            var l = InterfacedList.Value;
            for (int i = 0; i < l.Count; i++)
            {
                x += l[i];
            }
            return x;
        }
    }
}
