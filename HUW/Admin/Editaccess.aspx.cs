using BAL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;

public partial class Admin_Editaccess : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Getaccessdetails();
        }
    }
    public void Getaccessdetails()
    {
        using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
        {

            int UID = Convert.ToInt32(Request.QueryString["ID"]);


            def_accesss getuser = context.def_accesss.Where(x => x.AccessID == UID).First();
            txtname.Text = getuser.Pagename;
            txtpageurl.Text = getuser.PageUrl;
            CheckBox1.Checked = getuser.IsPage;
            //txtemail.Text = getuser.EmailId;
            //txtpass.Text = getuser.PassCode;
            //txtmbl.Text = (getuser.MobileNo).ToString();
        }
    }

    protected void btnSavedetails_Click(object sender, EventArgs e)
    {
        int UID = Convert.ToInt32(Request.QueryString["ID"]);
        def_accesss user = new def_accesss();
        db_Zon_HuwEntities context = new db_Zon_HuwEntities();

        user.AccessID = UID;
        user.Pagename = txtname.Text;
        user.PageUrl = txtpageurl.Text;
        user.IsPage = CheckBox1.Checked;


        user.UpdatedOn = DateTime.Now;
        context.SaveChanges();
        var repository = new AccessRepository();
        var currentProd = repository.Single(p => p.AccessID == UID);
        repository.Updatedata(user);
        //def.Employeeid = Convert.ToInt32(UID);
        //def.IsActive = false;
        //repository.Updateaccess(def);


    }
}
