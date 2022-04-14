using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Model
{
    [DataContract]
    public class Product
    {
        [DataMember]
        public int ProductId { get; set; }
        [DataMember]
        public string Brand { get; set; }
        [DataMember]
        public int Cost { get; set; }
        [DataMember]
        public Sector Sectors { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public string ProductName { get; set; }
    }
}
