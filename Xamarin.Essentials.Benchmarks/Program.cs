using ObjectLayoutInspector;
using System;
using System.Linq;

namespace Xamarin.Essentials.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var pad = typeof(MainThread).Assembly.GetExportedTypes().Where(t => t.IsValueType).Select(t => TypeLayout.GetLayout(t));
            foreach (var p in pad)
                Console.WriteLine(p.Type.FullName);
        }
    }
}
