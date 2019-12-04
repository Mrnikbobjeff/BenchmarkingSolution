using BenchmarkDotNet.Attributes;
using System.Runtime.CompilerServices;

namespace BenchmarkingSolution
{
    public interface IMarker { string M(); }
    public struct NonIFormattableStruct : IMarker
    {
        public string M()
        {
            return "2";
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public override string ToString()
        {
            return "1";
        }
    }

    [SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.NetCoreApp30)]
    [MemoryDiagnoser]
    public class BoxCall
    {
        [Benchmark]
        public string StringFormat_CallsToString()
        {
            var structTest = new NonIFormattableStruct();

            return $"{structTest.ToString()}";
        }


        [Benchmark]
        public string StringFormat_DirectBox()
        {
            var structTest = new NonIFormattableStruct();

            return $"{structTest}";
        }

        string Constrained<T>(T marker) where T : IMarker => marker.M();

        string Interfaced(IMarker m) => m.M();

        [Benchmark]
        public string ConstrainedCall()
        {
            var x = new NonIFormattableStruct();
            return Constrained(x);
        }

        [Benchmark]
        public string InterfaceCall()
        {
            var x = new NonIFormattableStruct();
            return Interfaced(x);
        }

    }
}
