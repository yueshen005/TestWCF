using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCFContract
{
    [ServiceContract]
    public interface IProductService
    {
        [OperationContract]
        Product GetProduct(string productid);
    }
}
