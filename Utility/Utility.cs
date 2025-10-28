using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;

namespace Utility
{
    public class Deliverypins
    {
        public int R_Pin { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class QcResponse
    {
        public string qc_image { get; set; }
        public string qc_failed_reason { get; set; }
    }

    public class Shiprockettracking
    {
        public TrackingData tracking_data { get; set; }
    }

    public class ShipmentTrack
    {
        public int id { get; set; }
        public string awb_code { get; set; }
        public int courier_company_id { get; set; }
        public int shipment_id { get; set; }
        public int order_id { get; set; }
        public string pickup_date { get; set; }
        public string delivered_date { get; set; }
        public string weight { get; set; }
        public int packages { get; set; }
        public string current_status { get; set; }
        public string delivered_to { get; set; }
        public string destination { get; set; }
        public string consignee_name { get; set; }
        public string origin { get; set; }
        public object courier_agent_details { get; set; }
        public object edd { get; set; }
    }

    public class ShipmentTrackActivity
    {
        public string date { get; set; }
        public string status { get; set; }
        public string activity { get; set; }
        public string location { get; set; }

        [JsonProperty("sr-status")]
        public string srstatus { get; set; }

        [JsonProperty("sr-status-label")]
        public string srstatuslabel { get; set; }
    }

    public class TrackingData
    {
        public int track_status { get; set; }
        public int shipment_status { get; set; }
        public List<ShipmentTrack> shipment_track { get; set; }
        public List<ShipmentTrackActivity> shipment_track_activities { get; set; }
        public string track_url { get; set; }
        public string etd { get; set; }
        public QcResponse qc_response { get; set; }
    }


    public class CustomResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }
        public string CartSum { get; set; }
        public object ShippingAmount { get; set; }
        public object PageRedirect { get; set; }
        public object price { get; set; }
    }
    public class NrmlMessage
    {
        public string message { get; set; }
        //public  string id { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Commodity
    {
        public string CommodityDetail1 { get; set; }
        public string CommodityDetail2 { get; set; }
        public string CommodityDetail3 { get; set; }
    }

    public class Consignee
    {
        public string AvailableDays { get; set; }
        public string AvailableTiming { get; set; }
        public string ConsigneeAddress1 { get; set; }
        public string ConsigneeAddress2 { get; set; }
        public string ConsigneeAddress3 { get; set; }
        public string ConsigneeAddressType { get; set; }
        public string ConsigneeAddressinfo { get; set; }
        public string ConsigneeAttention { get; set; }
        public string ConsigneeBusinessPartyTypeCode { get; set; }
        public string ConsigneeCityName { get; set; }
        public string ConsigneeCountryCode { get; set; }
        public string ConsigneeEmailID { get; set; }
        public string ConsigneeFiscalID { get; set; }
        public string ConsigneeFiscalIDType { get; set; }
        public string ConsigneeFullAddress { get; set; }
        public string ConsigneeGSTNumber { get; set; }
        public string ConsigneeID { get; set; }
        public string ConsigneeIDType { get; set; }
        public string ConsigneeLatitude { get; set; }
        public string ConsigneeLongitude { get; set; }
        public string ConsigneeMaskedContactNumber { get; set; }
        public string ConsigneeMobile { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneePincode { get; set; }
        public string ConsigneeStateCode { get; set; }
        public string ConsigneeTelephone { get; set; }
        public string ConsingeeFederalTaxId { get; set; }
        public string ConsingeeRegistrationNumber { get; set; }
        public string ConsingeeRegistrationNumberIssuerCountryCode { get; set; }
        public string ConsingeeRegistrationNumberTypeCode { get; set; }
        public string ConsingeeStateTaxId { get; set; }
    }

    public class Dimension
    {
        public double Breadth { get; set; }
        public int Count { get; set; }
        public double Height { get; set; }
        public double Length { get; set; }
    }

    public class Itemdtl
    {
        public double CGSTAmount { get; set; }
        public string CommodityCode { get; set; }
        public double Discount { get; set; }
        public string HSCode { get; set; }
        public double IGSTAmount { get; set; }
        public double IGSTRate { get; set; }
        public string Instruction { get; set; }
        public string InvoiceNumber { get; set; }
        public string IsMEISS { get; set; }
        public string ItemID { get; set; }
        public string ItemName { get; set; }
        public double ItemValue { get; set; }
        public int Itemquantity { get; set; }
        public string LicenseNumber { get; set; }
        public string ManufactureCountryCode { get; set; }
        public string ManufactureCountryName { get; set; }
        public double PerUnitRate { get; set; }
        public string PieceID { get; set; }
        public double PieceIGSTPercentage { get; set; }
        public string PlaceofSupply { get; set; }
        public string ProductDesc1 { get; set; }
        public string ProductDesc2 { get; set; }
        public string ReturnReason { get; set; }
        public double SGSTAmount { get; set; }
        public string SKUNumber { get; set; }
        public string SellerGSTNNumber { get; set; }
        public string SellerName { get; set; }
        public string SubProduct1 { get; set; }
        public string SubProduct2 { get; set; }
        public string SubProduct3 { get; set; }
        public string SubProduct4 { get; set; }
        public double TaxableAmount { get; set; }
        public double TotalValue { get; set; }
        public string Unit { get; set; }
        public double Weight { get; set; }
        public double cessAmount { get; set; }
        public string countryOfOrigin { get; set; }
        public string docType { get; set; }
        public int subSupplyType { get; set; }
        public string supplyType { get; set; }
    }

    public class Profile
    {
        public string Api_type { get; set; }
        public string Area { get; set; }
        public string Customercode { get; set; }
        public string IsAdmin { get; set; }
        public string LicenceKey { get; set; }
        public string LoginID { get; set; }
        public string Version { get; set; }
    }

    public class Request
    {
        public Consignee Consignee { get; set; }
        public Returnadds Returnadds { get; set; }
        public Services Services { get; set; }
        public Shipper Shipper { get; set; }
    }

    public class Returnadds
    {
        public string ManifestNumber { get; set; }
        public string ReturnAddress1 { get; set; }
        public string ReturnAddress2 { get; set; }
        public string ReturnAddress3 { get; set; }
        public string ReturnAddressinfo { get; set; }
        public string ReturnContact { get; set; }
        public string ReturnEmailID { get; set; }
        public string ReturnLatitude { get; set; }
        public string ReturnLongitude { get; set; }
        public string ReturnMaskedContactNumber { get; set; }
        public string ReturnMobile { get; set; }
        public string ReturnPincode { get; set; }
        public string ReturnTelephone { get; set; }
    }

    public class BluedartRequest
    {
        public List<Request> Request { get; set; }
        public Profile Profile { get; set; }
    }

    public class Services
    {
        public string AWBNo { get; set; }
        public string ActualWeight { get; set; }
        public string AdditionalDeclaration { get; set; }
        public string AuthorizedDealerCode { get; set; }
        public string BankAccountNumber { get; set; }
        public string BillToAddressLine1 { get; set; }
        public string BillToCity { get; set; }
        public string BillToCompanyName { get; set; }
        public string BillToContactName { get; set; }
        public string BillToCountryCode { get; set; }
        public string BillToCountryName { get; set; }
        public string BillToFederalTaxID { get; set; }
        public string BillToPhoneNumber { get; set; }
        public string BillToPostcode { get; set; }
        public string BillToState { get; set; }
        public string BillToSuburb { get; set; }
        public string BillingReference1 { get; set; }
        public string BillingReference2 { get; set; }
        public double CessCharge { get; set; }
        public double CollectableAmount { get; set; }
        public Commodity Commodity { get; set; }
        public string CreditReferenceNo { get; set; }
        public string CreditReferenceNo2 { get; set; }
        public string CreditReferenceNo3 { get; set; }
        public string CurrencyCode { get; set; }
        public double DeclaredValue { get; set; }
        public string DeliveryTimeSlot { get; set; }
        public List<Dimension> Dimensions { get; set; }
        public string ECCN { get; set; }
        public string EsellerPlatformName { get; set; }
        public string ExchangeWaybillNo { get; set; }
        public string ExportImportCode { get; set; }
        public string ExportReason { get; set; }
        public string ExporterAddressLine1 { get; set; }
        public string ExporterAddressLine2 { get; set; }
        public string ExporterAddressLine3 { get; set; }
        public string ExporterBusinessPartyTypeCode { get; set; }
        public string ExporterCity { get; set; }
        public string ExporterCompanyName { get; set; }
        public string ExporterCountryCode { get; set; }
        public string ExporterCountryName { get; set; }
        public string ExporterDivision { get; set; }
        public string ExporterDivisionCode { get; set; }
        public string ExporterEmail { get; set; }
        public string ExporterFaxNumber { get; set; }
        public string ExporterMobilePhoneNumber { get; set; }
        public string ExporterPersonName { get; set; }
        public string ExporterPhoneNumber { get; set; }
        public string ExporterPostalCode { get; set; }
        public string ExporterRegistrationNumber { get; set; }
        public string ExporterRegistrationNumberIssuerCountryCode { get; set; }
        public string ExporterRegistrationNumberTypeCode { get; set; }
        public string ExporterSuiteDepartmentName { get; set; }
        public string FavouringName { get; set; }
        public string ForwardAWBNo { get; set; }
        public string ForwardLogisticCompName { get; set; }
        public double FreightCharge { get; set; }
        public string GovNongovType { get; set; }
        public string IncotermCode { get; set; }
        public double InsuranceAmount { get; set; }
        public string InsurancePaidBy { get; set; }
        public double InsurenceCharge { get; set; }
        public string InvoiceNo { get; set; }
        public bool IsCargoShipment { get; set; }
        public string IsChequeDD { get; set; }
        public bool IsCommercialShipment { get; set; }
        public bool IsDedicatedDeliveryNetwork { get; set; }
        public bool IsDutyTaxPaidByShipper { get; set; }
        public bool IsEcomUser { get; set; }
        public bool IsForcePickup { get; set; }
        public bool IsIntlEcomCSBUser { get; set; }
        public bool IsPartialPickup { get; set; }
        public bool IsReversePickup { get; set; }
        public int ItemCount { get; set; }
        public string MarketplaceName { get; set; }
        public string MarketplaceURL { get; set; }
        public bool NFEIFlag { get; set; }
        public string NotificationMessage { get; set; }
        public string Officecutofftime { get; set; }
        public string OrderURL { get; set; }
        public bool PDFOutputNotRequired { get; set; }
        public string PackType { get; set; }
        public string ParcelShopCode { get; set; }
        public string PayableAt { get; set; }
        public double PayerGSTVAT { get; set; }
        public string PickupDate { get; set; }
        public string PickupMode { get; set; }
        public string PickupTime { get; set; }
        public string PickupType { get; set; }
        public string PieceCount { get; set; }
        public string PreferredPickupTimeSlot { get; set; }
        public string ProductCode { get; set; }
        public string ProductFeature { get; set; }
        public string PrinterLableSize { get; set; }
        public int ProductType { get; set; }
        public bool RegisterPickup { get; set; }
        public double ReverseCharge { get; set; }
        public string SignatureName { get; set; }
        public string SignatureTitle { get; set; }
        public string SpecialInstruction { get; set; }
        public string SubProductCode { get; set; }
        public string SupplyOfIGST { get; set; }
        public string SupplyOfwoIGST { get; set; }
        public string TermsOfTrade { get; set; }
        public double TotalCashPaytoCustomer { get; set; }
        public double Total_IGST_Paid { get; set; }
        public string itemImg { get; set; }
        public List<Itemdtl> itemdtl { get; set; }
        public int noOfDCGiven { get; set; }
    }

    public class Shipper
    {
        public string CustomerAddress1 { get; set; }
        public string CustomerAddress2 { get; set; }
        public string CustomerAddress3 { get; set; }
        public string CustomerAddressinfo { get; set; }
        public string CustomerBusinessPartyTypeCode { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerEmailID { get; set; }
        public string CustomerFiscalID { get; set; }
        public string CustomerFiscalIDType { get; set; }
        public string CustomerGSTNumber { get; set; }
        public string CustomerLatitude { get; set; }
        public string CustomerLongitude { get; set; }
        public string CustomerMaskedContactNumber { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPincode { get; set; }
        public string CustomerRegistrationNumber { get; set; }
        public string CustomerRegistrationNumberIssuerCountryCode { get; set; }
        public string CustomerRegistrationNumberTypeCode { get; set; }
        public string CustomerTelephone { get; set; }
        public bool IsToPayCustomer { get; set; }
        public string OriginArea { get; set; }
        public string Sender { get; set; }
        public string VendorCode { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ImportDataResult
    {
        public string AWBNo { get; set; }
        public byte[] AWBPrintContent { get; set; }
        public int AvailableAmountForBooking { get; set; }
        public int AvailableBalance { get; set; }
        public string CCRCRDREF { get; set; }
        public string ClusterCode { get; set; }
        public string DestinationArea { get; set; }
        public string DestinationLocation { get; set; }
        public bool IsError { get; set; }
        public bool IsErrorInPU { get; set; }
        public DateTime ShipmentPickupDate { get; set; }
        public List<Status> Status { get; set; }
        public string TokenNumber { get; set; }
        public int TransactionAmount { get; set; }
    }

    public class Bluedartresponse
    {
        public List<ImportDataResult> ImportDataResult { get; set; }
    }

    public class Status
    {
        public string StatusCode { get; set; }
        public string StatusInformation { get; set; }
    }



    public class Shiprockettoken
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public int company_id { get; set; }
        public string token { get; set; }
    }

    public class Boxs
    {
        public int BoxId { get; set; }
        public string BoxName { get; set; }
    }

    public static class Utility
    {
        public static string SetSelectCountry = "Select";
        public static string SetSelectState = "Select";
        public static string SetSelectCity = "Select";
        public static string SetOtherCity = "other";
        public static string SetSelectGender = "--Select--";
        public static string SetSelectVisastatus = "--Select--";
        public static string SetSelectMaritalstatus = "--Select--";
        public static string SetSelectRelationship = "--Select--";
        public static string SetSelectEmployer = "Select";
        public static string ReqBDetails = "Please enter your account number";
        public static string ValBDetails = "Account Number must be in numerics";
        public static string ReqRouting = "Please enter routing number";
        public static string ValRouting = "Routing number must be in number";
        public static string ReqBName = "Please enter Bank Name";
        public static string ValBName = "Bank Name must be in characters";
        public static string ReqAName = "Please enter your Account Name";
        public static string ValAName = "Account Name must be in characters";
        public static string ReqAType = "Please select Account type";
        public static string ReqNEmailId = "Please enter your e-mail address";
        public static string ValNEmailId = "Please enter valid e-mail address";
        public static string ReqCEmailId = "Please enter confirm e-mail address";
        public static string MatchEmailId = "Your EmailIds does not match";
        public static string ReqPwd = "Please enter Password";
        public static string ValPwd = "Password must contain atleast 6 characters long and contain atleast one character and one number";
        public static string ReqNPwd = "Please enter new password";
        public static string ValNPwd = "New Password must contain atleast 6 characters long and contain atleast one character and one number";
        public static string ReqCPwd = " Passwords should not be empty";
        public static string MatchPwd = "Your Passwords does not Match";
        public static string ReqEmp = "Please Select Employer";
        public static string ReqClientName = "Please enter Client Name";
        public static string ValClientName = "Client Name must be in characters";
        public static string ReqSDate = "Please enter Start Date";
        public static string ValSDate = "Please enter Start date in MM/DD/YYYY format";
        public static string ReqEDate = "Please enter End Date";
        public static string ValEDate = "Please enter End date in MM/DD/YYYY format";
        public static string LessSeDate = "Start Date Should be less than End Date";
        public static string ReqCountry = "Please Select Country";
        public static string ReqState = "Please Select State";
        public static string ReqCity = "Please Select City";
        public static string ReqFName = "Please enter your first name";
        public static string ValFName = "Firstname must be in characters";
        public static string ReqMName = "Please enter your middle name";
        public static string ValMName = "Middlename must be in characters";
        public static string ReqLName = "Please enter your last name";
        public static string ValLName = "Lastname must be in characters";
        public static string ReqGender = "Please Select Gender";
        public static string ReqZip = "Please enter Zipcode";
        public static string ValZip = "Please Enter Valid ZipCode";
        public static string ReqMobileNo = "Please enter your Mobile Number";
        public static string ValMobileNo = "Please Enter Valid Mobile Number";
        public static string ReqWorkNo = "Please enter your Work Number";
        public static string ValWorkNo = "Please enter Valid Work Number";
        public static string ReqHomeNo = "Please enter your Home Number";
        public static string ValHomeNo = "Please enter Valid Home Number";
        public static string ReqLandNo = "Please enter LandNumber";
        public static string ValLandNo = "Please enter Valid Land Number";
        public static string ReqEmpName = "Please enter employer name";
        public static string ValEmpName = "Employee name must be in Characters";
        public static string ReqRefbyName = "Please enter Referrer name";
        public static string ValRefbyName = "Referrer name must be in Characters";
        public static string ReqReferralName = "Please enter your Referalname";
        public static string ValReferralName = "Referral name must be in Characters";
        public static string ReqDob = "Please enter DOB";
        public static string ValDob = "Please enter DOB in MM/DD/YYYY format";
        public static string ReqRelationship = "Please Select Relationship";
        public static string ReqSsn = "Please enter SSN number";
        public static string ValSsn = "Please Enter Valid SSN Number";
        public static string ReqVisaStatus = "Please Select VisaStatus";
        public static string ReqDoe = "Please enter date of entry";
        public static string ValDoe = "Please enter DOE in MM/DD/YYYY format";
        public static string ReqOccupation = "Please enter Occupation details";
        public static string ValOccupation = "Occupation must be in characters";
        public static string ReqStreetNo = "Please enter Street Number";
        public static string ValStreetNo = "Please enter a valid StreetNumber";
        public static string ReqAptNo = "Please enter Appartment Number";
        public static string ValAptNo = "Please enter a valid Appartment Number";
        public static string ReqMaritalStatus = "Plese select Marital status";
        public static string ReqDoc = "Please enter Form Name";
        public static string ReqExpence = "Please enter your ExpenceType";
        public static string ValExpence = "ExpenceType must be in Characters";
        public static string ReqMaritalType = "Please enter your MaritalType";
        public static string ValMaritalType = "MaritalType must be in  Characters";
        public static string ReqReferralTypes = "Please enter your ReferralType";
        public static string ValReferralTypes = "ReferralType must be in Characters";
        public static string ReqRelationshipType = "Please enter your RelationshipType";
        public static string ValRelationshipType = "RelationshipType must be in Characters";
        public static string ValVisaStatus = "Enter Valid Visa Status";
        public static string ReqStateName = "Please enter State Name";
        public static string ValStateName = "State Name must be in Characters";
        public static string ReqCityName = "Please enter your City Name";
        public static string ValCityName = "City Name must be in Characters";
        public static string ReqDurationOc = "Please enter your Durationofconsent";
        public static string ValDurationOc = "Please enter valid Numbers";
        public static string ReqSign = "Please enter TaxPayer signature";
        public static string ValSign = "Signature must be in Characters";
        public static string ValFilenumber = "Please enter valid Numbers";
        public static string Requsername = "Please enter recipient email address";
        public static string Valusername = "Please enter valid email address";
        public static string ReqSubject = "Please enter subject";
        public static string ReqMessage = "Please enter Message";
        public static string ReqAnyField = "Please enter atleast one field";
        public static string ReqEnterCity = "Please Enter City";


    }
}