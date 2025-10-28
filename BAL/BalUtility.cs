using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DAL;
using System.Text;
using System.IO;
using System.Threading;
using Utility;
namespace BAL
{
    public class Caluclations
    {
        public decimal TotalPrice { get; set; }
        public decimal ServiceTax { get; set; }
        public decimal VAT { get; set; }
        public decimal OtherCharges { get; set; }
        public decimal ShippingCharges { get; set; }
        public decimal TotalPriceAfterDeductions { get; set; }
    }



    public static class BalUtility
    {
        public static dynamic Blockuser(string url)
        {
            db_Zon_HuwEntities context = new db_Zon_HuwEntities();
            bool istrue = true;
            var id = BalUtility.GetSession(Shared.Sessions.Employee);
            if (id != null)
            {
                var uid = ((User)BalUtility.GetSession(Shared.Sessions.Employee)).UserId;

                var accessid = context.tbl_accesspages.ToList().Where(x => x.Employeeid == uid && x.IsActive==true).ToList();
                foreach (var accid in accessid)
                {
                    var blockurl = (context.def_accesss.Where(x => x.AccessID == accid.Accessid && x.IsPage==true).FirstOrDefault());

                    string blockurls = blockurl.PageUrl;
                    if (blockurls.ToUpper() == url.ToUpper())
                    {
                        istrue = false;
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You are not authorized to access this page');window.location ='Home.aspx';", true);
                    }

                }
            }
            return istrue;
        }
        public static dynamic Logmaintainance(int statusid,string statusname,string refno,string orderid)
        {
            var isexist = BalUtility.GetSession(Shared.Sessions.Employee);
            var isexists = BalUtility.GetSession(Shared.Sessions.AdminLogin);
            var isexistss = BalUtility.GetSession(Shared.Sessions.SuperAdminLogin);
            db_Zon_HuwEntities context = new db_Zon_HuwEntities();
            tbl_Loghistory log = new tbl_Loghistory();
            log.OrdersStatusId =statusid;
            log.OrderStatusname = statusname;
            
            if (isexist != null) { 
            log.Createdby = ((User)BalUtility.GetSession(Shared.Sessions.Employee)).EmailId;
            }
            if (isexists!=null)
            {
                log.Createdby = ((User)BalUtility.GetSession(Shared.Sessions.AdminLogin)).EmailId;
            }
            if (isexistss != null)
            {
                log.Createdby = ((User)BalUtility.GetSession(Shared.Sessions.SuperAdminLogin)).EmailId;
            }
            log.ReferenceNO = refno;
            orderid = orderid.Replace(",", "");
            log.Paymenttransactionid = Convert.ToInt64(orderid);

            log.Createdon = DateTime.Now;
            context.tbl_Loghistory.Add(log);
            context.SaveChanges();
            return log;
        }


        //public dynamic  Filter(string todate ,string fromdate,int status )
        //{
        //    var repository = new UserProductTransactionRepository();
        //    int totalRecords;

        //    var data = repository.FetchAllByPage((p => p.Status = status), out totalRecords, 0, 10, (c => c.CreatedOn >= Convert.ToDateTime(fromdate) && c.CreatedOn <= Convert.ToDateTime(todate)), null, "", true).Distinct();
        //    return data;

        //}


        public static CustomResponse GetInvoice(int TransactionId)
        {
            try
            {


                var repository = new PaymentTransactionRepository();
                int totalRecords;
                PaymentTransaction data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, 0,
                                                          (p => p.PaymentTransactionId == TransactionId), null, "").FirstOrDefault();


                StringBuilder strResult = new StringBuilder();


                foreach (var item in data.UserProductTransactions)
                {

                    strResult.Append(@"<tr><td> " + item.ProductId + @" <br /><small>
                 </td><td>" + item.Product.ProductName + @"</td>
             <td >" + item.Quantity + @"</td>
            <td><span class='WebRupee'>Rs. </span>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.ProductCost * item.PaymentTransaction.CurrencyValue), true) + @"</td>
             <td><span class='WebRupee'>Rs. </span>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.ProductCost * item.PaymentTransaction.CurrencyValue), true) + "</td></tr>");

                }

                StringBuilder strInvoice = new StringBuilder();
                strInvoice.Append(@"
<br/>
<br/>
<br/>
<div style='page-break-after: always;'> <h1>Invoice</h1> 
<div style='page-break-after: always; padding-top:150px;'>     
                 <table border='1'  class='store' style='border:1px #333333; font-size:8px;'> 
        <tr>
<td>6-3-666/1/2, 3 & 4D, Opp NIMS Hospital<br />
      Panjagutta<br />
     Hyderabad-500082<br />
              AP, INDIA<br />
    +91-40-23311595, 23311600, 65506300 <br/>
        http:/www.healthurwealth.com</td>
      <td align='right' valign='top'><table border='1'  style='border:1px #333333; font-size:8px;'>
      <tr><td><b>Date Added:</b></td><td><label ID='lblCreateDate'></label></td>
      </tr> <tr><td><b>Order ID:</b></td> <td><label ID='lblOrderID'></td> </tr>
     <tr><td><b>Shipping Method:</b></td> <td>Flat Shipping Rate</td></tr>
 </table></td></tr></table>
  <table width='100%' border='1'   style='border:1px #CCCCCC; font-size:8px;'><tr class='heading'><td ><b>To</b></td>
  <td width='100%'><b>Ship To (if different address)</b></td></tr><tr><td>
  <label>" + data.User.FirstName + @"</label><br /><label >" + data.User.MobileNo + @"</label><br /><label >" + data.UserProductTransactions.FirstOrDefault().UserAddress.StreetAddress1 + "," + data.UserProductTransactions.FirstOrDefault().UserAddress.StreetAddress2 + "," + data.UserProductTransactions.FirstOrDefault().UserAddress.LandMark + @"</label><br />
      <label >" + data.UserProductTransactions.FirstOrDefault().UserAddress.City + @"</label><br/> <label >" + data.UserProductTransactions.FirstOrDefault().UserAddress.PinCode + @"</label><br/><label >" + data.UserProductTransactions.FirstOrDefault().UserAddress.State.StateName + @"</label>
     <label >" + data.UserProductTransactions.FirstOrDefault().UserAddress.Country.CountryName + @"</label><br /></td>
      <td> <label >" + data.User.FirstName + @"</label> <br /><label>" + data.User.MobileNo + @"</label><br />
       <label >" + data.UserProductTransactions.FirstOrDefault().UserAddress1.StreetAddress1 + "," + data.UserProductTransactions.FirstOrDefault().UserAddress1.StreetAddress2 + "," + data.UserProductTransactions.FirstOrDefault().UserAddress1.LandMark + @"</label><br /><label>" + data.UserProductTransactions.FirstOrDefault().UserAddress1.City + @"</label><br />
       <label >" + data.UserProductTransactions.FirstOrDefault().UserAddress1.PinCode + @"</label> <br /><label>" + data.UserProductTransactions.FirstOrDefault().UserAddress1.State.StateName + @"</label> <br />
       <label >" + data.UserProductTransactions.FirstOrDefault().UserAddress1.Country.CountryName + @"</label></td>
    </tr></table>
  <table class='product'  border='1'  style='border:1px #CCCCCC;font-size:8px;'>
  <tr><td><table  border='1'  style='border:1px #333333; cellspacing='2' cellpadding='2'><thead><tr><td class='left'>Product ID</td><td class='left'>ProductName</td><td class='right'>Quantity</td>
       <td class='right'>Unit Price</td><td class='right'>Total</td></tr>
     </thead><tbody>" + strResult + @" </tbody><tbody id='Tbody1'>
        <tr><td colspan='4' class='right'>ServiceTax:</td><td class='right'><span class='WebRupee'>Rs. </span>" + data.ServiceTax + @"</td></tr></tbody>
       <tbody id='Tbody2'> <tr><td colspan='4' class='right'>ShippingCharges:</td><td class='right'><span class='WebRupee'>Rs. </span>" + data.ShippingCharges + @"</td></tr>
       <tr><td colspan='4' class='right'>OtherCharges:</td><td class='right'><span class='WebRupee'>Rs. </span>" + data.OtherCharges + @"</td></tr>
       <tr><td colspan='4' class='right'>VAT:</td><td class='right'><span class='WebRupee'>Rs. </span>" + data.VAT + @"</td></tr></tbody>
        <tbody id='Tbody3'><tr><td colspan='4' class='right'>Total:</td><td class='right'><span class='WebRupee'>Rs. </span>" + data.TxnAmount + "</td></tr></tbody></table></div></td></tr></table></div>");





                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = data.PaymentTransactionId.ToString(),
                    Result = strInvoice.ToString()
                };

            }
            catch (Exception ex)
            {
                // // Shared.Log.Error(ex);
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Fail.ToString(),
                    Message = ex.Message,
                    Result = null
                };
                throw;
            }
        }




        public static string FormatedDoubleWithOutPrecision(this double DecimalValue, bool HasPrecision = true)
        {
            System.Globalization.NumberFormatInfo format = new System.Globalization.NumberFormatInfo();

            int[] indSizes = { 3, 2, 2 };

            if (HasPrecision)
                format.CurrencyDecimalDigits = 2;
            else
                format.CurrencyDecimalDigits = 0;

            format.CurrencyDecimalSeparator = ".";

            format.CurrencyGroupSeparator = ",";

            format.CurrencySymbol = " ";

            format.CurrencyGroupSizes = indSizes;

            return DecimalValue.ToString("C", format);

        }

        public static void CreateSession<T>(T sessionObject, Shared.Sessions sessionType)
        {
            try
            {

                HttpContext.Current.Session[sessionType.ToString()] = sessionObject;
            }
            catch (Exception ex)
            {
                //// Shared.Log.Error(ex);
                throw;
            }
        }





        public static dynamic GetSession(string sessionType)
        {




            try
            {
                var session = (Shared.Sessions)Enum.Parse(typeof(Shared.Sessions), sessionType);
                return GetSession(session);

            }
            catch (Exception ex)
            {
                //// Shared.Log.Error(ex);
                throw;
            }
        }

        public static void DeleteCartItem(int id, int sid)
        {
            var cartItems = (List<UserProductTransaction>)GetSession(Shared.Sessions.CustomerCart) ??
                            new List<UserProductTransaction>();
            if (sid != 0)
            {
                cartItems.Remove(cartItems.FirstOrDefault(p => p.ProductId == sid));
            }
            else
            {
                cartItems.Remove(cartItems.FirstOrDefault(p => p.ProductId == id));
            }

            CreateSession(cartItems, Shared.Sessions.CustomerCart);
            BAL.BalUtility.GenerateOrderSummary();
        }

        public static void DeleteCompareList(long id)
        {
            var cartItems = (List<UserProductTransaction>)GetSession(Shared.Sessions.CustomerCompareList) ??
                            new List<UserProductTransaction>();

            cartItems.Remove(cartItems.FirstOrDefault(p => p.ProductId == id));
            CreateSession(cartItems, Shared.Sessions.CustomerCompareList);
        }

        public static void DeleteWishListItem(long id)
        {
            var cartItems = (List<CustWishList>)GetSession(Shared.Sessions.CustomerWishList) ??
                            new List<CustWishList>();

            cartItems.Remove(cartItems.FirstOrDefault(p => p.ProductId == id));
            CreateSession(cartItems, Shared.Sessions.CustomerWishList);
        }

        public static string CartInfo()
        {
            var cartItems = (List<UserProductTransaction>)GetSession(Shared.Sessions.CustomerCart) ??
                            new List<UserProductTransaction>();
            return cartItems.Count + " item(s) - " + cartItems.Sum(p => p.ProductDiscountCost);
        }

        public static string CompareListInfo()
        {
            var cartItems = (List<UserProductTransaction>)GetSession(Shared.Sessions.CustomerCompareList) ??
                            new List<UserProductTransaction>();

            return cartItems.Count == 0 ? "Wish List (" + cartItems.Count + ")" : "<a href='WishList.aspx'>Wish List (" + cartItems.Count + ")</a>";
        }

        public static string WishListInfo()
        {
            var cartItems = (List<CustWishList>)GetSession(Shared.Sessions.CustomerWishList) ??
                            new List<CustWishList>();

            return cartItems.Count == 0 ? "Wish List (" + cartItems.Count + ")" : "<a href='WishList.aspx'>Wish List (" + cartItems.Count + ")</a>";
        }




        public static List<UserProductTransaction> GetCart()
        {
            try
            {
                return (List<UserProductTransaction>)GetSession(Shared.Sessions.CustomerCart) ??
                            new List<UserProductTransaction>();
            }
            catch (Exception ex)
            {
                //// Shared.Log.Error(ex);
                throw;
            }
        }

        public static List<UserProductTransaction> GetReplacementList()
        {
            try
            {
                return (List<UserProductTransaction>)GetSession(Shared.Sessions.ReplacementList) ??
                            new List<UserProductTransaction>();
            }
            catch (Exception ex)
            {
                // // Shared.Log.Error(ex);
                throw;
            }
        }

        public static void ManageCart(List<UserProductTransaction> cart)
        {
            try
            {
                HttpContext.Current.Session[Shared.Sessions.CustomerCart.ToString()] = cart;
            }
            catch (Exception ex)
            {
                //  // Shared.Log.Error(ex);
                throw;
            }
        }

        public static bool CheckSession(Shared.Sessions sessionType)
        {
            return HttpContext.Current.Session[sessionType.ToString()] != null;
        }

        public static dynamic GetSession(Shared.Sessions sessionType)
        {
            return (HttpContext.Current.Session[sessionType.ToString()]);
        }

    

        public static void ClearSession(Shared.Sessions sessionType)
        {
            HttpContext.Current.Session[sessionType.ToString()] = null;
        }

        public static void ClearAllSessions()
        {
            HttpContext.Current.Session.Abandon();
        }

        public static dynamic GetProductList(string sord, int minprice, int maxprice, string SubCategories, string Colors, int rows, int categoryId)
        {
            var repository = new ProductRepository();
            int totalRecords;

            List<long> SCat = new List<long>();
            if (SubCategories != null)
            {
                var stringarray = SubCategories.Split('^');
                foreach (var item in stringarray)
                {
                    SCat.Add(Convert.ToInt64(item));
                }
            }

            List<string> Clrs = new List<string>();
            if (Colors != null)
            {
                var stringarray = Colors.Split('^');
                foreach (var item in stringarray)
                {
                    Clrs.Add(item);
                }
            }
            bool desc = false;
            if (sord == "2")
            {
                desc = true;
            }
            Func<Product, object> keySelector = p => p.CreatedOn;
            if (sord == "1" || sord == "2")
            {
                keySelector = p => p.ProductDiscountCost;
            }


            var data = repository.FetchAllByPage(keySelector, out totalRecords, rows, 12,
                                                          (p =>
                                                           p.IsActive && p.IsDeleted == false && p.ProductDiscountCost >= minprice && p.ProductDiscountCost <= maxprice &&
                                                           p.SubCategory.CategoryId == categoryId && (SCat.Count == 0 ? true : SCat.Contains(p.SubCategory.SubCategoryId)) && (Clrs.Count == 0 ? true : Clrs.Contains(p.ProductColor))), null, "", desc).Distinct();

            return data;

        }


        public static Caluclations GetpincodeInfo(string pincode)
        {
            var repository = new PinCodesInformationRepository();
            var pincodeInfo = repository.First(p => p.Pincode == pincode);
            decimal orderSum = BalUtility.GetCart().Sum(p => p.ProductCost);

            CustomCurrency data = (CustomCurrency)BAL.BalUtility.GetSession(Shared.Sessions.Currency);
            orderSum = Math.Round(data.Value * orderSum, 4, MidpointRounding.ToEven);

            Caluclations caluclations = new Caluclations
            {
                TotalPrice = orderSum,

                // ShippingCharges =pincodeInfo.ShippingAmount,




                ShippingCharges = ((pincodeInfo == null) ? 0 : (pincodeInfo.ShippingAmount))
            };
            if (pincodeInfo != null)
            {
                caluclations.ShippingCharges = pincodeInfo.ShippingAmount;
                caluclations.TotalPriceAfterDeductions = Math.Round(orderSum + (caluclations.ShippingCharges), 2, MidpointRounding.ToEven);
                BalUtility.CreateSession(caluclations, Shared.Sessions.ShippingInfo);
                return caluclations;
            }
            else
            {
                caluclations.ShippingCharges = 0;
                caluclations.TotalPriceAfterDeductions = Math.Round(orderSum + (caluclations.ShippingCharges), 2, MidpointRounding.ToEven);
                BalUtility.CreateSession(caluclations, Shared.Sessions.ShippingInfo);
                return caluclations;
            }
        }



        public static Caluclations GenerateOrderSummary()
        {

            var repository = new TaxesConfigurationRepository();
            List<TaxesConfiguration> taxesConfiguration = repository.FetchAllByList();
            decimal orderSum = BalUtility.GetCart().Sum(p => p.ProductCost);
            var Shipping = orderSum <= 1000 ? 50 : 0;
            var totalsum1 = orderSum <= 1000 ? orderSum + 50 : orderSum;
            CustomCurrency data = (CustomCurrency)BAL.BalUtility.GetSession(Shared.Sessions.Currency);
            orderSum = Math.Round(data.Value * orderSum, 4, MidpointRounding.ToEven);

            Caluclations caluclations = new Caluclations
            {
                TotalPrice = orderSum,
                VAT = ((taxesConfiguration == null || taxesConfiguration.Count == 0) ? 0 : (from a in taxesConfiguration where a.TaxId == (int)Shared.ChargeTypes.VAT select a.Taxvalue).FirstOrDefault()) * orderSum / 100,
                ServiceTax = ((taxesConfiguration == null || taxesConfiguration.Count == 0) ? 0 : (from a in taxesConfiguration where a.TaxId == (int)Shared.ChargeTypes.ServiceTax select a.Taxvalue).FirstOrDefault()) * orderSum / 100,
                OtherCharges = ((taxesConfiguration == null || taxesConfiguration.Count == 0) ? 0 : (from a in taxesConfiguration where a.TaxId == (int)Shared.ChargeTypes.OtherCharges select a.Taxvalue).FirstOrDefault()) * orderSum / 100,
                ShippingCharges = Shipping
            };


            caluclations.TotalPriceAfterDeductions = Math.Round(orderSum + (caluclations.VAT + caluclations.ServiceTax + caluclations.OtherCharges + caluclations.ShippingCharges), 2, MidpointRounding.ToEven);
            BalUtility.CreateSession(caluclations, Shared.Sessions.OrderSummary);
            return caluclations;
        }
        public static void DeleteRelatedProductItem(long RelatedPrdctId, long PrdctID)
        {
            var cartItems = (List<RelatedProduct>)GetSession(Shared.Sessions.RelatedProductList) ??
                            new List<RelatedProduct>();
            cartItems.Remove(cartItems.FirstOrDefault(p => p.RelatedProductId == RelatedPrdctId));
            CreateSession(cartItems, Shared.Sessions.CustomerCart);

            var repository = new RelatedProductRepository();
            var data = repository.First(r => r.RelatedProductId == RelatedPrdctId & r.ProductId == PrdctID);
            if (data != null)
            {
                repository.Delete(data);
            }

        }

        public static Product GetProductInfo(int productId )
        {
            var repository = new ProductRepository();
            int totalRecords;

            var data = repository.FetchAllByPage(p => p.ProductId, out totalRecords, 0, 0,
                                                       (p =>
                                                         p.SubCategory.Category.IsActive && p.SubCategory.IsActive &&
                                                        p.ProductId == productId), null, "ProductFeatures,SpecificationType,ProductSpecifications,ProductsGalleries,RelatedProducts,SubProducts").FirstOrDefault();
            return data;
        }
        public static Tbl_AdditionalInfo GetProductAddInfo(long productId)
        {
            using(db_Zon_HuwEntities context=new db_Zon_HuwEntities()) {
                var data = context.Tbl_AdditionalInfo.Where(x => x.ProductId == productId).FirstOrDefault();
            return data;
            }
        }






        public static void DeleteProdcutgallery(int productid)
        {
            try
            {
                var repository = new ProductsGalleryRepository();
                var deletepdctgalry = repository.First(p => p.ProductId == productid);
                if (deletepdctgalry != null)
                {
                    repository.Delete(deletepdctgalry);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static void SendMailForProductStatus(PaymentTransaction data, string htmlPagePath, string PrdctStatsSubject)
        {
            try
            {
                var repository = new PaymentTransactionRepository();
                var PaymntDtls = repository.First(p => p.PaymentTransactionId == data.PaymentTransactionId);

                var rep = new UserProductTransactionRepository();
                var OrderdPrdcts = rep.First(o => o.PaymentTransactionId == PaymntDtls.PaymentTransactionId);

                var Repsitory = new UserRepository();
                var UsrDtls = Repsitory.First(u => u.UserId == PaymntDtls.User.UserId);

                int Transactionid = Convert.ToInt32(PaymntDtls.PaymentTransactionId);
                            

                Utility.MailMessage ms = new Utility.MailMessage();
                ms.Subject = PrdctStatsSubject;
                ms.To = UsrDtls.EmailId;

                string domain;
                Uri url = HttpContext.Current.Request.Url;
                domain = url.AbsoluteUri.Replace(url.PathAndQuery, string.Empty);
                
                string body = string.Empty;

                using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("" + htmlPagePath + "")))
                {
                    body = reader.ReadToEnd();
                }

                Product PrdctDetls = PaymntDtls.UserProductTransactions.FirstOrDefault().Product;

                body = body.Replace("##FirstName##", UsrDtls.FirstName);
                body = body.Replace("##OrderId##", Convert.ToString(PaymntDtls.PaymentTransactionId));
                body = body.Replace("##ProductName##", PrdctDetls.ProductName);
                body = body.Replace("##Brand##", PrdctDetls.Brand);
                body = body.Replace("##ProductQty##", Convert.ToString(PaymntDtls.UserProductTransactions.FirstOrDefault().Quantity));
                body = body.Replace("##Price/Unit##", Convert.ToString(PaymntDtls.UserProductTransactions.FirstOrDefault().ProductCost));
                body = body.Replace("##ConfirmPrdctQty##", Convert.ToString(PaymntDtls.UserProductTransactions.FirstOrDefault().Quantity));

                int subtotalCost = Convert.ToInt32((PaymntDtls.UserProductTransactions.FirstOrDefault().ProductCost) * (PaymntDtls.UserProductTransactions.FirstOrDefault().Quantity));
                body = body.Replace("##SubTotalCost##", (Convert.ToString(subtotalCost)));
                body = body.Replace("##ShippingAmount##", Convert.ToString(PaymntDtls.UserProductTransactions.FirstOrDefault().PaymentTransaction.ShippingCharges));
                int TotalAmount = Convert.ToInt32(subtotalCost + PaymntDtls.UserProductTransactions.FirstOrDefault().PaymentTransaction.ShippingCharges);
                body = body.Replace("##TotalAmount##", (Convert.ToString(TotalAmount)));
                body = body.Replace("##StreetAddress1##", (Convert.ToString(UsrDtls.UserProductTransactions.FirstOrDefault().UserAddress.StreetAddress1)));
                body = body.Replace("##StreetAddress2##", (Convert.ToString(UsrDtls.UserProductTransactions.FirstOrDefault().UserAddress.StreetAddress2)));
                body = body.Replace("##LandMark##", (Convert.ToString(UsrDtls.UserProductTransactions.FirstOrDefault().UserAddress.LandMark)));
                body = body.Replace("##City##", (Convert.ToString(UsrDtls.UserProductTransactions.FirstOrDefault().UserAddress.City)));
                body = body.Replace("##State##", (Convert.ToString(UsrDtls.UserProductTransactions.FirstOrDefault().UserAddress.StateName)));
                body = body.Replace("##PinCode##", (Convert.ToString(UsrDtls.UserProductTransactions.FirstOrDefault().UserAddress.PinCode)));
                body = body.Replace("##MobileNumber##", (Convert.ToString(UsrDtls.MobileNo)));
                body = body.Replace("##ShipmentName##", (PaymntDtls.CourierName));
                body = body.Replace("##ShipmentTrackingID##", (Convert.ToString(PaymntDtls.PaymentTransactionId)));
                body = body.Replace("##order page##", "Order Page");

                ms.Body = body;
                ms.IsBodyHtml = true;

                ms.SendMail();
            }
            catch (Exception ex)
            {
                //// Shared.Log.Error(ex);
                throw ex;
            }

        }


        public static void ForNewslettrEmailIdToInfo(string EmailId)
        {
            try
            {
                Utility.MailMessage ms = new Utility.MailMessage();
                ms.Subject = "EmailId for NewsLetter Signup";
                // ms.To = UsrDtls.EmailId;
                ms.To = "info@healthurwealth.com";
                string domain;
                Uri url = HttpContext.Current.Request.Url;
                domain = url.AbsoluteUri.Replace(url.PathAndQuery, string.Empty);

                //HttpRequest request = System.Web.HttpContext.Current.Request;
                //domain = request.Url.AbsoluteUri.Replace(request.Url.AbsolutePath, String.Empty);
                string body = string.Empty;
                body = "I request you to add my " + EmailId + "(EmailId) for getting NewsLetters from HUW.";

                //using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("")))
                //{
                //    body = reader.ReadToEnd();
                //}



                ms.Body = body;
                //   ms.Body = @"<a href=" + domain + "/UserLogin.aspx > Click here to login</a> <br>" + "Your New password is : " + objUser.Password + "<br>";
                ms.IsBodyHtml = true;


                ParameterizedThreadStart paraThread = new ParameterizedThreadStart(delegate(object o) { ms.SendMail(); });
                Thread th = new Thread(paraThread);
                th.Start();

                // ms.SendMail();

                var status = ms.SendMail();

            }
            catch (Exception ex)
            {
                // // Shared.Log.Error(ex);
                throw ex;
            }
        }

        public static Product GetDeletedProductInfo(int pId)
        {
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {
                Product getproduct = context.Products.Where(x => x.ProductId == pId).First();
                return getproduct;
            }
        }
    }
}
