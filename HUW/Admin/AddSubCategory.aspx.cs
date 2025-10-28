using BAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;

public partial class Admin_AddSubCategory : System.Web.UI.Page
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
        BindCategories();
    }
    public void BindCategories()
    {
        ddlCategory.Items.Clear();
        if (ddlSuperCatogeries.Items.Count > 0)
        {
            ddlCategory.DataTextField = "CategoryName";
            ddlCategory.DataValueField = "CategoryId";
            var repository = new CategoryRepository();
            int totalRecords;
            int categoryId = Convert.ToInt32(ddlSuperCatogeries.SelectedItem.Value);
            var data = repository.FetchAllByPage(p => p.SuperCategoryId, out totalRecords, 0, 0, (p => p.IsActive && p.IsDeleted == false && p.SuperCategoryId == categoryId), c => new { c.CategoryId, c.CategoryName, c.IsActive });
            ddlCategory.DataSource = data;
            ddlCategory.DataBind();
        }
    }
    protected void ddlSuperCatogeries_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindCategories();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        if (ddlSuperCatogeries.SelectedValue == "0" || ddlCategory.SelectedValue == "0"||txtSubCategoryname.Text=="")
        {
            lblError.Text = "Please select Supercategory type";
        }
        else
        {
            var isexist = BalUtility.GetSession(Shared.Sessions.Employee);
            var isexists = BalUtility.GetSession(Shared.Sessions.AdminLogin);
            var isexistss = BalUtility.GetSession(Shared.Sessions.SuperAdminLogin);
            var repository = new SubCategoryRepository();
            if (isexist != null)
            {
                repository.Save(txtSubCategoryname.Text, Convert.ToInt32(ddlSuperCatogeries.SelectedValue), Convert.ToInt32(ddlCategory.SelectedValue), ((User)BalUtility.GetSession(Shared.Sessions.Employee)).UserId);

            }
            if (isexists != null)
            {
                repository.Save(txtSubCategoryname.Text, Convert.ToInt32(ddlSuperCatogeries.SelectedValue), Convert.ToInt32(ddlCategory.SelectedValue), ((User)BalUtility.GetSession(Shared.Sessions.AdminLogin)).UserId);

            }
            if (isexistss != null)
            {
                repository.Save(txtSubCategoryname.Text, Convert.ToInt32(ddlSuperCatogeries.SelectedValue), Convert.ToInt32(ddlCategory.SelectedValue), ((User)BalUtility.GetSession(Shared.Sessions.SuperAdminLogin)).UserId);

            }
            lblError.Text = "New Category Added";

        }


    }
}