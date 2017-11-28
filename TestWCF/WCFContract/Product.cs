using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace WCFContract
{
    [DataContract]
    public class Product
    {
        private string _productID;

        [DataMember]
        public string ProductID
        {
            get { return _productID; }
            set { _productID = value; }
        }


        private string _productName;

        [DataMember]
        public string ProductName
        {
            get { return _productName; }
            set { _productName = value; }
        }


        public Product(string productID, string productName)
        {
            _productID = productID;
            _productName = productName;
        }
    }
}
