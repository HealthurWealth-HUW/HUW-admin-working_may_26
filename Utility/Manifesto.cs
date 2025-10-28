using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Manifesto
/// </summary>
/// 
namespace Utility
{
    public class Manifesto
    {
        public Manifesto()
        {
            //
            // TODO: Add constructor logic here
            //



        }

        public string  ShipmentId { get; set; }
        public string Address { get; set; }
        public decimal MobileNumber { get; set; }
        public DateTime? ShipmentDate { get; set; }
        public string ShortDate { get; set; }
        public string ProductName { get; set; }
    }
}