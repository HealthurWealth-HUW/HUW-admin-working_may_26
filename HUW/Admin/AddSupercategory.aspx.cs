using BAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using Utility;

public partial class Admin_AddSupercategory : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        if (txtsupercategory.Text == "")
        {
            lblError.Text = "Please select discount type";
        }
        else
        {
            var isexist = BalUtility.GetSession(Shared.Sessions.Employee);
            var isexists = BalUtility.GetSession(Shared.Sessions.AdminLogin);
            var isexistss = BalUtility.GetSession(Shared.Sessions.SuperAdminLogin);
            var repository = new SuperCategoryRepository();
            if (isexist != null)
            {
                repository.Save(txtsupercategory.Text, ((User)BalUtility.GetSession(Shared.Sessions.Employee)).UserId);

            }
            if (isexists != null)
            {
                repository.Save(txtsupercategory.Text, ((User)BalUtility.GetSession(Shared.Sessions.AdminLogin)).UserId);

            }
            if (isexistss != null)
            {
                repository.Save(txtsupercategory.Text, ((User)BalUtility.GetSession(Shared.Sessions.SuperAdminLogin)).UserId);

            }
            lblError.Text = "New Super Category Added";

        }


    }
}