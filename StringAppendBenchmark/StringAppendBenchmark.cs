using BenchmarkDotNet.Attributes;
using StringAppendPerformance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringAppendBenchmark
{
    /// <summary>
    /// 使用 Benchmark 評測字串串接效能
    /// </summary>
    [MemoryDiagnoser]
    public class StringAppendBenchmark
    {
        /// <summary>
        /// 通用元件
        /// </summary>
        public Utility Utility;

        /// <summary>
        /// 建構子
        /// </summary>
        public StringAppendBenchmark()
        {
            Utility = new Utility();
        }

        /// <summary>
        /// 串接次數
        /// </summary>
        [Params(10, 100, 1000)]
        public int Count;

        /// <summary>
        /// 字串串接(使用+=)
        /// </summary>
        [Benchmark]
        public void Concat()
        {
            Utility.Concat(Count);
        }

        /// <summary>
        /// 字串串接(StringBuilder)
        /// </summary>
        [Benchmark]
        public void Append()
        {
            Utility.Append(Count);
        }
    }
}
