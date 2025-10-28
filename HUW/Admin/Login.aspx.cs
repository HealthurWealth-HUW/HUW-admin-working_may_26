using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using Utility;
public partial class Admin_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = "Please enter your User name and password to log in.";
    }
    protected void btnLogIn_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtUserName.Text))
        {
            lblMsg.Text = Utility.Utility.ReqNEmailId;
        }
        else if (!txtUserName.Text.IsValidEmailId())
        {
            lblMsg.Text = Utility.Utility.ValNEmailId;
            txtUserName.Text = string.Empty;
            txtUserName.Focus();
        }
        else if (string.IsNullOrEmpty(txtPwd.Text))
        {
            lblMsg.Text = Utility.Utility.ReqNPwd;
        }
        else if (Validations.IsAlphaNumeric(txtPwd.Text))
        {
            lblMsg.Text = Utility.Utility.ValPwd;
            txtPwd.Text = string.Empty;
            txtPwd.Focus();
        }

        try
        {
            var repository = new UserRepository();
            var userDetails = repository.First(u => u.EmailId == txtUserName.Text && u.PassCode == txtPwd.Text && (u.RoleId == (int)Shared.UserRoles.Admin || u.RoleId == (int)Shared.UserRoles.SuperAdmin || u.RoleId == (int)Shared.UserRoles.SubAdmin||u.RoleId==(int)Shared.UserRoles.Employee) && u.IsActive==true);
            if (userDetails == null )
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "User does not exist, Try with Valid Credentials.";
            }
            else
            {
                if (userDetails.RoleId == 3)
                {
                    BalUtility.CreateSession(userDetails, Shared.Sessions.SuperAdminLogin);
                }
                else if (userDetails.RoleId == 1)
                {
                    BalUtility.CreateSession(userDetails, Shared.Sessions.AdminLogin);
                }
                else if (userDetails.RoleId == 5)
                {
                    BalUtility.CreateSession(userDetails, Shared.Sessions.SubAdminLogin);
                }
                else if (userDetails.RoleId == 6)
                {
                    BalUtility.CreateSession(userDetails, Shared.Sessions.Employee);
                }
                Session["RoleID"] = userDetails.RoleId;
                if(userDetails.EmailId== "nitesh@healthurwealth.com")
                {
                    Response.Redirect("../Admin/Prescriptionorders.aspx");
                }
                Response.Redirect("../Admin/Home.aspx");
            }
        }
        catch (Exception ex)
        {

        }

    }
}
