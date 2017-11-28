using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ServiceModel;
using WCFContract;

namespace WCFServer
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    [WorkerThreadPoolBehavior]
    public class ProductService : IProductService
    {
        public Product GetProduct(string productid)
        {
            Console.WriteLine("Start ThreadID: {0}  hashcode:{1}", Thread.CurrentThread.ManagedThreadId.ToString(), this.GetHashCode());
            Thread.Sleep(2000);
            Console.WriteLine("End ThreadID: {0}  hashcode:{1}", Thread.CurrentThread.ManagedThreadId.ToString(), this.GetHashCode());

            return new Product(productid, productid);
        }
    }
}
