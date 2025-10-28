using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using DAL;
using Utility;

public partial class Admin_OrderDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var orderid = Request.QueryString["transId"];
        long longorderid = Convert.ToInt64(orderid);
        var repository = new UserProductTransactionRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.PaymentTransactionId, out totalRecords, 0, 0, (p => p.PaymentTransactionId == longorderid), null, "", true);
        string statusvalue = data.FirstOrDefault().PaymentTransaction.OrderCurrentStatus.ToString();
        var status = Shared.GetOrderStatusEnum(statusvalue);
        if (status == Shared.OrderStatus.Delivered || status == Shared.OrderStatus.Dispatched)
        {
            ListItem removeItem = ddlCancel.Items.FindByText("Cancel ");
            ddlCancel.Items.Remove(removeItem);
        }
        else
        {
            ListItem removeItem = ddlCancel.Items.FindByText("Return ");
            ddlCancel.Items.Remove(removeItem);
        }
           
        //ddlCancel.Items.Remove(ddlCancel.Items.FindByText("Cancel"));
        var orderidinlong = Convert.ToInt64(orderid);
        db_Zon_HuwEntities context = new db_Zon_HuwEntities();
        //var coupdetails = context.Cashback_Coups_Transactions.Where(y => y.orderID == orderidinlong).FirstOrDefault().Coup_ID;

        if (context.CashBackTables.Where(x => x.orderid == orderid).FirstOrDefault() != null)
        {
            var coupdetails = (from a in context.Cashback_Coups_Transactions
                               join b in context.Cashback_Coupons on a.Coup_ID equals b.Coup_ID
                               where a.orderID == orderidinlong
                               select new { cashbackID = a.Coup_ID, cashbackName = b.Coup_Code }).FirstOrDefault();
            if (coupdetails != null)
            {
                var wallettransactions = context.CashBackTables.Where(x => x.orderid == orderid).ToList();

                CBcoupnid.InnerHtml = "Cash back coupon: <a href='../../Admin/UpdateCoupons.aspx?ID=" + coupdetails.cashbackID.ToString() + "'><span style='color: #0e4f97 !important;text - decoration: none;padding:0 10px;text-decration:none;float:right' >" + coupdetails.cashbackName.ToString() + "</span></a>";
                wallettransactionsgrid.DataSource = wallettransactions;
                wallettransactionsgrid.DataBind();
            }
        }
    }
   
}