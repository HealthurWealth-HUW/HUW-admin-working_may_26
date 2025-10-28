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

public partial class Admin_AddCoupon : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
         
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
                cmd.CommandText = "INSERT INTO [dbo].Tbl_Coupon_Info(Coupon_Code,Valid_From,Valid_To,Min_Cart_Value,Coupon_Percentage,Coupon_Amount,Total_No_Of_Times_Usage,No_Of_Usages_Per_User,Created_Date,Status) Values ('" + txtCouponCode.Text + "','" + txtValidfrom.Text + "','" + txtValidTo.Text + "','" + txtCartAMount.Text + "','" + txtPercentage.Text + "','" + txtAmount.Text + "','" + txtUsage.Text + "','" + txtUserUsage.Text + "','" + DateTime.Now + "',1)";
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 1)
                {
                    lblError.Text = "Coupon added successfully...";
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