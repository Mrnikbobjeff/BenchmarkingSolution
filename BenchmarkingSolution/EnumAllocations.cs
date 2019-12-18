using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BenchmarkingSolution
{
    [MemoryDiagnoser]
    public class EnumAllocations
    {
        [Benchmark]
        public bool EnumEq_Allocs()
        {
            return StringSplitOptions.RemoveEmptyEntries.Equals(StringSplitOptions.None);
        }
        [Benchmark]
        public bool EnumEq_DOesNotAllocate()
        {
            return StringSplitOptions.RemoveEmptyEntries == (StringSplitOptions.None);
        }
    }
}
