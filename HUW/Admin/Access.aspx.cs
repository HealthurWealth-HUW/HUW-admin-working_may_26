using BAL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;

public partial class Admin_Access : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSavedetails_Click(object sender, EventArgs e)
    {
        db_Zon_HuwEntities context = new db_Zon_HuwEntities();
        def_accesss admin = new def_accesss();
        admin.Pagename = txtname.Text;
        admin.PageUrl = txtpageurl.Text;
        admin.IsPage = CheckBox1.Checked;
        admin.CreatedOn = (DateTime.Now);
        admin.Createdby = (((User)BalUtility.GetSession(Shared.Sessions.Employee)).EmailId);
        context.def_accesss.Add(admin);
        context.SaveChanges();
        ScriptManager.RegisterStartupScript(this, this.GetType(),"alert","alert('Saved sucessfully');window.location ='Access.aspx';",true);


    }
}