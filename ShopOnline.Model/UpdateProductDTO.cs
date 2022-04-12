using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Model
{
    public class UpdateProductDTO
    {
        public Product OldProduct { get; set; }
        public Product NewProduct { get; set; }
    }
}
