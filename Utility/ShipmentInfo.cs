using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
    public class ShipmentInfo
    {
        public string username { get; set; }
        public string password { get; set; }
        public json_input json_input { get; set; }
    }

    public class json_input
    {
        public string AWB_NUMBER { get; set; }
        public string ORDER_NUMBER { get; set; }
        public string PRODUCT { get; set; }
        public string CONSIGNEE { get; set; }
        public string CONSIGNEE_ADDRESS1 { get; set; }
        public string CONSIGNEE_ADDRESS2 { get; set; }
        public string CONSIGNEE_ADDRESS3 { get; set; }
        public string DESTINATION_CITY { get; set; }
        public string PINCODE { get; set; }
        public string STATE { get; set; }
        public string MOBILE { get; set; }
        public string TELEPHONE { get; set; }
        public string ITEM_DESCRIPTION { get; set; }
        public string PIECES { get; set; }
        public string COLLECTABLE_VALUE { get; set; }
        public string DECLARED_VALUE { get; set; }
        public string ACTUAL_WEIGHT { get; set; }
        public string VOLUMETRIC_WEIGHT { get; set; }
        public string LENGTH { get; set; }
        public string BREADTH { get; set; }
        public string HEIGHT { get; set; }
        public string PICKUP_NAME { get; set; }
        public string PICKUP_ADDRESS_LINE1 { get; set; }
        public string PICKUP_ADDRESS_LINE2 { get; set; }
        public string PICKUP_PINCODE { get; set; }
        public string PICKUP_PHONE { get; set; }
        public string PICKUP_MOBILE { get; set; }
        public string RETURN_NAME { get; set; }
        public string RETURN_ADDRESS_LINE1 { get; set; }
        public string RETURN_ADDRESS_LINE2 { get; set; }

        public string RETURN_PINCODE { get; set; }
        public string RETURN_PHONE { get; set; }
        public string RETURN_MOBILE { get; set; }
        public string ADDONSERVICE { get; set; }
        public string DG_SHIPMENT { get; set; }
        public ADDITIONAL_INFORMATION ADDITIONAL_INFORMATION { get; set; }

    }

    public class ADDITIONAL_INFORMATION
    {
        public string SELLER_TIN { get; set; }
        public string INVOICE_NUMBER { get; set; }
        public string INVOICE_DATE { get; set; }

        public string ESUGAM_NUMBER { get; set; }
        public string ITEM_CATEGORY { get; set; }
        public string PACKING_TYPE { get; set; }
        public string PICKUP_TYPE { get; set; }
        public string RETURN_TYPE { get; set; }
        public string PICKUP_LOCATION_CODE { get; set; }
        public string SELLER_GSTIN { get; set; }
        public string GST_HSN { get; set; }
        public string GST_ERN { get; set; }
        public string GST_TAX_NAME { get; set; }
        public string GST_TAX_BASE { get; set; }
        public string DISCOUNT { get; set; }
        public string GST_TAX_RATE_CGSTN { get; set; }
        public string GST_TAX_RATE_SGSTN { get; set; }
        public string GST_TAX_RATE_IGSTN { get; set; }
        public string GST_TAX_TOTAL { get; set; }
        public string GST_TAX_CGSTN { get; set; }
        public string GST_TAX_SGSTN { get; set; }
        public string GST_TAX_IGSTN { get; set; }
    }
}
