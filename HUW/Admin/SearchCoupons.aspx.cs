using BAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_SearchCoupons : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
          
            string connectionString = ConfigurationManager.ConnectionStrings["db_Zon_constr"].ConnectionString;
            string selectSQL = "SELECT TOP 1000 [Coupon_Id],[Coupon_Code],[Valid_From],[Valid_To],[Min_Cart_Value],[Coupon_Percentage],[Coupon_Amount],[Total_No_Of_Times_Usage],[No_Of_Usages_Per_User],[Status] FROM [dbo].[Tbl_Coupon_Info]";
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(selectSQL, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "Coupon");
            gdvCoupons.DataSource = ds;
            gdvCoupons.DataBind();
        }
    }


    protected void gdvCoupons_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string id = e.CommandArgument.ToString();
        Response.Redirect("~/Admin/EditCoupon.aspx?Id=" + id);
    }
}