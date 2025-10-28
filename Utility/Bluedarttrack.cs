using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Utility
{
    public class Bluedarttrack
    {

        // using System.Xml.Serialization;
        // XmlSerializer serializer = new XmlSerializer(typeof(ShipmentData));
        // using (StringReader reader = new StringReader(xml))
        // {
        //    var test = (ShipmentData)serializer.Deserialize(reader);
        // }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class BluedartRoot
        {
            public ShipmentData ShipmentData { get; set; }
        }

        public class Shipment
        {
            public string PickUpDate { get; set; }
            public string Origin { get; set; }
            public string Status { get; set; }
            public string Destination { get; set; }
            public string SenderName { get; set; }
            public string StatusTime { get; set; }
            public string RefNo { get; set; }
            public string DestionationAreaCode { get; set; }
            public string CustomerCode { get; set; }
            public string ProductType { get; set; }
            public string Service { get; set; }
            public string WaybillNo { get; set; }
            public string Weight { get; set; }
            public string ExpectedDelivery { get; set; }
            public string StatusDate { get; set; }
            public string ReceivedBy { get; set; }
            public string OriginAreaCode { get; set; }
            public string StatusType { get; set; }
            public string PickUpTime { get; set; }
            public string ToAttention { get; set; }
            public string Prodcode { get; set; }
            public string CustomerName { get; set; }
        }

        public class ShipmentData
        {
            public List<Shipment> Shipment { get; set; }
        }




    }
}
