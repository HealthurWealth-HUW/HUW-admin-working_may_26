using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;
using BAL;
using DAL;
using Utility;

public partial class Admin_TopSellingProducts : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //onehide.Visible = false;
        //twohide.Visible = false;
        //threehide.Visible = false;
        //fourhide.Visible = false;
        //ctl00_ctl00_Main_Main_SelectProductUserControl_lisupplier.Visible = false;
        if (!IsPostBack)
        {
            ClientScript.RegisterHiddenField("isPostBack", "1");
            BindSuperCategories();
            BindProductBrands();
        }
    }
    private void BindProductBrands()
    {
        var Repository = new ProductRepository();
        var brands = Repository.GetBrandNames();
        ddlBrand.DataSource = brands;
        ddlBrand.DataTextField = "Brand";
        ddlBrand.DataValueField = "Brand";
        ddlBrand.DataBind();
        ddlBrand.Items.Insert(0, new ListItem("Select", "0"));
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
        ddlSuperCatogeries.Items.Insert(0, new ListItem("Select", "0"));
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
            ddlCategory.Items.Insert(0, new ListItem("Select", "0"));
            BindSubCategories();
        }
    }
    private void BindSubCategories()
    {
        ddlSubCategory.Items.Clear();
        if (ddlCategory.Items.Count > 0)
        {
            ddlSubCategory.DataTextField = "SubCategoryName";
            ddlSubCategory.DataValueField = "SubCategoryId";
            var repository = new SubCategoryRepository();
            int totalRecords;
            int categoryId = Convert.ToInt32(ddlCategory.SelectedItem.Value);
            var data = repository.FetchAllByPage(p => p.CategoryId, out totalRecords, 0, 0, (p => p.IsActive && p.IsDeleted == false && p.CategoryId == categoryId), c => new { c.CategoryId, c.CategoryName, c.IsActive });
            ddlSubCategory.DataSource = data;
            ddlSubCategory.DataBind();
            ddlSubCategory.Items.Insert(0, new ListItem("Select", "0"));
        }
    }
    protected void ddlSuperCatogeries_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindCategories();
    }
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindSubCategories();
    }
}