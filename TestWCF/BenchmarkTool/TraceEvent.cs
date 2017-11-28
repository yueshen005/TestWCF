using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BenchmarkTool
{
    public class TraceEvent
    {
        public string TraceName { get; set; }

        public int ThreadID { get; set; }

        public TimeSpan Elapsed { get; set; }

        public long ElapsedMilliseconds { get; set; }

        public DateTime CreationTime { get; set; }

        public override string ToString()
        {
            return string.Format("TraceName:{0}  ThreadID:{1}  Elapsed:{2}  ElapsedM:{3}  Time:{4}",
                TraceName,
                ThreadID,
                Elapsed,
                ElapsedMilliseconds,
                CreationTime);
        }
    }
}
