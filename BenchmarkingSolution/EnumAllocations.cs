using BenchmarkDotNet.Attributes;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace BenchmarkingSolution
{
    [MemoryDiagnoser]
    public class EnumAllocations
    {
        enum CustomEnum { a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z }
        static HashSet<int> optimized;
        static HashSet<CustomEnum> bad;
        static CustomEnum[] badIds;
        static int[] goodIds;
        static EnumAllocations()
        {
            optimized = new HashSet<int>(Enum.GetValues(typeof(CustomEnum)).Cast<CustomEnum>().Select(v => (int)v));
            bad = new HashSet<CustomEnum>(Enum.GetValues(typeof(CustomEnum)).Cast<CustomEnum>());
            badIds = (Enum.GetValues(typeof(CustomEnum)).Cast<CustomEnum>()).ToArray();
            goodIds = (Enum.GetValues(typeof(CustomEnum)).Cast<CustomEnum>().Select(v => (int)v)).ToArray();
        }
        [Benchmark]
        public void EnumEq_Allocs_HashSet()
        {
            for (int i = 0; i < badIds.Length; i++)
                bad.Contains(badIds[i]);
        }
        [Benchmark]
        public void EnumEq_DoesNotAlloc_RelaType_HashSet()
        {
            for (int i = 0; i < goodIds.Length; i++)
                optimized.Contains(goodIds[i]);
        }
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
