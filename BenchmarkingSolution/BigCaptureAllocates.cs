using BenchmarkDotNet.Attributes;
using System;

namespace BenchmarkingSolution
{
    [MemoryDiagnoser]
    public class BigCaptureAllocates
    {
        public byte[] BigArray { get; } = new byte[100];

        [Benchmark]
        public string AllocatesCapture()
        {
            Func<string> f = () => BigArray.ToString();
            return f();
        }

        [Benchmark]
        public string BetterSignatureNoCatpure()
        {
            Func<byte[], string> f = (x) => x.ToString();
            return f(BigArray);
        }
    }
}
