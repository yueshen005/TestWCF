using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using WCFServer;
using WCFContract;
using System.Threading;

namespace WCFHost
{
    class Program
    {
        static void Main(string[] args)
        {
            int cpuNumber = Environment.ProcessorCount;
            //
            //最佳线程数 = CPU核数 / （1 - 阻塞系数）
            //阻塞系数 = 阻塞时间 / （阻塞时间 + 使用CPU时间）
            //一般是CPU * 2 + 2
            //
            int minThread = 100;
            int ioThread = 200;
           
            Console.WriteLine("cpunumber: {0}", cpuNumber);

            using (ServiceHost host = new ServiceHost(typeof(ProductService)))
            {
                ThreadPool.SetMinThreads(minThread, ioThread);
                ThreadPool.SetMaxThreads(300, 600);

                host.Opened += (o, e) =>
                {
                    Console.WriteLine("服务已启动");
                };
                host.Closed += (o, e) =>
                {
                    Console.WriteLine("服务已停止");
                };
                host.Open();

                Console.WriteLine("按任意键停止服务...");
                Console.ReadKey();

                host.Close();

                Console.WriteLine("按任意键退出...");
                Console.ReadKey();
            }
        }
    }
}
