using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;

public partial class Admin_EditUser : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
            {
                int userid = Convert.ToInt32(Request.QueryString["Id"].ToString());
                User userdata = Context.Users.Where(x => x.UserId == userid).FirstOrDefault();
                txtUsername.Text = userdata.EmailId;
                txtPassword.Text = userdata.PassCode;
                txtMobileNumber.Text = userdata.MobileNo.ToString();
                txtAMobileNumber.Text = userdata.AlternateContactNumber.ToString();
            }
        }

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        using (db_Zon_HuwEntities Context = new db_Zon_HuwEntities())
        {
            int userid = Convert.ToInt32(Request.QueryString["Id"].ToString());
            User userdata = Context.Users.Where(x => x.UserId == userid).FirstOrDefault();
            userdata.EmailId = txtUsername.Text;
            userdata.PassCode = txtPassword.Text;
            userdata.MobileNo = Convert.ToDecimal(txtMobileNumber.Text);
            if (txtAMobileNumber.Text != "")
            {
                userdata.AlternateContactNumber = Convert.ToDecimal(txtAMobileNumber.Text);
            }
            Context.SaveChanges();
            Response.Redirect("http://admin.healthurwealth.com/Admin/UserInfo.aspx");
        }
    }
}