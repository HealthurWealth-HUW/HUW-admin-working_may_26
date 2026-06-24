
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceReference1;
using ServiceReference2;

/// <summary>
/// Summary description for CreateShipment
/// </summary>
public class CreateShipment
{
    public CreateShipment()
    {
        //
        // TODO: Add constructor logic here
        //

    }


    //for CreateShipment

    internal static ProcessedShipment[] ShippingOrders(string ConsigneeName, string ConsigneePhoneNumber, string ConsigneeAddress1, string ConsigneeAddress2, string ConsigneeLandMark, string ConsigneeCity, int ConsigneePincode, decimal? ConsigneeAmount, string ConsigneeEmailID)
    {

        ShipmentCreationRequest ShipmentCreationReq = new ShipmentCreationRequest();

        //client
        ServiceReference1.ClientInfo objclientinfo = new ServiceReference1.ClientInfo();

        //objclientinfo.AccountNumber = "36670436";
        //objclientinfo.AccountEntity = "BOM";
        //objclientinfo.AccountCountryCode = "IN";
        //objclientinfo.AccountPin = "443543";
        //objclientinfo.UserName = "testingapi@aramex.com";
        //objclientinfo.Password = "R123456789$r";
        //objclientinfo.Version = "v1.0";

        objclientinfo.AccountNumber = "90701139";
        objclientinfo.AccountEntity = "HYD";
        objclientinfo.AccountCountryCode = "IN";
        objclientinfo.AccountPin = "165165";
        objclientinfo.UserName = "info@healthurwealth.com";
        objclientinfo.Password = "info@HUW";
        objclientinfo.Version = "v1.0";

        ShipmentCreationReq.ClientInfo = objclientinfo;
        Shipment objshipment = new Shipment();
        objshipment.AccountingInstrcutions = "Accounting instructions";
        objshipment.Comments = "Comments";
        //objshipment.Details.NumberOfPieces = 5;

        //PartyAddress
        Party party = new Party();
        party.AccountNumber = "90701139";
        party.PartyAddress = new Address();
        party.PartyAddress.CountryCode = "IN";
        party.PartyAddress.Line1 = "Vat/Tin : 36088793856";
        party.PartyAddress.PostCode = ConsigneePincode.ToString();

        //Contact
        Contact contact = new Contact();
        contact.CellPhone = "";
        contact.CompanyName = "Sonal EnterPrises";
        contact.Department = "shipping";
        contact.EmailAddress = "info@healthurwealth.com";
        contact.FaxNumber = "fax";
        contact.Title = "HUW";
        contact.PersonName = "Mukesh";
        party.Contact = contact;

        Shipment[] shipments = new Shipment[1];

        ShipmentCreationReq.Shipments = shipments.ToArray<Shipment>();

        ServiceReference1.Transaction objtr = new ServiceReference1.Transaction();
        objtr.ExtensionData = null;
        objtr.Reference1 = "Sonal Enterprises";
        ShipmentCreationReq.Transaction = objtr;

        ShipmentCreationResponse res = null;
        ServiceReference1.Service_1_0Client client = new ServiceReference1.Service_1_0Client();

        //Details
        objshipment.Details = new ShipmentDetails();

        objshipment.Details.Dimensions = new Dimensions();

        //if ((nudShipmentDetailsActualWeightValue.Value > 0))
        //{
        objshipment.Details.ActualWeight = new Weight();

        objshipment.Details.ChargeableWeight = new Weight();

        objshipment.Details.CashAdditionalAmount = new Money();

        objshipment.Details.InsuranceAmount = new Money();

        objshipment.Details.CollectAmount = new Money();

        objshipment.Details.Items = new ShipmentItem[1];
        ShipmentItem objshipmentitem = new ShipmentItem { Quantity = Convert.ToInt32(1), PackageType = "Cans", Weight = new Weight { Value = Convert.ToDouble(0.25), Unit = "KG" }, Comments = "Comments" };

        objshipment.Details.Items[0] = objshipmentitem;

        //objshipment.Details.ActualWeight.Value = Convert.ToDouble(5.000);
        objshipment.Details.ActualWeight.Value = Convert.ToDouble(0.25);
        objshipment.Details.ActualWeight.Unit = "KG";
        objshipment.Details.ChargeableWeight.Value = Convert.ToDouble(0.01);
        objshipment.Details.ChargeableWeight.Unit = "KG";
        objshipment.Details.InsuranceAmount.CurrencyCode = "INR";
        objshipment.Details.InsuranceAmount.Value = Convert.ToDouble(2);
        objshipment.Details.CashAdditionalAmount.CurrencyCode = "INR";
        objshipment.Details.CashAdditionalAmount.Value = Convert.ToDouble(2);
        objshipment.Details.CollectAmount.Value = Convert.ToDouble(ConsigneeAmount);
        objshipment.Details.CollectAmount.CurrencyCode = "INR";
        //}


        objshipment.Details.ProductGroup = "DOM";
        objshipment.Details.ProductType = "OND";
        objshipment.Details.PaymentType = "P";
        //objshipment.Details.PaymentOptions = txtShipmentDetailsPaymentOptions.Text.Trim();
        //objshipment.Details.Services = txtShipmentDetailsServices.Text.Trim();
        objshipment.Details.NumberOfPieces = Convert.ToInt32(1);
        objshipment.Details.DescriptionOfGoods = "Health UR Wealth";
        objshipment.Details.GoodsOriginCountry = "IN";

        objshipment.Shipper = new Party();
        objshipment.Shipper.Reference1 = "Sonal Enterprises";



        objshipment.Shipper.PartyAddress = new Address();

        objshipment.Details.Dimensions = new Dimensions();

        objshipment.Details.Dimensions.Height = Convert.ToDouble(0);
        objshipment.Details.Dimensions.Width = Convert.ToDouble(0);
        objshipment.Details.Dimensions.Length = Convert.ToDouble(0);
        objshipment.Details.Dimensions.Unit = "CM";

        objshipment.Shipper.Contact = new Contact();
        //objshipment.Shipper.Contact.Department = txtShipperContactDepartment.Text.Trim();
        objshipment.Shipper.Contact.PersonName = "Mukesh";
        //objshipment.Shipper.Contact.Title = txtShipperContactTitle.Text.Trim();
        string Address1 = "Sonal Enterprises,\r6/3/906,A,1,1-C, Somajiguda,\rHyderabad,Telangana,\rINDIA, -500082.";
        var CompanyAddress = Address1.Split("\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
        string CN = "";
        foreach (var CA in CompanyAddress)
        {
            if (CN == "")
            {
                objshipment.Shipper.Contact.CompanyName = CA;
            }
            else
            {
                objshipment.Shipper.Contact.CompanyName = CN + "\r\n" + CA;
            }
            CN = objshipment.Shipper.Contact.CompanyName;
        }
        objshipment.Shipper.Contact.PhoneNumber1 = "7676";
        objshipment.Shipper.Contact.CellPhone = "435";
        objshipment.Shipper.Contact.EmailAddress = "test@g.com";
        objshipment.Shipper.Contact.FaxNumber = "342";
        objshipment.Shipper.PartyAddress = new Address();
        objshipment.Shipper.PartyAddress.Line1 = "Vat/Tin : 36088793856";
        objshipment.Shipper.PartyAddress.PostCode = "500082";
        objshipment.Shipper.PartyAddress.CountryCode = "IN";
        objshipment.Shipper.AccountNumber = "90701139";

        //Recipient
        objshipment.Consignee = new Party();
        objshipment.Consignee.Reference1 = "Vat/Tin : 36088793856";
        objshipment.Consignee.PartyAddress = new Address();
        objshipment.Consignee.PartyAddress.Line1 = ConsigneeAddress1.ToUpper() + ", " + ConsigneeAddress2.ToUpper() + ", " + ConsigneeLandMark.ToUpper() + ", " + ConsigneeCity.ToUpper();
        objshipment.Consignee.PartyAddress.CountryCode = "IN";
        objshipment.Consignee.PartyAddress.PostCode = ConsigneePincode.ToString();

        objshipment.Consignee.Contact = new Contact();
        objshipment.Consignee.Contact.PersonName = ConsigneeName.ToUpper();
        //objshipment.Shipper.Contact.Title = txtShipperContactTitle.Text.Trim();
        objshipment.Consignee.Contact.CompanyName = ConsigneeName.ToUpper();
        objshipment.Consignee.Contact.PhoneNumber1 = ConsigneePhoneNumber.ToString();
        objshipment.Consignee.Contact.CellPhone = ConsigneePhoneNumber.ToString();
        objshipment.Consignee.Contact.EmailAddress = ConsigneeEmailID;


        objshipment.ShippingDateTime = System.DateTime.Now;
        objshipment.DueDate = System.DateTime.Now.AddDays(10);

        shipments[0] = objshipment;

        //ThirdParty
        objshipment.ThirdParty = new Party();
        objshipment.ThirdParty.Reference1 = "Vat/Tin : 36088793856";
        objshipment.ThirdParty.Reference2 = "test2";
        objshipment.ThirdParty.AccountNumber = "90701139";
        // objshipment.ThirdParty.PartyAddress = objshipment.Consignee.PartyAddress;


        objshipment.ThirdParty.Contact = objshipment.Shipper.Contact;

        try
        {
            client.Open();
            bool her = true;
            ProcessedShipment[] processeditems = new ProcessedShipment[1];
            ProcessedShipment objprocessedshipment = new ProcessedShipment();
            processeditems[0] = objprocessedshipment;
            LabelInfo ln = new LabelInfo();
            ln.ReportID = 9201;
            ln.ReportType = "URL";
            ServiceReference1.Notification[] objnot = new ServiceReference1.Notification[1];

            objnot = client.CreateShipments(objclientinfo, ref objtr, shipments, ln, out her, out processeditems);

            client.Close();

            return processeditems;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    //for CreatePickUP

    internal static ProcessedPickup PickUPOrders(DateTime Date)
    {

        PickupCreationRequest PickupCreationReq = new PickupCreationRequest();

        //client
        ServiceReference1.ClientInfo objclientinfo = new ServiceReference1.ClientInfo();
        //objclientinfo.AccountNumber = "36670436";
        //objclientinfo.AccountEntity = "BOM";
        //objclientinfo.AccountCountryCode = "IN";
        //objclientinfo.AccountPin = "443543";
        //objclientinfo.UserName = "testingapi@aramex.com";
        //objclientinfo.Password = "R123456789$r";
        //objclientinfo.Version = "v1.0";
        objclientinfo.AccountNumber = "90701139";
        objclientinfo.AccountEntity = "HYD";
        objclientinfo.AccountCountryCode = "IN";
        objclientinfo.AccountPin = "165165";
        objclientinfo.UserName = "info@healthurwealth.com";
        objclientinfo.Password = "info@HUW";
        objclientinfo.Version = "v1.0";
        PickupCreationReq.ClientInfo = objclientinfo;
        Pickup objpickup = new Pickup();
        ServiceReference1.Transaction objtr = new ServiceReference1.Transaction();
        objpickup.PickupAddress = new Address();
        objpickup.PickupAddress.Line1 = "Line";
        objpickup.PickupAddress.PostCode = "500082";
        objpickup.PickupAddress.CountryCode = "IN";
        objpickup.PickupContact = new Contact();
        objpickup.PickupContact.CellPhone = "43243";
        objpickup.PickupContact.EmailAddress = "test@g.in";
        objpickup.PickupContact.PersonName = "Mukesh";
        objpickup.PickupContact.CompanyName = "Mukesh Medical Hall";
        objpickup.PickupContact.PhoneNumber1 = "676";
        objpickup.PickupLocation = "Hyderabad";
        DateTime s = Date;
        TimeSpan ts = new TimeSpan(0, 00, 0);
        s = s.Date + ts;
        //objpickup.PickupDate = Convert.ToDateTime("1/15/2015 12:00:00 AM");
        //objpickup.ReadyTime = Convert.ToDateTime("1/15/2015 04:00:00 PM");
        //objpickup.LastPickupTime = Convert.ToDateTime("1/15/2015 05:30:00 PM");
        //objpickup.ClosingTime = Convert.ToDateTime("1/15/2015 05:40:00 PM");
        objpickup.PickupDate = s.AddDays(1).AddHours(12);
        objpickup.ReadyTime = objpickup.PickupDate.AddHours(2);
        objpickup.LastPickupTime = objpickup.PickupDate.AddHours(5);
        objpickup.ClosingTime = objpickup.PickupDate.AddHours(5).AddMinutes(40);
        objpickup.Comments = "Comments";
        objpickup.Reference1 = "Test";
        objpickup.Vehicle = "Cycle";
        objpickup.Shipments = null;
        objpickup.PickupItems = new PickupItemDetail[1];
        ServiceReference1.Service_1_0Client client = new ServiceReference1.Service_1_0Client();

        PickupItemDetail objpickupitem = new PickupItemDetail { ProductGroup = "DOM", NumberOfShipments = 1, Payment = "P", PackageType = "BOX", ProductType = "ONP", Comments = "Comments", NumberOfPieces = 1 };
        objpickupitem.ShipmentWeight = new Weight();
        objpickupitem.ShipmentWeight.Unit = "KG";
        objpickupitem.ShipmentWeight.Value = Convert.ToDouble("1.023");
        objpickup.PickupItems[0] = objpickupitem;
        //objpickup.PickupItems[0].ProductGroup = "DOM";
        //objpickup.PickupItems[0].ProductType = "OND";
        //objpickup.PickupItems[0].Payment = "P";
        //objpickup.PickupItems[0].NumberOfShipments = 2;
        //objpickup.PickupItems[0].Comments = "Comments";
        objpickup.Status = "Ready";
        try
        {
            client.Open();
            bool her = true;
            //ProcessedPickup[] processeditems = new ProcessedPickup[1];
            ProcessedPickup objprocessedpickup = new ProcessedPickup();
            // processeditems[0] = objprocessedpickup;
            LabelInfo ln = null;
            ServiceReference1.Notification[] objnot = new ServiceReference1.Notification[1];
            objnot = client.CreatePickup(objclientinfo, ref objtr, objpickup, ln, out her, out objprocessedpickup);

            client.Close();

            return objprocessedpickup;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public static List<ServiceReference2.TrackingResult> ShipmentTracking(string ShipmentID)
    {
        ShipmentTrackingRequest Track = new ShipmentTrackingRequest();

        //client
        ServiceReference2.ClientInfo objclient = new ServiceReference2.ClientInfo();
        objclient.AccountNumber = "90701139";
        objclient.AccountEntity = "HYD";
        objclient.AccountCountryCode = "IN";
        objclient.AccountPin = "165165";
        objclient.UserName = "info@healthurwealth.com";
        objclient.Password = "info@HUW";
        objclient.Version = "v1.0";
        Track.ClientInfo = objclient;
        ServiceReference2.Transaction objtr = new ServiceReference2.Transaction();

        ServiceReference2.Service_1_0Client client = new ServiceReference2.Service_1_0Client();

        string[] Shipments = new string[1];
        Shipments[0] = ShipmentID;

        try
        {
            client.Open();

            bool hasErrors;

            bool getLastTrackingUpdateOnly = false;
            bool getUpdateTimeZone = false;
            bool getReferenceNumber = false;

            Dictionary<string, ServiceReference2.TrackingResult[]> trackingResults;

            string[] nonExistingWaybills;

            ServiceReference2.Notification[] notifications =
                client.TrackShipments(
                    objclient,
                    ref objtr,
                    Shipments,
                    getLastTrackingUpdateOnly,
                    getUpdateTimeZone,
                    getReferenceNumber,
                    out hasErrors,
                    out trackingResults,
                    out nonExistingWaybills
                );

            client.Close();

            if (trackingResults != null && trackingResults.ContainsKey(ShipmentID))
                return trackingResults[ShipmentID].ToList();

            return new List<ServiceReference2.TrackingResult>();
        }
        catch (Exception ex)
        {
            return null;
        }
    }

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
        public string[] ADDONSERVICE { get; set; }
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