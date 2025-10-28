using BAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;

public partial class Admin_Adminmaster1 : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var url = Request.Url.AbsolutePath.ToString();
            Regex Splitter = new Regex("/");
            String[] Parts = Splitter.Split(url);
            var x = BalUtility.Blockuser(Parts[2]);
            TabListItems();
            if (x == false)
            {
                isvisible.Visible = false;
                errmsgs.Visible = true;
                errmsgs.InnerHtml = "You are not authorized to access this page";
            }
            if (Convert.ToInt32(Session["RoleID"]) != null)
            {

                if (BalUtility.GetSession(Shared.Sessions.AdminLogin) == null && BalUtility.GetSession(Shared.Sessions.SuperAdminLogin) == null && BalUtility.GetSession(Shared.Sessions.SubAdminLogin) == null && BalUtility.GetSession(Shared.Sessions.Employee) == null)
                    Response.Redirect("Login.aspx");
                else
                {
                    var repository = new ProductRepository();
                    //    lblTotalProducts.Text = Convert.ToString(repository.Count(p => p.ProductId != 0));
                    //    lblActiveProducts.Text = Convert.ToString(repository.Count(p => p.IsActive == true && p.IsDeleted == false));
                    //    lblInActiveProducts.Text = Convert.ToString(repository.Count(p => p.IsActive == true && p.IsDeleted == true));
                    //    lblOutofstock.Text = Convert.ToString(repository.Count(p => p.IsActive == true && p.IsDeleted == false && p.Quantity <= 0));
                    //}
                }
            }
        }
    }
    private void TabListItems()
    {
        if (Convert.ToInt32(Session["RoleID"]) == 1)
            lblLoginUser.Text = "Welcome Admin";
        else if (Convert.ToInt32(Session["RoleID"]) == 3)
        {
            lblLoginUser.Text = "Welcome Super Admin";
            //liAWB.Visible = true;
            //liaddemployee.Visible = true;
            //liemplist.Visible = true;
        }
        else if (Convert.ToInt32(Session["RoleID"]) == 5)
        {
            lblLoginUser.Text = "Welcome Sub Admin";
            //liRefund.Visible = false;
            //liUserInfo.Visible = false;
            //liUserInfoSidetab.Visible = false;
            //liPaymentSatus.Visible = false;
            //liNotify.Visible = false;
            //liReports.Visible = false;
            //lioffline.Visible = false;
            //liTransaction.Visible = false;
            //liPaymentGateway.Visible = false;
            //liPerformance.Visible = false;
            //divstatistics.Visible = false;
        }
        else if (Convert.ToInt32(Session["RoleID"]) == 6)
        {
            lblLoginUser.Text = "Welcome " + ((User)BalUtility.GetSession(Shared.Sessions.Employee)).FirstName;
            User empdetails = ((User)BalUtility.GetSession(Shared.Sessions.Employee));
            var repository = new EmployeeRepository();
            var data = repository.Getaccesspages(empdetails.UserId);
            //    foreach (var accesspages in data)
            //    {
            //        if (accesspages.Accessid == 1)
            //        {
            //            liAddProductsSidetab.Visible = false;
            //        }
            //        else if (accesspages.Accessid == 2)
            //        {
            //            liActivateProductsSidetab.Visible = false;
            //        }
            //        else if (accesspages.Accessid == 3)
            //        {
            //            liProductSearchSidetab.Visible = false;
            //        }
            //        else if (accesspages.Accessid == 4)
            //        {
            //            liUserInfoSidetab.Visible = false;
            //        }
            //        else if (accesspages.Accessid == 5)
            //        {
            //            liSearchOrdersSidetab.Visible = false;
            //        }
            //        else if (accesspages.Accessid == 7)
            //        {
            //            liCheckoutSearchOrdersSidetab.Visible = false;
            //        }
            //        else if (accesspages.Accessid == 8)
            //        {
            //            liAuthorizedOrdersSidetab.Visible = false;

            //        }
            //        else if (accesspages.Accessid == 9)
            //        {
            //            liPickupOrdersSidetab.Visible = false;
            //        }
            //        else if (accesspages.Accessid == 10)
            //        {
            //            liDispatchedOrdersSidetab.Visible = false;
            //        }
            //        else if (accesspages.Accessid == 11)
            //        {
            //            liDeliveredOrdersSidetab.Visible = false;
            //        }
            //        else if (accesspages.Accessid == 12)
            //        {
            //            liAddCouponSidetab.Visible = false;
            //        }
            //        else if (accesspages.Accessid == 13)
            //        {
            //            liSearchCouponsSidetab.Visible = false;
            //        }
            //        else if (accesspages.Accessid == 14)
            //        {
            //            liRefund.Visible = false;
            //        }
            //        else if (accesspages.Accessid == 15)
            //        {
            //            liPaymentSatus.Visible = false;
            //        }
            //        else if (accesspages.Accessid == 16)
            //        {
            //            liNotify.Visible = false;
            //        }
            //        else if (accesspages.Accessid == 17)
            //        {
            //            liNeedHelp.Visible = false;
            //        }
            //        else if (accesspages.Accessid == 18)
            //        {
            //            liSalesReport.Visible = false;
            //        }
            //        else if (accesspages.Accessid == 19)
            //        {
            //            lioffline.Visible = false;
            //        }
            //        else if (accesspages.Accessid == 20)
            //        {
            //            liTransaction.Visible = false;
            //        }
            //        else if (accesspages.Accessid == 21)
            //        {
            //            liPaymentGateway.Visible = false;
            //        }
            //        else if (accesspages.Accessid == 22)
            //        {
            //            liPerformance.Visible = false;
            //        }
            //        lichangepassword.Visible = true;
            //    }
        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        if (BalUtility.GetSession(Shared.Sessions.AdminLogin) == null)
        {
            Response.Redirect("Login.aspx");
        }
        else
        {
            Response.Redirect("Home.aspx");
        }
    }
    protected void lnkNotifications_Click(object sender, EventArgs e)
    {
        // var repository = new ActionRepository();
        //var Data = repository.GetActionsPerformed();
    }
}
