using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace WCFServer
{
    public class WorkerThreadPoolSynchronizer : SynchronizationContext
    {
        public override void Post(SendOrPostCallback d, object state)
        {
            // WCF almost always uses Post
            ThreadPool.QueueUserWorkItem(new WaitCallback(d), state);
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            // Only the peer channel in WCF uses Send
            d(state);
        }
    }
}
