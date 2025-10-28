using BAL;
using DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;

public partial class Admin_Updateemployee : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Getaccess();
            Getempdetails();
        }
    }
    private void Getempdetails()
    {
        using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
        {
            long UID = Convert.ToInt32(Request.QueryString["ID"]);
            List<tbl_accesspages> checks = context.tbl_accesspages.Where(x => x.Employeeid == UID && x.IsActive==true).ToList();
            foreach (ListItem item in chkaccess.Items)
            {
                item.Selected = true;
                foreach (var ch in checks)
            {  
                    def_accesss accs = context.def_accesss.Where(x => x.AccessID == ch.Accessid).First();
                    int itemss = Int32.Parse(item.Value);
                    if (ch.Accessid == itemss)
                    {
                        item.Selected = false;
                    }
                }
            }
            User getuser = context.Users.Where(x => x.UserId == UID).First();
            txtfirst.Text = getuser.FirstName;
            txtsecond.Text = getuser.LastName;
            txtemail.Text = getuser.EmailId;
            txtpass.Text = getuser.PassCode;
            txtmbl.Text = (getuser.MobileNo).ToString();
        }
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
    protected void btnSavedetails_Click1(object sender, EventArgs e)
    {
        long UID = Convert.ToInt32(Request.QueryString["ID"]);
        tbl_accesspages def = new tbl_accesspages();
        db_Zon_HuwEntities context = new db_Zon_HuwEntities();
        User user = new User();
        user.UserId = UID;
        user.RoleId = 6;
        user.FirstName = txtfirst.Text;
        user.LastName = txtsecond.Text;
        user.MiddleName = "";
        user.IsActive = txtCheckBox.Checked;
        user.PassCode = txtpass.Text;
        user.EmailId = txtemail.Text;
        string a = txtmbl.Text;
        decimal b = decimal.Parse(a);
        user.MobileNo = b;
        
        user.RegStatus = 0;
        user.IsDeleted = false;
        
        user.UpDatedOn = DateTime.Now;
        var repository = new employeeRepository();
        var currentProd = repository.Single(p => p.UserId == UID);
        repository.Updatedata(user);
        def.Employeeid = Convert.ToInt32(UID);
        def.IsActive = false;
        repository.Updateaccess(def);
        foreach (ListItem item in chkaccess.Items)
        {
            if (item.Selected == false)
            {
                def.Employeeid = (int)user.UserId; //should be done when checkbox is done when page page load
                def.Accessid = Convert.ToInt32(item.Value);
                def.IsActive = true;
                context.tbl_accesspages.Add(def);
                context.SaveChanges();
            }
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Updated sucessfully');window.location ='Home.aspx';", true);
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
}