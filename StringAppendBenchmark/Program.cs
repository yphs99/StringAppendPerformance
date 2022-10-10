using BenchmarkDotNet.Running;

namespace StringAppendBenchmark
{
    public class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<StringAppendBenchmark>();
        }
    }
}