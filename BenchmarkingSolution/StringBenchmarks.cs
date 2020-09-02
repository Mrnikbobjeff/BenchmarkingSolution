using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BenchmarkingSolution
{
    public class StringBenchmarks
    {
        readonly string test = "testvalue";

        readonly string comparison = "testValux";

        [Benchmark]
        public bool AreEqual_NoArgs()
        {
            return test.Equals(comparison);
        }
        [Benchmark]
        public bool AreEqual_OrdinalIgnoreCase()
        {
            return test.Equals(comparison, StringComparison.OrdinalIgnoreCase);
        }
        [Benchmark]
        public bool AreEqual_Ordinal()
        {
            return test.Equals(comparison, StringComparison.Ordinal);
        }
        [Benchmark]
        public bool AreEqual_InvariantCultureIgnoreCase()
        {
            return test.Equals(comparison, StringComparison.InvariantCultureIgnoreCase);
        }
        [Benchmark]
        public bool AreEqual_CurrentCultureIgnoreCase()
        {
            return test.Equals(comparison, StringComparison.CurrentCultureIgnoreCase);
        }
        [Benchmark]
        public bool AreEqual_InvariantCulture()
        {
            return test.Equals(comparison, StringComparison.InvariantCulture);
        }
        [Benchmark]
        public bool AreEqual_CurrentCulture()
        {
            return test.Equals(comparison, StringComparison.CurrentCulture);
        }

    }
}
