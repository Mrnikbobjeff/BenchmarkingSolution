using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BenchmarkingSolution
{
    public class AnalyzeAttribute : Attribute { }

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
        [Benchmark]
        public int AllocatesVirtualCalls()
        {
            var x = new NonIFormattableStruct();
            return x.GetHashCode();
        }

        public int GetHashCodeG<T>(T obj) where T : struct => obj.GetHashCode();
        [Analyze]
        [Benchmark]
        public long DefensiveCopyOnHashcode()
        {
            return es.A;
        }

        [Analyze]
        [Benchmark]
        public long NoDefensiveCopyOnHashcode()
        {
            return gs.A;
        }
    }
}
