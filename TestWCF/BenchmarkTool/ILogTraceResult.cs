using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BenchmarkTool
{
    public interface ILogTraceResult
    {
        void Log(IEnumerable<TraceEvent> result);
    }
}
