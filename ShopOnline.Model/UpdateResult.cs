using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Model
{
    public class UpdateResult
    {
        public bool isNotExisting { get; set; }
        public bool isUpToDate { get; set; }
        public bool changeAlreadyExist { get; set; }
    }
}
