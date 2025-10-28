using BAL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;

public partial class Admin_Prescriptionupdate : System.Web.UI.Page
{




    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            List<Utility.Product> updateobj = (List<Product>)BalUtility.GetSession(Shared.Sessions.prescriptionbuy) ??
                         new List<Product>();

            BalUtility.CreateSession(updateobj, Shared.Sessions.prescriptionbuy);
            BalUtility.ClearSession(Shared.Sessions.prescriptionbuy);
        }
        string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        var session = ((User)BalUtility.GetSession(Shared.Sessions.AdminLogin));
    }

    protected void btnbuy_Click(object sender, EventArgs e)
    {

        var iscartnull = (List<Product>)BalUtility.GetSession(Shared.Sessions.prescriptionbuy) ??
                         new List<Product>(); ;
        if (iscartnull.Count == 0)
        {
            Response.Write("<script>alert('Cart is empty, Please add few items to cart in order to make purchase');</script>");
        }
        else
        {

            Utility.Tbl_Coupon_Info promodata = (Tbl_Coupon_Info)BalUtility.GetSession(Shared.Sessions.Promocode) ??
                               new Tbl_Coupon_Info();





            var transid = Request.QueryString["transid"];
            db_Zon_HuwEntities context = new db_Zon_HuwEntities();

            long transidd = Convert.ToInt64(transid);
            var data = context.UserProductTransactions.Where(x => x.PaymentTransactionId == transidd).FirstOrDefault();
            context.UserProductTransactions.Remove(data);
            context.SaveChanges();
            var address = context.UserAddresses.Where(x => x.UserId == data.UserId).FirstOrDefault();
            UserProductTransaction upt = new UserProductTransaction();
            List<Utility.Product> updateobj = (List<Product>)BalUtility.GetSession(Shared.Sessions.prescriptionbuy) ??
                             new List<Product>();
            decimal totalprdctcost = 0;
            foreach (var item in updateobj)
            {

                var cost = context.Products.Where(x => x.ProductName == item.ProductName).FirstOrDefault();
                upt.PaymentTransactionId = Convert.ToInt64(transid);
                upt.UserId = data.UserId;
                upt.ProductId = cost.ProductId;
                upt.IsActive = true;
                upt.IsDeleted = false;
                upt.Quantity = updateobj.Where(z => z.ProductId == item.ProductId).FirstOrDefault().Quantity;
                upt.ProductCost = cost.ProductCost;
                upt.ProductDiscountPercentage = cost.ProductDiscountPercentage;
                upt.ProductDiscountCost = cost.ProductDiscountCost;
                upt.ShippingAddressId = address.UserAddressId;
                upt.BillingAddressId = address.UserAddressId;
                upt.CreatedOn = DateTime.Now;
                upt.UpdatedOn = DateTime.Now;
                upt.CreatedBy = 1;
                upt.Status = 0;
                upt.CurrencyCode = "INR";
                upt.CurrencyValue = 1;
                upt.CurrencySymbol = "?";
                upt.OrdersReturnReason = "";
                upt.OrdersReturnAction = "";
                upt.ReplacementTransactionId = 0;
                upt.IsReplaced = false;
                upt.Isprescription = true;
                upt.CashbackAmount = "0";

                context.UserProductTransactions.Add(upt);
                context.SaveChanges();
                if (totalprdctcost == 0)
                {
                    totalprdctcost = item.ProductCost * item.Quantity;
                }
                else
                {
                    totalprdctcost = totalprdctcost + (item.ProductCost * item.Quantity);
                }
            }
            var update = context.PaymentTransactions.Where(x => x.PaymentTransactionId == transidd).FirstOrDefault();
            if (promodata != null)
            {
                decimal discountamount = Convert.ToDecimal(promodata.Coupon_Amount);
                totalprdctcost = totalprdctcost - discountamount;
            }
            update.PaymentMode = "Cash On Delivery";
            update.TxnAmount = totalprdctcost;
            update.TxnStatus = "SUCCESS";
            update.OrderCurrentStatus = 6;
            update.Authorized = true;
            context.SaveChanges();
            BalUtility.ClearSession(Shared.Sessions.Promocode);
            Response.Write("<script>alert('Order has successfully placed.');</script>");
            Response.Redirect("../Admin/OrderDetails.aspx?transId="+transid);
        }
    }
}

