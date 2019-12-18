using BenchmarkDotNet.Running;
using System;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;

namespace BenchmarkingSolution
{
    public class BenchmarkInformation
    {
        public Type BenchmarkType { get; set; }
        public string Description { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var infos = new BenchmarkInformation[]
            {
                new BenchmarkInformation
                {BenchmarkType = typeof(MethodGroupConversions), Description="Method group conversions allocate on each invocation." },

                new BenchmarkInformation
                {BenchmarkType = typeof(NonValueTypeEnumerator), Description="IEnumerators allocate, indexing calls don't." },
                new BenchmarkInformation
                {BenchmarkType = typeof(BoxCall), Description="Don't pass value types to reference type constrained things." },
                new BenchmarkInformation
                {BenchmarkType = typeof(BigCaptureAllocates), Description="Pass parameters instead of capturíng." },
                new BenchmarkInformation
                {BenchmarkType = typeof(StructShenanigans), Description="readonly etc" },
                new BenchmarkInformation
                {BenchmarkType = typeof(EnumAllocations), Description="Enum shit etc" }
            };
            for (var f = 0; f < infos.Length; f++)
                Console.WriteLine($"{f}:{infos[f].Description}");
            Console.WriteLine("Select which benchmark to run, 42 to exit.");
            var i = 0;
            while (!int.TryParse(Console.ReadLine(), out i) || i >= infos.Length)
                Console.WriteLine("Enter valid index!");

            var benchmark = infos[i];
            var a = AssemblyDefinition.ReadAssembly(typeof(Program).Assembly.Location);
            var attribute = a.MainModule.GetType(typeof(AnalyzeAttribute).FullName);
            var method = a.MainModule.Types.Where(z => z.CustomAttributes.Any(td => td.AttributeType == attribute)).SelectMany(ts => ts.Methods).Where(m => m.CustomAttributes.Any(a => a.AttributeType == attribute));
            foreach (var m in method)
            {
                Console.WriteLine(m.Name);
                foreach (var instr in m.Body.Instructions)
                    Console.WriteLine($"{instr.OpCode.Name} : {instr.Operand}");

            }
            Console.ReadKey();
            BenchmarkRunner.Run(benchmark.BenchmarkType);
        }
    }
}
