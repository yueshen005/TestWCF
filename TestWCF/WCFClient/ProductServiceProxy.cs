using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using WCFContract;

namespace WCFClient
{
    public class ProductServiceProxy : ClientBase<IProductService>, IProductService
    {
        public ProductServiceProxy(string endpointConfigurationName) : base(endpointConfigurationName) { }

        public Product GetProduct(string productid)
        {
            return base.Channel.GetProduct(productid);
        }
    }
}
