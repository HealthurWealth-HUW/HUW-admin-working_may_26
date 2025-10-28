using BAL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;

public partial class Admin_Changepassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        errLabel.Visible = false;

    }

    protected void btnchangepassword_Click(object sender, EventArgs e)
    {
        var userId = ((User)BalUtility.GetSession(Shared.Sessions.Employee)).UserId;
        db_Zon_HuwEntities context = new db_Zon_HuwEntities();
        string oldpassword = txtOldpassword.Text;
        User udetails = context.Users.Where(x => x.UserId == userId).FirstOrDefault();
        if (udetails.PassCode == oldpassword)
        {
            if (txtNewpassword.Text == txtConfirmnewpassword.Text)
            {
                udetails.PassCode = txtNewpassword.Text;
                context.SaveChanges();
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Password changed successfully')", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(),"alert","alert('Password changed successfully');window.location ='Home.aspx';",
true);
            }
            else
            {
                errLabel.Text = "New password and confirm password doesnot match.";
                errLabel.Visible = true;

            }
        }
        else
        {
            errLabel.Text = "Old password doesnot match.";
            errLabel.Visible = true;
        }

    }
}