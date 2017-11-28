using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace BenchmarkTool
{
    public static class TimeElapsedTracer
    {
        private static object _locker = new object();

        public static List<TraceEvent> TraceResult { get; private set; }

        public static void  Initial(int maxTraceEvents)
        {
            TraceResult = new List<TraceEvent>(maxTraceEvents);
        }

        public static Stopwatch CreateWatchAndStartTracing()
        {
            return Stopwatch.StartNew();
        }

        public static void TraceStart(Stopwatch stopwatch)
        {
            stopwatch.Start();
        }

        public static TimeSpan TraceStop(string traceName, Stopwatch stopwatch, TimeSpan lastElapsed)
        {
            stopwatch.Stop();

            var elapsed = stopwatch.Elapsed - lastElapsed;
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds - lastElapsed.Milliseconds;
            AddTraceEventSync(traceName, elapsed, elapsedMilliseconds);
            return elapsed;
        }

        public static TimeSpan TraceRestart(string traceName, Stopwatch stopwatch, TimeSpan lastElapsed)
        {
            var elapsed = TraceStop(traceName, stopwatch, lastElapsed);
            stopwatch.Start();
            return elapsed;
        }
        
        public static void TraceStopAndReset(string traceName, Stopwatch stopwatch)
        {
            stopwatch.Stop();

            var elapsed = stopwatch.Elapsed;
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            AddTraceEventSync(traceName, elapsed, elapsedMilliseconds);

            stopwatch.Reset();
        }

        private static void AddTraceEventSync(string traceName, TimeSpan elapsed, long elapsedMilliseconds)
        {
            lock (_locker)
            {
                TraceResult.Add(new TraceEvent
                {
                    ThreadID = Thread.CurrentThread.ManagedThreadId,
                    TraceName = traceName,
                    Elapsed = elapsed,
                    ElapsedMilliseconds = elapsedMilliseconds,
                    CreationTime = DateTime.Now,
                });
            }
        }

        public static void ClearTraceResult()
        {
            lock (_locker)
            {
                TraceResult.Clear();
            }
        }
    }
}
