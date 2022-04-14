using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Model
{
    [DataContract]
    public class Purchase
    {
        [DataMember]
        public int PurchaseId { get; set; }
        [DataMember]
        public int ProductId { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public DateTime? ShopDate { get; set; }
        [DataMember]
        public DateTime? ReturnDate { get; set; }
    }
}
