using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace BenchmarkingSolution
{
    public class AnalyzeAttribute : Attribute { }

    public readonly struct LongStruct
    {
        public long A { get; }

        public LongStruct(long a)
        {
            A = a;
        }
    }

    [AnalyzeAttribute]
    public struct EvilStruct
    {
        long a;
        public long A { get => !PreventSetter ? a : 0; }
        public bool PreventSetter;
        public EvilStruct(long x)
        {
            a = x;
            PreventSetter = false;
        }

        [AnalyzeAttribute]
        public static long Add(long g) => g + 2;
        public override int GetHashCode() => A.GetHashCode();
    }

    public struct BadStruct
    {
        long a;
        public long A { get => !PreventSetter ? a : 0; }
        public bool PreventSetter;
        public BadStruct(long x)
        {
            a = x;
            PreventSetter = false;
        }
        public long Add() => A + 2l;
        public override readonly int GetHashCode() => A.GetHashCode();
    }

    [AnalyzeAttribute]
    public readonly struct GoodStruct
    {
        readonly long a;
        public long A { get => !PreventSetter ? a : 0; }
        public readonly bool PreventSetter;
        public GoodStruct(long x)
        {
            a = x;
            PreventSetter = false;
        }

        [AnalyzeAttribute]
        public static long Add(long g) => g + 2l;
        public override readonly int GetHashCode() => A.GetHashCode();
    }
    [Analyze]
    [MemoryDiagnoser]
    public class StructShenanigans
    {
        readonly GoodStruct gs = new GoodStruct();
        readonly EvilStruct es = new EvilStruct();
        readonly static long[] samples = new long[16] {1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16 };
        [Benchmark]
        public int AllocatesVirtualCalls()
        {
            var x = new NonIFormattableStruct();
            return x.GetHashCode();
        }

        [Benchmark]
        public long AddingLongBaseline()
        {
            long accumulator = 0;
            for(int i = 0; i < samples.Length; i++)
            {
                AddLongNoInline( accumulator ,samples[i]);
            }
            return accumulator;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        long AddLongNoInline(long a, long b)
        {
            return a + b;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        LongStruct AddStructNoInline(LongStruct a, long b)
        {
            return new LongStruct(a.A + b);
        }

        [Benchmark]
        public LongStruct AddingLongStruct()
        {
            LongStruct accumulator = default;
            for (int i = 0; i < samples.Length; i++)
            {
                accumulator = AddStructNoInline(accumulator, samples[i]);
            }
            return accumulator;
        }

        public int GetHashCodeG<T>(T obj) where T : struct => obj.GetHashCode();
        [Analyze]
        public long DefensiveCopyOnHashcode()
        {
            return es.A;
        }

        [Analyze]
        public long NoDefensiveCopyOnHashcode()
        {
            return gs.A;
        }
    }
}
