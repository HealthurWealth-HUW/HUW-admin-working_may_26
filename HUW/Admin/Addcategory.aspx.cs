using BAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;

public partial class Admin_Addcategory : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindSuperCategories();
        }
    }
    public void BindSuperCategories()
    {
        ddlSuperCatogeries.Items.Clear();
        ddlSuperCatogeries.DataTextField = "SuperCategoryName";
        ddlSuperCatogeries.DataValueField = "SuperCategoryId";
        var repository = new SuperCategoryRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.SuperCategoryId, out totalRecords, 0, 0, (p => p.IsActive && p.IsDeleted == false), c => new { c.SuperCategoryId, c.SuperCategoryName, c.IsActive });
        ddlSuperCatogeries.DataSource = data;
        ddlSuperCatogeries.DataBind();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        if (ddlSuperCatogeries.SelectedValue == "0"||txtCategoryname.Text=="")
        {
            lblError.Text = "Please select Supercategory type";
        }
        else
        {
            var isexist = BalUtility.GetSession(Shared.Sessions.Employee);
            var isexists = BalUtility.GetSession(Shared.Sessions.AdminLogin);
            var isexistss = BalUtility.GetSession(Shared.Sessions.SuperAdminLogin);
            var repository = new CategoryRepository();
            if (isexist != null)
            {
                repository.Save(txtCategoryname.Text,Convert.ToInt32(ddlSuperCatogeries.SelectedValue), ((User)BalUtility.GetSession(Shared.Sessions.Employee)).UserId);

            }
            if (isexists != null)
            {
                repository.Save(txtCategoryname.Text, Convert.ToInt32(ddlSuperCatogeries.SelectedValue), ((User)BalUtility.GetSession(Shared.Sessions.AdminLogin)).UserId);

            }
            if (isexistss != null)
            {
                repository.Save(txtCategoryname.Text, Convert.ToInt32(ddlSuperCatogeries.SelectedValue), ((User)BalUtility.GetSession(Shared.Sessions.SuperAdminLogin)).UserId);

            }
            lblError.Text = "New Category Added";

        }


    }
}