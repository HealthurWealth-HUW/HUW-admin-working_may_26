using DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;

public partial class Admin_AddEmployee : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) { 
        Getaccess();
        }
    }

    protected void btnSavedetails_Click(object sender, EventArgs e)
    {
        tbl_accesspages def = new tbl_accesspages();
        db_Zon_HuwEntities context = new db_Zon_HuwEntities();
        User user = new User();
        var userdata = context.Users.Where(x => x.EmailId == txtemail.Text).ToList();
        if (userdata.Count == 0)
        {
            user.RoleId = 6;
            user.FirstName = txtfirst.Text;
            user.LastName = txtsecond.Text;
            user.MiddleName = "";
            user.PassCode = txtpass.Text;
            user.EmailId = txtemail.Text;
            string a = txtmbl.Text;
            decimal b = decimal.Parse(a);
            user.MobileNo = b;
            user.IsActive = true;
            user.RegStatus = 0;
            user.IsDeleted = false;
            user.CreatedOn = DateTime.Now;
            user.UpDatedOn = DateTime.Now;
            context.Users.Add(user);
            context.SaveChanges();
            foreach (ListItem item in chkaccess.Items)
            {
                if (item.Selected == false)
                {
                    def.Employeeid = (int)user.UserId;
                    def.Accessid = Convert.ToInt32(item.Value);
                    def.IsActive = true;
                    context.tbl_accesspages.Add(def);
                    context.SaveChanges();
                }

            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Saved sucessfully');window.location ='AddEmployee.aspx';", true);
        }
        else
        {
            lblerror.InnerHtml = "EmailAlreadyExist";
        }
        //for (int i = 0; i < chkaccess.Items.Count; i++)
        //       {
        //    foreach (ListItem item in chkaccess.Items)
        //        {
        //        def.Employeeid = (int)user.UserId;
        //           def.Accessid = Convert.ToInt32(item.Value);
        //           context.def_access.Add(def);
        //           context.SaveChanges();
        //        }
        //       }

    }
    private void Getaccess()
    {
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager
                    .ConnectionStrings["db_Zon_constr"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "select * from def_accesss";
                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        ListItem item = new ListItem();
                        item.Text = sdr["Pagename"].ToString();
                        item.Value = sdr["AccessID"].ToString();
                        
                        chkaccess.Items.Add(item);
                    }
                }
                conn.Close();
            }
        }
    }
}