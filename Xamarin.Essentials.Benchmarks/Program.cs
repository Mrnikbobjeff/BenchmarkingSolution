using ObjectLayoutInspector;
using System;
using System.Linq;

namespace Xamarin.Essentials.Benchmarks
{
    class Program
    {
        public class IGeneric<A, B, C, D, E, F> { }
        static void Main(string[] args)
        {
            long baseline = GC.GetTotalMemory(true);
            var valueTypes = new[] { typeof(int), typeof(byte), typeof(short), typeof(DateTime), typeof(double), typeof(long) };
            foreach (var vt in valueTypes)
            {
                foreach (var vt2 in valueTypes)
                    foreach (var vt3 in valueTypes)
                        foreach (var vt4 in valueTypes)
                            foreach (var vt5 in valueTypes)
                                foreach (var vt6 in valueTypes)
                                    Activator.CreateInstance(typeof(IGeneric<,,,,,>).MakeGenericType(vt, vt2, vt3, vt4, vt5, vt6));
            }
            GC.Collect(2, GCCollectionMode.Forced);
            GC.Collect(2, GCCollectionMode.Forced);
            GC.Collect(2, GCCollectionMode.Forced);
            long baseline2 = GC.GetTotalMemory(true);
            Console.ReadKey();
        }
    }
}
