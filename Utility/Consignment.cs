using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class Consignment
    {
        public string customer_code { get; set; }
        public string reference_number { get; set; }
        public string service_type_id { get; set; }
        public string load_type { get; set; }
        public string description { get; set; }
        public string consignment_type { get; set; }
        public string dimension_unit { get; set; }
        public string length { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public string weight_unit { get; set; }
        public string weight { get; set; }
        public string declared_value { get; set; }
        public string num_pieces { get; set; }
        public string customer_reference_number { get; set; }
        public bool is_risk_surcharge_applicable { get; set; }
        public string cod_amount { get; set; }
        public string cod_collection_mode { get; set; }

        public OriginDetails origin_details { get; set; }
        public DestinationDetails destination_details { get; set; }
        public PieceDetail[] pieces_detail { get; set; }
    }
    public class OriginDetails
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string alternate_phone { get; set; }
        public string address_line_1 { get; set; }
        public string address_line_2 { get; set; }
        public string pincode { get; set; }
        public string city { get; set; }
        public string state { get; set; }
    }

    public class DestinationDetails
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string alternate_phone { get; set; }
        public string address_line_1 { get; set; }
        public string address_line_2 { get; set; }
        public string pincode { get; set; }
        public string city { get; set; }
        public string state { get; set; }
    }

    public class PieceDetail
    {
        public string description { get; set; }
        public string declared_value { get; set; }
        public string weight { get; set; }
        public string height { get; set; }
        public string length { get; set; }
        public string width { get; set; }
    }
    public class ResponseData
    {
        public string status { get; set; }
        public List<DataItem> data { get; set; }
    }

    public class DataItem
    {
        public bool success { get; set; }
        public string reference_number { get; set; }
        public string courier_partner { get; set; }
        public string courier_account { get; set; }
        public string courier_partner_reference_number { get; set; }
        public double chargeable_weight { get; set; }
        public bool self_pickup_enabled { get; set; }
        public string customer_reference_number { get; set; }
        public List<Piece> pieces { get; set; }
        public string barCodeData { get; set; }
    }

    public class Piece
    {
        public string reference_number { get; set; }
        public string product_code { get; set; }
    }
    public class DTDCObject
    {
        public List<Consignment> consignments { get; set; }
    }
    public class ShippingLabelResponse
    {
        public string status { get; set; }
        public Data data { get; set; }
    }
    public class Data
    {
        public string url { get; set; }
    }
}
