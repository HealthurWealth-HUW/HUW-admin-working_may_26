using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BAL;
using System.Text;
using System.Threading;
using System.Net;
using System.Web;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Net.NetworkInformation;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
//using ServiceReference1;
using System.Web.Script.Serialization;
using System.Web.Configuration;
using DAL;
using Utility;
using System.Web.UI;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using ClosedXML.Excel;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using Newtonsoft.Json;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using RestSharp;
using Org.BouncyCastle.Ocsp;
using Org.BouncyCastle.Asn1.Ocsp;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Office2013.Excel;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class MasterController : ApiController
{

    string ApiUrl = System.Configuration.ConfigurationManager.AppSettings["ApiUrl"];

    //public dynamic GetOrderStatus(int PTransactionId) // Getting Order Details using PaymentTransactionID 
    //{
    //    var repository = new UserProductTransactionRepository();
    //    var data = repository.GetOrderDetls(PTransactionId); //getting Order details from BALRepository
    //    return data;
    //}

    public dynamic GetOrderDetls(int TransactionId) // Getting Order Details using PaymentTransactionID 
    {
        var repository = new UserProductTransactionRepository();
        var data = repository.GetOrderDetls(TransactionId); //getting Order details from BALRepository
        return data;
    }
    public dynamic getimage(int id)
    {
        RelatedProductRepository GetData = new RelatedProductRepository();
        var data = GetData.getimages(id);
        return data;
    }

    [HttpGet]
    public dynamic IsApproveReview(int rid)  // Review checking by Admin
    {
        try
        {
            var repository = new ReviewRepository();
            long ReviewId = Convert.ToInt32(rid);
            var data = repository.First(p => p.ReviewId == ReviewId);
            data.IsApproved = true;

            repository.Update(data);

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Message = "Review Apporved Successfully",
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }
    }

    [HttpGet]
    public dynamic PaymentReports(string sidx, string sord, int page, int rows) // Payment Reports in SuperAdmin Role
    {
        var repository = new PaymentTransactionRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, 0, null, c => new { c.PaymentTransactionId, c.UserId, c.TxnRefNo, c.TxnStatus, c.TxnMessage, c.TxnAmount, c.CurrencyCode }).ToList();

        return new
        {
            total = totalRecords / rows + 1,
            page,
            records = totalRecords,
            rows = data
        };
    }


    public dynamic GetProductReviews() // Getting Product Reviews
    {
        try
        {
            var repository = new ReviewRepository();
            int totalRecords;
            var data = repository.FetchAllByPage(p => p.ReviewId, out totalRecords, 0, 0, (p => p.IsApproved == false && p.IsDeleted == 0));
            StringBuilder strResult = new StringBuilder();
            StringBuilder strtrResult = new StringBuilder();
            StringBuilder strMsg = new StringBuilder();
            foreach (var item in data)
            {
                strtrResult.Append(@"<tr>
			 <td class='name'><a style='color:#064792' class='rm' href='UpdateProducts.aspx?id=" + item.Product.ProductId + @"'>" + item.Product.ProductId + @"</a></td>
			 <td class='name'>" + item.ReviewId + @"</td>
			 <td class='name'>" + item.UserId + @"</td>
			 <td class='name'>" + item.Review + @"</td>
			 <td class='name'>" + item.IsApproved + @"</td><td><div class='remove'><a class='rm' alt='Remove' title='Remove' onclick='AdminIsApproveReview(" + item.ReviewId + @");' ><span>Approve</span></a></div></td>
			 <td><div class='remove'><button id='Button1' class='button'  onclick='DeleteReviews(" + item.ReviewId + @")'  return false;' type='button'><span><span>Remove</span></span></button> ");
            }

            string msg = "No Reviews ";
            if (data.Count == 0)
            {
                strMsg.Append(@"<div><p>" + msg + "</p></div>");
            }

            strResult.Append(@"<table><thead>
			<tr><td class='image'>ProductId</td>
				<td class='model'>ReviewId</td><td class='quantity'>UserId</td>
				<td class='price'>Review</td><td class='total'>ApproveStatus</td><td>Approve</td><td>Remove</td></tr></thead><tbody>
					   " + strtrResult + "</tbody></table>" + strMsg + "");
            return new CustomResponse
            {
                Status = string.IsNullOrWhiteSpace(strResult.ToString()) ? Shared.ResponseStatus.NoData.ToString() : Shared.ResponseStatus.Success.ToString(),
                Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
                Result = strResult.ToString()
            };

            //if(data.Count > 0)
            //{
            //    return new CustomResponse
            //    {
            //        Status = Shared.ResponseStatus.Success.ToString(),
            //        Message = "",
            //        Result = data
            //    };
            //}
            //else
            //{
            //    return new CustomResponse
            //    {
            //        Status = Shared.ResponseStatus.NoData.ToString(),
            //        Message = "No Reviews are Found",
            //        Result = null
            //    };
            //}
        }

        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }

    }


    public dynamic GetCartItems() // Getting Cart Items
    {
        var cartItems = (List<UserProductTransaction>)BalUtility.GetSession(Shared.Sessions.CustomerCart) ??
                          new List<UserProductTransaction>();
        if (cartItems.Count == 0)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Message = "No Items",
                Result = null
            };
        }
        else
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Message = "",
                Result = cartItems
            };
        }
    }

    public dynamic GetProductDetails(int ProductId) // Getting Product Details using ProductID
    {
        var repository = new ProductRepository();
        var data = repository.GetProductDetails(ProductId); // Getting Product Details using ProductID from BALRepository
        return data;
    }

    public CustomResponse GetCartProductsSession()
    {
        var data = BalUtility.GetCart();

        var item = (from a in data select a);
        // item.Product = null;
        var totalSum = data.Sum(p => p.ProductCost);
        var sum = BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(totalSum), true);
        var prodCost = "";
        foreach (var items in item)
        {
            prodCost = BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(items.ProductCost), true);
        }
        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),

            Result = Convert.ToString(prodCost),
            // CartSum = Convert.ToString(sum)
        };
    }

    [HttpGet]
    public CustomResponse GetProfileDetails(int Uid)
    {
        try
        {
            // var userId = ((User)BalUtility.GetSession(Shared.Sessions.CustomerLogin)).UserId;

            var Userrepository = new UserRepository();
            var data = Userrepository.Single(p => p.UserId == Uid);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Message = "",
                Result = data

            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }
    }


    //Logout ClearAllSession
    [HttpGet]
    public CustomResponse ClearSession()
    {
        try
        {

            BalUtility.ClearSession(Shared.Sessions.CustomerLogin);
            BalUtility.ClearSession(Shared.Sessions.ShippingInfo);
            //BalUtility.ClearAllSessions();
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Message = "",
                Result = "Success",
            };
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = "Nodata",
            };
            throw;
        };
    }

    //End Logout


    //AddToCart
    [HttpPost]
    public dynamic AddToCart(UserProductTransaction cart)
    {
        try
        {
            var pId = cart.ProductId;

            var repository = new ProductRepository();
            var cartItems = (List<UserProductTransaction>)BalUtility.GetSession(Shared.Sessions.CustomerCart) ??
                            new List<UserProductTransaction>();

            //Currency Conversion
            CustomCurrency CurrentCurrency = (CustomCurrency)BAL.BalUtility.GetSession(Shared.Sessions.Currency);
            cart.CurrencyCode = CurrentCurrency.ToCurrency;
            cart.CurrencyValue = CurrentCurrency.Value;
            cart.CurrencySymbol = CurrentCurrency.Symbol;

            //Product Info
            var product = repository.Single(p => p.ProductId == pId);
            cart.Status = 0;
            cart.Product = product;
            cart.ProductCost = product.ProductDiscountCost;
            cart.ProductDiscountCost = product.ProductDiscountCost;
            cart.ProductDiscountPercentage = product.ProductDiscountPercentage;
            cartItems.Add(cart);
            deleteFromWishList(pId);
            BalUtility.CreateSession(cartItems, Shared.Sessions.CustomerCart);

            System.Web.HttpCookie cookie = new System.Web.HttpCookie("CartInfo");
            if (cookie == null)
            {
                cookie["ProductID"] = pId.ToString();
                cookie.Expires = DateTime.Now.AddDays(30);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }

            if (cartItems.Count > 0)
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = CustomMessages.CartAddedSuccessfully,
                    Result = ""
                };
            }
            else
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.NoData.ToString(),
                    Message = "Nodata",
                    Result = null

                };
            }
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
            throw;
        }
    }


    public dynamic AddToCart()
    {
        try
        {

            System.Web.HttpCookie cookie = HttpContext.Current.Request.Cookies["CartInfo"];
            long pId = Convert.ToInt64(cookie["ProductID"]);

            var repository = new ProductRepository();
            var cartItems = (List<UserProductTransaction>)BalUtility.GetSession(Shared.Sessions.CustomerCart) ??
                            new List<UserProductTransaction>();
            if (cartItems.Count == 0)
            {

                //Product Info
                var product = repository.Single(p => p.ProductId == pId);
                UserProductTransaction cart = new UserProductTransaction();
                CustomCurrency CurrentCurrency = (CustomCurrency)BAL.BalUtility.GetSession(Shared.Sessions.Currency);
                cart.CurrencyCode = CurrentCurrency.ToCurrency;
                cart.CurrencyValue = CurrentCurrency.Value;
                cart.CurrencySymbol = CurrentCurrency.Symbol;

                cart.Status = 0;
                cart.Product = product;
                cart.ProductCost = Convert.ToInt64(cookie["ProductCost"]);
                cart.ProductDiscountCost = product.ProductDiscountCost;
                cart.ProductDiscountPercentage = product.ProductDiscountPercentage;
                cart.Quantity = Convert.ToInt32(cookie["Quantity"]);
                cartItems.Add(cart);
                // deleteFromWishList(pId);
                BalUtility.CreateSession(cartItems, Shared.Sessions.CustomerCart);
            }
            if (cartItems.Count > 0)
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = CustomMessages.CartAddedSuccessfully,
                    Result = ""
                };
            }
            else
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.NoData.ToString(),
                    Message = "Nodata",
                    Result = null

                };
            }

        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
            throw;
        }
    }

    //Delete WishList
    [HttpGet]
    public dynamic deleteFromWishList(long id)
    {
        try
        {
            BalUtility.DeleteWishListItem(id);

            var cartItems = (List<CustWishList>)BalUtility.GetSession(Shared.Sessions.CustomerWishList) ??
                           new List<CustWishList>();

            if (cartItems.Count == 0)
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.NoData.ToString(),
                    Message = "No More Products in List",
                    Result = null
                };
            }
            else
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = "",
                    Result = BalUtility.WishListInfo()

                };

            }
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
            throw;
        }
    }
    //ConformOrder
    public dynamic ConfirmOrder()
    {


        Caluclations caluclations = BalUtility.GenerateOrderSummary();
        decimal orderSum = BalUtility.GetCart().Sum(p => p.ProductDiscountCost);
        var repository = new PaymentTransactionRepository();


        var data = BalUtility.GetCart();

        StringBuilder strtrResult = new StringBuilder();

        foreach (var item in data)
        {

            strtrResult.Append(@"<tr> <td class='name'><img style='height:50px;width:50px;' src='" + item.Product.ProductImgUrl.Replace("~/", ApiUrl) + "' alt='" + item.Product.ProductName + @"'> </td> 
		<td class='model'>" + item.Product.ProductName + @"</td> <td class='quantity'>" + item.Quantity + @"</td> <td class='price'>" + item.CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.ProductDiscountCost), true) + @"</td>
		<td class='total'>" + item.CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.Quantity * item.ProductDiscountCost), true) + "</td> </tr> ");

        }


        StringBuilder strResult = new StringBuilder();
        strResult.Append(@"  <div class='checkout-product'> <table> <thead> <tr> <td class='name'>Product Name</td> <td class='model'>Model</td> <td class='quantity'>Quantity</td> <td class='price'>Price</td> <td class='total'>Total</td> </tr> </thead> <tbody> 
" + strtrResult + @" </tbody> <tfoot> <tr> 
<td colspan='4' class='price'><b>Sub-Total:</b></td> <td class='total'>" + data.FirstOrDefault().CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(orderSum), true) + @"</td> 

</tr> <tr> <td colspan='4' class='price'><b>ServiceTax:</b></td> 
<td class='total'>" + data.FirstOrDefault().CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(caluclations.ServiceTax), true) + @"</td> </tr> <tr> <td colspan='4' class='price'><b>ShippingCharges:</b></td> 
<td class='total'>" + data.FirstOrDefault().CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(caluclations.ShippingCharges), true) + @"</td> </tr><tr> <td colspan='4' class='price'><b>OtherCharges:</b></td> 
<td class='total'>" + data.FirstOrDefault().CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(caluclations.OtherCharges), true) + "</td> </tr>  <tr> <td colspan='4' class='price'><b>VAT :</b></td> <td class='total'>" + data.FirstOrDefault().CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(caluclations.VAT), true) + @"</td> </tr> <tr>
<td colspan='4' class='price'><b>Total:</b></td> <td class='total'>" + data.FirstOrDefault().CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(orderSum), true) + @"</td> </tr> </tfoot> </table> </div>
<div class='payment'><div class='buttons'> <div class='right'>
<a id='button-confirm' class='button' onclick='MakePayment()'>
<span>Confirm Order</span></a></div> </div> ");
        return new CustomResponse
        {
            Status = string.IsNullOrWhiteSpace(strResult.ToString()) ? Shared.ResponseStatus.NoData.ToString() : Shared.ResponseStatus.Success.ToString(),
            Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
            Result = strResult.ToString()
        };
    }

    //New User
    public dynamic NewUserRegistrationForm()
    {
        StringBuilder strResult = new StringBuilder();
        strResult.Append(@"
<div class='content'>
			 <table class='form'> <tr> <td><span class='required'>*</span> First Name:</td>
	<td><input type='text'  id='txbFirstName' name='firstname' value='' class='large-field' /></td>
	</tr> <tr> <td><span class='required'>*</span> Last Name:</td> 
	<td><input type='text' name='lastname' id='txbLastName' value='' class='large-field' /></td> </tr> 
   
	<tr> <td><span class='required'>*</span> EmailId:</td> <td><input type='text' id='txbEmailId' value='' class='large-field' /></td> </tr> <tr> <td>Password:</td> <td><input type='password' id='txbPassword' value='' class='large-field' /></td> </tr> 
	<tr> <td><span class='required'>*</span> MobileNumber:</td> <td><input type='text' id='txbMobileNumber' value='' class='large-field' /></td> </tr>
 </table> </div> <br /> <div class='buttons'> <div class='right'><a id='button-address' class='button' onclick='UserRegistration()'><span>Continue</span></a></div> </div> 
   </div> ");
        return new CustomResponse
        {
            Status = string.IsNullOrWhiteSpace(strResult.ToString()) ? Shared.ResponseStatus.NoData.ToString() : Shared.ResponseStatus.Success.ToString(),
            Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
            Result = strResult.ToString()
        };
    }


    [HttpGet]
    public dynamic UserAddressDetails(int rows)
    {
        try
        {
            var userId = ((User)BalUtility.GetSession(Shared.Sessions.CustomerLogin)).UserId;
            StringBuilder strResult = new StringBuilder();
            var addressrepository = new AddressRepository();
            int totalRecords;
            var data2 = addressrepository.FetchAllByPage(p => p.UserId, out totalRecords, rows, 3,
                                             (p => p.UserId == userId));


            if (data2.Count > 0)
            {
                int k = 0;
                foreach (var item in data2)
                {
                    strResult.Append(@"
<div class='tradus-select-user-address-page2' style='line-height:20px;'>
<div class='tradus-select-user-address-bg'>
<span class='tradus-select-username'>
<label id='lblAddress1'>Shipping Address1 :</label>
<label id='lblAdd1'>" + item.StreetAddress1 + @"</label><br />
<label id='lblAddress2'>Shipping Address2 :</label>
<label id='lblAdd2'>" + item.StreetAddress2 + @"</label><br />
</span>
<span class='tradus-select-user-content'>
<label id='lblMark'>Nearest Landmark :</label>
<label id='lblLandMark'>" + item.LandMark + @"</label>
<br>
<label id='lblcityes'>City :</label>
<label id='lblCity'>" + item.City + @"</label>
<br/>
<label id='lblStates'>State :</label>
<label id='lblState'>" + item.StateName + @"</label>
<br>
<label id='lblCountryes'>Country :</label>
<label id='lblCountry'>" + item.Country.CountryName + @"</label>
<br>
<label id='lblPincode'>Zip/Postal Code :</label>
<label id='lblPinCode'>" + item.PinCode + @"</label></span>
<span class='tradus-select-user-edittext hideMobile'>
<a  onClick='vpb_show_sign_up_box1(this, " + item.UserAddressId + @");'>Edit</a>
</span>
<div class='clear'></div>

<div class='tradus-select-address'>
<span class='tradus-select-address-checkbox'>
");
                    if (k == 0)
                        strResult.Append(@"
<input class='ShippingAddress' type='checkbox' checked>");
                    else
                        strResult.Append(@"
<input class='ShippingAddress' type='checkbox'>");
                    k++;
                    strResult.Append(@"
</span>
<label class='tradus-select-address-label' for='ShippingAddress'>Select this as Shipping Address</label><br />
 <input type='hidden' id='hdnShippingAddId' value='" + item.UserAddressId + @"' />
</div>
<div class='clear'></div>
<div class='tradus-select-address'>
<span class='tradus-select-address-checkbox'>
<input class='BillingAddress' type='checkbox'>
</span>
<label class='tradus-select-address-label' for='BillingAddress'>Select this as Billing Address</label>

<div style='border-bottom: 1px solid #0084e1; height: 10px;'></div>
<input type='hidden' id='hdnBillingAddId' value='" + item.UserAddressId + @"' />
</div>
<div class='clear'></div>
</div>
</div>
");
                }


            }
            else
            {
                return new
                {
                    Status = Shared.ResponseStatus.NoData.ToString(),

                    Rows = totalRecords,
                };
            }
            return new
            {
                Rows = totalRecords,
                Status = string.IsNullOrWhiteSpace(strResult.ToString()) ? Shared.ResponseStatus.NoData.ToString() : Shared.ResponseStatus.Success.ToString(),
                Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
                Result = strResult.ToString()
            };
        }

        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
            throw;
        }
    }
    //  Post Address Method
    [HttpPost]
    public dynamic PostAddress(List<UserAddress> Address)
    {
        try
        {
            var uid = ((User)BalUtility.GetSession(Shared.Sessions.CustomerLogin)).UserId;
            var userCart = BalUtility.GetCart();

            var repository = new AddressRepository();
            var PincodeCheck = new PinCodeInfoRepository();

            UserAddress Addr = Address[0];
            UserAddress objBilling = null;
            double ShippingZipcode = Convert.ToDouble(Addr.PinCode);

            var Shippingpincode = PincodeCheck.First(u => u.PinCode == ShippingZipcode);

            bool canDelivar = true;
            if (Shippingpincode == null)
                canDelivar = false;

            if (Address.Count() == 2)
            {
                objBilling = Address[1];
                double BillingZipcode = Convert.ToDouble(objBilling.PinCode);
                var BillingPincode = PincodeCheck.First(u => u.PinCode == BillingZipcode);

                if (BillingPincode == null)
                    canDelivar = false;
            }

            if (canDelivar)
            {
                if (Addr.UserAddressId == 0)
                {
                    var shippingAddress = new UserAddress
                    {
                        StreetAddress1 = Addr.StreetAddress1,
                        StreetAddress2 = Addr.StreetAddress2,
                        LandMark = Addr.LandMark,
                        City = Addr.City,
                        CountryId = Addr.CountryId,
                        StateName = Addr.StateName,
                        StateId = Addr.StateId,
                        CreatedOn = System.DateTime.Now,
                        UpDatedOn = System.DateTime.Now,
                        PinCode = Addr.PinCode,
                        AddressTypeId = 2,
                        UserId = uid
                    };

                    var rst = repository.Insert(shippingAddress);

                    foreach (var userProductTransaction in userCart)
                    {
                        userProductTransaction.ShippingAddressId = shippingAddress.UserAddressId;
                    }
                }
                else
                {
                    foreach (var userProductTransaction in userCart)
                    {
                        userProductTransaction.ShippingAddressId = Convert.ToInt32(Addr.UserAddressId);
                    }
                }

                if (objBilling != null)
                {
                    if (objBilling.UserAddressId == 0)
                    {
                        var billingAddress = new UserAddress
                        {
                            StreetAddress1 = objBilling.StreetAddress1,
                            StreetAddress2 = objBilling.StreetAddress2,
                            LandMark = objBilling.LandMark,
                            City = objBilling.City,
                            CreatedOn = System.DateTime.Now,
                            UpDatedOn = System.DateTime.Now,
                            StateName = objBilling.StateName,
                            StateId = objBilling.StateId,
                            CountryId = objBilling.CountryId,
                            PinCode = objBilling.PinCode,
                            AddressTypeId = 1,
                            UserId = uid
                        };
                        var Result = repository.Insert(billingAddress);


                        foreach (var userProductTransaction in userCart)
                        {
                            userProductTransaction.BillingAddressId = billingAddress.UserAddressId;
                        }
                    }
                    else
                    {
                        foreach (var userProductTransaction in userCart)
                        {
                            userProductTransaction.BillingAddressId = Convert.ToInt32(objBilling.UserAddressId);
                        }
                    }
                }
                else//if billing address is not available , we are saving  shipping address  as the billing address
                {
                    foreach (var userProductTransaction in userCart)
                    {
                        userProductTransaction.BillingAddressId = userProductTransaction.ShippingAddressId;
                    }
                }


                foreach (var userProductTransaction in userCart)
                {
                    userProductTransaction.UserId = ((User)BalUtility.GetSession(Shared.Sessions.CustomerLogin)).UserId;
                    userProductTransaction.CreatedBy = ((User)BalUtility.GetSession(Shared.Sessions.CustomerLogin)).UserId;
                    userProductTransaction.UpdatedBy = ((User)BalUtility.GetSession(Shared.Sessions.CustomerLogin)).UserId;
                    userProductTransaction.CreatedOn = DateTime.Now;
                    userProductTransaction.UpdatedOn = DateTime.Now;
                    userProductTransaction.IsActive = true;
                    userProductTransaction.IsDeleted = false;
                }

                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = "",
                    Result = "Success"
                };

            }
            else
            {
                StringBuilder strResult = new StringBuilder();
                strResult.Append(@"<div>
<span class='tickIco' style='padding:0px 18px'>Product can be delivered at your location.</span>
</div>");
                return new CustomResponse
                {
                    Status = "No Pin Code",
                    Message = "Product can not be delivered at your location.",
                    Result = strResult.ToString()
                };
            }

        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
            throw;
        }
    }

    public CustomResponse EditUserAddressDetails(List<UserAddress> data)
    {
        try
        {
            //var uid = ((User)BalUtility.GetSession(Shared.Sessions.CustomerLogin)).UserId;
            //var userCart = BalUtility.GetCart();

            UserAddress objAddress = data[0];
            //UserAddress objBilling = data[1];

            var repository = new AddressRepository();
            var userDetails =
               repository.First(u => u.StateName == objAddress.StateName);
            if (userDetails == null)
            {

                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = "Invalid Zip/Postal Code",
                    Result = ""
                };
            }
            else
            {
                var uid = ((User)BalUtility.GetSession(Shared.Sessions.CustomerLogin)).UserId;

                objAddress.UserId = uid;
                objAddress.AddressTypeId = 1;
                objAddress.CreatedOn = System.DateTime.Now;
                objAddress.UpDatedOn = System.DateTime.Now;
                repository.Update(objAddress);
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = " Your Address Is Updated Successfully",
                    Result = ""
                };
            }
            //var objAddress = data[0];

            //var repository = new AddressRepository();

        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };

        }
    }
    //End Edit Address

    // GetOrderDetails

    public dynamic GetOrderDetails(int uId)
    {
        try
        {
            string sidx;
            string sord;
            int page = 0; int rows = 10;
            var repository = new PaymentTransactionRepository();
            int totalRecords;
            long UserId = Convert.ToInt64(uId);
            var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, (page - 1) * rows, rows, (p => p.UserId == UserId), null, "", true);


            var filteredData = (from q in (data as List<PaymentTransaction>)
                                select new
                                {
                                    q.User.FirstName,
                                    q.User.LastName,
                                    q.PaymentTransactionId,
                                    Quantity = q.UserProductTransactions.Count(),
                                    q.PaymentStatus,
                                    q.TxnAmount,
                                    q.TxnRefNo,
                                    q.TxnStatus,
                                    currency = q.CurrencyCode + " ( " + q.CurrencySymbol + " ) ",
                                    q.CurrencyCode,
                                    q.CurrencySymbol,
                                    q.PGTxnId,
                                    q.TxnMessage,
                                    q.CreatedOn,
                                    q.OrderCurrentStatus,
                                    q.Authorized,
                                    q.Pickup,
                                    q.PickupDate,
                                    q.Dispatched,
                                    q.DispatchedDate,
                                    q.Delivered,
                                    q.DeliveredDate,
                                    q.PaymentMode,
                                    products = q.UserProductTransactions.Where(cat => cat.PaymentTransactionId == q.PaymentTransactionId).
                                      Select(u => string.Join(",", (u.Product.ProductName + (u.SubProduct == null ? "" : " - " + u.SubProduct.SPName)))).ToList()

                                }).ToList();


            StringBuilder strtrResult = new StringBuilder();
            if (filteredData.Count > 0)
            {
                StringBuilder strtd = new StringBuilder();

                foreach (var item in filteredData)
                {

                    string statusvalue = item.OrderCurrentStatus.ToString();
                    var status = Shared.GetOrderStatusEnum(statusvalue);
                    strtd.Append(@"<tr>
				<td>
					   <span id='hypEdit' >" + item.PaymentTransactionId + @"</span>                        
												<span id='lblPartialShip'></span>
											</td>");
                    if (item.products.Count() > 1)
                    {
                        strtd.Append("<td >");
                        foreach (var pName in item.products)
                        {
                            strtd.Append("<span id='OrderDate'>" + pName + @"</span> , ");
                        }
                        strtd.Append("</td>");
                    }
                    else
                    {

                        strtd.Append(@"<td><span id='OrderDate'>" + item.products[0] + @"</span> </td>");
                    }

                    strtd.Append(@"<td><span id='OrderDate'>" + String.Format("{0:0.00}", item.TxnAmount) + @"</span> </td>");
                    strtd.Append(@"<td><span id='OrderMode'>" + item.PaymentMode + @"</span> </td>");

                    if (item.TxnStatus == null)
                    {
                        strtd.Append(@"<td>
												<span id='OrderDate' fore-color='Red'>Transaction-UnAuthorized</span> 
											</td>");
                    }
                    else
                    {
                        strtd.Append(@"<td>
												<span id='OrderDate'>" + item.TxnStatus + @"</span> 
											</td>");
                    }

                    strtd.Append(@"<td>                                                
												<a class='button' href='Order History.aspx?ID=" + item.PaymentTransactionId + @"'><span>View</span>
											</td>
			</tr>");
                }
                StringBuilder strResult = new StringBuilder();
                strResult.Append(@"
		<div class='widget'>                   
					
					<div class='widget_body'>
							<div id='Main_dvGrid' class='tab-grdpanel'>
								<div class='widget marg-0'> 
								<div class='widget_title' style='background:#d9387e;'>
							   <h5>&nbsp;&nbsp;&nbsp; Order Details</h5>
								</div>
								<div class='widget_body' style=''>
						
							<div class='cp_productlist_view'>
								<div class='products_views'>
									
								</div>
							</div>
						   
		<table class='activity_datatable' rules='all' id='ctl00_ctl00_Main_Main_grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
			<tbody>
<tr>

<th scope='col'>OrderId</th>
<th scope='col'>Products</th>
<th scope='col'>Amount</th>
<th scope='col'>Payment Mode</th>
<th scope='col'>Status</th>
<th scope='col'><div id='divCurrency'>Order Information </div></th>
			</tr>
" + strtd + @"
</tbody></table>	</div>

  <div class='cp_grdpager'>
					<div class='grdinfo'>
					  <label>Records per page</label>
					  <div id='ddlNoRow' class='selector'>
					<span>" + rows + @"</span>
<select style='opacity: 0;'  onchange='GetPaymentPendingOrder()' id='ddlNoRows' class='uniform_pager_list'>
		<option value='10' selected='selected'>5</option>
		<option value='5'>10</option>
		<option  value='10'>15</option>
		<option value='15'>20</option>
		<option value='20'>25</option>

	</select></div> 
		
						
					</div>
					  <div class='grd_pages'>
					  <label class='pages'>
			Records:
			<span id='ctl00_ctl00_Main_Main_usrGridPaging1_lblPageNo'>1</span>
			<span id='ctl00_ctl00_Main_Main_usrGridPaging1_lbl_of'>of</span>
			<span id='ctl00_ctl00_Main_Main_usrGridPaging1_lblPageCount'>1</span><span id='ctl00_ctl00_Main_Main_usrGridPaging1_lblPagestxt'> Pages</span></label>
			  
			 
<div id='dvHidden'>
	
	
	
</div>
					  </div>
					  <div class='clear'></div>
					</div>

	 </div>
						
						</div>
									</div>
							</div>
							
							
					</div>
				</div>
				<input name='ctl00$ctl00$Main$Main$hdnTodoSearchcriteria' id='ctl00_ctl00_Main_Main_hdnTodoSearchcriteria' value='[&quot;Order No&quot;,&quot;SKU&quot;,&quot;Email Id&quot;,&quot;Phone No&quot;,&quot;Coupon Code&quot;,&quot;Checkout Type&quot;]' type='hidden'>
				<img id='img441' alt='' onload='getdivCurrency();' src='Orders_files/designoption_009_13.jpg' height='1' width='1'>
			
</div>
		

<div class='widget_body'>
			<div style='display: block;' class='action_bar text_right' id='dvbtn'>

			   
				<div class='clear'>
				</div>
			</div>
		</div>");
                return new CustomResponse
                {
                    Status = string.IsNullOrWhiteSpace(strResult.ToString()) ? Shared.ResponseStatus.NoData.ToString() : Shared.ResponseStatus.Success.ToString(),
                    Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
                    Result = strResult.ToString()
                };
            }
            else
            {
                strtrResult.Append(@"No Transactions");
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.NoData.ToString(),
                    Message = "",
                    Result = strtrResult.ToString()
                };
            }

        }

        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
            throw;
        }
    }

    public string GetBearer()
    {
        //        var client = new RestClient("https://apiv2.shiprocket.in/v1/external/auth/login");
        //        client.Timeout = -1;
        //        var request = new RestRequest(Method.POST);
        //        request.AddHeader("Content-Type", "application/json");
        //        var body = @"{
        //" + "\n" +
        //        @"    ""email"": ""tejdeep.i@zoninnovative.com"",
        //" + "\n" +
        //        @"    ""password"": ""Huwsonal@123""
        //" + "\n" +
        //        @"}";
        //        request.AddParameter("application/json", body, ParameterType.RequestBody);
        //        IRestResponse response = client.Execute(request);
        ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

        // Ignore certificate errors (for testing purposes only)
        ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

        HttpClient client = new HttpClient();
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://apiv2.shiprocket.in/v1/external/auth/login");
        StringContent content = new StringContent("{\r\n\r\n    \"email\": \"tejdeep.i@zoninnovative.com\",\r\n     \"password\": \"Huwsonal@1234\"\r\n}", null, "application/json");
        request.Content = content;
        Task<HttpResponseMessage> responseTask = client.SendAsync(request);
        HttpResponseMessage response = responseTask.Result;

        response.EnsureSuccessStatusCode();
        Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        Shiprockettoken data = JsonConvert.DeserializeObject<Shiprockettoken>(response.Content.ReadAsStringAsync().Result);
        return data.token;
    }

    //Order history

    public CustomResponse GetorderHistory(int TransactionId, int UserId)
    {
        try
        {
            decimal orderSum = BalUtility.GetCart().Sum(p => p.ProductCost);
            Caluclations caluclations = BalUtility.GenerateOrderSummary();
            var repository = new PaymentTransactionRepository();
            int totalRecords;
            //PaymentTransaction data = repository.FetchAllByPage(p => p.PaymentTransactionId == TransactionId && p.UserId == UserId, out totalRecords, 0, 0,
            //                                          (p => p.PaymentTransactionId == TransactionId), null, "").FirstOrDefault();
            //PaymentTransaction data = repository.First(p => p.PaymentTransactionId == TransactionId && p.UserId == UserId, out totalRecords, 0, 0,
            //                                            (p => p.PaymentTransactionId == TransactionId), null, "");
            PaymentTransaction data = repository.First(p => p.PaymentTransactionId == TransactionId && p.UserId == UserId);
            StringBuilder strtrResult = new StringBuilder();
            if (data != null)
            {
                foreach (var item in data.UserProductTransactions)

                    if (item.Product.ProductCost != 0 && item.SubProduct != null)
                    {
                        strtrResult.Append(@"  <tr><td class='left' style='border-right:none;'>" + item.Product.SubCategory.SubCategoryName + @" </td><td class='cart_product' style='border-right:none;'>
		<a href=" + ApiUrl + "product/" + item.Product.ProductName + "/" + item.Product.ProductId + @"'><img onclick='NavigateToDetails(" + item.ProductId + ")' style='height:100px;' src='" + item.Product.ProductImgUrl.Replace("~/", ApiUrl) + "' alt='" + item.Product.ProductName + @"'></a>
	</td><td class='left' style='border-right:none;'>" + item.Product.ProductName + @"</td><td class='right'>" + item.Quantity + @"</td><td class='right'>" + item.PaymentTransaction.CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.SubProduct.ProductCost), true) + @"</td><td class='right'>" + item.PaymentTransaction.CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.ProductCost), true) + @"</td></tr>");
                    }
                    else if (item.Product.ProductCost != 0 || item.Product.ProductCost != null)
                    {
                        strtrResult.Append(@"  <tr><td class='left' style='border-right:none;'>" + item.Product.SubCategory.SubCategoryName + @" </td><td class='cart_product' style='border-right:none;'>
		<a href=" + ApiUrl + "product/" + item.Product.ProductName + "/" + item.Product.ProductId + @"'><img onclick='NavigateToDetails(" + item.ProductId + ")' style='height:100px;' src='" + item.Product.ProductImgUrl.Replace("~/", ApiUrl) + "' alt='" + item.Product.ProductName + @"'></a>
	</td><td class='left' style='border-right:none;'>" + item.Product.ProductName + @"</td><td class='right'>" + item.Quantity + @"</td><td class='right'>" + item.PaymentTransaction.CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.Product.ProductCost), true) + @"</td><td class='right'>" + item.PaymentTransaction.CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.ProductCost), true) + @"</td></tr>");

                    }

                db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();
                StringBuilder strOffer = new StringBuilder();

                if (data.Offer_Product_ID != null)
                {
                    Tbl_Offered_Products Offer = Entity.Tbl_Offered_Products.Where(x => x.Offer_Product_ID == data.Offer_Product_ID).First();

                    strOffer.Append(@"  <tr><td class='left' style='border-right:none;'></td><td class='cart_product' style='border-right:none;'>
		<img style='height:100px;' src='" + Offer.Product_Image + "' alt='" + Offer.Offer_Product_Name + @"'>
	</td><td class='left' style='border-right:none;'>" + Offer.Offer_Product_Name + @"</td><td class='right'>1</td><td class='right'>" + Offer.Product_Actual_Cost + @"</td><td class='right' style='font-weight:bold;'>FREE</td></tr>");

                }

                strtrResult.Append(strOffer);

                string statusvalue = data.OrderCurrentStatus.ToString();
                var status = Shared.GetOrderStatusEnum(statusvalue);

                StringBuilder strResult = new StringBuilder();
                StringBuilder strOrderResult = new StringBuilder();
                StringBuilder strOrderCancel = new StringBuilder();
                if (data.TxnStatus == "SUCCESS")
                {
                    strOrderCancel.Append(@"<a onclick='RequestforCancelOrder()' class='button'><span>Request for Cancel Order</span></a>");
                    strOrderResult.Append(@"<ul id='progressbar'>");
                    if (data.Authorized == true)
                    {
                        strOrderResult.Append(@"<li class='active' >
												 <a id='link1' title='Authorized by " + data.CreatedOn.ToShortDateString() + @"'>Authorized</a>
											  </li>");
                    }
                    if (data.Pickup == true)
                    {
                        strOrderResult.Append(@"<li class='active' title='Pickup by " + data.PickupDate + @"'>
												 <a id='link1' title='Pickup by " + data.PickupDate + @"'>Pickup</a>
											</li>");
                    }
                    else
                    {
                        strOrderResult.Append(@"<li>   <a id='link1' title='Pickup by " + data.PickupDate + @"'>Pickup</a>
													   </li>");
                    }
                    if (data.Dispatched == true)
                    {
                        strOrderResult.Append(@"<li class='active' title='Dispatched by " + data.DispatchedDate + @"'>
												 <a id='link1' title='Dispatched by " + data.DispatchedDate + @"'>Dispatched</a>
												 </li>");
                        strOrderCancel.Clear();
                    }
                    else
                    {
                        strOrderResult.Append(@"<li>  <a id='link1' title='Dispatched by " + data.DispatchedDate + @"'>Dispatched</a>
														  </li>");
                    }
                    if (data.Delivered == true)
                    {
                        strOrderResult.Append(@"<li class='active' title='Dispatched by " + data.DeliveredDate + @"'>
												 <a id='link1' title='Delivered by " + data.DeliveredDate + @"'>Delivered</a>
											</li>");
                        strOrderCancel.Clear();
                    }
                    else
                    {
                        strOrderResult.Append(@"<li > <a id='link1' title='Delivered by " + data.DeliveredDate + @"'>Delivered</a>
												   </li>");
                    }
                    if (status == Shared.OrderStatus.Refund)
                    {
                        strOrderCancel.Clear();
                    }
                    strOrderResult.Append(@"</ul>");
                }

                strResult.Append(@"<table class='list'>
	<thead>
	  <tr><td class='left' colspan='2'>Order Details</td></tr></thead>
	<tbody><tr><td class='left' style='width: 50%;'>        
		  <!--<b>Invoice No.:</b> INV-2011-059<br />--><b>Order ID:</b> " + data.PaymentTransactionId + @"<br />
		  <b>Order Date:</b> " + data.CreatedOn.ToShortDateString() + @"</td>
	   <td class='left'><!-- <b>Payment Method:</b>" + data.PaymentMode + @"<br />-->
		 <!--<b>Shipping Method:</b> Flat Rate--></td></tr></tbody></table>
		 <table class='list'>
	<thead><tr><td class='left'>Billing Address</td><td class='left'>Shipping Address</td></tr> </thead>
	<tbody> <tr><td class='left'>" + data.User.FirstName + "" + data.User.LastName + @"<br />" + data.UserProductTransactions.FirstOrDefault().UserAddress.StreetAddress1 + "," + data.UserProductTransactions.FirstOrDefault().UserAddress.StreetAddress2 + @"<br />" + data.UserProductTransactions.FirstOrDefault().UserAddress.LandMark + @"<br />" + data.UserProductTransactions.FirstOrDefault().UserAddress.City + @"<br />" + data.UserProductTransactions.FirstOrDefault().UserAddress.StateName + @"<br />" + data.UserProductTransactions.FirstOrDefault().UserAddress.PinCode + @"</td>
<td class='left'>" + data.User.FirstName + data.User.LastName + @"<br />" + data.UserProductTransactions.FirstOrDefault().UserAddress1.StreetAddress1 + "," + data.UserProductTransactions.FirstOrDefault().UserAddress1.StreetAddress2 + @"<br />" + data.UserProductTransactions.FirstOrDefault().UserAddress1.LandMark + @"<br />" + data.UserProductTransactions.FirstOrDefault().UserAddress1.City + @"<br />" + data.UserProductTransactions.FirstOrDefault().UserAddress1.StateName + @"<br />" + data.UserProductTransactions.FirstOrDefault().UserAddress1.PinCode + @"</td></tr></tbody></table>
	<form >
	<table class='list'>
	  <thead><tr><td class='left'>Model</td>
		  <td class='left'>Product</td>
		  <td class='left'>Product Name</td>
		  <td class='right'>Quantity</td>
		  <td class='right'>Unit Price</td>
		  <td class='right'>Total</td></tr></thead><tbody>
  <tr><td>" + strtrResult + @"</td></tr></tbody>
	  <!--<tfoot><tr><td colspan='3'></td><td class='right'><b>ServiceTax:</b></td><td class='right'>" + data.CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(caluclations.ServiceTax), true) + @"</td></tr>-->
<tr><td colspan='4'></td><td class='right'><b>Shipping Charges:</b></td><td class='right'>" + data.CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(data.ShippingCharges), true) + @"</td></tr>
<!--  <tr><td colspan='3'></td><td class='right'><b>OtherCharges:</b></td><td class='right'>" + data.CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(caluclations.OtherCharges), true) + @"</td></tr>-->
<!-- <tr><td colspan='3'></td><td class='right'><b>VAT:</b></td><td class='right'>" + data.CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(caluclations.VAT), true) + @"</td></tr>-->
  <tr><td colspan='4'></td><td class='right'><b>Total:</b></td><td class='right'>" + data.CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(data.TxnAmount), true) + @"<br/>(inclusive of all taxes.)</td></tr></tfoot></table>
  </form>
	  <h2>Order History</h2>
  <table class='list'><thead><tr><td class='left'>Order Date</td><td class='left'>Order Status</td><td class='left'>Tran. Status</td><td class='left'>Message</td></tr></thead>
	<tbody><tr><td class='left'>" + data.CreatedOn + @"</td><td class='left'>" + status + @"</td><td class='left'>" + data.TxnStatus + @"</td><td class='left'>" + data.TxnMessage + @"</td></tr>
</tbody></table>
<table class='list'><thead><tr><td class='left'>Order Process</td></tr></thead>
<tbody><tr><td class='left'><div class='divprocess'>" + strOrderResult + @"</div></td></tr>
</tbody></table>
	<div class='buttons'>
	<div class='right'>" + strOrderCancel + "<a href='javascript:;' onclick='Trackshipment(" + data.ShipmentId + ")' class='button'><span>Track Order</span></a><a href='" + ApiUrl + @"' class='button'><span>Continue</span></a></div>
  </div>");




                return new CustomResponse
                {
                    Status = string.IsNullOrWhiteSpace(strResult.ToString()) ? Shared.ResponseStatus.NoData.ToString() : Shared.ResponseStatus.Success.ToString(),
                    Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
                    Result = strResult.ToString()
                };
            }
            else
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Fail.ToString(),
                    Message = "No Data Found",
                    Result = null

                };
            }
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
            throw;
        }
    }

    public CustomResponse RequestforCancelOrder(int TransactionId, int UserId)
    {
        db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();

        User UserDetails = Entity.Users.Where(x => x.UserId == UserId).First();

        Utility.MailMessage ms = new Utility.MailMessage();
        ms.Subject = "Order Cancelling Request By User";
        ms.To = "customercare@healthurwealth.com";

        string domain;
        domain = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port;

        string body = string.Empty;
        string htmlPagePath = Shared.GethtmlPage(Shared.ProductStatusForSendMail.UserOrderCancel);
        using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("" + htmlPagePath + "")))
        {
            body = reader.ReadToEnd();
        }

        body = body.Replace("##FirstName##", UserDetails.FirstName);
        body = body.Replace("##EmailId##", UserDetails.EmailId);
        body = body.Replace("##MobileNo##", UserDetails.MobileNo.ToString());
        body = body.Replace("##OrderNo##", TransactionId.ToString());
        ms.Body = body;

        ms.IsBodyHtml = true;

        try
        {
            ms.SendMail();
        }
        catch (Exception ex)
        {

        }

        string Message = "Dear Admin, Mr/Mrs. " + UserDetails.FirstName + ", is Requested to cancel Order (OrderID: " + TransactionId + "). Contact User (" + UserDetails.MobileNo + ") for further details.";
        string Url = WebConfigurationManager.AppSettings["SmsUrl"].ToString();
        string UserName = WebConfigurationManager.AppSettings["SmsId"].ToString();
        string password = WebConfigurationManager.AppSettings["SmsPwd"].ToString();
        decimal AdminMobileNumber = Convert.ToDecimal(WebConfigurationManager.AppSettings["AdminMobileNumber"].ToString());
        string Status = Utility.MailMessage.SendSms(Url, UserName, password, AdminMobileNumber, Message, "N", "");

        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Message = "Request Sent."
        };
    }





    //UC_Login page


    public dynamic UserLogin(User objUser)
    {
        try
        {
            var repository = new UserRepository();
            var userDetails =
                repository.First(u => u.EmailId == objUser.EmailId && u.PassCode == objUser.PassCode && u.RoleId == (int)Shared.UserRoles.Customer);
            if (objUser.EmailId != "" && objUser.PassCode != "")
            {
                if (userDetails == null)
                {
                    string msg = "User does not exist";
                    StringBuilder strmsg = new StringBuilder();
                    strmsg.Append(@"<div class='warning'>" + msg + "</div>");
                    return new CustomResponse
                    {
                        Status = Shared.ResponseStatus.NoData.ToString(),
                        Message = "The email or password you entered is incorrect.",
                        Result = strmsg.ToString(),

                    };
                }
                if (userDetails.IsDeleted == true)
                {
                    return new CustomResponse
                    {
                        Status = Shared.ResponseStatus.NoData.ToString(),
                        Message = "User does not exist",
                        Result = null
                    };
                }
                else
                {
                    BalUtility.CreateSession(userDetails, Shared.Sessions.CustomerLogin);
                    HttpContext.Current.Session["FirstName"] = userDetails.FirstName;
                    HttpContext.Current.Session["LoginName"] = objUser.EmailId;
                    HttpContext.Current.Session["Password"] = objUser.PassCode;
                    return new CustomResponse
                    {
                        Status = Shared.ResponseStatus.Success.ToString(),
                        Message = "",
                        Result = userDetails,
                        PageRedirect = Shared.PageRedirections(objUser.Redirectvalue)
                    };
                }
            }
            else
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Fail.ToString(),
                    Message = CustomMessages.emptyText,
                    Result = userDetails
                };


            }

        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
            throw;
        }
    }

    public dynamic AdminLogin(User objUser)
    {
        try
        {
            var repository = new UserRepository();
            var userDetails =
                repository.First(u => u.IsDeleted == false && u.EmailId == objUser.EmailId && u.PassCode == objUser.PassCode && u.RoleId == (int)Shared.UserRoles.Admin);
            if (userDetails == null)
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.NoData.ToString(),
                    Message = "User does not exist",
                    Result = null
                };
            }
            if (userDetails.IsDeleted == true)
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.NoData.ToString(),
                    Message = "User does not exist",
                    Result = null
                };
            }
            else
            {
                BalUtility.CreateSession(userDetails, Shared.Sessions.AdminLogin);
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = "",
                    Result = userDetails
                };
            }
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
            throw;
        }
    }




    public dynamic SmartRegistration(User objUser)
    {
        try
        {
            UserRepository repository = new UserRepository();
            User userDetails = repository.First(u => u.EmailId == objUser.EmailId);
            objUser.CreatedOn = System.DateTime.Now;
            objUser.UpDatedOn = System.DateTime.Now;
            if (userDetails != null)
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Fail.ToString(),
                    Message = CustomMessages.UserExistsWithEmail,
                    Result = null
                };
            }
            else
            {
                objUser.MiddleName = "";
                objUser.IsActive = true;
                objUser.RoleId = 2;
                userDetails = repository.Insert(objUser);
                BalUtility.CreateSession(userDetails, Shared.Sessions.CustomerLogin);
                HttpContext.Current.Session["FirstName"] = objUser.FirstName;

                string Message = "Thank you for registering with Healthurwealth.com We would keep you updated with our best deals and offer periodically.Healthurwealth.com(SONAL ENTERPRISES)";
                string Url = WebConfigurationManager.AppSettings["SmsUrl"].ToString();
                string UserName = WebConfigurationManager.AppSettings["SmsId"].ToString();
                string password = WebConfigurationManager.AppSettings["SmsPwd"].ToString();
                string Status = Utility.MailMessage.SendSms(Url, UserName, password, objUser.MobileNo, Message, "N", "1707160960597051906");

                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = CustomMessages.sucReg,
                    Result = userDetails
                };
            }
        }

        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
            throw;
        }
    }



    public dynamic Insertpin(PinCodesInformation objUser)
    {
        try
        {

            objUser.LocationCode = "";
            objUser.DefaultCity = "";
            objUser.Zone = "";
            objUser.Region = "";
            objUser.EXP = "Y";
            objUser.CGO = "Y";
            objUser.SFC = "Y";
            objUser.ToPay = "Y";
            objUser.StationType = "";
            PinCodesInformationRepository repository = new PinCodesInformationRepository();
            PinCodesInformation userDetails = repository.Insert(objUser);

            if (userDetails != null)
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Fail.ToString(),
                    Message = CustomMessages.pincode,
                    Result = null

                };
            }
            else
            {


                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = CustomMessages.pininsertionfailed,
                    Result = userDetails
                };
            }
        }

        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
            throw;
        }
    }

    //Insertofflinesales added by gopi

    public dynamic Insertofflinesales(OfflineSale objUser)
    {
        try
        {

            OfflineSalesRepository repository = new OfflineSalesRepository();
            OfflineSale userDetails = repository.Insert(objUser);

            if (userDetails != null)
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Fail.ToString(),
                    Message = CustomMessages.offline,
                    Result = null

                };
            }
            else
            {


                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = CustomMessages.pininsertionfailed,
                    Result = userDetails
                };
            }
        }

        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
            throw;
        }
    }
    // End Insertofflinesales



    public dynamic NewUserRegistration(User objUser)
    {
        try
        {

            if (objUser.FirstName != "" && objUser.LastName != "" && objUser.EmailId != "" && objUser.MobileNo != null && objUser.MobileNo != 0)
            {
                UserRepository repository = new UserRepository();
                User userDetails = repository.First(u => u.EmailId == objUser.EmailId);
                objUser.CreatedOn = System.DateTime.Now;
                objUser.UpDatedOn = System.DateTime.Now;
                if (userDetails != null)
                {
                    objUser.FirstName = "";
                    return new CustomResponse
                    {
                        Status = Shared.ResponseStatus.Fail.ToString(),
                        Message = CustomMessages.UserExistsWithEmail,
                        Result = null,
                    };
                }
                else
                {
                    objUser.MiddleName = "";
                    objUser.RoleId = 2;
                    objUser.IsActive = true;
                    userDetails = repository.Insert(objUser);

                    string Message = "Thank you for registering with Healthurwealth.com We would keep you updated with our best deals and offer periodically.Healthurwealth.com(SONAL ENTERPRISES)";
                    string Url = WebConfigurationManager.AppSettings["SmsUrl"].ToString();
                    string UserName = WebConfigurationManager.AppSettings["SmsId"].ToString();
                    string password = WebConfigurationManager.AppSettings["SmsPwd"].ToString();
                    string Status = Utility.MailMessage.SendSms(Url, UserName, password, objUser.MobileNo, Message, "N", "1707160960597051906");

                    return new CustomResponse
                    {
                        Status = Shared.ResponseStatus.Success.ToString(),
                        Message = CustomMessages.sucReg,
                        Result = userDetails
                    };
                }
            }

            else
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Fail.ToString(),
                    Message = CustomMessages.emptyText,
                    Result = null
                };
            }
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
            throw;
        }
    }

    //UC_Registration

    public dynamic Uc_UserRegistration(User objUser)
    {
        try
        {
            var repository = new UserRepository();
            objUser.MiddleName = "";
            User userDetails = repository.Insert(objUser);



            BalUtility.CreateSession(userDetails, Shared.Sessions.CustomerLogin);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Message = "",
                Result = userDetails
            };
        }

        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
            throw;
        }
    }

    public dynamic GetproductStatus(int ProductId)
    {
        var repository = new ProductRepository();

        Product status = repository.Single(p => p.ProductId == ProductId);
        return status;
    }

    public bool PropertyExist(object obj, string propertyName)
    {
        return obj.GetType().GetProperty(propertyName) != null;
    }
    public CustomResponse GetMedicineInvoice(int TransactionId)
    {
        try
        {
            db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();
            var repository = new PaymentTransactionRepository();
            int totalRecords;
            PaymentTransaction data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, 0,
                                                      (p => p.PaymentTransactionId == TransactionId), null, "").FirstOrDefault();
            string body = string.Empty;

            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/Mail_Pages/invoice.html")))
            {
                body = reader.ReadToEnd();
            }

            StringBuilder strResult = new StringBuilder();
            StringBuilder shippinggst = new StringBuilder();

            int count = 1;
            int totalQuantity = 0;
            if (data.UserProductTransactions.ElementAtOrDefault(0).UserAddress.StateName != "Telangana")
            {
                if (data.UserProductTransactions.Max(x => x.Product.GST) > 18)
                    shippinggst.Append("<td class='prices'>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(((((data.ShippingCharges) * (100 / (100 + 18))))))) + @"</td><td>IGST</td><td>" + 18 + @"%</td><td>" + BalUtility.FormatedDoubleWithOutPrecision((Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + 18))))))))) + @"</td><td hidden class='gstprices'>" + BalUtility.FormatedDoubleWithOutPrecision((Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + 18))))))))) + @"</td>");

                else
                    shippinggst.Append("<td class='prices'>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(((((data.ShippingCharges) * (100 / (100 + data.UserProductTransactions.Max(x => x.Product.GST)))))))) + @"</td><td>IGST</td><td>" + data.UserProductTransactions.Max(x => x.Product.GST) + @"%</td><td>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + data.UserProductTransactions.Max(x => x.Product.GST))))))))) + @"</td><td hidden class='gstprices'>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + data.UserProductTransactions.Max(x => x.Product.GST))))))))) + @"</td>");
            }
            else
            {
                if (data.UserProductTransactions.Max(x => x.Product.GST) > 18)
                    shippinggst.Append("<td class='prices'>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(((((data.ShippingCharges) * (100 / (100 + 18))))))) + @"</td><td>CGST</br>SGST</td><td>" + 18 / 2 + @"%</br>" + 18 / 2 + @"%</td><td> " + (Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + 18)))))))) / 2 + "<br>" + (Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + 18)))))))) / 2 + @"</td><td hidden class='gstprices'>" + (Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + 18)))))))) + @"</td>");

                else
                    shippinggst.Append("<td class='prices'>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(((((data.ShippingCharges) * (100 / (100 + data.UserProductTransactions.Max(x => x.Product.GST)))))))) + @"</td><td>CGST</br>SGST</td><td>" + data.UserProductTransactions.Max(x => x.Product.GST) / 2 + @"%</br>" + data.UserProductTransactions.Max(x => x.Product.GST) / 2 + @"%</td><td> " + BalUtility.FormatedDoubleWithOutPrecision((Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + data.UserProductTransactions.Max(x => x.Product.GST))))))))) / 2) + "<br>" + BalUtility.FormatedDoubleWithOutPrecision((Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + data.UserProductTransactions.Max(x => x.Product.GST))))))))) / 2) + @"</td><td hidden class='gstprices'>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + data.UserProductTransactions.Max(x => x.Product.GST))))))))) + @"</td>");


            }
            foreach (var item in data.UserProductTransactions)
            {
                string ProductName = "";
                decimal ProductCost = 0;
                if (item.SubProductId == null)
                {
                    ProductName = item.ProductName;
                    if (item.ProductName == null)
                        ProductName = item.Product.ProductName;

                    ProductCost = item.Product.ProductCost;
                }
                else
                {
                    SubProduct Sp = Entity.SubProducts.Where(x => x.SubProductId == item.SubProductId).First();
                    ProductName = item.ProductName + "(" + Sp.SPName + ")";
                    ProductCost = Sp.ProductCost;
                }


                if (data.UserProductTransactions.ElementAtOrDefault(0).UserAddress.StateName != "Telangana")
                {
                    strResult.Append("<tr><td>" + count + "</td><td>" + ProductName + "(" + item.Product.Brand + ")</td><td><input type='text' class='no-outline' /></td><td><input type='text' class='no-outline'/></td><td><input type='text' class='no-outline'/></td><td><input type='text' class='no-outline'/></td><td><input type='text' class='no-outline'/></td><td>" + item.Quantity + @"</td>
<td class='prices'> " + BalUtility.FormatedDoubleWithOutPrecision((Convert.ToDouble(item.ProductCost / item.Quantity)) - Convert.ToDouble((((item.ProductCost / item.Quantity) - (((item.ProductCost / item.Quantity) * (100 / (100 + item.Product.GST))))))), true) + @"</td> 
<td>IGST</td><td>" + item.Product.GST + "%</td><td>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(((item.ProductCost) - (((item.ProductCost) * (100 / (100 + item.Product.GST))))))) + @"</td>
<td class='gstprices' hidden>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(((item.ProductCost) - (((item.ProductCost) * (100 / (100 + item.Product.GST))))))).Replace(",", "") + @"</td>
<td align='right'>" + item.ProductCost + @"</td>
 </tr>");
                }
                else
                {
                    strResult.Append("<tr><td>" + count + "</td><td>" + ProductName + "(" + item.Product.Brand + ")</td><td><input type='text' class='no-outline'/></td><td><input type='text' class='no-outline'/></td><td><input type='text' class='no-outline'/></td><td><input type='text' class='no-outline'/></td><td><input type='text' class='no-outline'/></td><td>" + item.Quantity + @"</td>
<td class='prices'> " + BalUtility.FormatedDoubleWithOutPrecision((Convert.ToDouble(item.ProductCost / item.Quantity)) - Convert.ToDouble((((item.ProductCost / item.Quantity) - (((item.ProductCost / item.Quantity) * (100 / (100 + item.Product.GST))))))), true) + @"</td> 
<td>CGST<br/>SGST</td><td>" + item.Product.GST / 2 + "%<br/>" + item.Product.GST / 2 + "%</td><td> " + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(((item.ProductCost) - (((item.ProductCost) * (100 / (100 + item.Product.GST))))) / 2)) + "<br/>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(((item.ProductCost) - (((item.ProductCost) * (100 / (100 + item.Product.GST))))) / 2)) + @"</td>
<td  class='gstprices' hidden>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(((item.ProductCost) - (((item.ProductCost) * (100 / (100 + item.Product.GST))))))).Replace(",", "") + @"</td>
<td align='right'>" + item.ProductCost + @"</td>

 </tr>");
                }
                count++;
                totalQuantity = totalQuantity + item.Quantity;
            }


            //<div>
            //           Order No: <span style='float:right'>" + data.PaymentTransactionId + @"</span>
            //       </div>
            body = body.Replace("##Productdetails##", strResult.ToString());
            body = body.Replace("##Shippingdetails##", shippinggst.ToString());
            body = body.Replace("##lblOrderid##", TransactionId.ToString());
            body = body.Replace("##Amount##", Convert.ToDecimal(data.TxnAmount).ToString("N2").ToString());
            body = body.Replace("##lblDate##", data.CreatedOn.ToString("dd/MMM/yyyy"));
            body = body.Replace("##lblUsername##", data.User.FirstName);
            body = body.Replace("##lblLocation##", data.UserProductTransactions.FirstOrDefault().UserAddress.StreetAddress1.ToString());
            body = body.Replace("##lblstate##", data.UserProductTransactions.FirstOrDefault().UserAddress.StateName.ToString());
            body = body.Replace("##lblMobile##", data.User.MobileNo.ToString());
            body = body.Replace("##lblAddress##", data.UserProductTransactions.FirstOrDefault().UserAddress.StreetAddress2.ToString());
            body = body.Replace("##lblTotalvalue##", Convert.ToDecimal(data.TxnAmount).ToString("N2").ToString());
            body = body.Replace("##lbltotalquantity##", totalQuantity.ToString());
            body = body.Replace("##Shippingcharges##", data.ShippingCharges.ToString());
            body = body.Replace("##lblPaymentmode##", data.PaymentMode);
            if (data.DoctorName != null)
            {
                body = body.Replace("##lblDoctorName##", data.DoctorName);
            }
            else
            {
                body = body.Replace("##lblDoctorName##", "<input type='text'");

            }







            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = body.ToString()
            };

        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
            throw;
        }
    }

    //  Invoice page created by ZIIS6016
    public CustomResponse GetInvoice(int TransactionId)
    {
        try
        {
            db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();
            var repository = new PaymentTransactionRepository();
            int totalRecords;
            PaymentTransaction data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, 0,
                                                      (p => p.PaymentTransactionId == TransactionId), null, "").FirstOrDefault();


            StringBuilder strResult = new StringBuilder();
            StringBuilder shippinggst = new StringBuilder();

            int count = 1;
            int totalQuantity = 0;
            if (data.UserProductTransactions.ElementAtOrDefault(0).UserAddress.StateName != "Telangana")
            {
                if (data.UserProductTransactions.Max(x => x.Product.GST) > 18)
                    shippinggst.Append("<td class='prices'>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(((((data.ShippingCharges) * (100 / (100 + 18))))))) + @"</td><td>IGST</td><td>" + 18 + @"%</td><td>" + BalUtility.FormatedDoubleWithOutPrecision((Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + 18))))))))) + @"</td><td hidden class='gstprices'>" + BalUtility.FormatedDoubleWithOutPrecision((Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + 18))))))))) + @"</td>");
                else
                    shippinggst.Append("<td class='prices'>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(((((data.ShippingCharges) * (100 / (100 + data.UserProductTransactions.Max(x => x.Product.GST)))))))) + @"</td><td>IGST</td><td>" + data.UserProductTransactions.Max(x => x.Product.GST) + @"%</td><td>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + data.UserProductTransactions.Max(x => x.Product.GST))))))))) + @"</td><td hidden class='gstprices'>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + data.UserProductTransactions.Max(x => x.Product.GST))))))))) + @"</td>");
            }
            else
            {
                if (data.UserProductTransactions.Max(x => x.Product.GST) > 18)
                    shippinggst.Append("<td class='prices'>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(((((data.ShippingCharges) * (100 / (100 + 18))))))) + @"</td><td>CGST</br>SGST</td><td>" + 18 / 2 + "</br>" + 18 / 2 + @"%</td><td> " + BalUtility.FormatedDoubleWithOutPrecision((Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + 18)))))))) / 2) + "<br>" + BalUtility.FormatedDoubleWithOutPrecision((Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + 18)))))))) / 2) + @"</td><td hidden class='gstprices'>" + BalUtility.FormatedDoubleWithOutPrecision((Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + 18))))))))) + @"</td>");

                else
                    shippinggst.Append("<td class='prices'>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(((((data.ShippingCharges) * (100 / (100 + data.UserProductTransactions.Max(x => x.Product.GST)))))))) + @"</td><td>CGST</br>SGST</td><td>" + data.UserProductTransactions.Max(x => x.Product.GST) / 2 + @"%</br> " + data.UserProductTransactions.Max(x => x.Product.GST) / 2 + @" % </td><td>" + BalUtility.FormatedDoubleWithOutPrecision((Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + data.UserProductTransactions.Max(x => x.Product.GST))))))))) / 2) + "<br>" + BalUtility.FormatedDoubleWithOutPrecision((Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + data.UserProductTransactions.Max(x => x.Product.GST))))))))) / 2) + @"</td><td hidden class='gstprices'>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + data.UserProductTransactions.Max(x => x.Product.GST))))))))) + @"</td>");
            }
            foreach (var item in data.UserProductTransactions)
            {
                Tbl_AdditionalInfo addinfo = Entity.Tbl_AdditionalInfo.Where(x => x.ProductId == item.ProductId).FirstOrDefault();
                string expdate = "";
                string batchno = "";

                if (addinfo != null)
                {
                    expdate = addinfo.Manufacturer_Date.Value.AddMonths(addinfo.Best_Before_Date.Value).ToString("dd/MM/yyyy");
                    if (addinfo.BatchNo != null)
                        batchno = addinfo.BatchNo;
                    else
                        batchno = "N/A";

                }
                string ProductName = "";
                decimal ProductCost = 0;
                if (item.SubProductId == null)
                {
                    ProductName = item.ProductName;
                    if (item.ProductName == null)
                        ProductName = item.Product.ProductName;

                    ProductCost = item.Product.ProductCost;
                }
                else
                {
                    SubProduct Sp = Entity.SubProducts.Where(x => x.SubProductId == item.SubProductId).First();
                    ProductName = item.ProductName + "(" + Sp.SPName + ")";
                    ProductCost = Sp.ProductCost;
                }


                if (data.UserProductTransactions.ElementAtOrDefault(0).UserAddress.StateName != "Telangana")
                {
                    strResult.Append("<tr><td>" + count + "</td><td>" + ProductName + "(Exp:" + expdate + @")

                        (" + batchno + @")</td><td>" + item.Product.Brand + "</td><td>" + item.Quantity + @"</td>
<td class='prices'> " + BalUtility.FormatedDoubleWithOutPrecision((Convert.ToDouble(item.ProductCost / item.Quantity)) - Convert.ToDouble((((item.ProductCost / item.Quantity) - (((item.ProductCost / item.Quantity) * (100 / (100 + item.Product.GST))))))), true) + @"</td> 
<td>IGST</td><td>" + item.Product.GST + "%</td><td>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(((item.ProductCost) - (((item.ProductCost) * (100 / (100 + item.Product.GST))))))) + @"</td>
<td class='gstprices' hidden>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(((item.ProductCost) - (((item.ProductCost) * (100 / (100 + item.Product.GST))))))).Replace(",", "") + @"</td>
<td align='right'>" + item.ProductCost + @"</td>
 </tr>");
                }
                else
                {
                    strResult.Append("<tr><td>" + count + "</td><td>" + ProductName + "(Exp:" + expdate + @")

(" + batchno + ")</td><td>" + item.Product.Brand + "</td><td>" + item.Quantity + @"</td>
<td class='prices'> " + BalUtility.FormatedDoubleWithOutPrecision((Convert.ToDouble(item.ProductCost / item.Quantity)) - Convert.ToDouble((((item.ProductCost / item.Quantity) - (((item.ProductCost / item.Quantity) * (100 / (100 + item.Product.GST))))))), true) + @"</td> 
<td>CGST</br>SGST</td><td>" + item.Product.GST / 2 + "%</br>" + item.Product.GST / 2 + "%</td><td>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(((item.ProductCost) - (((item.ProductCost) * (100 / (100 + item.Product.GST))))) / 2)) + "<br/>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(((item.ProductCost) - (((item.ProductCost) * (100 / (100 + item.Product.GST))))) / 2)) + @"</td>
<td  class='gstprices' hidden>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(((item.ProductCost) - (((item.ProductCost) * (100 / (100 + item.Product.GST))))))).Replace(",", "") + @"</td>
<td align='right'>" + item.ProductCost + @"</td>

 </tr>");
                }
                count++;
                totalQuantity = totalQuantity + item.Quantity;
            }

            if (data.Offer_Product_ID != null)
            {
                Tbl_Offered_Products Offer = Entity.Tbl_Offered_Products.Where(x => x.Offer_Product_ID == data.Offer_Product_ID).First();

                strResult.Append("<tr><td>" + count + "</td><td>" + Offer.Offer_Product_Name + "</td><td>1</td><td>" + Offer.Product_Brand + @"</td>
<td> " + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(Offer.Product_Actual_Cost), true) + @"</td>
<td style='font-weight:bold;'> FREE</td> 
 </tr>");
            }

            if (data.Free_Product_ID != null)
            {
                Product freeProduct = Entity.Products.Where(x => x.ProductId == data.Free_Product_ID).First();

                strResult.Append("<tr><td>" + count + "</td><td>" + freeProduct.ProductName + "</td><td>1</td><td>" + freeProduct.Brand + @"</td>
<td> " + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(freeProduct.ProductCost), true) + @"</td>
<td style='font-weight:bold;'> FREE</td> 
 </tr>");
            }


            StringBuilder strInvoice = new StringBuilder();

            string ShippingCharges = "";
            string Total = "";

            if (data.UserProductTransactions.ElementAtOrDefault(0).Product.ProductCost >= 1000)
            {
                ShippingCharges = "0.00";
                Total = (Convert.ToDouble(data.UserProductTransactions.ElementAtOrDefault(0).Product.ProductCost)) + (Convert.ToDouble(ShippingCharges)).ToString();
            }
            else
            {
                ShippingCharges = "50.00";
                Total = (Convert.ToDouble(data.UserProductTransactions.ElementAtOrDefault(0).Product.ProductCost)) + (Convert.ToDouble(ShippingCharges)).ToString();
            }

            StringBuilder strPromocode = new StringBuilder();

            if (data.Has_Promo_Code == true)
            {
                Tbl_Coupon_Info PromocodeInfo = Entity.Tbl_Coupon_Info.Where(x => x.Coupon_Id == data.Promo_Code_ID).First();

                strPromocode.Append(@"<tr>
				<td colspan='3'>
					Discount
				</td>
				<td>
					
				</td>
				<td colspan='3' align='right'>
					
<strong>" + Convert.ToDecimal(data.Promo_Code_Amount).ToString("N2") + @"</strong>
				</td>
			</tr>");
            }

            StringBuilder strCOD = new StringBuilder();
            string COD = "Prepaid";
            string PrepaidAmounttext = "<span style='font-size:16px;margin-right:10px;float:right;'>&#8377; " + Convert.ToDecimal(data.TxnAmount).ToString("N2") + @"/-</span></div>";
            if (data.PaymentMode == "Cash On Delivery")
            {
                PrepaidAmounttext = "<span style='font-size:16px;margin-right:10px;float:right;'>&#8377; " + Convert.ToDecimal(data.TxnAmount).ToString("N2") + @"/-</span></div>";
                COD = "COD";
            }
            StringBuilder strdc = new StringBuilder();
            if (data.DoctorName != null)
            {
                strdc.Append(@"</br>
						<strong style='float:left'>Doctor Name/Hospital Name:" + data.DoctorName + @"</strong>
					</div>");
            }
            strInvoice.Append(@"<div style='margin:0 auto;width:595px;border-color:#999;'>
		<table width='100%' border='1' style='border-collapse:collapse;font-size:10px;border-color:#999;border-radius:5px;line-height:13px;font-family:Lucida Grande,Lucida Sans Unicode,Lucida Sans,DejaVu Sans,Verdana,sans-serif;' cellpadding='3' cellspacing='5'>
			<tr>
				<td colspan='10'>
					<div style='width:50%;float:left;padding-bottom:15px;'>
					<img src='https://www.healthurwealth.com/assets/images/logo-Black.png' width='50%' style='margin-left:5%;margin-top:5px;'>
 
 <div style='margin-top:20px;'>INVOICE <strong>#HUW00000" + data.PaymentTransactionId + @"</strong></div>
<div style='padding-top:8px;'>INVOICE Date: " + data.CreatedOn.ToShortDateString() + @"</div>
</div>
  
				   <div style='width:50%;float:left;line-height:32px;text-align:right;'>
<span style='background: #020000;padding: 3px 17px;margin-right: 5px;color: #fff;font-weight: 900;margin-top: 10px;font-size: 14px;display: inline-block;'>" + COD + @"</span>					
				   </div>
 <div style='font-size: 16px;float: right;margin-top: 24px;display: inline-block;'><small>Amount:</small> <span style='font-size:16px;float:right;font-weight: bold;'>" + PrepaidAmounttext + @"</span></div></div>
				</td>
			</tr>
			<tr>
				<td width='50%' colspan='3'>
					Name &amp; Shipping Address
				</td>
				<td width='50%' colspan='7'>
					<!--Billing Address--> Order Details:
				</td>
			</tr>
			<tr>
				<td colspan='3'>
					<strong>" + data.User.FirstName + " " + data.User.LastName + @"</strong>
					<div>
						" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress.StreetAddress1 + " ,</br>" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress.StreetAddress2 + " ,</br>" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress.LandMark + @"
					</div>
					<div>
						" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress.City + @" <span style='float:right'>" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress.PinCode + @"</span>
					</div>
					<div>
						" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress.StateName + @"
					</div>
					<div>
						<strong style='float:left'>" + data.User.MobileNo + @"</strong>
					</div>
" + strdc + @"
<input type='text' value='" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress.StateName + @"'  id='Statename' hidden/>
				</td>
				<td valign='top' colspan='7' style='line-height:20px;'>
					<!-- <div>
						" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress1.StreetAddress1 + " ,</br>" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress1.StreetAddress2 + " ,</br>" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress1.LandMark + @"
					</div>
					<div>
						" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress1.City + @" <span style='float:right'>" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress1.PinCode + @"</span>
					</div>
					<div >
						" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress1.StateName + @"
					</div> -->
				   
					<div>
						Payment Mode : <span style='float:right'><strong>" + data.PaymentMode + @"</strong></span>
					</div>                   
					<div>
						GST No : <span style='float:right'>36AAUPK4988K1ZO</span>
					</div>
				</td>
			</tr>
			<tr align='center'>
				<td>
					S.No
				</td>
				<td>
					Product
				</td>  
<td>
					Brand
				</td>
				<td>
					Qty
				</td>
				 
				<td>
					Price
				</td> 
<td><span  id='CGST'>
					 GST type
</span>
			<td>
					Tax Rate
				</td><td>
					Tax Amount
				</td>
				<td>
					Amount
				</td>
			</tr>
			" + strResult + @"
			<tr>
				<td colspan='3' style='border:none'>
					Shipping Charges
				</td>
<td>
				</td>
" + shippinggst + @"
				<td colspan='4' align='right' style='border:none'>
				" + Convert.ToDecimal(data.ShippingCharges).ToString("N2") + @"
				</td>
			</tr>     
			" + strPromocode + @"
			<tr>
				<td colspan='3' >
					Total
				</td>
				<td>
					" + totalQuantity + @"
				</td>
<td id='producttotal'>
					
				</td>
<td></td>
<td></td>

     <td id='gsttotal'>
					
				</td>
				<td colspan='3' align='right'>
					<strong>" + Convert.ToDecimal(data.TxnAmount).ToString("N2") + @"</strong>
				</td>
			</tr>
			" + strCOD + @"
			<tr>
				<td colspan='10'>
					Prices are inclusive of all applicable taxes
				</td>
			</tr>
			<tr>
				<td colspan='10'>
					<u>If Undelivered, please return to :</u><br>
<strong>SONAL ENTERPRISES,6-3-850/1, Ground floor, Shop no 4, Sirisha plaza,<br> main road, Dharam Karan Rd, Ameerpet,<br> Hyderabad, Telangana 500016</strong>
				</td>
			</tr>
		</table>
	<div >This is a computer generated invoice. No signature required. <br />
** Conditions Apply. Please refer to the product page for more details</div></div>
");

            //<div>
            //           Order No: <span style='float:right'>" + data.PaymentTransactionId + @"</span>
            //       </div>

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strInvoice.ToString()
            };

        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
            throw;
        }
    }

    public bool CheckSession(string sessionType)
    {
        try
        {
            var session = (Shared.Sessions)Enum.Parse(typeof(Shared.Sessions), sessionType);
            return BalUtility.CheckSession(session);
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            throw;
        }
    }

    public bool CreateSession(string sessionType, User sessionObject)
    {
        try
        {
            var session = (Shared.Sessions)Enum.Parse(typeof(Shared.Sessions), sessionType);
            BalUtility.CreateSession(sessionObject, session);
            return true;
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            throw;
        }
    }
    public dynamic GetSession(string sessionType)
    {
        try
        {
            var session = (Shared.Sessions)Enum.Parse(typeof(Shared.Sessions), sessionType);
            return BalUtility.GetSession(session);

        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            throw;
        }
    }

    public dynamic GetUserCartAddresses()
    {
        try
        {
            if (BalUtility.GetCart().Count > 0)
            {
                var cartShippingAddressId = BalUtility.GetCart()[0].ShippingAddressId;
                var cartBillingAddressId = BalUtility.GetCart()[0].BillingAddressId;

                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = "",
                    Result = new { ShippingAddressId = cartShippingAddressId, BillingAddressId = cartBillingAddressId }
                };
            }
            else
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.NoData.ToString(),
                    Message = "No Data Found",
                    Result = null,
                };
            }
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null,
            };
            throw;
        }

    }

    //public dynamic GetUserSession()
    //{
    //    try
    //    {
    //        long UserId = 0;

    //        var firstName = "";
    //        var lastName = "";
    //        var email = "";
    //        var phoneNumber = "";
    //        var addressStreet1 = "";
    //        var addressCity = "";
    //        var addressState = "";
    //        var addressZip = "";
    //        var orderAmount = "0";
    //        var CurrencyCode = "";
    //        var CurrencySymbol = "";
    //        decimal CurrencyValue = 1;
    //        Caluclations caluclations = BalUtility.GenerateOrderSummary();
    //        var User = (User)BalUtility.GetSession(Shared.Sessions.CustomerLogin);


    //        if (User != null)
    //        {
    //            UserId = User.UserId;
    //        }

    //        if (BalUtility.GetCart().Count > 0)
    //        {
    //            var cartItems = (List<UserProductTransaction>)BalUtility.GetSession(Shared.Sessions.CustomerCart) ??
    //                       new List<UserProductTransaction>();
    //            var totalsum = cartItems.Sum(p => p.ProductCost);
    //            var addressrepository = new AddressRepository();
    //            var cartShippingAddressId = BalUtility.GetCart()[0].ShippingAddressId;
    //            UserAddress cartShippingAddress = addressrepository.First(p => p.UserId == UserId && p.UserAddressId == cartShippingAddressId);

    //            firstName = User.FirstName;
    //            lastName = User.LastName;
    //            email = User.EmailId;
    //            phoneNumber = User.MobileNo.ToString();
    //            addressStreet1 = cartShippingAddress != null ? cartShippingAddress.StreetAddress1 + " " +
    //                cartShippingAddress.StreetAddress2 + " " +
    //                cartShippingAddress.LandMark : "";
    //            addressCity = cartShippingAddress != null ? cartShippingAddress.City : "";
    //            addressState = cartShippingAddress != null ? cartShippingAddress.StateName : "";
    //            addressZip = cartShippingAddress != null ? cartShippingAddress.PinCode : "";
    //            orderAmount = totalsum.ToString();
    //            CurrencyCode = ((CustomCurrency)BalUtility.GetSession(Shared.Sessions.Currency)).ToCurrency;
    //            CurrencySymbol = ((CustomCurrency)BalUtility.GetSession(Shared.Sessions.Currency)).Symbol;
    //            CurrencyValue = ((CustomCurrency)BalUtility.GetSession(Shared.Sessions.Currency)).Value;
    //        }

    //        return new
    //        {
    //            UserId = UserId,
    //            firstName = firstName,
    //            lastName = lastName,
    //            email = User.EmailId,
    //            phoneNumber = phoneNumber,
    //            addressStreet1 = addressStreet1,
    //            addressCity = addressCity,
    //            addressState = addressState,
    //            addressZip = addressZip,
    //            orderAmount = orderAmount,
    //            CurrencyCode = CurrencyCode,
    //            CurrencySymbol = CurrencySymbol,
    //            CurrencyValue = CurrencyValue
    //        };


    //    }
    //    catch (Exception ex)
    //    {
    //        // Shared.Log.Error(ex);
    //        throw;
    //    }
    //}

    [HttpGet]
    public dynamic DeleteFromCart(int id, int sid)
    {
        try
        {
            HttpContext.Current.Session["OfferedProductID"] = null;
            BalUtility.DeleteCartItem(id, sid);
            System.Web.HttpCookie cookie = HttpContext.Current.Request.Cookies["CartInfo"];
            if (cookie != null)
            {
                HttpContext.Current.Response.Cookies["CartInfo"].Expires = DateTime.Now.AddDays(-1);
            }
            var cartItems = (List<UserProductTransaction>)BalUtility.GetSession(Shared.Sessions.CustomerCart) ??
                          new List<UserProductTransaction>();
            StringBuilder strresult = new StringBuilder();
            strresult.Append(@"<a href='" + ApiUrl + "' id='lblempty'  class='button_large' title='Continue shopping'>Your Cart is empty, Continue Shopping</a>");

            if (cartItems.Count == 0)
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.NoData.ToString(),
                    Message = "Your cart is empty",
                    Result = null
                };
            }
            else
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = "",
                    Result = BalUtility.WishListInfo()
                };
            }
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
            throw;
        }

    }

    public bool CheckProductInCart(int productId)
    {
        try
        {
            var cartItems = (List<UserProductTransaction>)BalUtility.GetSession(Shared.Sessions.CustomerCart) ?? new List<UserProductTransaction>();
            var count = (from a in cartItems where a.ProductId == productId select a).Count();
            return count != 0;
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            throw;
        }
    }

    public bool CheckProductInWishList(int productId)
    {
        try
        {

            var cartItems = (List<CustWishList>)BalUtility.GetSession(Shared.Sessions.CustomerWishList) ?? new List<CustWishList>();
            var count = (from a in cartItems where a.ProductId == productId select a).Count();
            return count != 0;
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            throw;
        }
    }


    public string ShowCart()
    {
        try
        {
            var items = BalUtility.GetCart();
            if (items.Count == 0)
            {
                return @" <div class='minicart'> 
	<a class='minicart_link'>0 item(s) - $0.00</a>
	<div class='cart_drop'> <span class='darw'></span>
				<div class='empty'>Your shopping cart is empty!</div>
			</div>
</div>";
            }
            var output = items.GroupBy(webResource => new { webResource.ProductId })
                              .Select(group =>
                              {
                                  var userProductTransaction = @group.FirstOrDefault();
                                  return userProductTransaction != null ? new
                                  {
                                      @group.Key,
                                      Count = @group.Count(),
                                      Total = @group.Sum(p => p.ProductDiscountCost),
                                      img = userProductTransaction.Product.ProductImgUrl.Replace("~/", ApiUrl),
                                      userProductTransaction.Product.ProductName,
                                      userProductTransaction.Product.ProductId
                                  } : null;
                              });

            string customersHtml = @"<div class='minicart'> 
	<a class='minicart_link'>" + items.Count + @" item(s) - " + items.Sum(p => p.ProductDiscountCost) + @"</a>
	<div class='cart_drop'> <span class='darw'></span>
					<ul>";

            var strlist = output.Aggregate("", (current, webResource) => current + (@"<li><a  href='" + ApiUrl + "product/" + webResource.ProductName + @"/" + webResource.ProductId + @"'>
<img src='" + webResource.img + @"' alt='" + webResource.ProductName + @"' title='" + webResource.ProductName + @"' style='width: 40px; height: 40px;'/></a>
<a  href='" + ApiUrl + "product/" + webResource.ProductName + @"/" + webResource.ProductId + @"'>" + webResource.ProductName + @"</a> x&nbsp;" + webResource.ProductId + @" <span class='price'>" + webResource.Total + @"</span>
					<div class='remove'><a class='rm' alt='Remove' title='Remove' onclick='removeCartItem(" + webResource.ProductId + @");' ></a></div>
					</li>"));

            customersHtml += strlist + @"<div class='cart_bottom'>
<div class='subtotal_menu'><small>Total:</small><big>" + items.Sum(p => p.ProductDiscountCost) + @"</big></div>
													<a href='MyCart.aspx'>Checkout</a>
			</div>
			</ul>
			</div>
</div>";

            return customersHtml;
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            throw;
        }
    }


    [HttpGet]
    public dynamic GetCartSession()
    {
        try
        {
            var items = BalUtility.GetCart();
            return items.Count();
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
            throw;
        }


    }

    [HttpGet]
    public dynamic GetCompareProductList()
    {
        try
        {
            CustomCurrency CurrentCurrency = (CustomCurrency)BAL.BalUtility.GetSession(Shared.Sessions.Currency);
            var repository = new ProductFeatureRepository();
            // int totalRecords;

            // List<ProductFeature> data =  repository.GetProductFeaturesToCompare();

            var CompareItems = (List<UserProductTransaction>)BalUtility.GetSession(Shared.Sessions.CustomerCompareList) ??
                    new List<UserProductTransaction>();

            StringBuilder strFeactureInfo = new StringBuilder();

            List<ProductFeature> productFeatures = new List<ProductFeature>();
            foreach (var item in CompareItems)
            {
                productFeatures.AddRange(item.Product.ProductFeatures.ToList());
            }

            var productFeaturesGroupbyCategory = (from a in productFeatures
                                                  group a by new { a.FeaturesSubCategory.FeaturesCategory.FeaturesCategoryId, a.FeaturesSubCategory.FeaturesCategory.FeaturesCategoryName } into gGroup
                                                  select new
                                                  {
                                                      gGroup.Key.FeaturesCategoryId,
                                                      gGroup.Key.FeaturesCategoryName,

                                                  }).ToList();

            foreach (var item in productFeaturesGroupbyCategory)
            {
                strFeactureInfo.Append("<tr><td class='name'  align='center'> <h2>" + item.FeaturesCategoryName + "</h2></td></tr>");//Feature Category Name like Genaral 

                var featuresubcatitems = (from a in productFeatures
                                          where a.FeaturesSubCategory.FeaturesCategoryId == item.FeaturesCategoryId
                                          select new
                                          {
                                              a.FeaturesSubCategory.FeaturesSubCategoryId,
                                              a.FeaturesSubCategory.FeaturesSubCategoryName
                                          }).Distinct().ToList();

                foreach (var subitem in featuresubcatitems)
                {
                    strFeactureInfo.Append("<tr> ");
                    strFeactureInfo.Append("<td><h4>" + subitem.FeaturesSubCategoryName + "</h4></td>");//Feature sub cat item names like OS, Model ,Price

                    var itemsInfo = (from a in productFeatures
                                     where a.FeaturesSubCategory.FeaturesCategoryId == item.FeaturesCategoryId && a.FeaturesSubCategoryId == subitem.FeaturesSubCategoryId
                                     select new
                                     {
                                         a.FeatureInfo
                                     }).ToList();
                    foreach (var iteminfo in itemsInfo)
                    {
                        strFeactureInfo.Append("<td>" + iteminfo.FeatureInfo + "</td>");
                    }
                    strFeactureInfo.Append("</tr>");
                }


            }
            StringBuilder strProductName = new StringBuilder();
            StringBuilder strProdImage = new StringBuilder();
            StringBuilder strProdCost = new StringBuilder();
            StringBuilder strProdmodel = new StringBuilder();

            StringBuilder strAddtocart = new StringBuilder();
            StringBuilder StrRemove = new StringBuilder();


            if (CompareItems.Count > 0)
            {
                foreach (var item in CompareItems)
                {
                    strProdImage.Append(@"<td class='name'><img style='height:50px;width:50px;' src='" + item.Product.ProductImgUrl.Replace("~/", ApiUrl) + "' alt='" + item.Product.ProductImgUrl + @"'></td>");


                }
                foreach (var item in CompareItems)
                {
                    strProdCost.Append(@"<td class='name'>" + CurrentCurrency.Symbol + item.ProductDiscountCost + "</td>");


                }



                foreach (var item in CompareItems)
                {
                    strProductName.Append(@"<td class='name'>" + item.Product.SubCategory.SubCategoryName + "</td>");


                }
                foreach (var item in CompareItems)
                {
                    strProdmodel.Append(@"<td class='name'>" + item.Product.ProductName + "</td>");


                }

                foreach (var item in CompareItems)
                {
                    strAddtocart.Append(@"<td><a  onclick='addToCart(" + item.ProductId + @",1,1)'('48');' class='button'><span>Add to Cart</span></a></td>");


                }
                foreach (var item in CompareItems)
                {
                    StrRemove.Append(@"<td>
		  <input type='hidden' name='remove' value='48' />
		  <a class='button' onclick='deleteFromCompareList(" + item.ProductId + ")' ><span>Remove</span></a></td>");


                }


                StringBuilder strResilt = new StringBuilder();
                strResilt.Append(@"<table class='compare-info'>
	<thead>
	  <tr>
		<td colspan='5'>Product Details</td>
	  </tr>
	</thead>
	<tbody>
	  <tr>
		<td>Product</td>" + strProductName + @"</tr>
	  <tr>
		<td>Image</td>
		 " + strProdImage + @"</tr>
	  <tr><td>Price</td>" + strProdCost + @"</tr>
	  <tr><td>Model</td>" + strProdmodel + @"</tr>
	<tr>" + strFeactureInfo + @"<//tr>
	</tbody>
		<tr>
<td></td>
	 " + strAddtocart + @"</tr>
	<tr>
	<td></td> " + StrRemove + @"</tr>
  </table>");
                return new CustomResponse
                {

                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = "",
                    Result = strResilt.ToString()
                };
            }
            else
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.NoData.ToString(),
                    Message = "No More Products "
                };
            }
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
            throw;
        }
    }


    //CompareList Display
    [HttpGet]
    public dynamic CompareDisPlayInfo()
    {
        try
        {
            CustomCurrency CurrentCurrency = (CustomCurrency)BAL.BalUtility.GetSession(Shared.Sessions.Currency);
            var CompareItems = (List<UserProductTransaction>)BalUtility.GetSession(Shared.Sessions.CustomerCompareList) ??
                           new List<UserProductTransaction>();
            StringBuilder strUlItem = new StringBuilder();
            if (CompareItems.Count > 0)
            {
                foreach (var item in CompareItems)
                {

                    strUlItem.Append(@"<ul>
<li>
<img width='48' src='" + item.Product.ProductImgUrl.Replace("~/", ApiUrl) + "' alt='" + item.Product.ProductName + @"'>
</li>
<li class='compName'>
<div>" + item.Product.ProductName + @"</div>
<div style='color:black; font-weight:bold'>" + CurrentCurrency.Symbol + item.ProductDiscountCost + @"</div>
</li>
<li>

<a>
<img src='c1/fancy_close.png' onclick='deleteFromCompareList(" + item.Product.ProductId + @")' />
</a>
</li>
</ul>");
                }
                StringBuilder strResult = new StringBuilder();
                strResult.Append(@"" + strUlItem + @"<ul>
<li>
  <a href='CompareProduct.aspx'><img src='c1/compare_button.png' /> </a>
</li>
</ul>");

                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = "",
                    Result = strResult.ToString()
                };
            }

            else
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.NoData.ToString(),
                    Message = "",
                    Result = null
                };
            }

        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
            throw;
        }
    }

    //End

    [HttpGet]
    public dynamic CompareListShortInfo()
    {
        try
        {
            var cartItems = (List<UserProductTransaction>)BalUtility.GetSession(Shared.Sessions.CustomerCompareList) ??
                              new List<UserProductTransaction>();


            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Message = "",

                Result = cartItems
            };
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null,
            };
            throw;
        }
    }

    public dynamic AddToCompare(UserProductTransaction cart)
    {
        try
        {
            var pId = cart.ProductId;
            var repository = new ProductRepository();
            var cartItems = (List<UserProductTransaction>)BalUtility.GetSession(Shared.Sessions.CustomerCompareList) ??
                            new List<UserProductTransaction>();

            //Currency Conversion
            CustomCurrency CurrentCurrency = (CustomCurrency)BAL.BalUtility.GetSession(Shared.Sessions.Currency);
            cart.CurrencyCode = CurrentCurrency.ToCurrency;
            cart.CurrencyValue = CurrentCurrency.Value;
            cart.CurrencySymbol = CurrentCurrency.Symbol;

            var product = repository.Single(p => p.ProductId == pId);
            //Product Info 
            if (cartItems.Count == 0)
            {
                cart.Status = 0;
                //cart.Product.SubCategory.CategoryId=
                cart.Product = product;
                cart.ProductCost = product.ProductDiscountCost;
                cart.ProductDiscountCost = product.ProductDiscountCost;
                cart.ProductDiscountPercentage = product.ProductDiscountPercentage;
                cartItems.Add(cart);
                //koti later u can change this accordingly
                // deleteFromWishList(pId);
                BalUtility.CreateSession(cartItems, Shared.Sessions.CustomerCompareList);

                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = CustomMessages.CompareProductAddedSuccessfully,

                };
            }
            else if (cartItems.Count >= 4)
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Fail.ToString(),
                    Message = CustomMessages.MaxCountOfProducts,
                    Result = cartItems.Count()
                };
            }
            else
            {
                int aa = (from a in cartItems where a.Product.SubCategory.CategoryId == product.SubCategory.CategoryId select a.ProductId).Count();
                if (aa == 0)
                {
                    return new CustomResponse
                    {
                        Status = Shared.ResponseStatus.Success.ToString(),
                        Message = CustomMessages.compareMessage,
                        Result = cartItems.Count()
                    };
                }
                else
                {
                    cart.Status = 0;
                    //cart.Product.SubCategory.CategoryId=
                    cart.Product = product;
                    cart.ProductCost = product.ProductDiscountCost;
                    cart.ProductDiscountCost = product.ProductDiscountCost;
                    cart.ProductDiscountPercentage = product.ProductDiscountPercentage;
                    cartItems.Add(cart);
                    //koti later u can change this accordingly
                    //  deleteFromWishList(pId);
                    BalUtility.CreateSession(cartItems, Shared.Sessions.CustomerCompareList);
                    return new CustomResponse
                    {
                        Status = Shared.ResponseStatus.Success.ToString(),
                        Message = CustomMessages.CompareProductAddedSuccessfully,
                        Result = cartItems.Count()
                    };
                }
            }
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
            throw;
        }
    }
    [HttpGet]
    public dynamic DeleteFromCompare(int id)
    {

        try
        {
            BalUtility.DeleteCompareList(id);

            var cartItems = (List<UserProductTransaction>)BalUtility.GetSession(Shared.Sessions.CustomerCompareList) ??
                           new List<UserProductTransaction>();

            if (cartItems.Count == 0)
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.NoData.ToString(),
                    Message = "No More Products",
                    Result = cartItems.Count()


                };
            }
            else
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = "",
                    Result = cartItems.Count()


                };

            }
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
            throw;
        }
    }
    [HttpGet]
    public CustomResponse CartShortInfo()
    {
        db_Zon_HuwEntities entity = new db_Zon_HuwEntities();
        CustomCurrency data = (CustomCurrency)GetCurrency();
        decimal cost = 0;
        StringBuilder strli = new StringBuilder();
        StringBuilder strResult = new StringBuilder();
        var cartItems = (List<UserProductTransaction>)BalUtility.GetSession(Shared.Sessions.CustomerCart) ??
                              new List<UserProductTransaction>();
        var totalsum = cartItems.Sum(p => p.ProductCost);
        var count = 0;

        if (cartItems.Count > 0)
        {
            foreach (var item in cartItems)
            {
                if (item.Product == null)
                {
                    long productid = item.ProductId;
                    if (productid == 0)
                    {

                    }
                    else
                    {
                        Product ProductDetails = entity.Products.Where(x => x.ProductId == productid).First();
                        item.Product = ProductDetails;
                    }
                }

                var sid = item.SubProductId == null ? 0 : item.SubProductId;
                strli.Append(@"  <li class='item last odd'>
			<a  title=" + item.Product.ProductName + @" class='product-image' href='" + ApiUrl + "product/" + item.Product.ProductName + @"/" + item.ProductId + @"' onclick='NavigateToDetails(" + item.ProductId + @")'>
			<img src=" + item.Product.ProductImgUrl.Replace("~/", ApiUrl) + " alt=" + item.Product.ProductName + @" height='50' width='50'></a>
		<div class='product-details'>
<a class='btn-remove'  onclick='deleteFromCart(" + item.ProductId + @"," + sid + @")'  return false;' title='Remove This Item' >Remove This Item</a>
<p class='product-name'><a href='" + ApiUrl + "product/" + item.Product.ProductName + @"/" + item.ProductId + @"' onclick='NavigateToDetails(" + item.ProductId + @")'>" + item.Product.ProductName + @"</a></p>
   <strong><label value=" + item.Quantity + @"></label></strong>  x
	 <span class='price'>" + data.Symbol + "" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.Product.ProductCost), true) + @"</span>                    
		<div class='truncated'>
		
		<a href='" + ApiUrl + "product/" + item.Product.ProductName + @"/" + item.ProductId + @"' onclick='NavigateToDetails(" + item.ProductId + @")' class='details'>Details</a>
		</div>
			</div></li>");
                count = count + 1;
            }
            strResult.Append(@" 
		 <div class='summary'>
			<h2 class='classy'>
	  <a href='" + ApiUrl + "MyCart.aspx' ><span class='Itext'>Cart</span>(" + count + @")</a></h2>
		</div>
  <div style='display: none;' class='remain_cart' id='minicart'>
		
						
		<p class='empty'>" + CustomMessages.AddSuccesCat + @"</p>
	   <ol id='cart-sidebar' class='mini-products-list'>
	  " + strli + @"
   </ol>
	   <div class='actions_checkout'>
	<p class='subtotal'>
	<span class='label'>Total:</span> <span class='price'>" + data.Symbol + "" + BAL.BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(totalsum), true) + @" </span>                                                </p>
   <button type='button' title='Checkout' class='button'  onclick='CheckCart()' ><span><span>Checkout</span></span></button>
	</div></div>");
        }
        else
        {
            StringBuilder strempty = new StringBuilder();
            strempty.Append(@"   <div class='summary'>
			<h2 class='classy'>
	  <a><span class='Cart'>Cart</span>(" + count + @")</a> </h2>
		</div>
  <div style='display: none;' class='remain_cart' id='minicart'>
		
						
		<p class='empty'>" + CustomMessages.CartEmpty + @"</p>
	  
   </div>");
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Message = "",
                Result = strempty.ToString()


            };
        }
        try
        {


            Caluclations caluclations = BalUtility.GenerateOrderSummary();
            cost = Math.Round(caluclations.TotalPriceAfterDeductions, 2, MidpointRounding.ToEven);

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Message = "",
                Result = strResult.ToString()
            };
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null,
            };
            throw;
        }
    }


    [HttpGet]
    public CustomResponse WishListShortInfo()
    {
        try
        {
            var cartItems = (List<CustWishList>)BalUtility.GetSession(Shared.Sessions.CustomerWishList) ??
                              new List<CustWishList>();

            if (cartItems.Count > 0)
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = "",
                    Result = cartItems.Count == 0 ? "Wish List (" + cartItems.Count + ")" : "<a href='" + ApiUrl + "WishList.aspx'>Wish List (" + cartItems.Count + ")</a>"
                };
            }
            else
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.NoData.ToString(),
                    Message = CustomMessages.CartEmpty.ToString(),
                    Result = "<a href='" + ApiUrl + "WishList.aspx'>Wish List (" + cartItems.Count + ")</a>".ToString()
                };
            }
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null,
            };
            throw;
        }
    }


    [HttpGet]
    public CustomResponse ShowWishList()
    {
        try
        {
            List<CustWishList> items = (List<CustWishList>)BalUtility.GetSession(Shared.Sessions.CustomerWishList) ?? new List<CustWishList>();



            StringBuilder strtrResult = new StringBuilder();
            CustomCurrency CurrentCurrency = (CustomCurrency)BAL.BalUtility.GetSession(Shared.Sessions.Currency);

            foreach (var item in items)
            {

                strtrResult.Append(@"<tr> <td class='name'> <img style='height:50px;width:50px;' src='" + item.ProductImgUrl.Replace("~/", ApiUrl) + "' alt='" + item.ProductName + @"'> </td> 
			<td class='model'>" + item.ProductName + @"</td> <td class='quantity'>" + CurrentCurrency.Symbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.ProductCost)) + @"</td> 
			 <td><div class='quantity'>
	<a class='button' href='" + ApiUrl + "product/" + item.ProductName + @"/" + item.ProductId + @"' onclick='NavigateToDetails(" + item.ProductId + @")'>
	<span>AddToCart</span>
	</a>
	</div>

</td>
  <td><div class='quantity'>
	<a class='button' onclick='deleteFromWishList(" + item.ProductId + @")'>
	<span>Remove</span>
	</a>
	</div>

</td></tr> ");

            }

            if (items.Count == 0)
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.NoData.ToString(),
                    Message = "No products currently in list",

                };

            }


            StringBuilder strResult = new StringBuilder();
            strResult.Append(@" <div class='checkout-product'><table id='log_table_1'> <thead> <tr> <td class='name'>Image</td> <td class='Customer'>ProductName</td> <td class='quantity'>UntiPrice</td> <td class='price'>BuyNow</td> <td class='total'>Remove</td></tr> </thead> <tbody> 
	" + strtrResult + @" </tbody> </table> </div>");
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Message = "",
                Result = strResult.ToString()
            };

        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
            throw;
        }
    }

    [HttpPost]
    public dynamic AddToWishList(int productId)
    {
        try
        {
            var WishlistItems = (List<CustWishList>)BalUtility.GetSession(Shared.Sessions.CustomerWishList) ??
                          new List<CustWishList>();

            var repository = new ProductRepository();
            Product product = repository.GetProduct(productId);

            var custWishList = new CustWishList();

            custWishList.PropertyId = product.ProductId;
            custWishList.ProductId = product.ProductId;
            custWishList.ProductImgUrl = product.ProductImgUrl;
            custWishList.ProductName = product.ProductName;
            custWishList.ProductCost = product.ProductDiscountCost;

            WishlistItems.Add(custWishList);
            BalUtility.CreateSession(WishlistItems, Shared.Sessions.CustomerWishList);

            return new
            {
                success = CustomMessages.WishlistAddedSuccessfully,
                total = WishlistItems.Count == 0 ? "Wish List (" + WishlistItems.Count + ")" : "<a href='WishList.aspx'>Wish List (" + WishlistItems.Count + ")</a>"
            };
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            throw;
        }
    }



    public dynamic UpdateReplacementProduct(long ptid, long trnsId)
    {
        var repository = new UserProductTransactionRepository();


        var data = repository.Single(p => p.TransactionId == trnsId);

        data.IsActive = false;
        data.IsDeleted = true;
        data.IsReplaced = true;
        data.UpdatedOn = DateTime.Now;
        data.ReplacementTransactionId = ptid;
        repository.Update(data);
        return data;
    }

    public CustomResponse MakePayment()
    {
        try
        {
            var repository = new PaymentTransactionRepository();
            var cart = BalUtility.GetCart();


            PaymentTransaction payment = repository.MakePayment(cart);

            if (payment.PaymentStatus == 0)
            {
                var cartItems = (List<UserProductTransaction>)BalUtility.GetSession(Shared.Sessions.CustomerCart) ??
                                  new List<UserProductTransaction>();
                var repository1 = new ProductRepository();
                Product product = null;

                foreach (var item in cartItems)
                    if (item.ProductId != 0)
                    {
                        product = repository1.Single(p => p.ProductId == item.ProductId);
                        var availableQuantity = product.Quantity - item.Quantity;
                        if (availableQuantity == 0)
                        {
                            product.Quantity = availableQuantity;
                            product.IsSold = true;
                            repository1.Update(product);
                        }
                        else
                        {
                            product.Quantity = availableQuantity;
                            repository1.Update(product);
                        }

                    }
            }
            BalUtility.ClearSession(Shared.Sessions.CustomerCart);

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Message = "",
                Result = "Success"
            };


        }
        catch (Exception ex)
        {

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = "",
                Result = null
            };
            // Shared.Log.Error(ex);
            throw;
        }
    }


    [HttpGet]
    public CustomResponse MakePaymentFromAdmin(int trnsId)
    {
        try
        {
            var repository = new PaymentTransactionRepository();

            GetReplacementDetails(trnsId);

            var items = BalUtility.GetReplacementList();
            PaymentTransaction payment = repository.MakePaymentFromAdmin(items);
            long ptid = payment.PaymentTransactionId;
            UpdateReplacementProduct(ptid, trnsId);
            BalUtility.ClearSession(Shared.Sessions.ReplacementList);

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = payment.PaymentTransactionId
            };


        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            throw;
        }
    }

    public dynamic GetCurrency()
    {
        try
        {
            return (CustomCurrency)BAL.BalUtility.GetSession(Shared.Sessions.Currency);
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            throw;
        }

    }

    #region SuperCategory Info

    public dynamic GetSuperCategoryList(string sidx, string sord, int page, int rows)
    {
        var repository = new SuperCategoryRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.SuperCategoryId, out totalRecords, (page - 1) * rows, rows, (p => p.IsActive && p.IsDeleted == false), c => new { c.SuperCategoryId, c.SuperCategoryName, c.IsActive });

        return new
        {
            total = totalRecords / rows + 1,
            page,
            records = totalRecords,
            rows = data
        };
    }

    public string GetSuperCategoryListAsHtml()
    {
        var repository = new SuperCategoryRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.SuperCategoryId, out totalRecords, 0, 0, (p => p.IsActive && p.IsDeleted == false), c => new { c.SuperCategoryId, c.SuperCategoryName, c.IsActive });

        string customersHtml = data.Aggregate(" <select>", (current, item) => current + string.Format("<option value={0}>{1}</option>", item.SuperCategoryId, item.SuperCategoryName));
        customersHtml += "</select>";
        return customersHtml;
    }

    [HttpPost]
    public string CreateSuperCategory(SuperCategory data)
    {
        try
        {
            var repository = new SuperCategoryRepository();
            repository.Insert(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }


    [HttpPost]
    public string EditSuperCategory(SuperCategory data)
    {
        try
        {
            var repository = new SuperCategoryRepository();

            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpGet]
    public string DeleteSuperCategory(int id, string name)
    {
        try
        {
            var repository = new SuperCategoryRepository();
            long categoryId = Convert.ToInt32(name);
            var data = repository.First(p => p.SuperCategoryId == categoryId);
            data.IsDeleted = true;
            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    #endregion

    #region Category Info

    public dynamic GetCategoryList(string sidx, string sord, int page, int rows)
    {
        var repository = new CategoryRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.SuperCategoryId, out totalRecords, (page - 1) * rows, rows,
                                             (p => p.IsActive && p.IsDeleted == false && p.SuperCategory.IsActive && p.SuperCategory.IsDeleted == false), null, "SuperCategory");
        foreach (var Category in data)
            Category.SuperCategoryName = Category.SuperCategory.SuperCategoryName;

        return new
        {
            total = totalRecords / rows + 1,
            page,
            records = totalRecords,
            rows = data
        };
    }

    public string GetCategoryListAsHtml()
    {
        var repository = new CategoryRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.CategoryId, out totalRecords, 0, 0, (p => p.IsActive && p.IsDeleted == false), c => new { c.CategoryId, c.CategoryName, c.IsActive });

        string customersHtml = data.Aggregate(" <select>", (current, item) => current + string.Format("<option value={0}>{1}</option>", item.SuperCategoryId, item.CategoryName));
        customersHtml += "</select>";
        return customersHtml;
    }


    public List<SuperCategory> GetCategoryList()
    {
        var repository = new SuperCategoryRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.SuperCategoryId, out totalRecords, 0, 0,
                                             (p => p.IsActive && p.IsDeleted == false), null, "Category");
        return data;
    }

    public List<Category> GetCategoryListBySuperCategory(int id)
    {
        var repository = new CategoryRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.CategoryId, out totalRecords, 0, 0,
                                             (p => p.IsActive && p.IsDeleted == false && p.SuperCategoryId == id), null, "SuperCategory");

        foreach (var Category in data)
            Category.SuperCategoryName = Category.SuperCategory.SuperCategoryName;

        return data;
    }

    [HttpPost]
    public string CreateCategory(Category data)
    {
        try
        {
            var repository = new CategoryRepository();
            repository.Insert(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpPost]
    public string EditCategory(Category data)
    {
        try
        {
            var repository = new CategoryRepository();
            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpGet]
    public string DeleteCategory(int id, string name)
    {
        try
        {
            var repository = new CategoryRepository();
            long CategoryId = Convert.ToInt32(name);
            var data = repository.First(p => p.CategoryId == CategoryId);
            data.IsDeleted = true;
            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }


    #endregion

    #region Sub Category Info

    public dynamic GetSubCategoryList(string sidx, string sord, int page, int rows)
    {
        var repository = new SubCategoryRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.CategoryId, out totalRecords, (page - 1) * rows, rows,
                                             (p => p.IsActive && p.IsDeleted == false && p.Category.IsActive && p.Category.IsDeleted == false), null, "Category");
        foreach (var subCategory in data)
            subCategory.CategoryName = subCategory.Category.CategoryName;

        return new
        {
            total = totalRecords / rows + 1,
            page,
            records = totalRecords,
            rows = data
        };
    }

    public List<Category> GetSubCategoryList()
    {
        var repository = new CategoryRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.CategoryId, out totalRecords, 0, 0,
                                             (p => p.IsActive && p.IsDeleted == false), null, "SubCategory");
        return data;
    }

    public CustomResponse GetSubCategoryListByCategory(int id)
    {
        try
        {
            //Top Menu From DataBase
            StringBuilder strResult = new StringBuilder();
            var repository = new CategoryRepository();
            IEnumerable<Object> subCatByGroup = repository.GetSubCatByCatByGroup(id);

            var CatList = (from a in subCatByGroup
                           select new
                           {
                               CategoryId = a.GetType().GetProperty("CategoryId").GetValue(a, null),
                               CategoryName = a.GetType().GetProperty("CategoryName").GetValue(a, null),
                               SubCategories = a.GetType().GetProperty("SubCategories").GetValue(a, null),
                           }).ToList();

            foreach (var Sitem in CatList)
            {
                strResult.Append(@"<ul id='side_menu'>");

                strResult.Append("<label>" + Sitem.CategoryName + "</label>");

                // SubCategoryId , SubCategoryName , CategoryId , CategoryName  , Count ,
                foreach (var item in (IEnumerable<Object>)Sitem.SubCategories)
                {
                    //   $("#category_nameheading").text(item.CategoryName);

                    strResult.Append(@"<li id='side_menu1'><a href='#' ><input type='checkbox' name='checkboxlist' fieldvalue='" +
                         item.GetType().GetProperty("SubCategoryId").GetValue(item, null) +
                         "'  fieldtext='" + item.GetType().GetProperty("SubCategoryName").GetValue(item, null) + "'   onclick='GetProducts()'/>   " +
                         item.GetType().GetProperty("SubCategoryName").GetValue(item, null) +
                                     "(" + item.GetType().GetProperty("Count").GetValue(item, null) + ")</a></li>");



                    //strResult.Append(@"<li><a href='#' onclick=SearchData(this," + Sitem.CategoryId + "," +
                    //    item.GetType().GetProperty("SubCategoryId").GetValue(item, null) + ")>" +
                    //    item.GetType().GetProperty("SubCategoryName").GetValue(item, null) +
                    //    "(" + item.GetType().GetProperty("Count").GetValue(item, null) + ")</a></li>");
                }

                strResult.Append(@"</ul>");
            }
            return new CustomResponse
            {
                Status = string.IsNullOrWhiteSpace(strResult.ToString()) ? Shared.ResponseStatus.NoData.ToString() : Shared.ResponseStatus.Success.ToString(),
                Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
                Result = strResult.ToString()
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }


    }


    //public CustomResponse GetBrandnames(string pId, string psubId)
    //{
    //    try
    //    {
    //        var repository = new ProductRepository();
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }

    //}

    public CustomResponse GetBrands(long pcategoryId = 0, string subCategories = "0")
    {
        try
        {
            var repository = new ProductRepository();
            StringBuilder strResult = new StringBuilder();
            int totalRecords;
            List<long> subcatlst = new List<long>();
            if (subCategories != null)
            {
                var stringarray = subCategories.Split('^');
                foreach (var item in stringarray)
                {
                    subcatlst.Add(Convert.ToInt64(item));
                }
            }

            var Brands = subCategories == "0" ? (repository.FetchAllByPage(p => p.ProductId, out totalRecords, 0, 0,
                       (p => p.IsSold == false && p.IsDeleted == false && p.SubCategory.CategoryId == pcategoryId), null, "").Distinct())
                       :
                       (repository.FetchAllByPage(p => p.ProductId, out totalRecords, 0, 0,
                   (p => p.IsSold == false && p.IsDeleted == false && p.SubCategory.CategoryId == pcategoryId && (subcatlst.Count == 0 ? true : subcatlst.Contains(p.SubCategoryId))), null, "").Distinct());
            // var Brands = psubCategoryId == 0 ? repository.GetbrndsByCatId(pcategoryId)

            //  var Brands = repository.Getbrnds( );     //.Select(item => item.Brand).Distinct();

            strResult.Append(@"<ul id='side_menu2'>");

            strResult.Append("<label><b>Brands</b></label>");
            foreach (var item in Brands)
            {
                strResult.Append(@"<li id='side_menu1'><a href='#' ><input type='checkbox' name='checkboxlist' fieldvalue='" + item.Brand +
                        "'  fieldtext='" + item.Brand + "'   onclick='GetProducts()'/>&nbsp;" + item.Brand + "</a></li>");

            }
            strResult.Append(@"</ul>");

            return new CustomResponse
            {
                Status = string.IsNullOrWhiteSpace(strResult.ToString()) ? Shared.ResponseStatus.NoData.ToString() : Shared.ResponseStatus.Success.ToString(),
                Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
                Result = strResult.ToString()
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }
    }

    [HttpPost]
    public string CreateSubCategory(SubCategory data)
    {
        try
        {
            var repository = new SubCategoryRepository();
            repository.Insert(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpPost]
    public string EditSubCategory(SubCategory data)
    {
        try
        {
            var repository = new SubCategoryRepository();
            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpGet]
    public string DeleteSubCategory(int id, string name)
    {
        try
        {
            var repository = new SubCategoryRepository();
            long subCategoryId = Convert.ToInt32(name);
            var data = repository.First(p => p.SubCategoryId == subCategoryId);
            data.IsDeleted = true;
            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    #endregion

    #region Role Info

    public dynamic GetRoleList(string sidx, string sord, int page, int rows)
    {
        var repository = new RoleRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.RoleId, out totalRecords, (page - 1) * rows, rows, (p => p.IsActive && p.IsDeleted == false));

        return new
        {
            total = totalRecords / rows + 1,
            page,
            records = totalRecords,
            rows = data
        };
    }

    public string GetRoleListAsHtml()
    {
        var repository = new RoleRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.RoleId, out totalRecords, 0, 0, (p => p.IsActive && p.IsDeleted == false));

        string customersHtml = data.Aggregate(" <select>", (current, item) => current + string.Format("<option value={0}>{1}</option>", item.RoleId, item.RoleName));
        customersHtml += "</select>";
        return customersHtml;
    }

    [HttpPost]
    public string CreateRole(Role data)
    {
        try
        {
            var repository = new RoleRepository();
            repository.Insert(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpPost]
    public string EditRole(Role data)
    {
        try
        {
            var repository = new RoleRepository();

            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpGet]
    public string DeleteRole(int id, string name)
    {
        try
        {
            var repository = new RoleRepository();
            long roleId = Convert.ToInt32(name);
            var data = repository.First(p => p.RoleId == roleId);
            data.IsDeleted = true;
            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    #endregion

    #region Country Info

    public dynamic GetCountryList(string sidx, string sord, int page, int rows)
    {
        var repository = new CountryRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.CountryId, out totalRecords, (page - 1) * rows, rows, (p => p.IsActive && p.IsDeleted == false), c => new { c.CountryId, c.CountryName, c.IsActive });

        return new
        {
            total = totalRecords / rows + 1,
            page,
            records = totalRecords,
            rows = data
        };
    }

    public string GetCountryListAsHtml()
    {
        var repository = new CountryRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.CountryId, out totalRecords, 0, 0, (p => p.IsActive && p.IsDeleted == false), c => new { c.CountryId, c.CountryName, c.IsActive });

        string customersHtml = data.Aggregate(" <select>", (current, item) => current + string.Format("<option value={0}>{1}</option>", item.CountryId, item.CountryName));
        customersHtml += "</select>";
        return customersHtml;
    }

    [HttpPost]
    public string CreateCountry(Country data)
    {
        try
        {
            var repository = new CountryRepository();
            repository.Insert(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpPost]
    public string EditCountry(Country data)
    {
        try
        {
            var repository = new CountryRepository();
            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpGet]
    public string DeleteCountry(int id, string name)
    {
        try
        {
            var repository = new CountryRepository();
            long countryId = Convert.ToInt32(name);
            var data = repository.First(p => p.CountryId == countryId);
            data.IsDeleted = true;
            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    #endregion

    #region States Info

    public dynamic GetStatesList(string sidx, string sord, int page, int rows)
    {
        var repository = new StatesRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.CountryId, out totalRecords, (page - 1) * rows, rows,
                                             (p => p.IsActive && p.IsDeleted == false), null, "Country");
        foreach (var States in data)
        {
            States.CountryName = States.Country.CountryName;
        }

        return new
        {
            total = totalRecords / rows + 1,
            page,
            records = totalRecords,
            rows = data
        };
    }


    [HttpPost]
    public string CreateStates(State data)
    {
        try
        {
            var repository = new StatesRepository();
            repository.Insert(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpPost]
    public string EditStates(State data)
    {
        try
        {
            var repository = new StatesRepository();
            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpGet]
    public string DeleteStates(int id, string name)
    {
        try
        {
            var repository = new StatesRepository();
            long StateId = Convert.ToInt32(name);
            var data = repository.First(p => p.StateId == StateId);
            data.IsDeleted = true;
            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    #endregion

    #region Product Info

    public dynamic GetproductListAdmin(string sidx, string sord, int page, int rows)
    {
        var repository = new ProductRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.ProductId, out totalRecords, (page - 1) * rows, rows,
                                             (p => p.IsActive && p.IsDeleted == false));

        return new
        {
            total = totalRecords / rows + 1,
            page,
            records = totalRecords,
            rows = data
        };
    }
    public CustomResponse GetDeactivatedProductList()
    {
        db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();

        var data = Entity.Products.Where(p => p.IsActive && p.IsDeleted == true).ToList();
        //using (var ctx = new db_Zon_HuwEntities())
        //{
        //    //this will throw an exception
        //    var data = ctx.Products.SqlQuery("Select top 100 * from Products where IsDeleted = 'true' order by productid desc").ToList();

        return new CustomResponse
        {
            Result = data,
            Status = Shared.ResponseStatus.Success.ToString()
        };

        //}
    }

    public CustomResponse GetCouponslist()
    {
        db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();

        var data = Entity.Cashback_Coupons.Where(p => p.Is_Active == true).ToList();
        //using (var ctx = new db_Zon_HuwEntities())
        //{
        //    //this will throw an exception
        //    var data = ctx.Products.SqlQuery("Select top 100 * from Products where IsDeleted = 'true' order by productid desc").ToList();

        return new CustomResponse
        {
            Result = data,
            Status = Shared.ResponseStatus.Success.ToString()
        };

        //}
    }

    public CustomResponse GetDeactivatedProductID(int ProductID)
    {
        db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();

        var data = Entity.Products.Where(p => (p.IsActive == false || p.IsDeleted == true) && p.ProductId == ProductID).ToList();

        return new CustomResponse
        {
            Result = data,
            Status = Shared.ResponseStatus.Success.ToString()
        };
    }

    public dynamic GetAllProductsBySuperCategory(int SuperCatId, int sort, int value)
    {
        JavaScriptSerializer objserializer = new JavaScriptSerializer();

        try
        {
            value = value * 15;
            var repository = new ProductRepository();
            CustomCurrency currency = (CustomCurrency)BAL.BalUtility.GetSession(Shared.Sessions.Currency);

            List<ProductListDTO> data = repository.GetProductsBySuperCategory(SuperCatId, sort, value);


            string jsonstring = objserializer.Serialize(data);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Message = "",
                Result = jsonstring
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }
    }

    public dynamic GetAllProductsBetweenPriceinSuperCategory(int SuperCatId, int sort, decimal MinPrice, decimal MaxPrice)
    {


        JavaScriptSerializer objserializer = new JavaScriptSerializer();

        try
        {

            var repository = new ProductRepository();
            CustomCurrency currency = (CustomCurrency)BAL.BalUtility.GetSession(Shared.Sessions.Currency);

            List<ProductListDTO> data = repository.GetProductsBetweenPriceinSuperCategory(SuperCatId, sort, MinPrice, MaxPrice);


            string jsonstring = objserializer.Serialize(data);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Message = "",
                Result = jsonstring
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }
    }

    public dynamic GetAllProductsByCategory(int SuperCatId, int SubId, int CatId, int sort, string brand)
    {
        JavaScriptSerializer objserializer = new JavaScriptSerializer();

        try
        {

            var repository = new ProductRepository();
            CustomCurrency currency = (CustomCurrency)BAL.BalUtility.GetSession(Shared.Sessions.Currency);

            List<ProductListDTO> data = repository.GetProductsByCategory(SuperCatId, SubId, CatId, sort, brand);
            List<ProductListDTO> data1 = new List<ProductListDTO>();

            if (CatId != 0)
            {
                data1 = repository.GetSubCategoryList(CatId);
            }
            StringBuilder strResult = new StringBuilder();

            strResult.Append(@"<ul id='side_menu'>");
            int i = 0;
            int subCategoryProductCount = 0;
            foreach (ProductListDTO s in data1)
            {
                subCategoryProductCount += s.SubCategoryProductCount;
                if (i == 0)
                {
                    strResult.Append(@"<div class='block-title'>
							<strong><span>" + s.CategoryName + @"</span></strong></div>");

                }
                strResult.Append(@"<li id='side_menu1'><a href='#'><label><input type='checkbox' name='checkboxlist' id='" + s.SubCategoryId + "' value='" + SuperCatId + "," + CatId + "," + s.SubCategoryId + "," + s.Brand + "' onclick='getSelectedProducts(" + SuperCatId + "," + CatId + "," + s.SubCategoryId + ", this)'  />  " + s.SubCategoryName +
                    "</label></a></li>");
                i++;
            }
            strResult.Append(@"</ul>");

            foreach (ProductListDTO p in data)
            {
                p.sidelist = strResult.ToString();
            }
            objserializer.MaxJsonLength = Int32.MaxValue;
            string jsonstring = objserializer.Serialize(data);
            if (data.Count == 0)
            {
                ProductListDTO p = new ProductListDTO();
                p.sidelist = strResult.ToString();
            }
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Message = strResult.ToString(),
                Result = jsonstring
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }
    }

    public dynamic GetAllProductsBetweenPriceinCategory(int SuperCatId, int SubId, int CatId, int sort, string brand, decimal MinPrice, decimal MaxPrice)
    {


        JavaScriptSerializer objserializer = new JavaScriptSerializer();

        try
        {

            var repository = new ProductRepository();
            CustomCurrency currency = (CustomCurrency)BAL.BalUtility.GetSession(Shared.Sessions.Currency);

            List<ProductListDTO> data = repository.GetProductsBetweenPriceinCategory(SuperCatId, SubId, CatId, sort, brand, MinPrice, MaxPrice);
            List<ProductListDTO> data1 = new List<ProductListDTO>();
            if (sort == 1)
            {

            }
            if (CatId != 0)
            {
                data1 = repository.GetSubCategoryList(CatId);
            }
            StringBuilder strResult = new StringBuilder();

            strResult.Append(@"<ul id='side_menu'>");
            int i = 0;
            int subCategoryProductCount = 0;
            foreach (ProductListDTO s in data1)
            {
                subCategoryProductCount += s.SubCategoryProductCount;
            }
            foreach (ProductListDTO s in data1)
            {
                if (i == 0)
                {
                    strResult.Append(@"<div class='block-title'>
							<strong><span>" + s.CategoryName + @"</span></strong></div>");
                    //" ( " + subCategoryProductCount +

                }
                strResult.Append(@"<li id='side_menu1'><a href='#'><label><input type='checkbox' name='checkboxlist' id='" + s.SubCategoryId + "' value='" + SuperCatId + "," + CatId + "," + s.SubCategoryId + "," + s.Brand + "' onclick='getSelectedProducts(" + SuperCatId + "," + CatId + "," + s.SubCategoryId + ", this)'  />  " + s.SubCategoryName +
                    //"(" + s.SubCategoryProductCount + 
                    "</label></a></li>");
                //onclick=javascript:window.location.href='Category.aspx?Sid=" + SuperCatId + "&Subid=" + s.SubCategoryId + "&id="+CatId+"'
                i++;
            }
            strResult.Append(@"</ul>");

            foreach (ProductListDTO p in data)
            {
                p.sidelist = strResult.ToString();
                // p.Brand = strBrand.ToString();
            }
            objserializer.MaxJsonLength = Int32.MaxValue;
            string jsonstring = objserializer.Serialize(data);
            if (data.Count == 0)
            {
                ProductListDTO p = new ProductListDTO();
                p.sidelist = strResult.ToString();
                // p.Brand = strBrand.ToString();
                //data.Add(p);
            }
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Message = strResult.ToString(),
                // CartSum = strBrand.ToString(),
                Result = jsonstring
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }
    }


    public dynamic GetAllBrands(int SuperCatId, int SubId, int CatId, int sort, string brand)
    {
        JavaScriptSerializer objserializer = new JavaScriptSerializer();
        try
        {
            var repository = new ProductRepository();
            CustomCurrency currency = (CustomCurrency)BAL.BalUtility.GetSession(Shared.Sessions.Currency);

            List<ProductListDTO> data = repository.GetProductsByCategory(SuperCatId, SubId, CatId, sort, brand);
            List<ProductListDTO> brandData = new List<ProductListDTO>();
            if (CatId != 0)
            {
                if (SubId != 0)
                {
                    brandData = repository.GetBrandList(SubId);
                }
                else
                {
                    brandData = repository.GetCategoryBrandList(CatId);
                }
            }
            else
            {
                brandData = repository.GetSuperCatBrandList(SuperCatId);

            }
            if (SuperCatId == 8)
            {
                data = repository.GetProductsBySuperCategory(SuperCatId, sort, 100);
                brandData.Clear();
                foreach (ProductListDTO s in data.OrderBy(x => x.Brand))
                {
                    ProductListDTO brands = new ProductListDTO();
                    brands.Brand = s.Brand;
                    brandData.Add(brands);
                }
            }

            var DistinctItems = brandData.GroupBy(x => x.Brand).Select(y => y.First());

            List<ProductListDTO> prods = new List<ProductListDTO>();
            foreach (var item in DistinctItems.OrderBy(x => x.Brand))
            {
                ProductListDTO prod = new ProductListDTO()
                {
                    Brand = item.Brand
                };
                prods.Add(prod);
            }
            StringBuilder strBrand = new StringBuilder();
            strBrand.Append(@"<ul id='side_menu'>");
            int l = 0;
            foreach (ProductListDTO s in prods)
            {
                if (l == null)
                {
                    strBrand.Append("<label>" + s.CategoryName +
                        "</label>");
                }
                else if (s.Brand != null)
                    strBrand.Append(@"<li id='side_menu1'><a href='#'><label><input type='checkbox' name='checkboxlist' id='" + s.Brand + "' value='" + SuperCatId + "," + CatId + "," + s.SubCategoryId + "," + s.Brand + "' onclick='getSelectedBrandProducts()'  /> " + s.Brand +
                        "</label></a></li>");
                l++;
            }
            strBrand.Append(@"</ul>");
            foreach (ProductListDTO p in prods)
            {
                p.sidelist = strBrand.ToString();
            }
            objserializer.MaxJsonLength = Int32.MaxValue;
            string jsonstring = objserializer.Serialize(prods);
            if (data.Count == 0)
            {
                ProductListDTO p = new ProductListDTO();
                p.sidelist = strBrand.ToString();
            }
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Message = strBrand.ToString(),
                Result = jsonstring
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }
    }


    //New*************************************************
    public CustomResponse GetProductList(string sord, int minprice, int maxprice, string SubCategories, string Brands, string Colors, int rows, int? categoryId, int subCategoryId, int SuperCatId)
    {
        try
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
            else
            {
                if (subCategoryId != 0)
                {
                    SCat.Add(subCategoryId);
                }
            }
            List<string> Brnds = new List<string>();
            if (Brands != null)
            {
                var stringaray = Brands.Split(',');
                foreach (var item in stringaray)
                {
                    Brnds.Add(item);
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
            //if (sord == "2")
            //{
            //    desc = true;
            //}
            Func<Product, object> keySelector = p => p.CreatedOn;
            if (sord == "1" || sord == "2")
            {
                keySelector = p => p.ProductCost;
            }

            CustomCurrency currency = (CustomCurrency)BAL.BalUtility.GetSession(Shared.Sessions.Currency);


            var data = SuperCatId == 0 ? (repository.FetchAllByPage(keySelector, out totalRecords, rows, rows,
                                                         (p => p.IsSold == false &&
                                                          p.IsDeleted == false && p.ProductCost >= minprice && p.ProductCost <= maxprice &&
                                                          p.SubCategory.CategoryId == categoryId && (SCat.Count == 0 ? true : SCat.Contains(p.SubCategoryId)) && (Clrs.Count == 0 ? true : Clrs.Contains(p.ProductColor)) && (Brnds.Count == 0 ? true : Brnds.Contains(p.Brand))), null, "", desc).Distinct()

                                                          ) : (repository.FetchAllByPage(keySelector, out totalRecords, rows, rows,
                                                         (p => p.IsSold == false &&
                                                        p.IsDeleted == false && p.ProductCost >= minprice && p.ProductCost <= maxprice &&
                                                          p.SubCategory.Category.SuperCategoryId == SuperCatId), null, "", desc).Distinct()

                                                          ).OrderByDescending(p => p.ProductId);

            StringBuilder strResult = new StringBuilder();
            StringBuilder strul = new StringBuilder();
            var pro = (from a in data select a).FirstOrDefault();

            List<Utility.Product> dta = new List<Utility.Product>();
            if (sord == "2")
            {
                dta = data.OrderByDescending(p => p.ProductCost).ToList();
            }

            else if (sord == "1")
            {
                dta = data.OrderBy(p => p.ProductCost).ToList();
            }
            else if (sord == "0")
            {
                dta = data.OrderBy(p => p.ProductId).ToList();
            }
            var CompareItems = (List<UserProductTransaction>)BalUtility.GetSession(Shared.Sessions.CustomerCompareList) ??
                        new List<UserProductTransaction>();
            strResult.Append(@"

<div class='breadcrumbs'> <ul>  <li class='home'> <a href='" + ApiUrl + @"' title='Go to Home Page'>Home</a><span>></span>
						</li> <li > <a >" + pro.SubCategory.Category.SuperCategory.SuperCategoryName + @"</a> <span>></span> </li>  <li class='product'>
							<a  title=''><strong>" + pro.SubCategory.Category.CategoryName + @" </strong></a> 
							  </li> </ul></div>");

            if (data.Count() > 0)
            {
                strResult.Append(@" <div class='breadcrumbs'><div style='color:#66BCDA; font-weight:bold;'>Showing&nbsp;<strong style='color:#D9387E;'>" + data.Count() + @"</strong>&nbsp;Product(s)</div></div>");
            }
            foreach (var item in dta)
            {
                string ischecked = (from a in CompareItems where a.ProductId == item.ProductId select a).Count() > 0 ? "checked= checked" : "";
                string ischecked1 = (from a in CompareItems select a).Count() >= 3 ? " disabled= disabled" : "";
                strResult.Append(@"
				  <li class='item first'> 
				 <div class='outer_pan'>
				<div class='image_rotate'>
			   <div class='image_rotate_inner'>
			   <a  class='product-image' href='" + ApiUrl + "product/" + item.ProductName + @"/" + item.ProductId + @"'  onclick='NavigateToDetails(" + item.ProductId + @")'>
				<img 
				src='" + item.ProductImgUrl.Replace("~/", ApiUrl) + "' alt='" + item.ProductName + @"'  width='220px;' height='210px;' /></a></div>
				</div>");
                if (item.ProductDiscountPercentage != 0)
                {
                    strResult.Append(@"<div class='badge'> 			
							<span class='sale'>Yes</span>         
							</div>");
                }
                strResult.Append(@"  </div>
				<div class='outer_bottom'>
			   <h2 class='product-name'>
			   <a style='overflow:hidden;text-overflow:ellipsis;  width:225px;item last quick10'  id='prolink' >" + item.ProductName + @"</a></h2>   
				   <div class='price-box' style='text-align:center;'>");


                if (item.ProductDiscountPercentage != 0)
                {
                    strResult.Append(@"<div style='float:left; font-size:15px; color:#666666;'> <s>
				 " + currency.Symbol + "" + (currency.Value * item.ProductOriginalCost).ToSpecialFormatString() + "</s>"
  + "|<span>" + item.ProductDiscountPercentage.ToSpecialFormatString() + @"%off</span> </div>");
                }
                else
                {
                    strResult.Append(@"<div></div>");
                }


                strResult.Append(@"  <div style='align:center;'> <span style='color:#D9387E; font-size:18px; font-weight:bold;'>" + currency.Symbol + " " + (currency.Value * item.ProductCost).ToSpecialFormatString()
                    + @"</span></div>
					</div>
					  <div class='product_icons' align='center'>
<table>
<tr><td style='padding-top:10px;'><a  class='link-wishlist'  onclick='addToWishList(" + item.ProductId + @")'>Add to Wishlist</a>  </td></tr>
<tr><td></td><td></td></tr>
</table>

	
 
<ul class='add-to-links'>



</li>
 </ul></div></div></li>");

            }
            strul.Append(@" <ul class='products-grid ajaxMdl3'>
" + strResult + @"
		  </ul>");


            return new CustomResponse
            {
                Status = string.IsNullOrWhiteSpace(strResult.ToString()) ? Shared.ResponseStatus.NoData.ToString() : Shared.ResponseStatus.Success.ToString(),
                Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
                Result = strul.ToString()
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }
    }


    [HttpGet]
    public CustomResponse GetProductInfo(int productId)
    {
        try
        {
            db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();
            var data = BalUtility.GetProductInfo(productId);

            var cartdata = BalUtility.GetCart();
            var cartItems = (List<UserProductTransaction>)BalUtility.GetSession(Shared.Sessions.CustomerCart) ??
                           new List<UserProductTransaction>();
            var cartitem = (from a in cartItems where a.ProductId == productId select a).FirstOrDefault();

            var cartCost = 0;
            var cartQty = 0;

            CustomCurrency currency = (CustomCurrency)BAL.BalUtility.GetSession(Shared.Sessions.Currency);

            StringBuilder strResult = new StringBuilder();
            if (data != null)
            {
                Category Cat = Entity.Categories.Where(x => x.CategoryId == data.SubCategory.CategoryId).First();
                SuperCategory SuperCat = Entity.SuperCategories.Where(x => x.SuperCategoryId == Cat.SuperCategoryId).First();

                strResult.Append(@" <div class='breadcrumbs'> <ul>  <li class='home'> <a href='" + ApiUrl + @"' title='Go to Home Page'>Home</a>
						<span>> </span></li><li ><a href='" + ApiUrl + SuperCat.SuperCategoryName + "/" + SuperCat.SuperCategoryId + @"' title=''>" + SuperCat.SuperCategoryName + @"</a> <span>></span> </li>  
						<li ><a href='" + ApiUrl + SuperCat.SuperCategoryName + "/" + Cat.CategoryName + "/" + SuperCat.SuperCategoryId + "/" + Cat.CategoryId + @"' title=''>" + Cat.CategoryName + @"</a> <span>></span> </li>  
						 <li ><a href='" + ApiUrl + SuperCat.SuperCategoryName + "/" + Cat.CategoryName + "/" + data.SubCategory.SubCategoryName + "/" + SuperCat.SuperCategoryId + "/" + Cat.CategoryId + "/" + data.SubCategory.SubCategoryId + @"' title=''>" + data.SubCategory.SubCategoryName + @"</a> <span>> </span>
						</li><li class='product'>  <strong>" + data.ProductName + @"</strong>
									</li> </ul></div> <div class='col-main sixteen columns'>");


                strResult.Append(@"<div id='messages_product_view'></div>
<div class='product-view sixteen columns'>
  <div class='product-essential'><style>.outOfStock{opacity:0.2;-webkit-filter: blur(2px); -moz-filter: blur(2px); -o-filter: blur(2px); -ms-filter: blur(2px); filter: blur(2px);}</style>
	<div class='product-img-box ");

                if (data.Quantity == 0)
                {

                    strResult.Append(@" outOfStock");
                }
                strResult.Append("'><div class='badge'></div>");

                if (data != null && data.ProductsGalleries.Count == 0)
                {
                    strResult.Append(@"<p class='product-image product-image-zoom'> <a rel=""zoomWidth: '440',zoomHeight: '400',position: 'right',smoothMove: 3,showTitle: true,titleOpacity: 0.01,lensOpacity: 0.07,tintOpacity: 1.055,softFocus: false""  
href='" + data.ProductImgUrl.Replace("~/", ApiUrl) + @"' class='cloud-zoom group1' id='cloudZoom'>
   <img id='image' src='" + data.ProductImgUrl.Replace("~/", ApiUrl) + @"' alt=" + data.ProductName + @" title=" + data.ProductName + @"  width='300px' height='400px' /> </a></p>");
                }
                if (data != null && data.ProductsGalleries != null && data.ProductsGalleries.Count > 0)
                {
                    strResult.Append(@"<p class='product-image product-image-zoom'> <a rel=""zoomWidth: '440',zoomHeight: '400',position: 'right',smoothMove: 3,showTitle: true,titleOpacity: 0.01,lensOpacity: 0.07,tintOpacity: 1.055,softFocus: false""  
href='" + data.ProductImgUrl.Replace("~/", ApiUrl) + @"' class='cloud-zoom group1' id='cloudZoom'>
   <img id='image' src='" + data.ProductImgUrl.Replace("~/", ApiUrl) + @"' alt=" + data.ProductName + @" title=" + data.ProductName + @" /> </a></p>");

                    strResult.Append(@"
		<div class='more-views'><ul>");
                    if (data != null)
                    {
                        foreach (var item in data.ProductsGalleries)
                        {
                            var imgurl = data.ProductImgUrl.Replace("~/", ApiUrl);
                            var largeImgUrl = data.ProductImgUrl.Replace("~/", ApiUrl);

                            strResult.Append(@"<li><a href='" + largeImgUrl + "' rel=\" useZoom: 'cloudZoom', smallImage: '" + largeImgUrl + @"'"" class='cloud-zoom-gallery' title=''>
<img src='" + largeImgUrl + "'  alt=" + data.ProductName + @" width='74px' height='74px' /></a></li>");
                        }
                        strResult.Append(@" </ul></div> ");
                    }
                }
                strResult.Append(@"  
</div>
				  <div class='no-display'>
					<input type='hidden' name='product' value='21' />
					<input type='hidden' name='related_product' id='related-products-field_1' value='' />
				  </div>                  
				  <!--Prev/Next Code Start-->                  
						<div class='NextPre'  > 
<a class='prod-prev disable' href='#'>Prev</a> 
<a class='prod-next disable' href='#'>NEXT</a>
						<!--    <a class='prod-prev'>PREV</a>
									<a class='prod-next'>NEXT</a>
						  </div>                  
				  <!--Prev/Next Code End-->                  
				  <div class='product-shop'>
					<div class='product_left'>
					  <div class='product-name'>
						<h1>" + data.ProductName + @"</h1>   
			<span class='brands'><h4>Brand: <a href='" + ApiUrl + "BrandwiseProducts.aspx?Brand=" + data.Brand + "'><span style='color:#66BCDA;'>" + data.Brand + @"</span></a></h4></span>                    
							  </div>

		   <div style='line-height:16px;'>" + System.Web.HttpUtility.HtmlDecode(data.ShortDescription) + @" </div>
					  <div class='pro-left'>
						<div class='add_to_cart'>
				<div class='product-options' id='product-options-wrapper'> 
<div class='deliveredDiv'>Delivered By <a onclick='Tooltip()' class='deleveredByTooltip' style='border:1px solid #999;border-radius:2px;'>&nbsp;*&nbsp;</a></div><br />
<ul><li style='height:25px;'>• Usually Delivered in 3-8 business days.</li></ul>
");

                if (data.Free_Product_ID == null || data.Free_Product_ID == 0 && data.Is_Free_Product_Active == false)
                {

                }
                else
                {
                    using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
                    {
                        Product freeProduct = context.Products.Where(x => x.ProductId == data.Free_Product_ID).First();
                        strResult.Append(@"<div>
<div class='deliveredDiv'>Free Product </div><br />
<img id='imgFreeProduct' src='" + freeProduct.ProductImgUrl.Replace("~/", ApiUrl) + @"' style='height:100px;width:100px;' />
<br/ >
<div style='float:left; font-size:15px; color:#666666;'>" + freeProduct.ProductName + @"
<br/ >
Actual Price: <strike>" + currency.Symbol + " " + freeProduct.ProductCost + @"</strike></div>
<br/ >
</div>");
                    }
                }

                //<span><h4>Features :</h4></span>
                strResult.Append(@"<ul>");
                foreach (var featureItem in data.ProductFeatures)
                {
                    if (featureItem.FeatureInfo != "")
                        strResult.Append(@" <li>" + featureItem.FeatureInfo + "</li>");
                }
                strResult.Append(@"</ul> <dl>");

                foreach (var specificationItem in data.ProductSpecifications)
                {
                    strResult.Append(@"<dt><label class='required'><em>*</em>" + specificationItem.ProductSpecificationName +
                        @": </label></dt>
					 <dd><div class='input-box'>");
                    var str = "";
                    switch (specificationItem.SpecificationTypeId)
                    {
                        case 1:
                            str = "<input type='text' name='Spec_" + specificationItem.ProductSpecificationId + "' />";
                            strResult.Append(str);
                            break;
                        case 2:

                            if (specificationItem.ProductSpecificationNameValues.Count() > 0)
                            {
                                str = "<select id='select'  name='Spec_" + specificationItem.ProductSpecificationId + "' style='width: 115px;'>";
                                str += "<option value='Select' title='Select'>Select</option>";
                                var dropdownitems = specificationItem.ProductSpecificationNameValues.Split(',');
                                for (var i = 0; i < dropdownitems.Count(); i++)
                                {
                                    var val = dropdownitems[i];
                                    var text = dropdownitems[i];
                                    str += "<option value='" + dropdownitems[i] + "' title='" + dropdownitems[i] + "'>" + dropdownitems[i] + "</option>";
                                }
                                str += "</select>";
                            }
                            strResult.Append(str);
                            break;
                        case 3:
                            if (specificationItem.ProductSpecificationNameValues.Count() > 0)
                            {
                                str = "<ul id='navlist'  name='Spec_" + specificationItem.ProductSpecificationId + "'>";
                                var listitems = specificationItem.ProductSpecificationNameValues.Split(',');
                                for (var i = 0; i < listitems.Count(); i++)
                                {
                                    var val = listitems[i];
                                    str += "<li>" + listitems[i] + "</li>";
                                }
                                str += "</ul>";
                            }
                            strResult.Append(str);
                            break;
                        case 4:
                            str = "<input type='text' name='Spec_'" + specificationItem.ProductSpecificationId + " />";
                            strResult.Append(str);
                            break;
                        default:
                            ;
                            break;
                    }
                    strResult.Append(@" </div> </dd></dt> ");
                }
                strResult.Append(@"</dl>
			
				</div>");
                if (data.SubProducts.Count == 0 || data.Quantity == 0)
                {
                    strResult.Append(@"<div class='product-options-bottom'>
 <div class='stock_box'>              
			   <div class='price-box' style='text-align:center;'>");

                    if (data.ProductDiscountPercentage != 0)
                    {

                        strResult.Append(@"<div style='float:left; font-size:15px; color:#666666;'>  <s>" + currency.Symbol + "" + (currency.Value * data.ProductOriginalCost).ToSpecialFormatString() + "</s>"
              + "  |<span>" + data.ProductDiscountPercentage.ToSpecialFormatString() + @"%off </span> </div>");

                    }
                    else
                    {
                        strResult.Append(@"<div></div>");
                    }
                    if (cartitem != null)
                    {
                        if (cartitem.ProductId == productId)
                        {
                            strResult.Append(@" <div style='align:center;'>
<span id='lblProductcost' style='color:#D9387E; font-size:18px; font-weight:bold;'>" + currency.Symbol + "" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(data.ProductCost), true)
                        + @"</span></div>");
                        }

                    }
                    else
                    {
                        strResult.Append(@" <div style='align:center;'>
<span id='lblProductcost' style='color:#D9387E; font-size:19px; font-weight:bold;'>" + currency.Symbol + " " + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(data.ProductCost), true)
               + @"</span></div>");
                    }

                    if (data.SizeGuidePath != null)
                    {
                        strResult.Append(@"  <div style='align:center;'>
				   <div style='float:right; font-size:15px; color:#666666;'>
					 <a onclick='SizeChart(""" + data.SizeGuidePath + @""")'>Size Chart </a> <span class='size-chart-icon common-sprite fl mt3 ml5 mr5'></span></div>  </div>");
                    }

                    if (data.Quantity == 0)
                    {
                        strResult.Append(@"</div>
			 </div>
<div id='divoutofstock' style='padding-left:30px'></div>
   <table style='padding-top:5px;'><tr><td style='padding-top:30px;'> <label for='qty' style='color:#D9387E;font-weight:bold;font-size:14px;'>Notify Me :</label></td><td style='padding-top:10px;'> <div class='cart-quantity'>");
                        strResult.Append(@" 

<div class='input' style='width:300px;'>
<div id='qtinc'>
<input id='txtNUserName' style='width:300px;margin-bottom:10px;' class='input-text required-entry' validate='customValidatorGroupName' require='Please enter your First Name.' name='Email' placeholder='Name'/>
<input type='text' style='width:300px;margin-bottom:10px;' id='txtEmailId'  validate='customValidatorGroupName' class='input-text required-entry'  email='Invalid Email Address, please check and try again.' require='Please enter your  EmailId.' name='Email' value='' placeholder=' Enter EmailId' />
 <input style='width:300px;margin-bottom:10px;' class='input-text required-entry' id='txtMobileNumber'  type='text' validate='customValidatorGroupName' require='Plese Enter Valid MobileNumber' Mobile='only numbers' placeholder='Enter Mobile Number'  />
<input type='button' name='commit' style='float:left;' onclick='ValidateNotifyingDetails(); return false;' value='Submit' />
<input type='button' name='commit' style='float:right;' onclick='vpb_hide_popup_boxes()' value='Cancel' />
<br/><br/>
<!--<div id='divMsg' style=' font-weight:bold; color:#8A0000;'>
</div>-->
</div>");
                    }
                    else
                    {
                        if (cartitem != null)
                        {
                            if (cartitem.ProductId == productId)
                            {
                                strResult.Append(@"</div>
			 </div>
<div id='divoutofstock' style='padding-left:30px'></div>
   <table style='padding-top:5px;'><tr><td style='padding-top:30px;'> <label for='qty'>Quantity:</label></td><td style='padding-top:10px;'> <div class='cart-quantity'>");
                                strResult.Append(@" 

<div class='input-clicker'>
<div id='qtinc'>
<input id='txtqt' readonly='readonly' class='clicker' data-max='10.00' data-min='1' value='1' loaded='true'><!--cartitem.Quantity-->
<a id='down' class='down'  onclick='UpdateQty(" + data.ProductId + @",0,this,1)'>Down</a>
<a id='up' class='up' onclick= 'UpdateQty(" + data.ProductId + @",0,this,1)'>Up</a>
</div>
</div>");
                                if (cartitem != null)
                                {
                                    strResult.Append(@"<button type='button' id='btnbuynow' title='Add to Cart' class='button btn-cart' style='margin-top:5px;'  onclick='addToCart(" + productId + @",0,1,1)'><span><span>Buy Now</span></span></button>");
                                }
                            }
                        }
                        else
                        {
                            strResult.Append(@"</div>
			 </div>
<div id='divoutofstock' style='padding-left:30px'></div>
   <table style='padding-top:5px;'><tr><td style='padding-top:30px;'> <label for='qty'>Quantity:</label></td><td style='padding-top:10px;'> <div class='cart-quantity'>");
                            strResult.Append(@" 

<div class='input-clicker'>
<div id='qtinc'>
<input id='txtqt' readonly='readonly' class='clicker' data-max='10.00' data-min='1' value ='1' loaded='true'>
<a id='down' class='down'  onclick='UpdateQty(" + data.ProductId + @",0,this,1)'>Down</a>
<a id='up' class='up' onclick= 'UpdateQty(" + data.ProductId + @",0,this,1)'>Up</a>
</div>
</div>");

                            strResult.Append(@" </div> </td><td style='padding-top:20px; padding-left:20px;'> <div class='add-to-cart' id='btnadtocart' >");

                            if (cartdata.Count == 0)
                            {
                                strResult.Append(@"<button type='button' id='btnbuynow' title='Add to Cart' class='button btn-cart' style='margin-top:5px;'  onclick='addToCart(" + data.ProductId + @",0,1,1)'><span><span>Buy Now</span></span></button>");
                            }

                            else
                            {
                                if (cartitem != null)
                                {
                                    if (cartdata.Count != 0 && cartitem.ProductId != productId)
                                    {
                                        strResult.Append(@"<button id='btnbuynow' type='button' title='Add to Cart' class='button btn-cart' style='margin-top:5px;'  onclick='addToCart(" + productId + @",0,1,1)'><span><span>Buy Now</span></span></button>");

                                    }
                                    else
                                    {
                                        strResult.Append(@"<button id='btnbuynow' type='button' title='Add to Cart' class='button btn-cart' style='margin-top:5px;'  onclick='NavigateToMyCart()'><span><span>Buy Now</span></span></button>");
                                    }
                                }
                                else
                                {
                                    strResult.Append(@"<button type='button' id='btnbuynow' title='Add to Cart' class='button btn-cart' style='margin-top:5px;'  onclick='addToCart(" + productId + @",0,1,1)'><span><span>Buy Now</span></span></button>");
                                }
                            }
                        }
                    }


                    string id = data.IsSold.ToString();

                    strResult.Append(@"</div></td></tr></table>
				   
			</div>  </div>   </div>  <div class='pro-right'>   
			<ul class='add-to-links'>
				<li><a  onclick='addToWishList(" + data.ProductId + @")' class='link-wishlist'>Add to Wishlist</a></li>
			</ul>
						
					
					  </div>
					</div>");

                }
                else
                {
                    if (data.SubProducts.Count != 0)
                    {
                        strResult.Append(@"<div style='padding-left:30px' id='divoutofstock'>
	<div id='divoutofstock'></div>			
				</div> ");

                        strResult.Append(@"<table class='data-table' id='cart_summary'>
<thead>
			<tr>
				<th class='cart_description item hidden-phone'>Product Size</th>
				<th class='cart_quantity item'>Qty</th>
				<!--<th class='cart_quantity item'>Available Qty</th>-->
				<th class='cart_total item'>Total</th>
				<th class='cart_delete last_item'></th>
			</tr>
		</thead>
		

		<tbody>");


                    }
                    int spitemcount = 0;
                    string spfirstitem = "";
                    foreach (var spitem in data.SubProducts)
                    {
                        strResult.Append(@"<tr class='cart_item first_item address_0 odd' id='product_5_9_0_0'>

	<td class='cart_description hidden-phone' style='align:center'>
		<p class='s_title_block'><a>" + spitem.SPName + @"</a></p>
		</td>
	<td>
   <div class='cart-quantity'>
		<input type='hidden' value='1' id='txtPrvQty'>
 </div>
<div class=input-clicker>
<div id=qtinc>
<input id='txtqt' readonly='readonly' class='clicker' data-max='10.00' data-min='1' value='1' loaded='true'/>
<a id='down' class='down'  onclick='SubProdUpdateQty(" + data.ProductId + @"," + spitem.SubProductId + @",this,1)'>Down</a>
<a id='up' class='up' onclick='SubProdUpdateQty(" + data.ProductId + @"," + spitem.SubProductId + @",this,1)'>Up</a>
</div>
</div>
</td>

<!--<td class='cart_total'>
		<span style='color:#D9387E; font-weight:bold;'>" + data.Quantity + @"</label></span>
	
</td>-->
	<td class='cart_total'>
		<span style='color:#D9387E; font-weight:bold;'>" + currency.Symbol + "<label class='lblSubProd'> " + (currency.Value * spitem.ProductCost).ToSpecialFormatString() + @"</label></span>
	
</td>

			<td class='cart_delete'>
	<span style='color:#7aac4c; font-weight:bold;display:none' class='spancart'>Added To Cart</span>
<button type=button id='AddtoCartSub' title='Add to Cart' width='10' class='button btn-cart' onclick='addToCartbySubprod(" + data.ProductId + @"," + spitem.SubProductId + @",1,0,this)' ><span><span>Add To Cart</span></span></button> 
					 </div>
				</td>");


                        var val = @"  <div class=product>
							 <div class=product-details><div class=price-box style=text-align:center;>";
                        if (data.ProductDiscountPercentage != 0)
                        {

                            val = val + @"<div style=float:left; font-size:15px; color:#ffffff;> <s>" + currency.Symbol + " " + (currency.Value * spitem.ProductOriginalCost).ToSpecialFormatString() + "</s>"
              + "  |<span> " + spitem.ProductDiscountPercentage.ToSpecialFormatString() + @"%off </span> </div> ";
                        }
                        else
                        {
                            val = val + @"<div></div>";
                        }

                        val = val + @"</div> <div align=center style='display:none' id='DivTotal'><h2>
		<div style=align:center;><span>Total:</span> <span id= lblProductcost style=color:#D9387E; font-size:18px; font-weight:bold;> " + 0
                   + @"</span> &nbsp;&nbsp;&nbsp;&nbsp;<button type='button' id='btnbuynow' title='Add to Cart' width='10' class='button btn-cart' onclick='cartRedirect()'><span><span>Buy Now</span></span></button>     </div> </div> ";

                        val = val + @"</div>  
 <table><tr>
<td style=padding-top:30px; padding-left:20px;>
	<div class=product-options-bottom id=btnadtocart>";
                        if (data.Quantity != 0)
                        {
                            if (cartdata.Count != 0)
                            {

                            }
                            if (cartdata.Count == 0)
                            {

                            }

                        }

                        else
                        {
                            val = val + "   <button type=button title=Add to Cart width=10 class=button btn-cart onclick=NotifyMe()><span><span>Notify Me</span></span></button>   ";
                        }


                        val = val + @"</div></td><tr></table>
						<lable id=lblNotifysuccess></lable>
															  </div>
															</div> ";
                        if (spitemcount == 0)
                        {
                            spfirstitem = val;
                        }
                        else
                        {
                        }
                        spitemcount++;
                    }
                    strResult.Append(@"</tr></tbody></table>");
                    strResult.Append(@" </select> </div>
</br></br>
<div id='dvSubProducts' class='block-content'> " + spfirstitem + @"  </div>

</div>");
                }
                strResult.Append(@" <div class='product_right'> 
			</div>  </div> </form>");


                strResult.Append("<div class='product-collateral' id='tabs'>  <ul class='product-tabs'>  <li id='product_tabs_description' class=' active first' style='text-align:justify;line-height:16px;'>  <a href='#product_tabs_description_contents'>Description</a></li>");

                int length = data.ProductFeaturesContent.ToString().Length;
                if (length > 1)
                {
                    strResult.Append("<li id='product_tabs_tabreviews' class=''><a href='#tab-features' style='display: inline;'>Features</a> </li>");

                }
                if (data.HasReviews)
                    strResult.Append(@"<li id='product_tabs_tabreviews' class=''><a href='#customer-reviews'>Reviews</a></li> ");
                strResult.Append(@"</ul> 
						<div class='product-tabs-content' id='product_tabs_description_contents'>    <h2>Details</h2>
							<div class='std'> <ul>  <li style='text-align:justify;'>" + data.ProductDescription + @"</li>  </ul>    </div>
						</div>");
                if (data.ProductId == 1400)
                {
                    strResult.Append(@"<iframe width='310' height='210' src='https://www.youtube.com/embed/4LpI89h7FyU' frameborder='0' allowfullscreen></iframe>");
                }

                if (length > 0)
                {
                    strResult.Append("<div class='product-tabs-content' id='tab-features'>    <h2>Features</h2> <div class='std'>  <ul>  <li>" + System.Web.HttpUtility.HtmlDecode(data.ProductFeaturesContent) + @"</li>  </ul>    </div>         </div>");
                }
                if (data.HasReviews)
                {
                    strResult.Append(@"<div class='product-tabs-content' id='customer-reviews'>
							<div class='box-collateral box-reviews' id='customer-reviews'> 
<div class='form-add'>
	<h2>Write Your Own Review</h2>
		<form >
		<fieldset>
						<h3>You're reviewing: <span>" + data.ProductName + @"</span></h3>
							<ul class='form-list'>
				  
				   
					<li>
						<label for='review_field' class='required'><em>*</em>Review</label>
						<div class='input-box'>
							<textarea name='detail' id='txtReview' cols='5' rows='3' class='required-entry' validate='customReviewLoginGroupName' require='Please enter your Password.'></textarea>
						</div>
					</li>
				</ul>
			</fieldset>
			<div class='buttons-set'>
				<button type='button' title='Submit Review' class='button' onclick ='CheckReviesLogin();' ><span><span>Submit Review</span></span></button>
			</div>
	</form>
   
	</div>
											<ul>"); foreach (var reviewItem in data.ProductReviews)
                    {
                        if (reviewItem.IsApproved)
                            strResult.Append(@"<div class='review'>
By
<b>" + reviewItem.User.FirstName + @"</b>
,
<span class='easy-date'>" + reviewItem.CreatedOn + @"</span>
<br />
<p class='review-text'>" + reviewItem.Review + @"</p>
</div>");
                    }
                    strResult.Append(@"</ul></div>
						</div>");
                }
                strResult.Append(@"</div>");
                strResult.Append(@"</div>  </div>");
                if (data.RelatedProducts.Count() > 0)
                {

                    strResult.Append(@" <div class='block block-related'>
			<h2>Related Products</h2>
			  <div class='block-content'>
				<ul class='mini-products-list jcarousel-skin-tango mycarousel_related' id='block-related'>");
                    foreach (var RelatedprodItem in data.RelatedProducts)
                    {
                        if (RelatedprodItem.Product.IsDeleted == false && RelatedprodItem.Product.IsSold == false)
                        {
                            strResult.Append(@"<li class='item'>
					<div class='product'> <a  href='" + ApiUrl + "product/" + RelatedprodItem.Product.ProductName + @"/" + RelatedprodItem.RelatedProductId + @"' onclick='NavigateToDetails(" + RelatedprodItem.RelatedProductId + ")' title='" + RelatedprodItem.Product1.ProductName + @"' class='product-image'><img src='" + RelatedprodItem.Product1.ProductImgUrl.Replace("~/", ApiUrl) + @"' alt='" + RelatedprodItem.Product1.ProductName + @"'  width='220px;' height='150px;' /></a>
					  <div class='product-details'>
						<p class='product-name'><a  href='" + ApiUrl + "product/" + RelatedprodItem.Product.ProductName + @"/" + RelatedprodItem.RelatedProductId + @"' onclick='NavigateToDetails(" + RelatedprodItem.RelatedProductId + ")'>" + RelatedprodItem.Product1.ProductName + @"</a></p> 
				<div class='price-box' style='text-align:center;'>");
                        }

                        if (data.ProductDiscountPercentage != 0)
                        {
                            strResult.Append(@" <div style='float:left; font-size:15px; color:#666666;'> <s>
				 " + currency.Symbol + " " + (currency.Value * RelatedprodItem.Product1.ProductOriginalCost).ToSpecialFormatString() + "</s>"
              + " |<span> " + RelatedprodItem.Product1.ProductDiscountPercentage.ToSpecialFormatString() + @"%off  </span> </div>");
                        }
                        else
                        {
                            strResult.Append(@"<div></div>");
                        }
                        strResult.Append(@"<div style='align:center;'> <span style='color:#D9387E; font-size:19px; font-weight:bold;'>" + currency.Symbol + " " + (currency.Value * RelatedprodItem.Product1.ProductCost).ToSpecialFormatString()
          + @"</span></div>
					</div>          
						<!--            <a href='' class='link-wishlist'>Add to Wishlist</a>
						-->
					  </div>
					</div>
				  </li>");
                    }
                }


                strResult.Append(@"</ul>
			  </div>
			</div>
			 </div> </div> </div> </div> </div> </div>");


                return new CustomResponse
                {
                    Status = string.IsNullOrWhiteSpace(strResult.ToString()) ? Shared.ResponseStatus.NoData.ToString() : Shared.ResponseStatus.Success.ToString(),
                    Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
                    Result = strResult.ToString()
                };
            }
            else
            {
                return new CustomResponse
                {
                    Status = string.IsNullOrWhiteSpace(strResult.ToString()) ? Shared.ResponseStatus.NoData.ToString() : Shared.ResponseStatus.Success.ToString(),
                    Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
                    Result = strResult.ToString()
                };
            }
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }
    }

    [HttpGet]
    public int NeedHelpComments(long? HelpId, string Comments, int? status)
    {
        int i = 0;
        using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
        {
            Tbl_Need_Help_Comments comments = new Tbl_Need_Help_Comments()
            {
                Comment = Comments,
                Comment_Date = DateTime.Now,
                Help_Id = HelpId,
                Status = true
            };
            context.Tbl_Need_Help_Comments.Add(comments);

            Tbl_Need_Help need_Help = context.Tbl_Need_Help.Where(x => x.Help_Id == HelpId).FirstOrDefault();
            if (status.Value != 0)
            {
                need_Help.Status = status;
            }
            i = context.SaveChanges();
        }
        return i;
    }

    [HttpGet]
    public List<Tbl_Need_Help_Comments> getHelpComments(long? HelpId)
    {
        List<Tbl_Need_Help_Comments> comments = new List<Tbl_Need_Help_Comments>();
        using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
        {
            comments = context.Tbl_Need_Help_Comments.Where(x => x.Help_Id == HelpId).ToList();
        }
        return comments;
    }

    public dynamic GetCartList()
    {
        try
        {
            db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();

            string shippingPrice = System.Configuration.ConfigurationManager.AppSettings["ShippingCost"];
            var data = BalUtility.GetCart();
            decimal Price = 0;
            decimal Total = 0;
            StringBuilder strtrResult = new StringBuilder();
            var cartItems = (List<UserProductTransaction>)BalUtility.GetSession(Shared.Sessions.CustomerCart) ??
                                 new List<UserProductTransaction>();
            var count = 0;
            var totalsum = cartItems.Sum(p => p.ProductCost);
            var orderSummary = (Caluclations)BalUtility.GetSession(Shared.Sessions.OrderSummary);

            Caluclations caluclations = BalUtility.GenerateOrderSummary();

            foreach (var item in data)
            {
                if (item.Product.ShippingCost != 0 && item.Product.ShippingCost != null)
                {
                }
                else
                {
                    Price = Price + item.ProductCost;
                }

                var sid = item.SubProductId == null ? 0 : item.SubProductId;
                strtrResult.Append(@"<tr id='product_5_9_0_0' class='cart_item first_item address_0 odd'>
	<td class='cart_product'>
		<a href='" + ApiUrl + "product/" + item.Product.ProductName.Replace(" ", "-") + @"/" + item.ProductId + @"'><img onclick='NavigateToDetails(" + item.ProductId + ")' style='height:100px;' src='" + item.Product.ProductImgUrl.Replace("~/", ApiUrl) + "' alt='" + item.Product.ProductName + @"'></a>
	</td>
	<td class='cart_description hidden-phone'>
		<p class='s_title_block'><a href='" + ApiUrl + "product/" + item.Product.ProductName.Replace(" ", "-") + @"/" + item.ProductId + @"' onclick='NavigateToDetails(" + item.ProductId + ")'>" + item.Product.ProductName + @"</a>
</p>
<!--Brand:   <a style='cursor:default'>" + item.Product.Brand + @"</a>-->
		</td>
	<td>
   <div class='cart-quantity'>
						<input type='hidden' id='txtPrvQty' value=" + item.Quantity + @" />
<div class='input-clicker'>
<div id='qtinc'>
<input id='txtqt' readonly='readonly' class='clicker' data-max='10.00' data-min='1' value=" + item.Quantity + @" loaded='true'>
<a id='down' class='down'  onclick='UpdateQty(" + item.ProductId + @"," + sid + @",this,1)'>Down</a>
<a id='up' class='up' onclick= 'UpdateQty(" + item.ProductId + @"," + sid + @",this,1)'>Up</a>
</div>
</div>
 </div>
<div id='divoutofstock' style='padding-left:30px'>
	<div id='divoutofstock'></div>			
				</div> 
</td>
	<td class='cart_unit'>
		<span class='price' id='product_price_5_9_0'>" + item.CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.CurrencyValue * item.Product.ProductCost), true) + @"</span>
	</td>");

                strtrResult.Append(@"
	<td class='cart_total'>");
                if (item.Product.ShippingCost != 0 && item.Product.ShippingCost != null)
                {
                    Total = Total + Convert.ToDecimal(item.Quantity * item.CurrencyValue * item.Product.ProductCost + item.Product.ShippingCost);
                    strtrResult.Append("<span class='price' id='total_product_price_5_9_0'>" + item.CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.Quantity * item.CurrencyValue * item.Product.ProductCost + item.Product.ShippingCost), true) + @"</span>");
                }

                else if (Price >= 1000)
                {
                    Total = Total + Convert.ToDecimal(item.Quantity * item.CurrencyValue * item.Product.ProductCost);
                    strtrResult.Append("<span class='price' id='total_product_price_5_9_0'>" + item.CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.Quantity * item.CurrencyValue * item.Product.ProductCost), true) + @"</span>");
                }
                else
                {
                    Total = Total + Convert.ToDecimal(item.Quantity * item.CurrencyValue * item.Product.ProductCost);
                    strtrResult.Append("<span class='price' id='total_product_price_5_9_0'>" + item.CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.Quantity * item.CurrencyValue * item.Product.ProductCost), true) + @"</span>");
                }

                strtrResult.Append(@"</td>
			<td class='cart_delete'>
					 <div class='cart-remove'> 
					<button id='Button1' class='button'  onclick='deleteFromCart(" + item.ProductId + @"," + sid + @")'(" + item.ProductId + @"," + sid + @")'  return false;' type='button'><img src='../cart/delete.png' style='width:20px;' /></button>
					 </div>
				</td>
	</tr>");

            }
            StringBuilder strMsg = new StringBuilder();

            strMsg.Append(@"" + strMsg + "");
            if (cartItems.Count == 0)
            {
                return new CustomResponse
                {

                    Status = Shared.ResponseStatus.NoData.ToString(),
                    Message = CustomMessages.CartEmpty,
                    Result = strMsg.ToString()


                };
            }
            decimal ProductCostforFree = 0;
            StringBuilder strFree = new StringBuilder();

            foreach (var item in data)
            {

                if (item.Product.Free_Product_ID == 0 || item.Product.Free_Product_ID == null || item.Product.Is_Free_Product_Active == false)
                {

                }
                else
                {
                    ProductCostforFree = item.Product.ProductCost;

                    Product freeProduct = Entity.Products.Where(x => x.ProductId == item.Product.Free_Product_ID).First();

                    strFree.Append(@"<tr id='product_5_9_0_0' class='cart_item first_item address_0 odd'>
											<td class='cart_product'>
												<img style='height:100px;' src='" + freeProduct.ProductImgUrl + "' alt='" + freeProduct.ProductName + @"'>
											</td>
											<td class='cart_description hidden-phone'>
												<p class='s_title_block'>" + freeProduct.ProductName + @"
										</p>
										<!--Brand:   <a style='cursor:default'>" + freeProduct.Brand + @"</a>-->
												</td>
											<td>
										  1
										</td>
											<td class='cart_unit'>
												<span style='font-weight:bold;'>" + freeProduct.ProductCost + @"</span>
											</td>
										<td class='cart_total'> <span style='font-weight:bold;'>FREE</span>
										</td>
													<td class='cart_delete'>
 <span style='font-weight:bold;'> - </span>
										</td>
											</tr>
									   ");
                }
            }
            strtrResult.Append(strFree);


            StringBuilder strOffer = new StringBuilder();
            StringBuilder strOfferedProduct = new StringBuilder();

            if (Total - ProductCostforFree != 0)
            {
                decimal OfferCost = Total - ProductCostforFree;


                List<Tbl_Offered_Products> Prods = Entity.Tbl_Offered_Products.Where(x => x.Offer_Valid_From <= DateTime.Now && x.Offer_Valid_To >= DateTime.Now && x.Status == true && x.Offer_Cost <= OfferCost).OrderBy(x => x.Offer_Cost).ToList();

                if (HttpContext.Current.Session["OfferedProductID"] != null)
                {
                    if (Prods.Count == 0)
                    {
                        HttpContext.Current.Session["OfferedProductID"] = null;
                        strOffer.Clear();
                    }
                    else
                    {
                        int OfferedProductID = Convert.ToInt32(HttpContext.Current.Session["OfferedProductID"]);

                        Tbl_Offered_Products Products = Entity.Tbl_Offered_Products.Where(x => x.Offer_Product_ID == OfferedProductID).First();

                        strOffer.Clear();
                        strOffer.Append(@"<tr id='product_5_9_0_0' class='cart_item first_item address_0 odd'>
											<td class='cart_product'>
												<img style='height:100px;' src='" + Products.Product_Image + "' alt='" + Products.Offer_Product_Name + @"'>
											</td>
											<td class='cart_description hidden-phone'>
												<p class='s_title_block'>" + Products.Offer_Product_Name + @"
										</p>
										<!--Brand:   <a style='cursor:default'>" + Products.Product_Brand + @"</a>-->
												</td>
											<td>
										  1
										</td>
											<td class='cart_unit'>
												<span style='font-weight:bold;'>" + Products.Product_Actual_Cost + @"</span>
											</td>
										<td class='cart_total'> <span style='font-weight:bold;'>FREE</span>
										</td>
													<td class='cart_delete'>
 <span style='font-weight:bold;'> - </span>
										</td>
											</tr>
									   ");
                    }
                }
                else
                {
                    if (Prods.Count > 1)
                    {
                        strOfferedProduct.Append(@"<script type='text/javascript'>
										  $(document).ready(function () { 
											SuggestedOfferedProducts(" + Total + @");
											 });
											</script>");
                    }
                    else if (Prods.Count == 1)
                    {
                        strOffer.Clear();
                        strOffer.Append(@"<tr id='product_5_9_0_0' class='cart_item first_item address_0 odd'>
											<td class='cart_product'>
												<img style='height:100px;' src='" + Prods.First().Product_Image + "' alt='" + Prods.First().Offer_Product_Name + @"'>
											</td>
											<td class='cart_description hidden-phone'>
												<p class='s_title_block'>" + Prods.First().Offer_Product_Name + @"
										</p>
										<!--Brand:   <a style='cursor:default'>" + Prods.First().Product_Brand + @"</a>-->
												</td>
											<td>
										  1
										</td>
											<td class='cart_unit'>
												<span style='font-weight:bold;'>" + Prods.First().Product_Actual_Cost + @"</span>
											</td>
										<td class='cart_total'> <span style='font-weight:bold;'>FREE</span>
										</td>
													<td class='cart_delete'>
 <span style='font-weight:bold;'> - </span>
										</td>
											</tr>
									   ");
                    }
                }
            }
            strtrResult.Append(strOffer);


            StringBuilder strResult = new StringBuilder();
            strResult.Append(@"
<table id='cart_summary' class='std'>
<thead>
			<tr>
				<th class='cart_product first_item'>Product</th>
				<th class='cart_description item hidden-phone'>Product Name</th>
				<th class='cart_quantity item'>Qty</th>
				<th class='cart_unit item'>Unit price</th>
				<th class='cart_total item'>Total</th>
				<th class='cart_delete last_item'>Remove</th>
			</tr>
		</thead>
		
		<tbody>
					   " + strtrResult + "</tbody></table>" + strMsg + "");

            StringBuilder strItemBilling = new StringBuilder();

            if (Total < 1000)
            {
                decimal RemainingAmount = 1050 - Total;
                List<Product> Products = Entity.Products.Where(x => x.ProductCost <= RemainingAmount && x.IsActive == true && x.IsDeleted == false && x.Quantity > 0).OrderByDescending(x => x.ProductCost).ToList();
                strItemBilling.Append(@"<ul class='products-grid ajaxMdl3 cartproducts'><div id='HOOK_ItemBilling_CART' class='span8 suggestedProductsCart'>
				<form class='std' id='compare_ItemBilling' method='post' action='#'>
				<fieldset id='compare_Item'>
					<h3 style='margin-bottom:20px;'>Suggested Products <small style='float:right;'><a onclick='SuggestedProductsPopup(" + RemainingAmount + ")' style='background:#D9387E;padding:10px 25px;color:#fff;'>View more</a></small> </h3>");
                int i = 1;
                foreach (var Items in Products)
                {
                    if (i <= 3)
                    {
                        strItemBilling.Append(@"<li class='item first'><div class='outer_pan'><div class='image_rotate'>
				<div class='image_rotate_inner'><a href='#' class='product-image' onclick='NavigateToDetails(" + Items.ProductId + @")'><img src='" + Items.ProductImgUrl.Replace("~/", ApiUrl) + @"' alt='" + Items.ProductName + @"' width='100%'
				></a></div>
				<div class='outer_bottom'><h2 class='product-name'><a style='overflow:hidden;text-overflow:ellipsis;  width:225px;item last quick10' title='" + Items.ProductName + @"' id='prolink'>" + Items.ProductName + @"</a></h2>
				<div class='price-box' style='text-align:center;'><div></div><div></div>  <div style='align:center;'> <span style='color:#D9387E; font-size:18px; font-weight:bold;'>₹ " + Items.ProductCost + @"</span></div></div><div class='product_icons' align='center'>
				<table><tbody><tr><td style='padding-top:10px;'><button type='button' class='link-wishlist' onclick='addToCart(" + Items.ProductId + @",0,1,1)'>Add to Cart</button>  </td></tr><tr><td></td><td></td></tr></tbody></table><ul class='add-to-links'></ul></div></div></div></li>");
                    }
                    i++;
                }
                strItemBilling.Append(@"</fieldset>
				</form>
				</div></ul>");
            }


            strResult.Append(@" <div>
			   <font color='white' size='2'> <marquee bgcolor='Red'>* Shipping charges <b>Rs.99</b> applicable on Cash on delivery for order value below Rs.1000 *&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
* Shipping charges <b>Rs.50</b> applicable on Cash on delivery for order value Rs.1000 to Rs.2000 *&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
* Free Shipping on Cash on delivery for order value above Rs.2000 *</marquee></font>
			</div><div id='HOOK_SHOPPING_CART' style='margin-left: 0px;' class='span8'>

<form class='std' id='compare_shipping_form' method='post' action='#'>
	<fieldset id='compare_shipping'>
	 <span style='font-weight:bold; font-size:18px; padding-left:20px;'>Promocode</span><span style='color:#0088CC;'></span>
							<br/>
								<br/>
							<p style='padding-left:25px;'>
								<label for='promocode'>Promocode</label>
								  <input type='text' placeholder='Enter Promo code' id='txtPromocode' />
								<input type='button' class='button' value='Apply' onclick='Promocode();' />
								<br /><span id='lblPromocode' ></span>
<a href='PromocodeTermsConditions.aspx' style='float:right;margin-right:10px;margin-bottom:7px;'>* T&C apply</a>
					</p>                            

		<div id='carriercompare_errors' style='display: none;'>
			<ul id='carriercompare_errors_list'></ul><br>
		</div>		
		<div style='display: none;' id='SE_AjaxDisplay'>
			<img src='cart/loader.gif' alt='Loading data'><br>
			<p></p>
		</div>


<div id='availableCarriers' style=''>
		 <div id='divCarriers' style='color:Green'></div> 
</div>
		<p class='warning center' id='noCarrier' style='display: none;'>No courier has been made available for this selection.</p>
		
	</fieldset>
</form>

</div>");



            StringBuilder strPromocode = new StringBuilder();

            if (HttpContext.Current.Session["Promocode"] != null)
            {
                Tbl_Promo_Codes PromoCode = (Tbl_Promo_Codes)HttpContext.Current.Session["Promocode"];
                if (Total >= PromoCode.Minimum_Order_amount)
                {
                    strPromocode.Append(@"<tr class='cart_total_price'>
							<td>Promocode Amount </td>
							<td class='price' id='total_product'>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(PromoCode.Amount), true) + @"</td>
				  </tr>
					<tr class='cart_total_price'>
							<td>Total Amount </td>
							<td class='price' id='total_product'>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(Total - Convert.ToDecimal(PromoCode.Amount)), true) + @"</td>
				  </tr>
				");
                    totalsum = Total - PromoCode.Amount;
                }
            }
            else
            {
                strPromocode.Append(@"<tr class='cart_total_price'>
							<td>Promocode Amount </td>
							<td class='price' id='total_product'>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(0.00), true) + @"</td>
				  </tr> <tr class='cart_total_price'>
							<td>Total Amount </td>
							<td class='price' id='total_product'>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(Total - Convert.ToDecimal(0.00)), true) + @"</td>
				  </tr>");
                totalsum = Total;
            }

            strResult.Append(@"

<div class='clearfix row-fluid' id='customer_cart_total'>
		
		<div class='pull-right span4 shopCartAmount'>
			<table class='std'>
				<tbody>
															<tr class='cart_total_price'>
							<td>Sub Total</td>
							<td class='price' id='total_product'>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(Total), true) + @"</td>
				  </tr>" + strPromocode);

            if (totalsum < 1000)
            {
                strResult.Append(@"<tr class='cart_total_price'>
						<td class='price shipping_price_container' id='shipping_price_container'> <span>Shipping Charges: <br/><span style='font-size:8px;'>* Shipping Charges are Applicable <br/> for Shopping Value Below Rs.1000/-</span></span></td>
												<td class='price' id='shipping_price_container'>
							
							<span id='shipping_price'>" + cartItems.FirstOrDefault().CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(Convert.ToDecimal(shippingPrice)), true) + @"</span>
						</td>
											</tr>");

                strResult.Append(@"<tr class='cart_total_price'>
						<td class='price total_price_container' id='total_price_container'>
						<p><span>Total: </span></p>
												</td>
												<td class='price total_price_container' id='total_price_container'>
							
							<span id='total_price'>" + cartItems.FirstOrDefault().CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(totalsum + Convert.ToDecimal(shippingPrice)), true) + @"</span>
						</td>
											</tr>
<tr><td colspan='2'>( inclusive of shipping, handling and taxes. )</td></tr>
");//<tr><td colspan='2' style='line-height:16px;'>* Avail Cash on Delivery for Shopping Value of Rs.1000 and above</td></tr>

            }
            else
                if (totalsum >= 1000)
            {
                strResult.Append(@"<tr class='cart_total_price'>
						<td class='price shipping_price_container' id='shipping_price_container'>
						<span>Shipping Charges: <br/><span style='font-size:9px;'>* Shipping Charges are Applicable <br/> for Shopping Value Below Rs.1000/-</span></span>
												</td>
												<td class='price shipping_price_container' id='shipping_price_container'>
							
							<span id='shipping_price'>" + cartItems.FirstOrDefault().CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(Convert.ToDecimal(0.00)), true) + @"</span>
						</td></tr>");

                strResult.Append(@"<tr class='cart_total_price'>
						<td class='price total_price_container' id='total_price_container'>
						<p><span>Total: </span></p>
												</td>
												<td class='price total_price_container' id='total_price_container'>
							
							<span id='total_price'>" + cartItems.FirstOrDefault().CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(totalsum), true) + @"</span>
						</td>
											</tr>
<tr><td colspan='2'>( inclusive of shipping, handling and taxes. )</td></tr>
");//<tr><td colspan='2' style='line-height:16px;'>* Avail Cash on Delivery  for Shopping Value of Rs.1000 and above</td></tr>
            }
            else
            {
                strResult.Append(@"<tr class='cart_total_price'>
						<td id='cart_voucher' class='cart_voucher'>
												</td>
												<td class='price total_price_container' id='total_price_container'>
							<p>Total :</p>
							<span id='total_price'>" + cartItems.FirstOrDefault().CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(totalsum), true) + @"</span>
						</td>
											</tr>");
            }
            strResult.Append(@"</tbody>
			</table>
	</div></div> <p>
				<button style='float: right' id='ButtonContinue' class='button' onclick='ValidatePincode();' title='Continue' type='button'>
					<span>
						<span>Continue</span>
					</span>
				</button>
			</p>
");


            // strResult.Append(strFree);
            strResult.Append(strItemBilling);
            strResult.Append(strOfferedProduct);
            return new CustomResponse
            {
                Status = string.IsNullOrWhiteSpace(strResult.ToString()) ? Shared.ResponseStatus.NoData.ToString() : Shared.ResponseStatus.Success.ToString(),
                Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
                Result = strResult.ToString()
            };
        }

        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }
    }

    [HttpGet]
    public CustomResponse GetSuggestedProducts(decimal Amount)
    {
        StringBuilder strbuildr = new StringBuilder();
        var data = (CustomCurrency)BAL.BalUtility.GetSession(Shared.Sessions.Currency);
        db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();
        List<Product> Products = Entity.Products.Where(x => x.ProductCost <= Amount && x.IsActive == true && x.IsDeleted == false && x.Quantity > 0).OrderByDescending(x => x.ProductCost).ToList();
        foreach (var item in Products)
        {
            string ProductFullName = item.ProductName;
            ProductFullName = ProductFullName.Replace(" ", "-");
            if (item.ProductName.Length > 33)
            {
                item.ProductName = item.ProductName.Substring(0, 33) + "...";
            }

            strbuildr.Append(@"   
							<li class='item last quick10'>                      
							 <div class='outer_pan'>
					<div class='image_rotate'>
  <div >
				  
						<a  href='" + ApiUrl + "product/" + ProductFullName + @"/" + item.ProductId + @"' title='" + ProductFullName + "' class='product-image'><img  height='210px' src= '" + item.ProductImgUrl.Replace("~/", ApiUrl) + @"' alt='" + item.ProductName.Replace(" ", "-") + @"' /></a>
 </div>
					</div>");

            if (item.ProductDiscountPercentage != 0)
            {
                strbuildr.Append(@"<div class='badge'> 			
							<div class='ribbon'><span>Sale</span></div>        
							</div>");
            }

            strbuildr.Append(@"</div>
					 <div class='outer_bottom'>
				   <h2 class='product-name'><a  href='" + ApiUrl + "product/" + ProductFullName + @"/" + item.ProductId + @"' title='" + ProductFullName + @"'>" + item.ProductName + @"</a></h2>
			  
					<div class='price-box' style='text-align:center;'>");
            if (item.ProductDiscountPercentage != 0)
            {
                strbuildr.Append(@"<div style='float:left; font-size:13px; color:#999999;'><s>" + data.Symbol + "" + (data.Value * item.ProductOriginalCost).ToSpecialFormatString() + "</s>"
+ " |" + item.ProductDiscountPercentage.ToSpecialFormatString() + @"%off </span></div> ");
            }
            else
            {
                strbuildr.Append(@"<div></div>");
            }
            strbuildr.Append(@" <div style='align:center;'> <span style='color:#D9387E; font-size:18px; font-weight:bold;'>"
           + data.Symbol + "" + (data.Value * item.ProductCost).ToSpecialFormatString()
                                    + @"</span>
</div>
</div>
	<div class='product_icons' align='center'> 
<table><tr>
<td  style='padding-top:0px;'> <ul class='add-to-links'><li><button type='button' class='link-wishlist' onclick='addToCart(" + item.ProductId + @",0,1,1)'>Add to Cart</button></li><td></tr></table>               
	 <ul>   </ul>
					</div>
					</div>
				</li>");

        }


        return new CustomResponse
        {
            Result = strbuildr.ToString(),
            Status = "Success"
        };

    }



    [HttpGet]
    public CustomResponse GetSuggestedOfferedProducts(int Amount)
    {
        StringBuilder strbuildr = new StringBuilder();
        db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();

        List<Tbl_Offered_Products> Products = Entity.Tbl_Offered_Products.Where(x => x.Offer_Valid_From <= DateTime.Now && x.Offer_Valid_To >= DateTime.Now && x.Status == true && x.Offer_Cost <= Amount).OrderBy(x => x.Offer_Cost).ToList();

        foreach (var item in Products)
        {
            string ProductFullName = item.Offer_Product_Name;
            ProductFullName = ProductFullName.Replace(" ", "-");
            if (item.Offer_Product_Name.Length > 33)
            {
                item.Offer_Product_Name = item.Offer_Product_Name.Substring(0, 33) + "...";
            }

            strbuildr.Append(@"   
							<li class='item last quick10'>                      
							 <div class='outer_pan'>
					<div class='image_rotate'>
  <div >
				  
						<a title='" + ProductFullName + "' class='product-image'><img  height='210px' src= '" + item.Product_Image.Replace("~/", ApiUrl) + @"' alt='" + item.Offer_Product_Name.Replace(" ", "-") + @"' /></a>
 </div>
					</div>");

            strbuildr.Append(@"</div>
					 <div class='outer_bottom'>
				   <h2 class='product-name'><a title='" + ProductFullName + @"'>" + item.Offer_Product_Name + @"</a></h2>
			  
					<div class='price-box' style='text-align:center;'>");

            strbuildr.Append(@"<div></div>");

            strbuildr.Append(@" <div style='align:center;'> <span style='color:#D9387E; font-size:18px; font-weight:bold;'> FREE </span>
</div>
</div>
	<div class='product_icons' align='center'> 
<table><tr>
<td  style='padding-top:0px;'> <ul class='add-to-links'><li><button type='button' class='link-wishlist' onclick='addOfferedProduct(" + item.Offer_Product_ID + @")'>Add to Cart</button></li><td></tr></table>               
	 <ul>   </ul>
					</div>
					</div>
				</li>");

        }


        return new CustomResponse
        {
            Result = strbuildr.ToString(),
            Status = "Success"
        };

    }

    [HttpGet]
    public CustomResponse AddOfferProducttoCart(int ProductID)
    {
        HttpContext.Current.Session["OfferedProductID"] = ProductID;
        db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();

        Tbl_Offered_Products OfferProduct = Entity.Tbl_Offered_Products.Where(x => x.Offer_Product_ID == ProductID).First();

        return new CustomResponse
        {
            Status = "",
            Message = "",
            Result = OfferProduct
        };
    }


    public dynamic OrderCheckOut()
    {
        try
        {
            db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();

            string shippingPrice = System.Configuration.ConfigurationManager.AppSettings["ShippingCost"];
            var data = BalUtility.GetCart();
            decimal Price = 0;
            decimal Total = 0;
            StringBuilder strtrResult = new StringBuilder();
            var cartItems = (List<UserProductTransaction>)BalUtility.GetSession(Shared.Sessions.CustomerCart) ??
                                 new List<UserProductTransaction>();
            var count = 0;
            var totalsum = cartItems.Sum(p => p.ProductCost);
            var orderSummary = (Caluclations)BalUtility.GetSession(Shared.Sessions.OrderSummary);

            Caluclations caluclations = BalUtility.GenerateOrderSummary();
            foreach (var item in data)
            {
                if (item.Product.ShippingCost != 0 && item.Product.ShippingCost != null)
                {
                }
                else
                {
                    Price = Price + item.ProductCost;
                }
            }
            foreach (var item in data)
            {
                var sid = item.SubProductId == null ? 0 : item.SubProductId;
                strtrResult.Append(@"<tr id='product_5_9_0_0' class='cart_item first_item address_0 odd'>
	<td class='cart_product'>
		<a href='" + ApiUrl + "product/" + item.Product.ProductName.Replace(" ", "-") + @"/" + item.ProductId + @"'><img onclick='NavigateToDetails(" + item.ProductId + ")' style='height:100px;' src='" + item.Product.ProductImgUrl.Replace("~/", ApiUrl) + "' alt='" + item.Product.ProductName + @"'></a>
	</td>
	<td class='cart_description hidden-phone'>
		<p class='s_title_block'><a href='" + ApiUrl + "product/" + item.Product.ProductName.Replace(" ", "-") + @"/" + item.ProductId + @"' onclick='NavigateToDetails(" + item.ProductId + ")'>" + item.Product.ProductName + @"</a>
</p>
<!--Brand:   <a style='cursor:default'>" + item.Product.Brand + @"</a>-->
		</td>
	<td>
   <div class='cart-quantity'>
						<input type='hidden' id='txtPrvQty' value=" + item.Quantity + @" />
<div class='input-clicker'>
<div id='qtinc'>
<input id='txtqt' readonly='readonly' class='clicker' data-max='10.00' data-min='1' value=" + item.Quantity + @" loaded='true'>
<a id='down' class='down'  onclick='UpdateQty(" + item.ProductId + @"," + sid + @",this,1)'>Down</a>
<a id='up' class='up' onclick= 'UpdateQty(" + item.ProductId + @"," + sid + @",this,1)'>Up</a>
</div>
</div>
 </div>
<div id='divoutofstock' style='padding-left:30px'>
	<div id='divoutofstock'></div>			
				</div> 
</td>
	<td class='cart_unit'>
		<span class='price' id='product_price_5_9_0'>" + item.CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.CurrencyValue * item.Product.ProductCost), true) + @"</span>
	</td>");

                strtrResult.Append(@"
	<td class='cart_total'>");
                if (item.Product.ShippingCost != 0 && item.Product.ShippingCost != null)
                {
                    Total = Total + Convert.ToDecimal(item.Quantity * item.CurrencyValue * item.Product.ProductCost + item.Product.ShippingCost);
                    strtrResult.Append("<span class='price' id='total_product_price_5_9_0'>" + item.CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.Quantity * item.CurrencyValue * item.Product.ProductCost + item.Product.ShippingCost), true) + @"</span>");
                }

                else if (Price >= 1000)
                {
                    Total = Total + Convert.ToDecimal(item.Quantity * item.CurrencyValue * item.Product.ProductCost);
                    strtrResult.Append("<span class='price' id='total_product_price_5_9_0'>" + item.CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.Quantity * item.CurrencyValue * item.Product.ProductCost), true) + @"</span>");
                }
                else
                {
                    Total = Total + Convert.ToDecimal(item.Quantity * item.CurrencyValue * item.Product.ProductCost);
                    strtrResult.Append("<span class='price' id='total_product_price_5_9_0'>" + item.CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.Quantity * item.CurrencyValue * item.Product.ProductCost), true) + @"</span>");
                }

                strtrResult.Append(@"</td>
			<td class='cart_delete'>
					 <div class='cart-remove'> 
					<button id='Button1' class='button'  onclick='deleteFromCart(" + item.ProductId + @"," + sid + @")'(" + item.ProductId + @"," + sid + @")'  return false;' type='button'><img src='../cart/delete.png' style='width:20px;' /></button>
					 </div>
				</td>
	</tr>");

            }
            StringBuilder strMsg = new StringBuilder();

            strMsg.Append(@"" + strMsg + "");
            if (cartItems.Count == 0)
            {
                return new CustomResponse
                {

                    Status = Shared.ResponseStatus.NoData.ToString(),
                    Message = CustomMessages.CartEmpty,
                    Result = strMsg.ToString()


                };
            }
            decimal ProductCostForFree = 0;
            StringBuilder strFree = new StringBuilder();

            foreach (var item in data)
            {

                if (item.Product.Free_Product_ID == 0 || item.Product.Free_Product_ID == null || item.Product.Is_Free_Product_Active == false)
                {

                }
                else
                {
                    ProductCostForFree = item.Product.ProductCost;

                    Product freeProduct = Entity.Products.Where(x => x.ProductId == item.Product.Free_Product_ID).First();

                    strFree.Append(@"<tr id='product_5_9_0_0' class='cart_item first_item address_0 odd'>
											<td class='cart_product'>
												<img style='height:100px;' src='" + freeProduct.ProductImgUrl + "' alt='" + freeProduct.ProductName + @"'>
											</td>
											<td class='cart_description hidden-phone'>
												<p class='s_title_block'>" + freeProduct.ProductName + @"
										</p>
										<!--Brand:   <a style='cursor:default'>" + freeProduct.Brand + @"</a>-->
												</td>
											<td>
										  1
										</td>
											<td class='cart_unit'>
												<span style='font-weight:bold;'>" + freeProduct.ProductCost + @"</span>
											</td>
										<td class='cart_total'> <span style='font-weight:bold;'>FREE</span>
										</td>
													<td class='cart_delete'>
 <span style='font-weight:bold;'> - </span>
										</td>
											</tr>
									   ");
                }
            }
            strtrResult.Append(strFree);
            StringBuilder strOffer = new StringBuilder();
            if (Total - ProductCostForFree != 0)
            {
                decimal Offercost = Total - ProductCostForFree;
                List<Tbl_Offered_Products> Prods = Entity.Tbl_Offered_Products.Where(x => x.Offer_Valid_From <= DateTime.Now && x.Offer_Valid_To >= DateTime.Now && x.Status == true && x.Offer_Cost <= Offercost).OrderByDescending(x => x.Offer_Cost).ToList();

                if (HttpContext.Current.Session["OfferedProductID"] != null)
                {
                    int OfferedProductID = Convert.ToInt32(HttpContext.Current.Session["OfferedProductID"]);

                    Tbl_Offered_Products Products = Entity.Tbl_Offered_Products.Where(x => x.Offer_Product_ID == OfferedProductID).First();

                    strOffer.Clear();
                    strOffer.Append(@"<tr id='product_5_9_0_0' class='cart_item first_item address_0 odd'>
											<td class='cart_product'>
												<img style='height:100px;' src='" + Products.Product_Image + "' alt='" + Products.Offer_Product_Name + @"'>
											</td>
											<td class='cart_description hidden-phone'>
												<p class='s_title_block'>" + Products.Offer_Product_Name + @"
										</p>
										<!--Brand:   <a style='cursor:default'>" + Products.Product_Brand + @"</a>-->
												</td>
											<td>
										  1
										</td>
											<td class='cart_unit'>
												<span style='font-weight:bold;'>" + Products.Product_Actual_Cost + @"</span>
											</td>
										<td class='cart_total'> <span style='font-weight:bold;'>FREE</span>
										</td>
													<td class='cart_delete'>
 <span style='font-weight:bold;'> - </span>
										</td>
											</tr>
									   ");
                }
                else
                {
                    if (Prods.Count == 1)
                    {
                        HttpContext.Current.Session["OfferedProductID"] = Prods.First().Offer_Product_ID;
                        strOffer.Clear();
                        strOffer.Append(@"<tr id='product_5_9_0_0' class='cart_item first_item address_0 odd'>
											<td class='cart_product'>
												<img style='height:100px;' src='" + Prods.First().Product_Image + "' alt='" + Prods.First().Offer_Product_Name + @"'>
											</td>
											<td class='cart_description hidden-phone'>
												<p class='s_title_block'>" + Prods.First().Offer_Product_Name + @"
										</p>
										<!--Brand:   <a style='cursor:default'>" + Prods.First().Product_Brand + @"</a>-->
												</td>
											<td>
										  1
										</td>
											<td class='cart_unit'>
												<span style='font-weight:bold;'>" + Prods.First().Product_Actual_Cost + @"</span>
											</td>
										<td class='cart_total'> <span style='font-weight:bold;'>FREE</span>
										</td>
													<td class='cart_delete'>
 <span style='font-weight:bold;'> - </span>
										</td>
											</tr>
									   ");
                    }
                }
            }

            strtrResult.Append(strOffer);

            StringBuilder strResult = new StringBuilder();
            strResult.Append(@"
<table id='cart_summary' class='std'>
<thead>
			<tr>
				<th class='cart_product first_item'>Product</th>
				<th class='cart_description item hidden-phone'>Product Name</th>
				<th class='cart_quantity item'>Qty</th>
				<th class='cart_unit item'>Unit price</th>
				<th class='cart_total item'>Total</th>
				<th class='cart_delete last_item'>Remove</th>
			</tr>
		</thead>
		
		<tbody>
					   " + strtrResult + "</tbody></table>" + strMsg + "");

            StringBuilder strPromocode = new StringBuilder();

            if (HttpContext.Current.Session["Promocode"] != null)
            {
                Tbl_Promo_Codes PromoCode = (Tbl_Promo_Codes)HttpContext.Current.Session["Promocode"];
                if (Total > 1000)
                {
                    strPromocode.Append(@"<tr class='cart_total_price'>
							<td>Promocode Amount </td>
							<td class='price' id='total_product'>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(PromoCode.Amount), true) + @"</td>
				  </tr>
					<tr class='cart_total_price'>
							<td>Total Amount </td>
							<td class='price' id='total_product'>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(Total - Convert.ToDecimal(PromoCode.Amount)), true) + @"</td>
				  </tr>
				");
                    totalsum = Total - PromoCode.Amount;
                }
            }
            else
            {
                strPromocode.Append(@"<tr class='cart_total_price'>
							<td>Promocode Amount </td>
							<td class='price' id='total_product'>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(0.00), true) + @"</td>
				  </tr> <tr class='cart_total_price'>
							<td>Total Amount </td>
							<td class='price' id='total_product'>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(Total - Convert.ToDecimal(0.00)), true) + @"</td>
				  </tr>");
                totalsum = Total;
            }

            strResult.Append(@"

<div class='clearfix row-fluid' id='customer_cart_total'>
		
		<div class='pull-right span4 shopCartAmount'>
			<table class='std'>
				<tbody>
															<tr class='cart_total_price'>
							<td>Sub Total</td>
							<td class='price' id='total_product'>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(Total), true) + @"</td>
				  </tr>" + strPromocode);

            if (totalsum < 1000)
            {
                strResult.Append(@"<tr class='cart_total_price'>
						<td class='price shipping_price_container' id='shipping_price_container'> <span>Shipping Charges: <br/><span style='font-size:9px;'>* Shipping Charges are Applicable <br/> for Shopping Value Below Rs.1000/-</span></span></td>
												<td class='price' id='shipping_price_container'>
							
							<span id='shipping_price'>" + cartItems.FirstOrDefault().CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(Convert.ToDecimal(shippingPrice)), true) + @"</span>
						</td>
											</tr>");

                strResult.Append(@"<tr class='cart_total_price'>
						<td class='price total_price_container' id='total_price_container'>
						<p><span>Total: </span></p>
												</td>
												<td class='price total_price_container' id='total_price_container'>
							
							<span id='total_price'>" + cartItems.FirstOrDefault().CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(totalsum + Convert.ToDecimal(shippingPrice)), true) + @"</span>
						</td>
											</tr>
<tr><td colspan='2'>( inclusive of shipping, handling and taxes. )</td></tr>
<tr><td colspan='2' style='line-height:16px;'>* Avail Cash on Delivery  for Shopping Value of Rs.1000 and above</td></tr>
");

            }
            else
                if (totalsum >= 1000)
            {
                strResult.Append(@"<tr class='cart_total_price'>
						<td class='price shipping_price_container' id='shipping_price_container'>
						<span>Shipping Charges: <br/><span style='font-size:9px;'>* Shipping Charges are Applicable <br/> for Shopping Value Below Rs.1000/-</span></span>
												</td>
												<td class='price shipping_price_container' id='shipping_price_container'>
							
							<span id='shipping_price'>" + cartItems.FirstOrDefault().CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(Convert.ToDecimal(0.00)), true) + @"</span>
						</td></tr>");

                strResult.Append(@"<tr class='cart_total_price'>
						<td class='price total_price_container' id='total_price_container'>
						<p><span>Total: </span></p>
												</td>
												<td class='price total_price_container' id='total_price_container'>
							
							<span id='total_price'>" + cartItems.FirstOrDefault().CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(totalsum), true) + @"</span>
						</td>
											</tr>
<tr><td colspan='2'>( inclusive of shipping, handling and taxes. )</td></tr>
<tr><td colspan='2' style='line-height:16px;'>* Avail Cash on Delivery  for Shopping Value of Rs.1000 and above</td></tr>
");
            }
            else
            {
                strResult.Append(@"<tr class='cart_total_price'>
						<td id='cart_voucher' class='cart_voucher'>
												</td>
												<td class='price total_price_container' id='total_price_container'>
							<p>Total :</p>
							<span id='total_price'>" + cartItems.FirstOrDefault().CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(totalsum), true) + @"</span>
						</td>
											</tr>");
            }
            strResult.Append(@"</tbody>
			</table>
	</div></div>
");

            return new CustomResponse
            {
                Status = string.IsNullOrWhiteSpace(strResult.ToString()) ? Shared.ResponseStatus.NoData.ToString() : Shared.ResponseStatus.Success.ToString(),
                Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
                Result = strResult.ToString()
            };
        }

        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }
    }

    public dynamic GetOrderOverview()
    {
        try
        {
            string shippingPrice = System.Configuration.ConfigurationManager.AppSettings["ShippingCost"];
            decimal Price = 0;
            decimal Total = 0;
            var data = BalUtility.GetCart();
            StringBuilder strtrResult = new StringBuilder();
            var cartItems = (List<UserProductTransaction>)BalUtility.GetSession(Shared.Sessions.CustomerCart) ??
                                 new List<UserProductTransaction>();

            Caluclations cal = new Caluclations();
            var count = 0;
            var totalsum = cartItems.Sum(p => p.ProductCost);
            var orderSummary = (Caluclations)BalUtility.GetSession(Shared.Sessions.ShippingInfo);
            var calculations = BalUtility.GenerateOrderSummary();
            foreach (var item in data)
            {

                var sid = item.SubProductId == null ? 0 : item.SubProductId;
                strtrResult.Append(@"<tr id='product_5_9_0_0' class='cart_item first_item address_0 odd'>
	<td class='cart_product'>
		<a href='" + ApiUrl + "product/" + item.Product.ProductName + @"/" + item.ProductId + @"'><img onclick='NavigateToDetails(" + item.ProductId + ")' style='height:100px;' src='" + item.Product.ProductImgUrl.Replace("~/", ApiUrl) + "' alt='" + item.Product.ProductName + @"'></a>
	</td>
	<td class='cart_description hidden-phone'>
		<p class='s_title_block'><a href='" + ApiUrl + "product/" + item.Product.ProductName + @"/" + item.ProductId + @"' onclick='NavigateToDetails(" + item.ProductId + ")'>" + item.Product.ProductName + @"</a></p>
Brand:   <a>" + item.Product.Brand + @"</a>
		</td>");
                strtrResult.Append(@"<td>
   <div class='cart-quantity'>
						<input type='hidden' id='txtPrvQty' value=" + item.Quantity + @" />
<div class='input-clicker'>
<div id='qtinc'>
<input id='txtqt' readonly='readonly' class='clicker' data-max='10.00' data-min='1' value=" + item.Quantity + @" loaded='true'>
<a id='down' class='down'  onclick='UpdateQty(" + item.ProductId + @"," + sid + @",this,1)'>Down</a>
<a id='up' class='up' onclick= 'UpdateQty(" + item.ProductId + @"," + sid + @",this,1)'>Up</a>
</div>
</div>
 </div> 
				
				
						</td>
	
	<td class='cart_unit'>
		<span class='price' id='product_price_5_9_0'>" + item.CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.CurrencyValue * item.Product.ProductCost), true) + @"</span>
	</td>");

                if (item.Product.ShippingCost != 0 && item.Product.ShippingCost != null)
                {
                    Total = Total + Convert.ToDecimal(item.Quantity * item.CurrencyValue * item.Product.ProductCost + item.Product.ShippingCost);
                    strtrResult.Append("<td><span class='price' id='total_product_price_5_9_0'>" + item.CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.Quantity * item.CurrencyValue * item.Product.ProductCost + item.Product.ShippingCost), true) + @"</span></td>");
                }
                else
                    if (Price >= 1000)
                {
                    Total = Total + Convert.ToDecimal(item.Quantity * item.CurrencyValue * item.Product.ProductCost);
                    strtrResult.Append("<td><span class='price' id='total_product_price_5_9_0'>" + item.CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.Quantity * item.CurrencyValue * item.Product.ProductCost), true) + @"</span></td>");
                }
                else
                {
                    Total = Total + Convert.ToDecimal(item.Quantity * item.CurrencyValue * item.Product.ProductCost);
                    strtrResult.Append("<td><span class='price' id='total_product_price_5_9_0'>" + item.CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.Quantity * item.CurrencyValue * item.Product.ProductCost), true) + @"</span></td>");
                }
                strtrResult.Append(@"<td class='cart_delete'>
					 <div class='cart-remove'> 
					<button id='Button1' class='button'  onclick='deleteFromCart(" + item.ProductId + @"," + sid + @")'  return false;' type='button'><img src='../cart/delete.png' style='width:20px' /></button>
					 </div>
				</td>
	</tr>");

            }
            StringBuilder strMsg = new StringBuilder();

            strMsg.Append(@"" + strMsg + "");
            if (cartItems.Count == 0)
            {
                return new CustomResponse
                {

                    Status = Shared.ResponseStatus.NoData.ToString(),
                    Message = CustomMessages.CartEmpty,
                    Result = strMsg.ToString()


                };




            }
            StringBuilder strResult = new StringBuilder();
            strResult.Append(@"

<table id='cart_summary' class='std'>
<thead>
			<tr>
				<th class='cart_product first_item'>Product</th>
				<th class='cart_description item hidden-phone'>Product Name</th>
				<th class='cart_quantity item'>Qty</th>
				<th class='cart_unit item'>Unit price</th>
				<!--<th class='cart_unit item'>Shipping Charges</th>-->
				<th class='cart_total item'>Total</th>
				<th class='cart_delete last_item'>Remove</th>
			</tr>
		</thead>
		
		<tbody>
					   " + strtrResult + "</tbody></table>" + strMsg + "");

            strResult.Append(@"
<div class='clearfix row' id='customer_cart_total'>
		<div id='HOOK_SHOPPING_CART' style='margin-left: 0px;' class='span8'>

<form class='std' id='compare_shipping_form' method='post' action='#'>
	<fieldset id='compare_shipping'>
		<div id='carriercompare_errors' style='display: none;'>
			<ul id='carriercompare_errors_list'></ul><br>
		</div>		
		<div style='display: none;' id='SE_AjaxDisplay'>
			<img src='cart/loader.gif' alt='Loading data'><br>
			<p></p>
		</div>


<div id='availableCarriers' style=''>
		
</div>
		<p class='warning center' id='noCarrier' style='display: none;'>No courier has been made available for this selection.</p>
		
	</fieldset>
</form>

</div>
		<div class='span4'>
			<table class='std'>
				<tbody>
															<tr class='cart_total_price'>
							<td>Total Amount</td>
							<td class='price' id='total_product'>" + cartItems.FirstOrDefault().CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(Total), true) + @"</td>
						</tr>");

            if (totalsum < 1000)
            {
                strResult.Append(@"<tr class='cart_total_price'>
						<td class='price shipping_price_container' id='shipping_price_container'> <span>Shipping Charges: <br/><br/>
						<span style='font-size:8px; color:#666666;'>* Shipping Charges are Applicable <br/> for Shopping Value Below Rs.1000/-</span></span></td>
												<td class='price' id='shipping_price_container'>
							
							<span id='shipping_price'>" + cartItems.FirstOrDefault().CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(Convert.ToDecimal(shippingPrice)), true) + @"</span>
						</td>
											</tr>");

                strResult.Append(@"<tr class='cart_total_price'>
						<td class='price total_price_container' id='total_price_container'>
						<p><span>Total: </span></p>
												</td>
												<td class='price total_price_container' id='total_price_container'>
							
							<span id='total_price'>" + cartItems.FirstOrDefault().CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(Total + Convert.ToDecimal(shippingPrice)), true) + @"</span>
						</td>
											</tr>
<tr><td colspan='2'>( inclusive of shipping, handling and taxes. )</td></tr>
<tr><td colspan='2' style='line-height:16px;'>* Avail Cash on Delivery  for Shopping Value of Rs.1000 and above</td></tr>
");

            }
            else
                if (totalsum > 1000)
            {
                strResult.Append(@"<tr class='cart_total_price'>
						<td class='price shipping_price_container' id='shipping_price_container'>
						<span>Shipping Charges: <br/><span style='font-size:9px;'>* Shipping Charges are Applicable <br/> for Shopping Value Below Rs.1000/-</span></span>
												</td>
												<td class='price shipping_price_container' id='shipping_price_container'>
							
							<span id='shipping_price'>" + cartItems.FirstOrDefault().CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(Convert.ToDecimal(0.00)), true) + @"</span>
						</td>
											</tr>");

                strResult.Append(@"<tr class='cart_total_price'>
						<td class='price total_price_container' id='total_price_container'>
						<p><span>Total: </span></p>
												</td>
												<td class='price total_price_container' id='total_price_container'>
							
							<span id='total_price'>" + cartItems.FirstOrDefault().CurrencySymbol + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(Total), true) + @"</span>
						</td>
											</tr>
<tr><td colspan='2'>( inclusive of shipping, handling and taxes. )</td></tr>
<tr><td colspan='2' style='line-height:16px;'>* Avail Cash on Delivery  for Shopping Value of Rs.1000 and above</td></tr>
");
            }

            return new CustomResponse
            {
                Status = string.IsNullOrWhiteSpace(strResult.ToString()) ? Shared.ResponseStatus.NoData.ToString() : Shared.ResponseStatus.Success.ToString(),
                Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
                Result = strResult.ToString()
            };
        }

        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }
    }

    public CustomResponse PincodeAvailabilityCheck(string Pincode)
    {
        try
        {
            double Zipcode = Convert.ToDouble(Pincode);
            var Repository = new PinCodeInfoRepository();
            var data = Repository.First(P => P.PinCode == Zipcode);
            if (data != null)
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Result = ""
                };
            }
            else
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.NoData.ToString(),
                    //Message = "No courier has been made available for this selection.",
                    Result = null
                };
            }
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = null
            };
            throw ex;
        }

    }

    public CustomResponse Estimateshipping(string Pincode)
    {
        try
        {
            double ZipCode = Convert.ToDouble(Pincode);

            var Repository = new PinCodeInfoRepository();
            var data = Repository.First(P => P.PinCode == ZipCode);
            var cartItems = (List<UserProductTransaction>)BalUtility.GetSession(Shared.Sessions.CustomerCart) ??
                          new List<UserProductTransaction>();
            var pincodeInfo = BalUtility.GetpincodeInfo(Pincode);
            var orderSummary = BalUtility.GenerateOrderSummary();
            var ShippingInfo = (Caluclations)BalUtility.GetSession(Shared.Sessions.ShippingInfo);
            var totalsum = cartItems.Sum(p => p.ProductCost);
            if (data != null)
            {
                StringBuilder strresult = new StringBuilder();
                strresult.Append(@"Hello");
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Result = strresult.ToString(),
                    ShippingAmount = ShippingInfo.ShippingCharges,
                    CartSum = cartItems.FirstOrDefault().CurrencySymbol + ShippingInfo.TotalPriceAfterDeductions,
                    Message = "Delivered by " + DateTime.Now.AddDays(9).ToLongDateString()
                };
            }
            else
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.NoData.ToString(),
                    // Message = "No courier has been made available for this selection.",
                    Result = null
                };
            }


        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = null
            };
            throw ex;
        }

    }
    [HttpGet]
    public CustomResponse GetQuckInfo(int productId)
    {
        var repository = new ProductRepository();
        int totalRecords;

        var data = repository.FetchAllByPage(p => p.ProductId, out totalRecords, 0, 0,
                                                   (p => p.IsSold == false &&
                                                    p.IsActive && p.IsDeleted == false && p.SubCategory.Category.IsActive && p.SubCategory.IsActive &&
                                                    p.ProductId == productId), null, "ProductFeatures,SpecificationType,ProductSpecifications,ProductsGalleries,RelatedProducts,SubProducts").FirstOrDefault();


        StringBuilder strResult = new StringBuilder();
        strResult.Append(@"<div class='product-view' style='width:580px'>
	<div class='product-essential' style='width:580px'>
	<form id='product_addtocart_form' >
		<div class='no-display'>
			<input type='hidden' name='product' value='20' />
			<input type='hidden' name='related_product' id='related-products-field' value='' />
		</div>

	
		<div class='quick_product'>
			<div class='product-name'>
				<h1>" + data.ProductName + @"</h1>
			</div>
			<div class='product-img-box'>");
        if (data.ProductDiscountPercentage != 0)
        {
            strResult.Append(@"<div class='badge'> 			
									
							</div>");
        }
        strResult.Append(@"<div class='badge'>			
						
									</div>");
        if (data != null && data.ProductsGalleries != null && data.ProductsGalleries.Count > 0)
        {
            strResult.Append(@" <p class='product-image-zoom'>
	<img id='image' src='" + data.ProductImgUrl.Replace("~/", ApiUrl) + @"' alt=" + data.ProductName + @" title=" + data.ProductName + @" onclick='javascript:void(0);' />  
<img id='slide-loader' src='" + data.ProductImgUrl.Replace("~/", ApiUrl) + @"' />
</p>
");
        }
        strResult.Append(@" <div class='more-views'> <ul>");

        if (data != null)
            foreach (var item in data.ProductsGalleries)
            {
                var imgurl = item.ImgUrl.Replace("~/", ApiUrl);
                var largeImgUrl = item.LargeImgUrl.Replace("~/", ApiUrl);
                strResult.Append(@"
	
			<li>
		   <a href='" + largeImgUrl + "' rel=\" useZoom: 'cloudZoom', smallImage: '" + largeImgUrl + @"'"" class='cloud-zoom-gallery' title=''>
<img src='" + largeImgUrl + "'  alt=" + data.ProductName + @" width='74px' height='74px' /></a>
		</li>
		   ");
            }
        strResult.Append("</ul></div>");

        strResult.Append(@" </div>
			 <div class='quick_right pro-right tabs'>
			 <ul class='tabNavigation'>
				<li><a href='#proDetail' class='selected'>Overview</a></li>
				<li><a href='#prodescription'class='' >description</a></li>
			  </ul>
			 
		  <div id='prodescription' style='display: none;' >
			  <div class='short-description' >
			  <h2>Quick Overview</h2>
			 <div class='std'> <ul>
	 <li>" + data.ProductDescription + @"</li>
  
</ul></div>
			  </div>
		   </div>   
		  
		  <div id='proDetail'>
			<div class='rating_pan' id='target_rating'>   
<p class='no-rating'><a href='#product_tabs_tabreviews'>Be the first to review this product</a></p>
</div>
	<div class='price-box'>
	<span class='regular-price' id=" + data.ProductId + @">
   <span class='price'>" + data.ProductCost + @"</span></span>
						
		</div>

	<p class='availability in-stock'></p>
			<div class='quick_cart'>
			<div class='product-options' id='product-options-wrapper'>");
        strResult.Append(@"<ul>");
        foreach (var featureItem in data.ProductFeatures)
        {
            if (featureItem.FeatureInfo != "")
                strResult.Append(@" <li>" + featureItem.FeatureInfo + "</li>");
        }
        strResult.Append(@"</ul> <dl>");

        foreach (var specificationItem in data.ProductSpecifications)
        {
            strResult.Append(@"<dt><label class='required'><em>*</em>" + specificationItem.ProductSpecificationName +
                @": </label></dt>
					 <dd><div class='input-box'>");
            var str = "";
            switch (specificationItem.SpecificationTypeId)
            {

                case 1:
                    str = "<input type='text' name='Spec_" + specificationItem.ProductSpecificationId + "' />";
                    strResult.Append(str);
                    break;
                case 2:

                    if (specificationItem.ProductSpecificationNameValues.Count() > 0)
                    {
                        str = "<select id='select'  name='Spec_" + specificationItem.ProductSpecificationId + "' style='width: 115px;'>";
                        str += "<option value='Select' title='Select'>Select</option>";
                        var dropdownitems = specificationItem.ProductSpecificationNameValues.Split(',');
                        for (var i = 0; i < dropdownitems.Count(); i++)
                        {
                            var val = dropdownitems[i];
                            var text = dropdownitems[i];
                            str += "<option value='" + dropdownitems[i] + "' title='" + dropdownitems[i] + "'>" + dropdownitems[i] + "</option>";
                        }
                        str += "</select>";
                    }
                    strResult.Append(str);
                    break;
                case 3:
                    if (specificationItem.ProductSpecificationNameValues.Count() > 0)
                    {
                        str = "<ul id='navlist'  name='Spec_" + specificationItem.ProductSpecificationId + "'>";
                        var listitems = specificationItem.ProductSpecificationNameValues.Split(',');
                        for (var i = 0; i < listitems.Count(); i++)
                        {
                            var val = listitems[i];
                            str += "<li>" + listitems[i] + "</li>";
                        }
                        str += "</ul>";
                    }
                    strResult.Append(str);
                    break;
                case 4:
                    str = "<input type='text' name='Spec_'" + specificationItem.ProductSpecificationId + " />";
                    strResult.Append(str);
                    break;
                default:
                    ;
                    break;

            }
            strResult.Append(@" </div> </dd></dt> ");
        }

        strResult.Append(@" 
	</div>
<script type='text/javascript'>decorateGeneric($$('#product-options-wrapper dl'), ['last']);</script>
<div class='product-options-bottom'>
		<div class='add-to-cart'>
				<label for='qty'>Qty:</label>
		<div class='qty_pan'>
		<input type='text' name='qty' id='qty' maxlength='12' value='1' title='Qty' class='input-text qty' />
		</div>
				<button type='button' title='Add to Cart' class='button btn-cart' onclick='productAddToCartForm.submit(this)'><span><span>Add to Cart</span></span></button>
		<span id='ajax_loader' style='display:none'><img src='../../../../../../skin/frontend/default/megashop-pink/images/opc-ajax-loader.gif'/></span>
		
			</div>

</div>
				</div>
		  </div>
				<div class='view_product'><a target='_parent' href='../../../../../htc-one-1120.html' title=''>View Product Details</a></div>
		</div>
			
	   
		</div>
		<div class='clearer'></div>
	</form>



		</div>
</div>");
        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Message = "",
            Result = strResult.ToString()

        };

    }
    [HttpPost]
    public string CreateProduct(Product data)
    {
        try
        {
            var repository = new ProductRepository();
            repository.Insert(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpPost]
    public string EditProduct(Product data)
    {
        try
        {
            var repository = new ProductRepository();
            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpGet]
    public string DeleteProduct(int id, string name)
    {
        try
        {
            var repository = new ProductRepository();
            long ProductId = Convert.ToInt32(name);
            var data = repository.First(p => p.ProductId == id);
            data.IsDeleted = true;
            data.IsActive = false;

            // data.Quantity = 0;
            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    [HttpGet]
    public string ActiveProduct(int id, string name)
    {
        try
        {
            var repository = new ProductRepository();
            long ProductId = Convert.ToInt32(name);
            var data = repository.First(p => p.ProductId == id);
            data.IsDeleted = false;
            data.IsActive = true;
            // data.Quantity = 0;
            repository.Update(data);
            //Response.redirect("updateproduct.aspx");
            return "Success";



        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }


    #endregion

    [HttpGet]
    public string DeleteReviews(int id)
    {
        try
        {
            var repository = new ProductReviewsRepository();
            var data = repository.First(p => p.ReviewId == id);
            data.IsDeleted = 1;
            // data.Quantity = 0;
            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }




    public CustomResponse UpdateSoldoutProduct()
    {
        var cartItems = (List<UserProductTransaction>)BalUtility.GetSession(Shared.Sessions.CustomerCart) ??
                                   new List<UserProductTransaction>();
        var repository = new ProductRepository();
        Product product = null;

        foreach (var item in cartItems)
        {
            if (item.ProductId != 0)
            {

                var repository2 = new UserProductTransactionRepository();
                int totalRecords;
                var data = repository2.FetchAllByPage(p => p.ProductId, out totalRecords, 0, 0, (p => p.ProductId == item.ProductId));
                var soldoutQty = data.Sum(p => p.Quantity);

                product = repository.Single(p => p.ProductId == item.ProductId);

                var availableQuantity = product.Quantity - soldoutQty;

                if (availableQuantity == 0)
                {

                    product.IsSold = true;
                    repository.Update(product);
                }

            }
            BalUtility.ClearSession(Shared.Sessions.CustomerCart);
        }
        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Message = "",
            Result = null
        };
    }

    #region User Products Info

    [HttpPost]
    public string OrderProducts(ICollection<UserProductTransaction> data)
    {
        try
        {
            var repository = new UserProductTransactionRepository();
            repository.Insert(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    #endregion

    #region EditProductFeatures
    public dynamic GetproductFeaturesList(string id, string sidx, string sord, int page, int rows)
    {
        var repository = new ProductFeatureRepository();
        int totalRecords;
        long ProductId = Convert.ToInt32(id);
        var data = repository.FetchAllByPage(p => p.ProductFeatureId, out totalRecords, (page - 1) * rows, rows,
                                             (p => p.ProductId == ProductId && p.IsActive && p.IsDeleted == false));
        foreach (var States in data)
        {
            //States.CountryName = States.Country.CountryName;
        }

        return new
        {
            total = totalRecords / rows + 1,
            page,
            records = totalRecords,
            rows = data
        };
    }


    [HttpPost]
    public string CreateProductFeatures(string id, ProductFeature data)
    {
        try
        {
            var repository = new ProductFeatureRepository();
            repository.Insert(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpPost]
    public string EditProductFeatures(ProductFeature data)
    {
        try
        {
            var repository = new ProductFeatureRepository();
            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpGet]
    public string DeleteProductFeatures(int id, string name)
    {
        try
        {
            var repository = new ProductFeatureRepository();
            long ProductFeatureId = Convert.ToInt32(name);
            var data = repository.First(p => p.ProductFeatureId == ProductFeatureId);
            data.IsDeleted = true;
            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    #endregion

    #region EditProductSpecifications
    public dynamic GetproductSpecificationsList(string id, string sidx, string sord, int page, int rows)
    {
        var repository = new ProductSpecificationRepository();
        int totalRecords;
        long ProductId = Convert.ToInt32(id);
        var data = repository.FetchAllByPage(p => p.ProductSpecificationId, out totalRecords, (page - 1) * rows, rows,
                                             (p => p.ProductId == ProductId && p.IsActive && p.IsDeleted == false));
        foreach (var States in data)
        {
            //States.CountryName = States.Country.CountryName;
        }

        return new
        {
            total = totalRecords / rows + 1,
            page,
            records = totalRecords,
            rows = data
        };
    }


    [HttpPost]
    public string CreateProductSpecifications(string id, ProductSpecification data)
    {
        try
        {
            var repository = new ProductSpecificationRepository();
            repository.Insert(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpPost]
    public string EditProductSpecifications(ProductSpecification data)
    {
        try
        {
            var repository = new ProductSpecificationRepository();
            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpGet]
    public string DeleteProductSpecifications(int id, string name)
    {
        try
        {
            var repository = new ProductSpecificationRepository();
            long ProductSpecificationId = Convert.ToInt32(name);
            var data = repository.First(p => p.ProductSpecificationId == ProductSpecificationId);
            data.IsDeleted = true;
            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    #endregion

    #region EditProductsGallery
    public dynamic GetProductsGalleryList(string id, string sidx, string sord, int page, int rows)
    {
        var repository = new ProductsGalleryRepository();
        int totalRecords;
        long ProductId = Convert.ToInt32(id);
        var data = repository.FetchAllByPage(p => p.ProductGalleryId, out totalRecords, (page - 1) * rows, rows,
                                             (p => p.ProductId == ProductId && p.IsActive && p.IsDeleted == false));
        foreach (var States in data)
        {
            //States.CountryName = States.Country.CountryName;
        }

        return new
        {
            total = totalRecords / rows + 1,
            page,
            records = totalRecords,
            rows = data
        };
    }


    [HttpPost]
    public string CreateProductsGallery(string id, ProductsGallery data)
    {
        try
        {
            var repository = new ProductsGalleryRepository();
            repository.Insert(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpPost]
    public string EditProductsGallery(ProductsGallery data)
    {
        try
        {
            var repository = new ProductsGalleryRepository();
            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpGet]
    public string DeleteProductsGallery(int id, string name)
    {
        try
        {
            var repository = new ProductsGalleryRepository();
            long ProductGalleryId = Convert.ToInt32(name);
            var data = repository.First(p => p.ProductGalleryId == ProductGalleryId);
            data.IsDeleted = true;
            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    #endregion

    #region UsersGrid

    public dynamic GetUsersList(string sidx, string sord, int page, int rows)
    {
        var repository = new UserRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.UserId, out totalRecords, (page - 1) * rows, rows, (p => p.IsActive && p.IsDeleted == false));

        return new
        {
            total = totalRecords / rows + 1,
            page,
            records = totalRecords,
            rows = data
        };
    }

    public string GetUsersListAsHtml()
    {
        var repository = new UserRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.UserId, out totalRecords, 0, 0, (p => p.IsActive && p.IsDeleted == false));

        string customersHtml = data.Aggregate(" <select>", (current, item) => current + string.Format("<option value={0}>{1}</option>", item.RoleId, item.FirstName));
        customersHtml += "</select>";
        return customersHtml;
    }

    [HttpPost]
    public string CreateUsers(User data)
    {
        try
        {
            var repository = new UserRepository();
            repository.Insert(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpPost]
    public CustomResponse EditPassword(User data)
    {
        try
        {
            var repository = new UserRepository();
            data.MiddleName = "";
            var objUser = repository.Single(p => p.UserId == data.UserId);
            objUser.PassCode = data.PassCode;

            var result = repository.Update(objUser);

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Message = "",
                Result = objUser
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }
    }


    [HttpPost]
    public CustomResponse EditUsers(User data)
    {
        try
        {
            var repository = new UserRepository();
            data.MiddleName = "";
            var objUser = repository.Single(p => p.UserId == data.UserId);
            objUser.LastName = data.LastName;
            objUser.FirstName = data.FirstName;
            objUser.EmailId = data.EmailId;
            objUser.MobileNo = data.MobileNo;
            var result = repository.Update(objUser);

            //   data.MiddleName="";
            //   data.PassCode = "123456";
            //   var repository = new UserRepository();
            //var result=   repository.Update(data);

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Message = "",
                Result = objUser
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }
    }

    [HttpGet]
    public string DeleteUsers(int id, string name)
    {
        try
        {
            var repository = new UserRepository();
            long UserId = Convert.ToInt32(name);
            var data = repository.First(p => p.UserId == UserId);
            data.IsDeleted = true;
            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    #endregion


    //added by gopi for deleting pin
    [HttpGet]
    public string DeletePin(int id)
    {
        try
        {
            var repository = new PinCodesInformationRepository();
            var data = repository.First(p => p.PinCodeId == id);
            data.IsActive = false;
            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    //end deletepin




    #region UserProductTransactionsGrid

    public dynamic GetUserProductTransactionList(string id, string sidx, string sord, int page, int rows)
    {
        var repository = new PaymentTransactionRepository();
        int totalRecords;
        long UserId = Convert.ToInt64(id);
        var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, (page - 1) * rows, rows, (p => p.UserId == UserId), null, "", true);


        var filteredData = (from q in (data as List<PaymentTransaction>)
                            select new
                            {
                                q.PaymentTransactionId,
                                Quantity = q.UserProductTransactions.Count(),
                                q.PaymentStatus,
                                q.TxnAmount,
                                q.TxnRefNo,
                                q.TxnStatus,
                                currency = q.CurrencyCode + " ( " + q.CurrencySymbol + " ) ",
                                q.CurrencyCode,
                                q.CurrencySymbol,
                                q.PGTxnId,
                                q.TxnMessage,
                                q.CreatedOn,
                                products = q.UserProductTransactions.Where(cat => cat.PaymentTransactionId == q.PaymentTransactionId).
                                  Select(u => string.Join(",", u.Product.ProductName))

                            }).ToList();



        return new
        {
            total = totalRecords / rows + 1,
            page,
            records = totalRecords,
            rows = filteredData
        };
    }


    #region forgotpassword from userside
    [HttpPost]
    public dynamic SendForgotpassword(string EmailId)
    {
        var repository = new UserRepository();
        var data = repository.Find(x => x.EmailId == EmailId).First();
        var PaymntDtls = repository.First(p => p.EmailId == data.EmailId);
        Utility.MailMessage ms = new Utility.MailMessage();
        ms.Subject = "Recover Password";
        ms.To = PaymntDtls.EmailId;

        string domain;
        domain = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port;

        string body = string.Empty;
        string htmlPagePath = Shared.GethtmlPage(Shared.ProductStatusForSendMail.password);
        using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("" + htmlPagePath + "")))
        {
            body = reader.ReadToEnd();
        }

        body = body.Replace("##FirstName##", PaymntDtls.FirstName);
        body = body.Replace("##EmailId##", EmailId);
        body = body.Replace("##Password##", data.PassCode);
        body = body.Replace("##domain##", domain);
        ms.Body = body;

        ms.IsBodyHtml = true;
        try
        {
            ms.SendMail();
        }
        catch (Exception ex)
        {

        }
        return new
        {
            Status = "Success",
            Message = "Password has been sent to your registered email address"
        };
    }

    #endregion

    //#region admin approvereviews
    //  [HttpGet]
    //public CustomResponse GetProductReviews()
    //{
    //    var repository = new ProductReviewsRepository();
    //    int totalRecords;
    //    var data = repository.FetchAllByPage(p => p.ReviewId, out  totalRecords, 0, 0,null ,c => new {c.UserId, c.ReviewId, c.Review, c.IsApproved,c.ProductId }).ToList();

    //    if (data != null)
    //    {

    //        var resultdata = data.Select(x => new{x.ReviewId,x.Review,x.IsApproved });
    //    }
    //    return new CustomResponse
    //    {
    //        Status = Shared.ResponseStatus.Success.ToString(),
    //        Message = "",
    //        Result = data

    //    };
    //}
    //#endregion

    public string GetUserProductTransactionListAsHtml()
    {
        var repository = new UserProductTransactionRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.TransactionId, out totalRecords, 0, 0, (p => p.IsActive && p.IsDeleted == false));

        string customersHtml = data.Aggregate(" <select>", (current, item) => current + string.Format("<option value={0}>{1}</option>", item.TransactionId, item.Quantity));
        customersHtml += "</select>";
        return customersHtml;
    }

    [HttpPost]
    public string CreateUserProductTransaction(UserProductTransaction data)
    {
        try
        {
            var repository = new UserProductTransactionRepository();
            repository.Insert(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpPost]
    public string EditUserProductTransaction(UserProductTransaction data)
    {
        try
        {
            var repository = new UserProductTransactionRepository();
            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpGet]
    public string DeleteUserProductTransaction(int id, string name)
    {
        try
        {
            var repository = new UserProductTransactionRepository();
            long TransactionId = Convert.ToInt32(name);
            var data = repository.First(p => p.TransactionId == TransactionId);
            data.IsDeleted = true;
            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    #endregion

    #region MonthlyWiseUserProductTransactionsGrid


    public dynamic GetMonthlyWiseUserProductTransactionList(string sidx, string sord, int page, int rows, DateTime fromDate, DateTime toDate, int OrderIdTxt, string UsrMailIDTxt)
    {
        fromDate = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, 0, 0, 1);
        toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);
        int OrdrId = OrderIdTxt;
        string UserEmailID = UsrMailIDTxt;
        var repository = new PaymentTransactionRepository();
        int totalRecords;

        var data = OrdrId == 0 ? (UserEmailID == null ? (repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, (page - 1) * rows, rows, (p => p.CreatedOn >= fromDate && p.CreatedOn <= toDate), null, "", true)) :
            (repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, (page - 1) * rows, rows, (p => p.User.EmailId == UserEmailID), null, "", true)))
            : (repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, (page - 1) * rows, rows, (p => p.PaymentTransactionId == OrdrId), null, "", true));


        var filteredData = (from q in (data as List<PaymentTransaction>)
                            select new
                            {
                                q.PaymentTransactionId,
                                Quantity = q.UserProductTransactions.Count(),
                                q.PaymentStatus,
                                q.TxnAmount,
                                q.TxnRefNo,
                                q.TxnStatus,
                                currency = q.CurrencyCode + " ( " + q.CurrencySymbol + " ) ",
                                q.CurrencyCode,
                                q.CurrencySymbol,
                                q.PGTxnId,
                                q.TxnMessage,
                                q.CreatedOn,
                                products = q.UserProductTransactions.Where(cat => cat.PaymentTransactionId == q.PaymentTransactionId).
                                  Select(u => string.Join(",", u.Product.ProductName))

                            }).ToList();
        if (filteredData == null)
        {
        }

        return new
        {
            total = totalRecords / rows + 1,
            page,
            records = totalRecords,
            rows = filteredData
        };
    }

    public string GetMonthlyWiseUserProductTransactionListAsHtml()
    {
        var repository = new UserProductTransactionRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.TransactionId, out totalRecords, 0, 0, (p => p.IsActive && p.IsDeleted == false));

        string customersHtml = data.Aggregate(" <select>", (current, item) => current + string.Format("<option value={0}>{1}</option>", item.TransactionId, item.Quantity));
        customersHtml += "</select>";
        return customersHtml;
    }

    [HttpPost]
    public string CreateMonthlyWiseUserProductTransaction(UserProductTransaction data)
    {
        try
        {
            var repository = new UserProductTransactionRepository();
            repository.Insert(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpPost]
    public string EditMonthlyWiseUserProductTransaction(UserProductTransaction data)
    {
        try
        {
            var repository = new UserProductTransactionRepository();
            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpGet]
    public string DeleteMonthlyWiseUserProductTransaction(int id, string name)
    {
        try
        {
            var repository = new UserProductTransactionRepository();
            long TransactionId = Convert.ToInt32(name);
            var data = repository.First(p => p.TransactionId == TransactionId);
            data.IsDeleted = true;
            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    #endregion

    #region Business Info

    public dynamic GetBusinessTypeList(string sidx, string sord, int page, int rows)
    {
        var repository = new BusinessTypeRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.BusinessId, out totalRecords, (page - 1) * rows, rows, null, c => new { c.BusinessId, c.BusinessName, c.BusinessDescription });

        return new
        {
            total = totalRecords / rows + 1,
            page,
            records = totalRecords,
            rows = data
        };
    }

    public string GetBusinessTypeListAsHtml()
    {
        var repository = new BusinessTypeRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.BusinessId, out totalRecords, 0, 0, null, c => new { c.BusinessId, c.BusinessName, c.BusinessDescription });

        string customersHtml = data.Aggregate(" <select>", (current, item) => current + string.Format("<option value={0}>{1}</option>", item.BusinessId, item.BusinessName));
        customersHtml += "</select>";
        return customersHtml;
    }

    [HttpPost]
    public string CreateBusinessType(BusinessType data)
    {
        try
        {
            var repository = new BusinessTypeRepository();
            repository.Insert(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpPost]
    public string EditBusinessType(BusinessType data)
    {
        try
        {
            var repository = new BusinessTypeRepository();

            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpGet]
    public string DeleteBusinessType(int id, string name)
    {
        try
        {
            var repository = new BusinessTypeRepository();
            long FeaturesCategoryId = Convert.ToInt32(name);
            var data = repository.First(p => p.BusinessId == FeaturesCategoryId);
            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    #endregion



    public dynamic UpdateQuantityToCart(int isRedirect, UserProductTransaction cart)
    {

        try
        {
            var pId = cart.ProductId;
            var spid = cart.SubProductId;
            var repository = new ProductRepository();
            var cartItems = (List<UserProductTransaction>)BalUtility.GetSession(Shared.Sessions.CustomerCart) ??
                            new List<UserProductTransaction>();

            Product product = null;
            SubProduct subproduct = null;
            if (cart.SubProductId != 0)
            {
                var subproductrepository = new SubProductRepository();
                subproduct = subproductrepository.Single(p => p.SubProductId == cart.SubProductId);
                product = subproduct.Product;
                product.Quantity = subproduct.Quantity;
                product.ProductCost = subproduct.ProductCost;
                product.ProductName = product.ProductName + " - " + subproduct.SPName;
            }
            else
            {
                cart.SubProductId = null;
                product = repository.Single(p => p.ProductId == pId);
            }

            if (product.Quantity >= cart.Quantity)
            {
                if (cartItems.Count > 0)
                {
                    var item = (from a in cartItems where a.ProductId == pId && (a.SubProductId == 0 ? a.SubProductId == spid : true) select a).FirstOrDefault();
                    if (item != null)
                    {
                        item.Quantity = cart.Quantity;
                        item.ProductCost = item.Product.ProductCost * cart.Quantity;
                        BalUtility.CreateSession(cartItems, Shared.Sessions.CustomerCart);
                    }
                    else
                    {
                        CustomCurrency CurrentCurrency = (CustomCurrency)BAL.BalUtility.GetSession(Shared.Sessions.Currency);
                        cart.CurrencyCode = CurrentCurrency.ToCurrency;
                        cart.CurrencyValue = CurrentCurrency.Value;
                        cart.CurrencySymbol = CurrentCurrency.Symbol;
                        //Product Info
                        cart.Status = 0;
                        cart.Product = product;
                        cart.ProductCost = cart.Quantity * product.ProductCost;
                        cartItems.Add(cart);
                        //DeleteFromWishList(pId);
                        BalUtility.CreateSession(cartItems, Shared.Sessions.CustomerCart);
                    }
                }
                else
                {
                    CustomCurrency CurrentCurrency = (CustomCurrency)BAL.BalUtility.GetSession(Shared.Sessions.Currency);
                    cart.CurrencyCode = CurrentCurrency.ToCurrency;
                    cart.CurrencyValue = CurrentCurrency.Value;
                    cart.CurrencySymbol = CurrentCurrency.Symbol;
                    //Product Info
                    cart.Status = 0;
                    cart.Product = product;
                    cart.ProductCost = cart.Quantity * product.ProductCost;
                    cartItems.Add(cart);
                    // DeleteFromWishList(pId);
                    BalUtility.CreateSession(cartItems, Shared.Sessions.CustomerCart);
                }
                if (isRedirect == 1)
                {
                    return new CustomResponse
                    {
                        Status = Shared.ResponseStatus.Success.ToString(),
                        Message = CustomMessages.CartAddedSuccessfully,
                        Result = cartItems.Count + " item(s) - " + cartItems.Sum(p => p.ProductCost)
                    };
                }
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = CustomMessages.CartAddedSuccessfully,
                    CartSum = cartItems.FirstOrDefault().ProductCost.ToString(),
                    Result = cartItems.Count == 0 ? cartItems.Count + " item(s) - " + cartItems.Sum(p => p.ProductCost)
                    : "<a href='MyCart.aspx'>" + cartItems.Count + " item(s) - " + cartItems.Sum(p => p.ProductCost) + "</a>"
                };

            }
            else
            {

                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Fail.ToString(),
                    Message = CustomMessages.Outofstock.ToString(),
                    Result = null
                };
            }
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            throw;
        }
    }


    public void CheckallOrders(int rows)
    {
        db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();

        var repository = new PaymentTransactionRepository();
        int totalRecords;

        var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, rows, (p => p.PaymentTransactionId != 0), null, "", true);
        var filteredData = (from q in (data as List<PaymentTransaction>)
                            select new
                            {
                                q.User.FirstName,
                                q.User.LastName,
                                q.User.MobileNo,
                                q.User.EmailId,
                                q.User.UserId,
                                q.PaymentTransactionId,
                                Quantity = q.UserProductTransactions.Count(),
                                q.PaymentStatus,
                                q.TxnAmount,
                                q.TxnRefNo,
                                q.TxnStatus,
                                currency = q.CurrencyCode + " ( " + q.CurrencySymbol + " ) ",
                                q.CurrencyCode,
                                q.CurrencySymbol,
                                q.PGTxnId,
                                q.TxnMessage,
                                q.CreatedOn,
                                q.OrderCurrentStatus,
                                q.PaymentMode,
                                products = q.UserProductTransactions.Where(cat => cat.PaymentTransactionId == q.PaymentTransactionId).
                                  Select(u => string.Join(",", (u.Product.ProductName + (u.SubProduct == null ? "" : " - " + u.SubProduct.SPName))))

                            }).ToList();

        if (filteredData != null)
        {
            foreach (var filter in filteredData)
            {
                if (filter.products.Count() == 0)
                {
                    long PtId = filter.PaymentTransactionId - 1;
                    List<UserProductTransaction> UPT = Entity.UserProductTransactions.Where(x => x.PaymentTransactionId == PtId).ToList();
                    foreach (UserProductTransaction Transaction in UPT)
                    {
                        UserProductTransaction NewUPT = new UserProductTransaction();

                        NewUPT.PaymentTransactionId = filter.PaymentTransactionId;
                        NewUPT.ProductId = Transaction.ProductId;
                        NewUPT.UserId = filter.UserId;
                        NewUPT.SubProductId = Transaction.SubProductId;
                        NewUPT.Quantity = Transaction.Quantity;
                        NewUPT.ProductCost = Transaction.ProductCost;
                        NewUPT.ProductDiscountPercentage = Transaction.ProductDiscountPercentage;
                        NewUPT.ProductDiscountCost = Transaction.ProductDiscountCost;
                        NewUPT.ShippingAddressId = Transaction.ShippingAddressId;
                        NewUPT.BillingAddressId = Transaction.BillingAddressId;
                        NewUPT.IsActive = Transaction.IsActive;
                        NewUPT.IsDeleted = Transaction.IsDeleted;
                        NewUPT.CreatedOn = Transaction.CreatedOn;
                        NewUPT.UpdatedOn = Transaction.UpdatedOn;
                        NewUPT.CreatedBy = Transaction.CreatedBy;
                        NewUPT.UpdatedBy = Transaction.UpdatedBy;
                        NewUPT.Status = Transaction.Status;
                        NewUPT.CurrencyCode = Transaction.CurrencyCode;
                        NewUPT.CurrencyValue = Transaction.CurrencyValue;
                        NewUPT.CurrencySymbol = Transaction.CurrencySymbol;
                        NewUPT.OrdersReturnReason = Transaction.OrdersReturnReason;
                        NewUPT.OrdersReturnAction = Transaction.OrdersReturnAction;
                        NewUPT.ReplacementTransactionId = Transaction.ReplacementTransactionId;
                        NewUPT.IsReplaced = Transaction.IsReplaced;

                        Entity.UserProductTransactions.Add(NewUPT);
                        Entity.SaveChanges();
                    }
                }
            }
        }

    }
    public DateTime FirstDayOfWeek(DateTime date)
    {
        var candidateDate = date;
        while (candidateDate.DayOfWeek != DayOfWeek.Monday)
        {
            candidateDate = candidateDate.AddDays(-1);
        }
        return candidateDate;
    }

    public List<PaymentTransaction> GetPerformanceIndicatorReport(string FromDate, string ToDate, int CurrentStatus)
    {
        var repository = new PaymentTransactionRepository();
        int totalOrders;
        DateTime FD = Convert.ToDateTime(FromDate);
        DateTime TD = Convert.ToDateTime(ToDate);

        List<PaymentTransaction> data = null;
        if (CurrentStatus == 0)
        {
            data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalOrders, 0, 5000, (p => p.CreatedOn >= FD && p.CreatedOn <= TD), null, "", true);
        }
        else if (CurrentStatus == 1)
        {
            data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalOrders, 0, 5000, (p => p.CreatedOn >= FD && p.CreatedOn <= TD && p.TxnStatus == "SUCCESS"), null, "", true);
        }

        return data;
    }

    //public double GetPerformanceIndicatorAmount(string FromDate, string ToDate, int CurrentStatus)
    //{
    //    var repository = new PaymentTransactionRepository();
    //    int totalOrders;
    //    DateTime FD = Convert.ToDateTime(FromDate);
    //    DateTime TD = Convert.ToDateTime(ToDate);

    //    List<PaymentTransaction> data = null;
    //    if (CurrentStatus == 0)
    //    {
    //        data = repository.FetchAllByPage(p => p.PaymentTransactionId, out  totalOrders, 0, 5000, (p => p.CreatedOn >= FD && p.CreatedOn <= TD && p.TxnAmount > 50), null, "", true);
    //    }
    //    else if (CurrentStatus == 1)
    //    {
    //        data = repository.FetchAllByPage(p => p.PaymentTransactionId, out  totalOrders, 0, 5000, (p => p.CreatedOn >= FD && p.CreatedOn <= TD && p.TxnAmount > 50 && p.TxnStatus == "SUCCESS"), null, "", true);
    //    }
    //    return Convert.ToDouble(data.Sum(x => x.TxnAmount));
    //}
    public CustomResponse GetPerformanceIndicator()
    {
        DateTime WeekDay = FirstDayOfWeek(DateTime.Now);
        DateTime LastweekDay = WeekDay.AddDays(-7);
        DateTime Last2weekDay = WeekDay.AddDays(-14);
        DateTime Last3weekDay = WeekDay.AddDays(-21);
        DateTime Last4weekDay = WeekDay.AddDays(-28);

        DateTime date = DateTime.Now;
        DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
        DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1);

        StringBuilder strResult = new StringBuilder();
        strResult.Append(@"
		<div>                   
					
					<div >
							<div id='Main_dvGrid'>
								<div> 
				
								<div>   
		<table style='width:100%;' border='1' cellpadding='8' cellspacing='0'>
<tbody>
<tr>
<td style='width: 950px;'>&nbsp;</td>
<td style='width: 590px;'><strong>Week Starting " + WeekDay.ToShortDateString() + @"</strong></td>
<td style='width: 590px;'><strong>Week Starting " + LastweekDay.ToShortDateString() + @"</strong></td>
<td style='width: 590px;'><strong>Week Starting " + Last2weekDay.ToShortDateString() + @"</strong></td>
<td style='width: 590px;'><strong>Week Starting " + Last3weekDay.ToShortDateString() + @"</strong></td>
<td style='width: 590px;'><strong>Week Starting " + Last4weekDay.ToShortDateString() + @"</strong></td>
</tr>
<tr>
<td style='width: 950px;'>Total Orders</td>
<td style='width: 590px;'>" + GetPerformanceIndicatorReport(WeekDay.ToShortDateString(), DateTime.Now.AddDays(1).ToShortDateString(), 0).Count + @"</td>
<td style='width: 590px;'>" + GetPerformanceIndicatorReport(LastweekDay.ToShortDateString(), LastweekDay.AddDays(7).ToShortDateString(), 0).Count + @"</td>
<td style='width: 590px;'>" + GetPerformanceIndicatorReport(Last2weekDay.ToShortDateString(), Last2weekDay.AddDays(7).ToShortDateString(), 0).Count + @"</td>
<td style='width: 590px;'>" + GetPerformanceIndicatorReport(Last3weekDay.ToShortDateString(), Last3weekDay.AddDays(7).ToShortDateString(), 0).Count + @"</td>
<td style='width: 590px;'>" + GetPerformanceIndicatorReport(Last4weekDay.ToShortDateString(), Last4weekDay.AddDays(7).ToShortDateString(), 0).Count + @"</td>
</tr>
<tr>
<td style='width: 950px;'>Total Amount</td>
<td style='width: 590px;'><a style='color:blue;' target='_blank' href='Salesreport.aspx?From=" + WeekDay.ToShortDateString() + "&To=" + DateTime.Now.AddDays(1).ToShortDateString() + "&Status=2'>" + Convert.ToDouble(GetPerformanceIndicatorReport(WeekDay.ToShortDateString(), DateTime.Now.AddDays(1).ToShortDateString(), 0).Sum(x => x.TxnAmount)) + @".00</a></td>
<td style='width: 590px;'><a style='color:blue;' target='_blank' href='Salesreport.aspx?From=" + LastweekDay.ToShortDateString() + "&To=" + LastweekDay.AddDays(7).ToShortDateString() + "&Status=2'>" + Convert.ToDouble(GetPerformanceIndicatorReport(LastweekDay.ToShortDateString(), LastweekDay.AddDays(7).ToShortDateString(), 0).Sum(x => x.TxnAmount)) + @".00</a></td>
<td style='width: 590px;'><a style='color:blue;' target='_blank' href='Salesreport.aspx?From=" + Last2weekDay.ToShortDateString() + "&To=" + Last2weekDay.AddDays(7).ToShortDateString() + "&Status=2'>" + Convert.ToDouble(GetPerformanceIndicatorReport(Last2weekDay.ToShortDateString(), Last2weekDay.AddDays(7).ToShortDateString(), 0).Sum(x => x.TxnAmount)) + @".00</a></td>
<td style='width: 590px;'><a style='color:blue;' target='_blank' href='Salesreport.aspx?From=" + Last3weekDay.ToShortDateString() + "&To=" + Last3weekDay.AddDays(7).ToShortDateString() + "&Status=2'>" + Convert.ToDouble(GetPerformanceIndicatorReport(Last3weekDay.ToShortDateString(), Last3weekDay.AddDays(7).ToShortDateString(), 0).Sum(x => x.TxnAmount)) + @".00</a></td>
<td style='width: 590px;'><a style='color:blue;' target='_blank' href='Salesreport.aspx?From=" + Last4weekDay.ToShortDateString() + "&To=" + Last4weekDay.AddDays(7).ToShortDateString() + "&Status=2'>" + Convert.ToDouble(GetPerformanceIndicatorReport(Last4weekDay.ToShortDateString(), Last4weekDay.AddDays(7).ToShortDateString(), 0).Sum(x => x.TxnAmount)) + @".00</a></td>
</tr>
<tr>
<td style='width: 950px;'>Successful Orders</td>
<td style='width: 590px;'>" + GetPerformanceIndicatorReport(WeekDay.ToShortDateString(), DateTime.Now.AddDays(1).ToShortDateString(), 1).Count + @"</td>
<td style='width: 590px;'>" + GetPerformanceIndicatorReport(LastweekDay.ToShortDateString(), LastweekDay.AddDays(7).ToShortDateString(), 1).Count + @"</td>
<td style='width: 590px;'>" + GetPerformanceIndicatorReport(Last2weekDay.ToShortDateString(), Last2weekDay.AddDays(7).ToShortDateString(), 1).Count + @"</td>
<td style='width: 590px;'>" + GetPerformanceIndicatorReport(Last3weekDay.ToShortDateString(), Last3weekDay.AddDays(7).ToShortDateString(), 1).Count + @"</td>
<td style='width: 590px;'>" + GetPerformanceIndicatorReport(Last4weekDay.ToShortDateString(), Last4weekDay.AddDays(7).ToShortDateString(), 1).Count + @"</td>
</tr>
<tr>
<td style='width: 950px;'>Successful Amount</td>
<td style='width: 590px;'><a style='color:blue;' target='_blank' href='Salesreport.aspx?From=" + WeekDay.ToShortDateString() + "&To=" + DateTime.Now.AddDays(1).ToShortDateString() + "&Status=0'>" + Convert.ToDouble(GetPerformanceIndicatorReport(WeekDay.ToShortDateString(), DateTime.Now.AddDays(1).ToShortDateString(), 1).Sum(x => x.TxnAmount)) + @".00</a></td>
<td style='width: 590px;'><a style='color:blue;' target='_blank' href='Salesreport.aspx?From=" + LastweekDay.ToShortDateString() + "&To=" + LastweekDay.AddDays(7).ToShortDateString() + "&Status=0'>" + Convert.ToDouble(GetPerformanceIndicatorReport(LastweekDay.ToShortDateString(), LastweekDay.AddDays(7).ToShortDateString(), 1).Sum(x => x.TxnAmount)) + @".00</a></td>
<td style='width: 590px;'><a style='color:blue;' target='_blank' href='Salesreport.aspx?From=" + Last2weekDay.ToShortDateString() + "&To=" + Last2weekDay.AddDays(7).ToShortDateString() + "&Status=0'>" + Convert.ToDouble(GetPerformanceIndicatorReport(Last2weekDay.ToShortDateString(), Last2weekDay.AddDays(7).ToShortDateString(), 1).Sum(x => x.TxnAmount)) + @".00</a></td>
<td style='width: 590px;'><a style='color:blue;' target='_blank' href='Salesreport.aspx?From=" + Last3weekDay.ToShortDateString() + "&To=" + Last3weekDay.AddDays(7).ToShortDateString() + "&Status=0'>" + Convert.ToDouble(GetPerformanceIndicatorReport(Last3weekDay.ToShortDateString(), Last3weekDay.AddDays(7).ToShortDateString(), 1).Sum(x => x.TxnAmount)) + @".00</a></td>
<td style='width: 590px;'><a style='color:blue;' target='_blank' href='Salesreport.aspx?From=" + Last4weekDay.ToShortDateString() + "&To=" + Last4weekDay.AddDays(7).ToShortDateString() + "&Status=0'>" + Convert.ToDouble(GetPerformanceIndicatorReport(Last4weekDay.ToShortDateString(), Last4weekDay.AddDays(7).ToShortDateString(), 1).Sum(x => x.TxnAmount)) + @".00</a></td>
</tr>
<tr>
<td style='width: 950px;'>UnSuccessful Orders</td>
<td style='width: 590px;'>" + (GetPerformanceIndicatorReport(WeekDay.ToShortDateString(), DateTime.Now.AddDays(1).ToShortDateString(), 0).Count - GetPerformanceIndicatorReport(WeekDay.ToShortDateString(), DateTime.Now.AddDays(1).ToShortDateString(), 1).Count) + @"</td>
<td style='width: 590px;'>" + (GetPerformanceIndicatorReport(LastweekDay.ToShortDateString(), LastweekDay.AddDays(7).ToShortDateString(), 0).Count - GetPerformanceIndicatorReport(LastweekDay.ToShortDateString(), LastweekDay.AddDays(7).ToShortDateString(), 1).Count) + @"</td>
<td style='width: 590px;'>" + (GetPerformanceIndicatorReport(Last2weekDay.ToShortDateString(), Last2weekDay.AddDays(7).ToShortDateString(), 0).Count - GetPerformanceIndicatorReport(Last2weekDay.ToShortDateString(), Last2weekDay.AddDays(7).ToShortDateString(), 1).Count) + @"</td>
<td style='width: 590px;'>" + (GetPerformanceIndicatorReport(Last3weekDay.ToShortDateString(), Last3weekDay.AddDays(7).ToShortDateString(), 0).Count - GetPerformanceIndicatorReport(Last3weekDay.ToShortDateString(), Last3weekDay.AddDays(7).ToShortDateString(), 1).Count) + @"</td>
<td style='width: 590px;'>" + (GetPerformanceIndicatorReport(Last4weekDay.ToShortDateString(), Last4weekDay.AddDays(7).ToShortDateString(), 0).Count - GetPerformanceIndicatorReport(Last4weekDay.ToShortDateString(), Last4weekDay.AddDays(7).ToShortDateString(), 1).Count) + @"</td>
</tr>
<tr>
<td style='width: 950px;'>UnSuccessful Amount</td>
<td style='width: 590px;'><a style='color:blue;' target='_blank' href='Salesreport.aspx?From=" + WeekDay.ToShortDateString() + "&To=" + DateTime.Now.AddDays(1).ToShortDateString() + "&Status=-1'>" + Convert.ToDouble(GetPerformanceIndicatorReport(WeekDay.ToShortDateString(), DateTime.Now.AddDays(1).ToShortDateString(), 0).Sum(x => x.TxnAmount) - GetPerformanceIndicatorReport(WeekDay.ToShortDateString(), DateTime.Now.AddDays(1).ToShortDateString(), 1).Sum(x => x.TxnAmount)) + @".00</a></td>
<td style='width: 590px;'><a style='color:blue;' target='_blank' href='Salesreport.aspx?From=" + LastweekDay.ToShortDateString() + "&To=" + LastweekDay.AddDays(7).ToShortDateString() + "&Status=-1'>" + Convert.ToDouble(GetPerformanceIndicatorReport(LastweekDay.ToShortDateString(), LastweekDay.AddDays(7).ToShortDateString(), 0).Sum(x => x.TxnAmount) - GetPerformanceIndicatorReport(LastweekDay.ToShortDateString(), LastweekDay.AddDays(7).ToShortDateString(), 1).Sum(x => x.TxnAmount)) + @".00</a></td>
<td style='width: 590px;'><a style='color:blue;' target='_blank' href='Salesreport.aspx?From=" + Last2weekDay.ToShortDateString() + "&To=" + Last2weekDay.AddDays(7).ToShortDateString() + "&Status=-1'>" + Convert.ToDouble(GetPerformanceIndicatorReport(Last2weekDay.ToShortDateString(), Last2weekDay.AddDays(7).ToShortDateString(), 0).Sum(x => x.TxnAmount) - GetPerformanceIndicatorReport(Last2weekDay.ToShortDateString(), Last2weekDay.AddDays(7).ToShortDateString(), 1).Sum(x => x.TxnAmount)) + @".00</a></td>
<td style='width: 590px;'><a style='color:blue;' target='_blank' href='Salesreport.aspx?From=" + Last3weekDay.ToShortDateString() + "&To=" + Last3weekDay.AddDays(7).ToShortDateString() + "&Status=-1'>" + Convert.ToDouble(GetPerformanceIndicatorReport(Last3weekDay.ToShortDateString(), Last3weekDay.AddDays(7).ToShortDateString(), 0).Sum(x => x.TxnAmount) - GetPerformanceIndicatorReport(Last3weekDay.ToShortDateString(), Last3weekDay.AddDays(7).ToShortDateString(), 1).Sum(x => x.TxnAmount)) + @".00</a></td>
<td style='width: 590px;'><a style='color:blue;' target='_blank' href='Salesreport.aspx?From=" + Last4weekDay.ToShortDateString() + "&To=" + Last4weekDay.AddDays(7).ToShortDateString() + "&Status=-1'>" + Convert.ToDouble(GetPerformanceIndicatorReport(Last4weekDay.ToShortDateString(), Last4weekDay.AddDays(7).ToShortDateString(), 0).Sum(x => x.TxnAmount) - GetPerformanceIndicatorReport(Last4weekDay.ToShortDateString(), Last4weekDay.AddDays(7).ToShortDateString(), 1).Sum(x => x.TxnAmount)) + @".00</a></td>
</tr>
<tr>
<td style='width: 950px;'>Success Percentage</td>
<td style='width: 590px;'>" + (GetPerformanceIndicatorReport(WeekDay.ToShortDateString(), DateTime.Now.AddDays(1).ToShortDateString(), 1).Count * 100) / (GetPerformanceIndicatorReport(WeekDay.ToShortDateString(), DateTime.Now.AddDays(1).ToShortDateString(), 0).Count) + @" %</td>
<td style='width: 590px;'>" + (GetPerformanceIndicatorReport(LastweekDay.ToShortDateString(), LastweekDay.AddDays(7).ToShortDateString(), 1).Count * 100) / (GetPerformanceIndicatorReport(LastweekDay.ToShortDateString(), LastweekDay.AddDays(7).ToShortDateString(), 0).Count) + @" %</td>
<td style='width: 590px;'>" + (GetPerformanceIndicatorReport(Last2weekDay.ToShortDateString(), Last2weekDay.AddDays(7).ToShortDateString(), 1).Count * 100) / (GetPerformanceIndicatorReport(Last2weekDay.ToShortDateString(), Last2weekDay.AddDays(7).ToShortDateString(), 0).Count) + @" %</td>
<td style='width: 590px;'>" + (GetPerformanceIndicatorReport(Last3weekDay.ToShortDateString(), Last3weekDay.AddDays(7).ToShortDateString(), 1).Count * 100) / (GetPerformanceIndicatorReport(Last3weekDay.ToShortDateString(), Last3weekDay.AddDays(7).ToShortDateString(), 0).Count) + @" %</td>
<td style='width: 590px;'>" + (GetPerformanceIndicatorReport(Last4weekDay.ToShortDateString(), Last4weekDay.AddDays(7).ToShortDateString(), 1).Count * 100) / (GetPerformanceIndicatorReport(Last4weekDay.ToShortDateString(), Last4weekDay.AddDays(7).ToShortDateString(), 0).Count) + @" %</td>
</tr>
</tbody></table>

<br/><br/>

<table style='width:100%;' border='1' cellpadding='8' cellspacing='0'><tbody>
<tr>
<td style='width: 950px;'>&nbsp;</td>
<td style='width: 590px;'><strong>" + String.Format("{0:MMMM}", DateTime.Now) + @"</strong></td>
<td style='width: 590px;'><strong>" + String.Format("{0:MMMM}", DateTime.Now.AddMonths(-1)) + @"</strong></td>
<td style='width: 590px;'><strong>" + String.Format("{0:MMMM}", DateTime.Now.AddMonths(-2)) + @"</strong></td>
<td style='width: 590px;'><strong>" + String.Format("{0:MMMM}", DateTime.Now.AddMonths(-3)) + @"</strong></td>
<td style='width: 590px;'><strong>" + String.Format("{0:MMMM}", DateTime.Now.AddMonths(-4)) + @"</strong></td>
</tr>
<tr>
<td style='width: 950px;'>Total Orders</td>
<td style='width: 590px;'>" + GetPerformanceIndicatorReport(firstDayOfMonth.ToShortDateString(), lastDayOfMonth.ToShortDateString(), 0).Count + @"</td>
<td style='width: 590px;'>" + GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-1).ToShortDateString(), lastDayOfMonth.AddMonths(-1).ToShortDateString(), 0).Count + @"</td>
<td style='width: 590px;'>" + GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-2).ToShortDateString(), lastDayOfMonth.AddMonths(-2).ToShortDateString(), 0).Count + @"</td>
<td style='width: 590px;'>" + GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-3).ToShortDateString(), lastDayOfMonth.AddMonths(-3).ToShortDateString(), 0).Count + @"</td>
<td style='width: 590px;'>" + GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-4).ToShortDateString(), lastDayOfMonth.AddMonths(-4).ToShortDateString(), 0).Count + @"</td>
</tr>
<tr>
<td style='width: 950px;'>Total Amount</td>
<td style='width: 590px;'><a style='color:blue;' target='_blank' href='Salesreport.aspx?From=" + firstDayOfMonth.ToShortDateString() + "&To=" + lastDayOfMonth.ToShortDateString() + "&Status=2'>" + Convert.ToDouble(GetPerformanceIndicatorReport(firstDayOfMonth.ToShortDateString(), lastDayOfMonth.ToShortDateString(), 0).Sum(x => x.TxnAmount)) + @".00</a></td>
<td style='width: 590px;'><a style='color:blue;' target='_blank' href='Salesreport.aspx?From=" + firstDayOfMonth.AddMonths(-1).ToShortDateString() + "&To=" + lastDayOfMonth.AddMonths(-1).ToShortDateString() + "&Status=2'>" + Convert.ToDouble(GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-1).ToShortDateString(), lastDayOfMonth.AddMonths(-1).ToShortDateString(), 0).Sum(x => x.TxnAmount)) + @".00</a></td>
<td style='width: 590px;'><a style='color:blue;' target='_blank' href='Salesreport.aspx?From=" + firstDayOfMonth.AddMonths(-2).ToShortDateString() + "&To=" + lastDayOfMonth.AddMonths(-2).ToShortDateString() + "&Status=2'>" + Convert.ToDouble(GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-2).ToShortDateString(), lastDayOfMonth.AddMonths(-2).ToShortDateString(), 0).Sum(x => x.TxnAmount)) + @".00</a></td>
<td style='width: 590px;'><a style='color:blue;' target='_blank' href='Salesreport.aspx?From=" + firstDayOfMonth.AddMonths(-3).ToShortDateString() + "&To=" + lastDayOfMonth.AddMonths(-3).ToShortDateString() + "&Status=2'>" + Convert.ToDouble(GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-3).ToShortDateString(), lastDayOfMonth.AddMonths(-3).ToShortDateString(), 0).Sum(x => x.TxnAmount)) + @".00</a></td>
<td style='width: 590px;'><a style='color:blue;' target='_blank' href='Salesreport.aspx?From=" + firstDayOfMonth.AddMonths(-4).ToShortDateString() + "&To=" + lastDayOfMonth.AddMonths(-4).ToShortDateString() + "&Status=2'>" + Convert.ToDouble(GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-4).ToShortDateString(), lastDayOfMonth.AddMonths(-4).ToShortDateString(), 0).Sum(x => x.TxnAmount)) + @".00</a></td>
</tr>
<tr>
<td style='width: 950px;'>Successful Orders</td>
<td style='width: 590px;'>" + GetPerformanceIndicatorReport(firstDayOfMonth.ToShortDateString(), lastDayOfMonth.ToShortDateString(), 1).Count + @"</td>
<td style='width: 590px;'>" + GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-1).ToShortDateString(), lastDayOfMonth.AddMonths(-1).ToShortDateString(), 1).Count + @"</td>
<td style='width: 590px;'>" + GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-2).ToShortDateString(), lastDayOfMonth.AddMonths(-2).ToShortDateString(), 1).Count + @"</td>
<td style='width: 590px;'>" + GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-3).ToShortDateString(), lastDayOfMonth.AddMonths(-3).ToShortDateString(), 1).Count + @"</td>
<td style='width: 590px;'>" + GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-4).ToShortDateString(), lastDayOfMonth.AddMonths(-4).ToShortDateString(), 1).Count + @"</td>
</tr>
<tr>
<td style='width: 950px;'>Successful Amount</td>
<td style='width: 590px;'><a style='color:blue;' target='_blank' href='Salesreport.aspx?From=" + firstDayOfMonth.ToShortDateString() + "&To=" + lastDayOfMonth.ToShortDateString() + "&Status=0'>" + Convert.ToDouble(GetPerformanceIndicatorReport(firstDayOfMonth.ToShortDateString(), lastDayOfMonth.ToShortDateString(), 1).Sum(x => x.TxnAmount)) + @".00</a></td>
<td style='width: 590px;'><a style='color:blue;' target='_blank' href='Salesreport.aspx?From=" + firstDayOfMonth.AddMonths(-1).ToShortDateString() + "&To=" + lastDayOfMonth.AddMonths(-1).ToShortDateString() + "&Status=0'>" + Convert.ToDouble(GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-1).ToShortDateString(), lastDayOfMonth.AddMonths(-1).ToShortDateString(), 1).Sum(x => x.TxnAmount)) + @".00</a></td>
<td style='width: 590px;'><a style='color:blue;' target='_blank' href='Salesreport.aspx?From=" + firstDayOfMonth.AddMonths(-2).ToShortDateString() + "&To=" + lastDayOfMonth.AddMonths(-2).ToShortDateString() + "&Status=0'>" + Convert.ToDouble(GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-2).ToShortDateString(), lastDayOfMonth.AddMonths(-2).ToShortDateString(), 1).Sum(x => x.TxnAmount)) + @".00</a></td>
<td style='width: 590px;'><a style='color:blue;' target='_blank' href='Salesreport.aspx?From=" + firstDayOfMonth.AddMonths(-3).ToShortDateString() + "&To=" + lastDayOfMonth.AddMonths(-3).ToShortDateString() + "&Status=0'>" + Convert.ToDouble(GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-3).ToShortDateString(), lastDayOfMonth.AddMonths(-3).ToShortDateString(), 1).Sum(x => x.TxnAmount)) + @".00</a></td>
<td style='width: 590px;'><a style='color:blue;' target='_blank' href='Salesreport.aspx?From=" + firstDayOfMonth.AddMonths(-4).ToShortDateString() + "&To=" + lastDayOfMonth.AddMonths(-4).ToShortDateString() + "&Status=0'>" + Convert.ToDouble(GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-4).ToShortDateString(), lastDayOfMonth.AddMonths(-4).ToShortDateString(), 1).Sum(x => x.TxnAmount)) + @".00</a></td>
</tr>
<tr>
<td style='width: 950px;'>UnSuccessful Orders</td>
<td style='width: 590px;'>" + (GetPerformanceIndicatorReport(firstDayOfMonth.ToShortDateString(), lastDayOfMonth.ToShortDateString(), 0).Count - GetPerformanceIndicatorReport(firstDayOfMonth.ToShortDateString(), lastDayOfMonth.ToShortDateString(), 1).Count) + @"</td>
<td style='width: 590px;'>" + (GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-1).ToShortDateString(), lastDayOfMonth.AddMonths(-1).ToShortDateString(), 0).Count - GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-1).ToShortDateString(), lastDayOfMonth.AddMonths(-1).ToShortDateString(), 1).Count) + @"</td>
<td style='width: 590px;'>" + (GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-2).ToShortDateString(), lastDayOfMonth.AddMonths(-2).ToShortDateString(), 0).Count - GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-2).ToShortDateString(), lastDayOfMonth.AddMonths(-2).ToShortDateString(), 1).Count) + @"</td>
<td style='width: 590px;'>" + (GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-3).ToShortDateString(), lastDayOfMonth.AddMonths(-3).ToShortDateString(), 0).Count - GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-3).ToShortDateString(), lastDayOfMonth.AddMonths(-3).ToShortDateString(), 1).Count) + @"</td>
<td style='width: 590px;'>" + (GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-4).ToShortDateString(), lastDayOfMonth.AddMonths(-4).ToShortDateString(), 0).Count - GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-4).ToShortDateString(), lastDayOfMonth.AddMonths(-4).ToShortDateString(), 1).Count) + @"</td>
</tr>
<tr>
<td style='width: 950px;'>UnSuccessful Amount</td>
<td style='width: 590px;'><a style='color:blue;' target='_blank' href='Salesreport.aspx?From=" + firstDayOfMonth.ToShortDateString() + "&To=" + lastDayOfMonth.ToShortDateString() + "&Status=-1'>" + Convert.ToDouble(GetPerformanceIndicatorReport(firstDayOfMonth.ToShortDateString(), lastDayOfMonth.ToShortDateString(), 0).Sum(x => x.TxnAmount) - GetPerformanceIndicatorReport(firstDayOfMonth.ToShortDateString(), lastDayOfMonth.ToShortDateString(), 1).Sum(x => x.TxnAmount)) + @".00</a></td>
<td style='width: 590px;'><a style='color:blue;' target='_blank' href='Salesreport.aspx?From=" + firstDayOfMonth.AddMonths(-1).ToShortDateString() + "&To=" + lastDayOfMonth.AddMonths(-1).ToShortDateString() + "&Status=-1'>" + Convert.ToDouble(GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-1).ToShortDateString(), lastDayOfMonth.AddMonths(-1).ToShortDateString(), 0).Sum(x => x.TxnAmount) - GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-1).ToShortDateString(), lastDayOfMonth.AddMonths(-1).ToShortDateString(), 1).Sum(x => x.TxnAmount)) + @".00</a></td>
<td style='width: 590px;'><a style='color:blue;' target='_blank' href='Salesreport.aspx?From=" + firstDayOfMonth.AddMonths(-2).ToShortDateString() + "&To=" + lastDayOfMonth.AddMonths(-2).ToShortDateString() + "&Status=-1'>" + Convert.ToDouble(GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-2).ToShortDateString(), lastDayOfMonth.AddMonths(-2).ToShortDateString(), 0).Sum(x => x.TxnAmount) - GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-2).ToShortDateString(), lastDayOfMonth.AddMonths(-2).ToShortDateString(), 1).Sum(x => x.TxnAmount)) + @".00</a></td>
<td style='width: 590px;'><a style='color:blue;' target='_blank' href='Salesreport.aspx?From=" + firstDayOfMonth.AddMonths(-3).ToShortDateString() + "&To=" + lastDayOfMonth.AddMonths(-3).ToShortDateString() + "&Status=-1'>" + Convert.ToDouble(GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-3).ToShortDateString(), lastDayOfMonth.AddMonths(-3).ToShortDateString(), 0).Sum(x => x.TxnAmount) - GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-3).ToShortDateString(), lastDayOfMonth.AddMonths(-3).ToShortDateString(), 1).Sum(x => x.TxnAmount)) + @".00</a></td>
<td style='width: 590px;'><a style='color:blue;' target='_blank' href='Salesreport.aspx?From=" + firstDayOfMonth.AddMonths(-4).ToShortDateString() + "&To=" + lastDayOfMonth.AddMonths(-4).ToShortDateString() + "&Status=-1'>" + Convert.ToDouble(GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-4).ToShortDateString(), lastDayOfMonth.AddMonths(-4).ToShortDateString(), 0).Sum(x => x.TxnAmount) - GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-4).ToShortDateString(), lastDayOfMonth.AddMonths(-4).ToShortDateString(), 1).Sum(x => x.TxnAmount)) + @".00</a></td>
</tr>
<tr>
<td style='width: 950px;'>Success Percentage</td>
<td style='width: 590px;'>" + (GetPerformanceIndicatorReport(firstDayOfMonth.ToShortDateString(), lastDayOfMonth.ToShortDateString(), 1).Count * 100) / (GetPerformanceIndicatorReport(firstDayOfMonth.ToShortDateString(), lastDayOfMonth.ToShortDateString(), 0).Count) + @" %</td>
<td style='width: 590px;'>" + (GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-1).ToShortDateString(), lastDayOfMonth.AddMonths(-1).ToShortDateString(), 1).Count * 100) / (GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-1).ToShortDateString(), lastDayOfMonth.AddMonths(-1).ToShortDateString(), 0).Count) + @" %</td>
<td style='width: 590px;'>" + (GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-2).ToShortDateString(), lastDayOfMonth.AddMonths(-2).ToShortDateString(), 1).Count * 100) / (GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-2).ToShortDateString(), lastDayOfMonth.AddMonths(-2).ToShortDateString(), 0).Count) + @" %</td>
<td style='width: 590px;'>" + (GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-3).ToShortDateString(), lastDayOfMonth.AddMonths(-3).ToShortDateString(), 1).Count * 100) / (GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-3).ToShortDateString(), lastDayOfMonth.AddMonths(-3).ToShortDateString(), 0).Count) + @" %</td>
<td style='width: 590px;'>" + (GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-4).ToShortDateString(), lastDayOfMonth.AddMonths(-4).ToShortDateString(), 1).Count * 100) / (GetPerformanceIndicatorReport(firstDayOfMonth.AddMonths(-4).ToShortDateString(), lastDayOfMonth.AddMonths(-4).ToShortDateString(), 0).Count) + @" %</td>
</tr>
</tbody></table>
<br/>
<div>* All the weeks are starting on Monday and ending on Sunday</div>
</div></div></div>
</div></div></div>
</div>
		</div>");

        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Result = strResult.ToString()

        };
    }


    public CustomResponse GetEmployeelist(int rows)
    {
        db_Zon_HuwEntities context = new db_Zon_HuwEntities();
        var data = context.Users.Where(x => x.RoleId == 6).ToList();
        var fdata = (from q in (data as List<User>)
                     select new
                     {
                         q.UserId,
                         q.FirstName,
                         q.LastName,
                         q.EmailId,
                         q.PassCode,
                         q.MobileNo


                     }).ToList();
        StringBuilder strtd = new StringBuilder();
        if (fdata != null)
        {
            foreach (var item in fdata)
            {
                strtd.Append(@"<tr>
				<td>
												<a id='hypEdit' onclick='NavigatetoupdatePage(" + item.UserId + ")'  >" + item.UserId + @"</a><br>
												<span id='lblPartialShip'></span>
											</td>								
<td>
												<span id='OrderDate'>" + item.FirstName + @"</span>
												
											</td>
<td>
												<span id='OrderDate'>" + item.MobileNo + @"</span>
												
											</td>
<td>
												<span id='OrderDate'>" + item.EmailId + @"</span>
												
											</td>
<td>
												<span id='OrderDate'>" + item.PassCode + @"</span>
												
											</td>
		</tr>");
            }
            StringBuilder strResult = new StringBuilder();
            strResult.Append(@"
		<div>                   
					
					<div >
							<div id='Main_dvGrid'>
								<div> 
				
								<div>   
		<table class='activity_datatable' rules='all' id='ctl00_ctl00_Main_Main_grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
			<thead><tr class='checked'>
		
<th scope='col'>ID</th>
<th scope='col'>Name</th>
<th scope='col'>Mobile</th>
<th scope='col'>Email</th>
<th scope='col'>Password</th>

			</tr></thead><tbody>
" + strtd + @"
</tbody></table>
</div></div></div>
</div></div></div>
</div>
<div class='action_bar text_right' style='display: block;' id='dvbtn'>

			   
				<div class='clear'>
				</div>
			</div>
		</div>");
            //<option value='10' selected='selected'>5</option>
            //<option value='5'>10</option>
            //<option  value='10'>15</option>
            //<option value='15'>20</option>
            //<option value='20'>25</option>

            //<th scope='col'>Customer</th>

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strResult.ToString()

            };

        }
        else
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = ""
            };
        }

    }
    public CustomResponse GetRTOOrders(int rows)
    {
        CheckallOrders(rows);

        var repository = new PaymentTransactionRepository();
        int totalRecords;

        var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, rows, (p => p.TxnStatus == "SUCCESS" && (p.Orderdeliverystatus.Contains("rto")) || p.Orderdeliverystatus == "Product received" || p.OrderCurrentStatus == 5), null, "", true);

        var filteredData = (from q in (data as List<PaymentTransaction>)
                            select new
                            {
                                q.User.FirstName,
                                q.User.LastName,
                                q.User.MobileNo,
                                q.User.EmailId,
                                q.User.UserId,
                                q.PaymentTransactionId,
                                Quantity = q.UserProductTransactions.First().Quantity,
                                q.PaymentStatus,
                                q.TxnAmount,
                                q.TxnRefNo,
                                q.TxnStatus,
                                currency = q.CurrencyCode + " ( " + q.CurrencySymbol + " ) ",
                                q.CurrencyCode,
                                q.CurrencySymbol,
                                q.PGTxnId,
                                q.TxnMessage,
                                q.CreatedOn,
                                q.OrderCurrentStatus,
                                q.Orderdeliverystatus,
                                q.PaymentMode,
                                products = q.UserProductTransactions.Where(cat => cat.PaymentTransactionId == q.PaymentTransactionId).
                                  Select(u => string.Join(",", (u.Product.ProductName + (u.SubProduct == null ? "" : " - " + u.SubProduct.SPName)))),
                            }).ToList();

        StringBuilder strtd = new StringBuilder();

        if (filteredData != null)
        {
            foreach (var item in filteredData)
            {
                decimal Currency = Convert.ToDecimal(item.TxnAmount);
                string Amount = Currency.ToString("0.00");
                DateTime dt = item.CreatedOn;
                var OrderDate = dt.ToString("dd/MMM/yyyy");

                string ProductName = "";
                StringBuilder checkbox = new StringBuilder();

                for (var i = 0; i < item.products.Count(); i++)
                {
                    if (i == 0)
                    {
                        ProductName = item.products.ElementAtOrDefault(i).ToString();
                    }
                    else
                    {
                        ProductName = ProductName + " ," + item.products.ElementAtOrDefault(i).ToString();
                    }
                }

                string bgrowcolor = "";
                if (item.CreatedOn.ToShortDateString() == DateTime.Now.ToShortDateString())
                {
                    bgrowcolor = "#A9DFBF";
                }
                else if (item.CreatedOn >= DateTime.Now.AddDays(-2))
                {
                    bgrowcolor = "#F5CBA7";
                }
                else
                {
                    bgrowcolor = "#E6B0AA";
                }

                var addressrepository = new AddressRepository();
                UserAddress address = addressrepository.UserAddress(item.UserId);
                if (item.Orderdeliverystatus == "Product received")
                {
                    checkbox.Append(@"<input type=""checkbox"" class=""checkbox"" id='chkreturn_" + item.PaymentTransactionId + @"' disabled checked/>");
                }
                else
                {
                    checkbox.Append(@"<input type=""checkbox"" class=""checkbox""  id='chkreturn_" + item.PaymentTransactionId + @"' onclick='Updatedeliverystatus(" + item.PaymentTransactionId + ")'/>");
                }
                strtd.Append(@"<tr style='background-color:" + bgrowcolor + @"'>
				<td>
												<div id='chkBlkShip' >
<input  id='chkkShip' name='chkBlkShip' value=" + item.PaymentTransactionId + @"  type='checkbox' class='Check' onchange='CheckboxChecked(this)' ></div>
												<div style='visibility: hidden;'>
													<span id='lblTempOid'>" + item.PaymentTransactionId + @"</span>
												</div>
												
												<input  id='hdnShippingMode' class='hdnShippingMode' value='self' type='hidden'>
											</td><td>
												<a id='hypEdit' onclick='NavigatetoOrderDetailsPage(" + item.PaymentTransactionId + ")'  >" + item.PaymentTransactionId + @"</a><br>
												<span id='lblPartialShip'></span>
											</td><td>
												<span id='OrderDate'>" + ProductName + @"</span>
												
											</td>
			<td>
													<span id='lblTotalProducts'><span class='QuantityTipsySpan'>" + item.Quantity + @"</span></span>

											</td>
<td align='right'>                                                
												<span id='lblTotalPrice'><span class='WebRupee'>" + item.CurrencySymbol + @" </span>" + Amount + @"</span>
											</td>
											<td>
												<span id='OrderDate'>" + OrderDate + @"</span>
												
											</td>
<td>
												<span id='OrderDate'>" + item.PaymentMode + @"</span>
												
											</td>
<td>
												<span id=''>" + item.Orderdeliverystatus + @"</span>
												
											</td>
<td>
												<span id='Comments'><a href='javascript:;' onclick='GetComments(" + item.PaymentTransactionId + @")'>Comments</a></span>
												
											</td>
<td>
												
 <div class=""toggle-button-cover"">
      <div class=""button-cover"">
        <div class=""button r"" id=""button-1"">
        " + checkbox + @"
          <div class=""knobs""></div>
          <div class=""layer""></div>
        </div>
      </div>
    </div>												
											</td>

			</tr>");
            }
            //<td>
            //        <span id='lblFirstName'><span >" + item.FirstName + @"</span></span>
            //</td>
            StringBuilder strResult = new StringBuilder();
            strResult.Append(@"
		<div>                   
					
					<div >
							<div id='Main_dvGrid'>
								<div> 
				
								<div>   
		<table class='activity_datatable' rules='all' id='ctl00_ctl00_Main_Main_grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
			<thead><tr class='checked'>
				<th class='cp_product_select' scope='col'>                                              
												<span title='Select/Unselect all orders'>
<div id='chkSelectAll' class='checker'><span>
<input style='opacity: 0;' id='ctl00_ctl00_Main_Main_grdShippingOrders_ctl01_chkSelectAll'  onclick='selectUnselectCheckboxes(this.id, 'chkBlkShip', 'lblTempOid');' type='checkbox'></span></div></span>
											</th>
<th scope='col'>ID</th>
<th scope='col'>Product</th>
 <th scope='col'>Qty</th>
<th scope='col'><div id='divCurrency'>Amount</div></th>
<th scope='col'>Date</th>
<th scope='col'>Pay Mode</th>
<th scope='col'>Order Delivery Status</th>

<th scope='col'>Comments</th>
<th scope='col'>Return Received</th>
			</tr></thead><tbody>
" + strtd + @"
</tbody></table>
</div></div></div>
</div></div></div>
</div>
<div class='action_bar text_right' style='display: block;' id='dvbtn'>
<input style='display: inline-block;'  value='Book Shipment' class='button_small greyishBtn fl_right' onclick='CreateShippment();' type='button'>
			   
				<div class='clear'>
				</div>
			</div>
		</div>");
            //<option value='10' selected='selected'>5</option>
            //<option value='5'>10</option>
            //<option  value='10'>15</option>
            //<option value='15'>20</option>
            //<option value='20'>25</option>

            //<th scope='col'>Customer</th>

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strResult.ToString()

            };
        }
        else
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = ""
            };
        }
    }
    //Using new method for GetAuthorizedOrders  old one is not working properly
    public CustomResponse GetAuthorizedOrders(int rows)
    {
        CheckallOrders(rows);

        var repository = new PaymentTransactionRepository();
        int totalRecords;

        var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, rows, (p => p.TxnStatus == "SUCCESS" && p.Pickup == false && p.Authorized == true && p.Dispatched == false && p.Delivered == false), null, "", true);

        var filteredData = (from q in (data as List<PaymentTransaction>)
                            select new
                            {
                                q.User.FirstName,
                                q.User.LastName,
                                q.User.MobileNo,
                                q.User.EmailId,
                                q.User.UserId,
                                q.PaymentTransactionId,
                                Quantity = q.UserProductTransactions.First().Quantity,
                                q.PaymentStatus,
                                q.TxnAmount,
                                q.TxnRefNo,
                                q.TxnStatus,
                                currency = q.CurrencyCode + " ( " + q.CurrencySymbol + " ) ",
                                q.CurrencyCode,
                                q.CurrencySymbol,
                                q.PGTxnId,
                                q.TxnMessage,
                                q.CreatedOn,
                                q.OrderCurrentStatus,
                                q.PaymentMode,
                                products = q.UserProductTransactions.Where(cat => cat.PaymentTransactionId == q.PaymentTransactionId).
                                  Select(u => string.Join(",", (u.Product.ProductName + (u.SubProduct == null ? "" : " - " + u.SubProduct.SPName)))),
                            }).ToList();

        StringBuilder strtd = new StringBuilder();

        if (filteredData != null)
        {
            foreach (var item in filteredData)
            {
                decimal Currency = Convert.ToDecimal(item.TxnAmount);
                string Amount = Currency.ToString("0.00");
                DateTime dt = item.CreatedOn;
                var OrderDate = dt.ToString("dd/MMM/yyyy");

                string ProductName = "";

                for (var i = 0; i < item.products.Count(); i++)
                {
                    if (i == 0)
                    {
                        ProductName = item.products.ElementAtOrDefault(i).ToString();
                    }
                    else
                    {
                        ProductName = ProductName + " ," + item.products.ElementAtOrDefault(i).ToString();
                    }
                }

                string bgrowcolor = "";
                if (item.CreatedOn.ToShortDateString() == DateTime.Now.ToShortDateString())
                {
                    bgrowcolor = "#A9DFBF";
                }
                else if (item.CreatedOn >= DateTime.Now.AddDays(-2))
                {
                    bgrowcolor = "#F5CBA7";
                }
                else
                {
                    bgrowcolor = "#E6B0AA";
                }

                var addressrepository = new AddressRepository();
                UserAddress address = addressrepository.UserAddress(item.UserId);

                strtd.Append(@"<tr style='background-color:" + bgrowcolor + @"'>
				<td>
												<div id='chkBlkShip' >
<input  id='chkkShip' name='chkBlkShip' value=" + item.PaymentTransactionId + @"  type='checkbox' class='Check' onchange='CheckboxChecked(this)' ></div>
												<div style='visibility: hidden;'>
													<span id='lblTempOid'>" + item.PaymentTransactionId + @"</span>
												</div>
												
												<input  id='hdnShippingMode' class='hdnShippingMode' value='self' type='hidden'>
											</td><td>
												<a id='hypEdit' onclick='NavigatetoOrderDetailsPage(" + item.PaymentTransactionId + ")'  >" + item.PaymentTransactionId + @"</a><br>
												<span id='lblPartialShip'></span>
											</td><td>
												<span id='OrderDate'>" + ProductName + @"</span>
												
											</td>
			<td>
													<span id='lblTotalProducts'><span class='QuantityTipsySpan'>" + item.Quantity + @"</span></span>

											</td>
<td align='right'>                                                
												<span id='lblTotalPrice'><span class='WebRupee'>" + item.CurrencySymbol + @" </span>" + Amount + @"</span>
											</td>
											<td>
												<span id='OrderDate'>" + OrderDate + @"</span>
												
											</td>
<td>
												<span id='OrderDate'>" + item.PaymentMode + @"</span>
												
											</td>
<td>
												<span id='Comments'><a href='javascript:;' onclick='GetComments(" + item.PaymentTransactionId + @")'>Comments</a></span>
												
											</td>
<td style='display:none'>
												<span id='OrderDate'>" + item.FirstName + @"</span>
												
											</td>
<td style='display:none'>
												<span id='OrderDate'>" + item.MobileNo + @"</span>
												
											</td>
<td style='display:none'>
												<span id='OrderDate'>" + item.EmailId + @"</span>
												
											</td>
<td style='display:none'>
<span id='userAddress'>" + address.StreetAddress1 + ", " + address.StreetAddress2 + ", " + address.LandMark + ", " + address.City + ", " + address.StateName + ", " + address.PinCode + @"</span>
</td>
			</tr>");
            }
            //<td>
            //        <span id='lblFirstName'><span >" + item.FirstName + @"</span></span>
            //</td>
            StringBuilder strResult = new StringBuilder();
            strResult.Append(@"
		<div>                   
					
					<div >
							<div id='Main_dvGrid'>
								<div> 
				
								<div>   
		<table class='activity_datatable' rules='all' id='ctl00_ctl00_Main_Main_grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
			<thead><tr class='checked'>
				<th class='cp_product_select' scope='col'>                                              
												<span title='Select/Unselect all orders'>
<div id='chkSelectAll' class='checker'><span>
<input style='opacity: 0;' id='ctl00_ctl00_Main_Main_grdShippingOrders_ctl01_chkSelectAll'  onclick='selectUnselectCheckboxes(this.id, 'chkBlkShip', 'lblTempOid');' type='checkbox'></span></div></span>
											</th>
<th scope='col'>ID</th>
<th scope='col'>Product</th>
 <th scope='col'>Qty</th>
<th scope='col'><div id='divCurrency'>Amount</div></th>
<th scope='col'>Date</th>
<th scope='col'>Pay Mode</th>
<th scope='col'>Comments</th>

<th scope='col' style='display:none'>Customer</th>
<th scope='col' style='display:none'>Mobile</th>
<th scope='col' style='display:none'>Email</th>
<th scope='col' style='display:none'>Address</th>
			</tr></thead><tbody>
" + strtd + @"
</tbody></table>
</div></div></div>
</div></div></div>
</div>
<div class='action_bar text_right' style='display: block;' id='dvbtn'>
<input style='display: inline-block;'  value='Book Shipment' class='button_small greyishBtn fl_right' onclick='CreateShippment();' type='button'>
			   
				<div class='clear'>
				</div>
			</div>
		</div>");
            //<option value='10' selected='selected'>5</option>
            //<option value='5'>10</option>
            //<option  value='10'>15</option>
            //<option value='15'>20</option>
            //<option value='20'>25</option>

            //<th scope='col'>Customer</th>

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strResult.ToString()

            };
        }
        else
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = ""
            };
        }
    }


    public CustomResponse GetSalesReport(int rows)
    {

        var repository = new PaymentTransactionRepository();
        int totalRecords;
        db_Zon_HuwEntities context = new db_Zon_HuwEntities();
        var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, rows, (p => p.TxnStatus == "SUCCESS"), null, "", true);
        var filteredData = (from q in (data as List<PaymentTransaction>)
                            select new
                            {
                                q.PaymentTransactionId,
                                q.PaymentStatus,
                                q.OrderCurrentStatus,
                                q.TxnAmount,
                                q.TxnStatus,
                                q.CreatedOn,
                                q.CurrencySymbol,
                            }).Where(x => x.CreatedOn >= Convert.ToDateTime("" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-01") && x.CreatedOn <= DateTime.Now).ToList();

        decimal TotalAmount = 0;
        StringBuilder strtd = new StringBuilder();
        Tuple<string, string, string, string, string> alldata = LoadDashboard(Convert.ToDateTime("" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-01"), DateTime.Now);
        strtd.Append(@"<div class='row' style='background: #20bdc6;'>
        <div class='overview'>
            <h2>Dashboard Overview</h2>
            <p class='overview_total'>Overall Sales:
                <asp:Label ID='overallsales' runat='server'></asp:Label></p>
            <div class='overview_content'>
                <div class='overview_card'>
                    <div class='overview_top'>
                        <div>
                            <img src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADIAAAAyCAYAAAAeP4ixAAAACXBIWXMAAAsTAAALEwEAmpwYAAACj0lEQVR4nO2YO2sWQRSGn4gEiVGIJouXykKwtdFOjaawECQhXTrxB6gowU6Txs5SsNDWVIop9AcEbNSoX8DCQqyiWMQboqIZGXgXhmXvd3FeWNjsvrszz845Z84X8PLy8vLqmQJgEVgFvul4DiwAEw2OawocmZoFvqS84DMw03eQWWBTxvvAcWC7jhPAA937A0zTnrYCy3lBAmclrqT45uX5BIzTvIaAuxrzYx6QRWclQg0Dt4GliPehvNdpXjc0ls3TI3lAXshgwymECJdzPeKd1HVbAJrURY3zCzita5kgYViNOnE5BnyIAdkhr32mrAIdSZpTvm7qnLIgodZjQHY6FayMbAl/BbwG9sbcPwX81BiXIvcyQVZlsNUpC+SkvM9KQNhVGDgTGkRW5qjywSg/KAqyIIMtsa6WYpI9zJ1rFSAGkfMAOORUpjuqWIVBJhQqRiU2SVfl2QB2V4AIItfWgHc6X1aOxinXhjijzc6oxE4qZ0YVTuFKWM/ZihBx9wywAoxQg6a12SW1Bhs1QkQ9a6qUtWlcm51N5q+qaE+VE2447coooXkgXO9+5+/czWFV7QFeppTQsMQaeay/iEwbIAeAN85gUZiqEGkgtQEeBt7rZU8yKlFWOHUGcswpBI/V3mftDfQN5Azw3emQtyWEUlI4FZ2YaQJkTh2ofcEtYEtG/xSXE52DXHB+NdrfLGU72k5B5vWQBblc9OE+gAwBN/XAb+Ac1dU6yDBwT+YfKf8xKbrErYKMAI9ktK3JVIcTM2VBxtR5Gm14duPjXwQ5L8Nb4GCaseMJmzyhZUvtvixTSxMzde8jfZuw8SD068ub/2ZFTM8mZjwI/frCxq8I/frCpu6q5eXl5UUv9BdfLei2z8d16wAAAABJRU5ErkJggg=='>
                            <p>Total Sales</p>
                        </div>
                    </div>
                    <p class='overview_num'>
                        " + alldata.Item1 + @"
                    <p class='overview_text'>Rupees</p>
                </div>
                <div class='overview_card'>
                    <div class='overview_top'>
                        <div>
                            <img src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADIAAAAyCAYAAAAeP4ixAAAACXBIWXMAAAsTAAALEwEAmpwYAAABr0lEQVR4nO2ZTytEURTAf7NgtkTznpFGvoGkZHwEyUY25jsgbJk1WcqCrX8R+QqYoSRKDVt2io3FjMWgW0fdJuUa4p7p/uou3uJO5zfn3s5554EbMXAOFFFMDJSAN+ACSKCQCLgWiRsgjUKiIOEJUciEJ0QhE54QNUImDFci8Z/rGbgHToBVYBxoRaHIZ6sCbAF99bQgJXn+a5qAFDAAzABHQFViegW2XY+8DzK1mLu7CJQlridgRKuMIQPsW9mZQrFMApgXERPbtGYZw4TImDWKcpkF6850aT9mBxLXruumlFX1bz2q+hmpMyauQddNdmYu8YcliWnvO5tikTjFH2Ipmi9AO8o5lqzkUM6siKyjnKyInKGcSEQeUE7SavlVk2wUkbhRjlZWRHyqb3UxJyJrKKcgIqa9V0tstShtKGa5nqbRN7qtNr4fpSSAQ5HYQTF5kXgEOlFKTgYPVdcZl4/HKW+Ng5xmW77RY90JIzKJMtLyXl6x7sQwntMsE5sheesr1AyxN4GOr37kY4z/0+ffXmVgA+h1/Td8EDEfeu7kc8IKMAa0uAoEAoEA6ngHcT8OClSo5xgAAAAASUVORK5CYII='>
                            <p>Total Returns</p>
                        </div>
                    </div>
                    <p class='overview_num'>
                        " + alldata.Item3 + @"
                    <p class='overview_text'>Rupees</p>
                </div>
                <div class='overview_card'>
                    <div class='overview_top'>
                        <div>
                            <img src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADIAAAAyCAYAAAAeP4ixAAAACXBIWXMAAAsTAAALEwEAmpwYAAAB+0lEQVR4nO2YOUsDQRiGHzAmWphYiGjAQksre8XSxsKjyl/wwtouXRpttLPS1s4fYOcNHo1XHVQ8SgVBdGXgi6whGXZ39sjKPPBB2G923n13vjk2YLFYLBaLGU7M8QJsAJ1pN+JIrJNyJsTIM/8ARyKSTh3DsEbSjBNFaSVp5NFDyT4BO8Cw106jnhemmu/AdKsaKWlyNYaAXbn2AYzTQuhehNMg1wZsy/UHoJeUGlFkgH3J7Ym5P8RVTo00/eYGgFfJr6TZiGIS+AY+gTESxsSIYk3aVIEeUmykHTiWdlv1N0ZdTl4f1vGoOSh7yxcwkmYjilVpu0lC6B62qtks6xmVtne0oJFygJVUrWCRlY8OnWZGzFT9bgutZsQv1kgY2BExLa0OYBk4lU1IxQmwBORMxEPQcrwaKQKXmtXiAugPyUgxgJYnIzlXx1fAFNAloT47byR3DmQNjQTV8mRkSXLXQKFBvuASmDc0ElTrt697+aG2+3oOJafeTjNmpM2BoZEgWiXXcZ6Kh11TDW8z8gGOFGFrlZF6q7hGxm/nhRiNFOraVsWEOtZoqQ237v+k2QClFbvWotx402QCdssRWrWZ89t5nFpZWbvVzbcy2fISs66Oz3wuv4loFV0CjUKt632GJmLTysnQqw/+N4kjYCGEkUhSy2KxkDJ+ANIID7KqwJZkAAAAAElFTkSuQmCC'>
                            <p>Dispatched/In Transit Sales</p>
                        </div>
                    </div>
                    <p class='overview_num'>
                        " + alldata.Item4 + @"
                    <p class='overview_text'>Rupees</p>
                </div>
                <div class='overview_card'>
                    <div class='overview_top'>
                        <div>
                            <img src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADIAAAAyCAYAAAAeP4ixAAAACXBIWXMAAAsTAAALEwEAmpwYAAAEj0lEQVR4nO2ZXYiWRRTHf2+uKaalLyhCCWFoRVFk5BLKxqYLXgSBFtSdWGgR5o0SQVEqtey1Wt0UQSnirhgR4WrSB4RemNRVuWWQlUqWmUabH/nIWf4Ts+PzNe37PLuQfxh25+uc83/mzDkz80I6GsAaYAC4BCSjXC7JlmdlW2msGQPGJxlldQyRAU1aFvsFKkIDeEQ2HY6Z6NxpLJBwaHhuVhpuGccakli7rhKpGMnVFfm/rMhkoAN4AtgIbAHeAnYAu4FPVfrV9jbwBtANPA0sAmaNFpHrlDAPABdblOT+BPYDG4A76yByD/CT138B+AJ4B3gJeAZYATwKLAEeUFmithUa8wLwJvA5cDKF2IfAjSMhYklnmkqYDK3+vQTtl2G2Oq1AE3hIrvm7dHwLXBNh37/oAk54rI8Di73+u9T+NTCO6tAEjkjX3QX2WdswNIJBrhzzmC9TWx/Vo88765W1bwgzvM47VFzd+gzrVO+pgUiPdK2LsG8IM72OmSl1FDatvqoGIk9J1+sR9pUeuEd1f99UhS7p6q+CiNuAs2sgcot0fddqIm3AeeWN8TUQafP0tcUQKdpMs/W/rUpdOOJ5QOnN3lBczgpvzmf/1tFkuyKLnZeWAg8C8zylLmlN8HRM8Nrdx5mnuUslq0eyD0hXIt1F9g1DVzD4mA51KFIlo1RWyYbFOfZdgawjgIvr9ne+jidrgU1AL7AXOKjNacpOqQx6ige99uMae1BzeyVrrWTPD3QW2ZeJ8HDmMu1j1IfHpbM3x65ChBMOqW5fqi60S+ehHLsK4Sb8Gvjr9JyD3jbgdITv29itmpuG6cF435ZSWJmh2DJ7FraNYDO/myN3T8acJ8ss50WVboXIMpvqN80ZFtMLMENzbK5hqs5Y7wFHgb8UII6qbT2wGfhH8+7LE75LjF8lDoMqsbA554AXdd1NCsofXgjemSf4ZHCestfvsykCzwaPySMh4svtl9vcqtunlTl63NgdjP0l5s03jYRPZiREZuksZbK+ARaWmHO/xiay9aasgWFUyIoSYXsskWuVDE3Gx9ofZXE9sM97OzBZLSMSu9mf8+7/MSQcpmquyXieEgbmudYZb9zW/xh6y7hTFjokw448NxQRWZ1B5ozepxyaygcxCdHd/mLQruLQL1nLi4hUge3SYZEoBu36UKd1H0ERLjUU10HE+baFWB/2IjmpgESi5Og2+G1ZP8clNZYpnt6HdbXdl0LGJ7EriFJTUvZr7UTsJd9hLvCz2j/z+u7VZrb2D4IbpwvFLuOnEqkSAxmuZXVHxlam01uJnRmPHoWuVSX6ck6wc4PX/kTjxxec1PtGg8jygmvBHOBH73aY9/z0UVH4rat05JB5TW9aWeiUjFNpp4O6iRzWQ0Ismvr9JNFxh1jXapXrTQS+lKxPIsk0FdmiDo2x/TG42dsLA0qIRej0VuJCxs9zQ3Dhb0FK3wL1WVShhWS+8j7QXkWi25VLJuv/ld7GTsrY0V3Cr1+htbAk97KycxJRcu0wfzMybmXCL2CTU32yBbB9Ys+j7wM/pFyDc+24DHzHPhiDjlu0AAAAAElFTkSuQmCC'>
                            <p>Delivered</p>
                        </div>
                    </div>
                    <p class='overview_num'>
                        " + alldata.Item5 + @"
                    <p class='overview_text'>Rupees</p>
                </div>
            </div>
        </div>
    </div>");
        if (filteredData != null)
        {
            foreach (var item in filteredData)
            {
                //Product dataproduct=repository
                decimal Currency = Convert.ToDecimal(item.TxnAmount);
                string Amount = Currency.ToString("0.00");

                TotalAmount = TotalAmount + Convert.ToDecimal(Amount);

                DateTime dt = item.CreatedOn;
                var OrderDate = dt.ToString("MM/dd/yyyy");
                string Status = "";
                if (item.OrderCurrentStatus == 6)
                {
                    Status = "Ready to Ship";
                }
                if (item.OrderCurrentStatus == 8)
                {
                    Status = "Waiting for Pickup";
                }
                if (item.OrderCurrentStatus == 9)
                {
                    Status = "Dispatched";
                }
                if (item.OrderCurrentStatus == 3)
                {
                    Status = "Delivered";
                }
                //if (item.OrderCurrentStatus == 0)
                //{
                //    Status = "Delivered";
                //}


                strtd.Append(@"<tr><td></td><td>
												<a id='hypEdit' onclick='NavigatetoOrderDetailsPage(" + item.PaymentTransactionId + ")'  >" + item.PaymentTransactionId + @"</a><br>
												<span id='lblPartialShip'></span>
											</td><td>
												<span id='OrderDate'>" + OrderDate + @"</span>
												
											</td><td align='right'>                                                
												<span id='lblTotalPrice'><span class='WebRupee'>" + item.CurrencySymbol + @" </span>" + Amount + @"</span>
											</td><td>
													<span id='lblFirstName'><span >" + Status + @"</span></span>
											</td>                                          
			</tr>");
            }
            //<td>

            //                                        <span id='lblTotalProducts'><span class='QuantityTipsySpan'>" + item.Quantity + @"</span></span>

            //                                </td>
            StringBuilder strResult = new StringBuilder();
            strResult.Append(@"
		<div>                   
					
					<div >
							<div id='Main_dvGrid'>
								<div> 
				
								<div>
						
					   
						   
		<table class='activity_datatable' rules='all' id='ctl00_ctl00_Main_Main_grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
			<thead>
<tr>
		<th scope='col'></th>		
<th scope='col'>Order ID</th>

<th scope='col'>Order Date</th>

<th scope='col'><div id='divCurrency'>Amount</div></th>
<th scope='col'>Order Status</th>
			</tr></thead><tbody>
" + strtd + @"
</tbody></table>
</div></div></div>
</div></div></div>
</div>
<div class='action_bar text_right' style='display: block;' id='dvbtn'>
<input style='display: inline-block;'  value='Book Shipment' class='button_small greyishBtn fl_right' onclick='PopupAndClearControlsData();' type='button'>
			   
				<div class='clear'>
				</div>
			</div>
		</div>
<br/>
<div style='text-align:right'>Total Sale Amount: <input type='text' name='amount' id='lblTotalAmount' Value='" + TotalAmount + "' Readonly ></div>");

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strResult.ToString()

            };
        }
        else
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = ""

            };
        }

    }

    public Tuple<string, string, string, string, string> LoadDashboard(DateTime fromdate, DateTime todate)
    {
        string a = "", b = "", c = "", d = "", e = "";
        string constr = ConfigurationManager.ConnectionStrings["db_Zon_constr"].ConnectionString;
        //string query = "SELECT tpt.Payment_Transaction_Id, case when tpt.Delivered=1 then 'Deliverd' ELSE 'NOT' end as Delivered ,tui.Email_Id ,tui.Phone_Number ,tpi.Product_Name ,tsc.Super_Category_Name , tc.Category_Name ,tsubc.Sub_Category_Name ,tupt.Quantity,tupt.Product_Cost, (tupt.Quantity*tupt.Product_Cost) as cost , tpt.Txn_Status ,tpt.Payment_Mode , tpt.PGTxn_Id FROM [Tbl_Payment_Transactions] tpt join Tbl_User_Product_Transactions tupt on tpt.Payment_Transaction_Id =tupt.Payment_Transaction_Id join Tbl_Product_Info tpi on tpi.Product_Id =tupt.Product_Id left join Tbl_Super_Categories tsc on tsc.Super_Category_Id =tpi.Super_Category_Id left join Tbl_Categories tc on tc.Category_Id =tpi.Category_Id left join Tbl_Sub_Categories tsubc on tsubc.Sub_Category_Id =tpi.Sub_Category_Id join Tbl_User_Info tui on tui.User_Id =tpt.User_Id where tpt.Payment_Transaction_Id > "+ Orderid + "";
        //query += "SELECT tpt.Payment_Transaction_Id, case when tpt.Delivered=1 then 'Deliverd' ELSE 'NOT' end as Delivered ,tui.Email_Id ,tui.Phone_Number ,tui.User_Name ,TPT.Txn_Date, tpt.Txn_Status ,TPT.Txn_Amount,TPT.Payment_Mode , tpt.PGTxn_Id ,tpt.Txn_Amount ,tua.Address,Locality, LandMark,Pincode FROM [Tbl_Payment_Transactions] tpt join Tbl_User_Info tui on tui.User_Id =tpt.User_Id join Tbl_User_Address tua on tua.Address_Id =tpt.Address_Id where tpt.Payment_Transaction_Id >"+ Orderid + "";
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("GET_REPORT_New"))
            {
                int i = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@fromdate", SqlDbType.DateTime).Value = fromdate;
                cmd.Parameters.Add("@todate", SqlDbType.DateTime).Value = todate;
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {

                        sda.Fill(ds);
                        for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                        {
                            if (i == 0)
                                a = Convert.ToInt32(ds.Tables[0].Rows[i]["Value"]).ToString();
                            else if (i == 1)
                                b = Convert.ToInt32(ds.Tables[0].Rows[i]["Value"]).ToString();
                            else if (i == 2)
                                c = Convert.ToInt32(ds.Tables[0].Rows[i]["Value"]).ToString();
                            else if (i == 3)
                                d = Convert.ToInt32(ds.Tables[0].Rows[i]["Value"]).ToString();
                            else if (i == 4)
                                e = Convert.ToInt32(ds.Tables[0].Rows[i]["Value"]).ToString();
                        }

                    }

                }
            }
        }
        return Tuple.Create(a, b, c, d, e);
    }
    public CustomResponse GetSalesReport(int rows, string FromDate, string ToDate, int CurrentStatus)
    {

        var repository = new PaymentTransactionRepository();
        int totalRecords;
        DateTime FD;
        DateTime TD;
        try
        {
            FD = Convert.ToDateTime(FromDate);
            TD = Convert.ToDateTime(ToDate).AddDays(1);
        }
        catch (Exception ex)
        {
            string DateFrom = FromDate;
            string[] From = DateFrom.Split('/');
            DateFrom = From[1] + "/" + From[0] + "/" + From[2];
            FD = Convert.ToDateTime(DateFrom);

            string DateTo = ToDate;
            string[] To = DateTo.Split('/');
            DateTo = To[1] + "/" + To[0] + "/" + To[2];
            TD = Convert.ToDateTime(DateTo).AddDays(1);
        }

        List<PaymentTransaction> data = null;
        if (CurrentStatus == 0)
        {
            data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, rows, (p => p.TxnStatus == "SUCCESS" && p.CreatedOn >= FD && p.CreatedOn <= TD && p.OrderCurrentStatus != 0 && p.OrderCurrentStatus != 1), null, "", true);
        }
        else if (FromDate != null && ToDate != null)
        {
            if (CurrentStatus == -1)
            {
                data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, rows, (p => (p.TxnStatus != "SUCCESS" || p.TxnStatus == null) && p.CreatedOn >= FD && p.CreatedOn <= TD), null, "", true);
            }
            else if (CurrentStatus != null)
            {
                if (CurrentStatus == 2)
                {
                    data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, rows, (p => p.CreatedOn >= FD && p.CreatedOn <= TD && p.OrderCurrentStatus != 0 && p.OrderCurrentStatus != 1), null, "", true);
                }
                else
                {
                    data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, rows, (p => p.CreatedOn >= FD && p.CreatedOn <= TD && p.OrderCurrentStatus == CurrentStatus && p.OrderCurrentStatus != 0 && p.OrderCurrentStatus != 1), null, "", true);
                }
            }
        }
        else
        {
            data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, rows, (p => p.OrderCurrentStatus == CurrentStatus), null, "", true);
        }
        var filteredData = (from q in (data as List<PaymentTransaction>)
                            select new
                            {
                                q.PaymentTransactionId,
                                q.PaymentStatus,
                                q.OrderCurrentStatus,
                                q.TxnAmount,
                                q.TxnStatus,
                                q.CreatedOn,
                                q.CurrencySymbol,

                            }).ToList();
        decimal TotalAmount = 0;
        StringBuilder strtd = new StringBuilder();
        Tuple<string, string, string, string, string> alldata = LoadDashboard(FD, TD);
        strtd.Append(@"<div class='row' style='background: #20bdc6;'>
        <div class='overview'>
            <h2>Dashboard Overview</h2>
            <p class='overview_total'>Overall Sales:
                " + alldata.Item1 + @"
            <div class='overview_content'>
                <div class='overview_card'>
                    <div class='overview_top'>
                        <div>
                            <img src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADIAAAAyCAYAAAAeP4ixAAAACXBIWXMAAAsTAAALEwEAmpwYAAACj0lEQVR4nO2YO2sWQRSGn4gEiVGIJouXykKwtdFOjaawECQhXTrxB6gowU6Txs5SsNDWVIop9AcEbNSoX8DCQqyiWMQboqIZGXgXhmXvd3FeWNjsvrszz845Z84X8PLy8vLqmQJgEVgFvul4DiwAEw2OawocmZoFvqS84DMw03eQWWBTxvvAcWC7jhPAA937A0zTnrYCy3lBAmclrqT45uX5BIzTvIaAuxrzYx6QRWclQg0Dt4GliPehvNdpXjc0ls3TI3lAXshgwymECJdzPeKd1HVbAJrURY3zCzita5kgYViNOnE5BnyIAdkhr32mrAIdSZpTvm7qnLIgodZjQHY6FayMbAl/BbwG9sbcPwX81BiXIvcyQVZlsNUpC+SkvM9KQNhVGDgTGkRW5qjywSg/KAqyIIMtsa6WYpI9zJ1rFSAGkfMAOORUpjuqWIVBJhQqRiU2SVfl2QB2V4AIItfWgHc6X1aOxinXhjijzc6oxE4qZ0YVTuFKWM/ZihBx9wywAoxQg6a12SW1Bhs1QkQ9a6qUtWlcm51N5q+qaE+VE2447coooXkgXO9+5+/czWFV7QFeppTQsMQaeay/iEwbIAeAN85gUZiqEGkgtQEeBt7rZU8yKlFWOHUGcswpBI/V3mftDfQN5Azw3emQtyWEUlI4FZ2YaQJkTh2ofcEtYEtG/xSXE52DXHB+NdrfLGU72k5B5vWQBblc9OE+gAwBN/XAb+Ac1dU6yDBwT+YfKf8xKbrErYKMAI9ktK3JVIcTM2VBxtR5Gm14duPjXwQ5L8Nb4GCaseMJmzyhZUvtvixTSxMzde8jfZuw8SD068ub/2ZFTM8mZjwI/frCxq8I/frCpu6q5eXl5UUv9BdfLei2z8d16wAAAABJRU5ErkJggg=='>
                            <p>Total Sales</p>
                        </div>
                    </div>
                    <p class='overview_num'>
                        " + alldata.Item2 + @"
                    <p class='overview_text'>Rupees</p>
                </div>
                <div class='overview_card'>
                    <div class='overview_top'>
                        <div>
                            <img src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADIAAAAyCAYAAAAeP4ixAAAACXBIWXMAAAsTAAALEwEAmpwYAAABr0lEQVR4nO2ZTytEURTAf7NgtkTznpFGvoGkZHwEyUY25jsgbJk1WcqCrX8R+QqYoSRKDVt2io3FjMWgW0fdJuUa4p7p/uou3uJO5zfn3s5554EbMXAOFFFMDJSAN+ACSKCQCLgWiRsgjUKiIOEJUciEJ0QhE54QNUImDFci8Z/rGbgHToBVYBxoRaHIZ6sCbAF99bQgJXn+a5qAFDAAzABHQFViegW2XY+8DzK1mLu7CJQlridgRKuMIQPsW9mZQrFMApgXERPbtGYZw4TImDWKcpkF6850aT9mBxLXruumlFX1bz2q+hmpMyauQddNdmYu8YcliWnvO5tikTjFH2Ipmi9AO8o5lqzkUM6siKyjnKyInKGcSEQeUE7SavlVk2wUkbhRjlZWRHyqb3UxJyJrKKcgIqa9V0tstShtKGa5nqbRN7qtNr4fpSSAQ5HYQTF5kXgEOlFKTgYPVdcZl4/HKW+Ng5xmW77RY90JIzKJMtLyXl6x7sQwntMsE5sheesr1AyxN4GOr37kY4z/0+ffXmVgA+h1/Td8EDEfeu7kc8IKMAa0uAoEAoEA6ngHcT8OClSo5xgAAAAASUVORK5CYII='>
                            <p>Total Returns</p>
                        </div>
                    </div>
                    <p class='overview_num'>
                        " + alldata.Item3 + @"
                    <p class='overview_text'>Rupees</p>
                </div>
                <div class='overview_card'>
                    <div class='overview_top'>
                        <div>
                            <img src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADIAAAAyCAYAAAAeP4ixAAAACXBIWXMAAAsTAAALEwEAmpwYAAAB+0lEQVR4nO2YOUsDQRiGHzAmWphYiGjAQksre8XSxsKjyl/wwtouXRpttLPS1s4fYOcNHo1XHVQ8SgVBdGXgi6whGXZ39sjKPPBB2G923n13vjk2YLFYLBaLGU7M8QJsAJ1pN+JIrJNyJsTIM/8ARyKSTh3DsEbSjBNFaSVp5NFDyT4BO8Cw106jnhemmu/AdKsaKWlyNYaAXbn2AYzTQuhehNMg1wZsy/UHoJeUGlFkgH3J7Ym5P8RVTo00/eYGgFfJr6TZiGIS+AY+gTESxsSIYk3aVIEeUmykHTiWdlv1N0ZdTl4f1vGoOSh7yxcwkmYjilVpu0lC6B62qtks6xmVtne0oJFygJVUrWCRlY8OnWZGzFT9bgutZsQv1kgY2BExLa0OYBk4lU1IxQmwBORMxEPQcrwaKQKXmtXiAugPyUgxgJYnIzlXx1fAFNAloT47byR3DmQNjQTV8mRkSXLXQKFBvuASmDc0ElTrt697+aG2+3oOJafeTjNmpM2BoZEgWiXXcZ6Kh11TDW8z8gGOFGFrlZF6q7hGxm/nhRiNFOraVsWEOtZoqQ237v+k2QClFbvWotx402QCdssRWrWZ89t5nFpZWbvVzbcy2fISs66Oz3wuv4loFV0CjUKt632GJmLTysnQqw/+N4kjYCGEkUhSy2KxkDJ+ANIID7KqwJZkAAAAAElFTkSuQmCC'>
                            <p>Dispatched/In Transit Sales</p>
                        </div>
                    </div>
                    <p class='overview_num'>
                        " + alldata.Item4 + @"
                    <p class='overview_text'>Rupees</p>
                </div>
                <div class='overview_card'>
                    <div class='overview_top'>
                        <div>
                            <img src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADIAAAAyCAYAAAAeP4ixAAAACXBIWXMAAAsTAAALEwEAmpwYAAAEj0lEQVR4nO2ZXYiWRRTHf2+uKaalLyhCCWFoRVFk5BLKxqYLXgSBFtSdWGgR5o0SQVEqtey1Wt0UQSnirhgR4WrSB4RemNRVuWWQlUqWmUabH/nIWf4Ts+PzNe37PLuQfxh25+uc83/mzDkz80I6GsAaYAC4BCSjXC7JlmdlW2msGQPGJxlldQyRAU1aFvsFKkIDeEQ2HY6Z6NxpLJBwaHhuVhpuGccakli7rhKpGMnVFfm/rMhkoAN4AtgIbAHeAnYAu4FPVfrV9jbwBtANPA0sAmaNFpHrlDAPABdblOT+BPYDG4A76yByD/CT138B+AJ4B3gJeAZYATwKLAEeUFmithUa8wLwJvA5cDKF2IfAjSMhYklnmkqYDK3+vQTtl2G2Oq1AE3hIrvm7dHwLXBNh37/oAk54rI8Di73+u9T+NTCO6tAEjkjX3QX2WdswNIJBrhzzmC9TWx/Vo88765W1bwgzvM47VFzd+gzrVO+pgUiPdK2LsG8IM72OmSl1FDatvqoGIk9J1+sR9pUeuEd1f99UhS7p6q+CiNuAs2sgcot0fddqIm3AeeWN8TUQafP0tcUQKdpMs/W/rUpdOOJ5QOnN3lBczgpvzmf/1tFkuyKLnZeWAg8C8zylLmlN8HRM8Nrdx5mnuUslq0eyD0hXIt1F9g1DVzD4mA51KFIlo1RWyYbFOfZdgawjgIvr9ne+jidrgU1AL7AXOKjNacpOqQx6ige99uMae1BzeyVrrWTPD3QW2ZeJ8HDmMu1j1IfHpbM3x65ChBMOqW5fqi60S+ehHLsK4Sb8Gvjr9JyD3jbgdITv29itmpuG6cF435ZSWJmh2DJ7FraNYDO/myN3T8acJ8ss50WVboXIMpvqN80ZFtMLMENzbK5hqs5Y7wFHgb8UII6qbT2wGfhH8+7LE75LjF8lDoMqsbA554AXdd1NCsofXgjemSf4ZHCestfvsykCzwaPySMh4svtl9vcqtunlTl63NgdjP0l5s03jYRPZiREZuksZbK+ARaWmHO/xiay9aasgWFUyIoSYXsskWuVDE3Gx9ofZXE9sM97OzBZLSMSu9mf8+7/MSQcpmquyXieEgbmudYZb9zW/xh6y7hTFjokw448NxQRWZ1B5ozepxyaygcxCdHd/mLQruLQL1nLi4hUge3SYZEoBu36UKd1H0ERLjUU10HE+baFWB/2IjmpgESi5Og2+G1ZP8clNZYpnt6HdbXdl0LGJ7EriFJTUvZr7UTsJd9hLvCz2j/z+u7VZrb2D4IbpwvFLuOnEqkSAxmuZXVHxlam01uJnRmPHoWuVSX6ck6wc4PX/kTjxxec1PtGg8jygmvBHOBH73aY9/z0UVH4rat05JB5TW9aWeiUjFNpp4O6iRzWQ0Ismvr9JNFxh1jXapXrTQS+lKxPIsk0FdmiDo2x/TG42dsLA0qIRej0VuJCxs9zQ3Dhb0FK3wL1WVShhWS+8j7QXkWi25VLJuv/ld7GTsrY0V3Cr1+htbAk97KycxJRcu0wfzMybmXCL2CTU32yBbB9Ys+j7wM/pFyDc+24DHzHPhiDjlu0AAAAAElFTkSuQmCC'>
                            <p>Delivered</p>
                        </div>
                    </div>
                    <p class='overview_num'>
                        " + alldata.Item5 + @"
                    <p class='overview_text'>Rupees</p>
                </div>
            </div>
        </div>
    </div>");
        if (filteredData != null)
        {
            foreach (var item in filteredData)
            {

                decimal Currency = Convert.ToDecimal(item.TxnAmount);
                string Amount = Currency.ToString("0.00");

                TotalAmount = TotalAmount + Convert.ToDecimal(Amount);

                DateTime dt = item.CreatedOn;
                var OrderDate = dt.ToString("MM/dd/yyyy");
                string Status = "";
                if (item.OrderCurrentStatus == 6)
                {
                    Status = "Ready to Ship";
                }
                if (item.OrderCurrentStatus == 8)
                {
                    Status = "Waiting for Pickup";
                }
                if (item.OrderCurrentStatus == 9)
                {
                    Status = "Dispatched";
                }
                if (item.OrderCurrentStatus == 3)
                {
                    Status = "Delivered";
                }
                if (item.OrderCurrentStatus == 5)
                {
                    Status = "Returned";
                }
                if (item.OrderCurrentStatus == 1)
                {
                    Status = "Cancelled";
                }
                if (item.OrderCurrentStatus == 0)
                {
                    Status = "Pending";
                }


                strtd.Append(@"<tr><td></td><td>
												<a id='hypEdit' onclick='NavigatetoOrderDetailsPage(" + item.PaymentTransactionId + ")'  >" + item.PaymentTransactionId + @"</a><br>
												<span id='lblPartialShip'></span>
											</td><td>
												<span id='OrderDate'>" + OrderDate + @"</span>
												
											</td><td align='right'>                                                
												<span id='lblTotalPrice'><span class='WebRupee'>" + item.CurrencySymbol + @" </span>" + Amount + @"</span>
											</td><td>
													<span id='lblFirstName'><span >" + Status + @"</span></span>
											</td>                                          
			</tr>");
            }
            //<td>

            //                                        <span id='lblTotalProducts'><span class='QuantityTipsySpan'>" + item.Quantity + @"</span></span>

            //                                </td>
            StringBuilder strResult = new StringBuilder();
            strResult.Append(@"
		<div>                   
					
					<div >
							<div id='Main_dvGrid'>
								<div> 
				
								<div>
						
					   
						   
		<table class='activity_datatable' rules='all' id='ctl00_ctl00_Main_Main_grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
			<thead>
<tr>
				<th scope='col'></th>
<th scope='col'>Order ID</th>
<th scope='col'>Order Date</th>

<th scope='col'><div id='divCurrency'>Amount</div></th>
<th scope='col'>Order Status</th>
			</tr></thead><tbody>
" + strtd + @"
</tbody></table>
</div></div></div>
</div></div></div>
</div>
<div class='action_bar text_right' style='display: block;' id='dvbtn'>
<input style='display: inline-block;'  value='Book Shipment' class='button_small greyishBtn fl_right' onclick='PopupAndClearControlsData();' type='button'>
			   
				<div class='clear'>
				</div>
			</div>
		</div>
<br/>
<div style='text-align:right'>Total Sale Amount: <input type='text' name='amount' id='lblTotalAmount' Value='" + TotalAmount + "' Readonly ></div>");

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strResult.ToString()

            };
        }
        else
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = ""

            };
        }

    }

    public CustomResponse GetProductSalesReport(int rows, string FromDate, string ToDate)
    {

        var repository = new PaymentTransactionRepository();
        int totalRecords;
        DateTime FD = Convert.ToDateTime(FromDate);
        DateTime TD = Convert.ToDateTime(ToDate).AddDays(1);

        List<PaymentTransaction> data = null;
        if (FromDate != null && ToDate != null)
        {
            data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, rows, (p => p.TxnStatus == "SUCCESS" && p.CreatedOn >= FD && p.CreatedOn <= TD), null, "", true);
        }
        else
        {
            data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, rows, (p => p.TxnStatus == "SUCCESS"), null, "", true);
        }
        var filteredData = (from q in (data as List<PaymentTransaction>)
                            select new
                            {
                                q.PaymentTransactionId,
                                q.PaymentStatus,
                                q.OrderCurrentStatus,
                                q.TxnAmount,
                                q.TxnStatus,
                                q.CreatedOn,
                                q.CurrencySymbol
                            }).ToList();
        decimal TotalAmount = 0;
        StringBuilder strtd = new StringBuilder();
        if (filteredData != null)
        {
            foreach (var item in filteredData)
            {

                decimal Currency = Convert.ToDecimal(item.TxnAmount);
                string Amount = Currency.ToString("0.00");

                TotalAmount = TotalAmount + Convert.ToDecimal(Amount);

                DateTime dt = item.CreatedOn;
                var OrderDate = dt.ToString("MM/dd/yyyy");
                string Status = "";
                if (item.OrderCurrentStatus == 6)
                {
                    Status = "Ready to Ship";
                }
                if (item.OrderCurrentStatus == 8)
                {
                    Status = "Waiting for Pickup";
                }
                if (item.OrderCurrentStatus == 9)
                {
                    Status = "Dispatched";
                }
                if (item.OrderCurrentStatus == 3)
                {
                    Status = "Delivered";
                }
                //if (item.OrderCurrentStatus == 0)
                //{
                //    Status = "Delivered";
                //}


                strtd.Append(@"<tr><td></td><td>
												<a id='hypEdit' onclick='NavigatetoOrderDetailsPage(" + item.PaymentTransactionId + ")'  >" + item.PaymentTransactionId + @"</a><br>
												<span id='lblPartialShip'></span>
											</td><td>
												<span id='OrderDate'>" + OrderDate + @"</span>
												
											</td><td align='right'>                                                
												<span id='lblTotalPrice'><span class='WebRupee'>" + item.CurrencySymbol + @" </span>" + Amount + @"</span>
											</td><td>
													<span id='lblFirstName'><span >" + Status + @"</span></span>
											</td>                                          
			</tr>");
            }
            //<td>

            //                                        <span id='lblTotalProducts'><span class='QuantityTipsySpan'>" + item.Quantity + @"</span></span>

            //                                </td>
            StringBuilder strResult = new StringBuilder();
            strResult.Append(@"
		<div>                   
					
					<div >
							<div id='Main_dvGrid'>
								<div> 
				
								<div>
						
					   
						   
		<table class='activity_datatable' rules='all' id='ctl00_ctl00_Main_Main_grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
			<thead>
<tr>
				<th scope='col'></th>
<th scope='col'>Order ID</th>
<th scope='col'>Order Date</th>

<th scope='col'><div id='divCurrency'>Amount</div></th>
<th scope='col'>Order Status</th>
			</tr></thead><tbody>
" + strtd + @"
</tbody></table>
</div></div></div>
</div></div></div>
</div>
<div class='action_bar text_right' style='display: block;' id='dvbtn'>
<input style='display: inline-block;'  value='Book Shipment' class='button_small greyishBtn fl_right' onclick='PopupAndClearControlsData();' type='button'>
			   
				<div class='clear'>
				</div>
			</div>
		</div>
<br/>
<div style='text-align:right'>Total Sale Amount: <input type='text' name='amount' id='lblTotalAmount' Value='" + TotalAmount + "' Readonly ></div>");

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strResult.ToString()

            };
        }
        else
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = ""

            };
        }

    }

    public CustomResponse GetUserCancelledOrders(int rows)
    {

        var repository = new PaymentTransactionRepository();
        int totalRecords;

        var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, rows, (p => p.TxnStatus == "CANCELED"), null, "", true);
        var filteredData = (from q in (data as List<PaymentTransaction>)
                            select new
                            {
                                q.User.FirstName,
                                q.User.LastName,
                                q.User.MobileNo,
                                q.User.EmailId,
                                q.User.UserId,
                                q.PaymentTransactionId,
                                Quantity = q.UserProductTransactions.Count(),
                                q.PaymentStatus,
                                q.TxnAmount,
                                q.TxnRefNo,
                                q.TxnStatus,
                                currency = q.CurrencyCode + " ( " + q.CurrencySymbol + " ) ",
                                q.CurrencyCode,
                                q.CurrencySymbol,
                                q.PGTxnId,
                                q.TxnMessage,
                                q.CreatedOn,
                                q.OrderCurrentStatus,
                                products = q.UserProductTransactions.Where(cat => cat.PaymentTransactionId == q.PaymentTransactionId).
                                  Select(u => string.Join(",", (u.Product.ProductName + (u.SubProduct == null ? "" : " - " + u.SubProduct.SPName)))).FirstOrDefault()

                            }).ToList();

        StringBuilder strtd = new StringBuilder();
        if (filteredData != null)
        {
            foreach (var item in filteredData)
            {
                decimal Currency = Convert.ToDecimal(item.TxnAmount);
                string Amount = Currency.ToString("0.00");
                DateTime dt = item.CreatedOn;
                var OrderDate = dt.ToString("MM/dd/yyyy");


                strtd.Append(@"<tr><td></td>
				<td>
												<a id='hypEdit' onclick='NavigatetoOrderDetailsPage(" + item.PaymentTransactionId + ")'  >" + item.PaymentTransactionId + @"</a><br>
												<span id='lblPartialShip'></span>
											</td><td>
												<span id='OrderDate'>" + item.products + @"</span>
												
											</td><td align='right' style='width:30px;'>
												
													<span id='lblTotalProducts'><span class='QuantityTipsySpan'>" + item.Quantity + @"</span></span>
												
											</td><td align='right' style='width:70px;'>                                                
												<span id='lblTotalPrice'><span class='WebRupee'>" + item.CurrencySymbol + @" </span>" + Amount + @"</span>
											</td><td>
													<span id='lblFirstName'><span >" + item.FirstName + @"</span></span>
											</td>
											<td>
												<span id='OrderDate'>" + OrderDate + @"</span>
												
											</td>
			</tr>");
            }
            //            <td>
            //                                                <div id='chkBlkShip' >
            //<input  id='chkkShip' name='chkBlkShip' value=" + item.PaymentTransactionId + @"  type='checkbox' class='Check' onchange='CheckboxChecked(this)' ></div>
            //                                                <div style='visibility: hidden;'>
            //                                                    <span id='lblTempOid'>" + item.PaymentTransactionId + @"</span>
            //                                                </div>

            //                                                <input  id='hdnShippingMode' class='hdnShippingMode' value='self' type='hidden'>
            //                                            </td>
            StringBuilder strResult = new StringBuilder();
            strResult.Append(@"
		<div>                   
					
					<div>
							<div id='Main_dvGrid'>
								<div>                                
								<div>                      
						  
						   
		<table class='activity_datatable' rules='all' id='ctl00_ctl00_Main_Main_grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
			<thead>
<tr>
<th></th>
				<th scope='col'>ID</th>
<th scope='col'>Product</th>
<th scope='col'>Qty</th>
<th scope='col'><div id='divCurrency'>Cost</div></th>
<th scope='col'>Customer</th>
<th scope='col'>Date</th>
			</tr></thead><tbody>
" + strtd + @"
</tbody></table>	</div>
	 </div>
						
						</div>
									</div>
							</div>
							
							
					</div>
				</div></div>       

		</div>");
            //<input style='display: inline-block;'  value='Book Shipment' class='button_small greyishBtn fl_right' onclick='PopupAndClearControlsData();' type='button'>

            //<th class='cp_product_select' scope='col'>                                                
            //                                                <span title='Select/Unselect all orders'>
            //<div id='chkSelectAll' class='checker'><span>
            //<input style='opacity: 0;' id='ctl00_ctl00_Main_Main_grdShippingOrders_ctl01_chkSelectAll'  onclick='selectUnselectCheckboxes(this.id, 'chkBlkShip', 'lblTempOid');' type='checkbox'></span></div></span>
            //                                            </th>
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strResult.ToString()

            };
        }
        else
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = ""

            };
        }

    }

    public CustomResponse GetUnAuthorizedOrders(int rows)
    {

        var repository = new PaymentTransactionRepository();
        int totalRecords;

        var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, rows, (p => p.TxnStatus == null), null, "", true);
        var filteredData = (from q in (data as List<PaymentTransaction>)
                            select new
                            {
                                q.User.FirstName,
                                q.User.LastName,
                                q.User.MobileNo,
                                q.User.EmailId,
                                q.User.UserId,
                                q.PaymentTransactionId,
                                Quantity = q.UserProductTransactions.Count(),
                                q.PaymentStatus,
                                q.TxnAmount,
                                q.TxnRefNo,
                                q.TxnStatus,
                                currency = q.CurrencyCode + " ( " + q.CurrencySymbol + " ) ",
                                q.CurrencyCode,
                                q.CurrencySymbol,
                                q.PGTxnId,
                                q.TxnMessage,
                                q.CreatedOn,
                                q.OrderCurrentStatus,
                                products = q.UserProductTransactions.Where(cat => cat.PaymentTransactionId == q.PaymentTransactionId).
                                  Select(u => string.Join(",", (u.Product.ProductName + (u.SubProduct == null ? "" : " - " + u.SubProduct.SPName)))).FirstOrDefault()

                            }).ToList().OrderByDescending(q => q.CreatedOn);

        StringBuilder strtd = new StringBuilder();
        if (filteredData != null)
        {
            foreach (var item in filteredData)
            {
                decimal Currency = Convert.ToDecimal(item.TxnAmount);
                string Amount = Currency.ToString("0.00");
                DateTime dt = item.CreatedOn;
                var OrderDate = dt.ToString("MM/dd/yyyy");


                strtd.Append(@"<tr><td></td>
				<td>
												<a id='hypEdit' onclick='NavigatetoOrderDetailsPage(" + item.PaymentTransactionId + ")'  >" + item.PaymentTransactionId + @"</a><br>
												<span id='lblPartialShip'></span>
											</td><td>
												<span id='OrderDate'>" + item.products + @"</span>
												
											</td><td align='right' style='width:30px;'>
												
													<span id='lblTotalProducts'><span class='QuantityTipsySpan'>" + item.Quantity + @"</span></span>
												
											</td><td align='right' style='width:70px;'>                                                
												<span id='lblTotalPrice'><span class='WebRupee'>" + item.CurrencySymbol + @" </span>" + Amount + @"</span>
											</td><td>
													<span id='lblFirstName'><span >" + item.FirstName + @"</span></span>
											</td>
											<td>
												<span id='OrderDate'>" + OrderDate + @"</span>
												
											</td>
										   
			</tr>");
            }
            //            <td>
            //                                                <div id='chkBlkShip' >
            //<input  id='chkkShip' name='chkBlkShip' value=" + item.PaymentTransactionId + @"  type='checkbox' class='Check' onchange='CheckboxChecked(this)' ></div>
            //                                                <div style='visibility: hidden;'>
            //                                                    <span id='lblTempOid'>" + item.PaymentTransactionId + @"</span>
            //                                                </div>

            //                                                <input  id='hdnShippingMode' class='hdnShippingMode' value='self' type='hidden'>
            //                                            </td>
            StringBuilder strResult = new StringBuilder();
            strResult.Append(@"
		<div>                   
					
					<div>
							<div id='Main_dvGrid'>
								<div>                                
								<div>                      
						  
						   
		<table class='activity_datatable' rules='all' id='ctl00_ctl00_Main_Main_grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
			<thead>
<tr>
<th></th>
				<th scope='col'>ID</th>
<th scope='col'>Product</th>
<th scope='col'>Qty</th>
<th scope='col'><div id='divCurrency'>Cost</div></th>
<th scope='col'>Customer</th>
<th scope='col'>Date</th>
			</tr></thead><tbody>
" + strtd + @"
</tbody></table>	</div>
	 </div>
						
						</div>
									</div>
							</div>
							
							
					</div>
				</div></div>       

		</div>");

            // <th scope='col'>Re-Order</th>
            //<td>
            //                                   <input type='button' value='Re-Order' onclick='ResendOrdertoUser(" + item.PaymentTransactionId + @")'/>                                                
            //                               </td>



            //<input style='display: inline-block;'  value='Book Shipment' class='button_small greyishBtn fl_right' onclick='PopupAndClearControlsData();' type='button'>

            //<th class='cp_product_select' scope='col'>                                                
            //                                                <span title='Select/Unselect all orders'>
            //<div id='chkSelectAll' class='checker'><span>
            //<input style='opacity: 0;' id='ctl00_ctl00_Main_Main_grdShippingOrders_ctl01_chkSelectAll'  onclick='selectUnselectCheckboxes(this.id, 'chkBlkShip', 'lblTempOid');' type='checkbox'></span></div></span>
            //                                            </th>
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strResult.ToString()

            };
        }
        else
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = ""

            };
        }

    }

    public CustomResponse MakeRepayment(int OrderID)
    {
        db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();
        PaymentTransaction OrderInfo = Entity.PaymentTransactions.Where(x => x.PaymentTransactionId == OrderID).First();
        var Pgway = new
        {
            TxnAmount = OrderInfo.TxnAmount,
            PaymentTransactionId = OrderInfo.PaymentTransactionId,
            EmailId = OrderInfo.User.EmailId,
            FirstName = OrderInfo.User.FirstName,
            LastName = OrderInfo.User.LastName,
            MobileNo = OrderInfo.User.MobileNo,
            StreetAddress1 = OrderInfo.UserProductTransactions.FirstOrDefault().UserAddress.StreetAddress1,
            City = OrderInfo.UserProductTransactions.FirstOrDefault().UserAddress.City,
            PinCode = OrderInfo.UserProductTransactions.FirstOrDefault().UserAddress.PinCode,
            StateName = OrderInfo.UserProductTransactions.FirstOrDefault().UserAddress.StateName,
            LandMark = OrderInfo.UserProductTransactions.FirstOrDefault().UserAddress.LandMark
        };


        return new CustomResponse
        {
            Result = Pgway
        };
    }

    public CustomResponse ResendOrdertoUser(int OrderID)
    {
        var repository = new PaymentTransactionRepository();
        var PaymntDtls = repository.First(p => p.PaymentTransactionId == OrderID);

        var rep = new UserProductTransactionRepository();
        var OrderdPrdcts = rep.First(o => o.PaymentTransactionId == PaymntDtls.PaymentTransactionId);

        var Repsitory = new UserRepository();
        var UsrDtls = Repsitory.First(u => u.UserId == PaymntDtls.User.UserId);

        int Transactionid = Convert.ToInt32(PaymntDtls.PaymentTransactionId);

        Utility.MailMessage ms = new Utility.MailMessage();
        ms.Subject = "Order Pending - Your Order with Healthurwealth.com [" + PaymntDtls.PaymentTransactionId + "] is Pending";
        ms.To = UsrDtls.EmailId;

        string domain;
        Uri url = HttpContext.Current.Request.Url;
        domain = url.AbsoluteUri.Replace(url.PathAndQuery, string.Empty);

        string body = string.Empty;
        string htmlPagePath = Shared.GethtmlPage(Shared.ProductStatusForSendMail.ResendOrder);
        using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("" + htmlPagePath + "")))
        {
            body = reader.ReadToEnd();
        }

        db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();
        string PromocodeData = "";
        string PromocodeAmount = "";
        if (PaymntDtls.Has_Promo_Code == true)
        {
            Tbl_Promo_Codes PromoData = Entity.Tbl_Promo_Codes.Where(x => x.Promo_Code_ID == PaymntDtls.Promo_Code_ID).First();
            PromocodeData = "Your Applied Promocode : <span style='color:green;font-weight:bold;'>" + PromoData.PromoCode + "</span>";
            PromocodeAmount = "<tr><td colspan='2' style='margin:0px;padding:16px 0px 14px 16px;border-collapse:collapse;border-spacing:0px;text-align:right;font-size:14px;line-height:22px;color:#a1a2a5;border-bottom-width:1px;border-bottom-style:solid;border-bottom-color:#ebebeb;vertical-align:top;background-color:#f6f6f7'>Discount</td><td style='margin:0px;padding:16px 16px 14px;border-collapse:collapse;border-spacing:0px;text-align:right;font-size:14px;line-height:22px;color:#a1a2a5;border-bottom-width:1px;border-bottom-style:solid;border-bottom-color:#ebebeb;vertical-align:top;background-color:#f6f6f7;width:50px;'><span style='color:#54565c'>₹  " + PromoData.Amount + "</span></td></tr>";
        }
        body = body.Replace("##PromocodeData##", PromocodeData);
        body = body.Replace("##PromocodeAmount##", PromocodeAmount);
        body = body.Replace("##FirstName##", UsrDtls.FirstName);
        body = body.Replace("##OrderNo##", PaymntDtls.PaymentTransactionId.ToString());
        body = body.Replace("##OrderDate##", DateTime.Now.ToShortDateString());
        body = body.Replace("##PaymentMode##", PaymntDtls.PaymentMode);
        body = body.Replace("##StreetAddress1##", (Convert.ToString(UsrDtls.UserProductTransactions.FirstOrDefault().UserAddress.StreetAddress1)));
        body = body.Replace("##StreetAddress2##", (Convert.ToString(UsrDtls.UserProductTransactions.FirstOrDefault().UserAddress.StreetAddress2)));
        body = body.Replace("##LandMark##", (Convert.ToString(UsrDtls.UserProductTransactions.FirstOrDefault().UserAddress.LandMark)));
        body = body.Replace("##City##", (Convert.ToString(UsrDtls.UserProductTransactions.FirstOrDefault().UserAddress.City)));
        body = body.Replace("##State##", (Convert.ToString(UsrDtls.UserProductTransactions.FirstOrDefault().UserAddress.StateName)));
        body = body.Replace("##PinCode##", (Convert.ToString(UsrDtls.UserProductTransactions.FirstOrDefault().UserAddress.PinCode)));

        var dataa = BalUtility.GetCart();
        var dataaa = PaymntDtls.UserProductTransactions.ToList();
        StringBuilder strtrResult = new StringBuilder();
        var cartItems = (List<UserProductTransaction>)BalUtility.GetSession(Shared.Sessions.CustomerCart) ??
                            new List<UserProductTransaction>();

        var count = 0;
        var totalsum = cartItems.Sum(p => p.ProductCost);
        var orderSummary = (Caluclations)BalUtility.GetSession(Shared.Sessions.ShippingInfo);
        var calculations = BalUtility.GenerateOrderSummary();

        var PreviousTotal = 0;
        var Total = 0;
        var TotalAmount = 0;
        var GrandTotal = 0;
        var ShippingCharges = 0;

        foreach (var item in dataaa)
        {
            var sid = item.SubProductId == null ? 0 : item.SubProductId;
            strtrResult.Append(@"<tr>
									<td class='alignLeft prodImg'><img src='" + item.Product.ProductImgUrl.Replace("~/", "http://admin.healthurwealth.com/") + @"' style='width:80px;height:80px;' alt='Title' /></td>
									<td class='alignLeft prodDesc'>
										<h4>" + item.Product.ProductName + @" </h4>
										Quantity:" + item.Quantity + @" <br />                                        
									</td>
									<td class='alignRight'><span class='amount'>&#8377;" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.Quantity * item.Product.ProductCost), true) + @" </span></td>
								</tr>");

            Total = Convert.ToInt32(item.Quantity * item.Product.ProductCost);

            TotalAmount = PreviousTotal + Total;
            PreviousTotal = TotalAmount;
        }
        StringBuilder strMsg = new StringBuilder();

        if (TotalAmount >= 1000)
        {
            GrandTotal = TotalAmount + 0;
        }
        else if (TotalAmount < 1000)
        {
            ShippingCharges = 50;
            GrandTotal = TotalAmount + ShippingCharges;
        }
        body = body.Replace("##ProductList##", strtrResult.ToString());
        body = body.Replace("##Total##", TotalAmount.ToString());
        string ShippingCharge = String.Format("{0:0.##}", PaymntDtls.ShippingCharges.ToString());
        body = body.Replace("##ShippingCharges##", ShippingCharge);
        ms.Body = body.Replace("##GrandTotal##", PaymntDtls.TxnAmount.ToString());
        ms.IsBodyHtml = true;

        try
        {
            ms.SendMail();
        }
        catch (Exception ex)
        {

        }

        return new CustomResponse
        {
            Message = "Mail sent",
            Status = Shared.ResponseStatus.Success.ToString(),
            Result = ""
        };
    }

    public CustomResponse GetAdminCancelledOrders(int rows)
    {

        var repository = new PaymentTransactionRepository();
        int totalRecords;

        var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, rows, (p => p.TxnMessage == "Cancelled by Admin"), null, "", true);
        var filteredData = (from q in (data as List<PaymentTransaction>)
                            select new
                            {
                                q.User.FirstName,
                                q.User.LastName,
                                q.User.MobileNo,
                                q.User.EmailId,
                                q.User.UserId,
                                q.PaymentTransactionId,
                                Quantity = q.UserProductTransactions.Count(),
                                q.PaymentStatus,
                                q.TxnAmount,
                                q.TxnRefNo,
                                q.TxnStatus,
                                currency = q.CurrencyCode + " ( " + q.CurrencySymbol + " ) ",
                                q.CurrencyCode,
                                q.CurrencySymbol,
                                q.PGTxnId,
                                q.TxnMessage,
                                q.CreatedOn,
                                q.OrderCurrentStatus,
                                products = q.UserProductTransactions.Where(cat => cat.PaymentTransactionId == q.PaymentTransactionId).
                                  Select(u => string.Join(",", (u.Product.ProductName + (u.SubProduct == null ? "" : " - " + u.SubProduct.SPName)))).FirstOrDefault()

                            }).ToList();

        StringBuilder strtd = new StringBuilder();
        if (filteredData != null)
        {
            foreach (var item in filteredData)
            {
                decimal Currency = Convert.ToDecimal(item.TxnAmount);
                string Amount = Currency.ToString("0.00");
                DateTime dt = item.CreatedOn;
                var OrderDate = dt.ToString("MM/dd/yyyy");


                strtd.Append(@"<tr><td></td>
				<td>
												<a id='hypEdit' onclick='NavigatetoOrderDetailsPage(" + item.PaymentTransactionId + ")'  >" + item.PaymentTransactionId + @"</a><br>
												<span id='lblPartialShip'></span>
											</td><td>
												<span id='OrderDate'>" + item.products + @"</span>
												
											</td><td align='right' style='width:30px;'>
												
													<span id='lblTotalProducts'><span class='QuantityTipsySpan'>" + item.Quantity + @"</span></span>
												
											</td><td align='right' style='width:70px;'>                                                
												<span id='lblTotalPrice'><span class='WebRupee'>" + item.CurrencySymbol + @" </span>" + Amount + @"</span>
											</td><td>
													<span id='lblFirstName'><span >" + item.FirstName + @"</span></span>
											</td>
											<td>
												<span id='OrderDate'>" + OrderDate + @"</span>
												
											</td>
			</tr>");
            }
            //            <td>
            //                                                <div id='chkBlkShip' >
            //<input  id='chkkShip' name='chkBlkShip' value=" + item.PaymentTransactionId + @"  type='checkbox' class='Check' onchange='CheckboxChecked(this)' ></div>
            //                                                <div style='visibility: hidden;'>
            //                                                    <span id='lblTempOid'>" + item.PaymentTransactionId + @"</span>
            //                                                </div>

            //                                                <input  id='hdnShippingMode' class='hdnShippingMode' value='self' type='hidden'>
            //                                            </td>
            StringBuilder strResult = new StringBuilder();
            strResult.Append(@"
		<div >                   
					
					<div >
							<div id='Main_dvGrid'>
								<div> 
								<div>                      
						 
						   
		<table class='activity_datatable' rules='all' id='ctl00_ctl00_Main_Main_grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
			<thead>
<tr>
<th></th>
				<th scope='col'>ID</th>
<th scope='col'>Product</th>
<th scope='col'>Qty</th>
<th scope='col'><div id='divCurrency'>Cost</div></th>
<th scope='col'>Customer</th>
<th scope='col'>Date</th>
			</tr></thead><tbody>
" + strtd + @"
</tbody></table>	</div>

  <div >
					<div >
					</div>
					 
					</div>

	 </div>
						
						</div>
									</div>
							</div>
							
							
					</div>
				</div>
</div>        

		</div>");
            //<input style='display: inline-block;'  value='Book Shipment' class='button_small greyishBtn fl_right' onclick='PopupAndClearControlsData();' type='button'>

            //<th class='cp_product_select' scope='col'>                                                
            //                                                <span title='Select/Unselect all orders'>
            //<div id='chkSelectAll' class='checker'><span>
            //<input style='opacity: 0;' id='ctl00_ctl00_Main_Main_grdShippingOrders_ctl01_chkSelectAll'  onclick='selectUnselectCheckboxes(this.id, 'chkBlkShip', 'lblTempOid');' type='checkbox'></span></div></span>
            //                                            </th>
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strResult.ToString()

            };
        }
        else
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = ""

            };
        }

    }

    public dynamic GetShipmentDetails(int OrderID, string CourierName, string awb)
    {
        var y = BalUtility.Logmaintainance((int)Shared.OrderStatus.WaitingforPickup, "Authorized-->Pickup", "", OrderID.ToString());
        var repository = new PaymentTransactionRepository();

        var OrderDetails = repository.First(p => p.PaymentTransactionId == OrderID);

        long? UserID = OrderDetails.UserId;

        var Personal = new UserRepository();

        var PersonalDetails = Personal.First(p => p.UserId == UserID);

        var Address = new AddressRepository();

        var AddressDetails = Address.First(x => x.UserId == UserID);

        if (CourierName == "Aramex")
        {
            //ProcessedShipment[] ShipmentDetails = CreateShipment.ShippingOrders(PersonalDetails.FirstName, PersonalDetails.MobileNo.ToString(), AddressDetails.StreetAddress1, AddressDetails.StreetAddress2, AddressDetails.LandMark, AddressDetails.City, Convert.ToInt32(AddressDetails.PinCode), OrderDetails.TxnAmount, PersonalDetails.EmailId);
            //OrderDetails.ShipmentId = ShipmentDetails[0].ID;
            //OrderDetails.ShipmentURL = ShipmentDetails[0].ShipmentLabel.LabelURL;
            //OrderDetails.Comments = ShipmentDetails[0].ShipmentLabel.LabelURL;
        }
        else if (CourierName == "Ecom Express")
        {
            OrderDetails.ShipmentId = awb;
        }
        OrderDetails.PaymentTransactionId = OrderID;
        OrderDetails.ShipmentType = "Normal";
        OrderDetails.Location = AddressDetails.StreetAddress1;
        OrderDetails.CourierName = "Will be updated soon";
        OrderDetails.ShipmentDate = DateTime.Now;

        if (UpdateAuthorizedOrders(OrderDetails).Status == Shared.ResponseStatus.Success.ToString())
        {
            ShipmentMail(OrderID, PersonalDetails, OrderDetails);
        }
        return OrderDetails;
    }

    [HttpGet]
    public ShipmentInfo CreateShipmentEcom(int OrderID, string CourierName)
    {
        ShipmentInfo info = new ShipmentInfo();
        info.json_input = new json_input();
        info.json_input.ADDITIONAL_INFORMATION = new ADDITIONAL_INFORMATION();
        info.username = "ecomexpress";
        info.password = "Ke$3c@4oT5m6h#$";
        info.json_input.AWB_NUMBER = "106744291";
        info.json_input.ORDER_NUMBER = "106744291-001";
        info.json_input.PRODUCT = "PPD";
        info.json_input.CONSIGNEE = "Test API User";
        info.json_input.CONSIGNEE_ADDRESS1 = "H. No. A10";
        info.json_input.CONSIGNEE_ADDRESS2 = "Block-T";
        info.json_input.CONSIGNEE_ADDRESS3 = "Sector 39 Test";
        info.json_input.DESTINATION_CITY = "GURGAON";
        info.json_input.PINCODE = "111111";
        info.json_input.STATE = "DL";
        info.json_input.MOBILE = "1234567891";
        info.json_input.TELEPHONE = "0123456789";
        info.json_input.ITEM_DESCRIPTION = "Kids Bicycle";
        info.json_input.PIECES = "1";
        info.json_input.COLLECTABLE_VALUE = "0";
        info.json_input.DECLARED_VALUE = "4149.0";
        info.json_input.ACTUAL_WEIGHT = "0.5";
        info.json_input.VOLUMETRIC_WEIGHT = "0";
        info.json_input.LENGTH = "12";
        info.json_input.BREADTH = "5";
        info.json_input.HEIGHT = "2";
        info.json_input.PICKUP_NAME = "Pickup name 1";
        info.json_input.PICKUP_ADDRESS_LINE1 = "Pickup Addr 1 Changed";
        info.json_input.PICKUP_ADDRESS_LINE2 = "Pickup Addr 2 Changed";
        info.json_input.PICKUP_PINCODE = "111111";
        info.json_input.PICKUP_PHONE = "0123456789";
        info.json_input.PICKUP_MOBILE = "1234567891";
        info.json_input.RETURN_NAME = "Test Return Name 1";
        info.json_input.RETURN_ADDRESS_LINE1 = "Test Return Addr 1 Changed";
        info.json_input.RETURN_ADDRESS_LINE2 = "Test Return Addr 2 Changed";
        info.json_input.RETURN_MOBILE = "1234567891";
        info.json_input.RETURN_PHONE = "0123456789";
        info.json_input.RETURN_PINCODE = "111111";
        info.json_input.ADDONSERVICE = "";
        info.json_input.DG_SHIPMENT = "false";
        info.json_input.ADDITIONAL_INFORMATION.SELLER_TIN = "SELLER_TIN_1234";
        info.json_input.ADDITIONAL_INFORMATION.SELLER_GSTIN = "GISTN988787";
        info.json_input.ADDITIONAL_INFORMATION.INVOICE_NUMBER = "INVOICE_1234";
        info.json_input.ADDITIONAL_INFORMATION.INVOICE_DATE = "03APR2017";
        info.json_input.ADDITIONAL_INFORMATION.ESUGAM_NUMBER = "eSUGAM_1234";
        info.json_input.ADDITIONAL_INFORMATION.ITEM_CATEGORY = "ELECTRONICS";
        info.json_input.ADDITIONAL_INFORMATION.PACKING_TYPE = "BOX";
        info.json_input.ADDITIONAL_INFORMATION.PICKUP_TYPE = "WH";
        info.json_input.ADDITIONAL_INFORMATION.RETURN_TYPE = "WH";
        info.json_input.ADDITIONAL_INFORMATION.PICKUP_LOCATION_CODE = "PICKUP_ADDR_002";
        info.json_input.ADDITIONAL_INFORMATION.GST_HSN = "HSN_BLAH_BLAH";
        info.json_input.ADDITIONAL_INFORMATION.GST_ERN = "ERN_BLAH_BLAH";
        info.json_input.ADDITIONAL_INFORMATION.GST_TAX_NAME = "DELHI GST";
        info.json_input.ADDITIONAL_INFORMATION.GST_TAX_BASE = "4149.0";
        info.json_input.ADDITIONAL_INFORMATION.DISCOUNT = "0.0";
        info.json_input.ADDITIONAL_INFORMATION.GST_TAX_RATE_CGSTN = "9.0";
        info.json_input.ADDITIONAL_INFORMATION.GST_TAX_RATE_IGSTN = "0.0";
        info.json_input.ADDITIONAL_INFORMATION.GST_TAX_RATE_SGSTN = "9.0";
        info.json_input.ADDITIONAL_INFORMATION.GST_TAX_CGSTN = "373.41";
        info.json_input.ADDITIONAL_INFORMATION.GST_TAX_SGSTN = "373.41";
        info.json_input.ADDITIONAL_INFORMATION.GST_TAX_IGSTN = "0.0";
        info.json_input.ADDITIONAL_INFORMATION.GST_TAX_TOTAL = "746.82";

        return info;
    }

    private static void ShipmentMail(int OrderID, Utility.User PersonalDetails, PaymentTransaction OrderDetails)
    {
        string ApiUrl = System.Configuration.ConfigurationManager.AppSettings["ApiUrl"];

        var repository1 = new PaymentTransactionRepository();
        var PaymntDtls = repository1.First(p => p.PaymentTransactionId == OrderID);
        Utility.MailMessage ms = new Utility.MailMessage();
        ms.Subject = "Shipment Created";

        ms.To = PersonalDetails.EmailId;
        ms.Cc = WebConfigurationManager.AppSettings["AdminEmail"].ToString();

        string domain;
        Uri url = HttpContext.Current.Request.Url;
        domain = url.AbsoluteUri.Replace(url.PathAndQuery, string.Empty);

        string body = string.Empty;
        string htmlPagePath = Shared.GethtmlPage(Shared.ProductStatusForSendMail.Shipment);
        using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("" + htmlPagePath + "")))
        {
            body = reader.ReadToEnd();
        }

        db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();
        string PromocodeData = "";
        string PromocodeAmount = "";
        if (PaymntDtls.Has_Promo_Code == true)
        {
            Tbl_Promo_Codes PromoData = Entity.Tbl_Promo_Codes.Where(x => x.Promo_Code_ID == PaymntDtls.Promo_Code_ID).First();
            PromocodeData = "Your Applied Promocode : <span style='color:green;font-weight:bold;'>" + PromoData.PromoCode + "</span>";
            PromocodeAmount = "<tr><td colspan='2' style='margin:0px;padding:16px 0px 14px 16px;border-collapse:collapse;border-spacing:0px;text-align:right;font-size:14px;line-height:22px;color:#a1a2a5;border-bottom-width:1px;border-bottom-style:solid;border-bottom-color:#ebebeb;vertical-align:top;background-color:#f6f6f7'>Promocode Amount</td><td style='margin:0px;padding:16px 16px 14px;border-collapse:collapse;border-spacing:0px;text-align:right;font-size:14px;line-height:22px;color:#a1a2a5;border-bottom-width:1px;border-bottom-style:solid;border-bottom-color:#ebebeb;vertical-align:top;background-color:#f6f6f7;width:50px;'><span style='color:#54565c'>₹  " + PromoData.Amount + "</span></td></tr>";
        }
        if (OrderDetails.ShipmentId == null)
        {
            OrderDetails.ShipmentId = "0";
        }
        body = body.Replace("##PromocodeData##", PromocodeData);
        body = body.Replace("##PromocodeAmount##", PromocodeAmount);
        body = body.Replace("##FirstName##", PersonalDetails.FirstName);
        body = body.Replace("##OrderNo##", Convert.ToString(PaymntDtls.PaymentTransactionId));
        body = body.Replace("##PaymentMode##", PaymntDtls.PaymentMode);
        body = body.Replace("##Shipper##", OrderDetails.CourierName);
        body = body.Replace("##ShipmentID##", OrderDetails.ShipmentId.ToString());
        body = body.Replace("##OrderDate##", OrderDetails.CreatedOn.ToShortDateString());
        body = body.Replace("##StreetAddress1##", (Convert.ToString(PersonalDetails.UserProductTransactions.FirstOrDefault().UserAddress.StreetAddress1)));
        body = body.Replace("##StreetAddress2##", (Convert.ToString(PersonalDetails.UserProductTransactions.FirstOrDefault().UserAddress.StreetAddress2)));
        body = body.Replace("##LandMark##", (Convert.ToString(PersonalDetails.UserProductTransactions.FirstOrDefault().UserAddress.LandMark)));
        body = body.Replace("##City##", (Convert.ToString(PersonalDetails.UserProductTransactions.FirstOrDefault().UserAddress.City)));
        body = body.Replace("##State##", (Convert.ToString(PersonalDetails.UserProductTransactions.FirstOrDefault().UserAddress.StateName)));
        body = body.Replace("##PinCode##", (Convert.ToString(PersonalDetails.UserProductTransactions.FirstOrDefault().UserAddress.PinCode)));

        var dataa = BalUtility.GetCart();
        var dataaa = PaymntDtls.UserProductTransactions.ToList();
        StringBuilder strtrResult = new StringBuilder();
        var cartItems = (List<UserProductTransaction>)BalUtility.GetSession(Shared.Sessions.CustomerCart) ??
                            new List<UserProductTransaction>();

        var count = 0;
        var totalsum = cartItems.Sum(p => p.ProductCost);
        var orderSummary = (Caluclations)BalUtility.GetSession(Shared.Sessions.ShippingInfo);
        var calculations = BalUtility.GenerateOrderSummary();

        var PreviousTotal = 0;
        var Total = 0;
        var TotalAmount = 0;
        var GrandTotal = 0;
        var ShippingCharges = 0;

        foreach (var item in dataaa)
        {

            var sid = item.SubProductId == null ? 0 : item.SubProductId;
            strtrResult.Append(@"<tr>
									<td class='alignLeft prodImg'><img src='" + item.Product.ProductImgUrl.Replace("~/", ApiUrl) + @"' style='width:80px;height:80px;' alt='Title' /></td>
									<td class='alignLeft prodDesc'>
										<h4 style='-webkit-margin-after: 5px;'>" + item.Product.ProductName + @" </h4>
										Quantity: " + item.Quantity + @" <br />
										
									</td>
									<td class='alignRight'><span class='amount'>&#8377;" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.Quantity * item.Product.ProductCost), true) + @" </span></td>
								</tr>");

            Total = Convert.ToInt32(item.Quantity * item.Product.ProductCost);

            TotalAmount = PreviousTotal + Total;
            PreviousTotal = TotalAmount;
        }
        StringBuilder strMsg = new StringBuilder();


        if (TotalAmount >= 1000)
        {
            GrandTotal = TotalAmount + 0;
        }
        else if (TotalAmount < 1000)
        {
            ShippingCharges = 50;
            GrandTotal = TotalAmount + ShippingCharges;
        }
        body = body.Replace("##ProductList##", strtrResult.ToString());
        body = body.Replace("##Total##", TotalAmount.ToString());
        body = body.Replace("##ShippingCharges##", PaymntDtls.ShippingCharges.ToString());
        ms.Body = body.Replace("##GrandTotal##", PaymntDtls.TxnAmount.ToString());
        ms.IsBodyHtml = true;

        ms.SendMail();
    }

    //End
    //present not using this 
    public CustomResponse GetPaymentPendingOrder(int rows)
    {

        var repository = new PaymentTransactionRepository();
        int totalRecords;
        var Repository = new ProductRepository();
        var Box = Repository.GetBoxNames();

        var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, rows, (p => p.TxnStatus == "Success" && p.Pickup == false), null, "", true);
        var filteredData = (from q in (data as List<PaymentTransaction>)
                            select new
                            {
                                q.User.FirstName,
                                q.User.LastName,
                                q.User.MobileNo,
                                q.User.EmailId,
                                q.User.UserId,
                                q.PaymentTransactionId,
                                Quantity = q.UserProductTransactions.Count(),
                                q.PaymentStatus,
                                q.TxnAmount,
                                q.TxnRefNo,
                                q.TxnStatus,
                                currency = q.CurrencyCode + " ( " + q.CurrencySymbol + " ) ",
                                q.CurrencyCode,
                                q.CurrencySymbol,
                                q.PGTxnId,
                                q.TxnMessage,
                                q.CreatedOn,
                                q.OrderCurrentStatus,
                                products = q.UserProductTransactions.Where(cat => cat.PaymentTransactionId == q.PaymentTransactionId).
                                  Select(u => string.Join(",", (u.Product.ProductName + (u.SubProduct == null ? "" : " - " + u.SubProduct.SPName)))).FirstOrDefault()

                            }).ToList();

        StringBuilder strtd = new StringBuilder();
        if (filteredData != null)
        {
            foreach (var item in filteredData)
            {
                decimal Currency = Convert.ToDecimal(item.TxnAmount);
                string Amount = Currency.ToString("0.00");
                DateTime dt = item.CreatedOn;
                var OrderDate = dt.ToString("MM/dd/yyyy");


                strtd.Append(@"<tr>
				<td>
												<div id='chkBlkShip' >
<input  id='chkkShip' name='chkBlkShip' value=" + item.PaymentTransactionId + @"  type='checkbox' class='Check' onchange='CheckboxChecked(this)' ></div>
												<div style='visibility: hidden;'>
													<span id='lblTempOid'>" + item.PaymentTransactionId + @"</span>
												</div>
												
												<input  id='hdnShippingMode' class='hdnShippingMode' value='self' type='hidden'>
											</td><td>
												<a id='hypEdit' onclick='NavigatetoOrderDetailsPage(" + item.PaymentTransactionId + ")'  >" + item.PaymentTransactionId + @"</a><br>
												<span id='lblPartialShip'></span>
											</td><td>
												<span id='OrderDate'>" + item.products + @"</span>
												
											</td><td>
												
													<span id='lblTotalProducts'><span class='QuantityTipsySpan'>" + item.Quantity + @"</span></span>
												
											</td><td align='right'>                                                
												<span id='lblTotalPrice'><span class='WebRupee'>" + item.CurrencySymbol + @" </span>" + Amount + @"</span>
											</td><td>
													<span id='lblFirstName'><span class='WebRupee'>" + item.FirstName + @"</span></span>
											</td>
											<td>
												<span id='OrderDate'>" + OrderDate + @"</span>
												
											</td>
			</tr>");
            }
            StringBuilder strResult = new StringBuilder();
            strResult.Append(@"
		<div class='widget'>                   
					
					<div class='widget_body'>
							<div id='Main_dvGrid' class='tab-grdpanel'>
								<div class='widget marg-0'> 
								<div class='widget_title'>
									<span class='iconsweet'>r</span><h5>Available Orders</h5>
								</div>
								<div class='widget_body'>
						
							<div class='cp_productlist_view'>
								<div class='products_views'>
									
								</div>
							</div>
						   
		<table class='activity_datatable' rules='all' id='ctl00_ctl00_Main_Main_grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
			<tbody>
<tr>
				<th class='cp_product_select' scope='col'>                                                
												<span title='Select/Unselect all orders'>
<div id='chkSelectAll' class='checker'><span>
<input style='opacity: 0;' id='ctl00_ctl00_Main_Main_grdShippingOrders_ctl01_chkSelectAll'  onclick='selectUnselectCheckboxes(this.id, 'chkBlkShip', 'lblTempOid');' type='checkbox'></span></div></span>
											</th><th scope='col'>Order ID</th>
<th scope='col'>Product</th>
<th scope='col'>Qty</th>
<th scope='col'><div id='divCurrency'>Order Value</div></th>
<th scope='col'>Customer Name</th>
<th scope='col'>Order Date</th>
			</tr>
" + strtd + @"
</tbody></table>	</div>

  <div class='cp_grdpager'>
					<div class='grdinfo'>
					  <label>Records per page</label>
					  <div id='ddlNoRow' class='selector'>
					<span>" + rows + @"</span>
<select style='opacity: 0;'  onchange='GetPaymentPendingOrder()' id='ddlNoRows' class='uniform_pager_list'>
		<option value='10' selected='selected'>5</option>
		<option value='5'>10</option>
		<option  value='10'>15</option>
		<option value='15'>20</option>
		<option value='20'>25</option>

	</select></div> 
		
						
					</div>
					  <div class='grd_pages'>
					  <label class='pages'>
			Records:
			<span id='ctl00_ctl00_Main_Main_usrGridPaging1_lblPageNo'>1</span>
			<span id='ctl00_ctl00_Main_Main_usrGridPaging1_lbl_of'>of</span>
			<span id='ctl00_ctl00_Main_Main_usrGridPaging1_lblPageCount'>1</span><span id='ctl00_ctl00_Main_Main_usrGridPaging1_lblPagestxt'> Pages</span></label>
			  
			 
<div id='dvHidden'>
	
	
	
</div>
					  </div>
					  <div class='clear'></div>
					</div>

	 </div>
						
						</div>
									</div>
							</div>
							
							
					</div>
				</div>
				<input name='ctl00$ctl00$Main$Main$hdnTodoSearchcriteria' id='ctl00_ctl00_Main_Main_hdnTodoSearchcriteria' value='[&quot;Order No&quot;,&quot;SKU&quot;,&quot;Email Id&quot;,&quot;Phone No&quot;,&quot;Coupon Code&quot;,&quot;Checkout Type&quot;]' type='hidden'>
				<img id='img441' alt='' onload='getdivCurrency();' src='Orders_files/designoption_009_13.jpg' height='1' width='1'>
			
</div>
		

<div class='action_bar text_right' style='display: block;' id='dvbtn'>
<input style='display: inline-block;'  value='Book Shipment' class='button_small greyishBtn fl_right' onclick='PopupAndClearControlsData();' type='button'>
			   
				<div class='clear'>
				</div>
			</div>
		</div>");
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strResult.ToString()

            };
        }
        else
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = ""

            };
        }

    }




    //  start Super Categories //
    public CustomResponse GetAllSuperCategories(int rows)
    {

        var repository = new SuperCategoryRepository();
        int totalRecords;

        var data = repository.FetchAllByPage(p => p.SuperCategoryId, out totalRecords, 0, rows, (p => p.SuperCategoryId != null), null, "", true);

        if (data.Count != 0)
        {
            StringBuilder strtd = new StringBuilder();

            foreach (var item in data)
            {


                strtd.Append(@"<tr>
				<td>
												<div id='chkBlkShip' >
<input  id='chkkShip' name='chkBlkShip' value=" + item.SuperCategoryId + @"  type='checkbox' class='Check' onchange='SuperCatCheckboxChecked(this)' ></div>
												<div style='visibility: hidden;'>
													<span id='lblTempOid'>" + item.SuperCategoryId + @"</span>
												</div>
												
												<input  id='hdnShippingMode' class='hdnShippingMode' value='self' type='hidden'>
											</td><td>
<span id='lblTempOid'>" + item.SuperCategoryId + @"</span>
												<br>
												<span id='lblPartialShip'></span>
											</td><td>
												<span id='OrderDate'>" + item.SuperCategoryName + @"</span>
												
											</td><td>
												
													<span id='lblTotalProducts'><span class='QuantityTipsySpan'>" + item.CreatedOn + @"</span></span>
												
											</td><td align='right'>                                                
												<span id='lblTotalPrice'>" + item.IsActive + @"</span>
											</td>			</tr>");
            }
            StringBuilder strResult = new StringBuilder();
            strResult.Append(@"
		<div class='widget'>                   
					
					<div class='widget_body'>
							<div id='Main_dvGrid' class='tab-grdpanel'>
								<div class='widget marg-0'> 
								<div class='widget_title'>
									<span class='iconsweet'>r</span><h5>Super Categories</h5>
								</div>
								<div class='widget_body'>
						
							<div class='cp_productlist_view'>
								<div class='products_views'>
									
								</div>
							</div>
						   
		<table class='activity_datatable' rules='all' id='ctl00_ctl00_Main_Main_grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
			<tbody>
<tr>
				<th class='cp_product_select' scope='col'>                                                
												<span title='Select/Unselect all orders'>
<div id='chkSelectAll' class='checker'><span>
<input style='opacity: 0;' id='ctl00_ctl00_Main_Main_grdShippingOrders_ctl01_chkSelectAll'  onclick='selectUnselectCheckboxes(this.id, 'chkBlkShip', 'lblTempOid');' type='checkbox'></span></div></span>
											</th><th scope='col'>Super CategoryID</th><th scope='col'>Super CategoryName</th><th scope='col'>Created On</th><th scope='col'><div id='divCurrency'>Is Active</div></th>
			</tr>
" + strtd + @"
</tbody></table>	</div>

  <div class='cp_grdpager'>
					<div class='grdinfo'>
					  <label>Records per page</label>
					  <div id='ddlNoRow' class='selector'>
					<span>" + rows + @"</span>
<select style='opacity: 0;'  onchange='GetSuperCategories()' id='ddlNoRows' class='uniform_pager_list'>
		<option value='10' selected='selected'>5</option>
		<option value='5'>10</option>
		<option  value='10'>15</option>
		<option value='15'>20</option>
		<option value='20'>25</option>

	</select></div> 
		
						
					</div>
					  <div class='grd_pages'>
					  <label class='pages'>
			Records:
			<span id='ctl00_ctl00_Main_Main_usrGridPaging1_lblPageNo'>1</span>
			<span id='ctl00_ctl00_Main_Main_usrGridPaging1_lbl_of'>of</span>
			<span id='ctl00_ctl00_Main_Main_usrGridPaging1_lblPageCount'>1</span><span id='ctl00_ctl00_Main_Main_usrGridPaging1_lblPagestxt'> Pages</span></label>
			  
			 
<div id='dvHidden'>
	
	
	
</div>
					  </div>
					  <div class='clear'></div>
					</div>

	 </div>
						
						</div>
									</div>
							</div>
							
							
					</div>
				</div>
				<input name='ctl00$ctl00$Main$Main$hdnTodoSearchcriteria' id='ctl00_ctl00_Main_Main_hdnTodoSearchcriteria' value='[&quot;Order No&quot;,&quot;SKU&quot;,&quot;Email Id&quot;,&quot;Phone No&quot;,&quot;Coupon Code&quot;,&quot;Checkout Type&quot;]' type='hidden'>
				<img id='img441' alt='' onload='getdivCurrency();' src='Orders_files/designoption_009_13.jpg' height='1' width='1'>
			
</div>
		

<div class='widget_body'>
			<div style='display: block;' class='action_bar text_right' id='dvbtn'>
<input value='Add SuperCategory' onclick='ClearContrlsAndAdd()' id='AddSuperCategory' class='button_small greyishBtn' type='button' />
<input style='display: inline-block;'  value='Edit SuperCategory' onclick='PopupAndEdit()' id='EditSuperCategory' class='button_small greyishBtn fl_right'  type='button'>
 
				<div class='clear'>
				</div>
			</div>
		</div>");




            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strResult.ToString()

            };
        }
        else
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = ""

            };
        }

    }

    public CustomResponse GetSuperCategory(int ID)
    {
        var repository = new SuperCategoryRepository();
        int totalRecords;
        List<SuperCategory> data = repository.FetchAllByPage(p => p.SuperCategoryId, out totalRecords, 0, 0, (p => p.SuperCategoryId == ID), null, "", true);

        //  List<SuperCategory> da = new List<SuperCategory>();

        SuperCategory result = new SuperCategory
        {
            SuperCategoryId = data.FirstOrDefault().SuperCategoryId,
            SuperCategoryName = data.FirstOrDefault().SuperCategoryName
        };

        if (data != null)
        {

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = data
            };
        }
        else
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = ""
            };
        }
    }

    public CustomResponse UpdateSuperCategory(SuperCategory data)
    {
        try
        {
            var repository = new SuperCategoryRepository();
            var updatedata = repository.First(p => p.SuperCategoryId == data.SuperCategoryId);
            updatedata.SuperCategoryName = data.SuperCategoryName;
            updatedata.IsActive = data.IsActive;
            updatedata.UpdatedOn = DateTime.Now;

            repository.Update(updatedata);


            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = data.SuperCategoryId
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };

        }
    }

    public CustomResponse AddSuperCategory(SuperCategory data)
    {
        try
        {
            var repository = new SuperCategoryRepository();
            repository.Insert(data);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Message = ""
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };

        }
    }
    //  End Super Categories //



    //  start  Categories //
    public CustomResponse GetAllCategories(int rows)
    {

        var repository = new CategoryRepository();
        int totalRecords;

        var data = repository.FetchAllByPage(p => p.CategoryId, out totalRecords, 0, rows, (p => p.CategoryId != null), null, "", true);

        if (data.Count != 0)
        {
            StringBuilder strtd = new StringBuilder();

            foreach (var item in data)
            {


                strtd.Append(@"<tr>
				<td>
												<div id='chkBlkShip' >
<input  id='chkkShip' name='chkBlkShip' value=" + item.CategoryId + @"  type='checkbox' class='Check' onchange='CatCheckboxChecked(this)' ></div>
												<div style='visibility: hidden;'>
													<span id='lblTempOid'>" + item.CategoryId + @"</span>
												</div>                                                
												<input  id='hdnShippingMode' class='hdnShippingMode' value='self' type='hidden'>
											</td><td>
											   <span id='lblTempOid'>" + item.CategoryId + @"</span>
												<br>
												<span id='lblPartialShip'></span>
											</td><td>
												<span id='OrderDate'>" + item.CategoryName + @"</span>
												
											</td>
<td > <span id='OrderDate'>" + item.SuperCategory.SuperCategoryName + @"</span></td>
<td>                                                
													<span id='lblTotalProducts'><span class='QuantityTipsySpan'>" + (item.CreatedOn) + @"</span></span>
												
											</td><td align='right'>                                                
												<span id='lblTotalPrice'>" + item.IsActive + @"</span>
											</td>			</tr>");
            }
            StringBuilder strResult = new StringBuilder();
            strResult.Append(@"
		<div class='widget'>                   
					
					<div class='widget_body'>
							<div id='Main_dvGrid' class='tab-grdpanel'>
								<div class='widget marg-0'> 
								<div class='widget_title'>
									<span class='iconsweet'>r</span><h5>Categories</h5>
								</div>
								<div class='widget_body'>
						
							<div class='cp_productlist_view'>
								<div class='products_views'>
									
								</div>
							</div>
						   
		<table class='activity_datatable' rules='all' id='ctl00_ctl00_Main_Main_grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
			<tbody>
<tr>
				<th class='cp_product_select' scope='col'>                                                
												<span title='Select/Unselect all orders'>
<div id='chkSelectAll' class='checker'><span>
<input style='opacity: 0;' id='ctl00_ctl00_Main_Main_grdShippingOrders_ctl01_chkSelectAll'  onclick='selectUnselectCheckboxes(this.id, 'chkBlkShip', 'lblTempOid');' type='checkbox'></span></div></span>
											</th><th scope='col'>CategoryID</th><th scope='col'>CategoryName</th><th scope='col'>SuperCategoryName</th><th scope='col'>Created On</th><th scope='col'><div id='divCurrency'>Is Active</div></th>
			</tr>
" + strtd + @"
</tbody></table>	</div>

  <div class='cp_grdpager'>
					<div class='grdinfo'>
					  <label>Records per page</label>
					  <div id='ddlNoRow' class='selector'>
					<span>" + rows + @"</span>
<select style='opacity: 0;'  onchange='GetCategories()' id='ddlNoRows' class='uniform_pager_list'>
		<option value='10' selected='selected'>5</option>
		<option value='5'>10</option>
		<option  value='10'>15</option>
		<option value='15'>20</option>
		<option value='20'>25</option>
<option value='45'>50</option>
		<option value='95'>100</option>
	</select></div> 
		
						
					</div>
					  <div class='grd_pages'>
					  <label class='pages'>
			Records:
			<span id='ctl00_ctl00_Main_Main_usrGridPaging1_lblPageNo'>1</span>
			<span id='ctl00_ctl00_Main_Main_usrGridPaging1_lbl_of'>of</span>
			<span id='ctl00_ctl00_Main_Main_usrGridPaging1_lblPageCount'>1</span><span id='ctl00_ctl00_Main_Main_usrGridPaging1_lblPagestxt'> Pages</span></label>
			  
			 
<div id='dvHidden'>
	
	
	
</div>
					  </div>
					  <div class='clear'></div>
					</div>

	 </div>
						
						</div>
									</div>
							</div>
							
							
					</div>
				</div>
				<input name='ctl00$ctl00$Main$Main$hdnTodoSearchcriteria' id='ctl00_ctl00_Main_Main_hdnTodoSearchcriteria' value='[&quot;Order No&quot;,&quot;SKU&quot;,&quot;Email Id&quot;,&quot;Phone No&quot;,&quot;Coupon Code&quot;,&quot;Checkout Type&quot;]' type='hidden'>
				<img id='img441' alt='' onload='getdivCurrency();' src='Orders_files/designoption_009_13.jpg' height='1' width='1'>
			
</div>
		

<div class='widget_body'>
			<div style='display: block;' class='action_bar text_right' id='dvbtn'>
<input value='AddCategory' onclick='ClearContrlsAndAdd()' id='AddCategory' class='button_small greyishBtn' type='button' />
<input style='display: inline-block;'  value='EditCategory' onclick='PopupAndEdit()' id='EditCategory' class='button_small greyishBtn fl_right'  type='button'>
 
				<div class='clear'>
				</div>
			</div>
		</div>");




            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strResult.ToString()

            };
        }
        else
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = ""

            };
        }

    }

    public CustomResponse AddCategory(Category data)
    {
        try
        {
            //SuperCategoryRepository rep = new SuperCategoryRepository();
            //var SuprCatdata = rep.First(s => s.SuperCategoryName == data.SuperCategoryName);          
            // Category obj = new Category();            
            // obj.SuperCategoryId = SuprCatdata.SuperCategoryId;
            // obj.CategoryName = data.CategoryName;
            // obj.IsActive = data.IsActive;
            var repository = new CategoryRepository();
            repository.Insert(data);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Message = ""
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };

        }
    }

    public CustomResponse GetCategory(int ID)
    {
        var repository = new CategoryRepository();
        int totalRecords;
        List<Category> data = repository.FetchAllByPage(p => p.CategoryId, out totalRecords, 0, 0, (p => p.CategoryId == ID), null, "", true);

        Category result = new Category
        {
            CategoryId = data.FirstOrDefault().CategoryId,
            CategoryName = data.FirstOrDefault().CategoryName
        };

        if (data != null)
        {

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = data
            };
        }
        else
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = ""
            };
        }
    }

    public CustomResponse UpdateCategory(Category data)
    {
        try
        {
            var repository = new CategoryRepository();
            var updatedata = repository.First(p => p.CategoryId == data.CategoryId);
            updatedata.CategoryName = data.CategoryName;
            updatedata.IsActive = data.IsActive;
            updatedata.UpdatedOn = DateTime.Now;

            repository.Update(updatedata);


            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = data.SuperCategoryId
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };

        }
    }

    //  End  Categories //



    //  start Sub Categories //
    public CustomResponse GetAllSubCategories(int rows)
    {

        var repository = new SubCategoryRepository();
        int totalRecords;

        var data = repository.FetchAllByPage(p => p.SubCategoryId, out totalRecords, 0, rows, (p => p.SubCategoryId != null), null, "", true);

        if (data.Count != 0)
        {
            StringBuilder strtd = new StringBuilder();

            foreach (var item in data)
            {


                strtd.Append(@"<tr>
				<td>
												<div id='chkBlkShip' >
<input  id='chkkShip' name='chkBlkShip' value=" + item.SubCategoryId + @"  type='checkbox' class='Check' onchange='SubCatCheckboxChecked(this)' ></div>
												<div style='visibility: hidden;'>
													<span id='lblTempOid'>" + item.SubCategoryId + @"</span>
												</div>                                                
												<input  id='hdnShippingMode' class='hdnShippingMode' value='self' type='hidden'>
											</td><td>
											   <span id='lblTempOid'>" + item.SubCategoryId + @"</span>
												<br>
												<span id='lblPartialShip'></span>
											</td><td>
												<span id='OrderDate'>" + item.SubCategoryName + @"</span>
												
											</td>
<td > <span id='OrderDate'>" + item.Category.CategoryName + @"</span></td>
<td>                                                
													<span id='lblTotalProducts'><span class='QuantityTipsySpan'>" + (item.CreatedOn) + @"</span></span>
												
											</td><td align='right'>                                                
												<span id='lblTotalPrice'>" + item.IsActive + @"</span>
											</td>			</tr>");
            }
            StringBuilder strResult = new StringBuilder();
            strResult.Append(@"
		<div class='widget'>                   
					
					<div class='widget_body'>
							<div id='Main_dvGrid' class='tab-grdpanel'>
								<div class='widget marg-0'> 
								<div class='widget_title'>
									<span class='iconsweet'>r</span><h5>Sub Categories</h5>
								</div>
								<div class='widget_body'>
						
							<div class='cp_productlist_view'>
								<div class='products_views'>
									
								</div>
							</div>
						   
		<table class='activity_datatable' rules='all' id='ctl00_ctl00_Main_Main_grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
			<tbody>
<tr>
				<th class='cp_product_select' scope='col'>                                                
												<span title='Select/Unselect all orders'>
<div id='chkSelectAll' class='checker'><span>
<input style='opacity: 0;' id='ctl00_ctl00_Main_Main_grdShippingOrders_ctl01_chkSelectAll'  onclick='selectUnselectCheckboxes(this.id, 'chkBlkShip', 'lblTempOid');' type='checkbox'></span></div></span>
											</th><th scope='col'>SubCategoryID</th><th scope='col'>SubCategoryName</th><th scope='col'>CategoryName</th><th scope='col'>Created On</th><th scope='col'><div id='divCurrency'>Is Active</div></th>
			</tr>
" + strtd + @"
</tbody></table>	</div>

  <div class='cp_grdpager'>
					<div class='grdinfo'>
					  <label>Records per page</label>
					  <div id='ddlNoRow' class='selector'>
					<span>" + rows + @"</span>
<select style='opacity: 0;'  onchange='GetSubCategories()' id='ddlNoRows' class='uniform_pager_list'>
		<option value='10' selected='selected'>5</option>
		<option value='5'>10</option>
		<option  value='10'>15</option>
		<option value='15'>20</option>
		<option value='20'>25</option>
		<option value='45'>50</option>
		<option value=95'>100</option>
	</select></div> 
		
						
					</div>
					  <div class='grd_pages'>
					  <label class='pages'>
			Records:
			<span id='ctl00_ctl00_Main_Main_usrGridPaging1_lblPageNo'>1</span>
			<span id='ctl00_ctl00_Main_Main_usrGridPaging1_lbl_of'>of</span>
			<span id='ctl00_ctl00_Main_Main_usrGridPaging1_lblPageCount'>1</span><span id='ctl00_ctl00_Main_Main_usrGridPaging1_lblPagestxt'> Pages</span></label>
			  
			 
<div id='dvHidden'>
	
	
	
</div>
					  </div>
					  <div class='clear'></div>
					</div>

	 </div>
						
						</div>
									</div>
							</div>
							
							
					</div>
				</div>
				<input name='ctl00$ctl00$Main$Main$hdnTodoSearchcriteria' id='ctl00_ctl00_Main_Main_hdnTodoSearchcriteria' value='[&quot;Order No&quot;,&quot;SKU&quot;,&quot;Email Id&quot;,&quot;Phone No&quot;,&quot;Coupon Code&quot;,&quot;Checkout Type&quot;]' type='hidden'>
				<img id='img441' alt='' onload='getdivCurrency();' src='Orders_files/designoption_009_13.jpg' height='1' width='1'>
			
</div>
		

<div class='widget_body'>
			<div style='display: block;' class='action_bar text_right' id='dvbtn'>
<input value='Add SubCategory' onclick='ClearContrlsAndAdd()' id='Add SubCategory' class='button_small greyishBtn' type='button' />
<input style='display: inline-block;'  value='Edit SubCategory' onclick='PopupAndEdit()' id='EditSubCategory' class='button_small greyishBtn fl_right'  type='button'>
 
				<div class='clear'>
				</div>
			</div>
		</div>");




            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strResult.ToString()

            };
        }
        else
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = ""

            };
        }

    }

    public CustomResponse AddSubCategory(SubCategory data)
    {
        try
        {
            var repository = new SubCategoryRepository();
            data.CreatedOn = System.DateTime.Now;
            data.UpdatedOn = System.DateTime.Now;
            repository.Insert(data);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Message = ""
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };

        }
    }

    public CustomResponse GetSubCategory(int ID)
    {
        var repository = new SubCategoryRepository();
        int totalRecords;
        List<SubCategory> data = repository.FetchAllByPage(p => p.SubCategoryId, out totalRecords, 0, 0, (p => p.SubCategoryId == ID), null, "", true);

        SubCategory result = new SubCategory
        {
            SubCategoryId = data.FirstOrDefault().SubCategoryId,
            SubCategoryName = data.FirstOrDefault().SubCategoryName
        };

        if (data != null)
        {

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = data
            };
        }
        else
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = ""
            };
        }
    }

    // Get the top 3 Discount images Created On 21/11/2014 and Created By Nagul 
    public CustomResponse GetDiscountImg()
    {
        var repository = new ProductRepository();
        var data = repository.GetDiscountProductList();
        //List<SubCategory> data = repository.FetchAllByPage(p => p.SubCategoryId, out  totalRecords, 0, 0, (p => p.SubCategoryId == ID), null, "", true);

        if (data != null)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = data
            };
        }
        else
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = ""
            };
        }
    }

    public CustomResponse UpdateSubCategory(SubCategory data)
    {
        try
        {
            var repository = new SubCategoryRepository();
            var updatedata = repository.First(p => p.SubCategoryId == data.SubCategoryId);
            updatedata.SubCategoryName = data.SubCategoryName;
            updatedata.IsActive = data.IsActive;
            updatedata.UpdatedOn = DateTime.Now;
            repository.Update(updatedata);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = data.CategoryId
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };

        }
    }

    //  End  Sub Categories //


    // Start (Dropdown List)Get SuperCategories and categories //
    [HttpGet]
    public CustomResponse GetSuperCategries()
    {
        var repository = new SuperCategoryRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.SuperCategoryId, out totalRecords, 0, 0, (p => p.IsActive && p.IsDeleted == false), c => new { c.SuperCategoryId, c.SuperCategoryName, c.IsActive }).ToList();
        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Message = "",
            Result = data

        };
    }

    [HttpGet]
    public CustomResponse GetCategries()
    {
        var repository = new CategoryRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.CategoryId, out totalRecords, 0, 0, (p => p.IsActive && p.IsDeleted == false), c => new { c.CategoryId, c.CategoryName, c.IsActive }).ToList();
        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Message = "",
            Result = data

        };
    }

    [HttpGet]
    public CustomResponse GetCategoriesBySuprCatID(int ID)
    {
        var repository = new CategoryRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.CategoryId, out totalRecords, 0, 0, (p => p.SuperCategoryId == ID), c => new { c.CategoryId, c.CategoryName, c.IsActive }).ToList();
        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Message = "",
            Result = data

        };
    }

    // End Get SuperCategories and categories //



    public CustomResponse UpdateAuthorizedOrders(PaymentTransaction data)
    {
        try
        {
            var repository = new PaymentTransactionRepository();
            var updatedata = repository.First(p => p.PaymentTransactionId == data.PaymentTransactionId);
            //var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out  totalRecords, 0, rows, (p => p.Dispatched == false && p.Delivered == false && p.Pickup == false && p.Authorized == false), null, "", true);
            updatedata.ShipmentType = data.ShipmentType;
            updatedata.Location = data.Location;
            updatedata.CourierName = data.CourierName;
            updatedata.OrderCurrentStatus = (int)Shared.OrderStatus.WaitingforPickup;
            updatedata.ShipmentDate = data.ShipmentDate;
            updatedata.ShipmentId = data.ShipmentId;
            updatedata.ShipmentURL = data.ShipmentURL;
            updatedata.Pickup = true;

            repository.Update(updatedata);
            var userproductrepository = new userproducttransactionRepository();
            var userproduct = userproductrepository.First(p => p.PaymentTransactionId == updatedata.PaymentTransactionId);
            var userrepository = new UserRepository();
            var userdata = userrepository.First(u => u.UserId == updatedata.UserId);
            var productrepository = new ProductRepository();
            var productdata = productrepository.First(p => p.ProductId == userproduct.ProductId);
            string Message = "Your Product " + productdata.ProductName + "(" + data.PaymentTransactionId + ") is ready to Pickup.Your Tracking Details will be sent shortly. Healthurwealth.com(SONAL ENTERPRISES)";
            string Url = System.Configuration.ConfigurationManager.AppSettings["SmsUrl"].ToString();
            string UserName = System.Configuration.ConfigurationManager.AppSettings["SmsId"].ToString();
            string password = System.Configuration.ConfigurationManager.AppSettings["SmsPwd"].ToString();
            string Status = Utility.MailMessage.SendSms(Url, UserName, password, userdata.MobileNo, Message, "N", "1707160960587275617");
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = data.PaymentTransactionId
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };

        }
    }


    public string idsCopy = "";
    public CustomResponse GetWaitingForPickOrder()
    {
        var repository = new PaymentTransactionRepository();
        int totalRecords;

        var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, 1000, (p => p.TxnStatus == "SUCCESS" && p.Dispatched == false && p.Delivered == false && p.Pickup == true && p.Authorized == true && (p.IsPacking == null || p.IsPacking == false)), null, "", true);
        var filteredData = (from q in (data as List<PaymentTransaction>)
                            select new
                            {
                                q.User.FirstName,
                                q.User.LastName,
                                q.User.MobileNo,
                                q.User.EmailId,
                                q.User.UserId,
                                q.ShipmentDate,
                                q.CourierName,
                                q.PaymentTransactionId,
                                Quantity = q.UserProductTransactions.First().Quantity,
                                q.PaymentStatus,
                                q.TxnAmount,
                                q.TxnRefNo,
                                q.TxnStatus,
                                currency = q.CurrencyCode + " ( " + q.CurrencySymbol + " ) ",
                                q.CurrencyCode,
                                q.CurrencySymbol,
                                q.PGTxnId,
                                q.TxnMessage,
                                q.CreatedOn,
                                q.ShipmentId,
                                q.PickupID,
                                q.OrderCurrentStatus,
                                q.PickupDate,
                                q.PaymentMode,
                                q.BoxId,
                                products = q.UserProductTransactions.Where(cat => cat.PaymentTransactionId == q.PaymentTransactionId).
                                  Select(u => string.Join(",", (u.Product.ProductName + (u.SubProduct == null ? "" : " - " + u.SubProduct.SPName))))

                            }).OrderByDescending(p => p.PickupDate).ToList();
        StringBuilder strtd = new StringBuilder();
        var Repository = new ProductRepository();

        var Box = Repository.GetBoxNames();

        if (data.Count > 0)
        {
            long[] ids = new long[data.Count];
            string[] courier = new string[data.Count];
            int i = 0;
            StringBuilder dtrdropdown = new StringBuilder();
            StringBuilder dtrdropdownselect = new StringBuilder();
            foreach (var item in filteredData)
            {
                dtrdropdown = new StringBuilder();
                dtrdropdown.Append("<option value=''>--Select--</option>");
                foreach (var dditem in Box)
                {
                    if (dditem.BoxId == item.BoxId)
                        dtrdropdown.Append("<option selected value='" + dditem.BoxId + "'>" + dditem.BoxName + "</option>");
                    else
                        dtrdropdown.Append("<option  value='" + dditem.BoxId + "'>" + dditem.BoxName + "</option>");
                }
                dtrdropdownselect = new StringBuilder();
                dtrdropdownselect.Append("<select class='departmentsDropdown' id='ddbox_" + item.PaymentTransactionId + @"' onchange='getValue(" + item.PaymentTransactionId + @",this.value)'>" + dtrdropdown + "</select>");
                ids[i] = item.PaymentTransactionId;
                courier[i] = item.CourierName;
                i++;
                decimal Currency = Convert.ToDecimal(item.TxnAmount);
                string Amount = Currency.ToString("0.00");
                DateTime dt = Convert.ToDateTime(item.ShipmentDate);
                string ShipmentDate = dt.ToString("dd/MMM/yyy");
                string status = item.OrderCurrentStatus.ToString();
                var status1 = Shared.GetOrderStatusEnum(status);
                string orderDate = item.CreatedOn.ToString("dd/MMM/yyy");
                string ProductName = "";
                string PaymentMode = item.PaymentMode;

                for (var j = 0; j < item.products.Count(); j++)
                {
                    if (j == 0)
                    {
                        ProductName = item.products.ElementAtOrDefault(j).ToString();
                    }
                    else
                    {
                        ProductName = ProductName + " ," + item.products.ElementAtOrDefault(j).ToString();
                    }
                }

                string bgrowcolor = "";
                if (item.ShipmentDate != null)
                {
                    if (item.ShipmentDate.Value.ToShortDateString() == DateTime.Now.ToShortDateString())
                    {
                        bgrowcolor = "#A9DFBF";
                    }
                    else if (item.ShipmentDate >= DateTime.Now.AddDays(-2))
                    {
                        bgrowcolor = "#F5CBA7";
                    }
                    else
                    {
                        bgrowcolor = "#E6B0AA";
                    }
                }

                var addressrepository = new AddressRepository();
                UserAddress address = addressrepository.UserAddress(item.UserId);

                strtd.Append(@"<tr style='background-color:" + bgrowcolor + @"'>
				<td>
												 <div id='chkBlkShip' >
<input  id='chkkShip' name='chkBlkShip' value=" + item.PaymentTransactionId + @"  type='checkbox' class='Check CheckPickUP' onchange='CheckboxChecked(this)' ></div>
												<div style='visibility: hidden;'>
													<span id='lblTempOid'>" + item.PaymentTransactionId + @"</span>
												</div>
												
												<input  id='hdnShippingMode' class='hdnShippingMode' value='self' type='hidden'>
											</td><td>
												<a id='hypEdit' onclick='NavigatetoOrderDetailsPage(" + item.PaymentTransactionId + ")'  >" + item.PaymentTransactionId + @"</a><br>
												<span id='lblPartialShip'></span>
											</td>
<td>
												<span id='Products'>" + orderDate + @"</span>   
											</td>
<td>
												<span id='Products'>" + ProductName + @"</span>   
											</td>
												<td>
												<span id='Products'>" + ShipmentDate + @"</span>   
											</td>
									   <td>
												
													<span id='lblTotalProducts'><span class='QuantityTipsySpan'>" + item.products.Count() + @"</span></span>
												
											</td><td align='right'>                                                
												<span id='lblTotalPrice'><span class='WebRupee'>" + item.CurrencySymbol + @" </span>" + Amount + @"</span>
											</td>
<td>
												<span id='Paymentmode'>" + PaymentMode + @"</span>   
											</td>
<td>
												<span class='iconsweet' style='font-family:'iconSweets;'></span>" + dtrdropdownselect + @"</td>
<td style='display:none'>
												<span id='OrderDate'>" + item.FirstName + @"</span>
												
											</td>
<td style='display:none'>
												<span id='OrderDate'>" + item.MobileNo + @"</span>
												
											</td>
<td style='display:none'>
												<span id='OrderDate'>" + item.EmailId + @"</span>
												
											</td>
<td style='display:none'>
<span id='userAddress'>" + address.StreetAddress1 + ", " + address.StreetAddress2 + ", " + address.LandMark + ", " + address.City + ", " + address.StateName + ", " + address.PinCode + @"</span>
</td>
			</tr>");
            }
            strtd.Append("<input id='ids' type='hidden' value='" + string.Join(",", ids) + "'>");
            HttpContext.Current.Session["OrderID"] = string.Join(",", ids);

            StringBuilder strResult = new StringBuilder();
            strResult.Append(@"
		<div>                   
					
					<div>
							<div id='Main_dvGrid'>
								<div> 
								
								<div>
						
							
		<table class='activity_datatable' rules='all' id='ctl00_ctl00_Main_Main_grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
			<thead>
<tr class='checked'>
				<th class='cp_product_select' scope='col'>                                                
												<span title='Select/Unselect all orders'>
<div id='chkSelectAll' class='checker'><span>
<input style='opacity: 0;' id='ctl00_ctl00_Main_Main_grdShippingOrders_ctl01_chkSelectAll'  onclick='selectUnselectCheckboxes(this.id, 'chkBlkShip', 'lblTempOid');' type='checkbox'></span></div></span>
											</th><th scope='col'>ID</th>
<th scope='col'>Order Date</th>
			<th scope='col'>Product</th>
			<th scope='col'>Date</th>
			<th scope='col'>Qty</th>
			<th scope='col'><div id='divCurrency'>Amount</div></th>
<th scope='col'>Paymentmode</th>
<th scope='col' style='display:none'>Customer</th>
<th scope='col' style='display:none'>Mobile</th>
<th scope='col' style='display:none'>Email</th>
<th scope='col' style='display:none'>Address</th>
			</tr></thead><tbody>
" + strtd + @"
</tbody></table>	    </div></div>                        
						</div></div>
						</div></div>
						</div></div>
<div class='widget_body'>

			<div style='display: block;' class='action_bar text_right' id='dvbtn'>

<input type='button' value='DownloadDelhiveryExcel' class='button_small greyishBtn fl_right' onclick='DownloadDelhiveryExcel()'/>
<input type='button' value='PickUp Request to Bluedart' class='button_small greyishBtn fl_right' onclick='DownloadBluedartExcel()'/>
<input type='button' value='PickUp Request to DTDC' class='button_small greyishBtn fl_right' onclick='DownloadDTDCExcel()'/>
<input type='button' value='DownloadShipRocketExcel' class='button_small greyishBtn fl_right' onclick='DownloadShipRocketExcel()'/>
<br>
<br><br>

<input style='display: inline-block;'  value='Assign Delhivery AWB' class='button_small greyishBtn fl_right' onclick='AssignDelhiveryAWBS();' type='button'>
<input style='display: inline-block;'  value='Advanced Pickup' class='button_small greyishBtn fl_right' onclick='Advancedpickup();' type='button'>
<input style='display: inline-block;'  value='Pickup Orders' class='button_small greyishBtn fl_right' onclick='PopupData();' type='button'>
<input type='button' value='DownloadPDF' class='button_small greyishBtn fl_right' onclick='DownloadPDF()'/>
			   
				<div class='clear'>
				</div>
			</div>
		</div>");

            //<option value='5' selected='selected'>5</option>
            //<option value='5'>10</option>
            //<option  value='10'>15</option>
            //<option value='15'>20</option>
            //<option value='20'>25</option>
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strResult.ToString()

            };
        }

        else
        {
            StringBuilder strResult = new StringBuilder();
            strResult.Append(@" <div id='ctl00_ctl00_Main_Main_divMessage' class='msgbar msg_Info hide_onC'>
								<span id='ctl00_ctl00_Main_Main_msgicon' class='iconsweet'>*</span>
								<p><span id='ctl00_ctl00_Main_Main_lblMessage'>There are no 'WaitingForPickUp Orders' found.</span></p>
							</div>");

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = strResult.ToString()
            };
        }
    }
    public long DeleteBoxpackaing(int BoxId)
    {
        using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
        {
            try
            {
                //Tbl_AdditionalInfo datatoupdate = Context.Tbl_AdditionalInfo.Where(x => x.ProductId == info.ProductId).FirstOrDefault();
                Tbl_Packing_Box BoxpackingData = context.Tbl_Packing_Box.Where(x => x.BoxId == BoxId).FirstOrDefault();
                BoxpackingData.IsActive = false;

                //context.CheckOutUserProductTransactions.Add(commentData);
                context.SaveChanges();
                return BoxpackingData.BoxId;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
    [System.Web.Http.AcceptVerbs("GET", "POST")]
    [System.Web.Http.HttpGet]
    public CustomResponse DeleteBoxPacking(int BoxId)
    {
        //var repository = new BoxInpackaingRepository();

        //long Inpackaing = repository.BoxInpackaing(OrderID, BoxId);
        long Boxpackaing = DeleteBoxpackaing(BoxId);

        if (Boxpackaing != 0)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString()
            };
        }
        else
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString()
            };
        }
    }
    public CustomResponse GetBox()
    {
        var repository = new PaymentTransactionRepository();
        int totalRecords;
        db_Zon_HuwEntities Context = new db_Zon_HuwEntities();

        List<Tbl_Packing_Box> data = new List<Tbl_Packing_Box>();
        data = (from p in Context.Tbl_Packing_Box
                select p).Where(x => x.IsActive == true).OrderByDescending(x => x.BoxId).ToList();
        StringBuilder strtd = new StringBuilder();
        StringBuilder strResult = new StringBuilder();
        if (data.Count > 0)
        {
            foreach (var item in data)
            {
                try
                {
                    db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();
                    StringBuilder ProductName = new StringBuilder();

                    strtd.Append(@"<tr><td>
												<a id='hypEdit'  onclick='FillBoxDetails(" + item.BoxId + ")' >" + item.BoxId + @"</a><br>
												<span id='lblPartialShip'></span>
											</td><td>
												<span >" + item.Created_On + @"</span>   
											</td>
												<td>
												<span >" + item.BoxName + @"</span>   
											</td>
<td>                                                
													<span id='lblTotalProducts'>" + item.Lengths + @"</span>                                                
											</td>
<td>                                                
													<span id='lblTotalProducts'>" + item.Height + @"</span>                                                
											</td>
											 <td>                                                
													<span id='lblstatus'>" + item.Width + @"</span>                                                
											</td>
											 <td>                                                 
                                                    <a id='hypdelete' onclick='DeleteBoxDetails(" + item.BoxId + ")'>" + "Delete" + @"</a><br>
											</td>
			</tr>");
                }
                catch (Exception ex)
                {

                }
            }

            //strResult.Append(@" <table class='activity_datatable' rules='all' id='grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'><tbody>" + strtd + @"</tbody></table>");


            //StringBuilder strResult = new StringBuilder();
            strResult.Append(@"
		<div>                   
					
					<div>
							<div id='Main_dvGrid'>
								<div> 
								
								<div>
						
							
		<table class='activity_datatable' rules='all' id='ctl00_ctl00_Main_Main_grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
			<thead>
<tr>
<th scope='col'>ID</th>
<th scope='col'>Date</th>
			<th scope='col'>Box Name</th>
			<th scope='col'>Lengths</th>
			<th scope='col'>Height</th>
<th scope='col'>Width</th>
<th scope='col'>Action</th>
			</tr></thead><tbody>
" + strtd + @"
</tbody></table>	    </div></div>                        
						</div></div>
						</div></div>
						</div></div>
<div class='widget_body'>

				<div class='clear'>
				</div>
			</div>
		</div>");

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strResult.ToString(),
                //CartSum = (skipRecords + data.Count()).ToString()
            };
        }
        else
        {
            strResult.Append(@" <div id='divMessage' class='msgbar msg_Info hide_onC'>
								<span id='msgicon' class='iconsweet'>*</span>
								<p><span id='lblMessage'>There are no 'Box' found.</span></p>
							</div>");

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = strResult.ToString(),
                //CartSum = (skipRecords + data.Count()).ToString()
            };
        }
    }


    // modify by shashi bind box
    public Tbl_Packing_Box GetBoxDetails(int Id)
    {
        using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
        {
            Tbl_Packing_Box box = Context.Tbl_Packing_Box.Where(x => x.BoxId == Id).FirstOrDefault();
            return box;
        }


    }
    public CustomResponse Getinpacking()
    {
        var repository = new PaymentTransactionRepository();
        int totalRecords;
        var Repository = new ProductRepository();
        var Box = Repository.GetBoxNames();
        var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, 1000, (p => p.TxnStatus == "SUCCESS" && p.Dispatched == false && p.Delivered == false && p.Pickup == true && p.Authorized == true && p.IsPacking == true), null, "", true);
        var filteredData = (from q in (data as List<PaymentTransaction>)
                            select new
                            {
                                q.User.FirstName,
                                q.User.LastName,
                                q.User.MobileNo,
                                q.User.EmailId,
                                q.User.UserId,
                                q.ShipmentDate,
                                q.CourierName,
                                q.PaymentTransactionId,
                                Quantity = q.UserProductTransactions.First().Quantity,
                                q.PaymentStatus,
                                q.TxnAmount,
                                q.TxnRefNo,
                                q.TxnStatus,
                                currency = q.CurrencyCode + " ( " + q.CurrencySymbol + " ) ",
                                q.CurrencyCode,
                                q.CurrencySymbol,
                                q.PGTxnId,
                                q.TxnMessage,
                                q.CreatedOn,
                                q.ShipmentId,
                                q.PickupID,
                                q.OrderCurrentStatus,
                                q.PickupDate,
                                q.PaymentMode,
                                q.BoxId,
                                products = q.UserProductTransactions.Where(cat => cat.PaymentTransactionId == q.PaymentTransactionId).
                                  Select(u => string.Join(",", (u.Product.ProductName + (u.SubProduct == null ? "" : " - " + u.SubProduct.SPName))))

                            }).OrderByDescending(p => p.PickupDate).ToList();
        StringBuilder strbutton = new StringBuilder();

        StringBuilder strtd = new StringBuilder();
        StringBuilder dtrdropdown = new StringBuilder();
        StringBuilder dtrdropdownselect = new StringBuilder();


        if (data.Count > 0)
        {
            long[] ids = new long[data.Count];
            string[] courier = new string[data.Count];
            int i = 0;

            foreach (var item in filteredData)
            {
                dtrdropdown = new StringBuilder();
                dtrdropdown.Append("<option value=''>--Select--</option>");
                foreach (var dditem in Box)
                {
                    if (dditem.BoxId == item.BoxId)
                        dtrdropdown.Append("<option selected value='" + dditem.BoxId + "'>" + dditem.BoxName + "</option>");
                    else
                        dtrdropdown.Append("<option  value='" + dditem.BoxId + "'>" + dditem.BoxName + "</option>");
                }
                dtrdropdownselect = new StringBuilder();
                dtrdropdownselect.Append("<select class='departmentsDropdown'  id='ddbox_" + item.PaymentTransactionId + @"' onchange='getValue(" + item.PaymentTransactionId + @",this.value)'>" + dtrdropdown + "</select>");
                ids[i] = item.PaymentTransactionId;
                courier[i] = item.CourierName;
                i++;
                decimal Currency = Convert.ToDecimal(item.TxnAmount);
                string Amount = Currency.ToString("0.00");
                DateTime dt = Convert.ToDateTime(item.ShipmentDate);
                string ShipmentDate = dt.ToString("dd/MMM/yyy");
                string status = item.OrderCurrentStatus.ToString();
                var status1 = Shared.GetOrderStatusEnum(status);
                string orderDate = item.CreatedOn.ToString("dd/MMM/yyy");
                string ProductName = "";
                string PaymentMode = item.PaymentMode;

                for (var j = 0; j < item.products.Count(); j++)
                {
                    if (j == 0)
                    {
                        ProductName = item.products.ElementAtOrDefault(j).ToString();
                    }
                    else
                    {
                        ProductName = ProductName + " ," + item.products.ElementAtOrDefault(j).ToString();
                    }
                }

                string bgrowcolor = "";
                if (item.ShipmentDate != null)
                {
                    if (item.ShipmentDate.Value.ToShortDateString() == DateTime.Now.ToShortDateString())
                    {
                        bgrowcolor = "#A9DFBF";
                    }
                    else if (item.ShipmentDate >= DateTime.Now.AddDays(-2))
                    {
                        bgrowcolor = "#F5CBA7";
                    }
                    else
                    {
                        bgrowcolor = "#E6B0AA";
                    }
                }

                var addressrepository = new AddressRepository();
                UserAddress address = addressrepository.UserAddress(item.UserId);

                strtd.Append(@"<tr style='background-color:" + bgrowcolor + @"'>
				<td>
												 <div id='chkBlkShip'>
<input  id='chkkShip' name='chkBlkShip' value=" + item.PaymentTransactionId + @"  type='checkbox' class='Check CheckPickUP' onchange='CheckboxChecked(this)'></div>
												<div style='visibility: hidden;'>
													<span id='lblTempOid'>" + item.PaymentTransactionId + @"</span>
												</div>
												
												<input  id='hdnShippingMode' class='hdnShippingMode' value='self' type='hidden'>
											</td><td>
												<a id='hypEdit' onclick='NavigatetoOrderDetailsPage(" + item.PaymentTransactionId + ")'>" + item.PaymentTransactionId + @"</a><br>
												<span id='lblPartialShip'></span>
											</td>
<td>
												<span id='Products'>" + orderDate + @"</span>   
											</td>
<td>
												<span id='Products'>" + ProductName + @"</span>   
											</td>
												<td>
												<span id='Products'>" + ShipmentDate + @"</span>   
											</td>
									   <td>
												
													<span id='lblTotalProducts'><span class='QuantityTipsySpan'>" + item.products.Count() + @"</span></span>
												
											</td><td align='right'>                                                
												<span id='lblTotalPrice'><span class='WebRupee'>" + item.CurrencySymbol + @" </span>" + Amount + @"</span>
											</td>
<td>
												<span id='Paymentmode'>" + PaymentMode + @"</span>   
											</td>
<td>
												<span class='iconsweet' style='font-family:'iconSweets;'></span>" + dtrdropdownselect + @"</td>
<td style='display:none'>
												<span id='OrderDate'>" + item.FirstName + @"</span>
												
											</td>
<td style='display:none'>
												<span id='OrderDate'>" + item.MobileNo + @"</span>
												
											</td>
<td style='display:none'>
												<span id='OrderDate'>" + item.EmailId + @"</span>
												
											</td>
<td style='display:none'>
<span id='userAddress'>" + address.StreetAddress1 + ", " + address.StreetAddress2 + ", " + address.LandMark + ", " + address.City + ", " + address.StateName + ", " + address.PinCode + @"</span>
</td>
			</tr>");
            }
            //strtd.Replace("Option", option);
            strtd.Append("<input id='ids' type='hidden' value='" + string.Join(",", ids) + "'>");
            if (Convert.ToInt32(HttpContext.Current.Session["RoleID"]) == 3)
            {
                strbutton.Append(@"<input style='display: inline-block;'  value='Assign Delhivery AWB' class='button_small greyishBtn fl_right' onclick='AssignDelhiveryAWBS();' type='button'>

<input style='display: inline-block;'  value='Advanced Pickup' class='button_small greyishBtn fl_right' onclick='Advancedpickup();' type='button'>

<input style='display: inline-block;'  value='Pickup Orders' class='button_small greyishBtn fl_right' onclick='PopupData();' type='button'>

<input type='button' value='DownloadDelhiveryExcel' class='button_small greyishBtn fl_right' onclick='DownloadDelhiveryExcel()'/>
<input type='button' value='DownloadBluedartExcel' class='button_small greyishBtn fl_right' onclick='DownloadBluedartExcel()'/>

<input type='button' value='DownloadShipRocketExcel' class='button_small greyishBtn fl_right' onclick='DownloadShipRocketExcel()'/>
<input type='button' value='DownloadPDF' class='button_small greyishBtn fl_right' onclick='DownloadPDF()'/>");
            }
            else
            {
                strbutton.Append(@"");
            }
            HttpContext.Current.Session["OrderID"] = string.Join(",", ids);

            StringBuilder strResult = new StringBuilder();
            strResult.Append(@"
		<div>                   
					
					<div>
							<div id='Main_dvGrid'>
								<div> 
								
								<div>
						
							
		<table class='activity_datatable' rules='all' id='ctl00_ctl00_Main_Main_grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
			<thead>
<tr class='checked'>
				<th class='cp_product_select' scope='col'>                                                
												<span title='Select/Unselect all orders'>
<div id='chkSelectAll' class='checker'><span>
<input style='opacity: 0;' id='ctl00_ctl00_Main_Main_grdShippingOrders_ctl01_chkSelectAll'  onclick='selectUnselectCheckboxes(this.id, 'chkBlkShip', 'lblTempOid');' type='checkbox'></span></div></span>
											</th><th scope='col'>ID</th>
<th scope='col'>Order Date</th>
			<th scope='col'>Product</th>
			<th scope='col'>Date</th>
			<th scope='col'>Qty</th>
			<th scope='col'><div id='divCurrency'>Amount</div></th>
<th scope='col'>Paymentmode</th>
<th scope='col'>Box Name</th>
<th scope='col' style='display:none'>Customer</th>
<th scope='col' style='display:none'>Mobile</th>
<th scope='col' style='display:none'>Email</th>
<th scope='col' style='display:none'>Address</th>
			</tr></thead><tbody>
" + strtd + @"
</tbody></table>	    </div></div>                        
						</div></div>
						</div></div>
						</div></div>
<div class='widget_body'>

			<div style='display: block;' class='action_bar text_right' id='dvbtn'>
" + strbutton + @"
			   
				<div class='clear'>
				</div>
			</div>
		</div>");

            //<option value='5' selected='selected'>5</option>
            //<option value='5'>10</option>
            //<option  value='10'>15</option>
            //<option value='15'>20</option>
            //<option value='20'>25</option>
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strResult.ToString()

            };
        }

        else
        {
            StringBuilder strResult = new StringBuilder();
            strResult.Append(@" <div id='ctl00_ctl00_Main_Main_divMessage' class='msgbar msg_Info hide_onC'>
								<span id='ctl00_ctl00_Main_Main_msgicon' class='iconsweet'>*</span>
								<p><span id='ctl00_ctl00_Main_Main_lblMessage'>There are no 'WaitingForPickUp Orders' found.</span></p>
							</div>");

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = strResult.ToString()
            };
        }
    }
    //[HttpGet]
    //public CustomResponse DownloadEcomExcel(string OrderIDs)
    //{
    //    StringBuilder tableData = new StringBuilder();
    //    StringBuilder strResult = new StringBuilder();
    //    System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
    //    string constr = ConfigurationManager.ConnectionStrings["db_Zon_constr"].ConnectionString;
    //    using (SqlConnection con = new SqlConnection(constr))
    //    {
    //        string[] ids = OrderIDs.Split(',');
    //        foreach (string id in ids)
    //        {
    //            using (SqlCommand cmd = new SqlCommand("SELECT distinct SUM(UPT.Quantity) over (partition by PT.PaymentTransactionId) as Quantity,ISNULL(STUFF((SELECT ', ' + ProductName FROM dbo.Products SSM INNER JOIN dbo.UserProductTransactions SUB ON SUB.ProductId = SSM.ProductID where SUB.ProductId = SSM.ProductID and SUB.PaymentTransactionId=" + id + " FOR XML PATH('')), 1, 1, ''), ' ') AS ProductName,PT.UserID,FirstName,PT.PaymentTransactionId,TxnAmount,PaymentMode,StreetAddress1,StreetAddress2,LandMark,City,PinCode,StateName,MobileNo FROM [db_Zon_Huw].[dbo].[PaymentTransactions] PT join dbo.UserProductTransactions UPT on PT.PaymentTransactionId=UPT.PaymentTransactionId join dbo.UserAddresses UA on UA.UserId=PT.UserID join dbo.Users U on PT.UserId = U.UserId join dbo.Products P on UPT.ProductId=P.ProductId where PT.PaymentTransactionId=" + id))
    //            {
    //                using (SqlDataAdapter sda = new SqlDataAdapter())
    //                {
    //                    cmd.Connection = con;
    //                    sda.SelectCommand = cmd;
    //                    using (DataTable dt = new DataTable())
    //                    {
    //                        sda.Fill(dt);
    //                        foreach (DataRow row in dt.Rows)
    //                        {
    //                            string PaymentType = "PPD";
    //                            string CollectableValue = "0";
    //                            if (row["PaymentMode"].ToString() == "Cash On Delivery")
    //                            {
    //                                PaymentType = "COD";
    //                                CollectableValue = row["TxnAmount"].ToString();
    //                            }
    //                            else
    //                            {
    //                                PaymentType = "PPD";
    //                                CollectableValue = "0";
    //                            }

    //                            tableData.Append(@"<tr>
    //                                        <td></td>
    //                                        <td>" + row["PaymentTransactionId"].ToString() + "</td>" +
    //                                            "<td>" + PaymentType + "</td>" +
    //                                            "<td>" + row["FirstName"].ToString() + "</td>" +
    //                                            "<td>" + row["StreetAddress1"].ToString() + "</td>" +
    //                                            "<td>" + row["StreetAddress2"].ToString() + "</td>" +
    //                                            "<td>" + row["LandMark"].ToString() + "</td>" +
    //                                            "<td>" + row["City"].ToString() + "</td>" +
    //                                            "<td>" + row["PinCode"].ToString() + "</td>" +
    //                                            "<td>" + row["StateName"].ToString() + "</td>" +
    //                                            "<td>" + row["MobileNo"].ToString() + "</td>" +
    //                                            "<td>" + row["MobileNo"].ToString() + "</td>" +
    //                                            "<td>" + row["ProductName"].ToString() + "</td>" +
    //                                            "<td>" + row["Quantity"].ToString() + "</td>" +
    //                                            "<td>" + CollectableValue + "</td>" +
    //                                            "<td>" + row["TxnAmount"].ToString() + "</td>" +
    //                                            "<td>150</td>" +
    //                                            "<td>1</td>" +
    //                                            "<td>10</td>" +
    //                                            "<td>10</td>" +
    //                                            "<td>10</td>" +
    //                                            "<td>Sonal Enterprises</td>" +
    //                                            "<td>No. SONAL ENTERPRISES,6-3-850/1 & 97, sirisha plaza, Landmark:Opposite Sheeshmahal theatre, Ammerpet</td>" +
    //                                            "<td>Dharam Karam road,Hyderabad,Telangana</td>" +
    //                                            "<td>500016</td>" +
    //                                            "<td>9440689551</td>" +
    //                                            "<td>9440689551</td>" +
    //                                            "<td>Sonal Enterprises</td>" +
    //                                            "<td>No. 6-3-850/1 & 97, sirisha plaza, Landmark:Opposite Sheeshmahal theatre, Ammerpet</td>" +
    //                                            "<td>Dharam Karam road,Hyderabad,Telangana</td>" +
    //                                            "<td>500016</td>" +
    //                                            "<td>9440689551</td>" +
    //                                            "<td>9440689551</td>" +
    //                                            "<td>No</td>" +
    //                                            "<td></td>" +
    //                                            "<td>" + row["PaymentTransactionId"].ToString() + "</td>" +
    //                                            "<td></td>" +
    //                                            "<td></td>" +
    //                                            "<td></td>" +
    //                                            "<td></td>" +
    //                                            "<td></td>" +
    //                                            "<td></td>" +
    //                                            "<td></td>" +
    //                                            "<td></td>" +
    //                                            "<td></td>" +
    //                                            "<td></td>" +
    //                                            "<td></td>" +
    //                                            "<td></td>" +
    //                                            "<td></td>" +
    //                                            "<td></td>" +
    //                                            "<td></td>" +
    //                                            "<td></td>" +
    //                                            "<td></td>" +
    //                                            "<td></td>" +
    //                                            "<td></td>" +
    //                                            "<td></td></tr>");
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    strResult.Append(@"
    //                <div>
    //                <div>
    //                <div>
    //                <div> 								
    //                <div>
    // 	        <table class='activity_datatable' id='tblEcomExcel' rules='all' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
    //       <thead>
    //                <tr class='checked'>
    //                <th scope='col'>AWB_NUMBER</th>
    //       <th scope='col'>ORDER_NUMBER</th>
    //       <th scope='col'>PRODUCT</th>
    //       <th scope='col'>CONSIGNEE</th>
    //       <th scope='col'>CONSIGNEE_ADDRESS1</th>
    //                <th scope='col'>CONSIGNEE_ADDRESS2</th>
    //                <th scope='col'>CONSIGNEE_ADDRESS3</th>
    //                <th scope='col'>DESTINATION_CITY</th>
    //                <th scope='col'>PINCODE</th>
    //                <th scope='col'>STATE</th>
    //       <th scope='col'>MOBILE</th>
    //                <th scope='col'>TELEPHONE</th>
    //                <th scope='col'>ITEM_DESCRIPTION</th>
    //                <th scope='col'>PIECES</th>
    //                <th scope='col'>COLLECTABLE_VALUE</th>
    //                <th scope='col'>DECLARED_VALUE</th>
    //       <th scope='col'>ACTUAL_WEIGHT</th>
    //                <th scope='col'>VOLUMETRIC_WEIGHT</th>
    //                <th scope='col'>LENGTH</th>
    //                <th scope='col'>BREADTH</th>
    //                <th scope='col'>HEIGHT</th>
    //                <th scope='col'>PICKUP_NAME</th>
    //       <th scope='col'>PICKUP_ADDRESS_LINE1</th>
    //                <th scope='col'>PICKUP_ADDRESS_LINE2</th>
    //                <th scope='col'>PICKUP_PINCODE</th>
    //                <th scope='col'>PICKUP_PHONE</th>
    //                <th scope='col'>PICKUP_MOBILE</th>
    //                <th scope='col'>RETURN_NAME</th>
    //       <th scope='col'>RETURN_ADDRESS_LINE1</th>
    //                <th scope='col'>RETURN_ADDRESS_LINE2</th>
    //                <th scope='col'>RETURN_PINCODE</th>
    //                <th scope='col'>RETURN_PHONE</th>
    //                <th scope='col'>RETURN_MOBILE</th>
    //                <th scope='col'>DG_SHIPMENT</th>
    //       <th scope='col'>SELLER_TIN</th>
    //                <th scope='col'>INVOICE_NUMBER</th>
    //                <th scope='col'>INVOICE_DATE</th>
    //                <th scope='col'>ESUGAM_NUMBER</th>
    //                <th scope='col'>ITEM_CATEGORY</th>
    //                <th scope='col'>PACKING_TYPE</th>
    //       <th scope='col'>PICKUP_TYPE</th>
    //                <th scope='col'>RETURN_TYPE</th>
    //                <th scope='col'>PICKUP_LOCATION_CODE</th>
    //                <th scope='col'>SELLER_GSTIN</th>
    //                <th scope='col'>GST_HSN</th>
    //                <th scope='col'>GST_ERN</th>
    //       <th scope='col'>GST_TAX_NAME</th>
    //                <th scope='col'>GST_TAX_BASE</th>
    //                <th scope='col'>DISCOUNT</th>
    //                <th scope='col'>GST_TAX_RATE_CGSTN</th>
    //                <th scope='col'>GST_TAX_RATE_SGSTN</th>
    //                <th scope='col'>GST_TAX_RATE_IGSTN</th>
    //       <th scope='col'>GST_TAX_TOTAL</th>
    //                <th scope='col'>GST_TAX_CGSTN</th>
    //                <th scope='col'>GST_TAX_SGSTN</th>
    //                <th scope='col'>GST_TAX_IGSTN</th>
    //       </tr>
    //                </thead><tbody>
    //                " + tableData + @"
    //                </tbody>
    //                </table>
    //                </div>
    //                </div>
    //                </div>
    //                </div>
    //                </div>
    //                </div>
    //                </div>
    //                </div>
    //                </div>
    //                </div>
    //                ");
    //    return new CustomResponse
    //    {
    //        Status = Shared.ResponseStatus.Success.ToString(),
    //        Result = strResult
    //    };
    //}


    [HttpGet]
    public CustomResponse EverydayPerformanceGraphMC()
    {
        var something = "";
        try
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                var repository = new ProductRepository();
                var thesuperarray = repository.EverydayPerformanceGraphBR();
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Result = ""
                };
            }
        }

        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Message = ex.Message,
                Result = null
            };
        }

    }
    public CustomResponse GetRoutineCustomers()
    {

        try
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                var repository = new ProductRepository();
                List<RoutineCustomers> datas = new List<RoutineCustomers>();
                datas = repository.GetRoutineCustomersData();
                List<RoutineCustomers> calculatedrecords = new List<RoutineCustomers>();
                foreach (var item1 in datas.ToList())
                {
                    foreach (var item2 in datas.ToList())
                    {
                        if (item1.userid == item2.userid && item1.ProductId == item2.ProductId && item1.CreatedOn != item2.CreatedOn)
                        {
                            int datediff = ((TimeSpan)(item1.CreatedOn - item2.CreatedOn)).Days;
                            var estimateddate = (item2.CreatedOn).AddDays(Math.Abs(datediff));
                            item2.CreatedOn = estimateddate.Date;
                            calculatedrecords.Add(item2);
                            datas.Remove(item1);
                            datas.Remove(item2);
                        }
                    }
                }


                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Result = calculatedrecords
                };
            }
        }

        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Message = ex.Message,
                Result = null
            };
        }

    }
    public CustomResponse Routinecustomerscheckboxes(int userid, int productid, Boolean checkstatus)
    {
        db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();

        var data = Entity.Routine_Customers_Table.Where(p => p.User_ID == userid && p.Product_ID == productid).ToList();



        if (data.Count != 0)
        {
            foreach (var item in data)
            {
                item.Status = checkstatus;
            }
        }
        else
        {
            var datax = new Routine_Customers_Table();
            datax.User_ID = userid;
            datax.Product_ID = productid;
            datax.Status = checkstatus;
            datax.Date = DateTime.Now;
            datax.Comments = "no comments";
            Entity.Routine_Customers_Table.Add(datax);
        }
        Entity.SaveChanges();
        data = null;
        var response = "success";
        return new CustomResponse
        {
            Result = response,
            Status = Shared.ResponseStatus.Success.ToString()
        };
    }




    [HttpGet]
    public CustomResponse DownloadEcomExcel(string OrderIDs)
    {
        StringBuilder tableData = new StringBuilder();
        StringBuilder strResult = new StringBuilder();
        System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
        string constr = ConfigurationManager.ConnectionStrings["db_Zon_constr"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            string[] ids = OrderIDs.Split(',');
            foreach (string id in ids)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var y = BalUtility.Logmaintainance((int)Shared.OrderStatus.Downloaded, "DownloadedEcomExcel", "", id);

                    using (SqlCommand cmd = new SqlCommand("exec SP_DownloadEcomExcel " + id))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                foreach (DataRow row in dt.Rows)
                                {
                                    decimal gst = Convert.ToDecimal(row["GST"]);
                                    decimal cgst = 0;
                                    decimal sgst = 0;
                                    decimal igst = 0;
                                    string Taxname = "";
                                    decimal totalAmount = Convert.ToDecimal(row["TxnAmount"]);
                                    decimal cgsttotal = 0;
                                    decimal sgsttotal = 0;
                                    decimal igsttotal = 0;

                                    decimal gsttotalamount = (totalAmount * (gst / 100));
                                    if (row["StateName"].ToString() != "Telangana")
                                    {
                                        Taxname = "igst";
                                        igst = gst;
                                        igsttotal = (totalAmount * (gst / 100));
                                    }
                                    else
                                    {
                                        Taxname = "CGST";
                                        cgst = gst / 2;
                                        sgst = gst / 2;
                                        cgsttotal = (totalAmount * (gst / 100)) / 2;
                                        sgsttotal = (totalAmount * (gst / 100)) / 2;
                                    }
                                    string AWB_Number = "0";
                                    string PaymentType = "PPD";
                                    string CollectableValue = "0";
                                    long orderId = Convert.ToInt64(id);
                                    db_Zon_HuwEntities context = new db_Zon_HuwEntities();
                                    PaymentTransaction updatecourier = context.PaymentTransactions.Where(x => x.PaymentTransactionId == orderId).FirstOrDefault();
                                    updatecourier.CourierName = "Ecom Express";
                                    updatecourier.IsPacking = true;
                                    context.SaveChanges();
                                    if (row["PaymentMode"].ToString() == "Cash On Delivery")
                                    {
                                        Tbl_AWB _AWB = context.Tbl_AWB.Where(x => x.IsActive == true && x.PaymentMode == 1 && x.OrderId == 0 && x.Deliverytype == null).FirstOrDefault();
                                        _AWB.OrderId = orderId;
                                        _AWB.IsActive = false;
                                        if (context.SaveChanges() != 0)
                                        {
                                            AWB_Number = context.Tbl_AWB.Where(x => x.OrderId == orderId && x.PaymentMode == 1 && x.IsActive == false && x.Deliverytype == null).OrderByDescending(x => x.CreatedOn).Select(x => x.AWB_Number).FirstOrDefault();
                                        }
                                        PaymentType = "COD";
                                        CollectableValue = row["TxnAmount"].ToString();
                                    }
                                    else
                                    {
                                        Tbl_AWB _AWB = context.Tbl_AWB.Where(x => x.IsActive == true && x.PaymentMode == 2 && x.OrderId == 0 && x.Deliverytype == null).FirstOrDefault();
                                        _AWB.OrderId = Convert.ToInt64(id);
                                        _AWB.IsActive = false;
                                        if (context.SaveChanges() != 0)
                                        {
                                            AWB_Number = context.Tbl_AWB.Where(x => x.OrderId == orderId && x.PaymentMode == 2 && x.IsActive == false && x.Deliverytype == null).OrderByDescending(x => x.CreatedOn).Select(x => x.AWB_Number).FirstOrDefault();
                                        }
                                        PaymentType = "PPD";
                                        CollectableValue = "0";
                                    }



                                    tableData.Append(@"<tr>
                                            <td>" + AWB_Number + "</td>" +
                                            "<td>" + row["PaymentTransactionId"].ToString() + "</td>" +
                                                    "<td>" + PaymentType + "</td>" +
                                                    "<td>" + row["FirstName"].ToString() + "</td>" +
                                                    "<td>" + row["StreetAddress1"].ToString() + "</td>" +
                                                    "<td>" + row["StreetAddress2"].ToString() + "</td>" +
                                                    "<td>" + row["LandMark"].ToString() + "</td>" +
                                                    "<td>" + row["City"].ToString() + "</td>" +
                                                    "<td>" + row["PinCode"].ToString() + "</td>" +
                                                    "<td>" + row["StateName"].ToString() + "</td>" +
                                                    "<td>" + row["MobileNo"].ToString() + "</td>" +
                                                    "<td>" + row["MobileNo"].ToString() + "</td>" +
                                                    "<td>" + row["ProductName"].ToString() + "</td>" +
                                                    "<td>" + row["Quantity"].ToString() + "</td>" +
                                                    "<td>" + CollectableValue + "</td>" +
                                                    "<td>" + row["TxnAmount"].ToString() + "</td>" +
                                                    "<td>150</td>" +
                                                    "<td>1</td>" +
                                                    "<td>10</td>" +
                                                    "<td>10</td>" +
                                                    "<td>10</td>" +
                                                    "<td>Sonal Enterprises</td>" +
                                                    "<td>No. 6-3-850/1 & 97, sirisha plaza, Landmark:Opposite Sheeshmahal theatre, Ammerpet</td>" +
                                                    "<td>Dharam Karam road,Hyderabad,Telangana</td>" +
                                                    "<td>500016</td>" +
                                                    "<td>9440689551</td>" +
                                                    "<td>9440689551</td>" +
                                                    "<td>Sonal Enterprises</td>" +
                                                    "<td>No. 6-3-850/1 & 97, sirisha plaza, Landmark:Opposite Sheeshmahal theatre, Ammerpet</td>" +
                                                    "<td>Dharam Karam road,Hyderabad,Telangana</td>" +
                                                    "<td>500016</td>" +
                                                    "<td>9440689551</td>" +
                                                    "<td>9440689551</td>" +
                                                    "<td>No</td>" +
                                                    "<td></td>" +
                                                    "<td>" + row["PaymentTransactionId"].ToString() + "</td>" +
                                                    "<td>" + row["CreatedOn"].ToString() + "</td>" +
                                                    "<td></td>" +
                                                    "<td></td>" +
                                                    "<td></td>" +
                                                    "<td></td>" +
                                                    "<td></td>" +
                                                    "<td></td>" +
                                                    "<td>36AAUPK4988K1ZO</td>" +
                                                    "<td>1234</td>" +
                                                    "<td></td>" +
                                                    "<td>" + Taxname + "</td>" +
                                                    "<td>" + gst + "</td>" +
                                                    "<td></td>" +
                                                    "<td>" + cgst + "</td>" +
                                                    "<td>" + sgst + "</td>" +
                                                    "<td>" + igst + "</td>" +
                                                    "<td>" + gsttotalamount + "</td>" +
                                                    "<td>" + cgsttotal + "</td>" +
                                                    "<td>" + sgsttotal + "</td>" +
                                                    "<td>" + igsttotal + "</td>" +
                                                    "<td></td>" +
                                                    "<td></td>" +
                                                    "<td>871439</td></tr>");
                                }
                            }
                        }
                    }
                }
            }
        }

        strResult.Append(@"
                    <div>
                    <div>
                    <div>
                    <div> 								
                    <div>
	    	        <table class='activity_datatable' id='tblEcomExcel' rules='all' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
			        <thead>
                    <tr class='checked'>
                    <th scope='col'>AWB_NUMBER</th>
			        <th scope='col'>ORDER_NUMBER</th>
			        <th scope='col'>PRODUCT</th>
			        <th scope='col'>CONSIGNEE</th>
			        <th scope='col'>CONSIGNEE_ADDRESS1</th>
                    <th scope='col'>CONSIGNEE_ADDRESS2</th>
                    <th scope='col'>CONSIGNEE_ADDRESS3</th>
                    <th scope='col'>DESTINATION_CITY</th>
                    <th scope='col'>PINCODE</th>
                    <th scope='col'>STATE</th>
			        <th scope='col'>MOBILE</th>
                    <th scope='col'>TELEPHONE</th>
                    <th scope='col'>ITEM_DESCRIPTION</th>
                    <th scope='col'>PIECES</th>
                    <th scope='col'>COLLECTABLE_VALUE</th>
                    <th scope='col'>DECLARED_VALUE</th>
			        <th scope='col'>ACTUAL_WEIGHT</th>
                    <th scope='col'>VOLUMETRIC_WEIGHT</th>
                    <th scope='col'>LENGTH</th>
                    <th scope='col'>BREADTH</th>
                    <th scope='col'>HEIGHT</th>
                    <th scope='col'>PICKUP_NAME</th>
			        <th scope='col'>PICKUP_ADDRESS_LINE1</th>
                    <th scope='col'>PICKUP_ADDRESS_LINE2</th>
                    <th scope='col'>PICKUP_PINCODE</th>
                    <th scope='col'>PICKUP_PHONE</th>
                    <th scope='col'>PICKUP_MOBILE</th>
                    <th scope='col'>RETURN_NAME</th>
			        <th scope='col'>RETURN_ADDRESS_LINE1</th>
                    <th scope='col'>RETURN_ADDRESS_LINE2</th>
                    <th scope='col'>RETURN_PINCODE</th>
                    <th scope='col'>RETURN_PHONE</th>
                    <th scope='col'>RETURN_MOBILE</th>
                    <th scope='col'>DG_SHIPMENT</th>
			        <th scope='col'>SELLER_TIN</th>
                    <th scope='col'>INVOICE_NUMBER</th>
                    <th scope='col'>INVOICE_DATE</th>
                    <th scope='col'>ESUGAM_NUMBER</th>
                    <th scope='col'>ITEM_CATEGORY</th>
                    <th scope='col'>PACKING_TYPE</th>
			        <th scope='col'>PICKUP_TYPE</th>
                    <th scope='col'>RETURN_TYPE</th>
                    <th scope='col'>PICKUP_LOCATION_CODE</th>
                    <th scope='col'>SELLER_GSTIN</th>
                    <th scope='col'>GST_HSN</th>
                    <th scope='col'>GST_ERN</th>
			        <th scope='col'>GST_TAX_NAME</th>
                    <th scope='col'>GST_TAX_BASE</th>
                    <th scope='col'>DISCOUNT</th>
                    <th scope='col'>GST_TAX_RATE_CGSTN</th>
                    <th scope='col'>GST_TAX_RATE_SGSTN</th>
                    <th scope='col'>GST_TAX_RATE_IGSTN</th>
			        <th scope='col'>GST_TAX_TOTAL</th>
                    <th scope='col'>GST_TAX_CGSTN</th>
                    <th scope='col'>GST_TAX_SGSTN</th>
                    <th scope='col'>GST_TAX_IGSTN</th>
                    <th scope='col'>CPIN</th>
                    <th scope='col'>essentialProduct</th>
                    <th scope='col'>MpKey</th>

			        </tr>
                    </thead><tbody>
                    " + tableData + @"
                    </tbody>
                    </table>
                    </div>
                    </div>
                    </div>
                    </div>
                    </div>
                    </div>
                    </div>
                    </div>
                    </div>
                    </div>
                    ");
        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Result = strResult
        };
    }
    [HttpPost]
    public CustomResponse Movettoauthorized(string OrderIDs)
    {
        string[] ids = OrderIDs.Split(',');
        foreach (string id in ids)
        {
            long idss = Convert.ToInt64(id);
            var repository = new PaymentTransactionRepository();
            var updatedata = repository.First(p => p.PaymentTransactionId == idss);
            //var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out  totalRecords, 0, rows, (p => p.Dispatched == false && p.Delivered == false && p.Pickup == false && p.Authorized == false), null, "", true);
            updatedata.ShipmentType = null;
            updatedata.Location = null;
            updatedata.CourierName = null;
            updatedata.OrderCurrentStatus = (int)Shared.OrderStatus.Readytoship;
            updatedata.ShipmentDate = null;
            updatedata.ShipmentId = null;
            updatedata.ShipmentURL = null;
            updatedata.Pickup = false;
            updatedata.IsPacking = null;

            repository.Update(updatedata);

        }
        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
        };
    }

    [HttpGet]
    public CustomResponse DownloadShipRocketExcel(string OrderIDs)
    {
        StringBuilder tableData = new StringBuilder();
        StringBuilder strResult = new StringBuilder();
        System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
        string constr = ConfigurationManager.ConnectionStrings["db_Zon_constr"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            string[] ids = OrderIDs.Split(',');
            foreach (string id in ids)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var y = BalUtility.Logmaintainance((int)Shared.OrderStatus.Downloaded, "DownloadShipRocketExcel", "", id);

                    using (SqlCommand cmd = new SqlCommand("SELECT distinct(p.PaymentTransactionId),'' as Waybill, p.PaymentTransactionId as 'Reference No',u.FirstName as Firstname, u.LastName as Lastname ,ua.City, ua.StateName as State, 'India' as Country, ua.StreetAddress1 + ',' + ua.StreetAddress2 as MainAddress,ua.City+',' + ua.LandMark as Address , ua.PinCode as Pincode,u.EmailId, u.MobileNo as Phone, u.MobileNo as Mobile, '100' as Weight,case when p.PaymentMode='Cash On Delivery' then 'COD' when p.PaymentMode!='Cash On Delivery' then 'Prepaid' end as 'Payment Mode' , p.TxnAmount-p.ShippingCharges as 'Package Amount',p.ShippingCharges,case when p.PaymentMode='Cash On Delivery' then p.TxnAmount when p.PaymentMode!='Cash On Delivery' then '' end as 'Cod Amount','Surface' as 'Shipping Mode','HYDERABAD' as 'Return Address' , '500016' as 'Return Pin','SONAL ENTERPRISES' as 'Seller Name','6-3-850/1,SHIRISHA PLAZA,DHARAM KARAM ROAD,AMEERPET,HYDERABAD' as 'Seller Address', '' as 'Seller CST No','' as 'Seller TIN',p.PaymentTransactionId as 'Invoice No' ,GETDATE() as 'Invoice Date' ,'' as 'Commodity Value','' as 'Tax Value','' as 'Category of Goods' ,'' as 'Seller_GST_TIN','' as HSN_Code ,'' as 'Return Reason' ,'SONLENTERPRISES FRANCHISE' as 'Vendor Pickup Location' ,'' as EWBN,'' as address_specific ,'' as person_specific FROM [dbo].[PaymentTransactions] as p join [dbo].Users as u on p.UserId = u.UserId join dbo.[UserProductTransactions] ut on ut.PaymentTransactionId=p.PaymentTransactionId join [dbo].UserAddresses as ua on ua.UserAddressId = ut.BillingAddressId where p.PaymentTransactionId=" + id))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                foreach (DataRow row in dt.Rows)
                                {
                                    db_Zon_HuwEntities context = new db_Zon_HuwEntities();
                                    long paymentid = Convert.ToInt64(row["Reference No"]);

                                    PaymentTransaction updatecourier = context.PaymentTransactions.Where(x => x.PaymentTransactionId == paymentid).FirstOrDefault();
                                    updatecourier.CourierName = "ShipRocket";
                                    updatecourier.IsPacking = true;
                                    context.SaveChanges();
                                    Tbl_Packing_Box boxs = context.Tbl_Packing_Box.Where(x => x.BoxId == updatecourier.BoxId).FirstOrDefault();
                                    if (boxs == null)
                                    {
                                        boxs = new Tbl_Packing_Box();
                                        boxs.Lengths = "0";
                                        boxs.Width = "0";
                                        boxs.Height = "0";
                                    }
                                    List<UserProductTransaction> upt = context.UserProductTransactions.Where(x => x.PaymentTransactionId == paymentid).ToList();
                                    int quantity = upt.Sum(x => x.Quantity);
                                    List<string> productnames = new List<string>();
                                    List<string> productids = new List<string>();
                                    int weight = 0;
                                    foreach (var item in upt)
                                    {
                                        string productname = context.Products.Where(x => x.ProductId == item.ProductId).FirstOrDefault().ProductName;
                                        long productid = context.Products.Where(x => x.ProductId == item.ProductId).FirstOrDefault().ProductId;
                                        productids.Add(productid.ToString());
                                        productnames.Add(productname);
                                    }
                                    //<td>" + row["Waybill"].ToString() + "</td>" +
                                    tableData.Append(@"<tr>
                                                    <td>" + row["Reference No"].ToString() + "</td>" +
                                                    "<td>" + DateTime.Now.ToString("dd-MM-yyyy hh:mm") + "</td>" +
                                                    "<td>Custom</ td>" +
                                                    "<td>" + row["Payment Mode"].ToString() + "</td>" +
                                                    "<td>" + row["Firstname"].ToString() + "</td>" +
                                                    "<td>" + row["Lastname"].ToString() + "</td>" +
                                                    "<td>" + row["EmailId"].ToString() + "</td>" +
                                                    "<td>" + row["Phone"].ToString() + "</td>" +
                                                    "<td>" + row["Mobile"].ToString() + "</td>" +
                                                    "<td>" + row["MainAddress"].ToString() + "</td>" +
                                                    "<td>" + row["Address"].ToString() + "</td>" +
                                                    "<td>India</td>" +
                                                    "<td>" + row["State"].ToString() + "</td>" +
                                                    "<td>" + row["City"].ToString() + "</td>" +
                                                    "<td>" + row["Pincode"].ToString() + "</td>" +
                                                    "<td>6-3-850/1,SHIRISHA PLAZA,DHARAM KARAM ROAD,AMEERPET,HYDERABAD</td>" +
                                                    "<td>6-3-850/1,SHIRISHA PLAZA,DHARAM KARAM ROAD,AMEERPET,HYDERABAD</td>" +
                                                    "<td>India</td>" +
                                                    "<td>Telagana</td>" +
                                                    "<td>Hyderabad</td>" +
                                                    "<td>500016</td>" +
                                                    "<td>" + string.Join(",", productids) + "</td>" +
                                                    "<td>" + string.Join(",", productnames) + "</td>" +
                                                    "<td>" + quantity + "</td>" +
                                                    "<td></td>" +
                                                    "<td>" + (Convert.ToDecimal(row["Package Amount"].ToString())) / quantity + "</td>" +
                                                    "<td></td>" +
                                                    "<td>" + row["ShippingCharges"].ToString() + "</td>" +
                                                    "<td></td>" +
                                                    "<td></td>" +

                                                    "<td></td>" +
                                                    "<td>" + boxs.Lengths + "</td>" +
                                                    "<td>" + boxs.Width + "</td>" +
                                                    "<td>" + boxs.Height + "</td>" +
                                                    "<td>Please Enter</td>" +
                                                    "<td>TRUE</td>" +
                                                    "<td></td>" +
                                                    "<td></td>" +
                                                    "<td></td>" +
                                                    "<td></td>" +
                                                    "<td>SONLENTERPRISES FRANCHISE</ td>" +
                                                    "<td></td>" +
                                                    "<td></td>" +
                                                    "<td></td>" +
                                                    "<td></td>" +
                                                    "<td></td>" +
                                                    "<td></td></tr>");
                                }
                            }
                        }
                    }
                }
            }
        }

        strResult.Append(@"
                    <div>
                    <div>
                    <div>
                    <div> 								
                    <div>
	    	        <table class='activity_datatable' id='tblEcomExcel' rules='all' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
			        <thead>
                    <tr class='checked'>
                    <th scope='col'>*Order Id</th>
			        <th scope='col'>*Order Date as dd-mm-yyyy hh:MM</th>
			        <th scope='col'>*Channel</th>
			        <th scope='col'>*Payment Method(COD/Prepaid)</th>

			        <th scope='col'>*Customer First Name</th>
                    <th scope='col'>*Customer Last Name</th>
                    <th scope='col'>*Email</th>
                    <th scope='col'>*Customer Mobile</th>
                    <th scope='col'>Customer Alternate Mobile</th>
  <th scope='col'>*Shipping Address Line 1</th>
                    <th scope='col'>*Shipping Address Line 2</th>
                    <th scope='col'>*Shipping Address Country</th>
                    <th scope='col'>*Shipping Address State</th>
                    <th scope='col'>*Shipping Address City</th>
			        <th scope='col'>*Shipping Address Postcode</th>
                    <th scope='col'>Billing Address Line 1</th>
                    <th scope='col'>Billing Address Line 2</th>
<th scope='col'>Billing Address Country</th>
<th scope='col'>Billing Address State</th>
<th scope='col'>Billing Address City</th>
<th scope='col'>Billing Address Postcode</th>
<th scope='col'>*Master SKU</th>
<th scope='col'>*Product Name</th>
<th scope='col'>*Product Quantity</th>
<th scope='col'>Tax %</th>
<th scope='col'>*Selling Price(Per Unit Item, Inclusive of Tax)</th>
<th scope='col'>Discount(Per Unit Item)</th>
<th scope='col'>Shipping Charges(Per Order)</th>
<th scope='col'>COD Charges(Per Order)</th>
<th scope='col'>Gift Wrap Charges(Per Order)</th>
<th scope='col'>Total Discount (Per Order)</th>
<th scope='col'>*Length (cm)</th>
<th scope='col'>*Breadth (cm)</th>
<th scope='col'>*Height (cm)</th>
<th scope='col'>Weight Of Shipment(kg)</th>
<th scope='col'>Send Notification(True/False)</th>
<th scope='col'>Comment</th>
<th scope='col'>HSN Code</th>
<th scope='col'>Location Id</th>
<th scope='col'>Reseller Name</th>
<th scope='col'>Company Name</th>
<th scope='col'>latitude</th>
<th scope='col'>longitude</th>
<th scope='col'>Verified Order</th>
<th scope='col'>Is documents</th>
<th scope='col'>Order Type</th>
<th scope='col'>Order tag</th>
			        </tr>
                    </thead><tbody>
                    " + tableData + @"
                    </tbody>
                    </table>
                    </div>
                    </div>
                    </div>
                    </div>
                    </div>
                    </div>
                    </div>
                    </div>
                    </div>
                    </div>
                    ");
        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Result = strResult
        };
    }
    [HttpGet]
    public CustomResponse DownloadDelhiveryExcel(string OrderIDs)
    {
        StringBuilder tableData = new StringBuilder();
        StringBuilder strResult = new StringBuilder();
        System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
        string constr = ConfigurationManager.ConnectionStrings["db_Zon_constr"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            string[] ids = OrderIDs.Split(',');
            foreach (string id in ids)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var y = BalUtility.Logmaintainance((int)Shared.OrderStatus.Downloaded, "DownloadedDelhiveryExcel", "", id);

                    using (SqlCommand cmd = new SqlCommand("SELECT distinct(p.PaymentTransactionId),'' as Waybill, p.PaymentTransactionId as 'Reference No',u.FirstName+' ' + u.LastName as 'Consignee Name' ,ua.City, ua.StateName as State, 'India' as Country, ua.StreetAddress1 + ',' + ua.StreetAddress2 + ','+ua.City+',' + ua.LandMark as Address , ua.PinCode as Pincode, u.MobileNo as Phone, u.MobileNo as Mobile, '100' as Weight,case when p.PaymentMode='Cash On Delivery' then 'cod' when p.PaymentMode!='Cash On Delivery' then 'prepaid' end as 'Payment Mode' , p.TxnAmount as 'Package Amount',case when p.PaymentMode='Cash On Delivery' then p.TxnAmount when p.PaymentMode!='Cash On Delivery' then '' end as 'Cod Amount','Surface' as 'Shipping Mode','HYDERABAD' as 'Return Address' , '500016' as 'Return Pin','SONAL ENTERPRISES' as 'Seller Name','6-3-850/1,SHIRISHA PLAZA,DHARAM KARAM ROAD,AMEERPET,HYDERABAD' as 'Seller Address', '' as 'Seller CST No','' as 'Seller TIN',p.PaymentTransactionId as 'Invoice No' ,GETDATE() as 'Invoice Date' ,'' as 'Commodity Value','' as 'Tax Value','' as 'Category of Goods' ,'' as 'Seller_GST_TIN','' as HSN_Code ,'' as 'Return Reason' ,'SONLENTERPRISES FRANCHISE' as 'Vendor Pickup Location' ,'' as EWBN,'' as address_specific ,'' as person_specific FROM [dbo].[PaymentTransactions] as p join [dbo].Users as u on p.UserId = u.UserId join dbo.[UserProductTransactions] ut on ut.PaymentTransactionId=p.PaymentTransactionId join [dbo].UserAddresses as ua on ua.UserAddressId = ut.BillingAddressId where p.PaymentTransactionId=" + id))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                foreach (DataRow row in dt.Rows)
                                {
                                    db_Zon_HuwEntities context = new db_Zon_HuwEntities();
                                    long paymentid = Convert.ToInt64(row["Reference No"]);

                                    PaymentTransaction updatecourier = context.PaymentTransactions.Where(x => x.PaymentTransactionId == paymentid).FirstOrDefault();
                                    updatecourier.CourierName = "Delhivery";
                                    updatecourier.IsPacking = true;
                                    context.SaveChanges();
                                    List<UserProductTransaction> upt = context.UserProductTransactions.Where(x => x.PaymentTransactionId == paymentid).ToList();
                                    int quantity = upt.Sum(x => x.Quantity);
                                    List<string> productnames = new List<string>();

                                    foreach (var item in upt)
                                    {
                                        string productname = context.Products.Where(x => x.ProductId == item.ProductId).FirstOrDefault().ProductName;
                                        productnames.Add(productname);
                                    }
                                    //<td>" + row["Waybill"].ToString() + "</td>" +
                                    HttpClient client = new HttpClient();
                                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                                    HttpResponseMessage response = client.GetAsync("https://track.delhivery.com/waybill/api/fetch/json/?cl=client_name&token=d6ce66f9ff7dd9870364a226cea294052f7bf2f0").Result;
                                    string resData = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
                                    tableData.Append(@"<tr>
                                                    <td>" + resData.ToString() + "</td>" +

                                                    "<td>" + row["Reference No"].ToString() + "</td>" +
                                                    "<td>" + row["Consignee Name"].ToString() + "</td>" +
                                                    "<td>" + row["City"].ToString() + "</td>" +

                                                    "<td>" + row["State"].ToString() + "</td>" +
                                                    "<td>" + row["Country"].ToString() + "</td>" +
                                                    "<td>" + row["Address"].ToString() + "</td>" +
                                                    "<td>" + row["Pincode"].ToString() + "</td>" +
                                                    "<td>" + row["Phone"].ToString() + "</td>" +
                                                    "<td>" + row["Mobile"].ToString() + "</td>" +
                                                    "<td>1</td>" +
                                                    "<td>1</td>" +
                                                    "<td>1</td>" +

                                                    "<td>" + row["Weight"].ToString() + "</td>" +
                                                    "<td>" + row["Payment Mode"].ToString() + "</td>" +
                                                    "<td>" + row["Package Amount"].ToString() + "</td>" +
                                                    "<td>" + row["Cod Amount"].ToString() + "</td>" +
                                                     "<td>" + string.Join(",", productnames) + "</td>" +
                                                    "<td>" + row["Shipping Mode"].ToString() + "</td>" +
                                                    "<td>" + row["Return Address"].ToString() + "</td>" +

                                                    "<td>" + row["Return Pin"].ToString() + "</td>" +
                                                    "<td>true</td>" +

                                                    "<td>" + row["Seller Name"].ToString() + "</td>" +
                                                    "<td>" + row["Seller Address"].ToString() + "</td>" +
                                                    "<td>" + row["Seller CST No"].ToString() + "</td>" +
                                                    "<td>" + row["Seller TIN"].ToString() + "</td>" +
                                                    "<td>" + row["Invoice No"].ToString() + "</td>" +
                                                    "<td>" + row["Invoice Date"].ToString() + "</td>" +
                                                    "<td>" + quantity + "</td>" +
                                                    "<td>" + row["Commodity Value"].ToString() + "</td>" +
                                                    "<td>" + row["Tax Value"].ToString() + "</td>" +

                                                    "<td>" + row["Category of Goods"].ToString() + "</td>" +
                                                    "<td>" + row["Seller_GST_TIN"].ToString() + "</td>" +
                                                    "<td>" + row["HSN_Code"].ToString() + "</td>" +
                                                    "<td>" + row["Return Reason"].ToString() + "</td>" +
                                                    "<td>" + row["Vendor Pickup Location"].ToString() + "</td>" +
                                                    "<td>" + row["EWBN"].ToString() + "</td>" +
                                                    "<td>" + row["address_specific"].ToString() + "</td>" +
                                                    "<td>" + row["person_specific"].ToString() + "</td></tr>");
                                }
                            }
                        }
                    }
                }
            }
        }

        strResult.Append(@"
                    <div>
                    <div>
                    <div>
                    <div> 								
                    <div>
	    	        <table class='activity_datatable' id='tblEcomExcel' rules='all' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
			        <thead>
                    <tr class='checked'>
                    <th scope='col'>Waybill</th>
			        <th scope='col'>Reference No</th>
			        <th scope='col'>Consignee Name</th>
			        <th scope='col'>City</th>

			        <th scope='col'>State</th>
                    <th scope='col'>Country</th>
                    <th scope='col'>Address</th>
                    <th scope='col'>Pincode</th>
                    <th scope='col'>Phone</th>
  <th scope='col'>Mobile</th>
                    <th scope='col'>Shipment Length</th>
                    <th scope='col'>Shipment Breadth</th>
                    <th scope='col'>Shipment Height</th>
                    <th scope='col'>Weight</th>
			        <th scope='col'>Payment Mode</th>
                    <th scope='col'>Package Amount</th>
                    <th scope='col'>Cod Amount</th>
<th scope='col'>Product to be Shipped</th>
<th scope='col'>Shipping Mode</th>
<th scope='col'>Return Address</th>
<th scope='col'>Return Pin</th>
<th scope='col'>fragile_shipment</th>

<th scope='col'>Seller Name</th>
<th scope='col'>Seller Address</th>
<th scope='col'>Seller CST No</th>
<th scope='col'>Seller TIN</th>
<th scope='col'>Invoice No</th>
<th scope='col'>Invoice Date</th>
<th scope='col'>Quantity</th>
<th scope='col'>Commodity Value</th>
<th scope='col'>Tax Value</th>

<th scope='col'>Category of Goods</th>
<th scope='col'>Seller_GST_TIN</th>

<th scope='col'>HSN_Code</th>
<th scope='col'>Return Reason</th>
<th scope='col'>Vendor Pickup Location</th>
<th scope='col'>EWBN</th>
<th scope='col'>address_specific</th>
<th scope='col'>person_specific</th>
			        </tr>
                    </thead><tbody>
                    " + tableData + @"
                    </tbody>
                    </table>
                    </div>
                    </div>
                    </div>
                    </div>
                    </div>
                    </div>
                    </div>
                    </div>
                    </div>
                    </div>
                    ");
        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Result = strResult
        };
    }
    [HttpGet]
    public CustomResponse DownloadBluedartExcel(string OrderIDs)
    {
        try
        {
            string url = "https://netconnect.bluedart.com/Ver1.11/ShippingAPI/WayBill/WayBillGeneration.svc/rest/ImportData";
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;
            BluedartRequest bluereq = new BluedartRequest();
            db_Zon_HuwEntities context = new db_Zon_HuwEntities();
            List<Utility.Request> requests = new List<Utility.Request>();

            string constr = ConfigurationManager.ConnectionStrings["db_Zon_constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string[] ids = OrderIDs.Split(',');
                foreach (string id in ids)
                {
                    if (!string.IsNullOrEmpty(id))
                    {

                        using (SqlCommand cmd = new SqlCommand("SELECT distinct(p.PaymentTransactionId),'' as Waybill, p.PaymentTransactionId as 'Reference No',u.FirstName+' ' + u.LastName as 'Consignee Name' ,ua.City, ua.StateName as State, 'India' as Country, ua.StreetAddress1 + ',' + ua.StreetAddress2 + ','+ua.City+',' + ua.LandMark as Address , ua.PinCode as Pincode, u.MobileNo as Phone, u.MobileNo as Mobile, '100' as Weight,case when p.PaymentMode='Cash On Delivery' then 'cod' when p.PaymentMode!='Cash On Delivery' then 'prepaid' end as 'Payment Mode' , p.TxnAmount as 'Package Amount',case when p.PaymentMode='Cash On Delivery' then p.TxnAmount when p.PaymentMode!='Cash On Delivery' then '' end as 'Cod Amount','Surface' as 'Shipping Mode','HYDERABAD' as 'Return Address' , '500016' as 'Return Pin','SONAL ENTERPRISES' as 'Seller Name','6-3-850/1,SHIRISHA PLAZA,DHARAM KARAM ROAD,AMEERPET,HYDERABAD' as 'Seller Address', '' as 'Seller CST No','' as 'Seller TIN',p.PaymentTransactionId as 'Invoice No' ,GETDATE() as 'Invoice Date' ,'' as 'Commodity Value','' as 'Tax Value','' as 'Category of Goods' ,'' as 'Seller_GST_TIN','' as HSN_Code ,'' as 'Return Reason' ,'SONLENTERPRISES FRANCHISE' as 'Vendor Pickup Location' ,'' as EWBN,'' as address_specific ,'' as person_specific FROM [dbo].[PaymentTransactions] as p join [dbo].Users as u on p.UserId = u.UserId join dbo.[UserProductTransactions] ut on ut.PaymentTransactionId=p.PaymentTransactionId join [dbo].UserAddresses as ua on ua.UserAddressId = ut.BillingAddressId where p.PaymentTransactionId=" + id))
                        {
                            using (SqlDataAdapter sda = new SqlDataAdapter())
                            {
                                cmd.Connection = con;
                                sda.SelectCommand = cmd;
                                using (DataTable dt = new DataTable())
                                {
                                    sda.Fill(dt);

                                    foreach (DataRow row in dt.Rows)
                                    {
                                        Utility.Request singlereq = new Utility.Request();
                                        long paymentid = Convert.ToInt64(row["Reference No"]);
                                        PaymentTransaction updatecourier = context.PaymentTransactions.Where(x => x.PaymentTransactionId == paymentid).FirstOrDefault();

                                        Tbl_Packing_Box boxs = context.Tbl_Packing_Box.Where(x => x.BoxId == updatecourier.BoxId).FirstOrDefault();
                                        if (boxs == null)
                                        {
                                            boxs = new Tbl_Packing_Box();
                                            boxs.Lengths = "0";
                                            boxs.Width = "0";
                                            boxs.Height = "0";
                                        }
                                        List<UserProductTransaction> upt = context.UserProductTransactions.Where(x => x.PaymentTransactionId == paymentid).ToList();
                                        List<Itemdtl> allproducts = new List<Itemdtl>();

                                        foreach (var item in upt)
                                        {
                                            Itemdtl itm = new Itemdtl();
                                            Product product = context.Products.Where(x => x.ProductId == item.ProductId).FirstOrDefault();
                                            itm.ItemID = product.ProductId.ToString();
                                            itm.ItemName = product.ProductName;
                                            itm.ItemValue = Convert.ToDouble(item.ProductCost);
                                            itm.Itemquantity = item.Quantity;
                                            itm.TotalValue = Convert.ToDouble(item.ProductCost) * item.Quantity;
                                            itm.cessAmount = 0.0;
                                            allproducts.Add(itm);

                                        }
                                        singlereq.Consignee = new Consignee();
                                        //consignee
                                        singlereq.Consignee.ConsigneeAddress1 = row["Address"].ToString();
                                        singlereq.Consignee.ConsigneeEmailID = "healthurwealth@gmail.com";
                                        singlereq.Consignee.ConsigneeMobile = row["Phone"].ToString();
                                        singlereq.Consignee.ConsigneeName = row["Consignee Name"].ToString();
                                        singlereq.Consignee.ConsigneePincode = row["Pincode"].ToString();
                                        //returnaddr
                                        singlereq.Returnadds = new Returnadds();

                                        singlereq.Returnadds.ReturnAddress1 = row["Seller Address"].ToString();
                                        singlereq.Returnadds.ReturnAddress2 = row["Return Address"].ToString();
                                        singlereq.Returnadds.ReturnContact = "SONAL ENTERPRISES";
                                        singlereq.Returnadds.ReturnEmailID = "orders@healthurwealth.com";
                                        singlereq.Returnadds.ReturnMobile = "9440689551";
                                        singlereq.Returnadds.ReturnPincode = row["Return Pin"].ToString();
                                        //Services
                                        singlereq.Services = new Services();

                                        singlereq.Services.ActualWeight = "0.3";
                                        singlereq.Services.Commodity = new Commodity();
                                        singlereq.Services.Commodity.CommodityDetail1 = "Medicine";
                                        singlereq.Services.PrinterLableSize = "3";
                                        singlereq.Services.RegisterPickup = true;
                                        if (updatecourier.PaymentMode.ToUpper().Contains("CASH"))
                                        {
                                            singlereq.Services.CollectableAmount = Convert.ToDouble(row["Cod Amount"].ToString());
                                        }
                                        else
                                        {
                                            singlereq.Services.CollectableAmount = 0.0;
                                        }
                                        singlereq.Services.CreditReferenceNo = row["Reference No"].ToString();
                                        singlereq.Services.DeclaredValue = Convert.ToDouble(row["Package Amount"].ToString());
                                        //singlereq.Services.Dimensions.
                                        Dimension dmns = new Dimension();
                                        dmns.Breadth = Convert.ToDouble(boxs.Width);
                                        dmns.Length = Convert.ToDouble(boxs.Lengths);
                                        dmns.Height = Convert.ToDouble(boxs.Height);
                                        dmns.Count = 1;
                                        singlereq.Services.Dimensions = new List<Dimension>();
                                        singlereq.Services.Dimensions.Add(dmns);
                                        singlereq.Services.PickupDate = "/Date(" + (int)DateTime.UtcNow.AddDays(1).Subtract(new DateTime(1970, 1, 1)).TotalSeconds + "000)/";//timestamp
                                        singlereq.Services.PickupTime = "1600";
                                        singlereq.Services.PieceCount = "1";
                                        singlereq.Services.ProductCode = "A";
                                        singlereq.Services.ProductType = 2;
                                        if (updatecourier.PaymentMode.ToUpper().Contains("CASH"))
                                        {
                                            singlereq.Services.SubProductCode = "C";
                                        }
                                        else
                                        {
                                            singlereq.Services.SubProductCode = "P";
                                        }
                                        singlereq.Services.itemdtl = new List<Itemdtl>();
                                        singlereq.Services.itemdtl.AddRange(allproducts);
                                        singlereq.Shipper = new Shipper();

                                        singlereq.Shipper.CustomerAddress1 = row["Seller Address"].ToString();
                                        singlereq.Shipper.CustomerCode = "301221";
                                        singlereq.Shipper.CustomerEmailID = "orders@healthurwealth.com";
                                        singlereq.Shipper.CustomerMobile = "9440689551";
                                        singlereq.Shipper.CustomerName = "SONAL ENTERPRISES";
                                        singlereq.Shipper.CustomerPincode = row["Return Pin"].ToString();
                                        singlereq.Shipper.OriginArea = "HYD";
                                        singlereq.Shipper.Sender = "Sonal";
                                        singlereq.Shipper.VendorCode = "301221";
                                        requests.Add(singlereq);

                                    }

                                }
                            }
                        }
                    }
                    bluereq.Request = new List<Utility.Request>();
                    bluereq.Request.AddRange(requests);
                    bluereq.Profile = new Profile();
                    bluereq.Profile.Api_type = "S";
                    bluereq.Profile.Area = "Hyd";
                    bluereq.Profile.Customercode = "301221";
                    bluereq.Profile.LicenceKey = "4ulrirrhjpgjlutsumugwhifsisuqpip";
                    bluereq.Profile.LoginID = "HYD99570";
                    //bluereq.Profile.Version = "1.10";

                }
                var body = JsonConvert.SerializeObject(bluereq);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("JWTToken", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJzdWJqZWN0LXN1YmplY3QiLCJhdWQiOlsiYXVkaWVuY2UxIiwiYXVkaWVuY2UyIl0sImlzcyI6InVybjpcL1wvYXBpZ2VlLWVkZ2UtSldULXBvbGljeS10ZXN0IiwiZXhwIjoxNzM4NTIzOTg0LCJpYXQiOjE3Mzg0Mzc1ODQsImp0aSI6IjgwOTQ4ZDJiLWZiYWItNDcyNC1hOWY5LTUyNGE0ZjgwMzlkMCJ9.XY6Sxo8wjlzMcfuMEJggKN0NMnd3pQGmTfWrZt4WIUY");

                request.AddHeader("Cookie", "BIGipServerpl_netconnect-bluedart.dhl.com_443=!JA4PsphH5lKLEuXzvvsIVYa1K6PKfTwl/RvkARVxiMifZsSOx2eOEA9sYkji+nghy0c7cO3eXFaB8po=; BIGipServerpl_netconnect-bluedart.dhl.com_9446=!ZGtr+RxX3/EfU3DzvvsIVYa1K6PKfeYUBMXkyzTHIXdp/RrAEHuxHna2e+P2zpK4xqDLA9rz2K0KcAw=; TS01808994=01914b743d7cb25a2e095570db6893ca5e6e82550f8d20ec2da40938a5538468f93191561df9f87d486daa75a612514d2053c2cd0ebdc3dd8422985f3b21c027e62dc808e90190b9be4cec59437954971da6433da3");
                request.AddParameter("application/json", body, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                Bluedartresponse resData = JsonConvert.DeserializeObject<Bluedartresponse>(response.Content.ToString());
                string finalpath = "";
                foreach (var item in resData.ImportDataResult)
                {
                    long transid = Convert.ToInt64(item.CCRCRDREF);
                    var y = BalUtility.Logmaintainance((int)Shared.OrderStatus.Downloaded, "DownloadedBluedartExcel" + item.Status.FirstOrDefault().StatusInformation + "," + item.AWBNo + "," + item.DestinationArea + "," + item.DestinationLocation, "", transid.ToString());

                    PaymentTransaction updatecourier = context.PaymentTransactions.Where(x => x.PaymentTransactionId == transid).FirstOrDefault();

                    if (item.IsError == false)
                    {
                        string res = DownlaodBluedartPdf(transid.ToString(), item.AWBNo, item.DestinationArea, item.DestinationLocation);
                        finalpath += res + "*";

                        updatecourier.CourierName = "Blue Dart";
                        updatecourier.IsPacking = true;
                        updatecourier.ShipmentId = item.AWBNo;
                        updatecourier.ShipmentDate = DateTime.Now;

                    }
                    else
                    {
                        updatecourier.IsPacking = false;
                        updatecourier.ShipmentDate = DateTime.Now;
                    }
                    context.SaveChanges();

                }
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Result = finalpath
                };
            }
        }
        catch (Exception ex)
        {
            DbLogger.LogError(
        ex,
        null,
        "DownloadBluedartExcel"
    );

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Result = ex.Message + "      " + ex.InnerException
            };
        }
    }


    [HttpGet]
    public CustomResponse DownloadDTDCExcel(string OrderIDs)
    {
        try
        {
            string url = "https://dtdcapi.shipsy.io/api/customer/integration/consignment/softdata";
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;
            BluedartRequest bluereq = new BluedartRequest();
            db_Zon_HuwEntities context = new db_Zon_HuwEntities();
            List<Utility.Consignment> requests = new List<Utility.Consignment>();

            string constr = ConfigurationManager.ConnectionStrings["db_Zon_constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string[] ids = OrderIDs.Split(',');
                foreach (string id in ids)
                {
                    if (!string.IsNullOrEmpty(id))
                    {

                        using (SqlCommand cmd = new SqlCommand("SELECT distinct(p.PaymentTransactionId),'' as Waybill, p.PaymentTransactionId as 'Reference No',u.FirstName+' ' + u.LastName as 'Consignee Name' ,ua.City, ua.StateName as State, 'India' as Country, ua.StreetAddress1 + ',' + ua.StreetAddress2 + ','+ua.City+',' + ua.LandMark as Address , ua.PinCode as Pincode, u.MobileNo as Phone, u.MobileNo as Mobile, '100' as Weight,case when p.PaymentMode='Cash On Delivery' then 'cod' when p.PaymentMode!='Cash On Delivery' then 'prepaid' end as 'Payment Mode' , p.TxnAmount as 'Package Amount',case when p.PaymentMode='Cash On Delivery' then p.TxnAmount when p.PaymentMode!='Cash On Delivery' then '' end as 'Cod Amount','Surface' as 'Shipping Mode','HYDERABAD' as 'Return Address' , '500016' as 'Return Pin','SONAL ENTERPRISES' as 'Seller Name','6-3-850/1,SHIRISHA PLAZA,DHARAM KARAM ROAD,AMEERPET,HYDERABAD' as 'Seller Address', '' as 'Seller CST No','' as 'Seller TIN',p.PaymentTransactionId as 'Invoice No' ,GETDATE() as 'Invoice Date' ,'' as 'Commodity Value','' as 'Tax Value','' as 'Category of Goods' ,'' as 'Seller_GST_TIN','' as HSN_Code ,'' as 'Return Reason' ,'SONLENTERPRISES FRANCHISE' as 'Vendor Pickup Location' ,'' as EWBN,'' as address_specific ,'' as person_specific FROM [dbo].[PaymentTransactions] as p join [dbo].Users as u on p.UserId = u.UserId join dbo.[UserProductTransactions] ut on ut.PaymentTransactionId=p.PaymentTransactionId join [dbo].UserAddresses as ua on ua.UserAddressId = ut.BillingAddressId where p.PaymentTransactionId=" + id))
                        {
                            using (SqlDataAdapter sda = new SqlDataAdapter())
                            {
                                cmd.Connection = con;
                                sda.SelectCommand = cmd;
                                using (DataTable dt = new DataTable())
                                {
                                    sda.Fill(dt);

                                    foreach (DataRow row in dt.Rows)
                                    {
                                        Utility.Consignment singlereq = new Utility.Consignment();
                                        long paymentid = Convert.ToInt64(row["Reference No"]);
                                        PaymentTransaction updatecourier = context.PaymentTransactions.Where(x => x.PaymentTransactionId == paymentid).FirstOrDefault();

                                        Tbl_Packing_Box boxs = context.Tbl_Packing_Box.Where(x => x.BoxId == updatecourier.BoxId).FirstOrDefault();
                                        if (boxs == null)
                                        {
                                            boxs = new Tbl_Packing_Box();
                                            boxs.Lengths = "0";
                                            boxs.Width = "0";
                                            boxs.Height = "0";
                                        }
                                        List<UserProductTransaction> upt = context.UserProductTransactions.Where(x => x.PaymentTransactionId == paymentid).ToList();
                                        List<Itemdtl> allproducts = new List<Itemdtl>();

                                        foreach (var item in upt)
                                        {
                                            Itemdtl itm = new Itemdtl();
                                            Product product = context.Products.Where(x => x.ProductId == item.ProductId).FirstOrDefault();
                                            itm.ItemID = product.ProductId.ToString();
                                            itm.ItemName = product.ProductName;
                                            itm.ItemValue = Convert.ToDouble(item.ProductCost);
                                            itm.Itemquantity = item.Quantity;
                                            itm.TotalValue = Convert.ToDouble(item.ProductCost) * item.Quantity;
                                            itm.cessAmount = 0.0;
                                            allproducts.Add(itm);

                                        }
                                        //consignee
                                        singlereq.customer_code = "HL3633";
                                        singlereq.reference_number = "";
                                        singlereq.service_type_id = "B2C SMART EXPRESS";
                                        singlereq.load_type = "NON-DOCUMENT";
                                        singlereq.description = "Health Essentials, Wellness";
                                        singlereq.consignment_type = "Forward";
                                        singlereq.dimension_unit = "in";
                                        singlereq.length = (boxs.Lengths);
                                        singlereq.width = (boxs.Width);
                                        singlereq.height = (boxs.Height);
                                        singlereq.weight_unit = "kg";
                                        singlereq.weight = "0.3";
                                        singlereq.declared_value = row["Package Amount"].ToString();
                                        singlereq.num_pieces = "001";
                                        singlereq.customer_reference_number = paymentid.ToString();
                                        singlereq.is_risk_surcharge_applicable = true;
                                        singlereq.origin_details = new OriginDetails();
                                        singlereq.origin_details.name = "SONAL ENTERPRISES";
                                        singlereq.origin_details.phone = "9440689551";
                                        singlereq.origin_details.alternate_phone = "9440689551";
                                        singlereq.origin_details.address_line_1 = row["Seller Address"].ToString();
                                        singlereq.origin_details.address_line_2 = row["Return Address"].ToString();
                                        singlereq.origin_details.pincode = row["Return Pin"].ToString();
                                        singlereq.origin_details.city = "Hyderabad";
                                        singlereq.origin_details.state = "Telangana";

                                        if (updatecourier.PaymentMode.ToUpper().Contains("CASH"))
                                        {
                                            singlereq.cod_collection_mode = "cash";
                                            singlereq.cod_amount = row["Package Amount"].ToString();

                                        }
                                        else
                                        {
                                            singlereq.cod_collection_mode = "";
                                            singlereq.cod_amount = "0";
                                        }
                                        singlereq.destination_details = new DestinationDetails();

                                        singlereq.destination_details.name = row["Consignee Name"].ToString();
                                        singlereq.destination_details.phone = row["Phone"].ToString();
                                        singlereq.destination_details.alternate_phone = row["Phone"].ToString();
                                        singlereq.destination_details.address_line_1 = row["Address"].ToString();
                                        singlereq.destination_details.address_line_2 = "";
                                        singlereq.destination_details.pincode = row["Pincode"].ToString();
                                        singlereq.destination_details.city = row["City"].ToString();
                                        singlereq.destination_details.state = row["State"].ToString();
                                        singlereq.pieces_detail = new[]
                                        {
    new PieceDetail
    {
        description = "Medicine",
        declared_value = row["Package Amount"].ToString(),
        weight = "0.5",
        height = "0",
        length = "0",
        width = "0"
    }
};

                                        requests.Add(singlereq);

                                    }

                                }
                            }
                        }
                    }

                }
                DTDCObject dTDCObject = new DTDCObject();
                dTDCObject.consignments = requests;
                var body = JsonConvert.SerializeObject(dTDCObject);
                string path = "/UploadFiles/AWB/output";
                string savePath = System.Web.HttpContext.Current.Server.MapPath(@"~" + path + ".txt");
                // Write JSON to the file
                File.WriteAllText(savePath, body);
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                request.AddHeader("api-key", "fc3ca818b3b4f5be6dce68f2b003bf");
                IRestResponse response = client.Execute(request);
                Utility.ResponseData resData = JsonConvert.DeserializeObject<Utility.ResponseData>(response.Content.ToString());
                //var y = BalUtility.Logmaintainance((int)Shared.OrderStatus.Downloaded, "DownloadDTDCExcel", resData.status, "2247090");

                string finalpath = "";
                foreach (var item in resData.data)
                {
                    long transid = Convert.ToInt64(item.customer_reference_number);
                    var y = BalUtility.Logmaintainance((int)Shared.OrderStatus.Downloaded, "DownloadDTDCExcel" + item.success.ToString() + "," + item.reference_number, "", transid.ToString());

                    PaymentTransaction updatecourier = context.PaymentTransactions.Where(x => x.PaymentTransactionId == transid).FirstOrDefault();

                    if (item.success == true)
                    {
                        string res = DownlaodDTDCPdf(item.reference_number);
                        finalpath += res + "*";

                        updatecourier.CourierName = "DTDC";
                        updatecourier.IsPacking = true;
                        updatecourier.ShipmentId = item.reference_number;
                        updatecourier.ShipmentDate = DateTime.Now;

                    }
                    else
                    {
                        updatecourier.IsPacking = false;
                        updatecourier.ShipmentDate = DateTime.Now;
                    }
                    context.SaveChanges();

                }
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Result = finalpath
                };
            }
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Result = ex.Message + "      " + ex.InnerException
            };
        }
    }

    [HttpGet]
    public CustomResponse AssignDelhiveryAWBS(string IDS)
    {
        List<Manifesto> objmanifestolist = new List<Manifesto>();
        //ProcessedPickup data = new ProcessedPickup();
        var repository = new PaymentTransactionRepository();
        int totalRecords;

        idsCopy = IDS;//HttpContext.Current.Session["OrderID"].ToString();  
        int totalorders = idsCopy.Split(',').Count();
        var EachID = idsCopy.Split(',');
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
        HttpResponseMessage response = client.GetAsync("https://track.delhivery.com/waybill/api/bulk/json/?cl=client_name&token=3f8319389b604558e7f992b3f91ace966c941c3b&count=" + totalorders).Result;
        if (response.IsSuccessStatusCode)
        {
            object resData = new object();
            resData = JsonConvert.DeserializeObject<object>(response.Content.ReadAsStringAsync().Result);
            var Awbnumbers = resData.ToString().Split(',');
            db_Zon_HuwEntities context = new db_Zon_HuwEntities();
            int counter = 0;
            foreach (var item in EachID)
            {
                long paymenttransaction = Convert.ToInt64(item);

                var x = BalUtility.Logmaintainance((int)Shared.OrderStatus.Dispatched, "Assigned delhiveryAWBS", "0", item);

                Tbl_AWB awbalreadyexist = context.Tbl_AWB.Where(p => p.OrderId == paymenttransaction).FirstOrDefault();
                if (awbalreadyexist == null)
                {
                    Tbl_AWB awb = new Tbl_AWB();
                    awb.Deliverytype = 2;//2 means delhivery
                    awb.AWB_Number = Awbnumbers[counter];
                    awb.OrderId = paymenttransaction;
                    awb.CreatedOn = DateTime.Now;
                    counter++;
                    context.Tbl_AWB.Add(awb);
                    context.SaveChanges();
                    //var clients = new RestClient("https://staging-express.delhivery.com/api/cmu/create.json");
                    //client.Timeout = -1;
                    //var request = new RestRequest(Method.POST);
                    //request.AddHeader("Authorization", "Token 3f8319389b604558e7f992b3f91ace966c941c3b  ");
                    //request.AddHeader("Content-Type", "text/plain");
                    //request.AddParameter("text/plain", "format=json&data={\r\n\"pickup_location\": {\r\n\"pin\": \"500084\",\r\n\"add\": \"kondapur\",\r\n\"phone\": \"7702555494\",\r\n\"state\": \"hyderabad\",\r\n\"city\": \"hyderabad\",\r\n\"country\": \"India\",\r\n\"name\": \"SONLENT SURFACE \"\r\n},\r\n\"shipments\": [{\r\n\"return_name\": \"SONLENT SURFACE\",\r\n\"return_pin\": \"500084\",\r\n\"return_city\": \"hyderabad\",\r\n\"return_phone\": \"7776667776\",\r\n\"return_add\": \"\",\r\n\"return_state\": \"telangana\",\r\n\"return_country\": \"India\",\r\n\"order\": \"25547\",\r\n\"phone\": \"7702555494\",\r\n\"products_desc\": \"\",\r\n\"cod_amount\": \"\",\r\n\"name\": \"test\",\r\n\"country\": \"India\",\r\n\"seller_inv_date\": \"\",\r\n\"order_date\": \"2020-05-18 06:22:43\",\r\n\"total_amount\": \"100\",\r\n\"seller_add\": \"\",\r\n\"seller_cst\": \"\",\r\n\"add\": \"jaipur, \",\r\n\"seller_name\": \"\",\r\n\"seller_inv\": \"\",\r\n\"seller_tin\": \"\",\r\n\"pin\": \"500087\",\r\n\"quantity\": \"1\",\r\n\"payment_mode\": \"Prepaid\",\r\n\"state\": \"Delhi\",\r\n\"city\": \"Delhi\",\r\n\"client\": \"SONLENT SURFACE\"\r\n}]\r\n}\r\n", ParameterType.RequestBody);
                    //IRestResponse response = clients.Execute(request);
                    //Console.WriteLine(response.Content);
                }
            }

        }
        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Result = ""
        };

        //if(HttpContext.Current.Session["Data"] != null)
        //{
        //    HttpContext.Current.Response.Redirect("./Admin/ManiFest.aspx");
        //}
    }
    [HttpGet]
    public CustomResponse Advancedpickup(string IDS)
    {
        List<Manifesto> objmanifestolist = new List<Manifesto>();

        var repository = new PaymentTransactionRepository();
        int totalRecords;
        //ProcessedPickup data = new ProcessedPickup();

        idsCopy = IDS;//HttpContext.Current.Session["OrderID"].ToString();       
        var EachID = idsCopy.Split(',');

        HttpContext.Current.Session["Data"] = null;
        foreach (var OrderID in EachID)
        {
            var x = BalUtility.Logmaintainance((int)Shared.OrderStatus.Dispatched, "Pickup-->Dispathed", "0", OrderID);

            db_Zon_HuwEntities context = new db_Zon_HuwEntities();
            long paymenttranid = Convert.ToInt64(OrderID);
            Tbl_AWB awbnumber = context.Tbl_AWB.Where(p => p.OrderId == paymenttranid).FirstOrDefault();
            StringBuilder strtd = new StringBuilder();

            string couriername = repository.CourierName(Convert.ToInt64(OrderID));
            if (couriername == "Aramex")
            {
                //data = CreateShipment.PickUPOrders(DateTime.Now);
                //UpdateWaitingForPickOrderAramex(Convert.ToInt64(OrderID), data.ID.ToString());

                //List<Manifesto> objmanifesto = repository.Manifest(Convert.ToInt64(OrderID));
                //string productnames = "";
                //DateTime ShipmentDates;
                //foreach (Manifesto objmanifesto1 in objmanifesto)
                //{
                //    productnames += objmanifesto1.ProductName + ", ";
                //}
                //objmanifesto[0].ProductName = productnames;
                //if (objmanifesto[0].ShipmentDate.Value.ToShortDateString() == null)
                //{
                //    objmanifesto[0].ShortDate = DateTime.Now.ToShortDateString();
                //}
                //else
                //{
                //    objmanifesto[0].ShortDate = objmanifesto[0].ShipmentDate.Value.ToShortDateString();
                //}

                //objmanifestolist.Add(objmanifesto[0]);
            }
            else
            {
                //data.ID = "0";

                if (awbnumber != null)
                {
                    UpdateWaitingForPickOrderOther(Convert.ToInt64(OrderID), awbnumber.AWB_Number, awbnumber.AWB_Number);
                }
                else
                {
                    return new CustomResponse
                    {
                        Status = Shared.ResponseStatus.Fail.ToString(),
                        Result = "Awb number not assigned"
                    };
                }
            }
            long ptId = Convert.ToInt64(OrderID);

            var repository1 = new PaymentTransactionRepository();
            var PaymntDtls = repository1.First(p => p.PaymentTransactionId == ptId);

            var Repsitory2 = new UserRepository();
            var UsrDtls = Repsitory2.First(u => u.UserId == PaymntDtls.User.UserId);
            string Message = "";
            if (couriername == "Delhivery")
            {
                Message = "Your " + ptId + " is Dispatched. Tracking Url : https://www.delhivery.com/track/package/" + awbnumber.AWB_Number + " .Tracking ID " + awbnumber.AWB_Number + " .Healthurwealth.com (SONAL ENTERPRISES)";
                string Url = System.Configuration.ConfigurationManager.AppSettings["SmsUrl"].ToString();
                string UserName = System.Configuration.ConfigurationManager.AppSettings["SmsId"].ToString();
                string password = System.Configuration.ConfigurationManager.AppSettings["SmsPwd"].ToString();
                string Status = Utility.MailMessage.SendSms(Url, UserName, password, UsrDtls.MobileNo, Message, "N", "1707162122732475842");
                //Message = "Your Product is ready to Dispatch. You can track your shipment at https://www.delhivery.com/track/package/" + awbnumber.AWB_Number + " . Your Product sent through " + couriername + ". Your Tracking ID is " + awbnumber.AWB_Number + " .Healthurwealth.com (SONAL ENTERPRISES)";
            }
            else
            {
                Message = "Your " + ptId + " is Dispatched. Tracking Url : https://ecomexpress.in/tracking/?awb_field=" + awbnumber.AWB_Number + ". Tracking ID " + awbnumber.AWB_Number + " .Healthurwealth.com (SONAL ENTERPRISES)";
                string Url = System.Configuration.ConfigurationManager.AppSettings["SmsUrl"].ToString();
                string UserName = System.Configuration.ConfigurationManager.AppSettings["SmsId"].ToString();
                string password = System.Configuration.ConfigurationManager.AppSettings["SmsPwd"].ToString();
                string Status = Utility.MailMessage.SendSms(Url, UserName, password, UsrDtls.MobileNo, Message, "N", "1707162122737017503");
                //Message = "Your Product is ready to Dispatch. You can track your shipment at https://ecomexpress.in/tracking/?awb_field=" + awbnumber.AWB_Number + " . Your Product sent through " + couriername + ". Your Tracking ID is " + awbnumber.AWB_Number + " .Healthurwealth.com (SONAL ENTERPRISES)";
            }


        }

        HttpContext.Current.Session["Data"] = objmanifestolist;

        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Result = ""
        };

        //if(HttpContext.Current.Session["Data"] != null)
        //{
        //    HttpContext.Current.Response.Redirect("./Admin/ManiFest.aspx");
        //}
    }
    //Master Controller
    [HttpGet]
    public CustomResponse CreatePickup(string IDS, string ShipmentID, string PickUPID)
    {
        var x = BalUtility.Logmaintainance((int)Shared.OrderStatus.Dispatched, "Pickup-->Dispathed", ShipmentID, IDS);
        List<Manifesto> objmanifestolist = new List<Manifesto>();

        var repository = new PaymentTransactionRepository();
        int totalRecords;
        //ProcessedPickup data = new ProcessedPickup();

        idsCopy = IDS;//HttpContext.Current.Session["OrderID"].ToString();       
        var EachID = idsCopy.Split(',');

        HttpContext.Current.Session["Data"] = null;
        foreach (var OrderID in EachID)
        {

            StringBuilder strtd = new StringBuilder();

            string couriername = repository.CourierName(Convert.ToInt64(OrderID));
            if (couriername == "Aramex")
            {
                //data = CreateShipment.PickUPOrders(DateTime.Now);
                //UpdateWaitingForPickOrderAramex(Convert.ToInt64(OrderID), data.ID.ToString());

                //List<Manifesto> objmanifesto = repository.Manifest(Convert.ToInt64(OrderID));
                //string productnames = "";
                //DateTime ShipmentDates;
                //foreach (Manifesto objmanifesto1 in objmanifesto)
                //{
                //    productnames += objmanifesto1.ProductName + ", ";
                //}
                //objmanifesto[0].ProductName = productnames;
                //if (objmanifesto[0].ShipmentDate.Value.ToShortDateString() == null)
                //{
                //    objmanifesto[0].ShortDate = DateTime.Now.ToShortDateString();
                //}
                //else
                //{
                //    objmanifesto[0].ShortDate = objmanifesto[0].ShipmentDate.Value.ToShortDateString();
                //}

                //objmanifestolist.Add(objmanifesto[0]);
            }
            else
            {
                //data.ID = "0";
                UpdateWaitingForPickOrderOther(Convert.ToInt64(OrderID), ShipmentID, PickUPID);
            }
            long ptId = Convert.ToInt64(OrderID);

            var repository1 = new PaymentTransactionRepository();
            var PaymntDtls = repository1.First(p => p.PaymentTransactionId == ptId);

            var Repsitory2 = new UserRepository();
            var UsrDtls = Repsitory2.First(u => u.UserId == PaymntDtls.User.UserId);
            string Message = "";
            if (couriername == "Delhivery")
            {
                Message = "Your Product is ready to Dispatch. You can track your shipment at https://www.delhivery.com/track/package/" + ShipmentID + " . Your Product sent through " + couriername + ". Your Tracking ID is " + ShipmentID + " .";
            }
            else
            {
                Message = "Your Product is ready to Dispatch. You can track your shipment at https://ecomexpress.in/tracking/?awb_field=" + ShipmentID + " . Your Product sent through " + couriername + ". Your Tracking ID is " + ShipmentID + " .";
            }
            string Url = System.Configuration.ConfigurationManager.AppSettings["SmsUrl"].ToString();
            string UserName = System.Configuration.ConfigurationManager.AppSettings["SmsId"].ToString();
            string password = System.Configuration.ConfigurationManager.AppSettings["SmsPwd"].ToString();
            string Status = Utility.MailMessage.SendSms(Url, UserName, password, UsrDtls.MobileNo, Message, "N", "1707160960575460833");

        }

        HttpContext.Current.Session["Data"] = objmanifestolist;

        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Result = ""
        };

        //if(HttpContext.Current.Session["Data"] != null)
        //{
        //    HttpContext.Current.Response.Redirect("./Admin/ManiFest.aspx");
        //}
    }


    public CustomResponse UpdatePickup(long ID, string ShipmentID, string PickUPID)
    {
        db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();
        PaymentTransaction PT = Entity.PaymentTransactions.Where(x => x.PaymentTransactionId == ID).First();
        PT.ShipmentId = ShipmentID;
        PT.PickupID = PickUPID;
        Entity.SaveChanges();


        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Result = ""
        };

        //if(HttpContext.Current.Session["Data"] != null)
        //{
        //    HttpContext.Current.Response.Redirect("./Admin/ManiFest.aspx");
        //}
    }

    public CustomResponse UpdateWaitingForPickOrderAramex(long OrderID, string PickUPID)
    {
        try
        {
            var repository = new PaymentTransactionRepository();
            var updatedata = repository.First(p => p.PaymentTransactionId == OrderID);

            updatedata.Pickup = true;
            updatedata.Dispatched = true;
            updatedata.PickupID = PickUPID;
            updatedata.PickupDate = DateTime.Now;
            updatedata.DispatchedDate = DateTime.Now;

            updatedata.OrderCurrentStatus = (int)Shared.OrderStatus.Dispatched;
            string htmlpgePath = Shared.GethtmlPage(Shared.ProductStatusForSendMail.Dispatched);
            string subject = Shared.GetPrdctStatusSubject(Shared.ProductStatusForSendMail.Dispatched);
            PaymentTransaction PymntPrdctDispatcheddata = updatedata;
            BalUtility.SendMailForProductStatus(updatedata, Shared.GethtmlPage(Shared.ProductStatusForSendMail.Dispatched), Shared.GetPrdctStatusSubject(Shared.ProductStatusForSendMail.Dispatched));
            repository.Update(updatedata);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = OrderID
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };

        }
    }
    //private void SendDispatchMailIfRequired(PaymentTransaction transaction)
    //{
    //    if (transaction != null &&
    //        transaction.Dispatched &&
    //        transaction.OrderCurrentStatus == (int)Shared.OrderStatus.Dispatched)
    //    {
    //        BalUtility.SendMailForProductStatus(
    //            transaction,
    //            Shared.GethtmlPage(Shared.ProductStatusForSendMail.Dispatched),
    //            Shared.GetPrdctStatusSubject(Shared.ProductStatusForSendMail.Dispatched)
    //        );
    //    }
    //}

    public CustomResponse UpdateWaitingForPickOrderOther(long OrderID, string shipmentID, string PickUPID)
    {
        try
        {
            var repository = new PaymentTransactionRepository();
            var updatedata = repository.First(p => p.PaymentTransactionId == OrderID);

            updatedata.Pickup = true;
            updatedata.Dispatched = true;
            updatedata.PickupID = PickUPID;
            updatedata.ShipmentId = shipmentID;
            updatedata.PickupDate = DateTime.Now;
            updatedata.DispatchedDate = DateTime.Now;

            updatedata.OrderCurrentStatus = (int)Shared.OrderStatus.Dispatched;
            string htmlpgePath = Shared.GethtmlPage(Shared.ProductStatusForSendMail.Dispatched);
            string subject = Shared.GetPrdctStatusSubject(Shared.ProductStatusForSendMail.Dispatched);
            PaymentTransaction PymntPrdctDispatcheddata = updatedata;
            repository.Update(updatedata);
            BalUtility.SendMailForProductStatus(
                updatedata,
                Shared.GethtmlPage(Shared.ProductStatusForSendMail.Dispatched),
                Shared.GetPrdctStatusSubject(Shared.ProductStatusForSendMail.Dispatched)

            );

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = OrderID
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };

        }
    }


    public CustomResponse GetProductListBySrch(int rows, long ProductId, string ProductName = null, int SuperCategory = 0, int Category = 0, int SubCategory = 0, string ProductStatus = null, string Brand = null, int Quantity = 0)
    {
        try
        {
            if (Brand == "Select")
                Brand = null;
            var repository = new ProductRepository();
            var datas = repository.GetProducts(rows, ProductId, ProductName, SuperCategory, Category, SubCategory, ProductStatus, Brand, Quantity);

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = datas
            };
        }

        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Message = ex.Message,
                Result = null
            };
        }


    }
    public CustomResponse GetProductListBySrchfortopselling(int rows, long ProductId, string ProductName = null, int SuperCategory = 0, int Category = 0, int SubCategory = 0, string ProductStatus = null, string Brand = null, int Quantity = 0, int NoOfProds = -1, int NoOfPastDays = -1)
    {
        try
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                bool PrdctStats = true;
                var repository = new ProductRepository();
                if (Brand == "Select")
                    Brand = null;
                long ProductID = ProductId;

                var datas = repository.GetProductsfortopselling(rows, ProductId, ProductName, SuperCategory, Category, SubCategory, ProductStatus, Brand, Quantity, NoOfProds, NoOfPastDays);
                //if(NoOfProds!=-1)
                //{
                //    datas = repository.GetProducts(rows, ProductId, ProductName, SuperCategory, Category, SubCategory, ProductStatus, Brand, Quantity,NoOfProds);
                //}
                //if (NoOfPastDays != -1)
                //{
                //    datas = repository.GetProducts(rows, ProductId, ProductName, SuperCategory, Category, SubCategory, ProductStatus, Brand, Quantity,NoOfPastDays);
                //}
                //if (NoOfProds != -1 && NoOfPastDays != -1)
                //{
                //    datas = repository.GetProducts(rows, ProductId, ProductName, SuperCategory, Category, SubCategory, ProductStatus, Brand, Quantity,NoOfProds,NoOfPastDays);
                //}
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Result = datas
                };
            }
        }

        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Message = ex.Message,
                Result = null
            };
        }


    }
    public CustomResponse UpdateProductQuantity(int ProductID, int Quantity)
    {
        try
        {
            var repository = new ProductRepository();
            var P = repository.First(p => p.ProductId == ProductID);

            P.Quantity = Quantity;
            P.IsSold = false;
            if (P.Quantity >= 0)
            {
                db_Zon_HuwEntities context = new db_Zon_HuwEntities();
                Tbl_Outofstock data = context.Tbl_Outofstock.Where(x => x.Productid == P.ProductId).FirstOrDefault();
                if (data != null)
                {
                    context.Tbl_Outofstock.Remove(data);
                    context.SaveChanges();
                }
            }
            repository.Update(P);

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = "Quantity Updated Successfully"
            };
        }
        catch (Exception ex)
        {
            throw ex;
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = "Qunatity Not Updated"
            };
        }

    }

    public CustomResponse UpdateWaitingForPickOrder(PaymentTransaction data)
    {
        try
        {
            var repository = new PaymentTransactionRepository();
            var updatedata = repository.First(p => p.PaymentTransactionId == data.PaymentTransactionId);

            updatedata.Pickup = true;
            updatedata.Dispatched = true;
            updatedata.PickupDate = DateTime.Now;
            updatedata.DispatchedDate = DateTime.Now;

            updatedata.OrderCurrentStatus = (int)Shared.OrderStatus.Dispatched;
            string htmlpgePath = Shared.GethtmlPage(Shared.ProductStatusForSendMail.Dispatched);
            string subject = Shared.GetPrdctStatusSubject(Shared.ProductStatusForSendMail.Dispatched);
            PaymentTransaction PymntPrdctDispatcheddata = updatedata;
            BalUtility.SendMailForProductStatus(updatedata, Shared.GethtmlPage(Shared.ProductStatusForSendMail.Dispatched), Shared.GetPrdctStatusSubject(Shared.ProductStatusForSendMail.Dispatched));
            repository.Update(updatedata);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = data.PaymentTransactionId
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }
    }
    public CustomResponse GetDispatchedOrders()
    {
        var repository = new PaymentTransactionRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, 1000, (p => p.TxnStatus == "SUCCESS" && p.Dispatched == true && p.Delivered == false && p.Pickup == true && p.Authorized == true), null, "", true);
        //var getdata = data
        var filteredData = (from q in (data as List<PaymentTransaction>)
                            select new
                            {
                                q.User.FirstName,
                                q.User.LastName,
                                q.User.MobileNo,
                                q.User.EmailId,
                                q.User.UserId,
                                q.PaymentTransactionId,
                                q.PickupDate,
                                Quantity = q.UserProductTransactions.First().Quantity,
                                q.CourierName,
                                q.PaymentStatus,
                                q.TxnAmount,
                                q.TxnRefNo,
                                q.TxnStatus,
                                currency = q.CurrencyCode + " ( " + q.CurrencySymbol + " ) ",
                                q.CurrencyCode,
                                q.CurrencySymbol,
                                q.PGTxnId,
                                q.TxnMessage,
                                q.ShipmentId,
                                q.PickupID,
                                q.CreatedOn,
                                q.DispatchedDate,
                                q.OrderCurrentStatus,
                                q.Orderdeliverystatus,
                                products = q.UserProductTransactions.Where(cat => cat.PaymentTransactionId == q.PaymentTransactionId).
                                  Select(u => string.Join(",", (u.Product.ProductName + (u.SubProduct == null ? "" : " - " + u.SubProduct.SPName))))

                            }).OrderByDescending(p => p.DispatchedDate).ToList();
        StringBuilder strtd = new StringBuilder();

        if (data.Count > 0)
        {
            foreach (var item in filteredData)
            {
                decimal Currency = Convert.ToDecimal(item.TxnAmount);
                string Amount = Currency.ToString("0.00");
                DateTime dt = item.CreatedOn;
                DateTime dtpic = Convert.ToDateTime(item.PickupDate);
                string Pickupdate = dtpic.ToString("dd/MMM/yyyy");
                string CreateOn = dt.ToString("dd/MMM/yyyy");
                string DispatchDate = Convert.ToDateTime(item.DispatchedDate).ToString("dd/MM/yyyy");
                string ProductName = "";

                for (var j = 0; j < item.products.Count(); j++)
                {
                    if (j == 0)
                    {
                        ProductName = item.products.ElementAtOrDefault(j).ToString();
                    }
                    else
                    {
                        ProductName = ProductName + " ," + item.products.ElementAtOrDefault(j).ToString();
                    }
                }
                string bgrowcolor = "";
                if (item.PickupDate != null)
                {
                    if (item.PickupDate.Value.ToShortDateString() == DateTime.Now.ToShortDateString())
                    {
                        bgrowcolor = "#A9DFBF";
                    }
                    else if (item.PickupDate >= DateTime.Now.AddDays(-2))
                    {
                        bgrowcolor = "#F5CBA7";
                    }
                    else
                    {
                        bgrowcolor = "#E6B0AA";
                    }
                }
                var addressrepository = new AddressRepository();
                UserAddress address = addressrepository.UserAddress(item.UserId);
                StringBuilder strDTDC = new StringBuilder();
                if (item.CourierName == "DTDC")
                {
                    strDTDC.Append(@"<a href='http://www.dtdc.in/tracking/tracking_results.asp' target='_blank'>" + item.CourierName + "</a>");
                }
                else
                {
                    strDTDC.Append(item.CourierName);
                }
                strtd.Append(@"<tr style='background-color:" + bgrowcolor + @"'>
				<td >
												<div id='chkBlkShip' >
<input  id='chkkShip' name='chkBlkShip' value=" + item.PaymentTransactionId + @"  type='checkbox' class='Check' onchange='DispatchedCheckboxChecked(this)' ></div>
												<div style='visibility: hidden;'>
													<span id='lblTempOid'>" + item.PaymentTransactionId + @"</span>
												</div>
												
												<input  id='hdnShippingMode' class='hdnShippingMode' value='self' type='hidden'>
											</td><td>
												<a id='hypEdit'onclick='NavigatetoOrderDetailsPage(" + item.PaymentTransactionId + ")'  >" + item.PaymentTransactionId + @"</a><br>
												<span id='lblPartialShip'></span>
											</td><td>
												<span id='OrderDate'>" + CreateOn + @"</span>   
											</td>
<td>
												<span id='Products'>" + ProductName + @"</span>   
											</td>
<td>
												<span id='Products'>" + Pickupdate + @"</span>   
											</td>
<td>
												<span id='OrderDate'>Shipment ID:<a id='shipmentID' onclick=vpb_show_login_box_edit(" + item.PaymentTransactionId + ",'" + item.ShipmentId + "','" + item.PickupID + "')>" + item.ShipmentId + @"</a><br />
																	PickUp ID:<a id='pickUpID' onclick=vpb_show_login_box_edit(" + item.PaymentTransactionId + ",'" + item.ShipmentId + "','" + item.PickupID + "')>" + item.PickupID + @"</a><br />
Order Current Status:<a id='statuslbl' >" + item.Orderdeliverystatus + @"</a><br />
																	 CourierName:" + strDTDC.ToString() + @"
</span>
											</td>
<td>
													<span id='lblTotalProducts'><span class='QuantityTipsySpan'>" + item.products.Count() + @"</span></span>
												
											</td><td align='right'>                                                
												<span id='lblTotalPrice'><span class='WebRupee'>" + item.CurrencySymbol + @" </span>" + Amount + @"</span>
											</td>
<td style='display:none'>
												<span id='OrderDate'>" + item.FirstName + @"</span>
												
											</td>
<td style='display:none'>
												<span id='OrderDate'>" + item.MobileNo + @"</span>
												
											</td>
<td style='display:none'>
												<span id='OrderDate'>" + item.EmailId + @"</span>
												
											</td>
<td style='display:none'>
<span id='userAddress'>" + address.StreetAddress1 + ", " + address.StreetAddress2 + ", " + address.LandMark + ", " + address.City + ", " + address.StateName + ", " + address.PinCode + @"</span>
</td>
			</tr>");
            }
            StringBuilder strResult = new StringBuilder();
            strResult.Append(@"
		<div >                   
					<div >
							<div id='Main_dvGrid'>
								<div> 
								<div>
		<table class='activity_datatable' rules='all' id='ctl00_ctl00_Main_Main_grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
			<thead>
<tr class='checked'>
				<th class='cp_product_select' scope='col'>                                                
												<span title='Select/Unselect all orders'>
<div id='chkSelectAll' class='checker'><span>
<input style='opacity: 0;' id='ctl00_ctl00_Main_Main_grdShippingOrders_ctl01_chkSelectAll'  onclick='selectUnselectCheckboxes(this.id, 'chkBlkShip', 'lblTempOid');' type='checkbox'></span></div></span>
											</th><th scope='col'>ID</th>
							<th scope='col'>Order Date</th>
							<th scope='col'>Product</th>
                        <th scope='col'>Date</th>
						<th scope='col'>Courier</th>
						<th scope='col'>Qty</th>
<th scope='col'><div id='divCurrency'>Amount</div></th>
<th scope='col' style='display:none'>Customer</th>
<th scope='col' style='display:none'>Mobile</th>
<th scope='col' style='display:none'>Email</th>
<th scope='col' style='display:none'>Address</th>
			</tr></thead><tbody>
" + strtd + @"
</tbody></table>	</div></div></div></div></div></div></div></div>
<div class='widget_body'>
			<div style='display: block;' class='action_bar text_right' id='dvbtn'>
<div id='productListexample' class='k-content'>
	  <div id='divdeliver' clientidmode='static'>                                              
<input style='display: inline-block;'  value='Deliver' class='button_small greyishBtn fl_right' onclick='vpb_show_login_box();' type='button'></div>
</div>
<div class='clear'>
				</div>
			</div>
		</div>");
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strResult.ToString()
            };
        }
        else
        {
            StringBuilder strResult = new StringBuilder();
            strResult.Append(@" <div id='ctl00_ctl00_Main_Main_divMessage' class='msgbar msg_Info hide_onC'>
								<span id='ctl00_ctl00_Main_Main_msgicon' class='iconsweet'>*</span>
								<p><span id='ctl00_ctl00_Main_Main_lblMessage'>There are no 'WaitingForPickUp Orders' found.</span></p>
							</div>");
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = strResult.ToString()
            };
        }
    }
    public CustomResponse UpdateDispatchedOrders(PaymentTransaction data)
    {
        try
        {
            //var y = BalUtility.Logmaintainance((int)Shared.OrderStatus.Delivered, "MovedFromDispatchetodelivered", "", data.PaymentTransactionId.ToString());
            var repository = new PaymentTransactionRepository();
            var updatedata = repository.First(p => p.PaymentTransactionId == data.PaymentTransactionId);
            updatedata.Delivered = true;
            updatedata.DeliveredDate = DateTime.Now;
            updatedata.ReceivedBy = data.ReceivedBy;
            updatedata.Comments = data.Comments;
            updatedata.OrderCurrentStatus = (int)Shared.OrderStatus.Delivered;
            repository.Update(updatedata);
            //var userproductrepository = new userproducttransactionRepository();
            //var userproduct = userproductrepository.First(p => p.PaymentTransactionId == updatedata.PaymentTransactionId);
            //var userrepository = new UserRepository();
            //var userdata = userrepository.First(u => u.UserId == updatedata.UserId);
            //var productrepository = new ProductRepository();
            //var productdata = productrepository.First(p => p.ProductId == userproduct.ProductId);

            //string Message = "Your product " + productdata.ProductName + " was ready to Delivery. You can track your shipment at " + ApiUrl + "MyTransactions.aspx. Invoice will be sent to your registered email within 24 hours of delivery.";
            //string Url = System.Configuration.ConfigurationManager.AppSettings["SmsUrl"].ToString();
            //string UserName = System.Configuration.ConfigurationManager.AppSettings["SmsId"].ToString();
            //string password = System.Configuration.ConfigurationManager.AppSettings["SmsPwd"].ToString();
            //string Status = Utility.MailMessage.SendSms(Url, UserName, password, userdata.MobileNo, Message, "N");
            //BalUtility.SendMailForProductStatus(updatedata, Shared.GethtmlPage(Shared.ProductStatusForSendMail.Delivered), Shared.GetPrdctStatusSubject(Shared.ProductStatusForSendMail.Delivered));



            //Pending money from pending cashback wallet transactions will be transfered to user wallet since he has taken the order.
            //temporarily disabling since same function used on user side
            //PendingCashbackTransactionToCashbackTransaction(data.PaymentTransactionId);

            //--------------------------------------------------------------------




            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = data.ShipmentId
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };

        }
    }

    public void PendingCashbackTransactionToCashbackTransaction(long transactionid)
    {
        db_Zon_HuwEntities Context = new db_Zon_HuwEntities();
        //var repository = new List<PendingCashBackTable>();
        var transactionidinstring = Convert.ToString(transactionid);
        var Pending_transaction_record = Context.PendingCashBackTables.Where(x => x.orderid == transactionidinstring).FirstOrDefault();
        //var cashbackIDfrompending = Convert.ToString(Pending_transaction_record.CashBackID == null ? 9999 : Pending_transaction_record.CashBackID);

        //adding record to cahback transactions
        var Successful_transaction_record = new CashBackTable();
        {
            Successful_transaction_record.orderid = Pending_transaction_record.orderid;
            Successful_transaction_record.date = DateTime.Now;
            Successful_transaction_record.totalAmount = Pending_transaction_record.totalAmount;
            Successful_transaction_record.credit = Pending_transaction_record.credit;
            Successful_transaction_record.debit = Pending_transaction_record.debit;
            Successful_transaction_record.balance = Pending_transaction_record.balance;
            Successful_transaction_record.Userid = Pending_transaction_record.Userid;
            Successful_transaction_record.PGTxnid = Pending_transaction_record.PGTxnid;
            Successful_transaction_record.Messages = "Cashback amount credited to wallet since COD delivery successfull";
            //Successful_transaction_record.CashbackID = Convert.ToInt64(cashbackIDfrompending);
        }
        Context.CashBackTables.Add(Successful_transaction_record);

        //removing pre-existing record from pending transactions
        Context.PendingCashBackTables.Remove(Pending_transaction_record);

        //saving changes
        Context.SaveChanges();
    }



    public CustomResponse SignupNewslettrMailId(string EmailId)
    {
        try
        {
            BalUtility.ForNewslettrEmailIdToInfo(EmailId);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = ""
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }
    }
    public CustomResponse GetMyOrders(int rows)
    {

        var repository = new PaymentTransactionRepository();
        int totalRecords;

        var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, rows, (p => p.Dispatched == false && p.Delivered == false && p.Pickup == false && p.Authorized == false), null, "", true);

        StringBuilder strtd = new StringBuilder();

        if (data.Count > 0)
        {
            foreach (var item in data)
            {
                strtd.Append(@"<tr>
				<td>
												<div id='chkBlkShip' >
<input  id='chkkShip' name='chkBlkShip' value=" + item.PaymentTransactionId + @"  type='checkbox' class='Check' onchange='CheckboxChecked(this.value)' ></div>
												<div style='visibility: hidden;'>
													<span id='lblTempOid'>" + item.PaymentTransactionId + @"</span>
												</div>
												
												<input  id='hdnShippingMode' class='hdnShippingMode' value='self' type='hidden'>
											</td><td>
												<a id='hypEdit' onclick='NavigatetoMyOrderDetailsPage(" + item.PaymentTransactionId + ")' >" + item.PaymentTransactionId + @"</a><br>
												<span id='lblPartialShip'></span>
											</td><td>
												<span id='OrderDate'>" + item.CreatedOn + @"</span>   
											</td>
<td>
												<span id='OrderDate'>" + item.DeliveredDate + @"</span>   
											</td>
<td>
												<span id='OrderDate'>" + item.CourierName + @"</span>
												
											</td>

<td>
												
													<span id='lblTotalProducts'><span class='QuantityTipsySpan'>1</span></span>
												
											</td><td align='right'>                                                
												<span id='lblTotalPrice'><span class='WebRupee'>" + item.CurrencySymbol + @" </span>" + item.TxnAmount + @"</span>
											</td><td>                                                
												<span id='lblcustName'><span original-title='&lt;div class=&quot;addressdetails&quot;&gt;&lt;div class=&quot;p_nm&quot;&gt;" + item.ShipmentId + @"&lt;/div&gt;&lt;p&gt;" + item.User.MobileNo + @"&lt;/br&gt;Mumbai&lt;/br&gt;" + item.User.EmailId + @"&lt;/br&gt;Maharashtra&lt;/br&gt;" + item.User.UserId + @"&lt;/p&gt;&lt;' class='QuantityTipsySpan'>" + item.ShipmentId + @"</span></span>
											</td>
			</tr>");
            }
            StringBuilder strResult = new StringBuilder();
            strResult.Append(@"
		<div class='widget'>                   
					
					<div class='widget_body'>
							<div id='Main_dvGrid' class='tab-grdpanel'>
								<div class='widget marg-0'> 
								<div class='widget_title'>
									<span class='iconsweet'></span><h5>Available Orders</h5>
								</div>
								<div class='widget_body'>
						
							<div class='cp_productlist_view'>
								<div class='products_views'>
									
								</div>
							</div>
						   
		<table class='activity_datatable' rules='all' id='ctl00_ctl00_Main_Main_grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
			<tbody>
<tr>
				<th class='cp_product_select' scope='col'>                                                
												<span title='Select/Unselect all orders'>
<div id='chkSelectAll' class='checker'><span>
<input style='opacity: 0;' id='ctl00_ctl00_Main_Main_grdShippingOrders_ctl01_chkSelectAll'  onclick='selectUnselectCheckboxes(this.id, 'chkBlkShip', 'lblTempOid');' type='checkbox'></span></div></span>
											</th><th scope='col'>Order ID</th><th scope='col'>Order Date</th><th scope='col'>Delivered Date</th><th scope='col'>Courier Details</th><th scope='col'>Qty</th><th scope='col'><div id='divCurrency'>Order Value</div></th><th scope='col'>Shipment ID</th>
			</tr>
" + strtd + @"
</tbody></table>	</div>

  <div class='cp_grdpager'>
					<div class='grdinfo'>
					  <label>Records per page</label>
					  <div id='ddlNoRow' class='selector'>
					<span>" + rows + @"</span>
<select style='opacity: 0;'  onchange='GetDeliveredOrders()' id='ddlNoRows' class='uniform_pager_list'>	

	</select></div> 
		
						
					</div>
					  <div class='grd_pages'>
					  <label class='pages'>
			Records:
			<span id='ctl00_ctl00_Main_Main_usrGridPaging1_lblPageNo'>1</span>
			<span id='ctl00_ctl00_Main_Main_usrGridPaging1_lbl_of'>of</span>
			<span id='ctl00_ctl00_Main_Main_usrGridPaging1_lblPageCount'>1</span><span id='ctl00_ctl00_Main_Main_usrGridPaging1_lblPagestxt'> Pages</span></label>
			  
			 
<div id='dvHidden'>
	
	
	
</div>
					  </div>
					  <div class='clear'></div>
					</div>

	 </div>
						
						</div>
									</div>
							</div>
							
							
					</div>
				</div>
				<input name='ctl00$ctl00$Main$Main$hdnTodoSearchcriteria' id='ctl00_ctl00_Main_Main_hdnTodoSearchcriteria' value='[&quot;Order No&quot;,&quot;SKU&quot;,&quot;Email Id&quot;,&quot;Phone No&quot;,&quot;Coupon Code&quot;,&quot;Checkout Type&quot;]' type='hidden'>
				<img id='img441' alt='' onload='getdivCurrency();' src='Orders_files/designoption_009_13.jpg' height='1' width='1'>
			
</div>
		

<div class='widget_body'>
			<div style='display: block;' class='action_bar text_right' id='dvbtn'>
<input style='display: inline-block;'  value='Report Issue(s)' class='button_small greyishBtn fl_right' onclick='vpb_show_login_box();' type='button'>
			   
				<div class='clear'>
				</div>
			</div>
		</div>");


            //    <option value='10' selected='selected'>5</option>
            //<option value='5'>10</option>
            //<option  value='10'>15</option>
            //<option value='15'>20</option>
            //<option value='20'>25</option>

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strResult.ToString()

            };
        }

        else
        {
            StringBuilder strResult = new StringBuilder();
            strResult.Append(@" <div id='ctl00_ctl00_Main_Main_divMessage' class='msgbar msg_Info hide_onC'>
								<span id='msgicon' class='iconsweet'>*</span>
								<p><span id='lblMessage'>There are no 'DeliveredOrders' found.</span></p>
							</div>");

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = strResult.ToString()
            };
        }
    }



    public CustomResponse GetDeliveredOrders()
    {

        var repository = new PaymentTransactionRepository();
        int totalRecords;

        var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, 1000, (p => p.Dispatched == true && p.Delivered == true && p.Pickup == true && p.Authorized == true), null, "", true);
        var filteredData = (from q in (data as List<PaymentTransaction>)
                            select new
                            {
                                q.User.FirstName,
                                q.User.LastName,
                                q.User.MobileNo,
                                q.User.EmailId,
                                q.User.UserId,
                                q.PaymentTransactionId,
                                Quantity = q.UserProductTransactions.Count(),
                                q.PaymentStatus,
                                q.TxnAmount,
                                q.TxnRefNo,
                                q.TxnStatus,
                                currency = q.CurrencyCode + " ( " + q.CurrencySymbol + " ) ",
                                q.CurrencyCode,
                                q.CurrencySymbol,
                                q.PGTxnId,
                                q.TxnMessage,
                                q.CreatedOn,
                                q.OrderCurrentStatus,
                                q.PaymentMode,
                                q.CourierName,
                                q.ShipmentId,
                                q.PickupID,
                                q.DispatchedDate,
                                products = q.UserProductTransactions.Where(cat => cat.PaymentTransactionId == q.PaymentTransactionId).
                                  Select(u => string.Join(",", (u.Product.ProductName + (u.SubProduct == null ? "" : " - " + u.SubProduct.SPName))))

                            }).ToList();
        StringBuilder strtd = new StringBuilder();

        if (data.Count > 0)
        {
            foreach (var item in filteredData)
            {
                string ProductName = "";

                for (var j = 0; j < item.products.Count(); j++)
                {
                    if (j == 0)
                    {
                        ProductName = item.products.ElementAtOrDefault(j).ToString();
                    }
                    else
                    {
                        ProductName = ProductName + " ," + item.products.ElementAtOrDefault(j).ToString();
                    }
                }
                string bgrowcolor = "";
                if (item.DispatchedDate != null)
                {
                    if (item.DispatchedDate.Value.ToShortDateString() == DateTime.Now.ToShortDateString())
                    {
                        bgrowcolor = "#A9DFBF";
                    }
                    else if (item.DispatchedDate >= DateTime.Now.AddDays(-2))
                    {
                        bgrowcolor = "#F5CBA7";
                    }
                    else
                    {
                        bgrowcolor = "#E6B0AA";
                    }
                }

                var addressrepository = new AddressRepository();
                UserAddress address = addressrepository.UserAddress(item.UserId);

                strtd.Append(@"<tr  style='background-color:" + bgrowcolor + @"'>
				<td>
												<div id='chkBlkShip' >
<input  id='chkkShip' name='chkBlkShip' value=" + item.PaymentTransactionId + @"  type='checkbox' class='Check' onchange='CheckboxChecked(this.value)' ></div>
												<div style='visibility: hidden;'>
													<span id='lblTempOid'>" + item.PaymentTransactionId + @"</span>
												</div>
												
												<input  id='hdnShippingMode' class='hdnShippingMode' value='self' type='hidden'>
											</td><td>
												<a id='hypEdit' onclick='NavigatetoOrderDetailsPage(" + item.PaymentTransactionId + ")' >" + item.PaymentTransactionId + @"</a><br>
												<span id='lblPartialShip'></span>
											</td><td>
												<span id='OrderDate'>" + item.CreatedOn.ToString("dd/MMM/yyyy") + @"</span>   
											</td>
<td>
												<span id='OrderDate'>" + ProductName + @"</span>   
											</td>
<td>
												<span id='OrderDate'>" + Convert.ToDateTime(item.DispatchedDate).ToString("dd/MMM/yyyy") + @"</span>   
											</td>
<td>
												<span id='OrderDate'>Shipment ID:<a id='shipmentID' onclick=vpb_show_login_box_edit(" + item.PaymentTransactionId + ",'" + item.ShipmentId + "','" + item.PickupID + "')>" + item.ShipmentId + @"</a><br />
																	PickUp ID:<a id='pickUpID' onclick=vpb_show_login_box_edit(" + item.PaymentTransactionId + ",'" + item.ShipmentId + "','" + item.PickupID + "')>" + item.PickupID + @"</a><br />
																	 CourierName:" + item.CourierName + @"
</span>
												
											</td>

<td>
												
													<span id='lblTotalProducts'><span class='QuantityTipsySpan'>" + item.products.Count() + @"</span></span>
												
											</td><td align='right'>                                                
												<span id='lblTotalPrice'><span class='WebRupee'>" + item.CurrencySymbol + @" </span>" + item.TxnAmount + @"</span>
											</td>
<td style='display:none'>
												<span id='OrderDate'>" + item.FirstName + @"</span>
												
											</td>
<td style='display:none'>
												<span id='OrderDate'>" + item.MobileNo + @"</span>
												
											</td>
<td style='display:none'>
												<span id='OrderDate'>" + item.EmailId + @"</span>
												
											</td>
<td style='display:none'>
<span id='userAddress'>" + address.StreetAddress1 + ", " + address.StreetAddress2 + ", " + address.LandMark + ", " + address.City + ", " + address.StateName + ", " + address.PinCode + @"</span>
</td>
			</tr>");
            }
            StringBuilder strResult = new StringBuilder();
            strResult.Append(@"
		<div>                   
					
					<div>
							<div id='Main_dvGrid'>
								<div> 
								
								<div>
						
						  
						   
		<table class='activity_datatable' rules='all' id='ctl00_ctl00_Main_Main_grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
			<thead>
<tr class='checked'>
				<th class='cp_product_select' scope='col'>                                                
												<span title='Select/Unselect all orders'>
<div id='chkSelectAll' class='checker'><span>
<input style='opacity: 0;' id='ctl00_ctl00_Main_Main_grdShippingOrders_ctl01_chkSelectAll'  onclick='selectUnselectCheckboxes(this.id, 'chkBlkShip', 'lblTempOid');' type='checkbox'></span></div></span>
											</th><th scope='col'>ID</th><th scope='col'>Order Date</th><th scope='col'>Product</th><th scope='col'>Date</th><th scope='col'>Courier</th><th scope='col'>Qty</th><th scope='col'><div id='divCurrency'>Amount</div></th>

<th scope='col' style='display:none'>Customer</th>
<th scope='col' style='display:none'>Mobile</th>
<th scope='col' style='display:none'>Email</th>
<th scope='col' style='display:none'>Address</th>
			</tr></thead><tbody>
" + strtd + @"
</tbody></table>	</div></div></div></div></div></div></div></div>       

<div class='widget_body'>
			<div style='display: block;' class='action_bar text_right' id='dvbtn'>

			   
				<div class='clear'>
				</div>
			</div>
		</div>");


            //<input style='display: inline-block;'  value='Report Issue(s)' class='button_small greyishBtn fl_right' onclick='vpb_show_login_box();' type='button'>
            //<option value='10' selected='selected'>5</option>
            //<option value='5'>10</option>
            //<option  value='10'>15</option>
            //<option value='15'>20</option>
            //<option value='20'>25</option>


            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strResult.ToString()

            };
        }

        else
        {
            StringBuilder strResult = new StringBuilder();
            strResult.Append(@" <div id='ctl00_ctl00_Main_Main_divMessage' class='msgbar msg_Info hide_onC'>
								<span id='msgicon' class='iconsweet'>*</span>
								<p><span id='lblMessage'>There are no 'DeliveredOrders' found.</span></p>
							</div>");

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = strResult.ToString()
            };
        }
    }


    public CustomResponse AddNotifyMe(NoftifyMe objNotify)
    {
        if (objNotify.MobileNumber != 0 && objNotify.EmailId != "" && objNotify.UserName != "")
        {
            objNotify.CreatedOn = DateTime.Now;
            objNotify.UpDatedOn = DateTime.Now;
            objNotify.IsActive = true;
            var repository = new NotifyMeRepository();
            var data = new Product();
            repository.Insert(objNotify);
            var repositoryy = new ProductRepository();
            var dataa = repositoryy.First(p => p.ProductId == objNotify.ProductId);

            Utility.MailMessage ms = new Utility.MailMessage();
            ms.Subject = "Notification from HUW";
            ms.To = objNotify.EmailId;

            string domain;
            domain = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port;

            string body = string.Empty;
            string htmlPagePath = Shared.GethtmlPage(Shared.ProductStatusForSendMail.Notifyme);
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("" + htmlPagePath + "")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("##FirstName##", objNotify.UserName);
            body = body.Replace("##Product##", "<img height='100' src='" + dataa.ProductImgUrl.Replace("~/", ApiUrl) + "' alt='" + dataa.ProductName + "' />");
            body = body.Replace("##ProductName##", dataa.ProductName);
            body = body.Replace("##UnitPrice##", dataa.ProductCost.ToString());
            ms.Body = body;
            ms.IsBodyHtml = true;

            try
            {
                ms.SendMail();
            }
            catch (Exception ex)
            {

            }

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Message = CustomMessages.NotifySuccess.ToString()
            };
        }
        else
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = CustomMessages.NotifyFail.ToString()
            };
        }
    }


    public HttpResponseMessage Get()
    {

        var response = new HttpResponseMessage();

        var Coki = new CookieHeaderValue("session-Id", "123");

        Coki.Expires = DateTimeOffset.Now.AddDays(2);

        Coki.Domain = Request.RequestUri.Host;

        Coki.Path = "/";



        response.Headers.AddCookies(new CookieHeaderValue[] { Coki });

        return response;

    }

    public CustomResponse GetPaymentStatus(int rows, int value)
    {

        var repository = new PaymentTransactionRepository();
        int totalRecords;
        //        string input = "SUCCESS";
        //bool OutPut = System.Convert.ToBoolean(input);
        string status = "";
        if (value == 1)
        {
            status = "SUCCESS";
        }
        if (value == 2)
        {
            status = "Cancelled";
        }
        if (value == 3)
        {
            status = null;
        }
        var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, rows, (p => p.TxnStatus == status), null, "", true);
        //var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out  totalRecords, 0, rows, (p => p.Dispatched == false && p.Delivered == false && p.Pickup == false && p.Authorized == false), null, "", true);
        var filteredData = (from q in (data as List<PaymentTransaction>)
                            select new
                            {
                                q.User.FirstName,
                                q.User.LastName,
                                q.User.MobileNo,
                                q.User.EmailId,
                                q.User.UserId,
                                q.PaymentTransactionId,
                                Quantity = q.UserProductTransactions.Count(),
                                q.PaymentStatus,
                                q.TxnAmount,
                                q.TxnRefNo,
                                q.TxnStatus,
                                currency = q.CurrencyCode + " ( " + q.CurrencySymbol + " ) ",
                                q.CurrencyCode,
                                q.CurrencySymbol,
                                q.PGTxnId,
                                q.TxnMessage,
                                q.CreatedOn,
                                q.OrderCurrentStatus,
                                products = q.UserProductTransactions.Where(cat => cat.PaymentTransactionId == q.PaymentTransactionId).
                                  Select(u => string.Join(",", (u.Product.ProductName + (u.SubProduct == null ? "" : " - " + u.SubProduct.SPName)))).FirstOrDefault()

                            }).ToList();



        StringBuilder strtd = new StringBuilder();

        foreach (var item in filteredData)
        {
            decimal Currency = Convert.ToDecimal(item.TxnAmount);
            string Amount = Currency.ToString("0.00");
            DateTime dt = item.CreatedOn;
            string date = dt.ToString("MM/dd/yyyy");
            strtd.Append(@"<tr>
				<td>
												<div id='chkBlkShip' >
<input  id='chkkShip' name='chkBlkShip' value=" + item.PaymentTransactionId + @"  type='checkbox' class='Check' onchange='CheckboxChecked(this)' ></div>
												<div style='visibility: hidden;'>
													<span id='lblTempOid'>" + item.PaymentTransactionId + @"</span>
												</div>
												
												<input  id='hdnShippingMode' class='hdnShippingMode' value='self' type='hidden'>
											</td><td>
												<a id='hypEdit' onclick='NavigatetoOrderDetailsPage(" + item.PaymentTransactionId + ")'  >" + item.PaymentTransactionId + @"</a><br>
												<span id='lblPartialShip'></span>
											</td><td>
												<span id='OrderDate'>" + item.products + @"</span>
												
											</td><td>
												
													<span id='lblTotalProducts'><span class='QuantityTipsySpan'>1</span></span>
												
											</td><td align='right'>                                                
												<span id='lblTotalPrice'><span class='WebRupee'>" + item.CurrencySymbol + @" </span>" + Amount + @"</span>
											</td><td>  
 <span id='lblcustName'><span original-title='&lt;div class=&quot;addressdetails&quot;&gt;&lt;div class=&quot;p_nm&quot;&gt;" + item.FirstName + @"&lt;/div&gt;&lt;p&gt;" + item.MobileNo + @"&lt;/br&gt;Mumbai&lt;/br&gt;" + item.EmailId + @"&lt;/br&gt;Maharashtra&lt;/br&gt;" + item.UserId + @"&lt;/p&gt;&lt;' class='QuantityTipsySpan'>" + item.FirstName + @"</span></span>                                              
											</td>
<td>                                                
<span id='lblTempOid'>" + item.TxnStatus + @"</span>
</td>
<td>                                                
<span id='lblDate'>" + date + @"</span>
</td>
</tr>");
        }
        StringBuilder strResult = new StringBuilder();
        strResult.Append(@"
		<div class='widget'>                   
					
					<div class='widget_body'>
							<div id='Main_dvGrid' class='tab-grdpanel'>
								<div class='widget marg-0'> 
							  
								<div class='widget_body'>
						
							<div class='cp_productlist_view'>
								<div class='products_views'>
									
								</div>
							</div>
						   
		<table class='activity_datatable' rules='all' id='ctl00_ctl00_Main_Main_grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
			<thead>
<tr>
				<th class='cp_product_select' scope='col'>                                                
												<span title='Select/Unselect all orders'>
<div id='chkSelectAll' class='checker'><span>
<input style='opacity: 0;' id='ctl00_ctl00_Main_Main_grdShippingOrders_ctl01_chkSelectAll'  onclick='selectUnselectCheckboxes(this.id, 'chkBlkShip', 'lblTempOid');' type='checkbox'></span></div></span>
											</th>
<th scope='col'>Order ID</th>
<th scope='col'>Product</th>
<th scope='col'>Qty</th>
<th scope='col'><div id='divCurrency'>Order Value</div></th>
<th scope='col'>Customer Name</th>
<th scope='col'>Status</th>
<th scope='col'>Order Date</th>
			</tr></thead><tbody>
" + strtd + @"
</tbody></table>	
</div></div></div>
</div></div></div>
</div></div>");




        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Result = strResult.ToString()

        };
    }
    [AllowAnonymous]
    [HttpPost]
    public CustomResponse Remiting_Customers_ajax(object[][] arr)
    {
        var data = arr;
        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Result = ""
        };
    }
    [HttpGet]
    public CustomResponse Autocompleteforproductname(string Prefix)
    {
        db_Zon_HuwEntities context = new db_Zon_HuwEntities();
        var playername = (from N in context.Products
                          where N.ProductName.Contains(Prefix)
                          select N.ProductName);
        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Result = playername
        };
        //return Json(patientnames, JsonRequestBehavior.AllowGet);

    }

    #region GetFeaturesCategoryList Info

    public dynamic GetFeaturesCategoryList(string sidx, string sord, int page, int rows)
    {
        var repository = new FeaturesCategoryRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.BusinessId, out totalRecords, (page - 1) * rows, rows, null, null, "BusinessType");
        foreach (var FeaturesCategory in data)
            FeaturesCategory.BusinessName = FeaturesCategory.BusinessType.BusinessName;

        return new
        {
            total = totalRecords / rows + 1,
            page,
            records = totalRecords,
            rows = data
        };
    }

    public string GetFeaturesCategoryListAsHtml()
    {

        var repository = new FeaturesCategoryRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.FeaturesCategoryId, out totalRecords, 0, 0, null, c => new { c.FeaturesCategoryId, c.FeaturesCategoryName });

        string customersHtml = data.Aggregate(" <select>", (current, item) => current + string.Format("<option value={0}>{1}</option>", item.BusinessId, item.FeaturesCategoryName));
        customersHtml += "</select>";
        return customersHtml;
    }


    public List<BusinessType> GetFeaturesCategoryList()
    {
        var repository = new BusinessTypeRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.BusinessId, out totalRecords, 0, 0, null, null, "FeaturesCategory");
        return data;
    }

    public List<FeaturesCategory> GetFeaturesCategoryListByBusinessType(int id)
    {
        var repository = new FeaturesCategoryRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.FeaturesCategoryId, out totalRecords, 0, 0, null, null, "BusinessType");

        foreach (var FeaturesCategory in data)
            FeaturesCategory.BusinessName = FeaturesCategory.BusinessType.BusinessName;

        return data;
    }

    [HttpPost]
    public string CreateFeaturesCategory(FeaturesCategory data)
    {
        try
        {
            var repository = new FeaturesCategoryRepository();
            repository.Insert(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpPost]
    public string EditFeaturesCategory(FeaturesCategory data)
    {
        try
        {
            var repository = new FeaturesCategoryRepository();
            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpGet]
    public string DeleteFeaturesCategory(int id, string name)
    {
        try
        {
            var repository = new FeaturesCategoryRepository();
            long FeaturesCategoryId = Convert.ToInt32(name);
            var data = repository.First(p => p.FeaturesCategoryId == FeaturesCategoryId);
            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }


    #endregion

    #region Features  Sub Category Info

    public dynamic GetFeaturesSubCategoryList(string sidx, string sord, int page, int rows)
    {
        var repository = new FeaturesSubCategoryRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.FeaturesCategoryId, out totalRecords, (page - 1) * rows, rows, null, null, "FeaturesCategory");
        foreach (var FeaturesSubCategory in data)
            FeaturesSubCategory.FeaturesCategoryName = FeaturesSubCategory.FeaturesCategory.FeaturesCategoryName;

        return new
        {
            total = totalRecords / rows + 1,
            page,
            records = totalRecords,
            rows = data
        };
    }

    public List<FeaturesCategory> GetFeaturesSubCategoryList()
    {
        var repository = new FeaturesCategoryRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.FeaturesCategoryId, out totalRecords, 0, 0, null, null, "FeaturesSubCategory");
        return data;
    }

    public CustomResponse GetFeaturesSubCategoryListByFeaturesCategory(int id)
    {
        try
        {
            //Top Menu From DataBase
            StringBuilder strResult = new StringBuilder();
            var repository = new FeaturesCategoryRepository();
            IEnumerable<Object> FeaturesSubCatByGroup = repository.GetFeaturesSubCatByFeaturesCatByGroup(id);

            var CatList = (from a in FeaturesSubCatByGroup
                           select new
                           {
                               FeaturesCategoryId = a.GetType().GetProperty("FeaturesCategoryId").GetValue(a, null),
                               FeaturesCategoryName = a.GetType().GetProperty("FeaturesCategoryName").GetValue(a, null),
                               FeaturesSubCategories = a.GetType().GetProperty("FeaturesSubCategories").GetValue(a, null),
                           }).ToList();

            foreach (var Sitem in CatList)
            {
                strResult.Append(@"<ul class='side_sub_menu'>");
                strResult.Append(Sitem.FeaturesCategoryName);


                foreach (var item in (IEnumerable<Object>)Sitem.FeaturesSubCategories)
                {


                    strResult.Append(@"<li><a href='#' ><input type='checkbox' name='checkboxlist' fieldvalue='" +
                         item.GetType().GetProperty("FeaturesSubCategoryId").GetValue(item, null) +
                         "'  fieldtext='" + item.GetType().GetProperty("FeaturesSubCategoryName").GetValue(item, null) + "'   onclick='GetProducts()'/>   " +
                         item.GetType().GetProperty("FeaturesSubCategoryName").GetValue(item, null) +
                                     "(" + item.GetType().GetProperty("Count").GetValue(item, null) + ")</a></li>");



                    //strResult.Append(@"<li><a href='#' onclick=SearchData(this," + Sitem.FeaturesCategoryId + "," +
                    //    item.GetType().GetProperty("FeaturesSubCategoryId").GetValue(item, null) + ")>" +
                    //    item.GetType().GetProperty("FeaturesSubCategoryName").GetValue(item, null) +
                    //    "(" + item.GetType().GetProperty("Count").GetValue(item, null) + ")</a></li>");
                }

                strResult.Append(@"</ul>");
            }
            return new CustomResponse
            {
                Status = string.IsNullOrWhiteSpace(strResult.ToString()) ? Shared.ResponseStatus.NoData.ToString() : Shared.ResponseStatus.Success.ToString(),
                Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
                Result = strResult.ToString()
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }


    }

    [HttpPost]
    public string CreateFeaturesSubCategory(FeaturesSubCategory data)
    {
        try
        {
            var repository = new FeaturesSubCategoryRepository();
            repository.Insert(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpPost]
    public string EditFeaturesSubCategory(FeaturesSubCategory data)
    {
        try
        {
            var repository = new FeaturesSubCategoryRepository();
            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpGet]
    public string DeleteFeaturesSubCategory(int id, string name)
    {
        try
        {
            var repository = new FeaturesSubCategoryRepository();
            long FeaturesSubCategoryId = Convert.ToInt32(name);
            var data = repository.First(p => p.FeaturesSubCategoryId == FeaturesSubCategoryId);
            repository.Update(data);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    #endregion


    #region Related Products
    [HttpGet]
    public CustomResponse SearchRelatedProductList(string sord)
    {
        try
        {
            var repository = new ProductRepository();
            int totalRecords;

            var data = repository.FetchAllByPage(p => p.ProductId, out totalRecords, 0, 0,
                                                        (p => p.IsSold == false &&
                                                         p.IsActive && p.IsDeleted == false && (p.ProductCode == sord || p.ProductName.Contains(sord))), null, "").Distinct();

            StringBuilder strResult = new StringBuilder();
            strResult.Append(@"<table>");
            foreach (var item in data)
            {
                strResult.Append(@"<tr>");
                strResult.Append(@"<td> 
<input  type='checkbox' name='checkboxlist' value=" + item.ProductId + @"  fieldvalue='" +
                         item.ProductId + "' class='CheckRltd' onclick=' addToRelatedProduct(" + item.ProductId + ")'/> </td>");
                strResult.Append(@"<td>" + item.ProductName + "</td>");
                strResult.Append(@"</tr>");
            }
            strResult.Append(@"</table>");

            return new CustomResponse
            {
                Status = string.IsNullOrWhiteSpace(strResult.ToString()) ? Shared.ResponseStatus.NoData.ToString() : Shared.ResponseStatus.Success.ToString(),
                Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
                Result = strResult.ToString()
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }
    }
    #endregion
    #region Free Products
    [HttpGet]
    public CustomResponse SearchFreeProductList(string sord)
    {
        try
        {
            var repository = new ProductRepository();
            int totalRecords;

            var data = repository.FetchAllByPage(p => p.ProductId, out totalRecords, 0, 0,
                                                        (p => p.IsSold == false &&
                                                         p.IsActive && p.IsDeleted == false && (p.ProductCode == sord || p.ProductName.Contains(sord))), null, "").Distinct();

            StringBuilder strResult = new StringBuilder();
            strResult.Append(@"<table>");
            foreach (var item in data)
            {
                strResult.Append(@"<tr>");
                strResult.Append(@"<td> 
<input  type='radio' name='radiolist' value=" + item.ProductId + @"  fieldvalue='" +
                         item.ProductId + "' class='CheckRltd' onclick=' addToFreeProduct(" + item.ProductId + ")'/> </td>");
                strResult.Append(@"<td>" + item.ProductName + "</td>");
                strResult.Append(@"</tr>");
            }
            strResult.Append(@"</table>");

            return new CustomResponse
            {
                Status = string.IsNullOrWhiteSpace(strResult.ToString()) ? Shared.ResponseStatus.NoData.ToString() : Shared.ResponseStatus.Success.ToString(),
                Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
                Result = strResult.ToString()
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }
    }


    [HttpGet]
    public CustomResponse SearchAllProductList(string sord)
    {
        try
        {
            var repository = new ProductRepository();
            int totalRecords;

            var data = repository.FetchAllByPage(p => p.ProductId, out totalRecords, 0, 0,
                                                        (p => p.IsSold == false &&
                                                         p.IsActive && p.IsDeleted == false && (p.ProductCode == sord || p.ProductName.Contains(sord))), null, "").Distinct();

            StringBuilder strResult = new StringBuilder();
            strResult.Append(@"<ul>");
            foreach (var item in data)
            {
                strResult.Append(@"<li><a onclick=ProductAutocompletess('" + item.ProductName.Replace(" ", ",") + "')>");
                strResult.Append(item.ProductName);
                strResult.Append(@"</a></li>");
            }
            strResult.Append(@"</ul>");

            return new CustomResponse
            {
                Status = string.IsNullOrWhiteSpace(strResult.ToString()) ? Shared.ResponseStatus.NoData.ToString() : Shared.ResponseStatus.Success.ToString(),
                Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
                Result = strResult.ToString()
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }
    }

    [HttpGet]
    public CustomResponse SearchAllBrandList(string sord)
    {
        try
        {

            db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();

            var data = (from Brandlist in Entity.Products
                        where Brandlist.Brand.Contains(sord)
                        select new
                        {
                            Brand = Brandlist.Brand
                        })
              .ToList()
              .Distinct();

            StringBuilder strResult = new StringBuilder();
            strResult.Append(@"<ul>");
            foreach (var item in data)
            {
                strResult.Append(@"<li><a onclick=BrandAutocomplete('" + item.Brand.Replace(" ", ",") + "')>");
                strResult.Append(item.Brand);
                strResult.Append(@"</a></li>");
            }
            strResult.Append(@"</ul>");

            return new CustomResponse
            {
                Status = string.IsNullOrWhiteSpace(strResult.ToString()) ? Shared.ResponseStatus.NoData.ToString() : Shared.ResponseStatus.Success.ToString(),
                Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
                Result = strResult.ToString()
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }
    }


    [HttpGet]
    public CustomResponse GetSearchorders(int? rows, long? PaymentTransactionId, DateTime? CreatedOn, DateTime? UpdatedOn, int? OrderCurrentStatus, string PaymentMode, int? PaymentStatus, string ShipmentId, string CourierName/*, PaymentTransaction objtrn*/)
    {
        var repository = new PaymentTransactionRepository();
        //int totalRecords;

        db_Zon_HuwEntities Context = new db_Zon_HuwEntities();

        List<PaymentTransaction> data = new List<PaymentTransaction>();
        DateTime UpdatedDate = Convert.ToDateTime("1/1/0001 12:00:00 AM");
        DateTime CreatedDate = Convert.ToDateTime("1/1/0001 12:00:00 AM");
        if (UpdatedOn != null || UpdatedOn != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
        {
            UpdatedOn = UpdatedOn.Value.AddDays(1);
        }
        if (!string.IsNullOrEmpty(PaymentTransactionId.ToString()) || OrderCurrentStatus != -2 || CreatedOn != CreatedDate || !string.IsNullOrEmpty(PaymentMode) || PaymentStatus != -1 || CourierName != "Select" || !string.IsNullOrEmpty(ShipmentId))
        {
            data = (from p in Context.PaymentTransactions
                    where (PaymentTransactionId == null ? true : (p.PaymentTransactionId == PaymentTransactionId))
                    && (PaymentMode == null ? true : (p.PaymentMode == PaymentMode))
                    && (PaymentStatus == -1 ? true : (PaymentStatus == 0 ? (p.OrderCurrentStatus != 0 && p.OrderCurrentStatus != 1) : (p.OrderCurrentStatus == 0 || p.OrderCurrentStatus == 1 /*|| p.OrderCurrentStatus == null*/)))
                    && (OrderCurrentStatus == -2 ? true : (p.OrderCurrentStatus == OrderCurrentStatus))
                    && (CreatedOn == CreatedDate ? true : (p.CreatedOn >= CreatedOn && p.CreatedOn <= UpdatedOn))
                    && (ShipmentId == null ? true : (p.ShipmentId.Contains(ShipmentId)))
                    && (CourierName == "Select" ? true : (p.CourierName == CourierName))
                    select p).OrderByDescending(x => x.PaymentTransactionId).Take(rows.Value).ToList();
            //data = Context.PaymentTransactions.Where(x=>CreatedOn == CreatedDate ? true : (x.CreatedOn >= CreatedOn && x.CreatedOn <= UpdatedOn)).ToList();
            //(PaymentTransactionId == 0 ? true : (p.PaymentTransactionId == PaymentTransactionId)) &&
        }
        else
        {
            data = (from p in Context.PaymentTransactions
                    select p).OrderByDescending(x => x.PaymentTransactionId).Take(rows.Value).ToList();
        }

        StringBuilder strtd = new StringBuilder();
        StringBuilder strResult = new StringBuilder();
        if (data.Count > 0)
        {
            foreach (var item in data)
            {
                try
                {
                    db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();
                    string statusvalue = item.OrderCurrentStatus.ToString();
                    var status = Shared.GetOrderStatusEnum(statusvalue);

                    StringBuilder ProductName = new StringBuilder();

                    for (var i = 0; i < item.UserProductTransactions.Count; i++)
                    {
                        if (i == 0)
                        {
                            ProductName.Append("<a target='_blank' title='Remaining Quantity: " + item.UserProductTransactions.ElementAtOrDefault(i).Product.Quantity + "' style='color:blue;' href='/Admin/UpdateProducts.aspx?ID=" + item.UserProductTransactions.ElementAtOrDefault(i).Product.ProductId + "'>" + item.UserProductTransactions.ElementAtOrDefault(i).Product.ProductName + "</a>");
                        }
                        else
                        {
                            ProductName.Append(", " + "<a target='_blank' title='Remaining Quantity: " + item.UserProductTransactions.ElementAtOrDefault(i).Product.Quantity + "' style='color:blue;' href='/Admin/UpdateProducts.aspx?ID=" + item.UserProductTransactions.ElementAtOrDefault(i).Product.ProductId + "'>" + item.UserProductTransactions.ElementAtOrDefault(i).Product.ProductName + "</a>");
                        }
                    }

                    string Payment = "";
                    if (item.PaymentMode == "Cash On Delivery")
                    {
                        Payment = "Postpaid";
                    }
                    else if (item.PaymentMode == "Paytm")
                    {
                        Payment = "Paytm";
                    }
                    else if (item.PaymentMode == "Payumoney")
                    {
                        Payment = "PayU money";
                    }
                    else
                    {
                        Payment = "Wallet";
                    }

                    strtd.Append(@"<tr><td>
												<a id='hypEdit'  onclick='NavigatetoOrderDetailsPage(" + item.PaymentTransactionId + ")' >" + item.PaymentTransactionId + @"</a><br>
												<span id='lblPartialShip'></span>
											</td><td>
												<span >" + item.CreatedOn.ToString("dd/MMM/yyyy") + @"</span>   
											</td>
												<td>
												<span >" + ProductName + @"</span>   
											</td>
<td>                                                
													<span id='lblTotalProducts'>" + item.UserProductTransactions.Count + @"</span>                                                
											</td>
<td>                                                
													<span id='lblTotalProducts'>" + item.User.FirstName + @"</span>                                                
											</td>
<td>                                                
													<span id='lblTotalProducts'>" + Payment + @"</span>                                                
											</td>
											 <td>                                                
													<span id='lblstatus'>" + status + @"</span>                                                
											</td><td>                                                
												<span id='lblcustName'><span>" + item.CurrencySymbol + @" </span>" + String.Format("{0:0.00}", item.TxnAmount) + @"</span></span>
											</td>
<td style='display:none'>" + item.User.FirstName + @"</td>
<td style='display:none'>" + item.User.MobileNo + @"</td>
<td style='display:none'>" + item.UserProductTransactions.FirstOrDefault().UserAddress.StreetAddress1 + " ," + item.UserProductTransactions.FirstOrDefault().UserAddress.StreetAddress2 + " ," + item.UserProductTransactions.FirstOrDefault().UserAddress.LandMark + " ," + item.UserProductTransactions.FirstOrDefault().UserAddress.City + " ," + item.UserProductTransactions.FirstOrDefault().UserAddress.PinCode + @"</td>
			</tr>");
                }
                catch (Exception ex)
                {

                }
            }

            strResult.Append(@"
		<div class='widget'>                   
					
					<div class='widget_body'>
							<div id='Main_dvGrid'>
								<div class='widget marg-0'> 
								<div class='widget_title'>
									<span class='iconsweet'>r</span><h5>Available Orders</h5>
								</div>
								<div class='widget_body'>
						
							<div class='cp_productlist_view'>
								<div class='products_views'>
									
								</div>
							</div>
						   
										<table class='activity_datatable' rules='all' id='grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
											<thead>
								<tr>
												<th scope='col'>ID</th><th scope='col'>Date</th><th scope='col'>Product Name</th><th scope='col'>Qty</th><th scope='col'>Customer Name</th><th scope='col'>Payment Mode</th><th scope='col'>Status</th><th scope='col'>Amount</th><th scope='col' style='display:none'>Name</th><th scope='col' style='display:none'>Phone</th><th style='display:none'>Address</th>
											</tr></thead><tbody>
								" + strtd + @"
								</tbody></table>	</div>

  
									</div>
							</div>
							
							
					</div>
				</div>
				<input name='ctl00$ctl00$Main$Main$hdnTodoSearchcriteria' id='ctl00_ctl00_Main_Main_hdnTodoSearchcriteria' value='[&quot;Order No&quot;,&quot;SKU&quot;,&quot;Email Id&quot;,&quot;Phone No&quot;,&quot;Coupon Code&quot;,&quot;Checkout Type&quot;]' type='hidden'>
				<img id='img441' alt='' onload='getdivCurrency();' src='Orders_files/designoption_009_13.jpg' height='1' width='1'>
			
</div>      

			</div>
		</div>");

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strResult.ToString(),
                CartSum = data.Count().ToString()
            };
        }
        else
        {
            strResult.Append(@" <div id='divMessage' class='msgbar msg_Info hide_onC'>
								<span id='msgicon' class='iconsweet'>*</span>
								<p><span id='lblMessage'>There are no 'Orders' found.</span></p>
							</div>");

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = strResult.ToString(),
                CartSum = data.Count().ToString()
            };
        }

    }
    [HttpPost]
    public CustomResponse GetUsers(UserInfo Userobj)
    {
        try
        {
            db_Zon_HuwEntities Context = new db_Zon_HuwEntities();
            StringBuilder strtd = new StringBuilder();
            StringBuilder strResult = new StringBuilder();
            List<UserInfo> result = new List<UserInfo>();

            if (!string.IsNullOrEmpty(Userobj.Name) || !string.IsNullOrEmpty(Userobj.Email) || Userobj.Mobile != 0 || Userobj.OrderId != 0)
            {
                result = (from a in Context.Users
                          join b in Context.UserAddresses on a.UserId equals b.UserId into address
                          join c in Context.PaymentTransactions on a.UserId equals c.UserId into payment
                          from d in payment.DefaultIfEmpty()
                          from e in address.DefaultIfEmpty()
                          where (string.IsNullOrEmpty(Userobj.Name) ? true : (a.FirstName.Contains(Userobj.Name)))
                          && (string.IsNullOrEmpty(Userobj.Email) ? true : (a.EmailId.Contains(Userobj.Email)))
                          && (Userobj.Mobile == 0 ? true : (a.MobileNo == Userobj.Mobile))
                          && (Userobj.OrderId == 0 ? true : (d.PaymentTransactionId == Userobj.OrderId))



                          select new UserInfo
                          {
                              UserId = a.UserId,
                              Name = a.FirstName,
                              Email = a.EmailId,
                              Password = a.PassCode,
                              Mobile = a.MobileNo,
                              Address = e.StreetAddress1,
                              Status = a.RegStatus,
                              OrdersPalced = a.PaymentTransactions.Where(x => x.UserId == a.UserId).Count(),
                              SuccessOrders = a.PaymentTransactions.Where(x => x.UserId == a.UserId && x.TxnStatus == "SUCCESS").Count(),
                          }).Distinct().OrderByDescending(x => x.UserId).ToList();
            }
            else
            {
                result = (from a in Context.Users
                          join b in Context.UserAddresses on a.UserId equals b.UserId into address
                          join c in Context.PaymentTransactions on a.UserId equals c.UserId into payment
                          from d in payment.DefaultIfEmpty()
                          from e in address.DefaultIfEmpty()
                          select new UserInfo
                          {
                              UserId = a.UserId,
                              Name = a.FirstName,
                              Email = a.EmailId,
                              Password = a.PassCode,
                              Mobile = a.MobileNo,
                              AlternateContactNumber = a.AlternateContactNumber,
                              Address = e.StreetAddress1,
                              Status = a.RegStatus,
                              OrdersPalced = a.PaymentTransactions.Where(x => x.UserId == a.UserId).Count(),
                              SuccessOrders = a.PaymentTransactions.Where(x => x.UserId == a.UserId && x.TxnStatus == "SUCCESS").Count(),
                          }).Distinct().OrderByDescending(x => x.UserId).ToList();

            }
            if (result.Count > 0)
            {
                int Sno = 0;
                foreach (var items in result.GroupBy(x => x.UserId).Select(y => y.First()).Take(100))
                {
                    string EmailId = items.Email;
                    //if (EmailId.Length > 20)
                    //{
                    //    EmailId = items.Email.Substring(0, 20) + " ...";
                    //}

                    Sno++;
                    strtd.Append(@"<tr id='use_" + items.UserId + @"'>
<td>" + Sno + @"</td>
<td>
<a id='lblName' data-toggle='modal' data-target='#myModal' onClick='UserOrderDetailsPage(" + items.UserId + ")'> " + items.Name + @" </a><br>
				 </td>
				  <td title=" + items.Email + @">
					<span>" + EmailId + @"</span >   
					 </td>
					  <td>                                                
					<span id='lblPassword'>" + items.Password + @"</span >                                                
					 </td>
						<td>                                                
						 <span id='lblMobile'>" + items.Mobile + "  " + items.AlternateContactNumber + @"</span >                                                
					   </td><td>                                                
						 <span id='lblOrders'>" + items.OrdersPalced + @"</span >
					   </td>
                    <td>                                                
						 <span id='lblSuccess'>" + items.SuccessOrders + @"</span >                                                
					   </td>
 <td>                                                
                         <a id='' href='../../Admin/EditUser.aspx?Id=" + items.UserId + @"' class='btn btn-danger'>Edit</span >                                                
                       </td>
			</tr>");
                }
            }
            strResult.Append(@"
				 <div class='widget'>                   
					<div class='widget_body'>
					   <div id='Main_dvGrid'>
						  <div class='widget marg-0'> 
							  <div class='widget_title'>
									<!--<span class='iconsweet'>r</span>--><h5>Users Info</h5>
							  </div>
							<div class='widget_body'>
							  <table id='tblContact' class='table table-striped table-bordered' cellspacing='0' style='width:100%;'>
							   <thead>
								<tr>
								<th scope='col'>S.No</th>
                                <th scope='col'>Name</th>
                                <th scope='col'>Email</th>
                                <th scope='col'>Password</th>
                                <th scope='col'>Mobile</th>
                                <th scope='col' >Orders Placed</th>
                                <th scope='col'>Successful Orders</th>
<th scope='col'>Action</th>
								</tr>
								</thead>
									<tbody>
								" + strtd + @"
								</tbody>
								</table>
								 </div>
							   </div>
							</div>
					</div>
				</div>              
				</div>      
			</div>
		</div>
	  </div>");

            return new CustomResponse
            {
                Result = strResult.ToString(),
                Status = "Success"
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [HttpPost]
    public CustomResponse GetUserOrdesInfo(UserInfo Userobj)
    {
        try
        {

            db_Zon_HuwEntities context = new db_Zon_HuwEntities();
            var result = (from a in context.PaymentTransactions
                          join b in context.UserProductTransactions on a.PaymentTransactionId equals b.PaymentTransactionId
                          join c in context.Products on b.ProductId equals c.ProductId
                          where a.UserId == Userobj.UserId
                          select new
                          {
                              OrderId = a.PaymentTransactionId,
                              OrderDate = a.CreatedOn,
                              ProductName = c.ProductName,
                              TransactionAmount = a.TxnAmount,
                              TransactionStatu = a.TxnStatus
                          }).ToList();
            StringBuilder strtd = new StringBuilder();
            StringBuilder str = new StringBuilder();
            if (result.Count > 0)
            {
                foreach (var items in result)
                {

                    strtd.Append(@"
<tr class='revs'>
<td>
				<a id='hypEdit' onclick='SingleOrderPage(" + items.OrderId + ")' >" + items.OrderId + @"</a><br>
				<span id='lblUserId'></span>
				</td><td>
				<span id='lblOrderDate'>" + items.OrderDate.ToShortDateString() + @"</span>   
				 </td>
				  <td>
					<span id='lblProductName'>" + items.ProductName + @"</span>   
					 </td>
					  <td>                                                
					<span id='lblTransactionAmount'>" + String.Format("{0:0.00}", items.TransactionAmount) + @"</span>                                                
					 </td>
						<td>                                                
						 <span id='lblTransactionStatu'>" + items.TransactionStatu + @"</span>                                                
					   </td>
			</tr>
");
                }
                str.Append(@"
 <div class='x_content'>
				<div class='pagnation'>

<table  class='activity_datatable' rules='all' id='grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='5' cellspacing='0'>
							   <thead>
								<tr>
								 <th scope='col'>OrderId</th><th scope='col'>OrderDate</th><th scope='col'>ProductName</th><th scope='col'>TransactionAmount</th><th scope='col'>TransactionStatu</th> </tr>
								</thead>
									<tbody>

								" + strtd + @"

								</tbody>
								</table>

</div>
</div>
");
            }
            return new CustomResponse
            {
                Result = str.ToString()
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [HttpPost]
    public CustomResponse GetNextOrders(PaymentTransaction objtrn)
    {
        var repository = new PaymentTransactionRepository();
        int totalRecords;
        int skipRecords = Convert.ToInt32(objtrn.ServiceTax.Value);
        db_Zon_HuwEntities Context = new db_Zon_HuwEntities();

        List<PaymentTransaction> data = new List<PaymentTransaction>();
        DateTime UpdatedOn = Convert.ToDateTime("1/1/0001 12:00:00 AM");
        DateTime CreatedDate = Convert.ToDateTime("1/1/0001 12:00:00 AM");
        if (objtrn.UpdatedOn != null || objtrn.UpdatedOn != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
        {
            UpdatedOn = objtrn.UpdatedOn.Value.AddDays(1);
        }
        if (objtrn.PaymentTransactionId != 0 || objtrn.OrderCurrentStatus != -2 || objtrn.CreatedOn != CreatedDate || objtrn.PaymentMode != "" || objtrn.PaymentStatus != -1)
        {
            data = (from p in Context.PaymentTransactions
                    where (objtrn.PaymentTransactionId == 0 ? true : (p.PaymentTransactionId == objtrn.PaymentTransactionId))
                    && (objtrn.PaymentMode == "" ? true : (p.PaymentMode == objtrn.PaymentMode))
                    && (objtrn.PaymentStatus == -1 ? true : (objtrn.PaymentStatus == 0 ? (p.OrderCurrentStatus != 0 && p.OrderCurrentStatus != 1) : (p.OrderCurrentStatus == 0 || p.OrderCurrentStatus == 1 || p.OrderCurrentStatus == null)))
                    && (objtrn.OrderCurrentStatus == -2 ? true : (p.OrderCurrentStatus == objtrn.OrderCurrentStatus))
                    && (objtrn.CreatedOn == CreatedDate ? true : (p.CreatedOn >= objtrn.CreatedOn && p.CreatedOn <= UpdatedOn))
                    select p).OrderByDescending(x => x.PaymentTransactionId).Skip(skipRecords).Take(objtrn.rows).ToList();
        }
        else
        {
            data = (from p in Context.PaymentTransactions
                    select p).OrderByDescending(x => x.PaymentTransactionId).Skip(skipRecords).Take(objtrn.rows).ToList();
        }

        StringBuilder strtd = new StringBuilder();
        StringBuilder strResult = new StringBuilder();
        if (data.Count > 0)
        {
            foreach (var item in data)
            {
                try
                {
                    db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();
                    string statusvalue = item.OrderCurrentStatus.ToString();
                    var status = Shared.GetOrderStatusEnum(statusvalue);

                    StringBuilder ProductName = new StringBuilder();

                    for (var i = 0; i < item.UserProductTransactions.Count; i++)
                    {
                        if (i == 0)
                        {
                            ProductName.Append("<a target='_blank' title='Remaining Quantity: " + item.UserProductTransactions.ElementAtOrDefault(i).Product.Quantity + "' style='color:blue;' href='/Admin/UpdateProducts.aspx?ID=" + item.UserProductTransactions.ElementAtOrDefault(i).Product.ProductId + "'>" + item.UserProductTransactions.ElementAtOrDefault(i).Product.ProductName + "</a>");
                        }
                        else
                        {
                            ProductName.Append(", " + "<a target='_blank' title='Remaining Quantity: " + item.UserProductTransactions.ElementAtOrDefault(i).Product.Quantity + "' style='color:blue;' href='/Admin/UpdateProducts.aspx?ID=" + item.UserProductTransactions.ElementAtOrDefault(i).Product.ProductId + "'>" + item.UserProductTransactions.ElementAtOrDefault(i).Product.ProductName + "</a>");
                        }
                    }

                    string PaymentMode = "";
                    if (item.PaymentMode == "Cash On Delivery")
                    {
                        PaymentMode = "Postpaid";
                    }
                    else
                    {
                        PaymentMode = "Prepaid";
                    }

                    strtd.Append(@"<tr><td>
												<a id='hypEdit'  onclick='NavigatetoOrderDetailsPage(" + item.PaymentTransactionId + ")' >" + item.PaymentTransactionId + @"</a><br>
												<span id='lblPartialShip'></span>
											</td><td>
												<span >" + item.CreatedOn.ToShortDateString() + @"</span>   
											</td>
												<td>
												<span >" + ProductName + @"</span>   
											</td>
<td>                                                
													<span id='lblTotalProducts'>" + item.UserProductTransactions.Count + @"</span>                                                
											</td>
<td>                                                
													<span id='lblTotalProducts'>" + item.User.FirstName + @"</span>                                                
											</td>
<td>                                                
													<span id='lblTotalProducts'>" + PaymentMode + @"</span>                                                
											</td>
											 <td>                                                
													<span id='lblstatus'>" + status + @"</span>                                                
											</td><td>                                                
												<span id='lblcustName'><span>" + item.CurrencySymbol + @" </span>" + String.Format("{0:0.00}", item.TxnAmount) + @"</span></span>
											</td>
			</tr>");
                }
                catch (Exception ex)
                {

                }
            }

            strResult.Append(@" <table class='activity_datatable' rules='all' id='grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'><tbody>" + strtd + @"</tbody></table>");

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strResult.ToString(),
                CartSum = (skipRecords + data.Count()).ToString()
            };
        }
        else
        {
            strResult.Append(@" <div id='divMessage' class='msgbar msg_Info hide_onC'>
								<span id='msgicon' class='iconsweet'>*</span>
								<p><span id='lblMessage'>There are no 'Orders' found.</span></p>
							</div>");

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = strResult.ToString(),
                CartSum = (skipRecords + data.Count()).ToString()
            };
        }
    }
    [HttpPost]
    public CustomResponse GetSearchordersbyMobile(decimal Mobile)
    {

        var repository = new PaymentTransactionRepository();
        int totalRecords;

        db_Zon_HuwEntities Context = new db_Zon_HuwEntities();

        List<PaymentTransaction> data = new List<PaymentTransaction>();
        var UID = (from a in Context.Users
                   where a.MobileNo == Mobile
                   select a.UserId).FirstOrDefault();

        StringBuilder strtd = new StringBuilder();
        StringBuilder strResult = new StringBuilder();
        data = (from p in Context.PaymentTransactions
                where p.UserId == UID
                select p).OrderByDescending(x => x.PaymentTransactionId).ToList();
        if (data.Count > 0)
        {
            foreach (var item in data)
            {
                try
                {
                    db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();
                    string statusvalue = item.OrderCurrentStatus.ToString();
                    var status = Shared.GetOrderStatusEnum(statusvalue);

                    StringBuilder ProductName = new StringBuilder();

                    for (var i = 0; i < item.UserProductTransactions.Count; i++)
                    {
                        if (i == 0)
                        {
                            ProductName.Append("<a target='_blank' title='Remaining Quantity: " + item.UserProductTransactions.ElementAtOrDefault(i).Product.Quantity + "' style='color:blue;' href='/Admin/UpdateProducts.aspx?ID=" + item.UserProductTransactions.ElementAtOrDefault(i).Product.ProductId + "'>" + item.UserProductTransactions.ElementAtOrDefault(i).Product.ProductName + "</a>");
                        }
                        else
                        {
                            ProductName.Append(", " + "<a target='_blank' title='Remaining Quantity: " + item.UserProductTransactions.ElementAtOrDefault(i).Product.Quantity + "' style='color:blue;' href='/Admin/UpdateProducts.aspx?ID=" + item.UserProductTransactions.ElementAtOrDefault(i).Product.ProductId + "'>" + item.UserProductTransactions.ElementAtOrDefault(i).Product.ProductName + "</a>");
                        }
                    }

                    string PaymentMode = "";
                    if (item.PaymentMode == "Cash On Delivery")
                    {
                        PaymentMode = "Postpaid";
                    }
                    else if (item.PaymentMode == "Paytm")
                    {
                        PaymentMode = "Paytm";
                    }
                    else if (item.PaymentMode == "Payumoney")
                    {
                        PaymentMode = "PayU money";
                    }
                    else
                    {
                        PaymentMode = "Wallet";
                    }

                    strtd.Append(@"<tr><td>
												<a id='hypEdit'  onclick='NavigatetoOrderDetailsPage(" + item.PaymentTransactionId + ")' >" + item.PaymentTransactionId + @"</a><br>
												<span id='lblPartialShip'></span>
											</td><td>
												<span >" + item.CreatedOn.ToShortDateString() + @"</span>   
											</td>
												<td>
												<span >" + ProductName + @"</span>   
											</td>
<td>                                                
													<span id='lblTotalProducts'>" + item.User.FirstName + @"</span>                                                
											</td>
<td>                                                
													<span id='lblTotalProducts'>" + PaymentMode + @"</span>                                                
											</td>
											 <td>                                                
													<span id='lblstatus'>" + status + @"</span>                                                
											</td><td>                                                
												<span id='lblcustName'><span>" + item.CurrencySymbol + @" </span>" + String.Format("{0:0.00}", item.TxnAmount) + @"</span></span>
											</td>
			</tr>");
                }
                catch (Exception ex)
                {

                }
            }

            strResult.Append(@"
		<div class='widget'>                   
					
					<div class='widget_body'>
							<div id='Main_dvGrid'>
								<div class='widget marg-0'> 
								<div class='widget_title'>
									<span class='iconsweet'>r</span><h5>Available Orders</h5>
								</div>
								<div class='widget_body'>
						
							<div class='cp_productlist_view'>
								<div class='products_views'>
									
								</div>
							</div>
						   
										<table class='activity_datatable' rules='all' id='grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
											<thead>
								<tr>
												<th scope='col'>ID</th><th scope='col'>Date</th><th scope='col'>Product Name</th><th scope='col'>Customer Name</th><th scope='col'>Payment Mode</th><th scope='col'>Status</th><th scope='col'>Amount</th>
											</tr></thead><tbody>
								" + strtd + @"
								</tbody></table>	</div>

  
									</div>
							</div>
							
							
					</div>
				</div>
				<input name='ctl00$ctl00$Main$Main$hdnTodoSearchcriteria' id='ctl00_ctl00_Main_Main_hdnTodoSearchcriteria' value='[&quot;Order No&quot;,&quot;SKU&quot;,&quot;Email Id&quot;,&quot;Phone No&quot;,&quot;Coupon Code&quot;,&quot;Checkout Type&quot;]' type='hidden'>
				<img id='img441' alt='' onload='getdivCurrency();' src='Orders_files/designoption_009_13.jpg' height='1' width='1'>
			
</div>      

			</div>
		</div>");

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strResult.ToString(),
                CartSum = data.Count().ToString()
            };
        }
        else
        {
            strResult.Append(@" <div id='divMessage' class='msgbar msg_Info hide_onC'>
								<span id='msgicon' class='iconsweet'>*</span>
								<p><span id='lblMessage'>There are no 'Orders' found.</span></p>
							</div>");

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = strResult.ToString(),
                CartSum = data.Count().ToString()
            };
        }
    }
    [HttpPost]
    public CustomResponse GetNextCheckOutOrders(PaymentTransaction objtrn)
    {
        var repository = new PaymentTransactionRepository();
        int totalRecords;
        int skipRecords = 20; /*Convert.ToInt32(objtrn.ServiceTax.Value);*/
        db_Zon_HuwEntities Context = new db_Zon_HuwEntities();

        List<CheckOutPaymentTransaction> data = new List<CheckOutPaymentTransaction>();
        DateTime UpdatedOn = Convert.ToDateTime("1/1/0001 12:00:00 AM");
        DateTime CreatedDate = Convert.ToDateTime("1/1/0001 12:00:00 AM");
        if (objtrn.UpdatedOn != null || objtrn.UpdatedOn != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
        {
            UpdatedOn = objtrn.UpdatedOn.Value.AddDays(1);
        }
        if (objtrn.PaymentTransactionId != 0 || objtrn.OrderCurrentStatus != 0 || objtrn.CreatedOn != CreatedDate)
        {
            //data = (from q in Context.CheckOutPaymentTransactions
            //        where (objtrn.PaymentTransactionId == 0 || objtrn.PaymentTransactionId == null)? true : (p.PaymentTransactionId == objtrn.PaymentTransactionId))
            //         //&& (objtrn.PaymentMode == "" ? true : (p.PaymentMode == objtrn.PaymentMode))
            //         //&& (objtrn.PaymentStatus == -1 ? true : (objtrn.PaymentStatus == 0 ? (p.OrderCurrentStatus != 0 && p.OrderCurrentStatus != 1) : (p.OrderCurrentStatus == 0 || p.OrderCurrentStatus == 1 || p.OrderCurrentStatus == null)))
            //         //&& (objtrn.OrderCurrentStatus == -2 ? true : (p.OrderCurrentStatus == objtrn.OrderCurrentStatus))
            //         //&& (objtrn.CreatedOn == CreatedDate ? true : (p.CreatedOn >= objtrn.CreatedOn && p.CreatedOn <= UpdatedOn))
            //         && (objtrn.OrderStatus == "Null" ? true : (q.OrderStatus == objtrn.OrderStatus))
            //         && (objtrn.CreatedOn == CreatedDate ? true : (q.CreatedOn >= objtrn.CreatedOn && p.CreatedOn <= UpdatedOn))
            //        select p).OrderByDescending(x => x.PaymentTransactionId).Skip(skipRecords).Take(objtrn.rows).ToList();
        }
        else
        {
            data = (from p in Context.CheckOutPaymentTransactions
                    select p).OrderByDescending(x => x.PaymentTransactionId).Skip(skipRecords).Take(objtrn.rows).ToList();
        }

        StringBuilder strtd = new StringBuilder();
        StringBuilder strResult = new StringBuilder();
        if (data.Count > 0)
        {
            foreach (var item in data)
            {
                try
                {
                    db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();
                    string statusvalue = item.OrderCurrentStatus.ToString();
                    var status = Shared.GetOrderStatusEnum(statusvalue);

                    StringBuilder ProductName = new StringBuilder();

                    for (var i = 0; i < item.CheckOutUserProductTransactions.Count; i++)
                    {
                        if (i == 0)
                        {
                            ProductName.Append("<a target='_blank' title='Remaining Quantity: " + item.CheckOutUserProductTransactions.ElementAtOrDefault(i).Product.Quantity + "' style='color:blue;' href='/Admin/UpdateProducts.aspx?ID=" + item.CheckOutUserProductTransactions.ElementAtOrDefault(i).Product.ProductId + "'>" + item.CheckOutUserProductTransactions.ElementAtOrDefault(i).Product.ProductName + "</a>");
                        }
                        else
                        {
                            ProductName.Append(", " + "<a target='_blank' title='Remaining Quantity: " + item.CheckOutUserProductTransactions.ElementAtOrDefault(i).Product.Quantity + "' style='color:blue;' href='/Admin/UpdateProducts.aspx?ID=" + item.CheckOutUserProductTransactions.ElementAtOrDefault(i).Product.ProductId + "'>" + item.CheckOutUserProductTransactions.ElementAtOrDefault(i).Product.ProductName + "</a>");
                        }
                    }

                    string PaymentMode = "";
                    if (item.PaymentMode == "Cash On Delivery")
                    {
                        PaymentMode = "Postpaid";
                    }
                    else
                    {
                        PaymentMode = "Prepaid";
                    }

                    strtd.Append(@"<tr><td>
                                                 <a id='hypEdit'  onclick='NavigatetoCheckoutOrderDetailsPage(" + item.PaymentTransactionId + ")' >" + item.PaymentTransactionId + @"</a><br>
                                                <span id='lblPartialShip'></span>
                                            </td><td>
                                                <span >" + item.CreatedOn.ToShortDateString() + @"</span>   
                                            </td>
                                                <td>
                                                <span >" + ProductName + @"</span>   
                                            </td>
<td>                                                
                                                    <span id='lblTotalProducts'>" + item.CheckOutUserProductTransactions.Count + @"</span>                                                
                                            </td>
<td>                                                
                                                    <span id='lblTotalProducts'>" + item.User.FirstName + @"</span>                                                
                                            </td>
<td>                                                
                                                    <span id='lblTotalProducts'>" + PaymentMode + @"</span>                                                
                                            </td>
                                             <td>                                                
                                                                                             <span id='lblOrderStatus'>" + item.OrderStatus + @"</span>                                      
                                            </td><td>                                                
                                                <span id='lblcustName'><span>" + item.CurrencySymbol + @" </span>" + String.Format("{0:0.00}", item.TxnAmount) + @"</span></span>
                                            </td>
            </tr>");
                }
                catch (Exception ex)
                {

                }
            }

            strResult.Append(@" <table class='activity_datatable' rules='all' id='grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'><tbody>" + strtd + @"</tbody></table>");

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strResult.ToString(),
                CartSum = (skipRecords + data.Count()).ToString()
            };
        }
        else
        {
            strResult.Append(@" <div id='divMessage' class='msgbar msg_Info hide_onC'>
                                <span id='msgicon' class='iconsweet'>*</span>
                                <p><span id='lblMessage'>There are no 'Orders' found.</span></p>
                            </div>");

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = strResult.ToString(),
                CartSum = (skipRecords + data.Count()).ToString()
            };
        }
    }
    [HttpPost]
    public CustomResponse GetSearchordersbyProductName(string ProductName, DateTime? CreatedOn, DateTime? UpdatedOn)
    {
        DateTime UpdatedDate = Convert.ToDateTime("1/1/0001 12:00:00 AM");
        DateTime CreatedDate = Convert.ToDateTime("1/1/0001 12:00:00 AM");
        if (UpdatedOn != null || UpdatedOn != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
        {
            UpdatedOn = UpdatedOn.Value.AddDays(1);
        }
        var repository = new PaymentTransactionRepository();
        int totalRecords;
        db_Zon_HuwEntities db = new db_Zon_HuwEntities();

        var ProductIDs = from c in db.Products
                         where c.ProductName.Contains(ProductName)
                         select c.ProductId;
        StringBuilder strResult = new StringBuilder();
        StringBuilder strtd = new StringBuilder();
        foreach (var ProductID in ProductIDs)
        {

            var PaymentTxnIDs = from c in db.UserProductTransactions
                                where c.ProductId == ProductID
 && (CreatedOn == CreatedDate ? true : (c.CreatedOn >= CreatedOn && c.CreatedOn <= UpdatedOn))
                                select c.PaymentTransactionId;
            foreach (var TxnID in PaymentTxnIDs)
            {
                var data = (repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, 50, (p => p.PaymentTransactionId == TxnID), null, "", true));
                string PrdctName = db.Products.Where(x => x.ProductId == ProductID).First().ProductName;

                string statusvalue = data.FirstOrDefault().OrderCurrentStatus.ToString();
                var status = Shared.GetOrderStatusEnum(statusvalue);

                strtd.Append(@"<tr><td>
												<a id='hypEdit' onclick='NavigatetoOrderDetailsPage(" + data.FirstOrDefault().PaymentTransactionId + ")' >" + data.FirstOrDefault().PaymentTransactionId + @"</a><br>
												<span id='lblPartialShip'></span>
											</td><td>
												<span >" + data.FirstOrDefault().CreatedOn.ToShortDateString() + @"</span>   
											</td><td>
												<span >" + PrdctName + @"</span>   
											</td><td>
												<span >" + data.FirstOrDefault().User.FirstName + @"</span>   
											</td><td>                                                
													<span id='lblTotalProducts'>" + data.FirstOrDefault().PaymentMode + @"</span>                                                
											</td>
												<td>                                                
													<span id='lblTotalProducts'>" + status + @"</span>                                                
											</td><td>                                                
												<span id='lblcustName'><span>" + data.FirstOrDefault().CurrencySymbol + @" </span>" + String.Format("{0:0.00}", data.FirstOrDefault().TxnAmount) + @"</span></span>
											</td>
			</tr>");

            }

        }
        strResult.Append(@"
		<div class='widget'>                   
					
					<div class='widget_body'>
							<div id='Main_dvGrid'>
								<div class='widget marg-0'> 
								<div class='widget_title'>
									<span class='iconsweet'>r</span><h5>Available Orders</h5>
								</div>
								<div class='widget_body'>
						
							<div class='cp_productlist_view'>
								<div class='products_views'>
									
								</div>
							</div>
						   
									<table class='activity_datatable' rules='all' id='grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
										<thead>
							<tr>
											<th scope='col'>ID</th><th scope='col'>Date</th><th scope='col'>Product Name</th><th scope='col'>Customer Name</th><th scope='col'>Payment Mode</th><th scope='col'>Status</th><th scope='col'>Amount</th>
										</tr></thead><tbody>
							" + strtd + @"
							</tbody></table>	</div>

  
									</div>
							</div>
							
							
					</div>
				</div>
				<input name='ctl00$ctl00$Main$Main$hdnTodoSearchcriteria' id='ctl00_ctl00_Main_Main_hdnTodoSearchcriteria' value='[&quot;Order No&quot;,&quot;SKU&quot;,&quot;Email Id&quot;,&quot;Phone No&quot;,&quot;Coupon Code&quot;,&quot;Checkout Type&quot;]' type='hidden'>
				<img id='img441' alt='' onload='getdivCurrency();' src='Orders_files/designoption_009_13.jpg' height='1' width='1'>
			
				</div>     

			</div>
		</div>");
        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Result = strResult.ToString()
        };
    }
    public CustomResponse GetMyProductOverview(int transId)
    {

        var repository = new UserProductTransactionRepository();

        int totalRecords;
        var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, 0, (p => p.PaymentTransactionId == transId), null, "", true);
        StringBuilder strResult = new StringBuilder();
        StringBuilder strtd = new StringBuilder();

        if (data.Count > 0)
        {
            foreach (var item in data)
            {
                strtd.Append(@"<tr>
				<td>
							<center>
								
								<span ><div id='chkDeliver'><span>
						   <input  id='chkkShip'  value=" + item.PaymentTransactionId + @"  type='checkbox' class='Check' onclick='CheckboxChecked(this)' >
							</center>
							<input id='hdnShippingMode' class='hdnShippingMode' type='hidden'>
						</td><td style='width:45%;'>
							<div class='product_image fl_left'>
								<img src=../" + item.Product.ProductImgUrl.Replace("~/", ApiUrl) + @" id='imgProductImage'>
							</div>
							<div class='product_info fl_left'>
								<ul>
									<li>
										<div class='title'>
										" + item.Product.ProductName + @"
										</div>
									</li>
									<li class='topmargin_15'>
										<span id='lblSKUText'>SKU:</span>
										<a id='Sku'  style='text-decoration: underline;color: #3B5998; cursor: pointer;'>" + item.PaymentTransactionId + @"</a>
									</li>
								   
									<li>
										<br>
									</li>
								</ul>
							</div>
						</td><td>
							
							<span id='lblProdQuantity' style='padding-left: 3px;'>" + item.Quantity + @"</span><br>
							
							<span id='lblShipQuantity' style='padding-left: 3px;'>" + item.PaymentTransaction.ShippingCharges + @"</span>
						</td><td>
							<span id='ProductCost'> <span class='WebRupee'>Rs. </span> " + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.ProductCost), true) + @"</span>
						</td><td>
						  
							<span id='ctl00_ctl00_Main_Main_rptSuppliers_ctl01_grdProducts_ctl02_lblTotalPrice'> <span class='WebRupee'>Rs. </span> " + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.ProductCost)) + @"</span>
						</td>
			</tr>");
            }

            //<span>Ordered</span>
            //<span id='lblShip'>Shipped</span>

            strResult.Append(@"<div class='one_wrap'>
	<div class='widget'>
		 
		 <div class='widget_body'>
		  <div class='order_details'>
			   <ul>
			  <li> <span class='field_caption'>Order ID: </span> <span class='field_value'><span>" + data.FirstOrDefault().PaymentTransactionId + @"</span></span> 
					 <div class='fl_right text_right user_details'>
						<p>
						<span class='u_name'><span>" + data.FirstOrDefault().User.FirstName + @"</span></span>
						<br>
						<a href='#'><span>" + data.FirstOrDefault().User.EmailId + @"</span></a>
						<br>
						<span class='u_phone'><span>" + data.FirstOrDefault().User.MobileNo + @"</span></span>
						</p>
					</div>
			  </li>
			  <li> <span class='order_date'><span>" + data.FirstOrDefault().CreatedOn.ToString("dd-MMM-yyyy HH:mm:ss") + @"</span></span> </li>
			</ul>
		  </div>
		 
		<div id='dvgrdProducts'> 
			   
					</div>
	 
		<div class='cp_productlist_view'>
		  

			<div>
		
					  
	  </div>
	<div class='clear'>
				</div>
			</div>
			 <div class='action_bar' id='dvButtons'>
			<input  value='Return/Refund' class='button_small greyishBtn  fl_right' onclick='NavigatetoOrderReturnsPage()'>
					<input value='Cancel Order' onclick='' class='button_small greyishBtn  fl_right' type='submit'>
				<input value='Print Invoice' onclick='' id='btnInvoice' class='button_small greyishBtn  fl_right' type='submit'>   
				 
				<div class='clear'>
				</div>
			</div>
			 </div>
		 </div>
 </div>
<div class='inlineblock'>
	<!--Block 1 -->
	<div class='one_two_wrap fl_left'>
			<div class='widget'>
				<div class='widget_title'><span id='spnBillAddress' class='iconsweet'>r</span>
					<h5>
						Billing Address
					</h5>
				</div>
				<div class='widget_body'>
				<div class='content_pad'>
				<p>
						" + data.FirstOrDefault().User.FirstName + @"<br>" + data.FirstOrDefault().UserAddress1.StreetAddress1 + "<br>" + data.FirstOrDefault().UserAddress1.StreetAddress2 + @"<br><span class='hightlight'>" + data.FirstOrDefault().UserAddress1.LandMark + @"</span>," + data.FirstOrDefault().UserAddress1.City + @", <span class='hightlight'>" + data.FirstOrDefault().UserAddress1.StateName + @"</span><br>" + data.FirstOrDefault().UserAddress1.PinCode + @"<br>" + data.FirstOrDefault().UserAddress1.Country.CountryName + @"<br>Ship Mobile:" + data.FirstOrDefault().User.MobileNo + @"<br>
					   <br>
					   <a id='hypBillUserName' >" + data.FirstOrDefault().User.EmailId + @"</a>
				</p>
				</div>
				</div>
			</div>
		</div>
	<!-- Block 1 Ends -->
 <!-- Block 2 -->
		<div class='one_two_wrap fl_right'>
			<div class='widget'>
				<div class='widget_title'><span id='spnShipAddress' class='iconsweet'>r</span><h5>  Shipping Address</h5></div>
				<div class='widget_body'>
				<div class='content_pad'>
				<p>
				 " + data.FirstOrDefault().User.FirstName + @"<br>" + data.FirstOrDefault().UserAddress.StreetAddress1 + "<br>" + data.FirstOrDefault().UserAddress.StreetAddress2 + @"<br><span class='hightlight'>" + data.FirstOrDefault().UserAddress.LandMark + @"</span>," + data.FirstOrDefault().UserAddress.City + @", <span class='hightlight'>" + data.FirstOrDefault().UserAddress.StateName + @"</span><br>" + data.FirstOrDefault().UserAddress.PinCode + @"<br>" + data.FirstOrDefault().UserAddress.Country.CountryName + @"<br>Ship Mobile:" + data.FirstOrDefault().User.MobileNo + @"<br>" + data.FirstOrDefault().User.EmailId + @"<br>
				 
				</p>
				</div>
				</div>
			</div>
		</div>  
	<!-- Block 2 ends here-->
	</div>




");
            //<input  value='Waiting for Pickup' onclick='' id='btnShipOrder' class='button_small greyishBtn  fl_right' type='submit'>  
            //  <input value='Reverse Ship' onclick='' id='btnReverseShip' class='button_small greyishBtn  fl_right' type='submit'>
            //<br>
            //<a id='ctl00_ctl00_Main_Main_hlnkEditShipping' onclick='vpb_show_sign_up_box(this, " + data.FirstOrDefault().UserAddress.UserAddressId + @");'>Edit Shipping</a> 

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strResult.ToString()

            };
        }

        else
        {
            StringBuilder strRlt = new StringBuilder();
            strResult.Append(@" <div id='ctl00_ctl00_Main_Main_divMessage' class='msgbar msg_Info hide_onC'>
								<span id='ctl00_ctl00_Main_Main_msgicon' class='iconsweet'>*</span>
								<p><span id='ctl00_ctl00_Main_Main_lblMessage'>There are no 'Orders' found.</span></p>
							</div>");

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = strRlt.ToString()
            };
        }

    }

    public dynamic GetReplacementDetails(int trnsId)
    {
        var repository1 = new UserProductTransactionRepository();
        int totalRecords;
        var repository = new ProductRepository();
        var data = repository1.FetchAllByPage(p => p.TransactionId, out totalRecords, 0, 0,
                                          (p => p.TransactionId == trnsId), null, "");
        var pId = data.FirstOrDefault().ProductId;
        Product product = null;
        product = repository.Single(p => p.ProductId == pId);
        data.FirstOrDefault().PaymentTransactionId = 0;
        data.FirstOrDefault().PaymentTransaction = null;


        CustomCurrency CurrentCurrency = (CustomCurrency)BAL.BalUtility.GetSession(Shared.Sessions.Currency);
        var cart = new UserProductTransaction()
        {
            PaymentTransaction = null,
            PaymentTransactionId = 0,
            CurrencyCode = data.FirstOrDefault().CurrencyCode,
            CurrencySymbol = data.FirstOrDefault().CurrencySymbol,
            CurrencyValue = data.FirstOrDefault().CurrencyValue,
            Status = 0,
            ProductId = pId,
            Quantity = data.FirstOrDefault().Quantity,
            UserId = data.FirstOrDefault().UserId,
            Product = product,
            ProductCost = data.FirstOrDefault().ProductCost,
            CreatedOn = DateTime.Now,
            UpdatedOn = DateTime.Now,
            BillingAddressId = data.FirstOrDefault().BillingAddressId,
            ShippingAddressId = data.FirstOrDefault().ShippingAddressId,
            TransactionId = data.FirstOrDefault().TransactionId
        };
        BalUtility.ClearSession(Shared.Sessions.ReplacementList);
        var cartItems = (List<UserProductTransaction>)BalUtility.GetSession(Shared.Sessions.ReplacementList) ??
                           new List<UserProductTransaction>();

        cartItems.Add(cart);

        BalUtility.CreateSession(cartItems, Shared.Sessions.ReplacementList);
        return cartItems.ToList();
    }

    public CustomResponse CheckoutOrderStatus(int OrderID, string OrderStatus)
    {
        var repository = new CheckoutStatusRepository();

        long CommentStatus = repository.CheckoutStatus(OrderID, OrderStatus, 160);

        if (CommentStatus != 0)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString()
            };
        }
        else
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString()
            };
        }
    }

    public CustomResponse GetProductOverview(int transId)
    {
        var repositorys = new ImagerespoRepository();
        var repository = new UserProductTransactionRepository();
        var newrepository = new LogRepository();
        int totalRecords;
        var newdata = newrepository.FetchAllByPage(x => x.Paymenttransactionid, out totalRecords, 0, 0, (p => p.Paymenttransactionid == transId), null, "", true);
        var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, 0, (p => p.PaymentTransactionId == transId), null, "", true);
        var datas = repositorys.FetchAllByPage(x => x.Payment_Transaction_Id, out totalRecords, 0, 0, (p => p.Payment_Transaction_Id == transId), null, "", true);

        StringBuilder strResult = new StringBuilder();
        StringBuilder strtd = new StringBuilder();
        StringBuilder strprc = new StringBuilder();

        StringBuilder strRefundAmount = new StringBuilder();
        db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();
        if (data.FirstOrDefault().PaymentTransaction.DoctorName != null)
        {
            strprc.Append(@"<div class='fl_right'><label>Doctor/Hospital Name:</label><input type='text' id='txtdoctorname' style='width:70%' value='" + data.FirstOrDefault().PaymentTransaction.DoctorName + @"'/><input type='button' onclick='updatedoctor(" + data.FirstOrDefault().PaymentTransaction.PaymentTransactionId + @")' class='button_small greyishBtn ' value='Submit'/></div>");
        }
        else if (data.Where(x => x.Product.IsPresciption == true).FirstOrDefault() != null)
        {
            strprc.Append(@"<div class='fl_right'><label>Doctor/Hospital Name:</label><input type='text' id='txtdoctorname' style='width:70%' value='" + data.FirstOrDefault().PaymentTransaction.DoctorName + @"'/><input type='button' onclick='updatedoctor(" + data.FirstOrDefault().PaymentTransaction.PaymentTransactionId + @")' class='button_small greyishBtn ' value='Submit'/></div>");
        }
        if (data.Count > 0)
        {
            var TxnStatus = "";
            var TxnMessage = "";
            var RefundAmount = "";

            foreach (var item in data)
            {
                string ProductName = "";
                decimal ProductCost = 0;
                if (item.SubProductId == null)
                {
                    ProductName = item.ProductName;
                    if (item.ProductName == null)
                        ProductName = item.Product.ProductName;

                    ProductCost = item.Product.ProductCost;
                }
                else
                {
                    SubProduct Sp = Entity.SubProducts.Where(x => x.SubProductId == item.SubProductId).First();
                    ProductName = item.ProductName + "(" + Sp.SPName + ")";
                    ProductCost = Sp.ProductCost;
                }
                if (data.FirstOrDefault().UserAddress1.StateName != "Telangana")
                {
                    strtd.Append(@"

<tr>
				<td style='width:45%;'>
							<div class='product_image fl_left'>
								<img src=" + item.Product.ProductImgUrl.Replace("~/", ApiUrl) + @" id='imgProductImage'>
							</div>
							<div class='product_info fl_left'>
								<ul>
									<li>
										<div class='title'>
										" + ProductName + @"
									 </div>
									</li>
									<li class='topmargin_15'>
										<span id='lblSKUText'>SKU: " + item.Product.ProductId + @" </span>                                        
									</li>                                   
									<li>
										<br>
									</li>
								</ul>
							</div>
						</td><td>                            
							<span id='lblProdQuantity' style='padding-left: 3px;'>" + item.Quantity + @"</span><br>
						</td><td>
							<span id='ProductCost'> <span class='WebRupee'>Rs. </span> " + BalUtility.FormatedDoubleWithOutPrecision((Convert.ToDouble(item.ProductCost / item.Quantity)) - Convert.ToDouble((item.ProductCost / item.Quantity) - (((item.ProductCost / item.Quantity) * (100 / (100 + item.Product.GST))))), true) + @"</span>
						</td>
                        <td>
							<span id='GST'> <span class='WebRupee'> </span> " + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(((item.ProductCost) - (((item.ProductCost) * (100 / (100 + item.Product.GST)))))), true) + @"</span>
						</td>
                         <td>                          
							<span id='ctl00_ctl00_Main_Main_rptSuppliers_ctl01_grdProducts_ctl02_lblTotalPrice'> <span class='WebRupee'>Rs. </span> " + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.ProductCost)) + @"</span>
						</td></tr>");
                }
                else
                {
                    strtd.Append(@"

<tr>
				<td style='width:45%;'>
							<div class='product_image fl_left'>
								<img src=" + item.Product.ProductImgUrl.Replace("~/", ApiUrl) + @" id='imgProductImage'>
							</div>
							<div class='product_info fl_left'>
								<ul>
									<li>
										<div class='title'>
										" + ProductName + @"
									 </div>
									</li>
									<li class='topmargin_15'>
										<span id='lblSKUText'>SKU: " + item.Product.ProductId + @" </span>                                        
									</li>                                   
									<li>
										<br>
									</li>
								</ul>
							</div>
						</td><td>                            
							<span id='lblProdQuantity' style='padding-left: 3px;'>" + item.Quantity + @"</span><br>
						</td><td>
							<span id='ProductCost'> <span class='WebRupee'>Rs. </span> " + BalUtility.FormatedDoubleWithOutPrecision((Convert.ToDouble(item.ProductCost / item.Quantity)) - Convert.ToDouble((item.ProductCost / item.Quantity) - (((item.ProductCost / item.Quantity) * (100 / (100 + item.Product.GST))))), true) + @"</span>
						</td>
                        <td>
							<span id='cGST'> <span class='WebRupee'></span> " + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble((((item.ProductCost) - (((item.ProductCost) * (100 / (100 + item.Product.GST))))) / 2)), true) + @"</span>
						</td>
                         <td>
							<span id='IGST'> <span class='WebRupee'> </span> " + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble((((item.ProductCost) - (((item.ProductCost) * (100 / (100 + item.Product.GST))))) / 2)), true) + @"</span>
						</td>
                         <td>                          
							<span id='ctl00_ctl00_Main_Main_rptSuppliers_ctl01_grdProducts_ctl02_lblTotalPrice'> <span class='WebRupee'>Rs. </span> " + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.ProductCost)) + @"</span>
						</td></tr>");
                }
                //        foreach (var items in datas)
                //        {
                //            strtd.Append(@"<div class='product_image fl_left'>
                //<img src =" + items.Prescription_Image.Replace("~/", ApiUrl) + @"  id='imgProductImage' style='width: 296.99;'>
                //                                        </div>");
                //        }

                DateTime CreatedOn = data.FirstOrDefault().PaymentTransaction.CreatedOn;
                DateTime CurrentDate = DateTime.Now;
                TimeSpan span = CurrentDate.Subtract(CreatedOn);

                if (span.Minutes > 15 && data.FirstOrDefault().PaymentTransaction.TxnStatus == null && data.FirstOrDefault().PaymentTransaction.TxnMessage == null)
                {
                    TxnStatus = "No response from Payment Gateway";
                    TxnMessage = "No response from Payment Gateway";
                }
                else
                {
                    TxnStatus = data.FirstOrDefault().PaymentTransaction.TxnStatus;
                    TxnMessage = data.FirstOrDefault().PaymentTransaction.TxnMessage;
                }
                if (item.PaymentTransaction.OrdersReturnAction == "Refund")
                {
                    TxnStatus = "Amount Refunded";
                    RefundAmount = BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(data.FirstOrDefault().PaymentTransaction.TxnAmount));
                    strRefundAmount.Append(@"<span>Refunded Amount : <span style='font-size:16px; color:green;'>" + RefundAmount + @"</span></span><br/>");
                }
            }

            if (data.FirstOrDefault().PaymentTransaction.Free_Product_ID != null)
            {
                long freeProductId = data.FirstOrDefault().PaymentTransaction.Free_Product_ID.Value;

                Product free = Entity.Products.Where(x => x.ProductId == freeProductId).First();

                strtd.Append(@"<tr>
				<td style='width:45%;'>
							<div class='product_image fl_left'>
								<img src=" + free.ProductImgUrl.Replace("~/", ApiUrl) + @" id='imgProductImage'>
							</div>
							<div class='product_info fl_left'>
								<ul>
									<li>
										<div class='title'>
										" + free.ProductName + @"
									 </div>
									</li>
									<li class='topmargin_15'>                                        
										
									</li>
								   
									<li>
										<br>
									</li>
								</ul>
							</div>
						</td><td>                            
							<span id='lblProdQuantity' style='padding-left: 3px;'>1</span><br>
						</td><td>
							<span id='ProductCost'> <span class='WebRupee'>Rs. </span> " + free.ProductCost + @"</span>
						</td><td>                          
							<span style='font-weight:bold;' id='ctl00_ctl00_Main_Main_rptSuppliers_ctl01_grdProducts_ctl02_lblTotalPrice'> FREE</span>
						</td>
			</tr>");
            }


            if (data.FirstOrDefault().PaymentTransaction.Offer_Product_ID != null)
            {
                int? ProductId = data.FirstOrDefault().PaymentTransaction.Offer_Product_ID;

                Tbl_Offered_Products Offer = Entity.Tbl_Offered_Products.Where(x => x.Offer_Product_ID == ProductId).First();

                strtd.Append(@"<tr>
				<td style='width:45%;'>
							<div class='product_image fl_left'>
								<img src=" + Offer.Product_Image + @" id='imgProductImage'>
							</div>
							<div class='product_info fl_left'>
								<ul>
									<li>
										<div class='title'>
										" + Offer.Offer_Product_Name + @"
									 </div>
									</li>
									<li class='topmargin_15'>                                        
										
									</li>
								   
									<li>
										<br>
									</li>
								</ul>
							</div>
						</td><td>
							
							<span id='lblProdQuantity' style='padding-left: 3px;'>1</span><br>
							
							
						</td><td>
							<span id='ProductCost'> <span class='WebRupee'>Rs. </span> " + Offer.Product_Actual_Cost + @"</span>
						</td><td>                          
							<span style='font-weight:bold;' id='ctl00_ctl00_Main_Main_rptSuppliers_ctl01_grdProducts_ctl02_lblTotalPrice'> FREE</span>
						</td>
			</tr>");

            }


            string statusvalue = data.FirstOrDefault().PaymentTransaction.OrderCurrentStatus.ToString();
            var status = Shared.GetOrderStatusEnum(statusvalue);

            StringBuilder strcode = new StringBuilder();
            StringBuilder strcod = new StringBuilder();
            StringBuilder strShipment = new StringBuilder();
            StringBuilder strOrderStatus = new StringBuilder();

            if (status == Shared.OrderStatus.Delivered || status == Shared.OrderStatus.Cancelled || status == Shared.OrderStatus.Refund || status == Shared.OrderStatus.Pending || status == Shared.OrderStatus.Returns)
            {
                if (status == Shared.OrderStatus.Pending || status == Shared.OrderStatus.Cancelled)
                {
                    strOrderStatus.Append(@"<input  value='Make Order Success'  class='button_small greyishBtn  fl_right' onclick='MakeOrderSuccess()'/><input value='Cancel Order' onclick='PopupDataCancel()' class='button_small greyishBtn  fl_right' type='button'>");
                }
                else if (status == Shared.OrderStatus.Delivered)
                {
                    strOrderStatus.Append(@"<input value='Cancel/Refund Order' onclick='PopupDataCancel()' class='button_small greyishBtn  fl_right' type='button'>");

                }
            }
            else
            {
                if (Convert.ToInt32(HttpContext.Current.Session["RoleID"]) == 6)
                {
                    User empdetails = ((User)BalUtility.GetSession(Shared.Sessions.Employee));
                    var emprepository = new EmployeeRepository();
                    var accessdata = emprepository.Getaccesspages(empdetails.UserId);

                    if (accessdata.LastOrDefault().Accessid == 26)
                    {
                        strOrderStatus.Append(@"<input  value='Return/Refund'  class='button_small greyishBtn  fl_right' onclick='Redirect()'>
                    <input value='Track Shipment' onclick='Trackshipment(" + data.FirstOrDefault().PaymentTransaction.ShipmentId + ")' class='button_small greyishBtn  fl_right' type='button'>");
                    }
                    else
                    {
                        strOrderStatus.Append(@"<input  value='Return/Refund'  class='button_small greyishBtn  fl_right' onclick='Redirect()'>
					<input value='Cancel/Refund Order' onclick='PopupDataCancel()' class='button_small greyishBtn  fl_right' type='button'>
                    <input value='Track Shipment' onclick='Trackshipment(" + data.FirstOrDefault().PaymentTransaction.ShipmentId + ")' class='button_small greyishBtn  fl_right' type='button'>");

                    }
                }
                else
                {
                    strOrderStatus.Append(@"<input  value='Return/Refund'  class='button_small greyishBtn  fl_right' onclick='Redirect()'>
					<input value='Cancel/Refund Order' onclick='PopupDataCancel()' class='button_small greyishBtn  fl_right' type='button'>
                    <input value='Track Shipment' onclick='Trackshipment(" + data.FirstOrDefault().PaymentTransaction.ShipmentId + ")' class='button_small greyishBtn  fl_right' type='button'>");

                }
            }
            string HasPromocode = "";
            CommentRepository repo = new CommentRepository();
            List<Tbl_OrderComments> orderComments = repo.GetOrderComments(transId);
            StringBuilder strOrderComments = new StringBuilder();
            StringBuilder strComment = new StringBuilder();
            for (int i = 0; i < orderComments.Count; i++)
            {
                strComment.Append(@"<p>" +
                 +(i + 1) + @") Comment: " + orderComments[i].Comment + @"<br> Date: " + orderComments[i].Comment_Date + @"<br>                 
				</p>");
            }
            if (orderComments.Count != 0)
            {
                strOrderComments.Append(@"
			<div class='widget'>
				<div class='widget_title'><span id = 'spnShipAddress' class='iconsweet'>r</span><h5>  Comments</h5></div>
				<div class='widget_body'>
				<div class='content_pad'>
				" + strComment.ToString() + @"
				</div>
				</div>
			</div>
		</div>  ");
            }

            if (data.FirstOrDefault().PaymentTransaction.Has_Promo_Code == true)
            {
                int PromocodeID = data.FirstOrDefault().PaymentTransaction.Promo_Code_ID.Value;
                Tbl_Coupon_Info PromocodeData = Entity.Tbl_Coupon_Info.Where(x => x.Coupon_Id == PromocodeID).First();
                HasPromocode = PromocodeData.Coupon_Code;

                strcode.Append(@"<tr id='trPromocode'>
									<td class='field_caption '> Promocode Amount: </td>
								   <td class='field_value text_right'> <span id='lblGrandTotalPrice'> <span class='WebRupee'>Rs. </span>" + data.FirstOrDefault().PaymentTransaction.Promo_Code_Amount + @"</span></td>
								</tr> 
							   <tr class='ordertotal'>
							<td class='field_caption text_right'>
							   Grand Total :
							</td>
							<td class='field_value text_right'>
								<span id='lblGrandTotalPrice'> <span class='WebRupee'>Rs. </span> " + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(data.FirstOrDefault().PaymentTransaction.TxnAmount)) + @"</span>
							</td>
						</tr>
						<tr id='trPromo'>
									<td class='field_caption '>Applied Promocode: </td>
								   <span id='lblGrandTotalPrice'> <td class='field_value'>" + HasPromocode + @"</span></td>
								</tr> ");

            }
            else
            {
                HasPromocode = "No Promocode Applied";
                strcode.Append(@"<tr id='trPromocode'>		                            
								</tr>
							<tr class='ordertotal'>
							<td class='field_caption text_right'>
							   Grand Total :
							</td>
							<td class='field_value text_right'>
								<span id='lblGrandTotalPrice'> <span class='WebRupee'>Rs. </span> " + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(data.FirstOrDefault().PaymentTransaction.TxnAmount)) + @"</span>
							</td>
						</tr>");
            }

            if (data.FirstOrDefault().PaymentTransaction.ShipmentId != null)
            {
                strShipment.Append(@"<div class='one_two_wrap fl_left'><div class='widget'>
					<div class='widget_title'><span id='spnBillAddress' class='iconsweet'>r</span>
					<h5>Shipment Details</h5></div>
					<div class='widget_body'><div class='content_pad'>
					   Shipment ID: " + data.FirstOrDefault().PaymentTransaction.ShipmentId + @"<br /><br /> Courier Name:" + data.FirstOrDefault().PaymentTransaction.CourierName + "<br /><br /> PickUp ID:" + data.FirstOrDefault().PaymentTransaction.PickupID + @"<br /><br />
					</p></div></div></div></div>"
                );
                if (data.FirstOrDefault().PaymentTransaction.ShipmentId != "EcomTest")
                {
                    if (data.FirstOrDefault().PaymentTransaction.CourierName == "Ecom Express")
                    {

                        strShipment.Append(@"<!-- iframe accordian -->
             <div class='one_two_wrap fl_right' style='width: 100%'>
                <div class='widget'>
                    <div class='widget_title min'>
                        <span id=''class='iconsweet'>r</span><h5>Ecom Tracking</h5>
                    </div>
                    <div class='widget_body max'>
                        <div class='content_pad'>
                           <iframe id = 'load' width='100%' src='http://ecomexpress.in/tracking/?awb_field=" + data.FirstOrDefault().PaymentTransaction.ShipmentId + @"' height='250'></iframe>
                        </div>
                    </div>
                </div>
            </div>
 <div class='plus'><i class='fa fa-plus'></i></div>
            <div class='minus' style='display':none'><i class='fa fa-minus'></i></div>
            <!-- iframe accordian -->"
                        );
                    }
                    else if (data.FirstOrDefault().PaymentTransaction.CourierName == "Delhivery")
                    {
                        strShipment.Append(@"<!-- iframe accordian -->
             <div class='one_two_wrap fl_right' style='width: 100%'>
                <div class='widget'>
                    <div class='widget_title min'>
                        <span id=''class='iconsweet'>r</span><h5>Delhivery Tracking</h5>
                    </div>
                    <div class='widget_body max'>
                        <div class='content_pad'>
                           <iframe id = 'load' width='100%' src='https://www.trackingmore.com/delhivery-tracking.html?number=" + data.FirstOrDefault().PaymentTransaction.ShipmentId + @"' height='250'></iframe>
                        </div>
                    </div>
                </div>
            </div>
 <div class='plus'><i class='fa fa-plus'></i></div>
            <div class='minus' style='display':none'><i class='fa fa-minus'></i></div>
            <!-- iframe accordian -->"
);
                    }
                }
            }
            string PaymentMode = "";
            if (data.FirstOrDefault().PaymentTransaction.PaymentMode == "Paytm")
            {
                PaymentMode = "Paytm";
            }
            else if (data.FirstOrDefault().PaymentTransaction.PaymentMode == "Cash On Delivery")
            {
                PaymentMode = "Postpaid";
            }
            else if (data.FirstOrDefault().PaymentTransaction.PaymentMode == "Payumoney")
            {
                PaymentMode = "Payumoney";
            }
            else
            {
                PaymentMode = "Wallet";
            }

            StringBuilder strStatus = new StringBuilder();
            StringBuilder strOrderResult = new StringBuilder();

            if (data.FirstOrDefault().PaymentTransaction.TxnStatus != "SUCCESS")
            {
                if (PaymentMode != "Paytm")
                {
                    strStatus.Append("<input type='button' class='btn btn-orange' name='btnSub' value='Get PaymentGateway Status' onclick='JavaScript: onGetTransDetails_Submit();' />");
                }
            }
            else
            {
                strOrderResult.Append(@"<ul id='progressbar'>");
                if (data.FirstOrDefault().PaymentTransaction.Authorized == true)
                {
                    strOrderResult.Append(@"<li class='active' >
												 <a id='link1' title='Authorized by " + data.FirstOrDefault().PaymentTransaction.CreatedOn.ToShortDateString() + @"'>Authorized <br/> " + data.FirstOrDefault().PaymentTransaction.CreatedOn.ToShortDateString() + @" (0 Days)</a>
											  </li>");
                }
                if (data.FirstOrDefault().PaymentTransaction.Pickup == true)
                {
                    DateTime d1 = data.FirstOrDefault().PaymentTransaction.ShipmentDate.Value;
                    DateTime d2 = data.FirstOrDefault().PaymentTransaction.CreatedOn;

                    TimeSpan t = d1 - d2;
                    double NrOfDays = t.TotalDays;
                    NrOfDays = Math.Round(NrOfDays);

                    strOrderResult.Append(@"<li class='active' title='Pickup by " + data.FirstOrDefault().PaymentTransaction.ShipmentDate.Value.ToShortDateString() + @"'>
												 <a id='link1' title='Pickup by " + data.FirstOrDefault().PaymentTransaction.ShipmentDate.Value + @"'>Pickup <br/>" + data.FirstOrDefault().PaymentTransaction.ShipmentDate.Value.ToShortDateString() + @" (" + NrOfDays + @" Days)</a>
											</li>");
                }
                else
                {
                    strOrderResult.Append(@"<li>   <a id='link1' title='Pickup by " + data.FirstOrDefault().PaymentTransaction.ShipmentDate + @"'>Pickup</a>
													   </li>");
                }
                if (data.FirstOrDefault().PaymentTransaction.Dispatched == true)
                {
                    DateTime? PickupDate = data.FirstOrDefault().PaymentTransaction.PickupDate;

                    if (PickupDate != null)
                    {
                        DateTime d1 = PickupDate.Value;
                        DateTime d2 = data.FirstOrDefault().PaymentTransaction.CreatedOn;

                        TimeSpan t = d1 - d2;
                        double NrOfDays = t.TotalDays;
                        NrOfDays = Math.Round(NrOfDays);

                        strOrderResult.Append(@"<li class='active' title='Dispatched by " + PickupDate.Value.ToShortDateString() + @"'>
												 <a id='link1' title='Dispatched by " + PickupDate.Value.ToShortDateString() + @"'>Dispatched <br/>" + PickupDate.Value.ToShortDateString() + @" (" + NrOfDays + @" Days)</a>
												 </li>");
                    }
                }
                else
                {
                    strOrderResult.Append(@"<li>  <a id='link1' title='Dispatched by " + data.FirstOrDefault().PaymentTransaction.PickupDate + @"'>Dispatched</a>
														  </li>");
                }
                if (data.FirstOrDefault().PaymentTransaction.Delivered == true)
                {
                    DateTime? DispatchedDate = data.FirstOrDefault().PaymentTransaction.DispatchedDate;

                    if (DispatchedDate != null)
                    {
                        DateTime d1 = DispatchedDate.Value;
                        DateTime d2 = data.FirstOrDefault().PaymentTransaction.CreatedOn;

                        TimeSpan t = d1 - d2;
                        double NrOfDays = t.TotalDays;
                        NrOfDays = Math.Round(NrOfDays);

                        strOrderResult.Append(@"<li class='active' title='Dispatched by " + DispatchedDate.Value.ToShortDateString() + @"'>
												 <a id='link1' title='Delivered by " + DispatchedDate.Value.ToShortDateString() + @"'>Delivered<br/> " + DispatchedDate.Value.ToShortDateString() + @" (" + NrOfDays + @" Days)</a>
											</li>");
                    }
                }
                else
                {
                    strOrderResult.Append(@"<li > <a id='link1' title='Delivered by " + data.FirstOrDefault().PaymentTransaction.DispatchedDate + @"'>Delivered</a></li>");
                }
                strOrderResult.Append(@"</ul>");
            }

            strResult.Append(@"" + strprc + @"<div class='one_wrap'>
	<div class='widget'>         
		 <div class='widget_body'>
		  <div class='order_details'>
			   <ul>

			  <li> <span class='field_caption'>Order ID: </span> <span class='field_value'><span>" + data.FirstOrDefault().PaymentTransactionId + @"</span></span> 
					 <div class='fl_right text_right user_details'>
						<p>
						<span class='u_name'>Name : <span>" + data.FirstOrDefault().User.FirstName + @"</span></span><br/>
						<span class='u_name'>Mobile No : <span class='u_phone'><span>" + data.FirstOrDefault().User.MobileNo + @"</span></span><br/>
						<span class='u_name'>E-mail ID : <span>" + data.FirstOrDefault().User.EmailId + @"</span></span><br/>
						<span class='u_name'>Order Status : <span style='font-size:16px; color:red;'>" + status + @"</span></span><br/>
						<span>Payment Mode : <span style='font-size:16px; color:green;'>" + PaymentMode + @"</span></span>                                         
						</p>
					</div>
			  </li>
			  <li> <span class='order_date'><span>" + data.FirstOrDefault().CreatedOn.ToString("dd-MMM-yyyy HH:mm:ss") + @"</span></span><br/>
				   <span>Transaction Status : <span style='font-size:16px; color:Red;font-weight:Bold;'>" + TxnStatus + @"</span></span><br/></li>
				   <span>Message from PG : <span style='font-size:16px; color:green;'>" + TxnMessage + @"</span></span><br/>
				   " + strRefundAmount + @"
			  </li>
			</ul>
		  </div>         
		<div id='dvgrdProducts'>                
					</div>     
		<div class='cp_productlist_view'>
			<div>
		<table class='activity_datatable ordergrd' rules='all' id='grdProducts' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
			<tbody>

<tr>
				<th scope='col'>Product Description</th><th scope='col'>Quantity</th><th scope='col'>Price</th><th scope='col' id='CGST'>CGST</th><th id='igst' scope='col'>SGST</th><th scope='col'>Total Amount</th>
			</tr>
					" + strtd + @"                       
					</tbody></table>
				</div>
				 <div class='horizontal_linetop top_margin2' style='width: 680px;'>

				<div class='fl_right order_summery'style='width: 240px;'>
					<table class='activity_datatable' border='0' cellpadding='8' cellspacing='0' width='100%'>
						<tbody>
			<tr id='trTotalShippingAmount'>
		<td class='field_caption '>
								Shipping Charges :</td>
		<td class='field_value text_right'>
								<span class='WebRupee'>Rs. </span> " + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(data.FirstOrDefault().PaymentTransaction.ShippingCharges)) + @"</span></td>
	</tr>
	 " + strcode + @"                       
					</tbody></table>
					  
	  </div>
	<div class='clear'>
				</div>
			</div>
			 <div class='action_bar' id='dvButtons'>
				   " + strOrderStatus.ToString() + @"
				<input value='Print Invoice' onclick='PrintInvoice()' id='btnInvoice' class='button_small greyishBtn  fl_right' type='button'>   
				 <input value='Print Medicine Invoice' onclick='PrintMedicineInvoice()' id='btnInvoice' class='button_small greyishBtn  fl_right' type='button'>  
				<div class='clear'>
				</div>
			</div>
			 </div>
		 </div>
 </div>");
            if (datas.Count != 0)
            {
                strResult.Append(@"<div class='inlineblock'>
<div class='one_two_wrap fl_left'>
			<div class='widget'>
				<div class='widget_title'><span id='spnBillAddress' class='iconsweet'>r</span>
					<h5>
						Prescription Images
					</h5>
				</div>
				<div class='widget_body'>
				<div class='content_pad' style='padding-bottom: 155px;'>
                                <p>");
                foreach (var items in datas)
                {
                    strResult.Append(@" <div class='product_image fl_left'>
								<img src = " + "https://healthurwealth.com/" + items.Prescription_Image + @"  id='imgProductImage' style='width: 84.989;height: 94.989;padding-left: 20px;'>
                                                  </div>");
                }
            }
            strResult.Append(@"</p>
				</div>
				</div>
			</div>
		</div>
	<!--Block 1 -->
	<div class='one_two_wrap fl_left'style='padding-left: 10px;'>
                        <div class='widget'>
				<div class='widget_title'><span id='spnBillAddress' class='iconsweet'>r</span>
					<h5>
						Billing Address
					</h5>
				</div>
				<div class='widget_body'>
				<div class='content_pad'>
				<p>
						" + data.FirstOrDefault().User.FirstName + @"<br>" + data.FirstOrDefault().UserAddress1.StreetAddress1 + "<br>" + data.FirstOrDefault().UserAddress1.StreetAddress2 + @"<br><span class='hightlight'>" + data.FirstOrDefault().UserAddress1.LandMark + @"</span>," + data.FirstOrDefault().UserAddress1.City + @", " + data.FirstOrDefault().UserAddress1.StateName + @"<span class='hightlight'><br>Contact:" + data.FirstOrDefault().User.MobileNo + @"</span><br>" + data.FirstOrDefault().UserAddress1.Country.CountryName + @"," + data.FirstOrDefault().UserAddress1.PinCode + @"<br>
					   <a id='hypBillUserName' >" + data.FirstOrDefault().User.EmailId + @"</a>
<input type='Text' value=" + data.FirstOrDefault().UserAddress1.StateName + @" id='statename' hidden> " + data.FirstOrDefault().UserAddress1.StateName + @"</div>
				</p>
				</div>
				</div>
			</div>
		</div>
	<!-- Block 1 Ends -->
 <!-- Block 2 -->
		<div class='one_two_wrap fl_right'>
			<div class='widget'>
				<div class='widget_title'><span id='spnShipAddress' class='iconsweet'>r</span><h5>  Shipping Address</h5></div>
				<div class='widget_body'>
				<div class='content_pad'>
				<p>
				 " + data.FirstOrDefault().User.FirstName + @"<br>" + data.FirstOrDefault().UserAddress.StreetAddress1 + "<br>" + data.FirstOrDefault().UserAddress.StreetAddress2 + @"<br><span class='hightlight'>" + data.FirstOrDefault().UserAddress.LandMark + @"</span>," + data.FirstOrDefault().UserAddress.City + @", " + data.FirstOrDefault().UserAddress.StateName + @"<span class='hightlight'><br>Contact:" + data.FirstOrDefault().User.MobileNo + @"</span><br>" + data.FirstOrDefault().UserAddress.Country.CountryName + @"," + data.FirstOrDefault().UserAddress.PinCode + @"<br>" + data.FirstOrDefault().User.EmailId + @"<br>                 
				</p>
				</div>
				</div>
			</div>
		</div>  
	<!-- Block 2 ends here-->
<!-- Block 3 -->
		<div class='one_two_wrap fl_left'>
			<div class='widget'>
				<div class='widget_title'><span id='spnShipAddress' class='iconsweet'>r</span><h5>  Transaction Details</h5></div>
				<div class='widget_body'>
				<div class='content_pad'>
				<p>
				Payment Gateway: <b>" + PaymentMode + "</b><br/> Pg TxnId:<b>" + data.FirstOrDefault().PaymentTransaction.PGTxnId + "</b><br/> Status:<b>" + data.FirstOrDefault().PaymentTransaction.TxnStatus + "</b><br/> Pg Ref No:<b>" + data.FirstOrDefault().PaymentTransaction.TxnRefNo + "</b><br/> Txn Date:<b>" + data.FirstOrDefault().PaymentTransaction.UpdatedOn + @"</b>                 
				</p>
				</div>
				</div>
			</div>
		</div>  
	<!-- Block 3 ends here-->


			<br/>" + strStatus.ToString() + @"<br/><table class='list'><thead><tr><td class='left'><b>Order Process</b></td></tr></thead>
<tbody><tr><td class='left'><div class='divprocess'>" + strOrderResult + @"</div></td></tr>
</tbody></table>
" + strOrderComments.ToString() + @"
</div>");
            if (newdata.Count != 0)
            {
                strResult.Append(@"<div class='one_one_wrap fl_left'>");

                strResult.Append(@"<div class='widget'>");

                strResult.Append(@"<div class='widget_title' style='width: 676.156;'><span id = 'spnShipAddress' class='iconsweet'>r</span><h5>  Log History</h5></div>");
                strResult.Append(@"<div class='widget_body'>");
                strResult.Append(@"<div class='content_pad' style='padding - right: 15px;height: 225px;width: 651.497;'>");
                foreach (var item in newdata)
                {
                    strResult.Append(@"<p>
                " + item.OrderStatusname + " " + item.ReferenceNO + " by " + item.Createdby + " on " + item.Createdon + @"
                
                                </p>");
                }
                strResult.Append(@"</div>
				</div>
			</div>
		</div>  ");
            }
            strResult.Append(@"" + strShipment.ToString());

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strResult.ToString()
            };
        }
        else
        {
            StringBuilder strRlt = new StringBuilder();
            strResult.Append(@" < div id='ctl00_ctl00_Main_Main_divMessage' class='msgbar msg_Info hide_onC'>
								<span id='ctl00_ctl00_Main_Main_msgicon' class='iconsweet'>*</span>
								<p><span id='ctl00_ctl00_Main_Main_lblMessage'>There are no 'Orders' found.</span></p>
							</div>");

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = strRlt.ToString()
            };
        }

    }
    public CustomResponse GetProductOverviewforpres(int transId)
    {
        var repositorys = new ImagerespoRepository();
        var repository = new UserProductTransactionRepository();
        var newrepository = new LogRepository();
        int totalRecords;
        var newdata = newrepository.FetchAllByPage(x => x.Paymenttransactionid, out totalRecords, 0, 0, (p => p.Paymenttransactionid == transId), null, "", true);
        var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, 0, (p => p.PaymentTransactionId == transId), null, "", true);
        var datas = repositorys.FetchAllByPage(x => x.Payment_Transaction_Id, out totalRecords, 0, 0, (p => p.Payment_Transaction_Id == transId), null, "", true);

        StringBuilder strResult = new StringBuilder();
        StringBuilder strprc = new StringBuilder();

        StringBuilder strtd = new StringBuilder();
        StringBuilder strRefundAmount = new StringBuilder();
        db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();
        if (data.Count > 0)
        {
            var TxnStatus = "";
            var TxnMessage = "";
            var RefundAmount = "";
            DateTime CreatedOn = data.FirstOrDefault().PaymentTransaction.CreatedOn;
            DateTime CurrentDate = DateTime.Now;
            TimeSpan span = CurrentDate.Subtract(CreatedOn);

            string ProductName = "";
            decimal ProductCost = 0;

            if (span.Minutes > 15 && data.FirstOrDefault().PaymentTransaction.TxnStatus == null && data.FirstOrDefault().PaymentTransaction.TxnMessage == null)
            {
                TxnStatus = "No response from Payment Gateway";
                TxnMessage = "No response from Payment Gateway";
            }
            else
            {
                TxnStatus = data.FirstOrDefault().PaymentTransaction.TxnStatus;
                TxnMessage = data.FirstOrDefault().PaymentTransaction.TxnMessage;
            }

            string statusvalue = data.FirstOrDefault().PaymentTransaction.OrderCurrentStatus.ToString();
            var status = Shared.GetOrderStatusEnum(statusvalue);
            if (data.FirstOrDefault().PaymentTransaction.DoctorName != null)
            {
                strprc.Append(@"<input type='text' id='txtdoctorname' value='" + data.FirstOrDefault().PaymentTransaction.DoctorName + @"' /><input type='button' onclick=updatedoctorname('" + data.FirstOrDefault().PaymentTransactionId + "')/>");

            }
            else if (data.FirstOrDefault().Isprescription == true)
            {
                strprc.Append(@"<input type='text' id='txtdoctorname' /><input type='button' onclick=updatedoctorname('" + data.FirstOrDefault().PaymentTransactionId + "')/>");
            }
            strtd.Append(@"<div class='order_details'>
			   <ul>

			  <li> <span class='field_caption'>Order ID: </span> <span class='field_value'><span>" + data.FirstOrDefault().PaymentTransactionId + @"</span></span> 
					 <div class='fl_right text_right user_details'>
						<p>
						<span class='u_name'>Name : <span>" + data.FirstOrDefault().User.FirstName + @"</span></span><br/>
						<span class='u_name'>Mobile No : <span class='u_phone'><span>" + data.FirstOrDefault().User.MobileNo + @"</span></span><br/>
						<span class='u_name'>E-mail ID : <span>" + data.FirstOrDefault().User.EmailId + @"</span></span><br/>
						<span class='u_name'>Order Status : <span style='font-size:16px; color:red;'>" + status + @"</span></span><br/>
					                                         
						</p>
					</div>
			  </li>
			  <li> <span class='order_date'><span>" + data.FirstOrDefault().CreatedOn.ToString("dd-MMM-yyyy HH:mm:ss") + @"</span></span><br/>
				   <span>Transaction Status : <span style='font-size:16px; color:Red;font-weight:Bold;'>" + TxnStatus + @"</span></span><br/></li>
				   <span>Message from PG : <span style='font-size:16px; color:green;'>" + TxnMessage + @"</span></span><br/>
				   " + strRefundAmount + @"
			  </li>
			</ul>
		  </div>
<div class='one_two_wrap fl_left'>
			<div class='widget'>
				<div class='widget_title'><span id='spnBillAddress' class='iconsweet'>r</span>
					<h5>
						Billing Address
					</h5>
				</div>
				<div class='widget_body'>
				<div class='content_pad'>
				<p>
						" + data.FirstOrDefault().User.FirstName + @"<br>" + data.FirstOrDefault().UserAddress1.StreetAddress1 + "<br>" + data.FirstOrDefault().UserAddress1.StreetAddress2 + @"<br><span class='hightlight'>" + data.FirstOrDefault().UserAddress1.LandMark + @"</span>," + data.FirstOrDefault().UserAddress1.City + @", <span class='hightlight'>" + data.FirstOrDefault().UserAddress1.StateName + @"</span><br>" + data.FirstOrDefault().UserAddress1.PinCode + @"<br>" + data.FirstOrDefault().UserAddress1.Country.CountryName + @"<br>Ship Mobile:" + data.FirstOrDefault().User.MobileNo + @"<br>
					   <br>
					   <a id='hypBillUserName' >" + data.FirstOrDefault().User.EmailId + @"</a>
				</p>
				</div>
				</div>
			</div>
		</div>
	<!-- Block 1 Ends -->
 <!-- Block 2 -->
		<div class='one_two_wrap fl_right'>
			<div class='widget'>
				<div class='widget_title'><span id='spnShipAddress' class='iconsweet'>r</span><h5>  Shipping Address</h5></div>
				<div class='widget_body'>
				<div class='content_pad'>
				<p>
				 " + data.FirstOrDefault().User.FirstName + @"<br>" + data.FirstOrDefault().UserAddress.StreetAddress1 + "<br>" + data.FirstOrDefault().UserAddress.StreetAddress2 + @"<br><span class='hightlight'>" + data.FirstOrDefault().UserAddress.LandMark + @"</span>," + data.FirstOrDefault().UserAddress.City + @", <span class='hightlight'>" + data.FirstOrDefault().UserAddress.StateName + @"</span><br>" + data.FirstOrDefault().UserAddress.PinCode + @"<br>" + data.FirstOrDefault().UserAddress.Country.CountryName + @"<br>Ship Mobile:" + data.FirstOrDefault().User.MobileNo + @"<br>" + data.FirstOrDefault().User.EmailId + @"<br>
				 
				</p>
				</div>
				</div>
			</div>
		</div>  
	<!-- Block 2 ends here-->
	</div>
	
");



            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strtd.ToString()
            };
        }
        else
        {
            StringBuilder strRlt = new StringBuilder();
            strResult.Append(@" < div id='ctl00_ctl00_Main_Main_divMessage' class='msgbar msg_Info hide_onC'>
								<span id='ctl00_ctl00_Main_Main_msgicon' class='iconsweet'>*</span>
								<p><span id='ctl00_ctl00_Main_Main_lblMessage'>There are no 'Orders' found.</span></p>
							</div>");

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = strRlt.ToString()
            };
        }

    }

    [HttpGet]
    public CustomResponse SendOtpToMakeOrderSuccess(int? transId)
    {
        string Url = WebConfigurationManager.AppSettings["SmsUrl"].ToString();
        string UserName = WebConfigurationManager.AppSettings["SmsId"].ToString();
        string password = WebConfigurationManager.AppSettings["SmsPwd"].ToString();

        Random r = new Random();
        int OTP = r.Next(100000, 999999);
        string Message = "OTP " + OTP + " for make Payment Success for OrderId " + transId + "(SONAL ENTERPRISES)";
        HttpContext.Current.Session["OrderOTP"] = OTP;
        SendMessage(Message, "9701830011");
        Utility.MailMessage ms = new Utility.MailMessage();
        ms.Subject = "Make order success";
        ms.To = "ravinder@zoninnovative.com";

        string domain;
        domain = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port;

        string body = "Otp to make order success " + OTP;
        ms.Body = body;
        ms.IsBodyHtml = false;

        try
        {
            ms.SendMail();
        }
        catch (Exception ex)
        {

        }
        string smsStatus = MailMessage.SendSms(Url, UserName, password, 9701830011, Message, "N", "1707160960527286800");
        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Result = ""
        };
    }
    public static void SendMessage(string Message, string To)
    {
        try
        {
            string url = "http://localhost:6001/chat/sendmessage/91" + Convert.ToInt64(To.Replace("+", ""));
            //string url = "http://localhost:" + servernumber + "000/chat/sendmessage/" + To + "/" + Message;
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            NrmlMessage msg = new NrmlMessage();
            msg.message = Message;
            var body = JsonConvert.SerializeObject(msg);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            client.Execute(request);
        }
        catch (Exception ex)
        {

        }
    }

    [HttpGet]
    public CustomResponse SendOtpToMakeCheckoutOrderSuccess(int? transId)
    {
        string Url = WebConfigurationManager.AppSettings["SmsUrl"].ToString();
        string UserName = WebConfigurationManager.AppSettings["SmsId"].ToString();
        string password = WebConfigurationManager.AppSettings["SmsPwd"].ToString();

        Random r = new Random();
        int OTP = r.Next(100000, 999999);
        string Message = "OTP " + OTP + " for make Checkout Order Success for OrderId " + transId + "(SONAL ENTERPRISES)";
        HttpContext.Current.Session["CheckoutOrderOTP"] = OTP;
        string smsStatus = MailMessage.SendSms(Url, UserName, password, 9701830011, Message, "N", "1707160960527286800");
        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Result = ""
        };
    }


    [HttpGet]
    public CustomResponse PendingtoSuccessOrder(int? transId, string PgTxnId, string OTP, string payMode)
    {
        if (HttpContext.Current.Session["OrderOTP"].ToString() == OTP.ToString())
        {
            PaymentTransactionRepository reposiry = new PaymentTransactionRepository();
            string data = reposiry.MakeOrderStatusChange(transId.Value, PgTxnId, payMode);
        }
        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Result = ""
        };
    }

    [HttpGet]
    public CustomResponse CheckouttoSuccessOrder(int? transId, string PgTxnId, string OTP, string payMode)
    {
        if (HttpContext.Current.Session["CheckoutOrderOTP"].ToString() == OTP.ToString())
        {
            PaymentTransactionRepository reposiry = new PaymentTransactionRepository();
            string data = reposiry.CheckoutOrderSuccess(transId.Value, PgTxnId, payMode);
        }
        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Result = ""
        };
    }

    [HttpGet]
    public CustomResponse CheckoutOrderClose(int? transId)
    {
        PaymentTransactionRepository reposiry = new PaymentTransactionRepository();
        string data = reposiry.CheckoutOrderClose(transId.Value);
        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Result = ""
        };
    }

    [HttpGet]
    public CustomResponse CancelOrder(int? transid)
    {
        var x = BalUtility.Logmaintainance((int)Shared.OrderStatus.Cancelled, "Cancelled", "", transid.ToString());
        var repository = new PaymentTransactionRepository();
        PaymentTransaction updatedata = repository.First(p => p.PaymentTransactionId == transid);
        if (updatedata != null)
        {
            updatedata.Authorized = false;
            updatedata.TxnMessage = "Cancelled by Admin";
            updatedata.TxnStatus = "Cancelled";
            updatedata.OrderCurrentStatus = 1;
            repository.Update(updatedata);

            OrderCancelledByAdmin(updatedata, transid.ToString());

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString()
            };
        }
        else
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString()
            };
        }

    }
    [HttpGet]
    public CustomResponse Refundorder(string IDS, string ShipmentID, string Reason)
    {
        db_Zon_HuwEntities context = new db_Zon_HuwEntities();
        //var x = BalUtility.Logmaintainance((int)Shared.OrderStatus.Returns, "Return", ShipmentID, IDS);
        PaymentTransaction paymentTransaction = new PaymentTransaction();
        paymentTransaction.OrdersReturnAction = ShipmentID;
        var repository = new PaymentTransactionRepository();
        var repositorys = new userproducttransactionRepository();
        var repositoryss = new ProductRepository();
        long idss = Convert.ToInt64(IDS);
        PaymentTransaction updatedata = repository.First(p => p.PaymentTransactionId == idss);

        UserProductTransaction utrans = repositorys.First(p => p.PaymentTransactionId == idss);
        Product prdt = repositoryss.First(p => p.ProductId == utrans.ProductId);
        if (updatedata != null)
        {
            if (updatedata.OrderCurrentStatus == (int)Shared.OrderStatus.Delivered)
            {
                var x = BalUtility.Logmaintainance((int)Shared.OrderStatus.Returns, "DR-Return after delivered", ShipmentID, IDS);
            }
            else
            {
                var x = BalUtility.Logmaintainance((int)Shared.OrderStatus.Returns, "Return", ShipmentID, IDS);
            }
            updatedata.OrderCurrentStatus = (int)Shared.OrderStatus.Returns;
            updatedata.TxnStatus = "RETURNED";
            updatedata.TxnMessage = "Returned by Admin";
            updatedata.OrdersReturnReason = Reason;
            updatedata.OrdersReturnAction = ShipmentID;
            repository.Update(updatedata);
        }
        if (utrans != null)
        {

            prdt.Quantity = prdt.Quantity + utrans.Quantity;
            repositoryss.Update(prdt);


        }
        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString()
        };
    }
    //else
    //{
    //    return new CustomResponse
    //    {
    //        Status = Shared.ResponseStatus.Fail.ToString()
    //    };
    //}

    //}
    public CustomResponse CommentOrder(int OrderID, string Comment)
    {
        var repository = new CommentRepository();

        long CommentStatus = repository.OrderComment(OrderID, Comment, 160);

        if (CommentStatus != 0)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString()
            };
        }
        else
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString()
            };
        }
    }

    public CustomResponse CheckoutCommentOrder(int OrderID, string Comment, string OrderStatus)
    {
        var repository = new CheckoutCommentRepository();

        long CommentStatus = repository.CheckoutOrderComment(OrderID, Comment, OrderStatus, 160);

        if (CommentStatus != 0)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString()
            };
        }
        else
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString()
            };
        }
    }

    public CustomResponse GetOrderComments(int trnsId)
    {
        var repository = new CommentRepository();
        var data = repository.GetOrderComments(trnsId);

        return new CustomResponse
        {
            Status = Shared.ResponseStatus.NoData.ToString(),
            Result = data
        };
    }
    public static void OrderCancelledByAdmin(PaymentTransaction data, string PrdctStatsSubject)
    {
        try
        {
            string ApiUrl = System.Configuration.ConfigurationManager.AppSettings["ApiUrl"];

            var repository = new PaymentTransactionRepository();
            var PaymntDtls = repository.First(p => p.PaymentTransactionId == data.PaymentTransactionId);

            var rep = new UserProductTransactionRepository();
            var OrderdPrdcts = rep.First(o => o.PaymentTransactionId == PaymntDtls.PaymentTransactionId);

            var Repsitory = new UserRepository();
            var UsrDtls = Repsitory.First(u => u.UserId == PaymntDtls.User.UserId);

            int Transactionid = Convert.ToInt32(PaymntDtls.PaymentTransactionId);

            Utility.MailMessage ms = new Utility.MailMessage();
            ms.Subject = "Cancellation of your order " + PaymntDtls.PaymentTransactionId + " from HealthurWealth.com";
            ms.To = UsrDtls.EmailId;

            string domain;
            Uri url = HttpContext.Current.Request.Url;
            domain = url.AbsoluteUri.Replace(url.PathAndQuery, string.Empty);

            string body = string.Empty;
            string htmlPagePath = Shared.GethtmlPage(Shared.ProductStatusForSendMail.AdminOrderCancel);
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("" + htmlPagePath + "")))
            {
                body = reader.ReadToEnd();
            }

            db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();
            string PromocodeData = "";
            string PromocodeAmount = "";
            if (PaymntDtls.Has_Promo_Code == true)
            {
                Tbl_Promo_Codes PromoData = Entity.Tbl_Promo_Codes.Where(x => x.Promo_Code_ID == PaymntDtls.Promo_Code_ID).First();
                PromocodeData = "Your Applied Promocode : <span style='color:green;font-weight:bold;'>" + PromoData.PromoCode + "</span>";
                PromocodeAmount = "<tr><td colspan='2' style='margin:0px;padding:16px 0px 14px 16px;border-collapse:collapse;border-spacing:0px;text-align:right;font-size:14px;line-height:22px;color:#a1a2a5;border-bottom-width:1px;border-bottom-style:solid;border-bottom-color:#ebebeb;vertical-align:top;background-color:#f6f6f7'>Promocode Amount</td><td style='margin:0px;padding:16px 16px 14px;border-collapse:collapse;border-spacing:0px;text-align:right;font-size:14px;line-height:22px;color:#a1a2a5;border-bottom-width:1px;border-bottom-style:solid;border-bottom-color:#ebebeb;vertical-align:top;background-color:#f6f6f7;width:50px;'><span style='color:#54565c'>₹  " + PromoData.Amount + "</span></td></tr>";
            }
            body = body.Replace("##PromocodeData##", PromocodeData);
            body = body.Replace("##PromocodeAmount##", PromocodeAmount);
            body = body.Replace("##FirstName##", UsrDtls.FirstName);
            body = body.Replace("##OrderNo##", PaymntDtls.PaymentTransactionId.ToString());
            body = body.Replace("##PaymentMode##", PaymntDtls.PaymentMode);
            body = body.Replace("##OrderDate##", DateTime.Now.ToShortDateString());
            body = body.Replace("##StreetAddress1##", (Convert.ToString(UsrDtls.UserProductTransactions.FirstOrDefault().UserAddress.StreetAddress1)));
            body = body.Replace("##StreetAddress2##", (Convert.ToString(UsrDtls.UserProductTransactions.FirstOrDefault().UserAddress.StreetAddress2)));
            body = body.Replace("##LandMark##", (Convert.ToString(UsrDtls.UserProductTransactions.FirstOrDefault().UserAddress.LandMark)));
            body = body.Replace("##City##", (Convert.ToString(UsrDtls.UserProductTransactions.FirstOrDefault().UserAddress.City)));
            body = body.Replace("##State##", (Convert.ToString(UsrDtls.UserProductTransactions.FirstOrDefault().UserAddress.StateName)));
            body = body.Replace("##PinCode##", (Convert.ToString(UsrDtls.UserProductTransactions.FirstOrDefault().UserAddress.PinCode)));

            var dataa = BalUtility.GetCart();
            var dataaa = PaymntDtls.UserProductTransactions.ToList();
            StringBuilder strtrResult = new StringBuilder();
            var cartItems = (List<UserProductTransaction>)BalUtility.GetSession(Shared.Sessions.CustomerCart) ??
                                new List<UserProductTransaction>();

            var count = 0;
            var totalsum = cartItems.Sum(p => p.ProductCost);
            var orderSummary = (Caluclations)BalUtility.GetSession(Shared.Sessions.ShippingInfo);
            var calculations = BalUtility.GenerateOrderSummary();

            var PreviousTotal = 0;
            var Total = 0;
            var GrandTotal = 0;

            foreach (var item in dataaa)
            {

                var sid = item.SubProductId == null ? 0 : item.SubProductId;
                strtrResult.Append(@"<tr>
									<td class='alignLeft prodImg'><img src='" + item.Product.ProductImgUrl.Replace("~/", ApiUrl) + @"' style='width:80px;height:80px;' alt='Title' /></td>
									<td class='alignLeft prodDesc'>
										<h4 style='-webkit-margin-after: 5px;'>" + item.Product.ProductName + @" </h4>
										Quantity: " + item.Quantity + @" <br />
										
									</td>
									<td class='alignRight'><span class='amount'>&#8377;" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.Quantity * item.Product.ProductCost), true) + @" </span></td>
								</tr>");
                Total = Convert.ToInt32(item.Quantity * item.Product.ProductCost);

                GrandTotal = PreviousTotal + Total;
                PreviousTotal = GrandTotal;
            }
            StringBuilder strMsg = new StringBuilder();

            if (GrandTotal >= 1000)
            {
                var ShippingCharges = 0;
                GrandTotal = GrandTotal + 0;
                body = body.Replace("##ShippingCharges##", PaymntDtls.ShippingCharges.ToString());
            }
            else if (GrandTotal < 1000)
            {
                var ShippingCharges = 50;
                GrandTotal = GrandTotal + ShippingCharges;
                body = body.Replace("##ShippingCharges##", PaymntDtls.ShippingCharges.ToString());
            }
            body = body.Replace("##ProductList##", strtrResult.ToString());
            body = body.Replace("##Total##", Total.ToString());
            ms.Body = body.Replace("##GrandTotal##", PaymntDtls.TxnAmount.ToString());
            ms.IsBodyHtml = true;
            try
            {
                ms.SendMail();

                string Message = "Your order no :" + PaymntDtls.PaymentTransactionId + "   has been cancelled due to the product you ordered is out of stock . Sorry for inconvenience. Healthurwealth.com (SONAL ENTERPRISES)";
                string Url = WebConfigurationManager.AppSettings["SmsUrl"].ToString();
                string UserName = WebConfigurationManager.AppSettings["SmsId"].ToString();
                string password = WebConfigurationManager.AppSettings["SmsPwd"].ToString();
                string Status = Utility.MailMessage.SendSms(Url, UserName, password, UsrDtls.MobileNo, Message, "N", "1707160960539510544");
                //string Statuss = Utility.MailMessage.SendSms(Url, UserName, password, 9246544001, Message, "N", "1707160960539510544");
            }
            catch (Exception ex)
            {

            }
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            throw ex;
        }
    }

    public CustomResponse GetReturnOrders(int trnsId)
    {
        var repository = new UserProductTransactionRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.TransactionId, out totalRecords, 0, 0, (p => p.TransactionId == trnsId && p.IsReplaced == false && p.IsDeleted == false && p.IsActive == true), null, "", true);
        StringBuilder strResult = new StringBuilder();
        StringBuilder strtd = new StringBuilder();

        if (data.Count > 0)
        {
            foreach (var item in data)
            {
                strtd.Append(@"><tr>
				<td>" + item.TransactionId + @"</td><td>" + item.Product.ProductName + @"</td><td>" + item.Quantity + @"</td><td>
										
										<span id='lblPrice'><span class='WebRupee'>Rs. </span>" + item.ProductCost + @"</span>
									</td><td>
										<div id='ChkSelect'><span><input  id='ChkSelect'  type='checkbox' class='Check' onclick='CheckboxSelect(this)'></span></div>
									</td>
			</tr>");
            }
            strResult.Append(@"<div id='ctl00_ctl00_Main_Main_OrderDetails'>
	<div class='one_two_wrap fl_left'>
		<div class='widget'>
				<div class='widget_title'><span class='iconsweet'></span><h5>Billing Address</h5></div>
				<div class='widget_body'>
				<ul class='form_fields_container padd_LR'>
								<li> <span id='lblresBillingName'>" + data.FirstOrDefault().User.FirstName + @"</span></li>
								<li>" + data.FirstOrDefault().User.FirstName + @"<br>" + data.FirstOrDefault().UserAddress.StreetAddress1 + @"<br>" + data.FirstOrDefault().UserAddress.StreetAddress2 + @"<br>" + data.FirstOrDefault().UserAddress.LandMark + @"<br>" + data.FirstOrDefault().UserAddress.City + @"<br>" + data.FirstOrDefault().UserAddress.PinCode + @"<br>" + data.FirstOrDefault().UserAddress.StateName + @"<br>" + data.FirstOrDefault().UserAddress.Country.CountryName + @"<br> </li>
				</ul>
				</div>
				</div>
	</div>
</div>
	<div class='one_two_wrap fl_right'>
				  <div class='widget'>
				<div class='widget_title'><span class='iconsweet'></span><h5>Shipping Address</h5></div>
				<div class='widget_body'>
				<ul class='form_fields_container padd_LR' '=''>
								<li><span id='lblresShippingName'>" + data.FirstOrDefault().User.FirstName + @"</span>
									</li>
								<li>
								   <span id='lblresShippingCourrrier'>" + data.FirstOrDefault().PaymentTransaction.CourierName + @"</span>
									</li>
								<li>" + data.FirstOrDefault().User.FirstName + @"<br>" + data.FirstOrDefault().UserAddress1.StreetAddress1 + @"<br><br>" + data.FirstOrDefault().UserAddress1.StreetAddress2 + @"<br>" + data.FirstOrDefault().UserAddress1.LandMark + @"<br>" + data.FirstOrDefault().UserAddress1.City + @"<br>" + data.FirstOrDefault().UserAddress1.PinCode + @"<br>" + data.FirstOrDefault().UserAddress1.StateName + @"<br>" + data.FirstOrDefault().UserAddress1.Country.CountryName + @"<br>" + data.FirstOrDefault().User.EmailId + @"<br>
								   </li>
							  
								<li>
									<span id='lblresShippingDate'>" + data.FirstOrDefault().PaymentTransaction.ShipmentDate + @"</span>
									</li>
				</ul>
			  
				</div>
				</div>
	</div>	
   </div>
   <div class='clear'></div>
		<div id='ctl00_ctl00_Main_Main_updRetProducts'>
	
					<div id='ctl00_ctl00_Main_Main_divOrderedItems'>
						<div class='widget'>
						<div class='widget_title'><span class='iconsweet'>r</span><h5>Ordered Product(s)</h5></div>
						<div class='widget_body'>
						
						<div>
		<table rules='all' class='activity_datatable' id='ctl00_ctl00_Main_Main_gvwReturnPRoducts' style='width:100%;border-collapse:collapse;' border='1' cellspacing='0'>
			<tbody><tr>
				<th scope='col'>Order Id</th><th scope='col'>Product Name</th><th scope='col'>Order Qty</th><th scope='col'>Unit Price</th><th scope='col'>Select Return Action</th>
			</tr
" + strtd + @"
		</tbody></table>
	</div>  <table class='activity_datatable' width='100%'>
							<tbody><tr id='TotalShippingAmount'>
		<td width='16%'></td>
		<td class='text_right text_bold'>
								  
										Total Shipping Amount :
								</td>
		<td class='text_right '>
								   
										<span id='lblTotalShippingAmount'><span class='WebRupee'>Rs. </span>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(data.FirstOrDefault().PaymentTransaction.ShippingCharges)) + @"</span>
									
								</td>
	</tr>
	
							
							<tr>
							<td width='16%'></td>
								<td class='text_right text_bold'>
									
										Total
								</td>
								<td class='text_right'>
									
										<span id='lblGrandTotalPrice'><span class='WebRupee'>Rs. </span></span>" + data.FirstOrDefault().ProductCost + @"
									
								</td>
							</tr>
						</tbody></table>
					
						</div>
						</div>
					</div>
  
</div>");

            StringBuilder strResult2 = new StringBuilder();
            StringBuilder stritemtd = new StringBuilder();
            foreach (var item in data)
            {
                stritemtd.Append(@"
<tr>
			<td>" + item.TransactionId + @"</td><td>" + item.Product.ProductName + @"</td><td>
										<div class='form_input'> <input  value= " + item.Quantity + @" id='txtOrderQty' tabindex='1' class='oneThree'  onkeyup='' type='text'></div>
									</td><td>
										<span id='lblSelectedPrice' style='display:inline-block;width:20px;'>" + item.ProductCost + @"</span>
									</td><td>
										<span id='lblSelectedTotal' style='display:inline-block;width:20px;'>" + item.ProductCost + @"</span>
									</td><td>
								  
										<div id='ddlReturnReason' class='selector'><span style='-moz-user-select: none;'>Received Damaged</span>
<select  name='ddlReturnReason' id='ddlReturnReason'>
				<option value='RD'>Received Damaged</option>
				<option value='DP'>Defective Product</option>
				<option value='II'>Incorrect Item received</option>
				<option value='UW'>Unwanted Gift</option>
				<option value='US'>Unsuitable</option>
				<option value='UP'>Unlike Photo</option>
				<option value='TS'>Too Small</option>
				<option value='OT'>Other</option>
				<option value='FD'>Fraud</option>
				<option value='IE'>Incorrect Size</option>

			</select>
										</div>
									</td><td>
										<div class='form_input'><div id='ddlReturnAction' class='selector'><span style='-moz-user-select: none;'>Refund</span><select   id='ddlReturnAction' class='uniform_pager_list'>
				<option selected='selected' value='1'>Refund</option>
				<option value='2'>Replacement</option>
				<option value='3'>Exchange</option>

			</select></div></div>
									</td><td>
										<input  id='hdnBackOrderSelected' value='False' type='hidden'>
										<div class='form_input'><div id='ddlStockAction' class='selector'><span style='-moz-user-select: none;'>Update Stock</span><select   id='ddlStockAction' class='uniform_pager_list'>
				<option selected='selected' value='US'>Update Stock</option>
				<option value='DS'>Do Not UpdateStock</option>
				<option value='NO'>None</option>

			</select></div></div>
									</td><td>
										<div class='form_input'><input  id='txtComments' style='width:240px;' type='text'></div>
									</td>
		</tr>

");
            }
            strResult2.Append(@"
" + strResult + @"
				<div id='divgvwReturnProductsSelected'>
						<div class='widget'>
						<div class='widget_title'><span class='iconsweet'>r</span><h5><label id='hdReturnItem'>Returned Product(s)</label></h5></div>
						<div class='widget_body'>
						<div class='horizontal_scroll'>
						<div>
	<table rules='all' class='activity_datatable' id='gvwReturnProductsSelected' style='width:100%;border-collapse:collapse;' border='1' cellspacing='0'>
		<tbody><tr>
			<th scope='col'>Product Id</th><th scope='col'>Product Name</th><th scope='col'>Return Qty</th><th scope='col'>Unit Price</th><th scope='col'>Total</th><th scope='col'>Return Reason</th><th scope='col'>Return Action</th><th scope='col'>Stock Action</th><th scope='col'>Comments</th>
		</tr>
" + stritemtd + @"
	</tbody></table>
</div>
						</div>
						</div>
						</div>
					</div>
</div>
		 

		
		<div id='ctl00_ctl00_Main_Main_btnglobalbtn' class='action_bar text_right'>

				<input  value='Process Return' id='btnProcessReturn' onclick='UpdateReturnProductAction(" + trnsId + @")' class='button_small bluishBtn fl_right' style='display: block;' type='button'>
				<input  value='Cancel' onclick='return ClearAll();' id='btnCancel' class='button_small greyishBtn' type='submit'>
		  
			</div>

");

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strResult2.ToString()

            };

        }
        else
        {
            StringBuilder strerror = new StringBuilder();
            strerror.Append(@"<div id='divMessage' class='msgbar msg_Info hide_onC'>
<span class='iconsweet'>
<span id='lblspan'>*</span>
</span>
<p>
<span id='lblMessage'>Order No. ‘" + trnsId + @"’ is not authorized to return.</span>
</p>
</div>");
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = strerror.ToString()
            };
        }

    }

    public CustomResponse UpdateReturnProductAction(UserProductTransaction objPaymentTrans)
    {
        try
        {
            var repository = new UserProductTransactionRepository();
            var updatedata = repository.First(p => p.TransactionId == objPaymentTrans.TransactionId);

            updatedata.OrdersReturnReason = objPaymentTrans.OrdersReturnReason;
            updatedata.OrdersReturnAction = objPaymentTrans.OrdersReturnAction;

            repository.Update(updatedata);


            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = objPaymentTrans.TransactionId
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };

        }

    }

    [HttpGet]
    public CustomResponse EditReplaceOrders(int trnsId)
    {
        try
        {
            var repository = new UserProductTransactionRepository();
            int totalRecords;
            var data = repository.FetchAllByPage(p => p.TransactionId, out totalRecords, 0, 0, (p => p.TransactionId == trnsId && p.IsDeleted == false && p.IsActive == true && p.IsReplaced == false), null, "", true).FirstOrDefault();
            if (data != null)
            {
                var data2 = new UserProductTransaction
                {
                    TransactionId = data.TransactionId,
                    Quantity = data.Quantity,
                    ProductCost = data.ProductCost,
                };
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = "",
                    Result = data2

                };
            }
            else
            {
                StringBuilder strErrormessage = new StringBuilder();
                strErrormessage.Append(@"
<div id='diverrormessage' class='msgbar msg_Error hide_onC' style='display: block;'>
<span class='iconsweet'>X</span>
Searched TransactionId has not found
</div>
");
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.NoData.ToString(),
                    Result = strErrormessage.ToString()

                };
            }

        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Result = null,
                Message = ex.Message
            };
        }
    }

    public CustomResponse GetProcessReturnsDetails(int trnsId)
    {
        var repository = new UserProductTransactionRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.TransactionId, out totalRecords, 0, 0, (p => p.TransactionId == trnsId), null, "", true);
        StringBuilder strResult = new StringBuilder();
        StringBuilder strtd = new StringBuilder();
        foreach (var item in data)
        {
            strtd.Append(@"<tr>
					<td>" + item.TransactionId + @"</td><td>" + item.Product.ProductName + @"</td><td>1</td><td>
												<span id='lblSingleProductCost'><span class='WebRupee'>Rs. </span>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.ProductCost), true) + @"</span>
											</td><td>
												
												
												
												
												
												<span id='lblTotalCost'><span class='WebRupee'>Rs. </span>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.ProductCost), true) + @"</span>
												
												
											</td><td>
												
												<span id='lblReturnReason'>" + item.OrdersReturnReason + @"</span>
											</td><td>
												
												<span id='lblReturnAction'>" + item.OrdersReturnAction + @"</span>
											</td>
				</tr>");
        }
        strResult.Append(@"

		 <div>
				   <div class='widget'>
			<div class='widget_title'>
				<span class='iconsweet'>r</span>
				<h5 id='legUpsDetails'> <label id='lblSubTitle'>Returned Product(s)</label> </h5>
				</div>
			<div class='widget_body'>

					
								<div>
			<table rules='all' class='activity_datatable' id='grdreturn' style='width:100%;border-collapse:collapse;' border='1' cellspacing='0'>
				<tbody><tr>
					<th scope='col'>Order Id</th><th scope='col'>Product Title</th><th scope='col'>Quantity</th><th scope='col'><div id='divProductCost'>Product Cost</div></th><th scope='col'><div id='divTotal'>Total</div></th><th scope='col'>Return Reason</th><th scope='col'>Return Action</th>
				</tr>
	" + strtd + @"
			</tbody></table>
		</div>
							</div>
					 
					</div>
				
	</div>
</div>");
        StringBuilder strRetunsval = new StringBuilder();
        StringBuilder strreturntd = new StringBuilder();

        foreach (var item in data)
        {
            var Balanceamount = item.ProductCost - item.ProductCost;
            strreturntd.Append(@"
  <tr>
										<td>
											<span id='lblReturningvalue'>" + item.ProductCost + @"</span>
										</td>
										<td>
											<span id='lblReplacementvalue'>" + item.ProductCost + @"</span>
										</td>
										<td>
											<span id='lblBalanceamount'>" + Balanceamount + @"</span>
										</td>
									</tr>

");
        }
        strRetunsval.Append(@"
" + strResult + @"
</div>
</div>
<div id='panelReplacement'>
		
					<div class='widget'>
			<div class='widget_title'>
				<span class='iconsweet'>r</span>
				<h5> <label id='Label1'>Replacement Product(s)</label> </h5>
				</div>
			<div class='widget_body'>

								<table class='activity_datatable' width='100%'>
									<tbody><tr>
										<th>
											<span>Returning Value</span>
										</th>
										<th>
											<span>Replacement Value</span>
										</th>
										<th>
											<span>Balance Amount</span>
										</th>
									</tr>
								  " + strreturntd + @"
								</tbody></table>
							</div>
					</div>");

        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Result = strRetunsval.ToString()

        };


    }

    public CustomResponse GetReplacementProduct(int trnsId)
    {
        var repository = new UserProductTransactionRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.TransactionId, out totalRecords, 0, 0, (p => p.TransactionId == trnsId), null, "", true);
        StringBuilder strResult = new StringBuilder();
        StringBuilder strtd = new StringBuilder();
        foreach (var item in data)
        {
            strtd.Append(@"   <tr> <td><label>" + item.TransactionId + @"</label></td> 
<td><label>" + item.Product.ProductName + @"</label></td> 
<td><label>" + item.Quantity + @"</label></td> 	
<td><label>" + item.ProductCost + @"</label></td> 	
<td><label>" + item.ProductCost + @"</label></td> 	
<td><span class='data_actions iconsweet'><a id=" + item.TransactionId + @" class='tip_north' original-title='Edit' target='_blank' onclick='EditReplaceOrders(this.id);'>C</a></span></td> 	
<td><span class='data_actions iconsweet'>
<a id=" + item.TransactionId + @" class='tip_north' original-title='Edit' target='_blank' onclick='Delete(this.id);'>X</a></span> </td> 
</tr>");
        }
        strResult.Append(@"<table class='activity_datatable' width='100%'> 	
 <tbody><tr><th class='grdHeader'><span>SKU</span></th><th class='grdHeader'><span>Product Name</span></th> 	<th class='grdHeader'><span>Quantity</span></th> 	<th class='grdHeader'><span>Price</span></th><th class='grdHeader'><span>Total</span></th><th class='grdHeader'><span>Edit</span></th><th class='grdHeader'><span>Delete</span></th></tr> 	
  " + strtd + @"
	</tbody>
	  </table>

</div>
								</div>
  </div>

			<div class='widget'>
			<div class='widget_title'>
				<span class='iconsweet'>r</span>
				<h5 id='ctl00_ctl00_Main_Main_H3'> Enter Shipping &amp; Tax Info </h5>
				</div>
			<div class='widget_body'>
								<ul class='form_fields_container'>
									<li>
										<label>
											Shipping Mode :
										</label>
									   <div class='form_input'>
											<div id='drpShippingMode' class='selector'><span style='-moz-user-select: none;'>Express</span><select style='opacity: 0;' id='drpShippingMode'>
			<option value='1923'>BUissness</option>
			<option selected='selected' value='347'>Express</option>

		</select></div>
										</div></li>
									<li>
										<label id='lblEmailId'>
											ShippingAmount :
										</label>
										<div class='form_input'>
											<input  value='100' id='txtShippingAmount' type='text'>
										</div></li>
									<li>
										<label id='lblOrdrNo'>
											Tax Amount :
										</label>
										<div class='form_input'>
											<input  value='0' id='txtTaxAmount' type='text'>
										</div></li>
								</ul>
							</div>
					   </div>
				
	</div>
				<div class='action_bar text_right'>
						<input  value='Complete Returns' onclick='MakePaymentFromAdmin(" + trnsId + @");' id='btncompletereturns' class='button_small greyishBtn' type='button'>
						<input  value='Cancel' id='btnCancelProcess' class='button_small greyishBtn' type='button'>
					   
					</div>



");

        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Result = strResult.ToString()

        };


    }

    [HttpGet]
    public CustomResponse SearchAutoCompletList(string sord)
    {
        try
        {
            var repository = new ProductRepository();
            db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();
            int totalRecords;

            var data = repository.FetchAllByPage(p => p.ProductId, out totalRecords, 0, 0,
                                                        (p => p.IsSold == false &&
                                                         p.IsActive && p.IsDeleted == false && (p.ProductCode == sord || p.ProductName.Contains(sord) || (p.ProductName.Replace(" ", "")).Contains(sord))), null, "").Distinct();
            var dat = repository.FetchAllByPage(p => p.ProductId, out totalRecords, 0, 0,
                                                        (p => p.IsSold == false &&
                                                         p.IsActive && p.IsDeleted == false && (p.ProductCode == sord || p.Brand.Contains(sord) || (p.Brand.Replace(" ", "")).Contains(sord))), null, "").Distinct();

            StringBuilder strResult = new StringBuilder();
            if (sord == null)
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.NoData.ToString(),
                    Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
                    Result = strResult.ToString()
                };
            }
            else
            {
                //                strResult.Append(@"<p>");
                //                foreach (var item in dat)
                //                {
                //                    strResult.Append(@"<p id='searchresults' onClick='NavigateToDetails(" + item.ProductId + @");'>
                //<span class='category'>" + item.SubCategory.CategoryName + @"</span>
                //<a href='#'>
                //<span class='brand'>" + item.Brand + @"</span>
                //<img style='height:30px;width:40px;' onClick='NavigateToDetails(" + item.ProductId + ");' alt=" + item.ProductName + " src=" + item.ProductImgUrl.Replace("~/", ApiUrl) + @">
                //<span class='searchheading' onClick='NavigateToDetails(" + item.ProductId + ");'>" + item.ProductName + @"</span>
                //<span></spa>
                //</a>
                //");
                //                }
                //                strResult.Append(@"</p>");

                strResult.Append(@"<p>");
                foreach (var item in data)
                {
                    strResult.Append(@"<p id='searchresults'>
<span class='category'>" + item.SubCategory.CategoryName + @"</span>
<a href='" + ApiUrl + "product/" + item.ProductName.Replace(" ", "-") + @"/" + item.ProductId + @"'>
<span class='brand'>" + item.Brand + @"</span>
<img style='height:30px;width:40px;' onClick='NavigateToDetails(" + item.ProductId + ");' alt=" + item.ProductName + " src=" + item.ProductImgUrl.Replace("~/", ApiUrl) + @">
<span class='searchheading' onClick='NavigateToDetails(" + item.ProductId + ");'>" + item.SubCategory.SubCategoryName + " / " + item.ProductName + @"</span>
<span></span>
</a>
");
                }
                strResult.Append(@"</p>");

                return new CustomResponse
                {
                    Status = string.IsNullOrWhiteSpace(strResult.ToString()) ? Shared.ResponseStatus.NoData.ToString() : Shared.ResponseStatus.Success.ToString(),
                    Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
                    Result = strResult.ToString()
                };
            }
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }
    }

    [HttpPost]
    public CustomResponse CashOnDelivery()
    {
        try
        {
            string EmailID = HttpContext.Current.Session["LoginName"].ToString();
            string Password = HttpContext.Current.Session["Password"].ToString();
            var repository = new UserRepository();
            var UserDetails = repository.First(u => u.EmailId == EmailID && u.PassCode == Password && u.RoleId == (int)Shared.UserRoles.Customer);

            decimal MobileNumber = UserDetails.MobileNo;
            string UserFirstName = UserDetails.FirstName;

            Random r = new Random();
            int randNum = r.Next(1000000);
            string OTP = randNum.ToString("D6");

            HttpContext.Current.Session["OTP"] = OTP;

            string Message = "OTP for Cash on Delivery in Health ur Wealth is: " + OTP;

            string Url = WebConfigurationManager.AppSettings["SmsUrl"].ToString();
            string UserName = WebConfigurationManager.AppSettings["SmsId"].ToString();
            string password = WebConfigurationManager.AppSettings["SmsPwd"].ToString();
            //string Status = Utility.MailMessage.SendSms(Url, UserName, password, MobileNumber, Message, "N","");

            Utility.MailMessage ms = new Utility.MailMessage();
            ms.Subject = "OTP for Cash on Delivery";
            ms.To = EmailID;

            string domain;
            Uri url = HttpContext.Current.Request.Url;
            domain = url.AbsoluteUri.Replace(url.PathAndQuery, string.Empty);

            string body = string.Empty;
            string htmlPagePath = Shared.GethtmlPage(Shared.ProductStatusForSendMail.CashOnDelivery);
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("" + htmlPagePath + "")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("##FirstName##", UserFirstName);
            ms.Body = body.Replace("##OTP##", OTP);
            ms.IsBodyHtml = true;

            try
            {
                ms.SendMail();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                SendCODPendingMailtoAdmin(UserDetails);
            }
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = null
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Result = null
            };
        }
    }

    public string SendCODPendingMailtoAdmin(User userInfo)
    {
        string mailStatus = "";

        Utility.MailMessage ms = new Utility.MailMessage();
        ms.Subject = "User trying to order via COD";

        string domain;
        Uri url = HttpContext.Current.Request.Url;
        domain = url.AbsoluteUri.Replace(url.PathAndQuery, string.Empty);

        var cartItems = (List<UserProductTransaction>)BalUtility.GetSession(Shared.Sessions.CustomerCart) ??
                          new List<UserProductTransaction>();

        string body = string.Empty;
        string htmlPagePath = Shared.GethtmlPage(Shared.ProductStatusForSendMail.CodPending);
        using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("" + htmlPagePath + "")))
        {
            body = reader.ReadToEnd();
        }

        UserProductTransaction PrdctDetls = cartItems[0];

        body = body.Replace("##FirstName##", userInfo.FirstName);
        body = body.Replace("##ProductName##", PrdctDetls.Product.ProductName);
        body = body.Replace("##Brand##", PrdctDetls.Product.Brand);
        body = body.Replace("##ProductQty##", Convert.ToString(PrdctDetls.Quantity));
        int subtotalCost = Convert.ToInt32((PrdctDetls.ProductCost));
        int shipping = 0;
        if (subtotalCost <= 1000)
        {
            shipping = 99;
        }
        else if (subtotalCost > 2000)
        {
            shipping = 0;
        }
        else
        {
            shipping = 50;
        }
        StringBuilder strtrResult = new StringBuilder();
        strtrResult.Append(@"<tr>
									<td class='alignLeft prodImg'><img src='" + PrdctDetls.Product.ProductImgUrl.Replace("~/", "http://admin.healthurwealth.com/") + @"' style='width:80px;height:80px;' alt='Title' /></td>
									<td class='alignLeft prodDesc'>
										<h4>" + PrdctDetls.Product.ProductName + @" </h4>
										Quantity:" + PrdctDetls.Quantity + @" <br />                                        
									</td>
									<td class='alignRight'><span class='amount'>&#8377;" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(PrdctDetls.Quantity * PrdctDetls.Product.ProductCost), true) + @" </span></td>
								</tr>");


        body = body.Replace("##SubTotalCost##", (Convert.ToString(subtotalCost)));
        body = body.Replace("##ShippingAmount##", Convert.ToString(shipping));
        int TotalAmount = Convert.ToInt32(subtotalCost + shipping);
        body = body.Replace("##Total##", (Convert.ToString(TotalAmount)));
        body = body.Replace("##GrandTotal##", (Convert.ToString(TotalAmount)));
        body = body.Replace("##StreetAddress1##", (Convert.ToString(userInfo.UserProductTransactions.FirstOrDefault().UserAddress.StreetAddress1)));
        body = body.Replace("##StreetAddress2##", (Convert.ToString(userInfo.UserProductTransactions.FirstOrDefault().UserAddress.StreetAddress2)));
        body = body.Replace("##LandMark##", (Convert.ToString(userInfo.UserProductTransactions.FirstOrDefault().UserAddress.LandMark)));
        body = body.Replace("##City##", (Convert.ToString(userInfo.UserProductTransactions.FirstOrDefault().UserAddress.City)));
        body = body.Replace("##State##", (Convert.ToString(userInfo.UserProductTransactions.FirstOrDefault().UserAddress.StateName)));
        body = body.Replace("##PinCode##", (Convert.ToString(userInfo.UserProductTransactions.FirstOrDefault().UserAddress.PinCode)));
        body = body.Replace("##MobileNumber##", (Convert.ToString(userInfo.MobileNo)));
        body = body.Replace("##EmailID##", (Convert.ToString(userInfo.EmailId)));
        body = body.Replace("##ProductList##", strtrResult.ToString());

        body = body.Replace("##order page##", "Order Page");
        ms.Body = body;
        ms.IsBodyHtml = true;

        ms.SendMail();

        return mailStatus;
    }

    public CustomResponse VerifyOTP(string Otp)
    {
        string SessionedOTPvalue = HttpContext.Current.Session["OTP"].ToString();
        if (SessionedOTPvalue == Otp)
        {
            PersonService PS = new PersonService();
            string PaymentMethod = "Cash On Delivery";
            var PaymentID = PS.MakePayment(PaymentMethod);

            return new CustomResponse()
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = PaymentID
            };
        }
        else
        {
            return new CustomResponse()
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Result = null
            };
        }
    }

    public CustomResponse EditMobileNumber(decimal Mobile, long UserID)
    {
        db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();
        User UserDetails = Entity.Users.Where(x => x.UserId == UserID).First();
        UserDetails.MobileNo = Mobile;
        int I = Entity.SaveChanges();

        if (I == 1)
        {
            return new CustomResponse()
            {
                Status = Shared.ResponseStatus.Success.ToString()
            };
        }
        else
        {
            return new CustomResponse()
            {
                Status = Shared.ResponseStatus.Fail.ToString()
            };
        }
    }

    [HttpGet]
    public CustomResponse SearchProductList(string sord)
    {
        try
        {
            var repository = new ProductRepository();
            db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();
            int totalRecords;
            StringBuilder strResult = new StringBuilder();

            var search = sord.Split(' ');

            if (search.Count() > 1)
            {

                if (search.Count() == 2)
                {

                    string sord1 = search[0];
                    string sord2 = search[1];

                    var data = repository.FetchAllByPage(p => p.ProductId, out totalRecords, 0, 0,
                                                                                   (p => p.IsSold == false &&
                                                                                    p.IsActive && p.IsDeleted == false && ((p.ProductCode == sord1 && p.ProductCode == sord2) || (p.ProductName.Contains(sord1) && p.ProductName.Contains(sord2)))), null, "").Distinct();

                    var dat = repository.FetchAllByPage(p => p.ProductId, out totalRecords, 0, 0,
                                                                (p => p.IsSold == false &&
                                                                 p.IsActive && p.IsDeleted == false && ((p.ProductCode == sord1 && p.ProductCode == sord2) || (p.Brand.Contains(sord1) && p.Brand.Contains(sord2)))), null, "").Distinct();



                    if (sord == null)
                    {
                        return new CustomResponse
                        {
                            Status = Shared.ResponseStatus.NoData.ToString(),
                            Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
                            Result = strResult.ToString()
                        };
                    }
                    else
                    {
                        strResult.Append(@"<p>");
                        if (dat.Count() <= 2)
                        {
                            foreach (var item in data)
                            {
                                //                        strResult.Append(@"<p id='searchresults'>
                                //<span class='category'>" + item.SubCategory.CategoryName + @"</span>
                                //<a href='"+ApiUrl+"product/" + item.ProductName + @"/" + item.ProductId + @"'>
                                //<span class='brand'>" + item.Brand + @"</span>
                                //<img style='height:30px;width:40px;' onClick='NavigateToDetails(" + item.ProductId + ");' alt=" + item.ProductName + " src=" + item.ProductImgUrl.Replace("~/", ApiUrl) + @">
                                //<span class='searchheading' onClick='NavigateToDetails(" + item.ProductId + ");'>" + item.SubCategory.SubCategoryName + " / " + item.ProductName + @"</span>
                                //<span></spa>
                                //</a>
                                //");
                                strResult.Append(@"<p id='searchresults' onclick='NavigateToDetails(" + item.ProductId + @");' style='line-height: 25px;'>
							 <a href='" + ApiUrl + "product/" + item.ProductName.Replace(" ", "-") + @"/" + item.ProductId + @"' style='display:block;height:60px;'>
							  <span style='float:left;'>
							   <img style='height:60px;width:60px;' onclick='NavigateToDetails(" + item.ProductId + @");' alt='" + item.ProductName + @"' src='" + item.ProductImgUrl.Replace("~/", ApiUrl) + @"'>
							  </span>
							  <span style='padding-left:10px;'>
							   <span class='searchheading' onclick='NavigateToDetails(" + item.ProductId + @");'>" + item.SubCategory.SubCategoryName + " / " + item.ProductName + @"</span>
							  </span>
<div>
  <span class='brand' style='display:block;font-size:12px;color:#b3b3b3;float:left;border-right:1px solid #d3d3d3; padding-right:10px;'><span style='padding-left:10px;'>BRAND:</span> " + item.Brand + @"</span><span class='brand' style='display:block;font-size:12px;color:#66BCDA;float:left;'><span style='padding-left:10px;'>PRICE:</span> " + item.ProductCost + @"</span>
	</div>
							 </a>
							</p>");
                            }
                            strResult.Append(@"</p>");
                        }
                        else
                        {
                            strResult.Append(@"<p>");
                            foreach (var item in dat)
                            {
                                strResult.Append(@"<p id='searchresults' onclick='NavigateToDetails(" + item.ProductId + @");' style='line-height: 25px;'>
							 <a href='" + ApiUrl + "product/" + item.ProductName.Replace(" ", "-") + @"/" + item.ProductId + @"' style='display:block;height:60px;'>
							  <span style='float:left;'>
							   <img style='height:60px;width:60px;' onclick='NavigateToDetails(" + item.ProductId + @");' alt='" + item.ProductName + @"' src='" + item.ProductImgUrl.Replace("~/", ApiUrl) + @"'>
							  </span>
							  <span style='padding-left:10px;'>
							   <span class='searchheading' onclick='NavigateToDetails(" + item.ProductId + @");'>" + item.SubCategory.SubCategoryName + " / " + item.ProductName + @"</span>
</span><span style='display:block;'>
  <span class='brand' style='display:block;font-size:12px;color:#b3b3b3;float:left;border-right:1px solid #d3d3d3; padding-right:10px;'><span style='padding-left:10px;'>BRAND:</span> " + item.Brand + @"</span><span class='brand' style='display:block;font-size:12px;color:#66BCDA;float:left;'><span style='padding-left:10px;'>PRICE:</span> " + item.ProductCost + @"</span>
	</span>
							  
							 </a>
							</p>");
                            }
                            strResult.Append(@"</p>");
                        }
                    }
                }
                else if (search.Count() == 3)
                {
                    string sord1 = search[0];
                    string sord2 = search[1];
                    string sord3 = search[2];

                    var data = repository.FetchAllByPage(p => p.ProductId, out totalRecords, 0, 0,
                                                               (p => p.IsSold == false &&
                                                                p.IsActive && p.IsDeleted == false && ((p.ProductCode == sord1 && p.ProductCode == sord2 && p.ProductCode == sord3) || (p.ProductName.Contains(sord1) && p.ProductName.Contains(sord2) && p.ProductName.Contains(sord3)))), null, "").Distinct();

                    var dat = repository.FetchAllByPage(p => p.ProductId, out totalRecords, 0, 0,
                                                                (p => p.IsSold == false &&
                                                                 p.IsActive && p.IsDeleted == false && ((p.ProductCode == sord1 && p.ProductCode == sord2 && p.ProductCode == sord3) || (p.Brand.Contains(sord1) && p.Brand.Contains(sord2) && p.Brand.Contains(sord3)))), null, "").Distinct();



                    if (sord == null)
                    {
                        return new CustomResponse
                        {
                            Status = Shared.ResponseStatus.NoData.ToString(),
                            Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
                            Result = strResult.ToString()
                        };
                    }
                    else
                    {
                        strResult.Append(@"<p>");
                        if (dat.Count() <= 2)
                        {
                            foreach (var item in data)
                            {
                                //                        strResult.Append(@"<p id='searchresults'>
                                //<span class='category'>" + item.SubCategory.CategoryName + @"</span>
                                //<a href='"+ApiUrl+"product/" + item.ProductName + @"/" + item.ProductId + @"'>
                                //<span class='brand'>" + item.Brand + @"</span>
                                //<img style='height:30px;width:40px;' onClick='NavigateToDetails(" + item.ProductId + ");' alt=" + item.ProductName + " src=" + item.ProductImgUrl.Replace("~/", ApiUrl) + @">
                                //<span class='searchheading' onClick='NavigateToDetails(" + item.ProductId + ");'>" + item.SubCategory.SubCategoryName + " / " + item.ProductName + @"</span>
                                //<span></spa>
                                //</a>
                                //");
                                strResult.Append(@"<p id='searchresults' onclick='NavigateToDetails(" + item.ProductId + @");' style='line-height: 25px;'>
							 <a href='" + ApiUrl + "product/" + item.ProductName.Replace(" ", "-") + @"/" + item.ProductId + @"' style='display:block;height:60px;'>
							  <span style='float:left;'>
							   <img style='height:60px;width:60px;' onclick='NavigateToDetails(" + item.ProductId + @");' alt='" + item.ProductName + @"' src='" + item.ProductImgUrl.Replace("~/", ApiUrl) + @"'>
							  </span>
							  <span style='padding-left:10px;'>
							   <span class='searchheading' onclick='NavigateToDetails(" + item.ProductId + @");'>" + item.SubCategory.SubCategoryName + " / " + item.ProductName + @"</span>
</span><span style='display:block;'>
  <span class='brand' style='display:block;font-size:12px;color:#b3b3b3;float:left;border-right:1px solid #d3d3d3; padding-right:10px;'><span style='padding-left:10px;'>BRAND:</span> " + item.Brand + @"</span><span class='brand' style='display:block;font-size:12px;color:#66BCDA;float:left;'><span style='padding-left:10px;'>PRICE:</span> " + item.ProductCost + @"</span>
	</span>
							  
							 </a>
							</p>");
                            }
                            strResult.Append(@"</p>");
                        }
                        else
                        {
                            strResult.Append(@"<p>");
                            foreach (var item in dat)
                            {
                                strResult.Append(@"<p id='searchresults' onclick='NavigateToDetails(" + item.ProductId + @");' style='line-height: 25px;'>
							 <a href='" + ApiUrl + "product/" + item.ProductName.Replace(" ", "-") + @"/" + item.ProductId + @"' style='display:block;height:60px;'>
							  <span style='float:left;'>
							   <img style='height:60px;width:60px;' onclick='NavigateToDetails(" + item.ProductId + @");' alt='" + item.ProductName + @"' src='" + item.ProductImgUrl.Replace("~/", ApiUrl) + @"'>
							  </span>
							  <span style='padding-left:10px;'>
							   <span class='searchheading' onclick='NavigateToDetails(" + item.ProductId + @");'>" + item.SubCategory.SubCategoryName + " / " + item.ProductName + @"</span>
</span><span style='display:block;'>
  <span class='brand' style='display:block;font-size:12px;color:#b3b3b3;float:left;border-right:1px solid #d3d3d3; padding-right:10px;'><span style='padding-left:10px;'>BRAND:</span> " + item.Brand + @"</span><span class='brand' style='display:block;font-size:12px;color:#66BCDA;float:left;'><span style='padding-left:10px;'>PRICE:</span> " + item.ProductCost + @"</span>
	</span>
							  
							 </a>
							</p>");
                            }
                            strResult.Append(@"</p>");
                        }
                    }
                }
                else if (search.Count() == 4)
                {
                    string sord1 = search[0];
                    string sord2 = search[1];
                    string sord3 = search[2];
                    string sord4 = search[3];

                    var data = repository.FetchAllByPage(p => p.ProductId, out totalRecords, 0, 0,
                                                               (p => p.IsSold == false &&
                                                                p.IsActive && p.IsDeleted == false && ((p.ProductCode == sord1 && p.ProductCode == sord2 && p.ProductCode == sord3 && p.ProductCode == sord4) || (p.ProductName.Contains(sord1) && p.ProductName.Contains(sord2) && p.ProductName.Contains(sord3) && p.ProductName.Contains(sord4)))), null, "").Distinct();

                    var dat = repository.FetchAllByPage(p => p.ProductId, out totalRecords, 0, 0,
                                                                (p => p.IsSold == false &&
                                                                 p.IsActive && p.IsDeleted == false && ((p.ProductCode == sord1 && p.ProductCode == sord2 && p.ProductCode == sord3 && p.ProductCode == sord4) || (p.Brand.Contains(sord1) && p.Brand.Contains(sord2) && p.Brand.Contains(sord3) && p.Brand.Contains(sord4)))), null, "").Distinct();



                    if (sord == null)
                    {
                        return new CustomResponse
                        {
                            Status = Shared.ResponseStatus.NoData.ToString(),
                            Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
                            Result = strResult.ToString()
                        };
                    }
                    else
                    {
                        strResult.Append(@"<p>");
                        if (dat.Count() <= 2)
                        {
                            foreach (var item in data)
                            {
                                //                        strResult.Append(@"<p id='searchresults'>
                                //<span class='category'>" + item.SubCategory.CategoryName + @"</span>
                                //<a href='"+ApiUrl+"product/" + item.ProductName + @"/" + item.ProductId + @"'>
                                //<span class='brand'>" + item.Brand + @"</span>
                                //<img style='height:30px;width:40px;' onClick='NavigateToDetails(" + item.ProductId + ");' alt=" + item.ProductName + " src=" + item.ProductImgUrl.Replace("~/", ApiUrl) + @">
                                //<span class='searchheading' onClick='NavigateToDetails(" + item.ProductId + ");'>" + item.SubCategory.SubCategoryName + " / " + item.ProductName + @"</span>
                                //<span></spa>
                                //</a>
                                //");
                                strResult.Append(@"<p id='searchresults' onclick='NavigateToDetails(" + item.ProductId + @");' style='line-height: 25px;'>
							 <a href='" + ApiUrl + "product/" + item.ProductName.Replace(" ", "-") + @"/" + item.ProductId + @"' style='display:block;height:60px;'>
							  <span style='float:left;'>
							   <img style='height:60px;width:60px;' onclick='NavigateToDetails(" + item.ProductId + @");' alt='" + item.ProductName + @"' src='" + item.ProductImgUrl.Replace("~/", ApiUrl) + @"'>
							  </span>
							  <span style='padding-left:10px;'>
							   <span class='searchheading' onclick='NavigateToDetails(" + item.ProductId + @");'>" + item.SubCategory.SubCategoryName + " / " + item.ProductName + @"</span>
 </span><span style='display:block;'>
  <span class='brand' style='display:block;font-size:12px;color:#b3b3b3;float:left;border-right:1px solid #d3d3d3; padding-right:10px;'><span style='padding-left:10px;'>BRAND:</span> " + item.Brand + @"</span><span class='brand' style='display:block;font-size:12px;color:#66BCDA;float:left;'><span style='padding-left:10px;'>PRICE:</span> " + item.ProductCost + @"</span>
	</span>
							 
							 </a>
							</p>");
                            }
                            strResult.Append(@"</p>");
                        }
                        else
                        {
                            strResult.Append(@"<p>");
                            foreach (var item in dat)
                            {
                                strResult.Append(@"<p id='searchresults' onclick='NavigateToDetails(" + item.ProductId + @");' style='line-height: 25px;'>
							 <a href='" + ApiUrl + "product/" + item.ProductName.Replace(" ", "-") + @"/" + item.ProductId + @"' style='display:block;height:60px;'>
							  <span style='float:left;'>
							   <img style='height:60px;width:60px;' onclick='NavigateToDetails(" + item.ProductId + @");' alt='" + item.ProductName + @"' src='" + item.ProductImgUrl.Replace("~/", ApiUrl) + @"'>
							  </span>
							  <span style='padding-left:10px;'>
							   <span class='searchheading' onclick='NavigateToDetails(" + item.ProductId + @");'>" + item.SubCategory.SubCategoryName + " / " + item.ProductName + @"</span>
 </span><span style='display:block;'>
  <span class='brand' style='display:block;font-size:12px;color:#b3b3b3;float:left;border-right:1px solid #d3d3d3; padding-right:10px;'><span style='padding-left:10px;'>BRAND:</span> " + item.Brand + @"</span><span class='brand' style='display:block;font-size:12px;color:#66BCDA;float:left;'><span style='padding-left:10px;'>PRICE:</span> " + item.ProductCost + @"</span>
	</span>
							 
							 </a>
							</p>");
                            }
                            strResult.Append(@"</p>");
                        }
                    }
                }

                else if (search.Count() == 5)
                {
                    string sord1 = search[0];
                    string sord2 = search[1];
                    string sord3 = search[2];
                    string sord4 = search[3];
                    string sord5 = search[4];

                    var data = repository.FetchAllByPage(p => p.ProductId, out totalRecords, 0, 0,
                                                               (p => p.IsSold == false &&
                                                               p.IsActive && p.IsDeleted == false && ((p.ProductCode == sord1 && p.ProductCode == sord2 && p.ProductCode == sord3 && p.ProductCode == sord4 && p.ProductCode == sord5) || (p.ProductName.Contains(sord1) && p.ProductName.Contains(sord2) && p.ProductName.Contains(sord3) && p.ProductName.Contains(sord4) && p.ProductName.Contains(sord5)))), null, "").Distinct();

                    var dat = repository.FetchAllByPage(p => p.ProductId, out totalRecords, 0, 0,
                                                                (p => p.IsSold == false &&
                                                                 p.IsActive && p.IsDeleted == false && ((p.ProductCode == sord1 && p.ProductCode == sord2 && p.ProductCode == sord3 && p.ProductCode == sord4 && p.ProductCode == sord5) || (p.Brand.Contains(sord1) && p.Brand.Contains(sord2) && p.Brand.Contains(sord3) && p.Brand.Contains(sord4) && p.Brand.Contains(sord5)))), null, "").Distinct();



                    if (sord == null)
                    {
                        return new CustomResponse
                        {
                            Status = Shared.ResponseStatus.NoData.ToString(),
                            Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
                            Result = strResult.ToString()
                        };
                    }
                    else
                    {
                        strResult.Append(@"<p>");
                        if (dat.Count() <= 2)
                        {
                            foreach (var item in data)
                            {
                                //                        strResult.Append(@"<p id='searchresults'>
                                //<span class='category'>" + item.SubCategory.CategoryName + @"</span>
                                //<a href='"+ApiUrl+"product/" + item.ProductName + @"/" + item.ProductId + @"'>
                                //<span class='brand'>" + item.Brand + @"</span>
                                //<img style='height:30px;width:40px;' onClick='NavigateToDetails(" + item.ProductId + ");' alt=" + item.ProductName + " src=" + item.ProductImgUrl.Replace("~/", ApiUrl) + @">
                                //<span class='searchheading' onClick='NavigateToDetails(" + item.ProductId + ");'>" + item.SubCategory.SubCategoryName + " / " + item.ProductName + @"</span>
                                //<span></spa>
                                //</a>
                                //");
                                strResult.Append(@"<p id='searchresults' onclick='NavigateToDetails(" + item.ProductId + @");' style='line-height: 25px;'>
							 <a href='" + ApiUrl + "product/" + item.ProductName.Replace(" ", "-") + @"/" + item.ProductId + @"' style='display:block;height:60px;'>
							  <span style='float:left;'>
							   <img style='height:60px;width:60px;' onclick='NavigateToDetails(" + item.ProductId + @");' alt='" + item.ProductName + @"' src='" + item.ProductImgUrl.Replace("~/", ApiUrl) + @"'>
							  </span>
							  <span style='padding-left:10px;'>
							   <span class='searchheading' onclick='NavigateToDetails(" + item.ProductId + @");'>" + item.SubCategory.SubCategoryName + " / " + item.ProductName + @"</span>
</span><span style='display:block;'>
  <span class='brand' style='display:block;font-size:12px;color:#b3b3b3;float:left;border-right:1px solid #d3d3d3; padding-right:10px;'><span style='padding-left:10px;'>BRAND:</span> " + item.Brand + @"</span><span class='brand' style='display:block;font-size:12px;color:#66BCDA;float:left;'><span style='padding-left:10px;'>PRICE:</span> " + item.ProductCost + @"</span>
	</span>
							  
							 </a>
							</p>");
                            }
                            strResult.Append(@"</p>");
                        }
                        else
                        {
                            strResult.Append(@"<p>");
                            foreach (var item in dat)
                            {
                                strResult.Append(@"<p id='searchresults' onclick='NavigateToDetails(" + item.ProductId + @");' style='line-height: 25px;'>
							 <a href='" + ApiUrl + "product/" + item.ProductName.Replace(" ", "-") + @"/" + item.ProductId + @"' style='display:block;height:60px;'>
							  <span style='float:left;'>
							   <img style='height:60px;width:60px;' onclick='NavigateToDetails(" + item.ProductId + @");' alt='" + item.ProductName + @"' src='" + item.ProductImgUrl.Replace("~/", ApiUrl) + @"'>
							  </span>
							  <span style='padding-left:10px;'>
							   <span class='searchheading' onclick='NavigateToDetails(" + item.ProductId + @");'>" + item.SubCategory.SubCategoryName + " / " + item.ProductName + @"</span>
</span><span style='display:block;'>
  <span class='brand' style='display:block;font-size:12px;color:#b3b3b3;float:left;border-right:1px solid #d3d3d3; padding-right:10px;'><span style='padding-left:10px;'>BRAND:</span> " + item.Brand + @"</span><span class='brand' style='display:block;font-size:12px;color:#66BCDA;float:left;'><span style='padding-left:10px;'>PRICE:</span> " + item.ProductCost + @"</span>
	</span>
							  
							 </a>
							</p>");
                            }
                            strResult.Append(@"</p>");
                        }
                    }
                }
            }
            else
            {
                var data = repository.FetchAllByPage(p => p.ProductId, out totalRecords, 0, 0,
                                                            (p => p.IsSold == false &&
                                                             p.IsActive && p.IsDeleted == false && (p.ProductCode == sord || p.ProductName.Contains(sord) || (p.ProductName.Replace(" ", "")).Contains(sord))), null, "").Distinct();

                var dat = repository.FetchAllByPage(p => p.ProductId, out totalRecords, 0, 0,
                                                            (p => p.IsSold == false &&
                                                             p.IsActive && p.IsDeleted == false && (p.ProductCode == sord || p.Brand.Contains(sord) || (p.Brand.Replace(" ", "")).Contains(sord))), null, "").Distinct();

                if (sord == null)
                {
                    return new CustomResponse
                    {
                        Status = Shared.ResponseStatus.NoData.ToString(),
                        Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
                        Result = strResult.ToString()
                    };
                }
                else
                {
                    strResult.Append(@"<p>");
                    if (dat.Count() <= 2)
                    {
                        foreach (var item in data)
                        {
                            //                        strResult.Append(@"<p id='searchresults'>
                            //<span class='category'>" + item.SubCategory.CategoryName + @"</span>
                            //<a href='"+ApiUrl+"product/" + item.ProductName + @"/" + item.ProductId + @"'>
                            //<span class='brand'>" + item.Brand + @"</span>
                            //<img style='height:30px;width:40px;' onClick='NavigateToDetails(" + item.ProductId + ");' alt=" + item.ProductName + " src=" + item.ProductImgUrl.Replace("~/", ApiUrl) + @">
                            //<span class='searchheading' onClick='NavigateToDetails(" + item.ProductId + ");'>" + item.SubCategory.SubCategoryName + " / " + item.ProductName + @"</span>
                            //<span></spa>
                            //</a>
                            //");
                            strResult.Append(@"<p id='searchresults' onclick='NavigateToDetails(" + item.ProductId + @");' style='line-height: 25px;'>
							 <a href='" + ApiUrl + "product/" + item.ProductName.Replace(" ", "-") + @"/" + item.ProductId + @"' style='display:block;height:60px;'>
							  <span style='float:left;'>
							   <img style='height:60px;width:60px;' onclick='NavigateToDetails(" + item.ProductId + @");' alt='" + item.ProductName + @"' src='" + item.ProductImgUrl.Replace("~/", ApiUrl) + @"'>
							  </span>
							  <span style='padding-left:10px;'>
							   <span class='searchheading' onclick='NavigateToDetails(" + item.ProductId + @");'>" + item.SubCategory.SubCategoryName + " / " + item.ProductName + @"</span>
</span><span style='display:block;'>
  <span class='brand' style='display:block;font-size:12px;color:#b3b3b3;float:left;border-right:1px solid #d3d3d3; padding-right:10px;'><span style='padding-left:10px;'>BRAND:</span> " + item.Brand + @"</span><span class='brand' style='display:block;font-size:12px;color:#66BCDA;float:left;'><span style='padding-left:10px;'>PRICE:</span> " + item.ProductCost + @"</span>
	</span>
							  
							 </a>
							</p>");
                        }
                        strResult.Append(@"</p>");
                    }
                    else
                    {
                        strResult.Append(@"<p>");
                        foreach (var item in dat)
                        {
                            strResult.Append(@"<p id='searchresults' onclick='NavigateToDetails(" + item.ProductId + @");' style='line-height: 25px;'>
							 <a href='" + ApiUrl + "product/" + item.ProductName.Replace(" ", "-") + @"/" + item.ProductId + @"' style='display:block;height:60px;'>
							  <span style='float:left;'>
							   <img style='height:60px;width:60px;' onclick='NavigateToDetails(" + item.ProductId + @");' alt='" + item.ProductName + @"' src='" + item.ProductImgUrl.Replace("~/", ApiUrl) + @"'>
							  </span>
							  <span style='padding-left:10px;'>
							   <span class='searchheading' onclick='NavigateToDetails(" + item.ProductId + @");'>" + item.SubCategory.SubCategoryName + " / " + item.ProductName + @"</span>
 </span><span style='display:block;'>
  <span class='brand' style='display:block;font-size:12px;color:#b3b3b3;float:left;border-right:1px solid #d3d3d3; padding-right:10px;'><span style='padding-left:10px;'>BRAND:</span> " + item.Brand + @"</span><span class='brand' style='display:block;font-size:12px;color:#66BCDA;float:left;'><span style='padding-left:10px;'>PRICE:</span> " + item.ProductCost + @"</span>
	</span>
							 
							 </a>
							</p>");
                        }
                        strResult.Append(@"</p>");
                    }
                }
            }


            return new CustomResponse
            {
                Status = string.IsNullOrWhiteSpace(strResult.ToString()) ? Shared.ResponseStatus.NoData.ToString() : Shared.ResponseStatus.Success.ToString(),
                Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
                Result = strResult.ToString()
            };

        }

        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }
    }

    [HttpPost]
    public CustomResponse ContactUS(string Name, string Email, string Mobile, string Comments)
    {
        try
        {
            //saved contact us info in newsletter table in DB.
            db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();
            dbo_Newsletter News = new dbo_Newsletter();
            News.EmailId = Email;
            News.Status = 1;
            News.CreatedBy = DateTime.Now;
            News.IsActive = true;
            News.CreatedOn = DateTime.Now;
            Entity.dbo_Newsletter.Add(News);
            Entity.SaveChanges();

            int ID = Entity.dbo_Newsletter.Where(x => x.EmailId == Email).OrderByDescending(x => x.EmailId).First().NewsletterId;

            Utility.MailMessage ms = new Utility.MailMessage();
            ms.Subject = "Contact US - " + ID;

            string body = string.Empty;
            string htmlPagePath = Shared.GethtmlPage(Shared.ProductStatusForSendMail.Contactus);
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("" + htmlPagePath + "")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("##Name##", Name);
            body = body.Replace("##EmailId##", Email);
            body = body.Replace("##Mobile##", Mobile);
            body = body.Replace("##Comments##", Comments);
            ms.Body = body;

            ms.IsBodyHtml = true;
            try
            {
                ms.SendMail();
            }
            catch (Exception ex)
            {

            }
            string Message = "Hello Admin, Mr/Ms " + Name + " is trying to contact our website HealthurWealth. Please Follow him/her with this Number " + Mobile + " and Email " + Email;
            string Url = WebConfigurationManager.AppSettings["SmsUrl"].ToString();
            string UserName = WebConfigurationManager.AppSettings["SmsId"].ToString();
            string password = WebConfigurationManager.AppSettings["SmsPwd"].ToString();
            string Status = Utility.MailMessage.SendSms(Url, UserName, password, 43242, Message, "N", "");

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Message = "Success",
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Message = "Nodata",
                Result = null

            };
        }
    }

    //AddToRelatedProduct
    // [HttpPost]
    [HttpGet]
    public dynamic AddToRelatedProductList(long id)
    {
        try
        {
            var cartItems = (List<RelatedProduct>)BalUtility.GetSession(Shared.Sessions.RelatedProductList) ??
                           new List<RelatedProduct>();

            if (cartItems.FirstOrDefault(p => p.RelatedProductId == id) == null)
            {
                cartItems.Add(new RelatedProduct { RelatedProductId = id });
            }

            BalUtility.CreateSession(cartItems, Shared.Sessions.RelatedProductList);

            if (cartItems.Count > 0)
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = "",
                    Result = ""
                };
            }
            else
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.NoData.ToString(),
                    Message = "Nodata",
                    Result = null

                };
            }
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            throw;
        }
    }

    [HttpGet]
    public dynamic AddToFreeProductList(long id)
    {
        try
        {
            var cartItems = (List<Product>)BalUtility.GetSession(Shared.Sessions.FreeProductList) ??
                           new List<Product>();

            if (cartItems.FirstOrDefault(p => p.ProductId == id) == null)
            {
                cartItems.Clear();
                cartItems.Add(new Product { ProductId = id });
            }

            BalUtility.CreateSession(cartItems, Shared.Sessions.FreeProductList);

            if (cartItems.Count > 0)
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = "",
                    Result = ""
                };
            }
            else
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.NoData.ToString(),
                    Message = "Nodata",
                    Result = null

                };
            }
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            throw;
        }
    }

    [HttpPost]
    public CustomResponse WriteReview(ProductReview objReview)
    {
        try
        {
            var repository = new ReviewRepository();
            repository.Insert(objReview);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Message = CustomMessages.reviewSuccess.ToString(),
                Result = ""


            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };

        }
    }

    //Update Product images created on 19/11/2014 created by nagul
    [HttpGet]
    public dynamic UpdateProductImgs(string imgurl, long PrdID, int imgtype)
    {
        try
        {
            var repository = new ProductRepository();
            string fullpath = "";
            var data = repository.Single(p => p.ProductId == PrdID);
            var httpPostedFile = HttpContext.Current.Request.Files["UploadedImage"];
            if (imgtype == 1)
            {
                data.ProductImgUrl = "https://www.healthurwealth.com/UploadFiles/" + imgurl;
                fullpath = HttpContext.Current.Server.MapPath("https://www.healthurwealth.com/UploadFiles/" + imgurl);
            }
            if (imgtype == 2)
            {
                data.SizeGuidePath = "https://www.healthurwealth.com/UploadFiles/Size_Chart" + imgurl;
                fullpath = HttpContext.Current.Server.MapPath(imgurl);
            }
            data.UpdatedOn = DateTime.Now;
            repository.Update(data);
            httpPostedFile.SaveAs(fullpath);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Message = "",
                Result = data,
            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }
    }

    [HttpGet]
    public dynamic deleteFromRelatedProductList(long RelatedPrdctId, long PrdctID)
    {

        try
        {


            var cartItems = (List<RelatedProduct>)BalUtility.GetSession(Shared.Sessions.RelatedProductList) ??
                           new List<RelatedProduct>();

            var cartItem = cartItems.Where(x => x.RelatedProductId == RelatedPrdctId).FirstOrDefault();
            cartItems.Remove(cartItem);
            if (cartItems.Count == 0)
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.NoData.ToString(),
                    Message = "No More Products in List",
                    Result = null
                };
            }
            else
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = "",
                    //  Result = BalUtility.RelatedProductListInfo()


                };

            }
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }
    }

    [HttpGet]
    public dynamic deleteFromFreeProductList(long PrdctID)
    {
        try
        {
            var cartItems = (List<Product>)BalUtility.GetSession(Shared.Sessions.FreeProductList) ??
                           new List<Product>();

            cartItems.Clear();

            if (cartItems.Count == 0)
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.NoData.ToString(),
                    Message = "No More Products in List",
                    Result = null
                };
            }
            else
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = "",
                    //  Result = BalUtility.RelatedProductListInfo()


                };

            }
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }
    }

    [HttpGet]
    public CustomResponse GetStates()
    {
        var repository = new StatesRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.StateId, out totalRecords, 0, 0, (p => p.IsActive && p.IsDeleted == false), c => new { c.StateId, c.StateName, c.IsActive }).ToList();
        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Message = "",
            Result = data

        };
    }

    [HttpGet]
    public dynamic GetPincodes(string sidx, string sord, int page, int rows)
    {

        var repository = new PinCodesInformationRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.PinCodeId, out totalRecords, 0, 0, (p => p.IsActive == true), c => new { c.PinCodeId, c.District, c.Pincode }).ToList();
        return new
        {
            total = totalRecords / rows + 1,
            page,
            records = totalRecords,
            rows = data
        };

    }

    public dynamic GetUserIP()
    {
        HttpRequest currentRequest = HttpContext.Current.Request;
        string ipAddress = currentRequest.ServerVariables["HTTP_X_FORWARDED_FOR"];

        if (ipAddress == null || ipAddress.ToLower() == "unknown")
            ipAddress = currentRequest.ServerVariables["HTTP_X_CLUSTER_CLIENT_IP"];
        var ips = currentRequest.ServerVariables["HTTP_X_CLUSTER_CLIENT_IP"];
        var LOGON_USER = currentRequest.ServerVariables["LOGON_USER"];
        var UserHostName = currentRequest.UserHostName;
        var UserAgent = currentRequest.UserAgent;
        //string HostName = ip.HostName;
        //var ipaddress = ip.AddressList[0].ToString();

        //string ComputerName = Environment.MachineName;
        //string UserDomainName = Environment.UserDomainName;
        //string UserName = Environment.UserName;
        // var id = "GUID: " + ((GuidAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(GuidAttribute), false)).Value.ToUpper();


        return new
        {
            ipAddress = ipAddress,
            ips = ips,
            UserHostName = UserHostName,
            UserAgent = UserAgent,
            LOGON_USER = LOGON_USER
        };
    }

    [HttpGet]
    public string GetAllFeaturedProducts()
    {
        try
        {
            BAL.ProductRepository repository = new ProductRepository();
            List<Product> products = repository.GetAllFeatureProductList();
            var data = (CustomCurrency)BAL.BalUtility.GetSession(Shared.Sessions.Currency);

            StringBuilder strbldr = new StringBuilder();
            foreach (var item in products)
            {
                string ProductFullName = item.ProductName.Replace(" ", "-");
                if (item.ProductName.Length > 33)
                {
                    item.ProductName = item.ProductName.Substring(0, 33) + "...";
                }

                string hideDiscountCost = item.ProductDiscountCost == 0 ? "style='display: none;" : "";
                strbldr.Append(@"
											
								 <li class='item last quick10'>
				<div class='outer_pan'>       
				   <div class='image_rotate'>
				   <div >
				   <a target='_blank' href='" + ApiUrl + "product/" + ProductFullName + @"/" + item.ProductId + @"' onclick='NavigateToDetails(" + item.ProductId + @")' class='product-image'>
								   <img class='firstImg'   height='210px' src='" + item.ProductImgUrl.Replace("~/", ApiUrl) + @"' alt='" + item.ProductName + @"' /></a>
					 </div>
					</div>");

                if (item.ProductDiscountPercentage != 0)
                {
                    strbldr.Append(@"<div class='product-img-box'>
				<div class='badge'>			
						<div class='ribbon'><span>Sale</span></div>
									</div>
						 </div>");
                }

                strbldr.Append(@"</div>
					<div class='outer_bottom'>
				   <h2 class='product-name' style='line-height:20px;'><a target='_blank' href='" + ApiUrl + "product/" + ProductFullName + @"/" + item.ProductId + @"' style='overflow:hidden; text-overflow:ellipsis; width:225px;' >" + item.ProductName + @"</a></h2>                                  
		<div class='price-box' style='text-align:center;'>");
                if (item.ProductDiscountPercentage != 0)
                {
                    strbldr.Append(@"     <div style='float:left; font-size:13px; color:#999999;' > <s>" + data.Symbol + " " + (data.Value * item.ProductOriginalCost).ToSpecialFormatString() + "</s>"
  + " | " + item.ProductDiscountPercentage.ToSpecialFormatString() + @"%off  </span> </div> ");
                }
                else
                {
                    strbldr.Append(@"<div></div>");
                }

                strbldr.Append(@"  <div style='align:center;'> <span style='color:#D9387E; font-size:19px; font-weight:bold;'>"
  + data.Symbol + " " + (data.Value * item.ProductCost).ToSpecialFormatString()
  + @"</span></div> </div>                     
					<div class='product_icons' align='center'>  
<table><tr>
<td  style='padding-top:0px;'> <ul class='add-to-links'><li><a  title='Add to Wishlist' class='link-wishlist' onclick='addToWishList(" + item.ProductId + @")'>Add to Wishlist</a></li></td></tr></table>               
	 <ul>   </ul>
					</div>
					</div>
				</li>");
            }
            // lt2.Text += strbldr.ToString();
            return strbldr.ToString();
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [HttpGet]
    public string GetAllNewProducts()
    {
        try
        {
            BAL.ProductRepository repository = new ProductRepository();
            List<Product> products = repository.GetAllLatestProducts();
            //var dat = repository.FetchAllByPage(p => p., out  totalRecords, 0, 0,(p => p.IsActive == true)
            var data = (CustomCurrency)BAL.BalUtility.GetSession(Shared.Sessions.Currency);
            StringBuilder strbldr = new StringBuilder();
            foreach (var item in products)
            {
                string ProductFullName = item.ProductName.Replace(" ", "-"); ;
                if (item.ProductName.Length > 33)
                {
                    item.ProductName = item.ProductName.Substring(0, 33) + "...";
                }

                string hideDiscountCost = item.ProductDiscountCost == 0 ? "style='display: none;" : "";
                strbldr.Append(@"
											
								 <li class='item last quick10'>
				<div class='outer_pan'>       
				   <div class='image_rotate'>
				   <div >
				   <a target='_blank' href='" + ApiUrl + "product/" + ProductFullName + @"/" + item.ProductId + @"' onclick='NavigateToDetails(" + item.ProductId + @")' class='product-image'>
								   <img class='firstImg'   height='210px' src='" + item.ProductImgUrl.Replace("~/", ApiUrl) + @"' alt='" + item.ProductName + @"' /></a>
					 </div>
					</div>");

                if (item.ProductDiscountPercentage != 0)
                {
                    strbldr.Append(@"<div class='product-img-box'>
				<div class='badge'>			
						<div class='ribbon'><span>Sale</span></div>    
									</div>
						 </div>");
                }

                strbldr.Append(@"</div>
					<div class='outer_bottom'>
				   <h2 class='product-name'><a target='_blank' href='" + ApiUrl + "product/" + ProductFullName + @"/" + item.ProductId + @"' style='overflow:hidden; text-overflow:ellipsis; width:225px;' >" + item.ProductName + @"</a></h2>                                  
		<div class='price-box' style='text-align:center;'>");
                if (item.ProductDiscountPercentage != 0)
                {
                    strbldr.Append(@"     <div style='float:left; font-size:13px; color:#999999;' > <s>" + data.Symbol + " " + (data.Value * item.ProductOriginalCost).ToSpecialFormatString() + "</s>"
  + " | " + item.ProductDiscountPercentage.ToSpecialFormatString() + @"%off  </span> </div> ");
                }
                else
                {
                    strbldr.Append(@"<div></div>");
                }

                strbldr.Append(@"  <div style='align:center;'> <span style='color:#D9387E; font-size:19px; font-weight:bold;'>"
  + data.Symbol + " " + (data.Value * item.ProductCost).ToSpecialFormatString()
  + @"</span></div> </div>                     
					<div class='product_icons' align='center'>  
<table><tr>
<td  style='padding-top:0px;'> <ul class='add-to-links'><li><a  title='Add to Wishlist' class='link-wishlist' onclick='addToWishList(" + item.ProductId + @")'>Add to Wishlist</a></li></td></tr></table>               
	 <ul>   </ul>
					</div>
					</div>
				</li>");
            }
            // lt2.Text += strbldr.ToString();
            return strbldr.ToString();
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [HttpGet]
    public string NewProducts()
    {
        try
        {

            BAL.ProductRepository repository = new BAL.ProductRepository();
            List<Product> products = repository.GetLatestProductList();
            var data = (CustomCurrency)BAL.BalUtility.GetSession(Shared.Sessions.Currency);
            decimal cost = 0;

            int cnt = 1;
            string txt = "";
            StringBuilder strbuildr = new StringBuilder();
            foreach (var item in products)
            {
                //string hideDiscountCost = item.ProductDiscountCost == 0 ? "style='display: none;" : "";
                strbuildr.Append(@"   
							<li class='item last quick10'>                      
							 <div class='outer_pan'>
					<div class='image_rotate'>
  <div >
				  
						<a  href='" + ApiUrl + "product/" + item.ProductName + @"/" + item.ProductId + @"' class='product-image'><img  height='210px' src= '" + item.ProductImgUrl.Replace("~/", ApiUrl) + @"' alt='" + item.ProductName + @"' /></a>
 </div>
					</div>");

                if (item.ProductDiscountPercentage != 0)
                {
                    strbuildr.Append(@"<div class='badge'> 			
							<div class='ribbon'><span>Sale</span></div> 
							</div>");
                }

                strbuildr.Append(@"</div>
					 <div class='outer_bottom'>
				   <h2 class='product-name'><a  href='" + ApiUrl + "product/" + item.ProductName + @"/" + item.ProductId + @"' title='" + item.ProductName + @"'>" + item.ProductName + @"</a></h2>
			  
					<div class='price-box' style='text-align:center;'>");
                if (item.ProductDiscountPercentage != 0)
                {
                    strbuildr.Append(@"<div style='float:left; font-size:13px; color:#999999;' > <s>" + data.Symbol + " " + (data.Value * item.ProductOriginalCost).ToSpecialFormatString() + "</s>"
    + " | " + item.ProductDiscountPercentage.ToSpecialFormatString() + @"%off  </span> </div> ");
                }
                else
                {
                    strbuildr.Append(@"<div></div>");
                }
                strbuildr.Append(@" <div style='align:center;'> <span style='color:#D9387E; font-size:19px; font-weight:bold;'>"
               + data.Symbol + " " + (data.Value * item.ProductCost).ToSpecialFormatString()
                                        + @"</span>
</div>
</div>
	<div class='product_icons' align='center'> 
<table><tr><td style='padding-top:30px;'> <button type='button' title='Add to Cart' class='button btn-cart' href='" + ApiUrl + "product/" + item.ProductName + @"/" + item.ProductId + @"' onclick='NavigateToDetails(" + item.ProductId + @")'><span><span>Buy Now</span></span></button></td>
<td  style='padding-top:0px;'> <ul class='add-to-links'><li><a  title='Add to Wishlist' class='link-wishlist' onclick='addToWishList(" + item.ProductId + @")'>Add to Wishlist</a></li><td></tr></table>               
	 <ul>   </ul>
					</div>
					</div>
				</li>");

            }
            return strbuildr.ToString();
        }
        catch (Exception ex)
        {
            return null;
        }

    }

    [HttpGet]
    public string FeaturedProducts()
    {
        BAL.ProductRepository repository = new ProductRepository();
        List<Product> products = repository.GetFeatureProductList();
        var data = (CustomCurrency)BAL.BalUtility.GetSession(Shared.Sessions.Currency);

        StringBuilder strbldr = new StringBuilder();
        foreach (var item in products)
        {


            string hideDiscountCost = item.ProductDiscountCost == 0 ? "style='display: none;" : "";
            strbldr.Append(@"
											
								 <li class='item last quick10'>
				<div class='outer_pan'>       
				   <div class='image_rotate'>
				   <div >
				   <a href='" + ApiUrl + "product/" + item.ProductName + @"/" + item.ProductId + @"' onclick='NavigateToDetails(" + item.ProductId + @")' class='product-image'>
								   <img class='firstImg'   height='210px' src='" + item.ProductImgUrl.Replace("~/", ApiUrl) + @"' alt='" + item.ProductName + @"' /></a>
					 </div>
					</div>");

            if (item.ProductDiscountPercentage != 0)
            {
                strbldr.Append(@"<div class='product-img-box'>
				<div class='badge'>			
						<div class='ribbon'><span>Sale</span></div>     
									</div>
						 </div>");
            }

            strbldr.Append(@"</div>
					<div class='outer_bottom'>
				   <h2 class='product-name'><a style='overflow:hidden; text-overflow:ellipsis; width:225px;' >" + item.ProductName + @"</a></h2>                                  
		<div class='price-box' style='text-align:center;'>");
            if (item.ProductDiscountPercentage != 0)
            {
                strbldr.Append(@"     <div style='float:left; font-size:13px; color:#999999;' > <s>" + data.Symbol + " " + (data.Value * item.ProductOriginalCost).ToSpecialFormatString() + "</s>"
+ " | " + item.ProductDiscountPercentage.ToSpecialFormatString() + @"%off  </span> </div> ");
            }
            else
            {
                strbldr.Append(@"<div></div>");
            }

            strbldr.Append(@"  <div style='align:center;'> <span style='color:#D9387E; font-size:19px; font-weight:bold;'>"
+ data.Symbol + " " + (data.Value * item.ProductCost).ToSpecialFormatString()
+ @"</span></div> </div>                     
					<div class='product_icons' align='center'>  
<table><tr><td style='padding-top:15px;'> <button type='button' title='Add to Cart' class='button btn-cart' href='" + ApiUrl + "product/" + item.ProductName + @"/" + item.ProductId + @"' onclick='NavigateToDetails(" + item.ProductId + @")'><span><span>Buy Now</span></span></button></td>
<td  style='padding-top:0px;'> <ul class='add-to-links'><li><a  title='Add to Wishlist' class='link-wishlist' onclick='addToWishList(" + item.ProductId + @")'>Add to Wishlist</a></li></td></tr></table>               
	 <ul>   </ul>
					</div>
					</div>
				</li>");
        }
        return strbldr.ToString();
    }


    [HttpGet]
    public CustomResponse ShowRelatedProductsList()
    {
        try
        {
            var items = (List<RelatedProduct>)BalUtility.GetSession(Shared.Sessions.RelatedProductList) ??
                               new List<RelatedProduct>();
            StringBuilder strResult = new StringBuilder();
            if (items.Count != 0)
            {
                strResult.Append(@"<table>");
                foreach (var item in items)
                {
                    strResult.Append(@"<tr>");
                    strResult.Append(@"<td>" + item.RelatedProductId + "</td>");
                    var repostry = new ProductRepository();
                    var ReltdPrdctDetails = repostry.First(r => r.ProductId == item.RelatedProductId);
                    strResult.Append(@"<td>" + ReltdPrdctDetails.ProductName + "</td>");
                    strResult.Append(@"<td> <a  style='cursor:pointer;'  fieldvalue='" +
                           item.RelatedProductId + "' onclick='removeRelatedProduct(" + item.RelatedProductId + "," + item.ProductId + ")'>  Delete</a> </td>");
                    strResult.Append(@"</tr>");
                }
                strResult.Append(@"</table>");
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = "",
                    Result = strResult.ToString()


                };
            }
            else
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = "",
                    Result = "No Products"


                };
            }

        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }
    }

    [HttpGet]
    public CustomResponse ShowFreeProductsList()
    {
        try
        {
            var items = (List<Product>)BalUtility.GetSession(Shared.Sessions.FreeProductList) ??
                               new List<Product>();
            StringBuilder strResult = new StringBuilder();
            if (items.Count != 0)
            {
                strResult.Append(@"<table>");
                foreach (var item in items)
                {
                    strResult.Append(@"<tr>");
                    strResult.Append(@"<td>" + item.ProductId + "</td>");
                    var repostry = new ProductRepository();
                    var ReltdPrdctDetails = repostry.First(r => r.ProductId == item.ProductId);
                    strResult.Append(@"<td>" + ReltdPrdctDetails.ProductName + "</td>");
                    strResult.Append(@"<td> <a  style='cursor:pointer;'  fieldvalue='" +
                           item.ProductId + "' onclick='removeFreeProduct(" + item.ProductId + ")'>  Delete</a> </td>");
                    strResult.Append(@"</tr>");
                }
                strResult.Append(@"</table>");
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = "",
                    Result = strResult.ToString()


                };
            }
            else
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = "",
                    Result = "No Products"


                };
            }

        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }
    }

    [HttpGet]
    public dynamic getProductImages(int PrdId)
    {
        try
        {
            RelatedProductRepository GetData = new RelatedProductRepository();
            var getimgs = GetData.GetImagesbyProductId(PrdId);
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Message = "",
                Result = getimgs,


            };
        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }
    }


    //transaction details added by gopi
    [HttpGet]
    public dynamic Transactiondetails(string sidx, string sord, int page, int rows)
    {
        var repository = new PaymentTransactionRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, 0, null, c => new { c.PaymentTransactionId, c.UserId, c.PGTxnId, c.TxnStatus, c.TxnMessage, c.TxnAmount }).ToList().OrderByDescending(c => c.PaymentTransactionId);

        return new
        {
            total = totalRecords / rows + 1,
            page,
            records = totalRecords,
            rows = data
        };
    }
    //end transaction details


    public CustomResponse CheckPromoCode(string PromoCode, string OrderAmount)
    {
        try
        {
            db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();
            Decimal Amount = Convert.ToDecimal(OrderAmount);

            Tbl_Promo_Codes PromocodeData = Entity.Tbl_Promo_Codes.Where(x => x.PromoCode == PromoCode && x.Valid_From < DateTime.Now && x.IsActive == true).First();

            if (PromocodeData.Valid_To > DateTime.Now)
            {
                if (PromocodeData.Minimum_Order_amount <= Amount)
                {
                    if (PromocodeData != null)
                    {
                        HttpContext.Current.Session["Promocode"] = PromocodeData;
                    }

                    return new CustomResponse
                    {
                        Message = "Promocode Successfully Added.",
                        Result = ((Amount) - (PromocodeData.Amount)),
                        Status = Shared.ResponseStatus.Success.ToString()
                    };
                }
                else
                {
                    return new CustomResponse
                    {
                        Message = "Minimum Order Amount for this Promocode is: " + PromocodeData.Minimum_Order_amount,
                        Result = OrderAmount,
                        Status = Shared.ResponseStatus.Fail.ToString()
                    };
                }

            }
            else
            {
                return new CustomResponse
                {
                    Message = "Promocode expired",
                    Result = OrderAmount,
                    Status = Shared.ResponseStatus.Fail.ToString()
                };
            }

        }
        catch (Exception ex)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = "Invalid Promocode",
                Result = OrderAmount
            };
        }
    }

    /// <summary>
    /// Written by tulasi to get categoryinputdata kept in session
    /// </summary>
    /// <returns></returns>
    public string GetCategoryInputData()
    {
        return (HttpContext.Current.Session["CategoryInputData"] == null ? "" : HttpContext.Current.Session["CategoryInputData"].ToString());
    }
    /// <summary>
    /// Written by tulasi to get Supercaetgory input data kept in session
    /// </summary>
    /// <returns></returns>
    public string GetSuperCategoryInputData()
    {
        return (HttpContext.Current.Session["SuperCategoryInputData"] == null ? "" : HttpContext.Current.Session["SuperCategoryInputData"].ToString());
    }

    public string GetProductInputData()
    {
        return (HttpContext.Current.Session["ProductInputData"] == null ? "" : HttpContext.Current.Session["ProductInputData"].ToString());
    }

    [HttpGet]
    public CustomResponse ViewMoreProducts(int SuperCategoryID)
    {
        string ApiUrl = System.Configuration.ConfigurationManager.AppSettings["ApiUrl"];
        var data = (CustomCurrency)BAL.BalUtility.GetSession(Shared.Sessions.Currency);
        int cnt = 1;
        var catRepository = new CategoryRepository();
        var ProductsByCategoryGroup = catRepository.GetProductsBySuperCatGroup(SuperCategoryID);

        StringBuilder strCotent = new StringBuilder();

        foreach (var Catitem in ProductsByCategoryGroup)
        {
            cnt = 1;

            var pitems = (IEnumerable<Object>)Catitem.GetType().GetProperty("products").GetValue(Catitem, null);
            if (pitems.Count() == 4)
            {
                strCotent.Append(@"<div class='new_products'>
	<div class='home_product sixteen columns'><h2 class='line_heading'><span>" + Catitem.GetType().GetProperty("CategoryName").GetValue(Catitem, null) + @"</span> <a class='viewall' title='View All' href='" + ApiUrl + Catitem.GetType().GetProperty("SuperCategoryName").GetValue(Catitem, null) + "/" + Catitem.GetType().GetProperty("SuperCategoryId").GetValue(Catitem, null) + "/" + Catitem.GetType().GetProperty("CategoryName").GetValue(Catitem, null) + "/" + Catitem.GetType().GetProperty("CategoryId").GetValue(Catitem, null) + @"'>View all</a> </h2>
	<ul class='products-grid'>");

                foreach (var item in pitems)
                {
                    strCotent.Append("");

                    string ProductName = item.GetType().GetProperty("ProductName").GetValue(item, null).ToString();
                    string ProductFullName = ProductName;
                    ProductFullName = ProductFullName.Replace(" ", "-");
                    if (ProductName.Length > 33)
                    {
                        ProductName = ProductName.Substring(0, 30) + " ...";
                    }

                    strCotent.Append(@"  
						<li class='item last quick10'>                       
							 <div class='outer_pan'>
					<div class='image_rotate'>
					<div >
						<a  href='" + ApiUrl + "product/" + ProductFullName + @"/" + item.GetType().GetProperty("ProductId").GetValue(item, null) + @"' class='product-image'><img  height='210px' src= '" + item.GetType().GetProperty("ProductImgUrl").GetValue(item, null).ToString().Replace("~/", ApiUrl) + @"' alt='" + item.GetType().GetProperty("ProductName").GetValue(item, null) + @"' /></a>
					</div>
					</div>");
                    int prdctdiscountPercntge = Convert.ToInt32(item.GetType().GetProperty("ProductDiscountPercentage").GetValue(item, null));
                    if (prdctdiscountPercntge != 0)
                    {
                        strCotent.Append(@"<div class='product-img-box'>
				<div class='badge'>			
					 
									</div>
						 </div>");
                    }


                    strCotent.Append(@"</div>
					<div class='outer_bottom'>
				   <h2 class='product-name'><a  href='" + ApiUrl + "product/" + ProductFullName + @"/" + item.GetType().GetProperty("ProductId").GetValue(item, null) + @"' title='" + ProductFullName + @"'>" + ProductName + @"</a></h2>
		  
				  
					  <div class='price-box' style='text-align:center;'>");
                    var proddiscount = Convert.ToDecimal(item.GetType().GetProperty("ProductDiscountPercentage").GetValue(item, null));

                    if (proddiscount != 0)
                    {
                        strCotent.Append(@"<div style='float:left; font-size:13px; color:#999999;'><s>" + data.Symbol + "" + (data.Value * Convert.ToDecimal(item.GetType().GetProperty("ProductOriginalCost").GetValue(item, null))).ToSpecialFormatString() + "</s>"
                                                                 + " |<span>" + Convert.ToDecimal(item.GetType().GetProperty("ProductDiscountPercentage").GetValue(item, null)).ToSpecialFormatString() + @"%off </span></div> ");
                    }
                    else
                    {
                        strCotent.Append(@"<div></div>");
                    }
                    strCotent.Append(@"<div style='align:center;'><span style='color:#D9387E; font-size:18px; font-weight:bold;'>"
                                                 + data.Symbol + "" + (data.Value * Convert.ToDecimal(item.GetType().GetProperty("ProductCost").GetValue(item, null))).ToSpecialFormatString()
                                                                  + @"</span></div></div>

<div class='product_icons' align='center'>  
<table><tr>
<td  style='padding-top:0px;'> <ul class='add-to-links'><li><a  title='Add to Wishlist' class='link-wishlist' onclick='addToWishList(" + item.GetType().GetProperty("ProductId").GetValue(item, null) + @")'>Add to Wishlist</a></li></ul></td></tr></table>               
	
					</div>
					</div>
 
				</li>");
                    cnt++;

                }
                strCotent.Append(@" </ul></div></div>");
            }
        }
        //ltProductsGroupbyCat.Text = strCotent.ToString();
        return new CustomResponse
        {
            Result = strCotent.ToString(),
            Status = "Success"
        };
    }

    [HttpGet]
    public void DownloadExcel(string OrderIDs)
    {
        try
        {
            StringBuilder tableData = new StringBuilder();
            StringBuilder strResult = new StringBuilder();
            System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
            string constr = ConfigurationManager.ConnectionStrings["db_Zon_constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string[] ids = OrderIDs.Split(',');
                foreach (string id in ids)
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * from PaymentTransactions where PaymentTransactionId=" + id))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                string attachment = "attachment; filename=city.xls";
                                Response.ClearContent();
                                Response.AddHeader("content-disposition", attachment);
                                Response.ContentType = "application/vnd.ms-excel";
                                string tab = "";
                                foreach (DataColumn dc in dt.Columns)
                                {
                                    Response.Write(tab + dc.ColumnName);
                                    tab = "\t";
                                }
                                Response.Write("\n");
                                int i;
                                foreach (DataRow dr in dt.Rows)
                                {
                                    tab = "";
                                    for (i = 0; i < dt.Columns.Count; i++)
                                    {
                                        Response.Write(tab + dr[i].ToString());
                                        tab = "\t";
                                    }
                                    Response.Write("\n");
                                }
                                Response.End();
                            }
                        }
                    }
                }
            }

        }
        catch (Exception ex)
        {
        }
    }

    [HttpGet]
    public CustomResponse GetCheckoutSearchorders(int? rows, long? PaymentTransactionId, DateTime? CreatedOn, DateTime? UpdatedOn, string OrderStatus)
    {
        var repository = new PaymentTransactionRepository();
        //int totalRecords;

        db_Zon_HuwEntities Context = new db_Zon_HuwEntities();

        List<CheckOutPaymentTransaction> data = new List<CheckOutPaymentTransaction>();
        DateTime UpdatedDate = Convert.ToDateTime("1/1/0001 12:00:00 AM");
        DateTime CreatedDate = Convert.ToDateTime("1/1/0001 12:00:00 AM");
        if (UpdatedOn != null || UpdatedOn != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
        {
            UpdatedOn = UpdatedOn.Value.AddDays(1);
        }

        if (!string.IsNullOrEmpty(PaymentTransactionId.ToString()) || OrderStatus != "Null" || CreatedOn != CreatedDate)
        {
            data = (from p in Context.CheckOutPaymentTransactions
                    where p.OrderStatus != "Stale"
                    && ((PaymentTransactionId == 0 || PaymentTransactionId == null) ? true : (p.PaymentTransactionId == PaymentTransactionId))
                    && (OrderStatus == "Null" ? true : (p.OrderStatus == OrderStatus))
                    && (CreatedOn == CreatedDate ? true : (p.CreatedOn >= CreatedOn && p.CreatedOn <= UpdatedOn))
                    select p)
                    .OrderByDescending(x => x.PaymentTransactionId)
                    .Take(rows.Value)
                    .ToList();
        }
        else
        {
            data = (from p in Context.CheckOutPaymentTransactions
                    where p.OrderStatus != "Stale"
                    select p)
                   .OrderByDescending(x => x.PaymentTransactionId)
                   .Take(rows.Value)
                   .ToList();
        }

        StringBuilder strtd = new StringBuilder();
        StringBuilder strResult = new StringBuilder();
        if (data.Count > 0)
        {
            foreach (var item in data)
            {
                try
                {
                    db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();
                    string statusvalue = item.OrderCurrentStatus.ToString();
                    var status = Shared.GetOrderStatusEnum(statusvalue);

                    StringBuilder ProductName = new StringBuilder();

                    for (var i = 0; i < item.CheckOutUserProductTransactions.Count; i++)
                    {
                        if (i == 0)
                        {
                            ProductName.Append("<a target='_blank' title='Remaining Quantity: " + item.CheckOutUserProductTransactions.ElementAtOrDefault(i).Product.Quantity + "' style='color:blue;' href='/Admin/UpdateProducts.aspx?ID=" + item.CheckOutUserProductTransactions.ElementAtOrDefault(i).Product.ProductId + "'>" + item.CheckOutUserProductTransactions.ElementAtOrDefault(i).Product.ProductName + "</a>");
                        }
                        else
                        {
                            ProductName.Append(", " + "<a target='_blank' title='Remaining Quantity: " + item.CheckOutUserProductTransactions.ElementAtOrDefault(i).Product.Quantity + "' style='color:blue;' href='/Admin/UpdateProducts.aspx?ID=" + item.CheckOutUserProductTransactions.ElementAtOrDefault(i).Product.ProductId + "'>" + item.CheckOutUserProductTransactions.ElementAtOrDefault(i).Product.ProductName + "</a>");
                        }
                    }

                    string Payment = "";
                    if (item.PaymentMode == "Cash On Delivery")
                    {
                        Payment = "Postpaid";
                    }
                    else if (item.PaymentMode == "Paytm")
                    {
                        Payment = "Paytm";
                    }
                    else if (item.PaymentMode == "Payumoney")
                    {
                        Payment = "Paytm";
                    }
                    else
                    {
                        Payment = "Citrus";
                    }

                    strtd.Append(@"<tr><td>
												<a id='hypEdit'  onclick='NavigatetoCheckoutOrderDetailsPage(" + item.PaymentTransactionId + ")' >" + item.PaymentTransactionId + @"</a><br>
												<span id='lblPartialShip'></span>
											</td><td>
												<span >" + item.CreatedOn.ToShortDateString() + @"</span>   
											</td>
												<td>
												<span >" + ProductName + @"</span>   
											</td>
<td>                                                
													<span id='lblTotalProducts'>" + item.CheckOutUserProductTransactions.Count + @"</span>                                                
											</td>
<td>                                                
													<span id='lblTotalProducts'>" + item.User.FirstName + @"</span>                                                
											</td>
<td>                                                
													<span id='lblTotalProducts'>" + Payment + @"</span>                                                
											</td>
<td>                                                
												<span id='lblOrderStatus'>" + item.OrderStatus + @"</span> 
											</td>
<td>                                                
												<span id='lblcustName'><span>" + item.CurrencySymbol + @" </span>" + String.Format("{0:0.00}", item.TxnAmount) + @"</span></span>
											</td>
<td style='display:none'>" + item.User.FirstName + @"</td>
<td style='display:none'>" + item.User.MobileNo + @"</td>
<td style='display:none'>" + item.CheckOutUserProductTransactions.FirstOrDefault().UserAddress.StreetAddress1 + " ," + item.CheckOutUserProductTransactions.FirstOrDefault().UserAddress.StreetAddress2 + " ," + item.CheckOutUserProductTransactions.FirstOrDefault().UserAddress.LandMark + " ," + item.CheckOutUserProductTransactions.FirstOrDefault().UserAddress.City + " ," + item.CheckOutUserProductTransactions.FirstOrDefault().UserAddress.PinCode + @"</td>
			</tr>");
                }
                catch (Exception ex)
                {

                }
            }

            strResult.Append(@"
		<div class='widget'>                   
					
					<div class='widget_body'>
							<div id='Main_dvGrid'>
								<div class='widget marg-0'> 
								<div class='widget_title'>
									<span class='iconsweet'>r</span><h5>Available Orders</h5>
								</div>
								<div class='widget_body'>
						
							<div class='cp_productlist_view'>
								<div class='products_views'>
									
								</div>
							</div>
						   
										<table class='activity_datatable' rules='all' id='grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
											<thead>
								<tr>
												<th scope='col'>ID</th><th scope='col'>Date</th><th scope='col'>Product Name</th><th scope='col'>Qty</th><th scope='col'>Customer Name</th><th scope='col'>Payment Mode</th><th scope='col'>Status</th><th scope='col'>Amount</th><th scope='col' style='display:none'>Name</th><th scope='col' style='display:none'>Phone</th><th style='display:none'>Address</th>
											</tr></thead><tbody>
								" + strtd + @"
								</tbody></table>	</div>

  
									</div>
							</div>
							
							
					</div>
				</div>
				<input name='ctl00$ctl00$Main$Main$hdnTodoSearchcriteria' id='ctl00_ctl00_Main_Main_hdnTodoSearchcriteria' value='[&quot;Order No&quot;,&quot;SKU&quot;,&quot;Email Id&quot;,&quot;Phone No&quot;,&quot;Coupon Code&quot;,&quot;Checkout Type&quot;]' type='hidden'>
				<img id='img441' alt='' onload='getdivCurrency();' src='Orders_files/designoption_009_13.jpg' height='1' width='1'>
			
</div>      

			</div>
		</div>");

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strResult.ToString(),
                CartSum = data.Count().ToString()
            };
        }
        else
        {
            strResult.Append(@" <div id='divMessage' class='msgbar msg_Info hide_onC'>
								<span id='msgicon' class='iconsweet'>*</span>
								<p><span id='lblMessage'>There are no 'Orders' found.</span></p>
							</div>");

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = strResult.ToString(),
                CartSum = data.Count().ToString()
            };
        }

    }


    [HttpPost]
    public CustomResponse GetCheckoutSearchordersbyMobile(decimal Mobile)
    {

        var repository = new PaymentTransactionRepository();
        int totalRecords;

        db_Zon_HuwEntities Context = new db_Zon_HuwEntities();

        List<CheckOutPaymentTransaction> data = new List<CheckOutPaymentTransaction>();
        var UID = (from a in Context.Users
                   where a.MobileNo == Mobile
                   select a.UserId).FirstOrDefault();

        StringBuilder strtd = new StringBuilder();
        StringBuilder strResult = new StringBuilder();
        data = (from p in Context.CheckOutPaymentTransactions
                where p.UserId == UID
                select p).OrderByDescending(x => x.PaymentTransactionId).ToList();
        if (data.Count > 0)
        {
            foreach (var item in data)
            {
                try
                {
                    db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();
                    string statusvalue = item.OrderStatus.ToString();
                    var status = Shared.GetOrderStatusEnum(statusvalue);

                    StringBuilder ProductName = new StringBuilder();

                    for (var i = 0; i < item.CheckOutUserProductTransactions.Count; i++)
                    {
                        if (i == 0)
                        {
                            ProductName.Append("<a target='_blank' title='Remaining Quantity: " + item.CheckOutUserProductTransactions.ElementAtOrDefault(i).Product.Quantity + "' style='color:blue;' href='/Admin/UpdateProducts.aspx?ID=" + item.CheckOutUserProductTransactions.ElementAtOrDefault(i).Product.ProductId + "'>" + item.CheckOutUserProductTransactions.ElementAtOrDefault(i).Product.ProductName + "</a>");
                        }
                        else
                        {
                            ProductName.Append(", " + "<a target='_blank' title='Remaining Quantity: " + item.CheckOutUserProductTransactions.ElementAtOrDefault(i).Product.Quantity + "' style='color:blue;' href='/Admin/UpdateProducts.aspx?ID=" + item.CheckOutUserProductTransactions.ElementAtOrDefault(i).Product.ProductId + "'>" + item.CheckOutUserProductTransactions.ElementAtOrDefault(i).Product.ProductName + "</a>");
                        }
                    }

                    string PaymentMode = "";
                    if (item.PaymentMode == "Cash On Delivery")
                    {
                        PaymentMode = "Postpaid";
                    }
                    else if (item.PaymentMode == "Paytm")
                    {
                        PaymentMode = "Paytm";
                    }
                    else if (item.PaymentMode == "Payumoney")
                    {
                        PaymentMode = "Payumoney";
                    }
                    else
                    {
                        PaymentMode = "Citrus";
                    }

                    strtd.Append(@"<tr><td>
												<a id='hypEdit'  onclick='NavigatetoCheckoutOrderDetailsPage(" + item.PaymentTransactionId + ")' >" + item.PaymentTransactionId + @"</a><br>
												<span id='lblPartialShip'></span>
											</td><td>
												<span >" + item.CreatedOn.ToShortDateString() + @"</span>   
											</td>
												<td>
												<span >" + ProductName + @"</span>   
											</td>
<td>                                                
													<span id='lblTotalProducts'>" + item.User.FirstName + @"</span>                                                
											</td>
<td>                                                
													<span id='lblTotalProducts'>" + PaymentMode + @"</span>                                                
											</td>
											 <td>                                                
													<span id='lblstatus'>" + status + @"</span>                                                
											</td>td>                                                
												<span id='lblcustName'><span>" + item.CurrencySymbol + @" </span>" + String.Format("{0:0.00}", item.TxnAmount) + @"</span></span>
											</td>
			</tr>");
                }
                catch (Exception ex)
                {

                }
            }

            strResult.Append(@"
		<div class='widget'>                   
					
					<div class='widget_body'>
							<div id='Main_dvGrid'>
								<div class='widget marg-0'> 
								<div class='widget_title'>
									<span class='iconsweet'>r</span><h5>Available Orders</h5>
								</div>
								<div class='widget_body'>
						
							<div class='cp_productlist_view'>
								<div class='products_views'>
									
								</div>
							</div>
						   
										<table class='activity_datatable' rules='all' id='grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
											<thead>
								<tr>
												<th scope='col'>ID</th><th scope='col'>Date</th><th scope='col'>Product Name</th><th scope='col'>Customer Name</th><th scope='col'>Payment Mode</th><th scope='col'>Status</th><th scope='col'>Amount</th>
											</tr></thead><tbody>
								" + strtd + @"
								</tbody></table>	</div>

  
									</div>
							</div>
							
							
					</div>
				</div>
				<input name='ctl00$ctl00$Main$Main$hdnTodoSearchcriteria' id='ctl00_ctl00_Main_Main_hdnTodoSearchcriteria' value='[&quot;Order No&quot;,&quot;SKU&quot;,&quot;Email Id&quot;,&quot;Phone No&quot;,&quot;Coupon Code&quot;,&quot;Checkout Type&quot;]' type='hidden'>
				<img id='img441' alt='' onload='getdivCurrency();' src='Orders_files/designoption_009_13.jpg' height='1' width='1'>
			
</div>      

			</div>
		</div>");

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strResult.ToString(),
                CartSum = data.Count().ToString()
            };
        }
        else
        {
            strResult.Append(@" <div id='divMessage' class='msgbar msg_Info hide_onC'>
								<span id='msgicon' class='iconsweet'>*</span>
								<p><span id='lblMessage'>There are no 'Orders' found.</span></p>
							</div>");

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = strResult.ToString(),
                CartSum = data.Count().ToString()
            };
        }
    }
    [HttpPost]
    public CustomResponse GetCheckoutSearchordersbyProductName(string ProductName)
    {
        var repository = new PaymentTransactionRepository();
        int totalRecords;
        db_Zon_HuwEntities db = new db_Zon_HuwEntities();

        var ProductIDs = from c in db.Products
                         where c.ProductName.Contains(ProductName)
                         select c.ProductId;
        StringBuilder strResult = new StringBuilder();
        StringBuilder strtd = new StringBuilder();
        foreach (var ProductID in ProductIDs)
        {

            var PaymentTxnIDs = from c in db.CheckOutUserProductTransactions
                                where c.ProductId == ProductID
                                select c.PaymentTransactionId;
            foreach (var TxnID in PaymentTxnIDs)
            {
                var data = (repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, 50, (p => p.PaymentTransactionId == TxnID), null, "", true));
                string PrdctName = db.Products.Where(x => x.ProductId == ProductID).First().ProductName;

                string statusvalue = data.FirstOrDefault().OrderCurrentStatus.ToString();
                var status = Shared.GetOrderStatusEnum(statusvalue);

                strtd.Append(@"<tr><td>
												<a id='hypEdit' onclick='NavigatetoCheckoutOrderDetailsPage(" + data.FirstOrDefault().PaymentTransactionId + ")' >" + data.FirstOrDefault().PaymentTransactionId + @"</a><br>
												<span id='lblPartialShip'></span>
											</td><td>
												<span >" + data.FirstOrDefault().CreatedOn.ToShortDateString() + @"</span>   
											</td><td>
												<span >" + PrdctName + @"</span>   
											</td><td>
												<span >" + data.FirstOrDefault().User.FirstName + @"</span>   
											</td><td>                                                
													<span id='lblTotalProducts'>" + data.FirstOrDefault().PaymentMode + @"</span>                                                
											</td>
												<td>                                                
													<span id='lblTotalProducts'>" + status + @"</span>                                                
											</td><td>                                                
												<span id='lblcustName'><span>" + data.FirstOrDefault().CurrencySymbol + @" </span>" + String.Format("{0:0.00}", data.FirstOrDefault().TxnAmount) + @"</span></span>
											</td>
			</tr>");

            }

        }
        strResult.Append(@"
		<div class='widget'>                   
					
					<div class='widget_body'>
							<div id='Main_dvGrid'>
								<div class='widget marg-0'> 
								<div class='widget_title'>
									<span class='iconsweet'>r</span><h5>Available Orders</h5>
								</div>
								<div class='widget_body'>
						
							<div class='cp_productlist_view'>
								<div class='products_views'>
									
								</div>
							</div>
						   
									<table class='activity_datatable' rules='all' id='grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
										<thead>
							<tr>
											<th scope='col'>ID</th><th scope='col'>Date</th><th scope='col'>Product Name</th><th scope='col'>Customer Name</th><th scope='col'>Payment Mode</th><th scope='col'>Status</th><th scope='col'>Amount</th>
										</tr></thead><tbody>
							" + strtd + @"
							</tbody></table>	</div>

  
									</div>
							</div>
							
							
					</div>
				</div>
				<input name='ctl00$ctl00$Main$Main$hdnTodoSearchcriteria' id='ctl00_ctl00_Main_Main_hdnTodoSearchcriteria' value='[&quot;Order No&quot;,&quot;SKU&quot;,&quot;Email Id&quot;,&quot;Phone No&quot;,&quot;Coupon Code&quot;,&quot;Checkout Type&quot;]' type='hidden'>
				<img id='img441' alt='' onload='getdivCurrency();' src='Orders_files/designoption_009_13.jpg' height='1' width='1'>
			
				</div>     

			</div>
		</div>");
        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Result = strResult.ToString()
        };
    }



    [HttpGet]
    public CustomResponse Cashbacktransactionsajaxcall(string userid, string tranasactionID, string datefrom, string datetill)
    {
        var repository = new PaymentTransactionRepository();
        int totalRecords;
        db_Zon_HuwEntities db = new db_Zon_HuwEntities();


        //initially setting date from and date till to largest possible dates( PL/SQL: from January 1, 4712 BC to December 31, 9999 AD.)
        DateTime startdate = String.IsNullOrEmpty(datefrom) ? Convert.ToDateTime("-4712-11-14 18:00:44.227") : Convert.ToDateTime(datefrom);
        DateTime enddate = String.IsNullOrEmpty(datetill) ? Convert.ToDateTime("9999-11-14 18:00:44.227") : Convert.ToDateTime(datetill);

        //conditions for filters
        //IQueryable<CashBackTable> data = new IQueryable<CashBackTable>();
        IQueryable<CashBackTable> data;
        if (userid != null)
        {
            data = from c in db.CashBackTables
                   where c.Userid == userid
                   && (c.date >= startdate && c.date <= enddate)
                   orderby c.date ascending
                   select c;
        }
        else if (tranasactionID != null)
        {
            data = from c in db.CashBackTables
                   where c.orderid == tranasactionID
                   && (c.date >= startdate && c.date <= enddate)
                   orderby c.date ascending
                   select c;
        }
        else if (userid != null && tranasactionID != null)
        {
            data = from c in db.CashBackTables
                   where c.orderid == tranasactionID
                   && (c.date >= startdate && c.date <= enddate)
                   orderby c.date ascending
                   select c;
        }
        else
        {
            data = from c in db.CashBackTables
                   where (c.date >= startdate && c.date <= enddate)
                   orderby c.date ascending
                   select c;
        }

        StringBuilder strcbt = new StringBuilder();
        foreach (var item in data)
        {
            strcbt.Append(@"<table><thead><tr><td>
										    <a id='hypEdit'</a><br>
												<span id='lblPartialShip'></span>
											</td><td>
												<span >" + item.orderid + @"</span>   
											</td><td>
												<span >" + item.date + @"</span>   
											</td><td>
												<span >" + item.totalAmount + @"</span>   
											</td><td> 
												<span >" + item.credit + @"</span>   
											</td><td>
												<span >" + item.debit + @"</span>   
											</td><td>
												<span >" + item.balance + @"</span>   
											</td><td>
												<span >" + item.Userid + @"</span>   
											</td><td>
												<span >" + item.PGTxnid + @"</span>   
											</td><td>
												<span >" + item.Messages + @"</span>
                                            </td><td>                                              
											</td>
												<td>                                                
													<span id='lblTotalProducts'></span>                                                
											</td><td>                                                
												<span id='lblcustName'><span></span></span>
											</td>
			</tr></thead><tbody></tbody></table>");
        }











        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Result = strcbt.ToString()
        };

    }
    [HttpGet]
    public CustomResponse Cashbacktransactionsajaxcall2(string userid, string tranasactionID, string datefrom, string datetill)
    {


        db_Zon_HuwEntities db = new db_Zon_HuwEntities();


        //initially setting date from and date till to largest possible dates( PL/SQL: from January 1, 4712 BC to December 31, 9999 AD.)
        DateTime startdate = String.IsNullOrEmpty(datefrom) ? Convert.ToDateTime("-4712-11-14 18:00:44.227") : Convert.ToDateTime(datefrom);
        DateTime enddate = String.IsNullOrEmpty(datetill) ? Convert.ToDateTime("9999-11-14 18:00:44.227") : Convert.ToDateTime(datetill);

        //conditions for filters
        //IQueryable<CashBackTable> data = new IQueryable<CashBackTable>();
        IQueryable<CashBackTable> data;
        if (userid != null)
        {
            data = from c in db.CashBackTables
                   where c.Userid == userid
                   && (c.date >= startdate && c.date <= enddate)
                   orderby c.date ascending
                   select c;
        }
        else if (tranasactionID != null)
        {
            data = from c in db.CashBackTables
                   where c.orderid == tranasactionID
                   && (c.date >= startdate && c.date <= enddate)
                   orderby c.date ascending
                   select c;
        }
        else if (userid != null && tranasactionID != null)
        {
            data = from c in db.CashBackTables
                   where c.orderid == tranasactionID
                   && (c.date >= startdate && c.date <= enddate)
                   orderby c.date ascending
                   select c;
        }
        else
        {
            data = from c in db.CashBackTables
                   where (c.date >= startdate && c.date <= enddate)
                   orderby c.date ascending
                   select c;
        }
        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Result = data.ToList()
        };

    }

    public CustomResponse GetCheckoutProductOverview(int transId)
    {
        var repository = new CheckoutUserProductTransactionRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, 0, (p => p.PaymentTransactionId == transId), null, "", true);
        StringBuilder strResult = new StringBuilder();
        StringBuilder strtd = new StringBuilder();
        StringBuilder strRefundAmount = new StringBuilder();
        db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();
        if (data.Count > 0)
        {
            var TxnStatus = "";
            var TxnMessage = "";
            var RefundAmount = "";

            foreach (var item in data)
            {
                string ProductName = "";
                decimal ProductCost = 0;
                if (item.SubProductId == null)
                {

                    ProductName = item.Product.ProductName;
                    ProductCost = item.Product.ProductCost;
                }
                else
                {
                    SubProduct Sp = Entity.SubProducts.Where(x => x.SubProductId == item.SubProductId).First();
                    ProductName = item.Product.ProductName + "(" + Sp.SPName + ")";
                    ProductCost = Sp.ProductCost;
                }

                strtd.Append(@"<tr>
				<td style='width:45%;'>
							<div class='product_image fl_left'>
								<img src=" + item.Product.ProductImgUrl.Replace("~/", ApiUrl) + @" id='imgProductImage'>
							</div>
							<div class='product_info fl_left'>
								<ul>
									<li>
										<div class='title'>
										" + ProductName + @"
									 </div>
									</li>
									<li class='topmargin_15'>
										<span id='lblSKUText'>SKU: " + item.Product.ProductId + @" </span>                                        
									</li>                                   
									<li>
										<br>
									</li>
								</ul>
							</div>
						</td><td>                            
							<span id='lblProdQuantity' style='padding-left: 3px;'>" + item.Quantity + @"</span><br>
						</td><td>
							<span id='ProductCost'> <span class='WebRupee'>Rs. </span> " + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(ProductCost), true) + @"</span>
						</td><td>                          
							<span id='ctl00_ctl00_Main_Main_rptSuppliers_ctl01_grdProducts_ctl02_lblTotalPrice'> <span class='WebRupee'>Rs. </span> " + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(item.ProductCost)) + @"</span>
						</td></tr>");

                DateTime CreatedOn = data.FirstOrDefault().CheckOutPaymentTransaction.CreatedOn;
                DateTime CurrentDate = DateTime.Now;
                TimeSpan span = CurrentDate.Subtract(CreatedOn);

                if (span.Minutes > 15 && data.FirstOrDefault().CheckOutPaymentTransaction.TxnStatus == null && data.FirstOrDefault().CheckOutPaymentTransaction.TxnMessage == null)
                {
                    TxnStatus = "No response from Payment Gateway";
                    TxnMessage = "No response from Payment Gateway";
                }
                else
                {
                    TxnStatus = data.FirstOrDefault().CheckOutPaymentTransaction.TxnStatus;
                    TxnMessage = data.FirstOrDefault().CheckOutPaymentTransaction.TxnMessage;
                }
                if (item.CheckOutPaymentTransaction.OrdersReturnAction == "Refund")
                {
                    TxnStatus = "Amount Refunded";
                    RefundAmount = BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(data.FirstOrDefault().CheckOutPaymentTransaction.TxnAmount));
                    strRefundAmount.Append(@"<span>Refunded Amount : <span style='font-size:16px; color:green;'>" + RefundAmount + @"</span></span><br/>");
                }
            }

            if (data.FirstOrDefault().CheckOutPaymentTransaction.Free_Product_ID != null)
            {
                long freeProductId = data.FirstOrDefault().CheckOutPaymentTransaction.Free_Product_ID.Value;

                Product free = Entity.Products.Where(x => x.ProductId == freeProductId).First();

                strtd.Append(@"<tr>
				<td style='width:45%;'>
							<div class='product_image fl_left'>
								<img src=" + free.ProductImgUrl.Replace("~/", ApiUrl) + @" id='imgProductImage'>
							</div>
							<div class='product_info fl_left'>
								<ul>
									<li>
										<div class='title'>
										" + free.ProductName + @"
									 </div>
									</li>
									<li class='topmargin_15'>                                        
										
									</li>
								   
									<li>
										<br>
									</li>
								</ul>
							</div>
						</td><td>                            
							<span id='lblProdQuantity' style='padding-left: 3px;'>1</span><br>
						</td><td>
							<span id='ProductCost'> <span class='WebRupee'>Rs. </span> " + free.ProductCost + @"</span>
						</td><td>                          
							<span style='font-weight:bold;' id='ctl00_ctl00_Main_Main_rptSuppliers_ctl01_grdProducts_ctl02_lblTotalPrice'> FREE</span>
						</td>
			</tr>");
            }


            if (data.FirstOrDefault().CheckOutPaymentTransaction.Offer_Product_ID != null)
            {
                int? ProductId = data.FirstOrDefault().CheckOutPaymentTransaction.Offer_Product_ID;

                Tbl_Offered_Products Offer = Entity.Tbl_Offered_Products.Where(x => x.Offer_Product_ID == ProductId).First();

                strtd.Append(@"<tr>
				<td style='width:45%;'>
							<div class='product_image fl_left'>
								<img src=" + Offer.Product_Image + @" id='imgProductImage'>
							</div>
							<div class='product_info fl_left'>
								<ul>
									<li>
										<div class='title'>
										" + Offer.Offer_Product_Name + @"
									 </div>
									</li>
									<li class='topmargin_15'>                                        
										
									</li>
								   
									<li>
										<br>
									</li>
								</ul>
							</div>
						</td><td>
							
							<span id='lblProdQuantity' style='padding-left: 3px;'>1</span><br>
							
							
						</td><td>
							<span id='ProductCost'> <span class='WebRupee'>Rs. </span> " + Offer.Product_Actual_Cost + @"</span>
						</td><td>                          
							<span style='font-weight:bold;' id='ctl00_ctl00_Main_Main_rptSuppliers_ctl01_grdProducts_ctl02_lblTotalPrice'> FREE</span>
						</td>
			</tr>");

            }


            string statusvalue = data.FirstOrDefault().CheckOutPaymentTransaction.OrderCurrentStatus.ToString();
            var status = Shared.GetOrderStatusEnum(statusvalue);

            StringBuilder strcode = new StringBuilder();
            StringBuilder strcod = new StringBuilder();
            StringBuilder strShipment = new StringBuilder();
            StringBuilder strOrderStatus = new StringBuilder();

            //if (status == Shared.OrderStatus.Delivered || status == Shared.OrderStatus.Cancelled || status == Shared.OrderStatus.Refund || status == Shared.OrderStatus.Pending || status == Shared.OrderStatus.Returns)
            //{
            //    if (status == Shared.OrderStatus.Pending || status == Shared.OrderStatus.Cancelled)
            //    {
            strOrderStatus.Append(@"<input  value='Make Order Success'  class='button_small greyishBtn  fl_right' onclick='MakeOrderSuccess()'/>
<input value='Close Order' onclick='CloseOrder()' class='button_small greyishBtn  fl_right' type='button'>");
            //           }
            //       }
            //       else
            //       {
            //           strOrderStatus.Append(@"<input  value='Return/Refund'  class='button_small greyishBtn  fl_right' onclick='Redirect()'>
            //<input value='Cancel Order' onclick='CancelOrder()' class='button_small greyishBtn  fl_right' type='button'>
            //               <input value='Track Shipment' onclick='Trackshipment(" + data.FirstOrDefault().CheckOutPaymentTransaction.ShipmentId + ")' class='button_small greyishBtn  fl_right' type='button'>");
            //       }
            string HasPromocode = "";
            CheckoutCommentRepository repo = new CheckoutCommentRepository();
            List<Tbl_CheckoutOrderComments> orderComments = repo.GetCheckoutOrderComments(transId);
            StringBuilder strOrderComments = new StringBuilder();
            StringBuilder strComment = new StringBuilder();
            for (int i = 0; i < orderComments.Count; i++)
            {
                strComment.Append(@"<p>" +
                 +(i + 1) + @") Comment: " + orderComments[i].Comment + @"<br> Date: " + orderComments[i].Comment_Date + @"<br>                 
				</p>");
            }
            if (orderComments.Count != 0)
            {
                strOrderComments.Append(@"
			<div class='widget'>
				<div class='widget_title'><span id = 'spnShipAddress' class='iconsweet'>r</span><h5>  Comments</h5></div>
				<div class='widget_body'>
				<div class='content_pad'>
				" + strComment.ToString() + @"
				</div>
				</div>
			</div>
		</div>  ");
            }

            if (data.FirstOrDefault().CheckOutPaymentTransaction.Has_Promo_Code == true)
            {
                int PromocodeID = data.FirstOrDefault().CheckOutPaymentTransaction.Promo_Code_ID.Value;
                Tbl_Coupon_Info PromocodeData = Entity.Tbl_Coupon_Info.Where(x => x.Coupon_Id == PromocodeID).First();
                HasPromocode = PromocodeData.Coupon_Code;

                strcode.Append(@"<tr id='trPromocode'>
									<td class='field_caption '> Promocode Amount: </td>
								   <td class='field_value text_right'> <span id='lblGrandTotalPrice'> <span class='WebRupee'>Rs. </span>" + data.FirstOrDefault().CheckOutPaymentTransaction.Promo_Code_Amount + @"</span></td>
								</tr> 
							   <tr class='ordertotal'>
							<td class='field_caption text_right'>
							   Grand Total :
							</td>
							<td class='field_value text_right'>
								<span id='lblGrandTotalPrice'> <span class='WebRupee'>Rs. </span> " + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(data.FirstOrDefault().CheckOutPaymentTransaction.TxnAmount)) + @"</span>
							</td>
						</tr>
						<tr id='trPromo'>
									<td class='field_caption '>Applied Promocode: </td>
								   <span id='lblGrandTotalPrice'> <td class='field_value'>" + HasPromocode + @"</span></td>
								</tr> ");

            }
            else
            {
                HasPromocode = "No Promocode Applied";
                strcode.Append(@"<tr id='trPromocode'>		                            
								</tr>
							<tr class='ordertotal'>
							<td class='field_caption text_right'>
							   Grand Total :
							</td>
							<td class='field_value text_right'>
								<span id='lblGrandTotalPrice'> <span class='WebRupee'>Rs. </span> " + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(data.FirstOrDefault().CheckOutPaymentTransaction.TxnAmount)) + @"</span>
							</td>
						</tr>");
            }

            if (data.FirstOrDefault().CheckOutPaymentTransaction.ShipmentId != null)
            {
                strShipment.Append(@"<div class='one_two_wrap fl_left'><div class='widget'>
					<div class='widget_title'><span id='spnBillAddress' class='iconsweet'>r</span>
					<h5>Shipment Details</h5></div>
					<div class='widget_body'><div class='content_pad'>
					   Shipment ID: " + data.FirstOrDefault().CheckOutPaymentTransaction.ShipmentId + @"<br /><br /> Courier Name:" + data.FirstOrDefault().CheckOutPaymentTransaction.CourierName + "<br /><br /> PickUp ID:" + data.FirstOrDefault().CheckOutPaymentTransaction.PickupID + @"<br /><br />
					</p></div></div></div></div>"
                );
                if (data.FirstOrDefault().CheckOutPaymentTransaction.ShipmentId != "EcomTest")
                {
                    if (data.FirstOrDefault().CheckOutPaymentTransaction.CourierName == "Ecom Express")
                    {

                        strShipment.Append(@"<!-- iframe accordian -->
             <div class='one_two_wrap fl_right' style='width: 100%'>
                <div class='widget'>
                    <div class='widget_title min'>
                        <span id=''class='iconsweet'>r</span><h5>Ecom Tracking</h5>
                    </div>
                    <div class='widget_body max'>
                        <div class='content_pad'>
                           <iframe id = 'load' width='100%' src='http://ecomexpress.in/tracking/?awb_field=" + data.FirstOrDefault().CheckOutPaymentTransaction.ShipmentId + @"' height='250'></iframe>
                        </div>
                    </div>
                </div>
            </div>
 <div class='plus'><i class='fa fa-plus'></i></div>
            <div class='minus' style='display':none'><i class='fa fa-minus'></i></div>
            <!-- iframe accordian -->"
                        );
                    }
                    else if (data.FirstOrDefault().CheckOutPaymentTransaction.CourierName == "Delhivery")
                    {
                        strShipment.Append(@"<!-- iframe accordian -->
             <div class='one_two_wrap fl_right' style='width: 100%'>
                <div class='widget'>
                    <div class='widget_title min'>
                        <span id=''class='iconsweet'>r</span><h5>Ecom Tracking</h5>
                    </div>
                    <div class='widget_body max'>
                        <div class='content_pad'>
                           <iframe id = 'load' width='100%' src='https://trackingi.com/delhivery/id-" + data.FirstOrDefault().CheckOutPaymentTransaction.ShipmentId + @"?awb=" + data.FirstOrDefault().CheckOutPaymentTransaction.ShipmentId + @"' height='250'></iframe>
                        </div>
                    </div>
                </div>
            </div>
 <div class='plus'><i class='fa fa-plus'></i></div>
            <div class='minus' style='display':none'><i class='fa fa-minus'></i></div>
            <!-- iframe accordian -->"
);
                    }
                }
            }
            string PaymentMode = "";
            if (data.FirstOrDefault().CheckOutPaymentTransaction.PaymentMode == "Paytm")
            {
                PaymentMode = "Paytm";
            }
            else if (data.FirstOrDefault().CheckOutPaymentTransaction.PaymentMode == "Cash On Delivery")
            {
                PaymentMode = "Postpaid";
            }
            else if (data.FirstOrDefault().CheckOutPaymentTransaction.PaymentMode == "Payumoney")
            {
                PaymentMode = "Payumoney";
            }
            else
            {
                PaymentMode = "Citrus";
            }

            StringBuilder strStatus = new StringBuilder();
            StringBuilder strOrderResult = new StringBuilder();

            if (data.FirstOrDefault().CheckOutPaymentTransaction.TxnStatus != "SUCCESS")
            {
                if (PaymentMode != "Paytm")
                {
                    strStatus.Append("<input type='button' class='btn btn-orange' name='btnSub' value='Get PaymentGateway Status' onclick='JavaScript: onGetTransDetails_Submit();' />");
                }
            }
            else
            {
                strOrderResult.Append(@"<ul id='progressbar'>");
                if (data.FirstOrDefault().CheckOutPaymentTransaction.Authorized == true)
                {
                    strOrderResult.Append(@"<li class='active' >
												 <a id='link1' title='Authorized by " + data.FirstOrDefault().CheckOutPaymentTransaction.CreatedOn.ToShortDateString() + @"'>Authorized <br/> " + data.FirstOrDefault().CheckOutPaymentTransaction.CreatedOn.ToShortDateString() + @" (0 Days)</a>
											  </li>");
                }
                if (data.FirstOrDefault().CheckOutPaymentTransaction.Pickup == true)
                {
                    DateTime d1 = data.FirstOrDefault().CheckOutPaymentTransaction.ShipmentDate.Value;
                    DateTime d2 = data.FirstOrDefault().CheckOutPaymentTransaction.CreatedOn;

                    TimeSpan t = d1 - d2;
                    double NrOfDays = t.TotalDays;
                    NrOfDays = Math.Round(NrOfDays);

                    strOrderResult.Append(@"<li class='active' title='Pickup by " + data.FirstOrDefault().CheckOutPaymentTransaction.ShipmentDate.Value.ToShortDateString() + @"'>
												 <a id='link1' title='Pickup by " + data.FirstOrDefault().CheckOutPaymentTransaction.ShipmentDate.Value + @"'>Pickup <br/>" + data.FirstOrDefault().CheckOutPaymentTransaction.ShipmentDate.Value.ToShortDateString() + @" (" + NrOfDays + @" Days)</a>
											</li>");
                }
                else
                {
                    strOrderResult.Append(@"<li>   <a id='link1' title='Pickup by " + data.FirstOrDefault().CheckOutPaymentTransaction.ShipmentDate + @"'>Pickup</a>
													   </li>");
                }
                if (data.FirstOrDefault().CheckOutPaymentTransaction.Dispatched == true)
                {
                    DateTime? PickupDate = data.FirstOrDefault().CheckOutPaymentTransaction.PickupDate;

                    if (PickupDate != null)
                    {
                        DateTime d1 = PickupDate.Value;
                        DateTime d2 = data.FirstOrDefault().CheckOutPaymentTransaction.CreatedOn;

                        TimeSpan t = d1 - d2;
                        double NrOfDays = t.TotalDays;
                        NrOfDays = Math.Round(NrOfDays);

                        strOrderResult.Append(@"<li class='active' title='Dispatched by " + PickupDate.Value.ToShortDateString() + @"'>
												 <a id='link1' title='Dispatched by " + PickupDate.Value.ToShortDateString() + @"'>Dispatched <br/>" + PickupDate.Value.ToShortDateString() + @" (" + NrOfDays + @" Days)</a>
												 </li>");
                    }
                }
                else
                {
                    strOrderResult.Append(@"<li>  <a id='link1' title='Dispatched by " + data.FirstOrDefault().CheckOutPaymentTransaction.PickupDate + @"'>Dispatched</a>
														  </li>");
                }
                if (data.FirstOrDefault().CheckOutPaymentTransaction.Delivered == true)
                {
                    DateTime? DispatchedDate = data.FirstOrDefault().CheckOutPaymentTransaction.DispatchedDate;

                    if (DispatchedDate != null)
                    {
                        DateTime d1 = DispatchedDate.Value;
                        DateTime d2 = data.FirstOrDefault().CheckOutPaymentTransaction.CreatedOn;

                        TimeSpan t = d1 - d2;
                        double NrOfDays = t.TotalDays;
                        NrOfDays = Math.Round(NrOfDays);

                        strOrderResult.Append(@"<li class='active' title='Dispatched by " + DispatchedDate.Value.ToShortDateString() + @"'>
												 <a id='link1' title='Delivered by " + DispatchedDate.Value.ToShortDateString() + @"'>Delivered<br/> " + DispatchedDate.Value.ToShortDateString() + @" (" + NrOfDays + @" Days)</a>
											</li>");
                    }
                }
                else
                {
                    strOrderResult.Append(@"<li > <a id='link1' title='Delivered by " + data.FirstOrDefault().CheckOutPaymentTransaction.DispatchedDate + @"'>Delivered</a></li>");
                }
                strOrderResult.Append(@"</ul>");
            }

            strResult.Append(@"<div class='one_wrap'>
	<div class='widget'>         
		 <div class='widget_body'>
		  <div class='order_details'>
			   <ul>
			  <li> <span class='field_caption'>Order ID: </span> <span class='field_value'><span>" + data.FirstOrDefault().PaymentTransactionId + @"</span></span> 
					 <div class='fl_right text_right user_details'>
						<p>
						<span class='u_name'>Name : <span>" + data.FirstOrDefault().User.FirstName + @"</span></span><br/>
						<span class='u_name'>Mobile No : <span class='u_phone'><span>" + data.FirstOrDefault().User.MobileNo + @"</span></span><br/>
						<span class='u_name'>E-mail ID : <span>" + data.FirstOrDefault().User.EmailId + @"</span></span><br/>
						<span class='u_name'>Order Status : <span style='font-size:16px; color:red;'>" + status + @"</span></span><br/>
						<span>Payment Mode : <span style='font-size:16px; color:green;'>" + PaymentMode + @"</span></span>                                         
						</p>
					</div>
			  </li>
			  <li> <span class='order_date'><span>" + data.FirstOrDefault().CreatedOn.ToString("dd-MMM-yyyy HH:mm:ss") + @"</span></span><br/>
				   <span>Transaction Status : <span style='font-size:16px; color:Red;font-weight:Bold;'>" + TxnStatus + @"</span></span><br/></li>
				   <span>Message from PG : <span style='font-size:16px; color:green;'>" + TxnMessage + @"</span></span><br/>
				   " + strRefundAmount + @"
			  </li>
			</ul>
		  </div>         
		<div id='dvgrdProducts'>   

					</div>     
		<div class='cp_productlist_view'>
			<div>
<table class='activity_datatable ordergrd' rules='all' id='grdProducts' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
			<tbody>

<tr>
				<th scope='col'>Product Description</th><th scope='col'>Quantity</th><th scope='col'>Price</th><th scope='col' id='CGST'>CGST</th><th id='igst' scope='col'>SGST</th><th scope='col'>Total Amount</th>
			</tr>
					" + strtd + @"                       
					</tbody></table>
		
" + strOrderComments.ToString() + @"
</div>" + strShipment.ToString());

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strResult.ToString()
            };
        }
        else
        {
            StringBuilder strRlt = new StringBuilder();
            strResult.Append(@" <div id='ctl00_ctl00_Main_Main_divMessage' class='msgbar msg_Info hide_onC'>
								<span id='ctl00_ctl00_Main_Main_msgicon' class='iconsweet'>*</span>
								<p><span id='ctl00_ctl00_Main_Main_lblMessage'>There are no 'Orders' found.</span></p>
							</div>");

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = strRlt.ToString()
            };
        }

    }
    [HttpPost]
    public CustomResponse addproducts(string text, string quan)
    {
        db_Zon_HuwEntities context = new db_Zon_HuwEntities();
        //var updateobj = (List<Product>)BalUtility.GetSession(Shared.Sessions.prescriptionbuy) ??
        //                new List<Product>();
        List<Product> item = new List<Product>();

        Product test = new Product();
        test.ProductName = "";
        test.Quantity = 999;
        item.Add(test);
        List<Utility.Product> updateobj = (List<Product>)BalUtility.GetSession(Shared.Sessions.prescriptionbuy) ??
                         new List<Product>();
        if (updateobj.Count == 0)
        {
            foreach (var tempobj in item)
            {
                tempobj.ProductName = text;
                tempobj.Quantity = Convert.ToInt32(quan);
                var cost = context.Products.Where(x => x.ProductName == tempobj.ProductName).FirstOrDefault();
                tempobj.ProductCost = cost.ProductCost;
                tempobj.ProductId = cost.ProductId;
                BalUtility.CreateSession(item, Shared.Sessions.prescriptionbuy);
            }
        }
        else
        {
            foreach (var tempobj in item)
            {
                tempobj.ProductName = text;
                tempobj.Quantity = Convert.ToInt16(quan);
                var cost = context.Products.Where(x => x.ProductName == tempobj.ProductName).FirstOrDefault();
                tempobj.ProductCost = cost.ProductCost;
                tempobj.ProductId = cost.ProductId;
                updateobj.Add(tempobj);
                var newdata = updateobj;
            }
        }
        List<Utility.Product> data = (List<Product>)BalUtility.GetSession(Shared.Sessions.prescriptionbuy) ??
                         new List<Product>();
        StringBuilder strResult = new StringBuilder();
        StringBuilder strtrResult = new StringBuilder();
        StringBuilder strMsg = new StringBuilder();
        StringBuilder strMsgs = new StringBuilder();
        decimal totalprdctcost = 0;
        foreach (var items in data)
        {

            if (items.ProductCost != null)
            {
                strtrResult.Append(@"<tr>
<td class='name' align='center'><a target='blank' style='color:#064792' class='rm' href='UpdateProducts.aspx?id=" + items.ProductId + @"'>" + items.ProductId + @"</a></td>

			 <td class='name' align='center'>" + items.ProductName + @"</td>
			 <td class='name' align='center'>" + items.Quantity + @"</td>
			<td class='name' align='center'>" + items.ProductCost + @"</td>
<td class='name' align='center'><a  style='color:#f60003;text-decoration:none;' class='rm btn btn-sm btn-danger text-white' href='Javascript:;'  onclick=removeproduct('" + items.ProductName.Replace(" ", ",") + @"');><i class='fa fa-trash'></i> Remove</a></td>
			 ");
                //decimal prdctcost = cost.ProductCost*items.Quantity;
                if (totalprdctcost == 0)
                {
                    totalprdctcost = items.ProductCost * items.Quantity;
                }
                else
                {
                    totalprdctcost = totalprdctcost + (items.ProductCost * items.Quantity);
                }

            }
        }
        strMsgs.Append(@"<div class='text-right'>Total Amount:  <label id='totalamount' class='font-weight-bold'> <strong>" + totalprdctcost + "</strong></label></div>");
        string msg = "No Products ";
        if (data.Count == 0)
        {
            strMsg.Append(@"<div style='text-align:center;'><p>" + msg + "</p></div>");
        }

        strResult.Append(@"<table class='table'><thead class='thead-light font-weight-bold'>
			<tr>
<th class='image text-center' align='center'>Product Id</th>
<th class='image text-center' align='center'>Product Name</th>
				<th class='model text-center' align='center'>Quantity</th>
                <th class='model text-center' align='center'>Cost</th>
<th class='model text-center' align='center'>Action</th>
				</tr></thead><tbody>
					   " + strtrResult + "</tbody></table>" + strMsg + "" + strMsgs + "");
        return new CustomResponse
        {
            Status = string.IsNullOrWhiteSpace(strResult.ToString()) ? Shared.ResponseStatus.NoData.ToString() : Shared.ResponseStatus.Success.ToString(),
            Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
            Result = strResult.ToString()
        };

    }
    [HttpPost]
    public CustomResponse removeproduct(string prdctname)
    {
        prdctname = prdctname.Replace(",", " ");
        db_Zon_HuwEntities context = new db_Zon_HuwEntities();
        List<Utility.Product> updateobj = (List<Product>)BalUtility.GetSession(Shared.Sessions.prescriptionbuy) ??
                         new List<Product>();
        updateobj.Remove(updateobj.FirstOrDefault(p => p.ProductName == prdctname));
        BalUtility.CreateSession(updateobj, Shared.Sessions.prescriptionbuy);
        List<Utility.Product> data = (List<Product>)BalUtility.GetSession(Shared.Sessions.prescriptionbuy) ??
                        new List<Product>();
        StringBuilder strResult = new StringBuilder();
        StringBuilder strtrResult = new StringBuilder();
        StringBuilder strMsg = new StringBuilder();
        StringBuilder strMsgs = new StringBuilder();
        decimal totalprdctcost = 0;
        foreach (var items in data)
        {

            strtrResult.Append(@"<tr>
<td class='name' align='center'><a target='blank' style='color:#064792' class='rm' href='UpdateProducts.aspx?id=" + items.ProductId + @"'>" + items.ProductId + @"</a></td>

			 <td class='name' align='center'>" + items.ProductName + @"</td>
			 <td class='name' align='center'>" + items.Quantity + @"</td>
			<td class='name' align='center'>" + items.ProductCost + @"</td>
<td class='name' align='center'><a style='color:#f60003' class='rm btn btn-sm btn-danger text-white' href='javascript:;' onclick=removeproduct('" + items.ProductName.Replace(" ", ",") + @"');><i class='fa fa-trash'></i> Remove</a></td>

             ");
            if (totalprdctcost == 0)
            {
                totalprdctcost = items.ProductCost * items.Quantity;
            }
            else
            {
                totalprdctcost = totalprdctcost + (items.ProductCost * items.Quantity);
            }
        }
        strMsgs.Append(@"<div class='text-right'>Total Amount:  <label id='totalamount' class='font-weight-bold'><strong> " + totalprdctcost + "</strong></label></div>");
        string msg = "No Products";
        if (data.Count == 0)
        {
            strMsg.Append(@"<div class='text-center' style='text-align:center;'><p><br />" + msg + "</p></div>");
        }

        strResult.Append(@"<table class='table'><thead class='thead-light font-weight-bold '>
			<tr>
<th class='image text-center' align='center'>Product Id</th>
<th class='image text-center' align='center'>Product Name</th>
				<th class='model text-center' align='center'>Quantity</th>
                <th class='model text-center' align='center'>Cost</th>
<th class='model text-center' align='center'>Action</th>
				</tr></thead><tbody>
					   " + strtrResult + "</tbody></table>" + strMsg + "");
        return new CustomResponse
        {
            Status = string.IsNullOrWhiteSpace(strResult.ToString()) ? Shared.ResponseStatus.NoData.ToString() : Shared.ResponseStatus.Success.ToString(),
            Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
            Result = strResult.ToString()
        };
    }
    [HttpPost]
    [AllowAnonymous]
    public CustomResponse AddCouponCode(string CouponCode)
    {
        //List<UserProductTransaction> cartitems = BalUtility.GetCart() ?? new List<UserProductTransaction>();
        List<Utility.Product> cartitems = (List<Product>)BalUtility.GetSession(Shared.Sessions.prescriptionbuy) ??
                        new List<Product>();
        using (db_Zon_HuwEntities entity = new db_Zon_HuwEntities())
        {
            Tbl_Coupon_Info PromocodeData = entity.Tbl_Coupon_Info.Where(x => x.Coupon_Code == CouponCode && x.Valid_From < DateTime.Now && x.Status == true).FirstOrDefault();
            if (PromocodeData != null)
            {
                if (PromocodeData.Valid_To > DateTime.Now)
                {
                    if (PromocodeData.Min_Cart_Value <= cartitems.ToList().Sum(x => x.ProductCost * x.Quantity))
                    {
                        decimal CouponAmount = 0;
                        if (PromocodeData.Coupon_Percentage == 0 || string.IsNullOrEmpty(PromocodeData.Coupon_Percentage.ToString()))
                        {
                            CouponAmount = PromocodeData.Coupon_Amount.Value;
                        }
                        else
                        {
                            CouponAmount = Math.Round(((cartitems.ToList().Sum(x => x.ProductCost) * PromocodeData.Coupon_Percentage.Value) / 100), 0);
                            PromocodeData.Coupon_Amount = CouponAmount;
                        }
                        BalUtility.CreateSession(PromocodeData, Shared.Sessions.Promocode);
                        return new CustomResponse
                        {
                            Status = "SUCCESS",
                            Message = "Coupon code applied successfully",
                            Result = "SUCCESS"
                        };

                    }
                    else
                    {
                        var messages = PromocodeData.Min_Cart_Value;
                        PromocodeData = null;
                        BalUtility.CreateSession(PromocodeData, Shared.Sessions.Promocode);
                        return new CustomResponse
                        {
                            Message = "Minimum Order Amount for this coupon is: " + messages + ""
                        };
                    }
                }
                else
                {
                    PromocodeData = null;
                    BalUtility.CreateSession(PromocodeData, Shared.Sessions.Promocode);
                    return new CustomResponse
                    {
                        Message = "Coupon expired... Try with another coupon"
                    };

                }
            }
            else
            {
                PromocodeData = null;
                BalUtility.CreateSession(PromocodeData, Shared.Sessions.Promocode);
                return new CustomResponse
                {
                    Message = "Invalid Coupon Code "
                };
            }
        }
    }
    public CustomResponse Getpendinglist()
    {
        //insertintoprorductstablefrommedtable();
        db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();

        //var data = Entity.PaymentTransactions.Where(p => p.TxnStatus=="prescription_pending").ToList();
        var data = from p in Entity.PaymentTransactions
                   join u in Entity.Users on p.UserId equals u.UserId
                   where p.TxnStatus == "prescription_pending"
                   select p;
        //using (var ctx = new db_Zon_HuwEntities())
        //{
        //    //this will throw an exception
        //    var data = ctx.Products.SqlQuery("Select top 100 * from Products where IsDeleted = 'true' order by productid desc").ToList();

        return new CustomResponse
        {
            Result = data,
            Status = Shared.ResponseStatus.Success.ToString()
        };

        //}
    }

    public CustomResponse Getpendinglistmedicinenew(int? status)
    {
        //insertintoprorductstablefrommedtable();
        db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();

        //var data = Entity.PaymentTransactions.Where(p => p.TxnStatus=="prescription_pending").ToList();
        if (status.HasValue)
        {
            var data = Entity.Tbl_Prescriptionorders.Where(x => x.Status == status.Value && x.OrderplacedUrl == "HUW").OrderByDescending(x => x.OrderId).ToList();
            return new CustomResponse
            {
                Result = data,
                Status = Shared.ResponseStatus.Success.ToString()
            };
        }
        else
        {
            var data = Entity.Tbl_Prescriptionorders.Where(x => x.Status == 1).ToList();
            return new CustomResponse
            {
                Result = data,
                Status = Shared.ResponseStatus.Success.ToString()
            };
        }
        //using (var ctx = new db_Zon_HuwEntities())
        //{
        //    //this will throw an exception
        //    var data = ctx.Products.SqlQuery("Select top 100 * from Products where IsDeleted = 'true' order by productid desc").ToList();



        //}
    }
    public CustomResponse Getpendinglistnew(int? status)
    {
        //insertintoprorductstablefrommedtable();
        db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();

        //var data = Entity.PaymentTransactions.Where(p => p.TxnStatus=="prescription_pending").ToList();
        if (status.HasValue)
        {
            var data = Entity.Tbl_Prescriptionorders.Where(x => x.Status == status.Value && x.OrderplacedUrl == "mmh").ToList();
            return new CustomResponse
            {
                Result = data,
                Status = Shared.ResponseStatus.Success.ToString()
            };
        }
        else
        {
            var data = Entity.Tbl_Prescriptionorders.Where(x => x.Status == 1).ToList();
            return new CustomResponse
            {
                Result = data,
                Status = Shared.ResponseStatus.Success.ToString()
            };
        }
        //using (var ctx = new db_Zon_HuwEntities())
        //{
        //    //this will throw an exception
        //    var data = ctx.Products.SqlQuery("Select top 100 * from Products where IsDeleted = 'true' order by productid desc").ToList();



        //}
    }

    public CustomResponse getimagesforpres(int id)
    {
        db_Zon_HuwEntities dbz = new db_Zon_HuwEntities();
        var data = dbz.Tbl_Presecription_Info.Where(x => x.Payment_Transaction_Id == id).ToList();

        StringBuilder abc = new StringBuilder();
        foreach (var item in data)
        {
            abc.Append(@"<div class='col-lg-4'>
                    <a href='" + item.Prescription_Image + @"'>
                        <img src='" + item.Prescription_Image + @"' alt = 'prescription image' class='img-thumbnail'/>
                    </a>
                </div>");
        }
        return new CustomResponse
        {
            //Status = string.IsNullOrWhiteSpace(strResult.ToString()) ? Shared.ResponseStatus.NoData.ToString() : Shared.ResponseStatus.Success.ToString(),
            //Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
            Result = abc.ToString()
        };
    }
    public CustomResponse getdetailsforpres(int id)
    {
        db_Zon_HuwEntities dbz = new db_Zon_HuwEntities();
        List<Prescription_Upload_tbl> data = new List<Prescription_Upload_tbl>();
        //var data = dbz.Tbl_Presecription_Info.Where(x => x.Payment_Transaction_Id == id).ToList();
        data = dbz.Prescription_Upload_tbl.Where(x => x.transaction_ID == id).ToList();
        StringBuilder xyz = new StringBuilder();
        if (data.FirstOrDefault().Prescription_Type == 2)
        {
            foreach (var item in data)
            {
                xyz.Append(@"<div class='col-lg-12 mb-3' style='padding-left:20px;'>medicines :" + item.Own_selection_Medicines + "&nbsp;&nbsp;&nbsp;duration:" + item.Own_selection_Duration + @"</div><br>");
            }
        }
        else if (data.FirstOrDefault().Prescription_Type == 1)
        {
            xyz.Append(@"<span id='onlyduration' style='padding-left:20px;'>" + data.FirstOrDefault().Only_Duration + "");
        }
        else
        {
            xyz.Append(@"<span style='padding-left:20px;'>call me for details");
        }
        return new CustomResponse
        {
            //Status = string.IsNullOrWhiteSpace(strResult.ToString()) ? Shared.ResponseStatus.NoData.ToString() : Shared.ResponseStatus.Success.ToString(),
            //Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
            Result = xyz.ToString()
        };
    }
    public void insertintoprorductstablefrommedtable()
    {
        db_Zon_HuwEntities context = new db_Zon_HuwEntities();
        List<Product> prodtbl = new List<Product>();
        Product prodobj = new Product();
        var medlist = context.Medicines_tbl.ToList();
        foreach (var item in medlist)
        {
            decimal rate = 999999;
            decimal mrp = 999999;
            decimal stock = 999999;
            if (item.Rate != null)
            {
                rate = Convert.ToDecimal(item.Rate);
            }
            if (item.MRP != null)
            {
                mrp = Convert.ToDecimal(item.MRP);
            }
            if (item.stock != null)
            {
                stock = Convert.ToDecimal(item.stock);
                if (stock < 0)
                {
                    stock = stock * -1;
                }
            }
            prodobj.Brand = item.company;
            prodobj.ProductCode = item.Item_Code;
            prodobj.ProductName = item.Med_Name;
            prodobj.ProductOriginalCost = rate;
            prodobj.ProductCost = mrp;
            prodobj.Quantity = Convert.ToInt32(stock);


            //prodobj.SubCategoryId=
            prodobj.IsFeaturedProduct = false;
            prodobj.ProductDescription = "medicines";
            prodobj.ProductDiscountPercentage = 0; //discount percentage here
            prodobj.ProductDiscountCost = 0;       //discount amount
            prodobj.ProductImgUrl = "";            //default image location
            prodobj.IsActive = true;
            prodobj.IsDeleted = false;
            prodobj.CreatedOn = DateTime.Now;
            prodobj.UpdatedOn = DateTime.Now;
            prodobj.IsSold = false;                 //presumming out of stock
            prodobj.CanCompare = false;
            prodobj.HasReviews = false;
            prodobj.CanReviewsNeedPermission = false;
            prodobj.ShortDescription = "medicines";

            prodobj.SubCategoryId = 203;
            prodobj.IsPresciption = true;

            context.Products.Add(prodobj);
            context.SaveChanges();
        }


    }
    [HttpPost]
    public CustomResponse Updatestatus(int status, long orderid)
    {
        using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
        {
            Tbl_Prescriptionorders data = context.Tbl_Prescriptionorders.Where(x => x.OrderId == orderid).FirstOrDefault();
            data.Status = status;
            context.SaveChanges();
        }
        return new CustomResponse
        {
            //Status = string.IsNullOrWhiteSpace(strResult.ToString()) ? Shared.ResponseStatus.NoData.ToString() : Shared.ResponseStatus.Success.ToString(),
            //Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
            Result = "Success"
        };
    }
    public CustomResponse Getcomments(int transid)
    {
        CommentRepository repo = new CommentRepository();

        List<Tbl_OrderComments> orderComments = repo.GetOrderComments(transid);

        StringBuilder str = new StringBuilder();
        str.Append(@"<div id='vpb_pop_up_backgroundcomment'></div>
    <div id='vpb_login_pop_up_boxcomment' class='tab_content ui-tabs-panel ui-widget-content ui-corner-bottom active'>
        <div style='background-image: url('Orders_files/widget_title_bg.png'); border: medium none; border-radius: 0; height: 37px;'>
            <span style='color: #FFFFFF; float: left; font-family: CorbelBold; font-size: 14px; font-weight: normal; padding: 13px 0 10px 13px; text-shadow: 0 1px 0 #1D2024'>Comments</span>

            <div class='widget'>
                <div class='widget_body'>
                    <table cellpadding='0' cellspacing='0' border='0' width='100%'>
                        
                        <tr>
                            <td style='width: 40%; text-align: right; padding-right: 10px;'>
                                <label id='Label6'>Comments</label></td>


                            <td style='text-align: left; padding-left: 10px;'>");
        foreach (var item in orderComments)
        {
            str.Append("" + item.Comment_Date + ":" + item.Comment + "<br /><br />");
        }
        str.Append(@"</td>


                        </tr>
                        
                    </table>

                    <div class='action_bar text_right'>
                        <input value='Close' onclick='vpb_hide_popup_boxes()' id='Submit2' class='button_small greyishBtn' type='button' />
                    </div>
                </div>
            </div>
        </div>


        <script src='../JavaScript/vpb_script.js' type='text/javascript'></script>
        <link href='../JavaScript/style.css' rel='stylesheet' type='text/css' />
    </div>");
        return new CustomResponse
        {
            //Status = string.IsNullOrWhiteSpace(strResult.ToString()) ? Shared.ResponseStatus.NoData.ToString() : Shared.ResponseStatus.Success.ToString(),
            //Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
            Result = str.ToString()
        };
    }

    public CustomResponse GetProductsoldLst(string Fromdate, string Todate, long? Productid, string Productname)
    {
        StringBuilder strtrResult = new StringBuilder();
        StringBuilder strMsg = new StringBuilder();

        StringBuilder strResult = new StringBuilder();

        ProductRepository prdt = new ProductRepository();
        List<Soldproducts> data = prdt.Getsoldproducts(Fromdate, Todate, Productid, Productname);
        foreach (var item in data)
        {
            strtrResult.Append(@"<tr>
			 <td class='name'>" + item.productid + @"</td>
			 <td class='name'>" + item.ProductName + @"</td>
			 <td class='name'>" + item.totalquantity + @"</td>
			 			 <td class='name'><a target='_blank' style='color:#b32323' href='/admin/getorders.aspx?fromdate=" + Fromdate + "&todate=" + Todate + "&Productid=" + item.productid + "'>" + item.totalorders + @"</a></td>


");
        }

        string msg = "No Data Found ";
        if (data.Count == 0)
        {
            strMsg.Append(@"<div><p>" + msg + "</p></div>");
        }

        strResult.Append(@"<table id='table2excel'><thead>
			<tr><th class='image'>ProductId</th>
				<th class='model'>ProductName</th><th class='quantity'>Quantity</th>
				<th class='price'>Orders</th></tr></thead><tbody>
					   " + strtrResult + "</tbody></table>" + strMsg + "");
        return new CustomResponse
        {
            //Status = string.IsNullOrWhiteSpace(strResult.ToString()) ? Shared.ResponseStatus.NoData.ToString() : Shared.ResponseStatus.Success.ToString(),
            //Message = string.IsNullOrWhiteSpace(strResult.ToString()) ? "No Data Found" : "",
            Result = strResult.ToString()
        };
    }
    public CustomResponse Getorderslist(DateTime? fromdate, DateTime? todate, long productid)
    {
        if (fromdate == null)
            fromdate = DateTime.Now.AddYears(-100);
        if (todate == null)
            todate = DateTime.Now;
        var repository = new PaymentTransactionRepository();
        db_Zon_HuwEntities Context = new db_Zon_HuwEntities();
        List<PaymentTransaction> data = new List<PaymentTransaction>();
        long pdtid = Convert.ToInt64(productid);
        //data = Context.Database.SqlQuery<PaymentTransaction>("select ut.* from PaymentTransactions ut join UserProductTransactions upt on upt.PaymentTransactionId = ut.PaymentTransactionId and ProductId = " + productid + " and TxnStatus = 'Success' and upt.CreatedOn>='" + fromdate + "' and upt.CreatedOn<='" + todate + "'").ToList();
        data = (from p in Context.PaymentTransactions
                where (p.TxnStatus == "SUCCESS" && p.CreatedOn >= fromdate && p.CreatedOn <= todate)
                select p).OrderByDescending(x => x.PaymentTransactionId).ToList();
        StringBuilder strtd = new StringBuilder();
        StringBuilder strResult = new StringBuilder();
        if (data.Count > 0)
        {
            foreach (var item in data)
            {

                try
                {
                    if (item.UserProductTransactions.Count != 0)
                    {
                        if (item.UserProductTransactions.FirstOrDefault().ProductId == productid)
                        {
                            db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();
                            string statusvalue = item.OrderCurrentStatus.ToString();
                            var status = Shared.GetOrderStatusEnum(statusvalue);

                            StringBuilder ProductName = new StringBuilder();

                            for (var i = 0; i < item.UserProductTransactions.Count; i++)
                            {
                                if (i == 0)
                                {
                                    ProductName.Append("<a target='_blank' title='Remaining Quantity: " + item.UserProductTransactions.ElementAtOrDefault(i).Product.Quantity + "' style='color:blue;' href='/Admin/UpdateProducts.aspx?ID=" + item.UserProductTransactions.ElementAtOrDefault(i).Product.ProductId + "'>" + item.UserProductTransactions.ElementAtOrDefault(i).Product.ProductName + "</a>");
                                }
                                else
                                {
                                    ProductName.Append(", " + "<a target='_blank' title='Remaining Quantity: " + item.UserProductTransactions.ElementAtOrDefault(i).Product.Quantity + "' style='color:blue;' href='/Admin/UpdateProducts.aspx?ID=" + item.UserProductTransactions.ElementAtOrDefault(i).Product.ProductId + "'>" + item.UserProductTransactions.ElementAtOrDefault(i).Product.ProductName + "</a>");
                                }
                            }

                            string Payment = "";
                            if (item.PaymentMode == "Cash On Delivery")
                            {
                                Payment = "Postpaid";
                            }
                            else if (item.PaymentMode == "Paytm")
                            {
                                Payment = "Paytm";
                            }
                            else if (item.PaymentMode == "Payumoney")
                            {
                                Payment = "PayU money";
                            }
                            else
                            {
                                Payment = "Wallet";
                            }

                            strtd.Append(@"<tr><td>
												<a id='hypEdit'  onclick='NavigatetoOrderDetailsPage(" + item.PaymentTransactionId + ")' >" + item.PaymentTransactionId + @"</a><br>
												<span id='lblPartialShip'></span>
											</td><td>
												<span >" + item.CreatedOn.ToString("dd/MMM/yyyy") + @"</span>   
											</td>
												<td>
												<span >" + ProductName + @"</span>   
											</td>
<td>                                                
													<span id='lblTotalProducts'>" + item.UserProductTransactions.Count + @"</span>                                                
											</td>
<td>                                                
													<span id='lblTotalProducts'>" + item.User.FirstName + @"</span>                                                
											</td>
<td>                                                
													<span id='lblTotalProducts'>" + Payment + @"</span>                                                
											</td>
											 <td>                                                
													<span id='lblstatus'>" + status + @"</span>                                                
											</td><td>                                                
												<span id='lblcustName'><span>" + item.CurrencySymbol + @" </span>" + String.Format("{0:0.00}", item.TxnAmount) + @"</span></span>
											</td>
<td style='display:none'>" + item.User.FirstName + @"</td>
<td style='display:none'>" + item.User.MobileNo + @"</td>
			</tr>");
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }

            strResult.Append(@"
		<div class='widget'>                   
					
					<div class='widget_body'>
							<div id='Main_dvGrid'>
								<div class='widget marg-0'> 
								<div class='widget_title'>
									<span class='iconsweet'>r</span><h5>Available Orders</h5>
								</div>
								<div class='widget_body'>
						
							<div class='cp_productlist_view'>
								<div class='products_views'>
									
								</div>
							</div>
						   
										<table class='activity_datatable' rules='all' id='grdShippingOrders' style='width:100%;border-collapse:collapse;' border='1' cellpadding='8' cellspacing='0'>
											<thead>
								<tr>
												<th scope='col'>ID</th><th scope='col'>Date</th><th scope='col'>Product Name</th><th scope='col'>Qty</th><th scope='col'>Customer Name</th><th scope='col'>Payment Mode</th><th scope='col'>Status</th><th scope='col'>Amount</th><th scope='col' style='display:none'>Name</th><th scope='col' style='display:none'>Phone</th><th style='display:none'>Address</th>
											</tr></thead><tbody>
								" + strtd + @"
								</tbody></table>	</div>

  
									</div>
							</div>
							
							
					</div>
				</div>
				<input name='ctl00$ctl00$Main$Main$hdnTodoSearchcriteria' id='ctl00_ctl00_Main_Main_hdnTodoSearchcriteria' value='[&quot;Order No&quot;,&quot;SKU&quot;,&quot;Email Id&quot;,&quot;Phone No&quot;,&quot;Coupon Code&quot;,&quot;Checkout Type&quot;]' type='hidden'>
				<img id='img441' alt='' onload='getdivCurrency();' src='Orders_files/designoption_009_13.jpg' height='1' width='1'>
			
</div>      

			</div>
		</div>");

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = strResult.ToString(),
                CartSum = data.Count().ToString()
            };
        }
        else
        {
            strResult.Append(@" <div id='divMessage' class='msgbar msg_Info hide_onC'>
								<span id='msgicon' class='iconsweet'>*</span>
								<p><span id='lblMessage'>There are no 'Orders' found.</span></p>
							</div>");

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.NoData.ToString(),
                Result = strResult.ToString(),
                CartSum = data.Count().ToString()
            };
        }
    }
    [HttpGet]
    public CustomResponse DownlaodPdf(string IDS)
    {
        string data = GetInvoicestring(IDS);
        StringReader sr = new StringReader(data);

        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        using (MemoryStream memoryStream = new MemoryStream())
        {
            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
            pdfDoc.Open();

            htmlparser.Parse(sr);
            pdfDoc.Close();

            byte[] bytes = memoryStream.ToArray();
            memoryStream.Close();
            string path = "/UploadFiles/AWB/Bulkpdf" + DateTime.Now.ToString("ddmmmyyyyss") + "";
            string savePath = System.Web.HttpContext.Current.Server.MapPath(@"~" + path + ".pdf");
            System.IO.File.WriteAllBytes(savePath, bytes);
            // Clears all content output from the buffer stream
            //  return new CustomResponse
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = "http://admin.healthurwealth.com" + path + ".pdf",
                CartSum = data.Count().ToString()
            };
        }

    }
    [HttpGet]
    public string DownlaodBluedartPdf(string IDS, string awbno, string area, string location)
    {
        try
        {
            string data = GetBluedartInvoicestring(IDS, area, location, awbno);
            //string data = GetInvoicestring(IDS);
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;
            StringReader sr = new StringReader(data);
            string fontpath = System.Web.HttpContext.Current.Server.MapPath(".");
            BaseFont customfont = BaseFont.CreateFont(fontpath + "/B39MHR.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
            float customWidth = 4 * 72; // 1 inch = 72 points
            float customHeight = 2 * 40;
            Rectangle pageSize = new Rectangle(customWidth, customHeight);

            Document pdfDoc = new Document(PageSize.A5, 10f, 10f, 10f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Font font = new Font(customfont, 40);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();
                string s = "*" + awbno + "*";
                Paragraph paragraph = new Paragraph(s, font);
                paragraph.Alignment = Element.ALIGN_RIGHT; // Align left vertically

                paragraph.SpacingBefore = 40f; // Space before the paragraph
                paragraph.SpacingAfter = 20f; // Space after the paragraph
                paragraph.IndentationLeft = 30f; // Left indentation
                paragraph.IndentationRight = 30f; // Right indentation
                pdfDoc.Add(paragraph);
                htmlparser.Parse(sr);
                pdfDoc.Close();

                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                string path = "/UploadFiles/AWB/Bluedartpdf" + DateTime.Now.ToString("ddmmmyyyyss") + "";
                string savePath = System.Web.HttpContext.Current.Server.MapPath(@"~" + path + ".pdf");
                System.IO.File.WriteAllBytes(savePath, bytes);
                // Clears all content output from the buffer stream
                //  return new CustomResponse
                return "http://admin.healthurwealth.com" + path + ".pdf";
                //{
                //    Status = Shared.ResponseStatus.Success.ToString(),
                //    Result = "http://admin.healthurwealth.com" + path + ".pdf",
                //    CartSum = data.Count().ToString()
                //};
            }
        }
        catch (Exception exception)
        {
            return exception.Message + "    " + exception.InnerException;
        }

    }
    [HttpGet]
    public string DownlaodDTDCPdf(string referenceNumber)
    {

        var client = new RestClient("https://dtdcapi.shipsy.io/api/customer/integration/consignment/shippinglabel/stream?reference_number=" + referenceNumber + "&label_code=SHIP_LABEL_4X6&label_format=pdf");
        client.Timeout = -1;
        var request = new RestRequest(Method.GET);
        request.AddHeader("api-key", "fc3ca818b3b4f5be6dce68f2b003bf");
        IRestResponse response = client.Execute(request);
        //var shippingLabelResponse = JsonConvert.DeserializeObject<ShippingLabelResponse>(response.Content);

        if (response.RawBytes != null)
        {
            // Define the path where the PDF should be saved
            string path = "/UploadFiles/AWB/" + DateTime.Now.ToString("ddmmmyyyyssffff") + "";
            string savePath = System.Web.HttpContext.Current.Server.MapPath("~" + path + ".pdf");

            // Save the raw bytes directly to a PDF file
            File.WriteAllBytes(savePath, response.RawBytes);
            return "http://admin.healthurwealth.com" + path + ".pdf";
        }
        else
        {
            return "Failed to retrieve PDF content.";
        }
        return response.Content;

    }
    [HttpGet]
    public CustomResponse UpdateReturnStatus(long ID)
    {
        {
            db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();
            PaymentTransaction ptm = Entity.PaymentTransactions.Where(x => x.PaymentTransactionId == ID).FirstOrDefault();
            ptm.Orderdeliverystatus = "Product received";
            Entity.SaveChanges();

            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString(),
                Result = "Success"
            };
        }

    }
    //modify by shashi

    public CustomResponse UpdateBoxInpackaing(int OrderID, int BoxId)
    {
        var repository = new BoxInpackaingRepository();

        long Inpackaing = repository.BoxInpackaing(OrderID, BoxId);

        if (Inpackaing != 0)
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Success.ToString()
            };
        }
        else
        {
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString()
            };
        }
    }
    public string GetInvoicestring(string ids)
    {
        try
        {
            var EachID = ids.Split(',');
            StringBuilder strInvoice = new StringBuilder();
            db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();

            foreach (var TransactionId in EachID)
            {
                var repository = new PaymentTransactionRepository();
                int totalRecords;
                long transid = Convert.ToInt64(TransactionId);
                PaymentTransaction data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, 0,
                                                          (p => p.PaymentTransactionId == transid), null, "").FirstOrDefault();


                StringBuilder strResult = new StringBuilder();
                StringBuilder shippinggst = new StringBuilder();

                int count = 1;
                int totalQuantity = 0;
                if (data.UserProductTransactions.ElementAtOrDefault(0).UserAddress.StateName != "Telangana")
                {
                    if (data.UserProductTransactions.Max(x => x.Product.GST) > 18)
                        shippinggst.Append("<td></td><td class='prices'>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(((((data.ShippingCharges) * (100 / (100 + 18))))))) + @"</td><td>IGST</td><td>" + 18 + @"%</td><td>" + BalUtility.FormatedDoubleWithOutPrecision((Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + 18))))))))) + @"</td>");

                    else
                        shippinggst.Append("<td></td><td class='prices'>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(((((data.ShippingCharges) * (100 / (100 + data.UserProductTransactions.Max(x => x.Product.GST)))))))) + @"</td><td>IGST</td><td>" + data.UserProductTransactions.Max(x => x.Product.GST) + @"%</td><td>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + data.UserProductTransactions.Max(x => x.Product.GST))))))))) + @"</td>");
                }
                else
                {
                    if (data.UserProductTransactions.Max(x => x.Product.GST) > 18)
                        shippinggst.Append("<td></td><td class='prices'>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(((((data.ShippingCharges) * (100 / (100 + 18))))))) + @"</td><td>CGST<br>SGST</td><td>" + 18 / 2 + "<br>" + 18 / 2 + @"%</td><td> " + BalUtility.FormatedDoubleWithOutPrecision((Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + 18)))))))) / 2) + "<br>" + BalUtility.FormatedDoubleWithOutPrecision((Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + 18)))))))) / 2) + @"</td>");

                    else
                        shippinggst.Append("<td></td><td class='prices'>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(((((data.ShippingCharges) * (100 / (100 + data.UserProductTransactions.Max(x => x.Product.GST)))))))) + @"</td><td>CGST<br>SGST</td><td>" + data.UserProductTransactions.Max(x => x.Product.GST) / 2 + @"%<br> " + data.UserProductTransactions.Max(x => x.Product.GST) / 2 + @" % </td><td>" + BalUtility.FormatedDoubleWithOutPrecision((Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + data.UserProductTransactions.Max(x => x.Product.GST))))))))) / 2) + "<br>" + BalUtility.FormatedDoubleWithOutPrecision((Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + data.UserProductTransactions.Max(x => x.Product.GST))))))))) / 2) + @"</td>");


                }
                foreach (var item in data.UserProductTransactions)
                {
                    Tbl_AdditionalInfo addinfo = Entity.Tbl_AdditionalInfo.Where(x => x.ProductId == item.ProductId).FirstOrDefault();
                    string expdate = "";
                    if (addinfo != null)
                        expdate = addinfo.Manufacturer_Date.Value.AddMonths(addinfo.Best_Before_Date.Value).ToString("dd/MM/yyyy");
                    string ProductName = "";
                    decimal ProductCost = 0;
                    if (item.SubProductId == null)
                    {
                        ProductName = item.ProductName;
                        if (item.ProductName == null)
                            ProductName = item.Product.ProductName;

                        ProductCost = item.Product.ProductCost;
                    }
                    else
                    {
                        SubProduct Sp = Entity.SubProducts.Where(x => x.SubProductId == item.SubProductId).First();
                        ProductName = item.Product.ProductName + "(" + Sp.SPName + ")";
                        ProductCost = Sp.ProductCost;
                    }


                    if (data.UserProductTransactions.ElementAtOrDefault(0).UserAddress.StateName != "Telangana")
                    {
                        strResult.Append("<tr><td>" + count + "</td><td>" + ProductName + "(Exp:" + expdate + ")</td><td>" + item.Product.Brand + "</td><td>" + item.Quantity + @"</td>
<td class='prices'> " + BalUtility.FormatedDoubleWithOutPrecision((Convert.ToDouble(item.ProductCost / item.Quantity)) - Convert.ToDouble((((item.ProductCost / item.Quantity) - (((item.ProductCost / item.Quantity) * (100 / (100 + item.Product.GST))))))), true) + @"</td> 
<td>IGST</td><td>" + item.Product.GST + "%</td><td>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(((item.ProductCost) - (((item.ProductCost) * (100 / (100 + item.Product.GST))))))) + @"</td>

<td></td>
<td align='right'>" + item.ProductCost + @"</td>
 </tr>");
                    }
                    else
                    {
                        strResult.Append("<tr><td>" + count + "</td><td>" + ProductName + "(Exp:" + expdate + ")</td><td>" + item.Product.Brand + "</td><td>" + item.Quantity + @"</td>
<td class='prices'> " + BalUtility.FormatedDoubleWithOutPrecision((Convert.ToDouble(item.ProductCost / item.Quantity)) - Convert.ToDouble((((item.ProductCost / item.Quantity) - (((item.ProductCost / item.Quantity) * (100 / (100 + item.Product.GST))))))), true) + @"</td> 
<td>CGST<br>SGST</td><td>" + item.Product.GST / 2 + "%<br>" + item.Product.GST / 2 + "%</td><td>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(((item.ProductCost) - (((item.ProductCost) * (100 / (100 + item.Product.GST))))) / 2)) + "<br>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(((item.ProductCost) - (((item.ProductCost) * (100 / (100 + item.Product.GST))))) / 2)) + @"</td>
<td></td>

<td align='right'>" + item.ProductCost + @"</td>

 </tr>");
                    }
                    count++;
                    totalQuantity = totalQuantity + item.Quantity;
                }

                if (data.Offer_Product_ID != null)
                {
                    Tbl_Offered_Products Offer = Entity.Tbl_Offered_Products.Where(x => x.Offer_Product_ID == data.Offer_Product_ID).First();

                    strResult.Append("<tr><td>" + count + "</td><td>" + Offer.Offer_Product_Name + "</td><td>1</td><td>" + Offer.Product_Brand + @"</td>
<td> " + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(Offer.Product_Actual_Cost), true) + @"</td>
<td style='font-weight:bold;'> FREE</td> 
 </tr>");
                }

                if (data.Free_Product_ID != null)
                {
                    Product freeProduct = Entity.Products.Where(x => x.ProductId == data.Free_Product_ID).First();

                    strResult.Append("<tr><td>" + count + "</td><td>" + freeProduct.ProductName + "</td><td>1</td><td>" + freeProduct.Brand + @"</td>
<td> " + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(freeProduct.ProductCost), true) + @"</td>
<td style='font-weight:bold;'> FREE</td> 
 </tr>");
                }



                string ShippingCharges = "";
                string Total = "";

                if (data.UserProductTransactions.ElementAtOrDefault(0).Product.ProductCost >= 1000)
                {
                    ShippingCharges = "0.00";
                    Total = (Convert.ToDouble(data.UserProductTransactions.ElementAtOrDefault(0).Product.ProductCost)) + (Convert.ToDouble(ShippingCharges)).ToString();
                }
                else
                {
                    ShippingCharges = "50.00";
                    Total = (Convert.ToDouble(data.UserProductTransactions.ElementAtOrDefault(0).Product.ProductCost)) + (Convert.ToDouble(ShippingCharges)).ToString();
                }

                StringBuilder strPromocode = new StringBuilder();

                if (data.Has_Promo_Code == true)
                {
                    Tbl_Coupon_Info PromocodeInfo = Entity.Tbl_Coupon_Info.Where(x => x.Coupon_Id == data.Promo_Code_ID).First();

                    strPromocode.Append(@"<tr>
				<td colspan='3'>
					Discount
				</td>
				<td>
					
				</td>
				<td colspan='3' align='right'>
					
<strong>" + Convert.ToDecimal(data.Promo_Code_Amount).ToString("N2") + @"</strong>
				</td>
			</tr>");
                }

                StringBuilder strCOD = new StringBuilder();
                string COD = "Prepaid";
                string PrepaidAmounttext = "<span style='font-size:16px;margin-right:10px;float:right;'>&#8377; " + Convert.ToDecimal(data.TxnAmount).ToString("N2") + @"/-</span></div>";
                if (data.PaymentMode == "Cash On Delivery")
                {
                    PrepaidAmounttext = "<span style='font-size:16px;margin-right:10px;float:right;'>&#8377; :" + Convert.ToDecimal(data.TxnAmount).ToString("N2") + @"/-</span></div>";
                    COD = "COD";
                }
                StringBuilder strdc = new StringBuilder();
                if (data.DoctorName != null)
                {
                    strdc.Append(@"</br>
						<strong style='float:left'>Doctor Name/Hospital Name:" + data.DoctorName + @"</strong>
					</div>");
                }
                strInvoice.Append(@"<div style='margin:0 auto;width:595px;border-color:#999;'>
		<table width='100%' border='1' style='border-collapse:collapse;font-size:10px;border-color:#999;border-radius:5px;line-height:13px;font-family:Lucida Grande,Lucida Sans Unicode,Lucida Sans,DejaVu Sans,Verdana,sans-serif;' cellpadding='3' cellspacing='5'>
			<tr>
				<td colspan='10'>
					<div style='width:50%;float:left;padding-bottom:15px;'>
					<img src='https://www.healthurwealth.com/assets/images/logo-Black.png' width='50%' style='margin-left:5%;margin-top:5px;'>
 
 <div style='margin-top:20px;'>INVOICE <strong>#HUW00000" + data.PaymentTransactionId + @"</strong></div>
<div style='padding-top:8px;'>INVOICE Date: " + data.CreatedOn.ToShortDateString() + @"</div>
</div>
  
				   <div style='width:50%;float:left;line-height:32px;text-align:right;'>
<span style='background: #020000;padding: 3px 17px;margin-right: 5px;color: #fff;font-weight: 900;margin-top: 10px;font-size: 14px;display: inline-block;'>" + COD + @"</span>					
				   </div>
 <div style='font-size: 16px;float: right;margin-top: 24px;display: inline-block;'><small>Amount:</small> <span style='font-size:16px;float:right;font-weight: bold;'>" + PrepaidAmounttext + @"</span></div></div>
				</td>
			</tr>
			<tr>
				<td width='50%' colspan='3'>
					Name &amp; Shipping Address
				</td>
				<td width='50%' colspan='7'>
					<!--Billing Address--> Order Details:
				</td>
			</tr>
			<tr>
				<td colspan='3'>
					<strong>" + data.User.FirstName + " " + data.User.LastName + @"</strong>
					<div>
						" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress.StreetAddress1 + " ,</br>" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress.StreetAddress2 + " ,</br>" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress.LandMark + @"
					</div>
					<div>
						" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress.City + @" <span style='float:right'>" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress.PinCode + @"</span>
					</div>
					<div>
						" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress.StateName + @"
					</div>
					<div>
						<strong style='float:left'>" + data.User.MobileNo + @"</strong>
					</div>
" + strdc + @"
<input type='text' value='" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress.StateName + @"'  id='Statename' hidden/>
				</td>
				<td valign='top' colspan='7' style='line-height:20px;'>
					<!-- <div>
						" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress1.StreetAddress1 + " ,</br>" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress1.StreetAddress2 + " ,</br>" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress1.LandMark + @"
					</div>
					<div>
						" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress1.City + @" <span style='float:right'>" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress1.PinCode + @"</span>
					</div>
					<div >
						" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress1.StateName + @"
					</div> -->
				   
					<div>
						Payment Mode : <span style='float:right'><strong>" + data.PaymentMode + @"</strong></span>
					</div>                   
					<div>
						GST No : <span style='float:right'>36AAUPK4988K1ZO</span>
					</div>
				</td>
			</tr>
			<tr align='center'>
				<td>
					S.No
				</td>
				<td>
					Product
				</td>  
<td>
					Brand
				</td>
				<td>
					Qty
				</td>
				 
				<td>
					Price
				</td> 
<td><span  id='CGST'>
					 GST type
</span>
			<td>
					Tax Rate
				</td><td>
					Tax Amount
				</td>
<td>
					
				</td>
				<td>
					Amount
				</td>


			</tr>
			" + strResult + @"
			<tr>
				<td colspan='3' style='border:none'>
					Shipping Charges
				</td>

" + shippinggst + @"
				<td colspan='4' align='right' style='border:none'>
				" + Convert.ToDecimal(data.ShippingCharges).ToString("N2") + @"
				</td>
			</tr>     
			" + strPromocode + @"
			<tr>
				<td colspan='3' >
					Total
				</td>
				<td>
					" + totalQuantity + @"
				</td>
<td id='producttotal'>
					
				</td>
<td></td>
<td></td>

     <td id='gsttotal'>
					
				</td>
				<td colspan='3' align='right'>
					<strong>" + Convert.ToDecimal(data.TxnAmount).ToString("N2") + @"</strong>
				</td>
			</tr>
			" + strCOD + @"
			<tr>
				<td colspan='10'>
					Prices are inclusive of all applicable taxes
				</td>
			</tr>
			<tr>
				<td colspan='10'>
					<u>If Undelivered, please return to :</u><br>
<strong>SONAL ENTERPRISES ,6-3-850/1, Ground floor, Shop no 4, Sirisha plaza,<br> main road, Dharam Karan Rd, Ameerpet,<br> Hyderabad, Telangana 500016</strong>
				</td>
			</tr>
		</table>
	<div >This is a computer generated invoice. No signature required. <br />
** Conditions Apply. Please refer to the product page for more details</div></div>
<br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br>
");
            }

            //<div>
            //           Order No: <span style='float:right'>" + data.PaymentTransactionId + @"</span>
            //       </div>

            return strInvoice.ToString();

        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);

            throw;
        }
    }
    public string GetBluedartInvoicestring(string ids, string area, string location, string awb)
    {
        try
        {
            var EachID = ids.Split(',');
            StringBuilder strInvoice = new StringBuilder();
            db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();

            foreach (var TransactionId in EachID)
            {
                var repository = new PaymentTransactionRepository();
                int totalRecords;
                long transid = Convert.ToInt64(TransactionId);
                PaymentTransaction data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, 0,
                                                          (p => p.PaymentTransactionId == transid), null, "").FirstOrDefault();


                StringBuilder strResult = new StringBuilder();
                StringBuilder shippinggst = new StringBuilder();

                int count = 1;
                int totalQuantity = 0;
                if (data.UserProductTransactions.ElementAtOrDefault(0).UserAddress.StateName != "Telangana")
                {
                    if (data.UserProductTransactions.Max(x => x.Product.GST) > 18)
                        shippinggst.Append("<td></td><td class='prices'>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(((((data.ShippingCharges) * (100 / (100 + 18))))))) + @"</td><td>IGST</td><td>" + 18 + @"%</td><td>" + BalUtility.FormatedDoubleWithOutPrecision((Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + 18))))))))) + @"</td>");

                    else
                        shippinggst.Append("<td></td><td class='prices'>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(((((data.ShippingCharges) * (100 / (100 + data.UserProductTransactions.Max(x => x.Product.GST)))))))) + @"</td><td>IGST</td><td>" + data.UserProductTransactions.Max(x => x.Product.GST) + @"%</td><td>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + data.UserProductTransactions.Max(x => x.Product.GST))))))))) + @"</td>");
                }
                else
                {
                    if (data.UserProductTransactions.Max(x => x.Product.GST) > 18)
                        shippinggst.Append("<td></td><td class='prices'>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(((((data.ShippingCharges) * (100 / (100 + 18))))))) + @"</td><td>CGST<br>SGST</td><td>" + 18 / 2 + "<br>" + 18 / 2 + @"%</td><td> " + BalUtility.FormatedDoubleWithOutPrecision((Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + 18)))))))) / 2) + "<br>" + BalUtility.FormatedDoubleWithOutPrecision((Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + 18)))))))) / 2) + @"</td>");

                    else
                        shippinggst.Append("<td></td><td class='prices'>" + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(((((data.ShippingCharges) * (100 / (100 + data.UserProductTransactions.Max(x => x.Product.GST)))))))) + @"</td><td>CGST<br>SGST</td><td>" + data.UserProductTransactions.Max(x => x.Product.GST) / 2 + @"%<br> " + data.UserProductTransactions.Max(x => x.Product.GST) / 2 + @" % </td><td>" + BalUtility.FormatedDoubleWithOutPrecision((Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + data.UserProductTransactions.Max(x => x.Product.GST))))))))) / 2) + "<br>" + BalUtility.FormatedDoubleWithOutPrecision((Convert.ToDouble(data.ShippingCharges - (((((data.ShippingCharges) * (100 / (100 + data.UserProductTransactions.Max(x => x.Product.GST))))))))) / 2) + @"</td>");


                }
                foreach (var item in data.UserProductTransactions)
                {
                    Tbl_AdditionalInfo addinfo = Entity.Tbl_AdditionalInfo.Where(x => x.ProductId == item.ProductId).FirstOrDefault();
                    string expdate = "";
                    if (addinfo != null)
                        expdate = addinfo.Manufacturer_Date.Value.AddMonths(addinfo.Best_Before_Date.Value).ToString("dd/MM/yyyy");
                    string ProductName = "";
                    decimal ProductCost = 0;
                    if (item.SubProductId == null)
                    {
                        ProductName = item.ProductName;
                        if (item.ProductName == null)
                            ProductName = item.Product.ProductName;

                        ProductCost = item.Product.ProductCost;
                    }
                    else
                    {
                        SubProduct Sp = Entity.SubProducts.Where(x => x.SubProductId == item.SubProductId).First();
                        ProductName = item.Product.ProductName + "(" + Sp.SPName + ")";
                        ProductCost = Sp.ProductCost;
                    }


                    if (data.UserProductTransactions.ElementAtOrDefault(0).UserAddress.StateName != "Telangana")
                    {
                        strResult.Append("<tr><td>" + item.Product.Brand + "</td><td>" + item.PaymentTransactionId + @"</td>
 </tr>");
                    }
                    else
                    {
                        strResult.Append("<tr><td>" + item.Product.Brand + "</td><td>" + item.PaymentTransactionId + @"</td>
 </tr>");
                    }
                    count++;
                    totalQuantity = totalQuantity + item.Quantity;
                }

                if (data.Offer_Product_ID != null)
                {
                    Tbl_Offered_Products Offer = Entity.Tbl_Offered_Products.Where(x => x.Offer_Product_ID == data.Offer_Product_ID).First();

                    strResult.Append("<tr><td>" + count + "</td><td>" + Offer.Offer_Product_Name + "</td><td>1</td><td>" + Offer.Product_Brand + @"</td>
<td> " + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(Offer.Product_Actual_Cost), true) + @"</td>
<td style='font-weight:bold;'> FREE</td> 
 </tr>");
                }

                if (data.Free_Product_ID != null)
                {
                    Product freeProduct = Entity.Products.Where(x => x.ProductId == data.Free_Product_ID).First();

                    strResult.Append("<tr><td>" + count + "</td><td>" + freeProduct.ProductName + "</td><td>1</td><td>" + freeProduct.Brand + @"</td>
<td> " + BalUtility.FormatedDoubleWithOutPrecision(Convert.ToDouble(freeProduct.ProductCost), true) + @"</td>
<td style='font-weight:bold;'> FREE</td> 
 </tr>");
                }



                string ShippingCharges = "";
                string Total = "";

                if (data.UserProductTransactions.ElementAtOrDefault(0).Product.ProductCost >= 1000)
                {
                    ShippingCharges = "0.00";
                    Total = (Convert.ToDouble(data.UserProductTransactions.ElementAtOrDefault(0).Product.ProductCost)) + (Convert.ToDouble(ShippingCharges)).ToString();
                }
                else
                {
                    ShippingCharges = "50.00";
                    Total = (Convert.ToDouble(data.UserProductTransactions.ElementAtOrDefault(0).Product.ProductCost)) + (Convert.ToDouble(ShippingCharges)).ToString();
                }

                StringBuilder strPromocode = new StringBuilder();

                if (data.Has_Promo_Code == true)
                {
                    Tbl_Coupon_Info PromocodeInfo = Entity.Tbl_Coupon_Info.Where(x => x.Coupon_Id == data.Promo_Code_ID).First();

                    strPromocode.Append(@"<tr>
				<td colspan='3'>
					Discount
				</td>
				<td>
					
				</td>
				<td colspan='3' align='right'>
					
<strong>" + Convert.ToDecimal(data.Promo_Code_Amount).ToString("N2") + @"</strong>
				</td>
			</tr>");
                }

                StringBuilder strCOD = new StringBuilder();
                string COD = "Prepaid";
                string PrepaidAmounttext = "<span style='font-size:16px;margin-right:10px;float:right;'>&#8377; Prepaid</span></div>";
                if (data.PaymentMode == "Cash On Delivery")
                {
                    PrepaidAmounttext = "<span style='font-size:16px;margin-right:10px;float:right;'>&#8377; " + Convert.ToDecimal(data.TxnAmount).ToString("N2") + @"/-</span></div>";
                    COD = "COD";
                }
                StringBuilder strdc = new StringBuilder();
                if (data.DoctorName != null)
                {
                    strdc.Append(@"</br>
						<strong style='float:left'>Doctor Name/Hospital Name:" + data.DoctorName + @"</strong>
					</div>");
                }
                strInvoice.Append(@"

<div style='margin:0 auto;width:595px;border-color:#999;'>

		<table width='100%' border='1' style='border-collapse:collapse;font-size:10px;border-color:#999;border-radius:5px;line-height:13px;font-family:Lucida Grande,Lucida Sans Unicode,Lucida Sans,DejaVu Sans,Verdana,sans-serif;' cellpadding='3' cellspacing='5'>
			<tr>
				<td>
<img src='https://tradebrains.in/wp-content/uploads/2022/11/Blue-Dart-logo.png' width='50%' style='float:left;display:inline-block;'><br/>
<img src='https://www.healthurwealth.com/assets/images/logo.png' width='45%' style='display:block;margin-top:5px;'>
<div> <strong>Way bill No:" + awb + @"</strong></div><br>
					<div style='width:50%;float:left;padding-bottom:15px;'>
					
 
 <div style='margin-top:20px;'>INVOICE <strong>#HUW00000" + data.PaymentTransactionId + @"</strong></div>
<div style='padding-top:8px;'>INVOICE Date:" + data.CreatedOn.ToShortDateString() + @"</div>
</div>

<div style='width:49%;float:right;padding-top:15px;'>
 <div style='margin-top:20px;'> <strong>" + COD + @"</strong>
</div>
<div style='margin-top:20px;'> <strong>" + data.ShipmentId + @"</strong></div>
</div>
  
				   <!--<div style=''>
<span style=''>" + COD + @"</span>					
				   </div>-->
<div style='width:50%;float:left;line-height:25px;text-align:right;margin-top:10px;'>
<span style=''><small>Collect Amount:</small> " + PrepaidAmounttext + @"</span>					
				   </div>
 </td>
				<td>
				<!--Billing AddressPick Up Address:<br/>
				
				
	
				
			
				   
					<div>

Shipper: 301221 Sonal<br>
Sender: SONAL ENTERPRISES<br>
Address:<strong>6-3-850/1,SHIRISHA PLAZA,DHARA
M KARAM ROAD,AMEERPET,HYDERABAD
</strong><br>
Pincode:500016        <strong>     Origin: HYD /SOM</strong><br><br>
Tel/Mob: 9440689551
					</div>-->
                <h3 style='font-weight:bold;text-align:right;'>" + COD + @"</h3>
					Delivery Address:<br/>
				
				
					<strong>" + data.User.FirstName + " " + data.User.LastName + @"</strong>
					<div>
						<strong>" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress.StreetAddress1 + "</strong> ,</br><strong>" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress.StreetAddress2 + " </strong>,</br><strong>" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress.LandMark + @"</strong>
					</div>
					<div>
						" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress.City + @" <span style='float:right'>" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress.PinCode + @"</span>                    <strong>     Destination: " + area + " /" + location + @"</strong><br>
					</div>
					<div>
						" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress.StateName + @"
					</div>
					<div>
						<strong style='float:left'>" + data.User.MobileNo + @"</strong>
					</div>
</td>
" + strdc + @"
<input type='text' value='" + data.UserProductTransactions.ElementAtOrDefault(0).UserAddress.StateName + @"'  id='Statename' hidden/>
				
				
			
			</tr>
			
			<tr align='center'>
				
				<td>
					Brand

				</td>  
<td>
Reference Id
				</td>


			</tr>
			" + strResult + @"
			
			
<!--<td></td>
<td></td>-->

     <td id='gsttotal'>
					Total Amount
				</td>
				<td colspan='3' align='right'>
					<strong>" + Convert.ToDecimal(data.TxnAmount).ToString("N2") + @"</strong>
				</td>
			</tr>
			" + strCOD + @"
			<tr>
				<td colspan='10' style='font-size:7px;'>
					Prices are inclusive of all applicable taxes
				</td>
			</tr>
			<tr>
				<td colspan='10'>
					<u style='font-size:7px;'>If Undelivered, please return to :</u><br>
<span style='font-size:8px;font-weight:bold;'>SONAL ENTERPRISES ,6-3-850/1, Ground floor, Shop no 4, Sirisha plaza, main road, Dharam Karan Rd, Ameerpet, Hyderabad, Telangana 500016.</span>
				</td>
			</tr>
		</table>

	<div style='font-size:6px;'>This is a computer generated invoice. No signature required. <br />
** Conditions Apply. Please refer to the product page for more details</div></div>
<!--<br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br>-->
");
            }

            //<div>
            //           Order No: <span style='float:right'>" + data.PaymentTransactionId + @"</span>
            //       </div>

            return strInvoice.ToString();

        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);

            throw;
        }
    }
    [HttpPost]
    public CustomResponse Updatetodeliveredusingapi()
    {
        var repository = new PaymentTransactionRepository();
        int totalRecords;

        var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, 1000, (p => p.TxnStatus == "SUCCESS" && p.Dispatched == true && p.Delivered == false && p.Pickup == true && p.Authorized == true), null, "", true);
        //var getdata = data
        var filteredData = (from q in (data as List<PaymentTransaction>)
                            select new
                            {
                                q.User.FirstName,
                                q.User.LastName,
                                q.User.MobileNo,
                                q.User.EmailId,
                                q.User.UserId,
                                q.PaymentTransactionId,
                                q.PickupDate,
                                Quantity = q.UserProductTransactions.First().Quantity,
                                q.CourierName,
                                q.PaymentStatus,
                                q.TxnAmount,
                                q.TxnRefNo,
                                q.TxnStatus,
                                currency = q.CurrencyCode + " ( " + q.CurrencySymbol + " ) ",
                                q.CurrencyCode,
                                q.CurrencySymbol,
                                q.PGTxnId,
                                q.TxnMessage,
                                q.ShipmentId,
                                q.PickupID,
                                q.CreatedOn,
                                q.DispatchedDate,
                                q.OrderCurrentStatus,
                                products = q.UserProductTransactions.Where(cat => cat.PaymentTransactionId == q.PaymentTransactionId).
                                  Select(u => string.Join(",", (u.Product.ProductName + (u.SubProduct == null ? "" : " - " + u.SubProduct.SPName))))

                            }).Where(x => x.CourierName == "Delhivery").OrderByDescending(p => p.DispatchedDate).ToList();
        foreach (var item in filteredData)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            HttpResponseMessage response = client.GetAsync("https://track.delhivery.com/api/v1/packages/json/?waybill=" + item.ShipmentId + "&token=d6ce66f9ff7dd9870364a226cea294052f7bf2f0").Result;
            Main resData = JsonConvert.DeserializeObject<Main>(response.Content.ReadAsStringAsync().Result);
            if (resData.ShipmentData != null)
            {
                if (resData.ShipmentData.FirstOrDefault().Shipment.Status.Status == "Delivered")
                {
                    PaymentTransaction pt = new PaymentTransaction();
                    pt.PaymentTransactionId = item.PaymentTransactionId;
                    pt.Comments = "Auto dispatch from api";
                    pt.ReceivedBy = "";

                    var updatedata = repository.First(p => p.PaymentTransactionId == item.PaymentTransactionId);
                    updatedata.Orderdeliverystatus = resData.ShipmentData.FirstOrDefault().Shipment.Status.Status;
                    repository.Update(updatedata);
                    UpdateDispatchedOrders(pt);


                }
                else
                {
                    PaymentTransaction updatedata = repository.First(p => p.PaymentTransactionId == item.PaymentTransactionId);
                    //var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out  totalRecords, 0, rows, (p => p.Dispatched == false && p.Delivered == false && p.Pickup == false && p.Authorized == false), null, "", true);
                    if (updatedata.Orderdeliverystatus != "Product received")
                    {
                        updatedata.Orderdeliverystatus = resData.ShipmentData.FirstOrDefault().Shipment.Status.Status;
                    }
                    repository.Update(updatedata);
                }
            }
        }
        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Message = "",
            Result = null
        };
    }
    [HttpPost]
    public CustomResponse UpdatetodeliveredusingBluedartapi()
    {
        string finalresponse = "";
        var repository = new PaymentTransactionRepository();
        int totalRecords;

        var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, 1000, (p => p.TxnStatus == "SUCCESS" && p.Dispatched == true && p.Delivered == false && p.Pickup == true && p.Authorized == true), null, "", true);
        //var getdata = data
        var filteredData = (from q in (data as List<PaymentTransaction>)
                            select new
                            {
                                q.User.FirstName,
                                q.User.LastName,
                                q.User.MobileNo,
                                q.User.EmailId,
                                q.User.UserId,
                                q.PaymentTransactionId,
                                q.PickupDate,
                                Quantity = q.UserProductTransactions.First().Quantity,
                                q.CourierName,
                                q.PaymentStatus,
                                q.TxnAmount,
                                q.TxnRefNo,
                                q.TxnStatus,
                                currency = q.CurrencyCode + " ( " + q.CurrencySymbol + " ) ",
                                q.CurrencyCode,
                                q.CurrencySymbol,
                                q.PGTxnId,
                                q.TxnMessage,
                                q.ShipmentId,
                                q.PickupID,
                                q.CreatedOn,
                                q.DispatchedDate,
                                q.OrderCurrentStatus,
                                products = q.UserProductTransactions.Where(cat => cat.PaymentTransactionId == q.PaymentTransactionId).
                                  Select(u => string.Join(",", (u.Product.ProductName + (u.SubProduct == null ? "" : " - " + u.SubProduct.SPName))))

                            }).Where(x => x.CourierName == "Blue Dart").OrderByDescending(p => p.DispatchedDate).ToList();
        foreach (var item in filteredData)
        {


            //var client = new RestClient(options);
            //var request = new RestRequest("/servlet/RoutingServlet?handler=tnt&action=custawbquery&loginid=HYD99570&format=xml&lickey=elfomgi3ogmmntelhliniiswmtkgnllp&verno=1.3&scan=1&awb=awb&numbers=80248697616", Method.Get);
            //request.AddHeader("Cookie", "BIGipServerpl_api-bluedart.dhl.com_443=!xsR4fiyfGWcfPkbzvvsIVYa1K6PKfYYO0wwKd4ix7E9wfkyYtk9AngYNop8DJBU1n4RrsOvN2eTohzI=");
            //RestResponse response = await client.ExecuteAsync(request);
            //Console.WriteLine(response.Content);


            var client = new RestClient("https://api.bluedart.com/servlet/RoutingServlet?handler=tnt&action=custawbquery&loginid=HYD99570&format=json&lickey=elfomgi3ogmmntelhliniiswmtkgnllp&verno=1.3&scan=1&awb=awb&numbers=" + item.ShipmentId);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Cookie", "BIGipServerpl_api-bluedart.dhl.com_443=!xsR4fiyfGWcfPkbzvvsIVYa1K6PKfYYO0wwKd4ix7E9wfkyYtk9AngYNop8DJBU1n4RrsOvN2eTohzI=");
            IRestResponse response = client.Execute(request);
            finalresponse = finalresponse + response.Content + "..........";
            Utility.Bluedarttrack.BluedartRoot resData = JsonConvert.DeserializeObject<Utility.Bluedarttrack.BluedartRoot>(response.Content.ToString());
            if (resData != null)
            {
                if (resData.ShipmentData != null)
                {
                    if (resData.ShipmentData.Shipment != null)
                    {
                        {
                            if (resData.ShipmentData.Shipment.FirstOrDefault().Status == "SHIPMENT DELIVERED")
                            {
                                PaymentTransaction pt = new PaymentTransaction();
                                pt.PaymentTransactionId = item.PaymentTransactionId;
                                pt.Comments = "Auto dispatch from api";
                                pt.ReceivedBy = "";
                                var updatedata = repository.First(p => p.PaymentTransactionId == item.PaymentTransactionId);
                                updatedata.Orderdeliverystatus = "Delivered";
                                repository.Update(updatedata);
                                UpdateDispatchedOrders(pt);
                                SendMessage(@"Hi " + item.FirstName + @",

Thank you for your order from Healthurwealth.com.

Your order " + item.PaymentTransactionId + @" has been delivered.

You might also like our Promepro Multi Vitamin Gummies: https://www.healthurwealth.com/product/Promepro-60s/92141

We hope you enjoyed your shopping experience with us and that you will visit us again soon.

For any queries Please Contact:+919440689551

Please follow us on instagram to get more updates on offers and products https://www.instagram.com/huwsocial/", item.MobileNo.ToString());

                            }
                            else
                            {
                                PaymentTransaction updatedata = repository.First(p => p.PaymentTransactionId == item.PaymentTransactionId);
                                if (updatedata.Orderdeliverystatus != "Product received")
                                {
                                    //var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out  totalRecords, 0, rows, (p => p.Dispatched == false && p.Delivered == false && p.Pickup == false && p.Authorized == false), null, "", true);
                                    updatedata.Orderdeliverystatus = resData.ShipmentData.Shipment.FirstOrDefault().Status;
                                }
                                repository.Update(updatedata);
                            }
                        }
                    }
                }
            }
        }
        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Message = finalresponse,
            Result = null
        };
    }
    [HttpPost]
    public CustomResponse UpdatetodeliveredusingShiprocketapi()
    {

        string finalresponse = "";
        var repository = new PaymentTransactionRepository();
        int totalRecords;

        var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, 1000, (p => p.TxnStatus == "SUCCESS" && p.CourierName == "ShipRocket" && p.Dispatched == true && p.Delivered == false && p.Pickup == true && p.Authorized == true), null, "", true);
        //var getdata = data
        var filteredData = (from q in (data as List<PaymentTransaction>)
                            select new
                            {
                                q.User.FirstName,
                                q.User.LastName,
                                q.User.MobileNo,
                                q.User.EmailId,
                                q.User.UserId,
                                q.PaymentTransactionId,
                                q.PickupDate,
                                Quantity = q.UserProductTransactions.First().Quantity,
                                q.CourierName,
                                q.PaymentStatus,
                                q.TxnAmount,
                                q.TxnRefNo,
                                q.TxnStatus,
                                currency = q.CurrencyCode + " ( " + q.CurrencySymbol + " ) ",
                                q.CurrencyCode,
                                q.CurrencySymbol,
                                q.PGTxnId,
                                q.TxnMessage,
                                q.ShipmentId,
                                q.PickupID,
                                q.CreatedOn,
                                q.DispatchedDate,
                                q.OrderCurrentStatus,
                                products = q.UserProductTransactions.Where(cat => cat.PaymentTransactionId == q.PaymentTransactionId).
                                  Select(u => string.Join(",", (u.Product.ProductName + (u.SubProduct == null ? "" : " - " + u.SubProduct.SPName))))

                            }).OrderByDescending(p => p.DispatchedDate).ToList();
        string bearer = GetBearer();
        foreach (var item in filteredData)
        {

            try
            {
                var client = new RestClient("https://apiv2.shiprocket.in/v1/external/courier/track/awb/" + item.ShipmentId);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", "Bearer " + bearer + "");
                IRestResponse response = client.Execute(request);
                finalresponse = finalresponse + response.Content + "..........";
                Shiprockettracking resData = JsonConvert.DeserializeObject<Shiprockettracking>(response.Content.ToString());
                if (resData != null)
                {
                    if (resData.tracking_data != null)
                    {
                        if (resData.tracking_data.shipment_track != null)
                        {
                            if (resData.tracking_data.shipment_track.FirstOrDefault().courier_company_id != null)
                            {
                                if (resData.tracking_data.shipment_track.FirstOrDefault().current_status == "Delivered")
                                {
                                    PaymentTransaction pt = new PaymentTransaction();
                                    pt.PaymentTransactionId = item.PaymentTransactionId;
                                    pt.Comments = "Auto dispatch from api";
                                    pt.ReceivedBy = "";
                                    var updatedata = repository.First(p => p.PaymentTransactionId == item.PaymentTransactionId);
                                    updatedata.Orderdeliverystatus = "Delivered";
                                    repository.Update(updatedata);
                                    CustomResponse isupdated = UpdateDispatchedOrders(pt);
                                    if (isupdated.Status == "Fail")
                                    {
                                        return new CustomResponse
                                        {
                                            Status = Shared.ResponseStatus.Fail.ToString(),
                                            Message = isupdated.Message + "..." + item.PaymentTransactionId,
                                            Result = null
                                        };
                                    }
                                    SendMessage(@"Hi " + item.FirstName + @",

Thank you for your order from Healthurwealth.com.

Your order " + item.PaymentTransactionId + @" has been delivered.

You might also like our Promepro Multi Vitamin Gummies: https://www.healthurwealth.com/product/Promepro-60s/92141

We hope you enjoyed your shopping experience with us and that you will visit us again soon.

For any queries Please Contact:+919440689551

Please follow us on instagram to get more updates on offers and products https://www.instagram.com/huwsocial/", item.MobileNo.ToString());

                                }
                                else
                                {
                                    PaymentTransaction updatedata = repository.First(p => p.PaymentTransactionId == item.PaymentTransactionId);
                                    if (updatedata.Orderdeliverystatus != "Product received")
                                    {
                                        //var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out  totalRecords, 0, rows, (p => p.Dispatched == false && p.Delivered == false && p.Pickup == false && p.Authorized == false), null, "", true);
                                        updatedata.Orderdeliverystatus = resData.tracking_data.shipment_track.FirstOrDefault().current_status;
                                    }
                                    repository.Update(updatedata);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Fail.ToString(),
                    Message = ex.Message + " " + item.PaymentTransactionId,
                    Result = null
                };
            }
        }
        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Message = finalresponse,
            Result = null
        };

    }
    [HttpPost]
    public void Updatedoctorname(long transid, string doctorname)
    {
        db_Zon_HuwEntities context = new db_Zon_HuwEntities();
        PaymentTransaction ptdata = context.PaymentTransactions.Where(x => x.PaymentTransactionId == transid).FirstOrDefault();
        ptdata.DoctorName = doctorname;
        context.SaveChanges();
    }


}

#endregion









