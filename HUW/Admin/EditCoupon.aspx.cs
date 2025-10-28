using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_EditCoupon : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string Id = Request.QueryString["Id"];
            if (Id != null)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["db_Zon_constr"].ConnectionString;
                string selectSQL = "SELECT TOP 1000 [Coupon_Id],[Coupon_Code],[Valid_From],[Valid_To],[Min_Cart_Value],[Coupon_Percentage],[Coupon_Amount],[Total_No_Of_Times_Usage],[No_Of_Usages_Per_User],[Status] FROM [dbo].[Tbl_Coupon_Info] where Coupon_Id=" + Id;
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand(selectSQL, con);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds, "Coupon");
                DataTable firstTable = ds.Tables[0];

                lblCoupon_Id.Text = firstTable.Rows[0][0].ToString();
                txtCouponCode.Text = firstTable.Rows[0][1].ToString();
                txtValidfrom.Text = firstTable.Rows[0][2].ToString();
                txtValidTo.Text = firstTable.Rows[0][3].ToString();
                txtCartAMount.Text = firstTable.Rows[0][4].ToString();
                txtPercentage.Text = firstTable.Rows[0][5].ToString();
                txtAmount.Text = firstTable.Rows[0][6].ToString();
                txtUsage.Text = firstTable.Rows[0][7].ToString();
                txtUserUsage.Text = firstTable.Rows[0][8].ToString();

                if (txtPercentage.Text == "0.00" || string.IsNullOrEmpty(txtPercentage.Text))
                {
                    divPercentage.Visible = false;
                    divAmount.Visible = true;
                    ddlDiscountType.SelectedValue = "1";
                }
                if (txtAmount.Text == "0.00" || string.IsNullOrEmpty(txtAmount.Text))
                {
                    divPercentage.Visible = true;
                    divAmount.Visible = false;
                    ddlDiscountType.SelectedValue = "2";
                }
            }
        }
    }

    protected void ddlDiscountType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDiscountType.SelectedValue == "0")
        {
            lblError.Text = "Please select discount type";
        }
        else if (ddlDiscountType.SelectedValue == "1")
        {
            lblError.Text = "";
            divAmount.Visible = true;
            divPercentage.Visible = false;
        }
        else if (ddlDiscountType.SelectedValue == "2")
        {
            lblError.Text = "";
            divAmount.Visible = false;
            divPercentage.Visible = true;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        if (ddlDiscountType.SelectedValue == "0")
        {
            lblError.Text = "Please select discount type";
        }
        else
        {
            if (ddlDiscountType.SelectedValue == "1")
            {
                txtPercentage.Text = "0";
            }
            else if (ddlDiscountType.SelectedValue == "2")
            {
                txtAmount.Text = "0";
            }
        }

        string connString = ConfigurationManager.ConnectionStrings["db_Zon_constr"].ConnectionString;
        SqlConnection conn = new SqlConnection(connString);
        try
        {
            conn.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update [dbo].Tbl_Coupon_Info set Coupon_Code='" + txtCouponCode.Text + "',Valid_From='" + txtValidfrom.Text + "',Valid_To='" + txtValidTo.Text + "',Min_Cart_Value='" + txtCartAMount.Text + "',Coupon_Percentage='" + txtPercentage.Text + "',Coupon_Amount='" + txtAmount.Text + "',Total_No_Of_Times_Usage='" + txtUsage.Text + "',No_Of_Usages_Per_User='" + txtUserUsage.Text + "' where Coupon_Id=" + lblCoupon_Id.Text;
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 1)
                {
                    lblError.Text = "Coupon updated successfully...";
                }
                else
                {
                    lblError.Text = "Something wrong with Coupon adding... please try again";
                }
            }
        }
        catch (Exception ex)
        {
            //log error 
            //display friendly error to user
        }
        finally
        {
            if (conn != null)
            {
                //cleanup connection i.e close 
            }
        }
    }
}