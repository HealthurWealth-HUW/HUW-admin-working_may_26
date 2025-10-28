using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
    public class Main
    {
        public List<ShipmentData> ShipmentData { get; set; }
    }
    public class ShipmentData
    {
        public Delhiveryapi Shipment { get; set; }
    }
  
    public class Delhiveryapi
    {
        public Statusmodel Status { get; set; }
    }
    public class Statusmodel
    {
        public string Status { get; set; }

        public string StatusLocation { get; set; }
        public DateTime StatusDateTime { get; set; }
        public string RecievedBy { get; set; }
        public string Instructions { get; set; }
        public string StatusType { get; set; }
    }
}
