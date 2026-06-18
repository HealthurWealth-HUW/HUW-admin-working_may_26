using BAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
//using log4net;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Utility
{
    public static class Shared
    {
        public enum SareeStatus
        {
            IsSold,
            IsNotSold,
            ProductNotAvaiable

        }

        public static string ProductStatus(SareeStatus status)
        {
            switch (status)
            {
                case SareeStatus.IsSold:
                    return "sold out already";
                case SareeStatus.IsNotSold:
                    return "available for sale";
                case SareeStatus.ProductNotAvaiable:
                    return "Not Available ";

            }
            return "";

        }

        private static string _currencyRegex = "rhs: \\\"(\\d*.\\d*)";

        public static decimal ConvertAmount(int amount, string fromCurrency, string toCurrency)
        {
            try
            {
                WebClient web = new WebClient();
                var query = string.Format("{0}{1}%3D%3F{2}", amount, fromCurrency, toCurrency);
                string url = "http://www.google.com/ig/calculator?hl=en&q=" + query;

                string response = web.DownloadString(url);

                //todo check for error

                //find right hand side rate
                var match = Regex.Match(response, _currencyRegex);
                var rate = System.Convert.ToDecimal(match.Groups[1].Value);
                rate = Math.Round(rate, 4, MidpointRounding.ToEven);
                return rate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string RandomString(int length)
        {
            var r = new Random(Environment.TickCount);
            const string chars = "0123456789abcdefghijklmnopqrstuvwxyz";
            var builder = new StringBuilder(length);

            for (var i = 0; i < length; ++i)
                builder.Append(chars[r.Next(chars.Length)]);

            return builder.ToString();
        }





        // public static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public enum PaymentTransactionStatus
        {
            Assigned = -1,
            Success = 0,
            Fail = 1,
            User_dropped = 2,
            Canceled = 3,
            Forwarded = 4,
            Pg_forward_fail = 5,
            Inquiry_status_failed = 6,
            Session_expired = 7,
            Refund_initiated = 8,
            Refund_forwarded = 9,
            Refund_process = 10,
            Refund_success = 11,
            Refund_failed = 12,
            Pending_verification = 13,
            Success_on_verification = 14,
            Rejected_by_payment_gateway = 15,
            PG_REJECTED = 16,
            Checkout = 17,
            Closed = 18
        }

        public enum RefundStatus
        {
            Pending = 1,
            Success = 2,

        }
        public enum ReturnAction
        {
            Refund = 1,
            Replacement = 2,
            Exchange = 3
        }
        public static ReturnAction GetReturnActionEnum(string ReturnAction)
        {
            return (ReturnAction)Enum.Parse(typeof(ReturnAction), ReturnAction, true);

        }
        public static RefundStatus GetRefundStatusEnum(string RefundStatus)
        {
            return (RefundStatus)Enum.Parse(typeof(RefundStatus), RefundStatus, true);

        }
        public enum OrderStatus
        {
            Cancelled = 1,
            Pending = 0,
            Delivered = 3,
            Refund = 4,
            Returns = 5,
            Readytoship = 6,
            StockPending = 7,
            WaitingforPickup = 8,
            Dispatched = 9,
            Checkout = 10,
            Closed=11,
            Downloaded = 12,
            Cashbackassigned=13

        }
        public static OrderStatus GetOrderStatusEnum(string Orderstatus)
        {
            return (OrderStatus)Enum.Parse(typeof(OrderStatus), Orderstatus, true);

        }
        public static string GetpaymentTransactionStatus(OrderStatus OrderStatus)
        {
            switch (OrderStatus)
            {
                case OrderStatus.Pending:
                    return "Pending";
                    break;
                case OrderStatus.WaitingforPickup:
                    return "Waiting for Pickup"; break;
                case OrderStatus.StockPending:
                    return "Stock Pending"; break;
                case OrderStatus.Readytoship:
                    return "Ready to ship"; break;
                case OrderStatus.Dispatched:
                    return "Dispatched"; break;
                case OrderStatus.Delivered:
                    return "Delivered"; break;
                case OrderStatus.Cancelled:
                    return "Cancelled"; break;
                case OrderStatus.Refund:
                    return "Refund"; break;
                case OrderStatus.Returns:
                    return "Returns"; break;
                case OrderStatus.Checkout:
                    return "Checkout"; break;
                case OrderStatus.Closed:
                    return "Closed"; break;
                default:
                    return ""; break;

            }
        }

        public static PaymentTransactionStatus GetpaymentTransactionStatusEnum(string paymenttransactionstatus)
        {
            return (PaymentTransactionStatus)Enum.Parse(typeof(PaymentTransactionStatus), paymenttransactionstatus, true);

        }

        public static string GetpaymentTransactionStatus(PaymentTransactionStatus paymenttransactionstatus)
        {
            switch (paymenttransactionstatus)
            {
                case PaymentTransactionStatus.Success:
                    return "Success";
                    break;
                case PaymentTransactionStatus.Fail:
                    return "Fail"; break;
                case PaymentTransactionStatus.User_dropped:
                    return "User dropped"; break;
                case PaymentTransactionStatus.Canceled:
                    return "Canceled"; break;
                case PaymentTransactionStatus.Forwarded:
                    return "Forwarded"; break;
                case PaymentTransactionStatus.Pg_forward_fail:
                    return "Pg forward fail"; break;
                case PaymentTransactionStatus.Inquiry_status_failed:
                    return "Inquiry status failed"; break;
                case PaymentTransactionStatus.Session_expired:
                    return "Session expired"; break;
                case PaymentTransactionStatus.Refund_initiated:
                    return "Refund initiated"; break;
                case PaymentTransactionStatus.Refund_forwarded:
                    return "Refund forwarded"; break;
                case PaymentTransactionStatus.Refund_process:
                    return "Refund process"; break;
                case PaymentTransactionStatus.Refund_success:
                    return "Refund success"; break;
                case PaymentTransactionStatus.Refund_failed:
                    return "Refund failed"; break;
                case PaymentTransactionStatus.Pending_verification:
                    return "Pending verification"; break;
                case PaymentTransactionStatus.Success_on_verification:
                    return "Success on verification"; break;
                case PaymentTransactionStatus.Rejected_by_payment_gateway:
                    return "Rejected by payment gateway"; break;
                case PaymentTransactionStatus.Checkout:
                    return "Checkout"; break;
                case PaymentTransactionStatus.Closed:
                    return "Closed"; break;
                default:
                    return ""; break;

            }
        }

        public static string GetpaymentTransactionMessageByStatus(string paymenttransactionstatus)
        {
            switch (paymenttransactionstatus.ToLower())
            {
                case "success":
                    return
                        @"<div   class='full_page'  > 
  <h1>Your Order {0} Has Been Processed {1}!</h1>
  <div class='success'><p style='line-height:20px; text-align:justify;'>Your order has been {1} .Reason : {2}</p>
<p style='line-height:20px; text-align:justify;'>Please direct any questions you have to the <a href='#'>Chettinad's Customer Care</a>.</p>
<p style='line-height:20px; text-align:justify;'>Thanks for shopping with us online!</p></div>
  <div class='action_buttonbar'>
		<button type='button' onClick=javascript:window.top.location.href='MyTransactions.aspx' title='' class='continue'>Continue</button>
	</div>
  </div>";
                    break;
                case "fail":
                case "canceled":
                    return @"<div   class='full_page'  > 
  <h1>Your Order {0} Has Been {1} !</h1>
  <div class='warning'><p style='line-height:20px; text-align:justify;'>Your order has been {1} .Reason : {2}</p>
<p style='line-height:20px; text-align:justify;'>Please direct any questions you have to the <a href='#'>Chettinad's Customer Care</a>.</p>
<p style='line-height:20px; text-align:justify;'>Thanks for shopping with us online!</p></div>
  <div class='action_buttonbar'>
		<button type='button' onClick=javascript:window.top.location.href='MyTransactions.aspx' title='' class='continue'>Continue</button>
	</div>
  </div>"; break;
                case "user dropped":
                case "forwarded":
                case "Pg forward fail":
                case "inquiry status failed":
                case "session expired":
                case "refund initiated":
                case "refund forwarded":
                case "refund process":
                case "refund success":
                case "refund failed":
                case "pending verification":
                case "success on verification":
                case "rejected by payment gateway":
                    return @"<div   class='full_page'  > 
  <h1>Your Order {0} Has Been Processed {1}!</h1>
  <div class='attention'><p style='line-height:20px; text-align:justify;'>Your order has been {0} .Reason : {2} </p>
<p style='line-height:20px; text-align:justify;'>Please direct any questions you have to the <a href='#'>Chettinad's Customer Care</a>.</p>
<p style='line-height:20px; text-align:justify;'>Thanks for shopping with us online!</p></div>
  <div class='action_buttonbar'>
		<button type='button' onClick=javascript:window.top.location.href='MyTransactions.aspx' title='' class='continue'>Continue</button>
	</div>
  </div>"; break;
                default:
                    return ""; break;

            }
        }

        public enum CountryCurrency
        {
            INR,
            USD,
        }

        public static string GetCountry(this CountryCurrency currency)
        {
            switch (currency)
            {
                case CountryCurrency.INR:
                    return "India";
                    //case CountryCurrency.USD:
                    //    return "America";
                    //   case default :
                    //   return "India";
            }
            return "";
        }

        public static string GetCurrencySymbol(this CountryCurrency currency)
        {
            switch (currency)
            {
                case CountryCurrency.INR:
                    return "₹";
                    //case CountryCurrency.USD:
                    //    return "$";
                    //   case default :
                    //   return "India";
            }
            return "";
        }

        public static string GetCurrencySymbol(this string strcurrency)
        {
            CountryCurrency currency = (CountryCurrency)Enum.Parse(typeof(CountryCurrency), strcurrency);

            switch (currency)
            {
                case CountryCurrency.INR:
                    return "₹";
                case CountryCurrency.USD:
                    return "$";
                    //   case default :
                    //   return "India";
            }
            return "";
        }

        public enum ResponseStatus
        {
            Fail = 0,
            Success = 1,
            NoData = 2
        }

        public enum DelivarytStatus
        {
            Pending = 0,//Delivery in progress
            Success = 1,//delivered 
            Fail = 2,//Delivery Failed
            TransactionFail = 3,//Amount Transaction Failed
            Cancelled = 4,
            ReadyForShippingORUnprocessed = 5,
            Delivered = 6,
            Refund = 7,
            Replacements = 8,
            Returns = 9,
            ReturnsandRefunds = 10,
            Sales = 11,
            Shipped = 12,
            StockPending = 13,
            WaitingForPickup = 14,
            Dispatched = 15,
            Checkout = 16
        }

        public enum UserRoles
        {
            None = 0,
            Admin,
            Customer,
            SuperAdmin,
            Administrator,
            SubAdmin,
            Employee
        }

        public enum ProductImageTypes
        {
            ProductImg,
            GalleryImg
        }

        public enum Address
        {
            Billing = 1,
            Shipping = 2
        }

        public static string GetImageSavingPaths(ProductImageTypes imgType)
        {
            //var filePath = @"C:\Users\Hi\Desktop\HUW1\";
            //switch (imgType)
            //{
            //    case ProductImageTypes.ProductImg:
            //        filePath = @"C:\Users\Hi\Desktop\HUW1\";
            //        break;
            //    case ProductImageTypes.GalleryImg:
            //        filePath = @"C:\Users\Hi\Desktop\HUW1\";
            //        break;
            //}

            var filePath = ConfigurationManager.AppSettings["UploadFilesPath"];
            switch (imgType)
            {
                case ProductImageTypes.ProductImg:
                    filePath = ConfigurationManager.AppSettings["UploadFilesPath"];
                    break;
                case ProductImageTypes.GalleryImg:
                    filePath = ConfigurationManager.AppSettings["UploadFilesPath"];
                    break;
            }
            //return filePath + String.Format("{0:d9}", (DateTime.Now.Ticks / 10) % 1000000000);
            return filePath;
        }

        public enum Actions
        {
            Create,
            Read,
            Update,
            Delete,
        }

        public enum AddressTypes
        {
            None = 0,
            BillingAddress,
            ShippingAddress
        }

        public enum SpecificationTypes
        {
            TextBox = 1,
            DropDown = 2,
            List = 3
        }

        public enum PageRedirection
        {
            WishList = 1,
            MyTransaction = 2,
        }

        public static string GetAddressType(AddressTypes addressType)
        {
            string strAddressType;
            switch (addressType)
            {
                case AddressTypes.None:
                    strAddressType = "";
                    break;
                case AddressTypes.BillingAddress:
                    strAddressType = "Billing Address";
                    break;
                case AddressTypes.ShippingAddress:
                    strAddressType = "Shipping Address";
                    break;
                default:
                    strAddressType = "";
                    break;
            }
            return strAddressType;
        }


        public static PageRedirection PageRedirect(string redirect)
        {
            return (PageRedirection)Enum.Parse(typeof(PageRedirection), redirect, true);

        }
        public static string PageRedirections(int redirectval)
        {
            string redirectUrl;


            if (redirectval == 1)
            {
                redirectUrl = " WishList.aspx";
                return redirectUrl;
            }

            if (redirectval == 2)
            {
                redirectUrl = "MyTransactions.aspx";
                return redirectUrl;
            }


            return "Search.aspx";
        }

        public enum RedirectFromLogin
        {
            None = 0,
            Admin,
            Customer,
            CustomerCartLogin,
        }

        public static string GetRedirectPage(RedirectFromLogin redirectFromLogin)
        {
            string redirectUrl;
            switch (redirectFromLogin)
            {
                case RedirectFromLogin.Admin:
                    redirectUrl = "~/Admin/Home.aspx";
                    break;
                case RedirectFromLogin.Customer:
                    redirectUrl = "Search.aspx";
                    break;
                case RedirectFromLogin.CustomerCartLogin:
                    redirectUrl = "SmartBilling.aspx";
                    break;

                case RedirectFromLogin.None:
                    redirectUrl = "";
                    break;
                default:
                    redirectUrl = "";
                    break;
            }
            return redirectUrl;
        }

        public enum ProductStatusForSendMail
        {
            None = 0,
            Pending,
            Dispatched,
            Delivered,
            Cancel,
            password,
            Notifyme,
            Shipment,
            UserOrderCancel,
            ResendOrder,
            AdminOrderCancel,
            Contactus,
            CashOnDelivery,
            Success,
            NotifyMeAvailable,
            CodPending,
        }
        public static string GethtmlPage(ProductStatusForSendMail redirectHtmlpage)
        {
            string HtmlPge = String.Empty;
            switch (redirectHtmlpage)
            {
                case ProductStatusForSendMail.Dispatched:
                    HtmlPge = "~/Mail_Pages/ProductDispatched.html";
                    break;
                case ProductStatusForSendMail.Pending:
                    HtmlPge = "~/Mail_Pages/ProductPending.html";
                    break;
                case ProductStatusForSendMail.Delivered:
                    HtmlPge = "~/Mail_Pages/ProductDelivered.html";
                    break;
                case ProductStatusForSendMail.Cancel:
                    HtmlPge = "~/Mail_Pages/ProductCancel.html";
                    break;
                case ProductStatusForSendMail.password:
                    HtmlPge = "~/Mail_Pages/Forgotpassword.html";
                    break;
                case ProductStatusForSendMail.Notifyme:
                    HtmlPge = "~/Mail_Pages/Notifyme.html";
                    break;
                case ProductStatusForSendMail.Shipment:
                    HtmlPge = "~/Mail_Pages/ShipmentCreated.html";
                    break;
                case ProductStatusForSendMail.ResendOrder:
                    HtmlPge = "~/Mail_Pages/OrderReSend.html";
                    break;
                case ProductStatusForSendMail.CashOnDelivery:
                    HtmlPge = "~/Mail_Pages/CashOnDelivery_OTP.html";
                    break;
                case ProductStatusForSendMail.UserOrderCancel:
                    HtmlPge = "~/Mail_Pages/UserRequestforOrderCancelling.html";
                    break;
                case ProductStatusForSendMail.Contactus:
                    HtmlPge = "~/Mail_Pages/ContactUs.html";
                    break;
                case ProductStatusForSendMail.AdminOrderCancel:
                    HtmlPge = "~/Mail_Pages/OrderCancelledbyAdmin.html";
                    break;
                case ProductStatusForSendMail.Success:
                    HtmlPge = "~/Mail_Pages/PaymentSuccess.html";
                    break;
                case ProductStatusForSendMail.NotifyMeAvailable:
                    HtmlPge = "~/Mail_Pages/Notifyme Available.html";
                    break;
                case ProductStatusForSendMail.CodPending:
                    HtmlPge = "~/Mail_Pages/ProductPending_000.html";
                    break;
            }
            return HtmlPge;
        }

        public static string GetPrdctStatusSubject(ProductStatusForSendMail MailSubject)
        {
            string Subject = String.Empty;
            switch (MailSubject)
            {
                case ProductStatusForSendMail.Dispatched:
                    Subject = "your Ordered Product(s) has Dispatched by HUW.com.";
                    break;
                case ProductStatusForSendMail.Pending:
                    Subject = "your Ordered Product(s) has been Pending by HUW.com.";
                    break;
                case ProductStatusForSendMail.Delivered:
                    Subject = "your Ordered Product(s) had Delivered by HUW.com.";
                    break;

            }
            return Subject;
        }


        public enum fromMailId
        {
            info,
            infoEmailId,
            CustomerCare
        }

        public static string GetFromMailId(fromMailId FmMailID)
        {
            string fmMail = string.Empty;
            switch (FmMailID)
            {
                case fromMailId.info:
                    fmMail = "orders@healthurwealth.com";
                    break;
                case fromMailId.infoEmailId:
                    fmMail = "info@healthurwealth.com";
                    break;
                case fromMailId.CustomerCare:
                    fmMail = "customercare@healthurwealth.com";
                    break;
            }
            return fmMail;
        }

        public enum Sessions
        {
            Promocode,
            prescriptionbuy,
            CustomerCompareList,
            CustomerLogin,
            AdminLogin,
            CustomerCart,
            CustomerWishList,
            Currency,
            OrderSummary,
            ShippingInfo,
            RelatedProductList,
            ReplacementList,
            SuperAdminLogin,
            SubAdminLogin,
            Employee,
            FreeProductList

        }

        public enum CartSteps
        {
            OrderDetails,
            ShippingDetails,
            OrderSummary,
            Payment
        }

        public enum ChargeTypes
        {
            ServiceTax = 1,
            VAT = 2,
            OtherCharges = 3,
            ShippingCharges = 4
        }

    }

    public static class Enumeration
    {
        public static IDictionary<int, string> GetAll<TEnum>() where TEnum : struct
        {
            var enumerationType = typeof(TEnum);

            if (!enumerationType.IsEnum)
                throw new ArgumentException("Enumeration type is expected.");

            var dictionary = new Dictionary<int, string>();

            foreach (int value in Enum.GetValues(enumerationType))
            {
                var name = Enum.GetName(enumerationType, value);
                dictionary.Add(value, name);
            }

            return dictionary;
        }
    }

    public static class UploadFile
    {
        public static string Upload(this HttpPostedFile file, Shared.ProductImageTypes imgType)
        {
            try
            {
                // var path = Shared.GetImageSavingPaths(imgType) + "." + file.FileName.Split('.')[1];
                var path = Shared.GetImageSavingPaths(imgType);
                var fileName = String.Format("{0:d9}", (DateTime.Now.Ticks / 10) % 1000000000) + "." + file.FileName.Split('.')[1];
                if (file.ContentLength > 0)
                {
                    file.SaveAs(Path.Combine(path, fileName));
                    //Test Code
                    //file.SaveAs(path);
                    //Live Code
                    // file.SaveAs(HttpContext.Current.Server.MapPath(path));
                }
                return path + fileName;

            }
            catch (Exception ex)
            {
                DbLogger.LogError(
                    ex,
                    "UploadFile.Upload",
                    "FileName: " + file?.FileName +
                    ", ContentLength: " + file?.ContentLength +
                    ", ImageType: " + imgType.ToString()
                );

                throw;
            }
        }
    }

    public static class ExtensionMethods
    {
        public static Collection<T> ToCollection<T>(this List<T> items)
        {
            var collection = new Collection<T>();

            foreach (var t in items)
            {
                collection.Add(t);
            }

            return collection;
        }

        public static string ToSpecialFormatString(this decimal val)
        {
            if (val == Math.Floor(val))
            {
                return val.ToString("N0");
            }
            return val.ToString("N2");
        }
    }
}
