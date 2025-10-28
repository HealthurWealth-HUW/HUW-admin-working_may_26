using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using Utility;

public partial class Admin_Admin_ForgotPassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {       
           
        if(!IsPostBack)
          lblErrormessage.Text="Enter your e-mail address below to reset your password.";

    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Admin/Login.aspx");
    }
    protected void btnForgotSubmit_Click(object sender, EventArgs e)    
    {
        //if (EmailId.Value != null || EmailId.Value != "" || EmailId.Value != string.Empty)
        if(EmailId.Value == "")
        {
            lblErrormessage.ForeColor = System.Drawing.Color.Red;
            lblErrormessage.Text = "Please Enter Valid EmailID, Try again.";            
        }
        else
        {
            Response.Redirect("../Admin/Login.aspx");
        }

        
    }
}