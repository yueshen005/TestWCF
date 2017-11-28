using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BenchmarkTool;
using WCFContract;
using System.Threading;
using System.ServiceModel;

namespace WCFClient
{
    class Program
    {
        static int threadCount = 100;
        
        static void Main(string[] args)
        {
            CountdownEvent waitHandle = new CountdownEvent(threadCount);
            
            ThreadPool.SetMaxThreads(400, 800);
            ThreadPool.SetMinThreads(40, 80);

            TimeElapsedTracer.Initial(threadCount);

            Enumerable.Range(0, threadCount).All(a =>
            {
                //InvokeOnceByNewClient(waitHandle);
                //Thread t = new Thread(InvokeOnceByNewClient);
                //t.Start(waitHandle);

                ThreadPool.QueueUserWorkItem(InvokeOnceByNewClient, waitHandle);
                //ThreadPool.QueueUserWorkItem(InvokeOnceByCachedFactory, waitHandle);
                return true;
            });

            waitHandle.WaitAll();
            waitHandle.Dispose();
            Console.WriteLine("==comlete tracing==");

            //var logger = new DBTraceResultLogger("Data Source=.;Initial Catalog=Test;Integrated Security=true", "test3000factoryhttp");
            //logger.Log(TimeElapsedTracer.TraceResult);
            
            //ConsoleTraceResultLogger logger = new ConsoleTraceResultLogger();
            //logger.Log(TimeElapsedTracer.TraceResult);

            Console.WriteLine("==comlete logging==");

            TimeElapsedTracer.ClearTraceResult();
            
            Console.WriteLine("=========comlete==============");
            Console.ReadKey();
        }

        private static void InvokeOnceByNewClient(object o)
        {
            Console.WriteLine("ThreadID: {0}", Thread.CurrentThread.ManagedThreadId.ToString());
            var waitHandle = o as CountdownEvent;

            var stopwatch = TimeElapsedTracer.CreateWatchAndStartTracing();

            ProductServiceProxy productService = new ProductServiceProxy("ProductServiceHttp");
            
            var elapsed = TimeElapsedTracer.TraceRestart("创建Proxy", stopwatch, TimeSpan.Zero);
            productService.Open();

            Console.WriteLine("Start tid {0}", Thread.CurrentThread.ManagedThreadId.ToString());
            var product = productService.GetProduct("XXXX");

            Console.WriteLine("END tid {0}", Thread.CurrentThread.ManagedThreadId.ToString());

            elapsed = TimeElapsedTracer.TraceRestart("调用proxy", stopwatch, elapsed);
            
            (productService as IDisposable).Dispose();
            TimeElapsedTracer.TraceStop("dispose proxy", stopwatch, elapsed);

            
            TimeElapsedTracer.TraceStopAndReset("全部", stopwatch);

           
            waitHandle.SetOne();
        }

        static ChannelFactory<IProductService> factory = new ChannelFactory<IProductService>("ProductServiceHttp");

        private static void InvokeOnceByCachedFactory(object o)
        {
            Console.WriteLine("ThreadID: {0}", Thread.CurrentThread.ManagedThreadId.ToString());
            var waitHandle = o as CountdownEvent;
            
            var stopwatch = TimeElapsedTracer.CreateWatchAndStartTracing();

            var proxy = factory.CreateChannel();
            var elapsed = TimeElapsedTracer.TraceRestart("创建Proxy", stopwatch, TimeSpan.Zero);
            (proxy as IClientChannel).Open();
            var product = proxy.GetProduct("xxxx");
            elapsed = TimeElapsedTracer.TraceRestart("调用proxy", stopwatch, elapsed);
            Console.WriteLine(elapsed.ToString());
            (proxy as IClientChannel).Close();
            TimeElapsedTracer.TraceStop("dispose proxy", stopwatch, elapsed);

            TimeElapsedTracer.TraceStopAndReset("全部", stopwatch);

            waitHandle.SetOne();

        }
    }
}
