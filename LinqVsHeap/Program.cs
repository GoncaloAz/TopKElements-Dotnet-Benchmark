using System;
using BenchmarkDotNet.Running;
using TopKBenchmark;
namespace Program{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<TopKBenchmarker>();
        }
        
    }
}








