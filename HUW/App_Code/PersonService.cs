using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Services;
using BAL;
using Utility;
using System.Net;
using System.Text.RegularExpressions;
using DAL;
using System.Web;
using paytm;
[ScriptService]
public class PersonService : WebService
{
    string ApiUrl = System.Configuration.ConfigurationManager.AppSettings["ApiUrl"];
    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public dynamic GetOrderStatusList()
    {
        var dict = new Dictionary<int, string>();
        foreach (var name in Enum.GetNames(typeof(Shared.DelivarytStatus)))
        {
            dict.Add((int)Enum.Parse(typeof(Shared.DelivarytStatus), name), name);
        }
        return dict.ToList();
    }
    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public dynamic UpdategePymentStatus(int TransactionId, int StatusVal)
    {
        try
        {
            var repository = new PaymentTransactionRepository();

            PaymentTransaction transaction = repository.Single(p => p.PaymentTransactionId == TransactionId);

            transaction.PaymentStatus = StatusVal;

            repository.Update(transaction);
            return new CustomResponse
             {
                 Status = Shared.ResponseStatus.Success.ToString(),
                 Message = CustomMessages.UpdatedStatus,
                 Result = "Success",
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

    [WebMethod]
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

    [WebMethod]
    public bool CreateSession(string sessionType, Utility.User sessionObject)
    {
        var session = (Shared.Sessions)Enum.Parse(typeof(Shared.Sessions), sessionType);
        BalUtility.CreateSession(sessionObject, session);
        return true;
    }

    [WebMethod]
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

    [WebMethod(EnableSession = true)]
    public dynamic GetAdminSession()
    {
        try
        {
            long UserId = 0;
            var firstName = "";
            var lastName = "";
            var email = "";
            var phoneNumber = "";
            var User = (User)BalUtility.GetSession(Shared.Sessions.AdminLogin);
            if (User != null)
            {
                UserId = User.UserId;
                firstName = User.FirstName;
                lastName = User.LastName;
                email = User.EmailId;
                phoneNumber = User.MobileNo.ToString();
            }
            return new
            {
                UserId = UserId,
                firstName = firstName,
                lastName = lastName,
                email = User.EmailId,
                phoneNumber = phoneNumber
            };
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            throw;
        }
    }

    [WebMethod(EnableSession = true)]
    public dynamic GetUserSession()
    {
        try
        {
            long UserId = 0;

            var firstName = "";
            var lastName = "";
            var email = "";
            var phoneNumber = "";
            var addressStreet1 = "";
            var addressCity = "";
            var addressState = "";
            var addressZip = "";
            var orderAmount = "0";
            var CurrencyCode = "";
            var CurrencySymbol = "";
            decimal CurrencyValue = 1;
            Caluclations caluclations = BalUtility.GenerateOrderSummary();
            var User = (User)BalUtility.GetSession(Shared.Sessions.CustomerLogin);
            var cartItems = (List<UserProductTransaction>)BalUtility.GetSession(Shared.Sessions.CustomerCart) ??
                              new List<UserProductTransaction>();
            var totalsum = cartItems.Sum(p => p.ProductCost);
            if (User != null)
            {
                UserId = User.UserId;
                firstName = User.FirstName;
                lastName = User.LastName;
                email = User.EmailId;
                phoneNumber = User.MobileNo.ToString();
            }

            if (BalUtility.GetCart().Count > 0)
            {
                var addressrepository = new AddressRepository();
                var cartShippingAddressId = BalUtility.GetCart()[0].ShippingAddressId;
                UserAddress cartShippingAddress = addressrepository.First(p => p.UserId == UserId && p.UserAddressId == cartShippingAddressId);


                addressStreet1 = cartShippingAddress != null ? cartShippingAddress.StreetAddress1 + " " +
                    cartShippingAddress.StreetAddress2 + " " +
                    cartShippingAddress.LandMark : "";
                addressCity = cartShippingAddress != null ? cartShippingAddress.City : "";
                addressState = cartShippingAddress != null ? cartShippingAddress.StateName : "";
                addressZip = cartShippingAddress != null ? cartShippingAddress.PinCode : "";
                var orderSummary = (Caluclations)BalUtility.GetSession(Shared.Sessions.ShippingInfo);

                orderAmount = ((caluclations == null) ? 0 : (caluclations.TotalPriceAfterDeductions)).ToString();
                CurrencyCode = ((CustomCurrency)BalUtility.GetSession(Shared.Sessions.Currency)).ToCurrency;
                CurrencySymbol = ((CustomCurrency)BalUtility.GetSession(Shared.Sessions.Currency)).Symbol;
                CurrencyValue = ((CustomCurrency)BalUtility.GetSession(Shared.Sessions.Currency)).Value;
            }

            return new
            {
                UserId = UserId,
                firstName = firstName,
                lastName = lastName,
                email = email,
                phoneNumber = phoneNumber,
                addressStreet1 = addressStreet1,
                addressCity = addressCity,
                addressState = addressState,
                addressZip = addressZip,
                orderAmount = orderAmount,
                CurrencyCode = CurrencyCode,
                CurrencySymbol = CurrencySymbol,
                CurrencyValue = CurrencyValue
            };


        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            throw;
        }
    }


    [WebMethod(EnableSession = true)]
    public dynamic AddToReplacement(UserProductTransaction list)
    {
        var pId = list.ProductId;
        var spid = list.SubProductId;
        Product product = null;
        var cartItems = (List<UserProductTransaction>)BalUtility.GetSession(Shared.Sessions.ReplacementList) ??
                           new List<UserProductTransaction>();

        list.CurrencyCode = list.CurrencyCode;
        list.CurrencyValue = list.CurrencyValue;
        list.CurrencySymbol = list.CurrencySymbol;
        //Product Info
        list.Status = 0;
        list.Product = product;
        list.ProductCost = list.ProductCost;

        BalUtility.CreateSession(cartItems, Shared.Sessions.ReplacementList);
        return new CustomResponse
        {
            Status = Shared.ResponseStatus.Success.ToString(),
            Message = CustomMessages.CartAddedSuccessfully,

        };
    }


    [WebMethod(EnableSession = true)]
    public dynamic AddToCart(int isRedirect, UserProductTransaction cart)
    {
        try
        {
            db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();
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


            //issue cause point

            int ProductExistingQty = 0;

            if (spid == 0)
            {
                ProductExistingQty = cartItems.Where(a => a.ProductId == pId).Sum(a => a.Quantity);
            }
            else
            {
                ProductExistingQty = cartItems.Where(a => a.ProductId == pId && a.SubProductId == spid).Sum(a => a.Quantity);
            }


            // issue cause point ending

            if (product.Quantity >= (cart.Quantity))
            {
                if (cartItems.Count > 0)
                {
                    //issue causing point


                    UserProductTransaction item = new UserProductTransaction();
                    int ActualProductQuantity = 0;

                    if (spid == 0)
                    {
                        item = (from a in cartItems where a.ProductId == pId select a).FirstOrDefault();
                        ActualProductQuantity = Entity.Products.Where(x => x.ProductId == pId).First().Quantity;
                    }

                    else
                    {
                        item = (from a in cartItems where a.ProductId == pId && a.SubProductId == spid select a).FirstOrDefault();
                        ActualProductQuantity = Entity.SubProducts.Where(x => x.SubProductId == spid && x.ProductId == pId).First().Quantity;
                    }



                    //issue causing end point

                    if (item != null)
                    {

                        item.Quantity = cart.Quantity + item.Quantity;
                        if (item.Quantity > ActualProductQuantity)
                        {
                            item.Quantity = item.Quantity - cart.Quantity;
                            item.ProductCost = item.Product.ProductCost * item.Quantity;

                            if (isRedirect == 1)
                            {
                                return new CustomResponse
                                {
                                    Status = Shared.ResponseStatus.NoData.ToString(),
                                    Message = CustomMessages.Outofstock.ToString() + " Avalable Quantity is " + product.Quantity,
                                    Result = ActualProductQuantity,
                                };
                            }
                        }
                        else
                        {
                            item.ProductCost = item.Product.ProductCost * item.Quantity;
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
                        DeleteFromWishList(pId);
                        BalUtility.CreateSession(cartItems, Shared.Sessions.CustomerCart);

                        HttpCookie cookie = new HttpCookie("CartInfo");
                        if (cookie != null)
                        {
                            cookie["ProductID"] = cookie["ProductID"] + "," + pId.ToString();
                            cookie["ProductCost"] = cookie["ProductCost"] + "," + product.ProductCost.ToString();
                            cookie["Quantity"] = cookie["Quantity"] + "," + cart.Quantity.ToString();
                        }
                        else
                        {
                            cookie["ProductID"] = pId.ToString();
                            cookie["ProductCost"] = product.ProductCost.ToString();
                            cookie["Quantity"] = cart.Quantity.ToString();
                        }
                        cookie.Expires = DateTime.Now.AddDays(30);
                        HttpContext.Current.Response.Cookies.Add(cookie);
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
                    DeleteFromWishList(pId);
                    BalUtility.CreateSession(cartItems, Shared.Sessions.CustomerCart);

                    HttpCookie cookie = new HttpCookie("CartInfo");
                    cookie["ProductID"] = pId.ToString();
                    cookie["ProductCost"] = product.ProductCost.ToString();
                    cookie["Quantity"] = cart.Quantity.ToString();
                    cookie.Expires = DateTime.Now.AddDays(30);
                    HttpContext.Current.Response.Cookies.Add(cookie);

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
                    Result = cartItems.Count == 0 ? cartItems.Count + " item(s) - " + cartItems.Sum(p => p.ProductCost)
                    : "<a href='" + ApiUrl + "../MyCart.aspx'>" + cartItems.Count + " item(s) - " + cartItems.Sum(p => p.ProductCost) + "</a>",
                    price = cartItems.Sum(p => p.ProductCost),
                };

            }
            else
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Fail.ToString(),
                    Message = CustomMessages.Outofstock.ToString() + " Avalable Quantity is " + product.Quantity,
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



    [WebMethod(EnableSession = true)]
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

            int ProductExistingQty = 0;
            if (spid == 0)
            {
                ProductExistingQty = cartItems.Where(a => a.ProductId == pId).Sum(a => a.Quantity);
            }
            else
            {
                ProductExistingQty = cartItems.Where(a => a.ProductId == pId && a.SubProductId == spid).Sum(a => a.Quantity);
            }

            if (product.Quantity >= (cart.Quantity))
            //  if (product.Quantity >= cart.Quantity)
            {
                if (cartItems.Count > 0)
                {
                    UserProductTransaction item = new UserProductTransaction();

                    if (spid == 0)
                    {
                        item = (from a in cartItems where a.ProductId == pId select a).FirstOrDefault();
                    }
                    else
                    {
                        item = (from a in cartItems where a.ProductId == pId && a.SubProductId == spid select a).FirstOrDefault();
                    }


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
                        DeleteFromWishList(pId);
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
                    DeleteFromWishList(pId);
                    BalUtility.CreateSession(cartItems, Shared.Sessions.CustomerCart);
                }
                if (isRedirect == 1)
                {
                    return new CustomResponse
                    {
                        Status = Shared.ResponseStatus.Success.ToString(),
                        Message = CustomMessages.CartAddedSuccessfully,
                        price = product.ProductCost * cart.Quantity,
                        CartSum = cartItems.FirstOrDefault().CurrencySymbol + product.ProductCost * cart.Quantity,
                        Result = cart.Quantity//cartItems.Count //+ " item(s) - " + cartItems.Sum(p => p.ProductCost)
                    };
                }
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = CustomMessages.CartAddedSuccessfully,
                    price = product.ProductCost * cart.Quantity,
                    CartSum = cartItems.FirstOrDefault().CurrencySymbol + product.ProductCost * cart.Quantity,
                    Result = cartItems.Count == 0 ? cartItems.Count + "" : cartItems.Count + ""
                };

            }
            else
            {

                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Fail.ToString(),
                    Message = CustomMessages.Outofstock.ToString() + ". Available Quantity is :" + product.Quantity,
                    //CartSum =cartItems.FirstOrDefault().CurrencySymbol + product.ProductCost*(cart.Quantity),
                    Result = product.Quantity,
                };
            }
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            throw;
        }
    }


    [WebMethod(EnableSession = true)]
    public dynamic SubPrdUpdateQuantityToCart(int isRedirect, UserProductTransaction cart)
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

            var ProductExistingQty = cartItems.Where(a => a.ProductId == pId && a.SubProductId == spid).Sum(a => a.Quantity);
            if (product.Quantity >= (cart.Quantity))
            //if (product.Quantity >= cart.Quantity)
            {



                if (cartItems.Count > 0)
                {
                    var item = (from a in cartItems where a.ProductId == pId && (a.SubProductId == 0 ? a.SubProductId == spid : true) select a).FirstOrDefault();
                    if (item != null)
                    {
                        item.Quantity = cart.Quantity;
                        item.ProductCost = item.Product.ProductCost * cart.Quantity;

                        //BalUtility.CreateSession(cartItems, Shared.Sessions.CustomerCart);
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
                        DeleteFromWishList(pId);
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
                    DeleteFromWishList(pId);
                    //BalUtility.CreateSession(cartItems, Shared.Sessions.CustomerCart);
                }






                if (isRedirect == 1)
                {
                    return new CustomResponse
                    {
                        Status = Shared.ResponseStatus.Success.ToString(),
                        Message = CustomMessages.CartAddedSuccessfully,
                        price = product.ProductCost * cart.Quantity,
                        CartSum = cartItems.FirstOrDefault().CurrencySymbol + product.ProductCost * cart.Quantity,
                        Result = cartItems.Count + " item(s) - " + cartItems.Sum(p => p.ProductCost)
                    };
                }
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = CustomMessages.CartAddedSuccessfully,
                    price = product.ProductCost * cart.Quantity,
                    CartSum = cartItems.FirstOrDefault().CurrencySymbol + product.ProductCost * cart.Quantity,
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
                    //CartSum = cartItems.FirstOrDefault().CurrencySymbol + product.ProductCost * (cart.Quantity),
                    price = product.ProductCost * cart.Quantity,
                    Result = product.Quantity,

                };
            }
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            throw;
        }
    }

    //decrease product quantity
    [WebMethod(EnableSession = true)]
    public dynamic decreaseSubPrdUpdateQuantityToCart(int isRedirect, UserProductTransaction cart)
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

            // var ProductExistingQty = cartItems.Where(a => a.ProductId == pId && (a.SubProductId == 0 ? true : a.SubProductId == spid)).Sum(a => a.Quantity);
            //if (product.Quantity >= (ProductExistingQty + cart.Quantity))
            if (product.Quantity >= cart.Quantity)
            {
                if (cartItems.Count > 0)
                {
                    var item = (from a in cartItems where a.ProductId == pId && (a.SubProductId == 0 ? a.SubProductId == spid : true) select a).FirstOrDefault();
                    if (item != null)
                    {
                        item.Quantity = cart.Quantity;
                        item.ProductCost = item.Product.ProductCost * cart.Quantity;

                        //BalUtility.CreateSession(cartItems, Shared.Sessions.CustomerCart);
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
                        DeleteFromWishList(pId);
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
                    DeleteFromWishList(pId);
                    //BalUtility.CreateSession(cartItems, Shared.Sessions.CustomerCart);
                }
                if (isRedirect == 1)
                {
                    return new CustomResponse
                    {
                        Status = Shared.ResponseStatus.Success.ToString(),
                        Message = CustomMessages.CartAddedSuccessfully,
                        price = product.ProductCost * cart.Quantity,
                        CartSum = cartItems.FirstOrDefault().CurrencySymbol + product.ProductCost * cart.Quantity,
                        Result = cartItems.Count + " item(s) - " + cartItems.Sum(p => p.ProductCost)
                    };
                }
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = CustomMessages.CartAddedSuccessfully,
                    price = product.ProductCost * cart.Quantity,
                    CartSum = cartItems.FirstOrDefault().CurrencySymbol + product.ProductCost * cart.Quantity,
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
                    //CartSum = cartItems.FirstOrDefault().CurrencySymbol + product.ProductCost * (cart.Quantity),
                    price = product.ProductCost * cart.Quantity,
                    Result = product.Quantity,

                };
            }
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            throw;
        }
    }
    //decrease product quantity

    public dynamic DeleteFromCart(int id, int sid)
    {
        try
        {
            BalUtility.DeleteCartItem(id, sid);

            return new
            {
                success = "true",
                total = BalUtility.CartInfo()
            };
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            throw;
        }
    }

    [WebMethod(EnableSession = true)]
    public dynamic DeleteFromWishList(long id)
    {
        try
        {
            BalUtility.DeleteWishListItem(id);
            return new
            {
                success = "true",
                total = BalUtility.WishListInfo()
            };
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            throw;
        }
    }

    [WebMethod(EnableSession = true)]
    public dynamic Checkcartcount()
    {
        try
        {
            var cartItems = BalUtility.GetCart();
            var count = cartItems.Count;
            if (count != 0)
            {
                return count;
            }
            else
            {
                return 0;
            }
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            throw;
        }
    }

    [WebMethod(EnableSession = true)]
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
    [WebMethod(EnableSession = true)]
    public bool CheckProductInWishList(int productId)
    {
        try
        {
            var User = (User)BalUtility.GetSession(Shared.Sessions.CustomerLogin);
            if (User != null)
            {
                var cartItems = (List<CustWishList>)BalUtility.GetSession(Shared.Sessions.CustomerWishList) ?? new List<CustWishList>();
                var count = (from a in cartItems where a.ProductId == productId select a).Count();
                return count != 0;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            throw;
        }
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true)]
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

    [ScriptMethod(UseHttpGet = true)]
    [WebMethod(EnableSession = true)]
    public dynamic CartShortInfo()
    {
        // var CurrentCurrency = (CustomCurrency)BAL.BalUtility.GetSession(Shared.Sessions.Currency);
        CustomCurrency data = (CustomCurrency)GetCurrency();
        decimal cost = 0;

        try
        {
            var cartItems = (List<UserProductTransaction>)BalUtility.GetSession(Shared.Sessions.CustomerCart) ??
                              new List<UserProductTransaction>();

            Caluclations caluclations = BalUtility.GenerateOrderSummary();
            cost = Math.Round(caluclations.TotalPriceAfterDeductions, 2, MidpointRounding.ToEven);

            return new
            {
                //  success = CustomMessages.CartAddedSuccessfully,
                total = cartItems.Count == 0 ? cartItems.Count + " item(s) - " + data.ToCurrency + " ( " + data.Symbol + " ) " + "" + Math.Round(cost, 2, MidpointRounding.ToEven)
                : "<a href='MyCart.aspx'>" + cartItems.Count + " item(s) - " + data.ToCurrency + " ( " + data.Symbol + " ) " + "" + Math.Round(cost, 2, MidpointRounding.ToEven) + "</a>"

            };
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            throw;
        }
    }

    //[ScriptMethod(UseHttpGet = true)]
    //[WebMethod(EnableSession = true)]
    //public CustomResponse WishListShortInfo()
    //{
    //    try
    //    {
    //        var cartItems = (List<CustWishList>)BalUtility.GetSession(Shared.Sessions.CustomerWishList) ??
    //                          new List<CustWishList>();


    //        return new CustomResponse
    //        {
    //            total = cartItems.Count == 0 ? "Wish List (" + cartItems.Count + ")" : "<a href='WishList.aspx'>Wish List (" + cartItems.Count + ")</a>"
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        // Shared.Log.Error(ex);
    //        throw;
    //    }
    //}

    [ScriptMethod(UseHttpGet = true)]
    [WebMethod(EnableSession = true)]
    public List<CustWishList> ShowWishList()
    {
        try
        {
            List<CustWishList> items = (List<CustWishList>)BalUtility.GetSession(Shared.Sessions.CustomerWishList) ?? new List<CustWishList>();


            //var data = subCategoryId == 0
            //              ? repository.FetchAllByPage(p => p.CreatedOn, out totalRecords, rows, 10,
            //                                          (p =>
            //                                           p.IsActive && p.IsDeleted == false  )).Where( y => y == x.CategoryID);
            //              : repository.FetchAllByPage(p => p.ProductId, out totalRecords, rows, 10,
            //                                          (p =>
            //                                           p.IsActive && p.IsDeleted == false &&
            //                                           p.SubCategoryId == categoryId)).Where( y => y == x.CategoryID);
            //return data;
            return items;
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            throw;
        }
    }

    [WebMethod(EnableSession = true)]
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
            custWishList.ProductCost = product.ProductCost;



            WishlistItems.Add(custWishList);
            BalUtility.CreateSession(WishlistItems, Shared.Sessions.CustomerWishList);
            if (WishlistItems.Count > 0)
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.Success.ToString(),
                    Message = CustomMessages.WishlistAddedSuccessfully,

                    Result = WishlistItems.Count == 0 ? "Wish List (" + WishlistItems.Count + ")" : "<a href='WishList.aspx'>Wish List (" + WishlistItems.Count + ")</a>"
                };
            }
            else
            {
                return new CustomResponse
                {
                    Status = Shared.ResponseStatus.NoData.ToString(),
                    Message = null,

                    Result = WishlistItems.Count == 0 ? "Wish List (" + WishlistItems.Count + ")" : "<a href='WishList.aspx'>Wish List (" + WishlistItems.Count + ")</a>"
                };
            }
        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            throw;
        }
    }

    [WebMethod(EnableSession = true)]
    public dynamic MakePaymentCitrus()
    {
        try
        {
            var repository = new PaymentTransactionRepository();
            var cart = BalUtility.GetCart();
            PaymentTransaction payment = repository.MakePayment(cart);
            try
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies["CartInfo"];
                if (cookie != null)
                {
                    HttpContext.Current.Response.Cookies["CartInfo"].Expires = DateTime.Now.AddDays(-1);
                }
                BalUtility.SendMailForProductStatus(payment, Shared.GethtmlPage(Shared.ProductStatusForSendMail.Pending), Shared.GetPrdctStatusSubject(Shared.ProductStatusForSendMail.Pending));
            }
            catch (Exception ex)
            {

            }
            BalUtility.ClearSession(Shared.Sessions.CustomerCart);

            return new
            {
                id = payment.PaymentTransactionId
            };

        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            throw;
        }
    }


    [WebMethod(EnableSession = true)]
    public dynamic MakePaymentPaytm()
    {
        try
        {
            var repository = new PaymentTransactionRepository();
            var cart = BalUtility.GetCart();
            PaymentTransaction payment = repository.MakePayment(cart, "Paytm");
            try
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies["CartInfo"];
                if (cookie != null)
                {
                    HttpContext.Current.Response.Cookies["CartInfo"].Expires = DateTime.Now.AddDays(-1);
                }
                BalUtility.SendMailForProductStatus(payment, Shared.GethtmlPage(Shared.ProductStatusForSendMail.Pending), Shared.GetPrdctStatusSubject(Shared.ProductStatusForSendMail.Pending));
            }
            catch (Exception ex)
            {

            }
            BalUtility.ClearSession(Shared.Sessions.CustomerCart);

            Session["PaymentTransactionId"] = payment.PaymentTransactionId.ToString();


            //String merchantKey = "5xuvD%oqWxJ@UHR@";
            String merchantKey = "c#%khtp1LtG8vxmh";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("REQUEST_TYPE", "DEFAULT");
            parameters.Add("MID", "SonalE04048888945700");
            parameters.Add("ORDER_ID", payment.PaymentTransactionId.ToString());
            parameters.Add("CUST_ID", payment.UserId.ToString());
            parameters.Add("TXN_AMOUNT", payment.TxnAmount.ToString() + ".00");
            parameters.Add("CHANNEL_ID", "WEB");
            parameters.Add("INDUSTRY_TYPE_ID", "Retail109");
            parameters.Add("WEBSITE", "SonalEWEB");
            parameters.Add("EMAIL", "test@g.com");
            parameters.Add("MOBILE_NO", "8885044001");
            string checksum = CheckSum.generateCheckSum(merchantKey, parameters);
            checksum = HttpContext.Current.Server.UrlEncode(checksum);
            //  bool checksumverify = CheckSum.verifyCheckSum(merchantKey, parameters, checksum);          

            return new
            {
                id = payment.PaymentTransactionId,
                userId = payment.UserId,
                checkSum = checksum
            };

        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            throw;
        }
    }


    [WebMethod(EnableSession = true)]
    public dynamic MakePayment(string COD)
    {
        try
        {
            var repository = new PaymentTransactionRepository();
            var cart = BalUtility.GetCart();
            
            PaymentTransaction payment = repository.MakePayment(cart, COD);
            try
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies["CartInfo"];
                if (cookie != null)
                {
                    HttpContext.Current.Response.Cookies["CartInfo"].Expires = DateTime.Now.AddDays(-1);
                }
                BalUtility.SendMailForProductStatus(payment, Shared.GethtmlPage(Shared.ProductStatusForSendMail.Pending), Shared.GetPrdctStatusSubject(Shared.ProductStatusForSendMail.Pending));
            }
            catch (Exception ex)
            {

            }
            BalUtility.ClearSession(Shared.Sessions.CustomerCart);

            return new
            {
                id = payment.PaymentTransactionId
            };

        }
        catch (Exception ex)
        {
            // Shared.Log.Error(ex);
            throw;
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
            ShippingAddressId = data.FirstOrDefault().ShippingAddressId

        };
        var cartItems = (List<UserProductTransaction>)BalUtility.GetSession(Shared.Sessions.ReplacementList) ??
                           new List<UserProductTransaction>();

        cartItems.Add(cart);

        BalUtility.CreateSession(cartItems, Shared.Sessions.ReplacementList);
        return cartItems.ToList();
    }

    [WebMethod]
    public static decimal Convert(string fromCurrency, string toCurrency)
    {
        try
        {
            return Utility.Shared.ConvertAmount(1, fromCurrency, toCurrency);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod(EnableSession = true)]
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

    [WebMethod(EnableSession = true)]
    public dynamic NewUserRegistration(User objUser)
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
            return new CustomResponse
            {
                Status = Shared.ResponseStatus.Fail.ToString(),
                Message = ex.Message,
                Result = null
            };
        }
    }


}
