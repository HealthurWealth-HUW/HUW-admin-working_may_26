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

public partial class Admin_AddBox : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
         
        }
    }

    protected void ddlDiscountType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlDiscountType.SelectedValue == "0")
        //{
        //    lblError.Text = "Please select discount type";
        //}
        //else if (ddlDiscountType.SelectedValue == "1")
        //{
        //    lblError.Text = "";
        //    divAmount.Visible = true;
        //    divPercentage.Visible = false;
        //}
        //else if (ddlDiscountType.SelectedValue == "2")
        //{
        //    lblError.Text = "";
        //    divAmount.Visible = false;
        //    divPercentage.Visible = true;
        //}
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string connString = ConfigurationManager.ConnectionStrings["db_Zon_constr"].ConnectionString;
        SqlConnection conn = new SqlConnection(connString);
        try
        {
            conn.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                if(txthiddent.Text=="")
                cmd.CommandText = "INSERT INTO Tbl_Packing_Box(BoxName,Lengths,Height,Width,Created_On,IsActive) Values ('" + txtBoxName.Text + "','" + txtLength.Text + "','" + txtHeight.Text + "','" + txtWidth.Text + "',GETDATE(),1)";
                else
                {
                    cmd.CommandText = "UPDATE Tbl_Packing_Box  SET BoxName='" + txtBoxName.Text + "',Lengths='" + txtLength.Text + "',Height='" + txtHeight.Text + "',Width='" + txtWidth.Text + "',Created_On=GETDATE() where BoxId="+txthiddent.Text;
                }
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 1)
                {
                    lblError.Text = "Box Submitted successfully...";
                    txtBoxName.Text = "";  txtHeight.Text = "";txtLength.Text = "";txtWidth.Text = "";
                }
                else
                {
                    lblError.Text = "Something wrong with Box adding... please try again";
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