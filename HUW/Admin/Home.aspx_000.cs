using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using Utility;

public partial class Admin_Home : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(Session["RoleID"]) == 5)
            {
                hdrOrderStatistics.Visible = false;
                hdrSaleStatistics.Visible = false;
                hdrUserStatistics.Visible = false;
                hdrAmountStatistics.Visible = false;
                divOrderStatistics.Visible = false;
                divSaleStatistics.Visible = false;
                divUserStatistics.Visible = false;
                divAmountStatistics.Visible = false;
            }

            if (BalUtility.GetSession(Shared.Sessions.AdminLogin) == null && BalUtility.GetSession(Shared.Sessions.SuperAdminLogin) == null && BalUtility.GetSession(Shared.Sessions.SubAdminLogin) == null)
                Response.Redirect("Login.aspx");
            else
            {
                var repository = new ProductRepository();
                lblTotalProducts.Text = Convert.ToString(repository.Count(p => p.ProductId != 0));
                int salesofthismonth = repository.ProductDetails(DateTime.Now, DateTime.Now.AddDays(-30));
                lblSalesofthismonth.Text = salesofthismonth.ToString();

                DateTime WeekStartDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);

                int salesofthisweek = repository.ProductDetails(WeekStartDate, DateTime.Now.AddDays(-7));
                lblSalesofthisweek.Text = salesofthisweek.ToString();
                lblActiveProducts.Text = Convert.ToString(repository.Count(p => p.IsActive == true && p.IsDeleted == false));
                lblInactiveProducts.Text = Convert.ToString(repository.Count(p => p.IsActive == true && p.IsDeleted == true));
                lblOutOfStockProducts.Text = Convert.ToString(repository.Count(p => p.IsActive == true && p.IsDeleted == false && p.Quantity <= 0));

                var PaymentRepository = new PaymentTransactionRepository();

                lblUserCancelledOrders.Text = PaymentRepository.Count(p => p.TxnStatus == "CANCELED").ToString();
                lblAdminCancelledOrders.Text = PaymentRepository.Count(p => p.TxnMessage == "Cancelled by Admin").ToString();
                lblUnauthorizedorders.Text = PaymentRepository.Count(p => p.TxnStatus == null && p.TxnMessage == null).ToString();

                try
                {
                    var cost = new userproducttransactionRepository();
                    var startOfTthisMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    var FromMonthDate = startOfTthisMonth.AddMonths(0);
                    // var FromWeekDate = DateTime.Today.AddDays(-7);
                    var ToDate = DateTime.Now.AddDays(1);
                    decimal cst = cost.productcost(DateTime.Now, FromMonthDate);
                    lblsalesmonth.Text = cst.ToString("N2");
                    decimal week = cost.productcost(DateTime.Now, WeekStartDate);
                    lblsalesweek.Text = week.ToString("N2");
                    decimal Total = cost.Productcost();
                    lblsalestotal.Text = Total.ToString("N2");
                    decimal Today = cost.productcost(DateTime.Now.AddDays(1), DateTime.Today);
                    lblTodaySales.Text = Today.ToString("N2");
                }
                catch (Exception ex)
                {
                }

                var user = new UserRepository();
                int usermonth = user.UserDetails(DateTime.Now, DateTime.Now.AddDays(-30));
                lblRegisterUsersmonth.Text = usermonth.ToString();
                int userweek = user.UserDetails(DateTime.Now, DateTime.Now.AddDays(-7));
                lblthisweek.Text = userweek.ToString();
                lblTotalUSers.Text = Convert.ToString(user.Count(p => p.IsDeleted == false));

                var AuthorizedOrders = new PaymentTransactionRepository();
                lblAuthorizedOrders.Text = Convert.ToString(AuthorizedOrders.Count(p => p.TxnStatus == "SUCCESS" && p.Pickup == false && p.Authorized == true && p.Dispatched == false && p.Delivered == false));
                lblPickupOrders.Text = Convert.ToString(AuthorizedOrders.Count(p => p.TxnStatus == "SUCCESS" && p.Dispatched == false && p.Delivered == false && p.Pickup == true && p.Authorized == true));
                lblDispatchedOrders.Text = Convert.ToString(AuthorizedOrders.Count(p => p.TxnStatus == "SUCCESS" && p.Dispatched == true && p.Delivered == false && p.Pickup == true && p.Authorized == true));
                lblDeliveredOrders.Text = Convert.ToString(AuthorizedOrders.Count(p => p.TxnStatus == "SUCCESS" && p.Delivered == true && p.Dispatched == true && p.Pickup == true && p.Authorized == true));
                // lbldeliverPendingproducts.Text = Convert.ToString(AuthorizedOrders.Count(p => p.Delivered == false & p.TxnStatus == "Success").ToString());
            }
        }
        catch
        {
            throw;
        }
    }
}