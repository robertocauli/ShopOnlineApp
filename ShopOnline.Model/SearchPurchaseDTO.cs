﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Model
{
    public class SearchPurchaseDTO
    {
        public Product Product { get; set; }
        public User User { get; set; }
        public bool ReturnOrNot { get; set; }
    }
}
