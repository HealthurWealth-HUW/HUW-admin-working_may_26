using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
    public class UserInfo
    {

        public long UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public decimal Mobile { get; set; }

        public decimal? AlternateContactNumber { get; set; }

        public string Address { get; set; }
        public int Status { get; set; }
        public decimal OrderId { get; set; }
        public int OrdersPalced { get; set; }
        public int SuccessOrders { get; set; }


    }
}
