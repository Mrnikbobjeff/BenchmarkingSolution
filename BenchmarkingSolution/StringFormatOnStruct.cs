using BenchmarkDotNet.Attributes;
using System;
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

    public struct IFormattableStruct : IFormattable
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

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return "1";
        }
    }

    [SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.NetCoreApp30)]
    [MemoryDiagnoser]
    public class BoxCall
    {
        static double a, b, c;
        static BoxCall()
        {
            var rnd = new System.Random();
            a = rnd.NextDouble();
            b = rnd.NextDouble();
            c = rnd.NextDouble();
        }

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

        [Benchmark]
        public string StringFormat_IFormattable()
        {
            var structTest = new IFormattableStruct();

            return $"{structTest}";
        }

        string Constrained<T>(T marker) where T : IMarker => marker.M();

        string Interfaced(IMarker m) => m.M();

        //[Benchmark]
        public string ConstrainedCall()
        {
            var x = new NonIFormattableStruct();
            return Constrained(x);
        }

        //[Benchmark]
        public string InterfaceCall()
        {
            var x = new NonIFormattableStruct();
            return Interfaced(x);
        }

        //[Benchmark]
        public string ToStringOnDouble()
        {
            return $"{nameof(ToStringOnDouble)}: {a}, " +
            $"{nameof(InterfaceCall)}: {b}, " +
            $"{nameof(StringFormat_DirectBox)}: {c}";
        }

        //[Benchmark]
        public string ToStringOnStringOfDouble()
        {
            return $"{nameof(ToStringOnDouble)}: {a.ToString()}, " +
            $"{nameof(InterfaceCall)}: {b.ToString()}, " +
            $"{nameof(StringFormat_DirectBox)}: {c.ToString()}";
        }

    }
}
