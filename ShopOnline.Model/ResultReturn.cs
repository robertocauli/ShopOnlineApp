using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Model
{
    public class ResultReturn
    {
        public Product ProductToReturn { get; set; }
        public bool isAvailable { get; set; }
        public bool isSuccessful { get; set; }

    }
}
