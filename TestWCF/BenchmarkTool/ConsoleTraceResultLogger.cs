using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BenchmarkTool
{
    public class ConsoleTraceResultLogger : ILogTraceResult
    {
        public ConsoleTraceResultLogger()
        {
           
        }

        public void Log(IEnumerable<TraceEvent> result)
        {
            foreach (var item in result)
            {
                Console.WriteLine(item.ToString());
            }
        }
    }
}
