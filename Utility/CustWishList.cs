using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
    public class CustWishList
    {
        public long PropertyId { get; set; }
        public long ProductId { get; set; }
        public string ProductImgUrl { get; set; }
        public string ProductName { get; set; }
        public decimal ProductCost { get; set; }
               
    }
}
