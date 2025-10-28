using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using Utility;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Collections;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Data.SqlClient;
using System.Configuration;
using DAL;

public partial class Admin_UpdateProducts : System.Web.UI.Page
{
    public long supercategoryId, CategoryId, SubCategoryId;
    public string supercategoryName;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request.QueryString["ID"] == null)
            Response.Redirect("EditProduct.aspx");
        if (!IsPostBack)
        {
            Getaccess();
            string PreviousURL = "";
            if (Request.UrlReferrer == null)
            {

            }
            else
            {
                PreviousURL = Request.UrlReferrer.AbsolutePath.ToString();
            }
            if (PreviousURL == "/Admin/ActivateProducts.aspx")
            {
                btnUpdateProduct.Visible = false;
                btndelete.Visible = false;
                var data = BalUtility.GetDeletedProductInfo(Convert.ToInt32(Request.QueryString["ID"]));
                if (data == null)
                {
                    UpdateIdDiv.Visible = false;
                    UpdateInvalidIdmsgDiv.Visible = true;
                    BindSuperCategories();
                }
                else
                {
                    BindProductFeaturesCategories();
                    UpdateIdDiv.Visible = true;
                    UpdateInvalidIdmsgDiv.Visible = false;
                    List<RelatedProduct> relatedproducttItems = new List<RelatedProduct>();
                    foreach (var item in data.RelatedProducts)
                    {
                        relatedproducttItems.Add(item);
                    }
                    BalUtility.CreateSession(relatedproducttItems, Shared.Sessions.RelatedProductList);

                    ddlSuperCatogeries.Items.Clear();
                    ddlSuperCatogeries.DataTextField = "SuperCategoryName";
                    ddlSuperCatogeries.DataValueField = "SuperCategoryId";
                    var repository = new SuperCategoryRepository();
                    int totalRecords;
                    var dataSuprcatgry = repository.FetchAllByPage(p => p.SuperCategoryId, out totalRecords, 0, 0, (p => p.IsActive && p.IsDeleted == false), c => new { c.SuperCategoryId, c.SuperCategoryName, c.IsActive });
                    ddlSuperCatogeries.DataSource = dataSuprcatgry;
                    ddlSuperCatogeries.DataBind();
                    //ddlSuperCatogeries.SelectedIndex = ddlSuperCatogeries.Items.IndexOf(ddlSuperCatogeries.Items.FindByText(data.SubCategory.Category.SuperCategory.SuperCategoryName));
                    ddlSuperCatogeries.SelectedValue = ddlSuperCatogeries.Items.FindByValue(data.SubCategory.Category.SuperCategory.SuperCategoryId.ToString()).Value;
                    ddlCategory.Items.Clear();
                    if (ddlSuperCatogeries.Items.Count > 0)
                    {
                        ddlCategory.DataTextField = "CategoryName";
                        ddlCategory.DataValueField = "CategoryId";
                        var repository1 = new CategoryRepository();
                        int suprcategoryId = Convert.ToInt32(ddlSuperCatogeries.SelectedItem.Value);
                        var datacatgry = repository1.FetchAllByPage(p => p.SuperCategoryId, out totalRecords, 0, 0, (p => p.SuperCategoryId == suprcategoryId && p.IsActive && p.IsDeleted == false), c => new { c.CategoryId, c.CategoryName, c.IsActive });
                        ddlCategory.DataSource = datacatgry;
                        ddlCategory.DataBind();
                        //ddlCategory.SelectedIndex = ddlCategory.Items.IndexOf(ddlCategory.Items.FindByText(data.SubCategory.Category.CategoryName));
                        ddlCategory.SelectedValue = ddlCategory.Items.FindByValue(data.SubCategory.Category.CategoryId.ToString()).Value;
                    }
                    if (ddlCategory.Items.Count > 0)
                    {
                        ddlSubCategory.DataTextField = "SubCategoryName";
                        ddlSubCategory.DataValueField = "SubCategoryId";
                        var repository2 = new SubCategoryRepository();
                        int categoryId = Convert.ToInt32(data.SubCategory.Category.CategoryId);
                        var datasubcatgry = repository2.FetchAllByPage(p => p.CategoryId, out totalRecords, 0, 0, (p => p.CategoryId == categoryId && p.IsActive && p.IsDeleted == false), c => new { c.SubCategoryId, c.SubCategoryName, c.IsActive });
                        ddlSubCategory.DataSource = datasubcatgry;
                        ddlSubCategory.DataBind();
                        //ddlSubCategory.SelectedItem.Text = data.SubCategory.SubCategoryName;
                        ddlSubCategory.SelectedValue = data.SubCategory.SubCategoryId.ToString();

                    }
                    Txtweight.Text = data.Weight.ToString();
                    txtProductName.Text = data.ProductName;
                    HSNCode.Text = data.HSNCode;
                    txtProductBrand.Text = data.Brand;
                    txtProductDescription.Text = data.ProductDescription;
                    elm1.InnerHtml = System.Web.HttpUtility.HtmlDecode(data.ShortDescription);
                    chkIsFeatureProduct.Checked = data.IsFeaturedProduct;
                    chkBoxCompare.Checked = data.CanCompare;
                    chkBoxHasReviews.Checked = data.HasReviews;
                    chkBoxAdminPerissionsOnReviews.Checked = data.CanReviewsNeedPermission;
                    txtProductCost.Text = Convert.ToString(data.ProductOriginalCost);
                    txtProductDiscount.Text = Convert.ToString(data.ProductDiscountPercentage);
                    txtProductCostAfterDiscount.Text = Convert.ToString(data.ProductCost);
                    txtProductQuantity.Text = Convert.ToString(data.Quantity);
                    txtMetaDescription.Text = data.Meta_Description;
                    txtMetaKeywords.Text = data.Meta_Keywords;
                    txtPageTitle.Text = data.Page_Title;
                    int FreeProductId = Convert.ToInt32(data.Free_Product_ID);

                    if (FreeProductId == 0 || data.Is_Free_Product_Active == false)
                    {

                    }
                    else
                    {
                        List<Product> freeproducttItems = new List<Product>();
                        Product FreeProductData = BalUtility.GetProductInfo(FreeProductId);
                        freeproducttItems.Add(FreeProductData);
                        BalUtility.CreateSession(freeproducttItems, Shared.Sessions.FreeProductList);
                        //lblFreeProductID.Text = FreeProductId.ToString();
                        //lblFreeProductName.Text = FreeProductData.ProductName;
                    }

                    ha.InnerHtml = System.Web.HttpUtility.HtmlDecode(data.ProductFeaturesContent);
                    Img1.Src = data.ProductImgUrl;
                    if (data.SubProducts.Count != 0)
                    {
                        GVSubProducts.DataSource = data.SubProducts;
                        GVSubProducts.DataBind();

                        btnCheck.Checked = true;
                        lidiscount.Visible = true;
                        liMRP.Visible = true;
                        lisubproducts.Visible = true;
                    }
                    //GVProductSpecifications.DataSource = data.ProductSpecifications;
                    //GVProductSpecifications.DataBind();

                    //foreach (GridViewRow row in GVProductSpecifications.Rows)
                    //{
                    //    var dropdown = row.FindControl("ddlProductSpecificationTypes") as DropDownList;
                    //    if (dropdown != null)
                    //    {
                    //        //   dropdown.SelectedItem.Value = productSpecification[row.RowIndex].SpecificationTypeId.ToString();
                    //        dropdown.SelectedIndex = dropdown.Items.IndexOf(dropdown.Items.FindByValue(data.ProductSpecifications.ToList()[row.RowIndex].SpecificationTypeId.ToString()));

                    //    }
                    //}
                    if (data.ProductFeatures.Count != 0)
                        ManageProductFeatures(Shared.Actions.Read);
                    //if (data.ProductSpecifications.Count != 0)
                    //    ManageProductSpecifications(Shared.Actions.Read);
                    // if (data.SubProducts.Count != 0)            
                    ManageSubProducts(Shared.Actions.Read);

                }
            }
            else
            {

                //btnActivateProduct.Visible = false;
                Button1.Visible = false;
                var data = BalUtility.GetProductInfo(Convert.ToInt32(Request.QueryString["ID"]));
                var adddata = BalUtility.GetProductAddInfo(Convert.ToInt32(Request.QueryString["ID"]));

                //txtGST.Text = data.GST.ToString();

                if (data == null)
                {
                    UpdateIdDiv.Visible = false;
                    UpdateInvalidIdmsgDiv.Visible = true;
                    BindSuperCategories();
                }
                else
                {
                    DDLgst.Items.Clear();
                    DDLgst.DataTextField = "GSTPer";
                    DDLgst.DataValueField = "GSTPer";
                    var gstrep = new GSTRepository();
                    int totalRecords;
                    var dataddl = gstrep.FetchAllByPage(p => p.GstID, out totalRecords, 0, 0, null, c => new { c.GstID, c.GSTPer });

                    DDLgst.DataSource = dataddl;
                    DDLgst.DataBind();
                    DDLgst.SelectedValue = data.GST.ToString();
                    BindProductFeaturesCategories();
                    UpdateIdDiv.Visible = true;
                    UpdateInvalidIdmsgDiv.Visible = false;
                    List<RelatedProduct> relatedproducttItems = new List<RelatedProduct>();
                    foreach (var item in data.RelatedProducts)
                    {
                        relatedproducttItems.Add(item);
                    }
                    BalUtility.CreateSession(relatedproducttItems, Shared.Sessions.RelatedProductList);

                    ddlSuperCatogeries.Items.Clear();
                    ddlSuperCatogeries.DataTextField = "SuperCategoryName";
                    ddlSuperCatogeries.DataValueField = "SuperCategoryId";
                    var repository = new SuperCategoryRepository();
                    var dataSuprcatgry = repository.FetchAllByPage(p => p.SuperCategoryId, out totalRecords, 0, 0, (p => p.IsActive && p.IsDeleted == false), c => new { c.SuperCategoryId, c.SuperCategoryName, c.IsActive });
                    ddlSuperCatogeries.DataSource = dataSuprcatgry;
                    ddlSuperCatogeries.DataBind();
                    int id = int.Parse(Request.QueryString["id"]);
                    MasterController m = new MasterController();
                    Image1.ImageUrl = m.getimage(id);

                    //ddlSuperCatogeries.SelectedIndex = ddlSuperCatogeries.Items.IndexOf(ddlSuperCatogeries.Items.FindByText(data.SubCategory.Category.SuperCategory.SuperCategoryName));
                    ddlSuperCatogeries.SelectedValue = ddlSuperCatogeries.Items.FindByValue(data.SubCategory.Category.SuperCategory.SuperCategoryId.ToString()).Value;
                    ddlCategory.Items.Clear();
                    if (ddlSuperCatogeries.Items.Count > 0)
                    {
                        ddlCategory.DataTextField = "CategoryName";
                        ddlCategory.DataValueField = "CategoryId";
                        var repository1 = new CategoryRepository();
                        int suprcategoryId = Convert.ToInt32(ddlSuperCatogeries.SelectedItem.Value);
                        var datacatgry = repository1.FetchAllByPage(p => p.SuperCategoryId, out totalRecords, 0, 0, (p => p.SuperCategoryId == suprcategoryId && p.IsActive && p.IsDeleted == false), c => new { c.CategoryId, c.CategoryName, c.IsActive });
                        ddlCategory.DataSource = datacatgry;
                        ddlCategory.DataBind();
                        //ddlCategory.SelectedIndex = ddlCategory.Items.IndexOf(ddlCategory.Items.FindByText(data.SubCategory.Category.CategoryName));
                        ddlCategory.SelectedValue = ddlCategory.Items.FindByValue(data.SubCategory.Category.CategoryId.ToString()).Value;
                    }
                    if (ddlCategory.Items.Count > 0)
                    {
                        ddlSubCategory.DataTextField = "SubCategoryName";
                        ddlSubCategory.DataValueField = "SubCategoryId";
                        var repository2 = new SubCategoryRepository();
                        int categoryId = Convert.ToInt32(data.SubCategory.Category.CategoryId);
                        var datasubcatgry = repository2.FetchAllByPage(p => p.CategoryId, out totalRecords, 0, 0, (p => p.CategoryId == categoryId && p.IsActive && p.IsDeleted == false), c => new { c.SubCategoryId, c.SubCategoryName, c.IsActive });
                        ddlSubCategory.DataSource = datasubcatgry;
                        ddlSubCategory.DataBind();
                        //ddlSubCategory.SelectedItem.Text = data.SubCategory.SubCategoryName;
                        ddlSubCategory.SelectedValue = data.SubCategory.SubCategoryId.ToString();

                    }
                    using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
                    {
                        List<CBC_Data_Tree> checks = context.CBC_Data_Tree.Where(x => x.Prod_ID == data.ProductId && x.Is_Active == true).ToList();
                        foreach (ListItem item in chkaccess.Items)
                        {

                            foreach (var ch in checks)
                            {
                                string defaultid = item.Value;
                                string couponsid = "";
                                string[] idss = defaultid.Split(',');

                                couponsid = idss[0];
                                int itemss = Int32.Parse(couponsid);
                                if (ch.Coup_ID == itemss)
                                {
                                    item.Selected = true;
                                }
                            }
                        }
                    }
                    if (adddata != null)
                    {
                        Best_Before_Date.Text = adddata.Best_Before_Date.ToString();
                        Country_Of_Origin.Text = adddata.Country_Of_Origin.ToString();
                        GTIN.Text = adddata.GTIN.ToString();
                        Manufacturer.Text = adddata.Manufacturer_Details.ToString();
                        txtBatch.Text = adddata.BatchNo;
                        Marketerdetails.Text = adddata.Marketerdetails.ToString();
                        if (adddata.Manufacturer_Date != null)
                        {
                            Manufacturer_Date.Text = adddata.Manufacturer_Date.Value.ToString("yyyy-MM-dd");
                        }
                    }

                    Txtweight.Text = data.Weight.ToString();
                    txtProductName.Text = data.ProductName;
                    HSNCode.Text = data.HSNCode;
                    txtProductBrand.Text = data.Brand;
                    txtProductDescription.Text = data.ProductDescription;
                    elm1.InnerHtml = System.Web.HttpUtility.HtmlDecode(data.ShortDescription);
                    chkIsFeatureProduct.Checked = data.IsFeaturedProduct;
                    chkBoxCompare.Checked = data.CanCompare;
                    chkBoxHasReviews.Checked = data.HasReviews;
                    chkBoxAdminPerissionsOnReviews.Checked = data.CanReviewsNeedPermission;
                    CheckBox1.Checked = (bool)data.IsPresciption;
                    txtProductCost.Text = Convert.ToString(data.ProductOriginalCost);
                    txtProductDiscount.Text = Convert.ToString(data.ProductDiscountPercentage);
                    txtProductCostAfterDiscount.Text = Convert.ToString(data.ProductCost);
                    txtProductQuantity.Text = Convert.ToString(data.Quantity);
                    txtMetaDescription.Text = data.Meta_Description;
                    txtMetaKeywords.Text = data.Meta_Keywords;
                    txtPageTitle.Text = data.Page_Title;
                    tcth2tag.Text = data.H2Tag;
                    int FreeProductId = Convert.ToInt32(data.Free_Product_ID);

                    if (FreeProductId == 0 || data.Is_Free_Product_Active == false)
                    {

                    }
                    else
                    {
                        List<Product> freeproducttItems = new List<Product>();
                        Product FreeProductData = BalUtility.GetProductInfo(FreeProductId);
                        freeproducttItems.Add(FreeProductData);
                        BalUtility.CreateSession(freeproducttItems, Shared.Sessions.FreeProductList);
                        //lblFreeProductID.Text = FreeProductId.ToString();
                        //lblFreeProductName.Text = FreeProductData.ProductName;
                    }

                    ha.InnerHtml = System.Web.HttpUtility.HtmlDecode(data.ProductFeaturesContent);
                    Img1.Src = data.ProductImgUrl;
                    //Img2.Src = data.ProductImgUrl2;
                    if (data.SubProducts.Count != 0)
                    {
                        GVSubProducts.DataSource = data.SubProducts;
                        GVSubProducts.DataBind();

                        btnCheck.Checked = true;
                        lidiscount.Visible = true;
                        liMRP.Visible = true;
                        lisubproducts.Visible = true;
                    }
                    //GVProductSpecifications.DataSource = data.ProductSpecifications;
                    //GVProductSpecifications.DataBind();

                    //foreach (GridViewRow row in GVProductSpecifications.Rows)
                    //{
                    //    var dropdown = row.FindControl("ddlProductSpecificationTypes") as DropDownList;
                    //    if (dropdown != null)
                    //    {
                    //        //   dropdown.SelectedItem.Value = productSpecification[row.RowIndex].SpecificationTypeId.ToString();
                    //        dropdown.SelectedIndex = dropdown.Items.IndexOf(dropdown.Items.FindByValue(data.ProductSpecifications.ToList()[row.RowIndex].SpecificationTypeId.ToString()));

                    //    }
                    //}
                    if (data.ProductFeatures.Count != 0)
                        ManageProductFeatures(Shared.Actions.Read);
                    //if (data.ProductSpecifications.Count != 0)
                    //    ManageProductSpecifications(Shared.Actions.Read);
                    // if (data.SubProducts.Count != 0)            
                    ManageSubProducts(Shared.Actions.Read);

                }
            }
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
        ddlSuperCatogeries.SelectedItem.Text = supercategoryName;
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
            int suprcategoryId = Convert.ToInt32(ddlSuperCatogeries.SelectedItem.Value);
            var data = repository.FetchAllByPage(p => p.SuperCategoryId, out totalRecords, 0, 0, (p => p.SuperCategoryId == suprcategoryId && p.IsActive && p.IsDeleted == false), c => new { c.CategoryId, c.CategoryName, c.IsActive });
            ddlCategory.DataSource = data;
            ddlCategory.DataBind();
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
            var data = repository.FetchAllByPage(p => p.CategoryId, out totalRecords, 0, 0, (p => p.CategoryId == categoryId && p.IsActive && p.IsDeleted == false), c => new { c.SubCategoryId, c.SubCategoryName, c.IsActive });
            ddlSubCategory.DataSource = data;
            ddlSubCategory.DataBind();
        }
    }
    private void BindProductFeaturesCategories()
    {
        //var repository = new FeaturesSubCategoryRepository();
        //int totalRecords;
        //int categoryId = Convert.ToInt32(ddlCategory.SelectedItem.Value);
        ////     var data = repository.FetchAllByPage(p => p.CategoryId, out  totalRecords, 0, 0, (p => p.IsActive && p.IsDeleted == false && p.CategoryId == categoryId), c => new { c.CategoryId, c.CategoryName, c.IsActive });

        //var data = repository.FetchAllByPage(p => p.FeaturesSubCategoryId, out totalRecords, 0, 0,
        //                                   null, null, "");
        //foreach (var subCategory in data)
        //    subCategory.CategoryName = subCategory.Category.CategoryName;



        //ddlSubCategory.DataSource = data;
        //ddlSubCategory.DataBind();
    }

    protected void btnRemoveProductFeatures_Click(object sender, EventArgs e)
    {
        var row = ((Button)sender).Parent.Parent as GridViewRow;
        if (row != null) ManageProductFeatures(Shared.Actions.Delete, row.RowIndex);
    }

    protected void btnAddProductFeatures_Click(object sender, EventArgs e)
    {
        ManageProductFeatures(Shared.Actions.Create);
    }

    private void ManageProductFeatures(Shared.Actions action, int rowId = -1)
    {
        //List<ProductFeature> productFeature = (from GridViewRow row in GVProductFeatures.Rows
        //                                       select new ProductFeature
        //                                       {
        //                                           FeatureInfo = ((TextBox)row.FindControl("txtProductFeature")).Text
        //                                       }).ToList();
        //switch (action)
        //{
        //    case Shared.Actions.Create:
        //        productFeature.Add(new ProductFeature());
        //        break;
        //    case Shared.Actions.Delete:
        //        if (rowId != -1) productFeature.RemoveAt(rowId);
        //        break;
        //}
        //GVProductFeatures.DataSource = productFeature;
        //GVProductFeatures.DataBind();
    }

    //protected void btnAddProductGallery_Click(object sender, EventArgs e)
    //{
    //    ManageProductGallery(Shared.Actions.Create);
    //}
    //protected void btnDeleteProductGallery_Click(object sender, EventArgs e)
    //{
    //    var row = ((Button)sender).Parent.Parent as GridViewRow;
    //    if (row != null) ManageProductGallery(Shared.Actions.Delete, row.RowIndex);
    //}

    //private void ManageProductGallery(Shared.Actions action, int rowId = -1)
    //{
    //    //List<ProductsGallery> productGallery = (from GridViewRow row in GVProductGallery.Rows
    //    //                                        select new ProductsGallery
    //    //                                       {
    //    //                                           ImgUrl = FileProductGallery.FileName
    //    //                                       }).ToList();
    //    //switch (action)
    //    //{
    //    //    case Shared.Actions.Create:
    //    //        productGallery.Add(new ProductsGallery());
    //    //        break;
    //    //    case Shared.Actions.Delete:
    //    //        if (rowId != -1) productGallery.RemoveAt(rowId);
    //    //        break;
    //    //}
    //    //GVProductGallery.DataSource = productGallery;
    //    //GVProductGallery.DataBind();
    //}

    //protected void btnDeleteProductSpecification_Click(object sender, EventArgs e)
    //{
    //    var row = ((Button)sender).Parent.Parent as GridViewRow;
    //    if (row != null) ManageProductSpecifications(Shared.Actions.Delete, row.RowIndex);

    //}
    //protected void btnAddProductSpecification_Click(object sender, EventArgs e)
    //{
    //    ManageProductSpecifications(Shared.Actions.Create);
    //}

    //private void ManageProductSpecifications(Shared.Actions action, int rowId = -1)
    //{
    //    var productSpecification = (from GridViewRow row in GVProductSpecifications.Rows
    //                                select new ProductSpecification
    //                                {
    //                                    ProductSpecificationId = Convert.ToInt64(((HiddenField)row.FindControl("hiddnProductSpecificationId")).Value),
    //                                    ProductSpecificationName = ((TextBox)row.FindControl("txtProductSpecificationName")).Text,
    //                                    ProductSpecificationNameValues = ((TextBox)row.FindControl("txtProductSpecificationValue")).Text,
    //                                    SpecificationTypeId = Convert.ToInt32(((DropDownList)row.FindControl("ddlProductSpecificationTypes")).SelectedItem.Value)
    //                                }).ToList();
    //    switch (action)
    //    {
    //        case Shared.Actions.Create:
    //            productSpecification.Add(new ProductSpecification());
    //            break;
    //        case Shared.Actions.Delete:
    //            HiddenField ProductSpecificationId = (HiddenField)(GVSubProducts.Rows[rowId].FindControl("hiddnProductSpecificationId"));
    //            long id = Convert.ToInt64(ProductSpecificationId.Value);
    //            var repository = new ProductSpecificationRepository();
    //            var deletePrdctSpecfcn = repository.First(sp => sp.ProductSpecificationId == id);
    //            if (deletePrdctSpecfcn != null)
    //                repository.Delete(deletePrdctSpecfcn);
    //            if (rowId != -1) productSpecification.RemoveAt(rowId);
    //            break;
    //    }
    //    GVProductSpecifications.DataSource = productSpecification;
    //    GVProductSpecifications.DataBind();

    //    foreach (GridViewRow row in GVProductSpecifications.Rows)
    //    {
    //        var dropdown = row.FindControl("ddlProductSpecificationTypes") as DropDownList;
    //        if (dropdown != null)
    //        {
    //            //   dropdown.SelectedItem.Value = productSpecification[row.RowIndex].SpecificationTypeId.ToString();
    //            dropdown.SelectedIndex = dropdown.Items.IndexOf(dropdown.Items.FindByValue(productSpecification[row.RowIndex].SpecificationTypeId.ToString()));

    //        }
    //    }
    //}

    //protected void GVProductSpecifications_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        var dropdown = e.Row.FindControl("ddlProductSpecificationTypes") as DropDownList;
    //        if (dropdown != null)
    //        {
    //            dropdown.DataSource = Enumeration.GetAll<Shared.SpecificationTypes>();
    //            dropdown.DataTextField = "Value";
    //            dropdown.DataValueField = "Key";
    //            dropdown.DataBind();

    //        }
    //    }
    //}

    //Sub Products

    protected void btnDeleteSubProducts_Click(object sender, EventArgs e)
    {
        var row = ((Button)sender).Parent.Parent as GridViewRow;
        if (row != null) ManageSubProducts(Shared.Actions.Delete, row.RowIndex);
        long Quantity = Convert.ToInt32(((TextBox)row.FindControl("txtSubProductQuantity")).Text);
        //txtProductQuantity.Text = Convert.ToString((Convert.ToInt32(txtProductQuantity.Text)) - Quantity);
        txtProductQuantity.Enabled = GVSubProducts.Rows.Count == 0 ? true : false;
    }

    protected void btnAddSubProducts_Click(object sender, EventArgs e)
    {
        ManageSubProducts(Shared.Actions.Create);
    }
    private void ManageSubProducts(Shared.Actions action, int rowId = -1)
    {
        var subProducts = (from GridViewRow row in GVSubProducts.Rows

                           select new SubProduct
                           {
                               SubProductId = Convert.ToInt64(((HiddenField)row.FindControl("hdnSubProductId")).Value),
                               SPName = (((TextBox)row.FindControl("txtSPName")).Text),

                               Quantity = Convert.ToInt32(((TextBox)row.FindControl("txtSubProductQuantity")).Text),
                               ProductOriginalCost = Convert.ToDecimal(((TextBox)row.FindControl("txtSubProductOriginalCost")).Text),

                               ProductDiscountPercentage = Convert.ToDecimal(((TextBox)row.FindControl("txtSubProductDiscountPercentage")).Text),

                               ProductDiscountCost = ((Convert.ToDecimal(((TextBox)row.FindControl("txtSubProductOriginalCost")).Text)) * (Convert.ToDecimal(((TextBox)row.FindControl("txtSubProductDiscountPercentage")).Text)) / 100),
                               ProductCost = ((Convert.ToDecimal(((TextBox)row.FindControl("txtSubProductOriginalCost")).Text)) - ((Convert.ToDecimal(((TextBox)row.FindControl("txtSubProductOriginalCost")).Text)) * (Convert.ToDecimal(((TextBox)row.FindControl("txtSubProductDiscountPercentage")).Text)) / 100))
                               // ProductCost = Convert.ToDecimal(((TextBox)row.FindControl("txtSubProductProductCost")).Text)                             

                           }).ToList();
        switch (action)
        {

            case Shared.Actions.Create:
                subProducts.Add(new SubProduct());
                break;
            case Shared.Actions.Delete:
                HiddenField subprctId = (HiddenField)(GVSubProducts.Rows[rowId].FindControl("hdnSubProductId"));
                long id = Convert.ToInt64(subprctId.Value);
                var repository = new SubProductRepository();
                var deletesubprdct = repository.First(sp => sp.SubProductId == id);
                if (deletesubprdct != null)
                    repository.Delete(deletesubprdct);
                if (rowId != -1) subProducts.RemoveAt(rowId);
                break;
        }
        GVSubProducts.DataSource = subProducts;
        GVSubProducts.DataBind();
        if (GVSubProducts.Rows.Count != 0)
        {
            txtProductQuantity.Text = "";
            foreach (var item in subProducts)
            {
                int subPrdctQty = item.Quantity;
                if (txtProductQuantity.Text == "")
                    txtProductQuantity.Text = "0";
                int TotalQty = (Convert.ToInt32(txtProductQuantity.Text) + subPrdctQty);
                txtProductQuantity.Text = Convert.ToString(TotalQty);
            }
        }
        txtProductQuantity.Enabled = GVSubProducts.Rows.Count == 0 ? true : false;
    }

    protected void GVSubProducts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    var dropdown = e.Row.FindControl("ddlProductSpecificationTypes") as DropDownList;
        //    if (dropdown != null)
        //    {
        //        dropdown.DataSource = Enumeration.GetAll<Shared.SpecificationTypes>();
        //        dropdown.DataTextField = "Value";
        //        dropdown.DataValueField = "Key";
        //        dropdown.DataBind();

        //    }
        //}
    }

    //End Sub Products

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindSubCategories();
    }
    //public void txtProductDiscount_TextChanged(object sender, EventArgs e)
    //{    

    //    int prodcost = Convert.ToInt32(txtProductCost.Text);
    //    int discount = Convert.ToInt32(txtProductDiscount.Text);
    //    int percent = prodcost - (prodcost * discount / 100);


    //}
    protected void btnUpdateProduct_Click(object sender, EventArgs e)
    {
        try
        {
            CBC_Data_Tree def = new CBC_Data_Tree();
            lblError.Text = "";
            int FileSize = FileUpload2.PostedFile.ContentLength;
            if (FileSize != 0)
            {
                Bitmap img = new Bitmap(FileUpload2.PostedFile.InputStream, false);

                int height = img.Height;
                int width = img.Width;
                int fileSize = (FileUpload2.PostedFile.ContentLength) / 1024;
                if (height < 500 && width < 300)
                {
                    lblError.Text = "Image size Should not less than 500 KB,500x300 px";
                    return;
                }
            }
            var repository = new ProductRepository();
            int totalRecords;
            long PID = Convert.ToInt32(Request.QueryString["ID"]);
            var y = BalUtility.Logmaintainance((int)Shared.OrderStatus.Cashbackassigned, "Cashbackassigned", "", PID.ToString());
            Product prodct = new Product();
            if (Txtweight.Text != "")
            {
                prodct.Weight = Convert.ToInt32(Txtweight.Text);
            }
            prodct.ProductId = PID;
            prodct.H2Tag = tcth2tag.Text;

            prodct.IsFeaturedProduct = chkIsFeatureProduct.Checked;
            prodct.CanCompare = chkBoxCompare.Checked;
            prodct.HasReviews = chkBoxHasReviews.Checked;
            prodct.CanReviewsNeedPermission = chkBoxAdminPerissionsOnReviews.Checked;
            prodct.IsPresciption = CheckBox1.Checked;
            prodct.Occasion = "";
            prodct.ProductCode = "";
            prodct.ProductColor = "";
            prodct.ProductDescription = txtProductDescription.Text;
            prodct.ShortDescription = elm1.InnerHtml;
            prodct.Meta_Keywords = txtMetaKeywords.Text;
            prodct.Meta_Description = txtMetaDescription.Text;
            prodct.Page_Title = txtPageTitle.Text;
            prodct.GST = Convert.ToDecimal(DDLgst.SelectedItem.Value);
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {
                var repositorys = new CBCRepository();
                def.Prod_ID = PID;
                def.Is_Active = false;
                repositorys.Updateaccessss(def);
                foreach (ListItem item in chkaccess.Items)
                {
                    if (item.Selected != false)
                    {
                        def.Prod_ID = PID; //should be done when checkbox is done when page page load

                        string defaultid = item.Value;
                        string couponsid = "";
                        string[] idss = defaultid.Split(',');
                        defaultid = idss[1];
                        couponsid = idss[0];
                        def.Coup_ID = Convert.ToInt32(couponsid);
                        def.Defaultid = Convert.ToInt32(defaultid);
                        def.Created_on = DateTime.Now;
                        def.Is_Active = true;
                        context.CBC_Data_Tree.Add(def);
                        context.SaveChanges();
                    }
                }
            }

            decimal ProductCost = 0;
            if (txtProductCost.Text != "")
                try
                {
                    ProductCost = Convert.ToDecimal(txtProductCost.Text);
                }
                catch { }
            decimal ProductDiscount = 0;
            if (txtProductDiscount.Text != "")
                try
                {
                    ProductDiscount = Convert.ToDecimal(txtProductDiscount.Text);
                }
                catch
                {

                }
            decimal FinalAount = ProductCost - (ProductCost * ProductDiscount / 100);

            prodct.ProductOriginalCost = ProductCost;
            prodct.ProductCost = FinalAount;
            prodct.ProductDiscountPercentage = ProductDiscount;
            prodct.ProductDiscountCost = (ProductCost - FinalAount);

            prodct.Quantity = Convert.ToInt32(txtProductQuantity.Text);
            if (prodct.Quantity >= 0)
            {
                db_Zon_HuwEntities context = new db_Zon_HuwEntities();
                Tbl_Outofstock data = context.Tbl_Outofstock.Where(x => x.Productid == prodct.ProductId).FirstOrDefault();
                if (data != null)
                {
                    context.Tbl_Outofstock.Remove(data);
                    context.SaveChanges();
                }
            }
            //prodct.ProductImgUrl = "";
            prodct.ProductName = txtProductName.Text;
            prodct.HSNCode = HSNCode.Text;
            prodct.Brand = txtProductBrand.Text;
            prodct.ProductFeaturesContent = ha.InnerHtml;
            prodct.SubCategoryId = Convert.ToInt32(ddlSubCategory.SelectedItem.Value);

            List<Product> freeProductItems = (List<Product>)BalUtility.GetSession(Shared.Sessions.FreeProductList) ?? new List<Product>();

            foreach (Product p in freeProductItems)
            {
                prodct.Free_Product_ID = p.ProductId;
                prodct.Is_Free_Product_Active = true;
            }

            List<SubProduct> SubProductList = new List<SubProduct>();

            SubProductList = (from GridViewRow row in GVSubProducts.Rows
                              select new SubProduct
                              {
                                  SubProductId = Convert.ToInt64(((HiddenField)row.FindControl("hdnSubProductId")).Value),//SubPrdctID,hdnSubProductId
                                  ProductId = PID,
                                  SPName = (((TextBox)row.FindControl("txtSPName")).Text),
                                  Quantity = Convert.ToInt32(((TextBox)row.FindControl("txtSubProductQuantity")).Text),
                                  ProductOriginalCost = Convert.ToDecimal(((TextBox)row.FindControl("txtSubProductOriginalCost")).Text),
                                  ProductDiscountPercentage = Convert.ToDecimal(((TextBox)row.FindControl("txtSubProductDiscountPercentage")).Text),
                                  ProductDiscountCost = ((Convert.ToDecimal(((TextBox)row.FindControl("txtSubProductOriginalCost")).Text)) * (Convert.ToDecimal(((TextBox)row.FindControl("txtSubProductDiscountPercentage")).Text)) / 100),
                                  ProductCost = ((Convert.ToDecimal(((TextBox)row.FindControl("txtSubProductOriginalCost")).Text)) - ((Convert.ToDecimal(((TextBox)row.FindControl("txtSubProductOriginalCost")).Text)) * (Convert.ToDecimal(((TextBox)row.FindControl("txtSubProductDiscountPercentage")).Text)) / 100))
                              }).ToList();

            prodct.SubProducts = SubProductList;
            if (ProductImage.HasFile)
            {
                prodct.ProductImgs = ProductImage.PostedFile;
                //Live Code
                //prodct.ProductImgUrl = prodct.ProductImgs.Upload(Shared.ProductImageTypes.ProductImg);
                //Test Code
                string ImgPath = prodct.ProductImgs.Upload(Shared.ProductImageTypes.ProductImg);
                string[] Img = ImgPath.Split('\\');
                prodct.ProductImgUrl = "https://www.healthurwealth.com/UploadFiles/" + Img[5];
            }
            if (FileUpload3.HasFile)
            {
                prodct.ProductImgss = FileUpload3.PostedFile;
                //Live Code
                //prodct.ProductImgUrl = prodct.ProductImgs.Upload(Shared.ProductImageTypes.ProductImg);
                //Test Code
                string ImgPaths = prodct.ProductImgss.Upload(Shared.ProductImageTypes.ProductImg);
                string[] Imgs = ImgPaths.Split('\\');
                prodct.ProductImgUrl2 = "https://www.healthurwealth.com/UploadFiles/" + Imgs[5];
            }
            if (FileUpload1.HasFile)
            {
                UploadGallery(prodct.ProductId.ToString());
                //  txtGalleryPath
            }
            var product = new Product();
            // product.ProductImgs = ProductImage.PostedFile;
            product.SizeGuidePath = @"C:\inetpub\wwwroot\HUW_live\UploadFiles/Size_Chart/" + FileUpload2.PostedFile.FileName;

            if (prodct.ProductId != 0)
            {

                UploadSize_Chart(prodct.ProductId);
                //UploadGallery(product.ProductId.ToString());
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alert", "Product has been saved Product Id :" + prodct.ProductId + "", true);
            }
            //updating related products 
            var RelatedProductItems = (List<RelatedProduct>)BalUtility.GetSession(Shared.Sessions.RelatedProductList) ??
                         new List<RelatedProduct>();
            RelatedProductItems.ForEach(p => p.ProductId = PID);

            prodct.RelatedProducts = RelatedProductItems;
            Tbl_AdditionalInfo additionalInfo = new Tbl_AdditionalInfo();
            additionalInfo.ProductId = PID;
            additionalInfo.Marketerdetails = Marketerdetails.Text;
            additionalInfo.Manufacturer_Date = Convert.ToDateTime(Manufacturer_Date.Text);
            additionalInfo.Best_Before_Date = Convert.ToInt32(Best_Before_Date.Text);
            additionalInfo.Country_Of_Origin = Country_Of_Origin.Text;
            additionalInfo.GTIN = GTIN.Text;
            additionalInfo.BatchNo = txtBatch.Text;
            additionalInfo.Manufacturer_Details = Manufacturer.Text;

            repository.UpdateAdditionalinfo(additionalInfo);

            repository.UpdateProduct(prodct);
            BalUtility.ClearSession(Shared.Sessions.RelatedProductList);
            Response.Redirect("updateproducts.aspx?id=" + PID);
        }
        catch (Exception ex)
        {
            throw;
        }

    }

    protected void btnActivateProduct_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Text = "";
            int FileSize = FileUpload2.PostedFile.ContentLength;
            if (FileSize != 0)
            {
                Bitmap img = new Bitmap(FileUpload2.PostedFile.InputStream, false);

                int height = img.Height;
                int width = img.Width;
                int fileSize = (FileUpload2.PostedFile.ContentLength) / 1024;
                if (height < 500 && width < 300)
                {
                    lblError.Text = "Image size Should not less than 500 KB,500x500 px";
                    return;
                }
            }
            var repository = new ProductRepository();
            int totalRecords;
            long PID = Convert.ToInt32(Request.QueryString["ID"]);

            Product prodct = new Product();

            prodct.ProductId = PID;
            prodct.IsDeleted = false;
            prodct.IsFeaturedProduct = chkIsFeatureProduct.Checked;
            prodct.CanCompare = chkBoxCompare.Checked;
            prodct.HasReviews = chkBoxHasReviews.Checked;
            prodct.CanReviewsNeedPermission = chkBoxAdminPerissionsOnReviews.Checked;
            prodct.Occasion = "";
            prodct.ProductCode = "";
            prodct.ProductColor = "";
            prodct.ProductDescription = txtProductDescription.Text;
            prodct.ShortDescription = elm1.InnerHtml;

            decimal ProductCost = 0;
            if (txtProductCost.Text != "")
                try
                {
                    ProductCost = Convert.ToDecimal(txtProductCost.Text);
                }
                catch { }
            decimal ProductDiscount = 0;
            if (txtProductDiscount.Text != "")
                try
                {
                    ProductDiscount = Convert.ToDecimal(txtProductDiscount.Text);
                }
                catch { }
            decimal FinalAount = ProductCost - (ProductCost * ProductDiscount / 100);

            prodct.ProductOriginalCost = ProductCost;
            prodct.ProductCost = FinalAount;
            prodct.ProductDiscountPercentage = ProductDiscount;
            prodct.ProductDiscountCost = (ProductCost - FinalAount);

            prodct.Quantity = Convert.ToInt32(txtProductQuantity.Text);
            //prodct.ProductImgUrl = "";
            prodct.ProductName = txtProductName.Text;
            prodct.HSNCode = HSNCode.Text;
            prodct.Brand = txtProductBrand.Text;
            prodct.ProductFeaturesContent = ha.InnerHtml;
            prodct.SubCategoryId = Convert.ToInt32(ddlSubCategory.SelectedItem.Value);


            List<SubProduct> SubProductList = new List<SubProduct>();

            SubProductList = (from GridViewRow row in GVSubProducts.Rows
                              select new SubProduct
                              {
                                  SubProductId = Convert.ToInt64(((HiddenField)row.FindControl("hdnSubProductId")).Value),//SubPrdctID,hdnSubProductId
                                  ProductId = PID,
                                  SPName = (((TextBox)row.FindControl("txtSPName")).Text),
                                  Quantity = Convert.ToInt32(((TextBox)row.FindControl("txtSubProductQuantity")).Text),
                                  ProductOriginalCost = Convert.ToDecimal(((TextBox)row.FindControl("txtSubProductOriginalCost")).Text),
                                  ProductDiscountPercentage = Convert.ToDecimal(((TextBox)row.FindControl("txtSubProductDiscountPercentage")).Text),
                                  ProductDiscountCost = ((Convert.ToDecimal(((TextBox)row.FindControl("txtSubProductOriginalCost")).Text)) * (Convert.ToDecimal(((TextBox)row.FindControl("txtSubProductDiscountPercentage")).Text)) / 100),
                                  ProductCost = Convert.ToDecimal(((TextBox)row.FindControl("txtSubProductProductCost")).Text)
                              }).ToList();

            prodct.SubProducts = SubProductList;
            if (ProductImage.HasFile)
            {
                prodct.ProductImgs = ProductImage.PostedFile;
                //Live Code
                //prodct.ProductImgUrl = prodct.ProductImgs.Upload(Shared.ProductImageTypes.ProductImg);
                //Test Code
                string ImgPath = prodct.ProductImgs.Upload(Shared.ProductImageTypes.ProductImg);
                string[] Img = ImgPath.Split('\\');
                prodct.ProductImgUrl = "https://www.healthurwealth.com/UploadFiles/" + Img[5];
            }
            if (FileUpload1.HasFile)
            {
                UploadGallery(prodct.ProductId.ToString());
                //  txtGalleryPath
            }
            var product = new Product();
            product.ProductImgs = ProductImage.PostedFile;
            product.SizeGuidePath = @"C:\inetpub\wwwroot\HUW_live\UploadFiles/Size_Chart/" + FileUpload2.PostedFile.FileName;

            if (prodct.ProductId != 0)
            {

                UploadSize_Chart(prodct.ProductId);
                UploadGallery(product.ProductId.ToString());
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alert", "Product has been saved Product Id :" + product.ProductId + "", true);
                //  txtGalleryPath
            }
            //updating related products 
            var RelatedProductItems = (List<RelatedProduct>)BalUtility.GetSession(Shared.Sessions.RelatedProductList) ??
                         new List<RelatedProduct>();
            RelatedProductItems.ForEach(p => p.ProductId = PID);

            prodct.RelatedProducts = RelatedProductItems;


            repository.UpdateProduct(prodct);
            BalUtility.ClearSession(Shared.Sessions.RelatedProductList);

            Response.Redirect("./ActivateProducts.aspx");
        }
        catch (Exception)
        {
            throw;
        }
    }


    public void UploadGallery(string ProductId)
    {
        var galleryPath1 = @"C:\inetpub\wwwroot\HUW_live\UploadFiles\Gallery\";
        if (FileUpload1.HasFile)
        {
            //SqlDataSource1.Insert();
            //     string filename = Path.GetExtension(FileUpload1.FileName);
            string filename = ProductId + Path.GetExtension(FileUpload1.FileName);
            string fullpath = Path.Combine(galleryPath1, filename); //Zip File Save On ServerSide.
            if (File.Exists(fullpath))
            {
                File.Delete(fullpath);
            }
            FileUpload1.SaveAs(fullpath);
            ArrayList zippedList = UnZipFile(ProductId, fullpath);  //method for Extracted the Zip File.

            string[] array1 = Directory.GetFiles(galleryPath1 + ProductId + "/small/");
            string[] array2 = Directory.GetFiles(galleryPath1 + ProductId + "/large/");


            List<ProductsGallery> gallery = new List<ProductsGallery>();
            for (int i = 0; i < array1.Length; i++)
            {
                gallery.Add(new ProductsGallery
                {
                    ProductId = Convert.ToInt64(ProductId),
                    ImgUrl = "https://www.healthurwealth.com/UploadFiles/Gallery/" + ProductId + "/small/" + Path.GetFileName(array1[i]),
                    LargeImgUrl = "https://www.healthurwealth.com/UploadFiles/Gallery/" + ProductId + "/large/" + Path.GetFileName(array2[i]),
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now
                });
            }

            var repository = new ProductsGalleryRepository();
            repository.Insert(gallery);
            File.Delete(Path.Combine(galleryPath1, filename));
        }
    }

    private ArrayList UnZipFile(string ProductId, string fullpath)
    {
        ArrayList pathList = new ArrayList(); //contain the number of Excel file.
        try
        {
            if (File.Exists(fullpath))
            {
                string baseDirectory = Path.GetDirectoryName(fullpath);

                using (ZipInputStream ZipStream = new ZipInputStream(File.OpenRead(fullpath)))
                {
                    string strNewDirectory = @"" + baseDirectory + @"\" + ProductId;
                    if (!Directory.Exists(strNewDirectory))
                    {
                        Directory.CreateDirectory(strNewDirectory);
                    }
                    baseDirectory = strNewDirectory;
                    ZipEntry theEntry;
                    ZipConstants.DefaultCodePage = 850;

                    while ((theEntry = ZipStream.GetNextEntry()) != null)
                    {
                        if (theEntry.IsFile)
                        {
                            if (theEntry.Name != "")
                            {
                                string strNewFile = @"" + baseDirectory + @"\" + theEntry.Name;
                                if (File.Exists(strNewFile))
                                {
                                    //continue;
                                }

                                using (FileStream streamWriter = File.Create(strNewFile))
                                {
                                    //pathList.Add(Path.Combine(@"C:\inetpub\wwwroot\UploadFiles\Gallery\" + ProductId + @"/", theEntry.Name));
                                    pathList.Add(Path.Combine(@"C:\inetpub\wwwroot\HUW_live\UploadFiles\HUW_live\Gallery\" + ProductId + @"/", theEntry.Name));
                                    int size = 2048;
                                    byte[] data = new byte[2048];
                                    while (true)
                                    {
                                        size = ZipStream.Read(data, 0, data.Length);
                                        if (size > 0)
                                            streamWriter.Write(data, 0, size);
                                        else
                                            break;
                                    }
                                    streamWriter.Close();
                                }
                            }
                        }
                        else if (theEntry.IsDirectory && (theEntry.Name == "small/" || theEntry.Name == "large/"))
                        {
                            strNewDirectory = @"" + baseDirectory + @"\" + theEntry.Name;
                            if (!Directory.Exists(strNewDirectory))
                            {
                                Directory.CreateDirectory(strNewDirectory);
                            }
                        }
                    }
                    ZipStream.Close();
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
        return pathList;
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        UploadGallery("123");
    }

    protected void ddlSuperCatogeries_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindCategories();
    }

    protected void chkDeletePhotosAndUpldphtos_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDeletePhotosAndUpldphtos.Checked == true)
        {
            var data1 = BalUtility.GetProductInfo(Convert.ToInt32(Request.QueryString["ID"]));
            //Live Code
            //File.Delete(Server.MapPath(data1.ProductImgUrl));
            //Test Code
            File.Delete(@"C:\inetpub\wwwroot\\HUW_live\UploadFiles\" + data1.ProductImgUrl.Split('/')[4]);
            File.Delete(@"C:\inetpub\wwwroot\HUW_live\UploadFiles\" + data1.ProductImgUrl2.Split('/')[4]);
        }
    }
    protected void chkDeleteFolderPhtosandUpload_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDeleteFolderPhtosandUpload.Checked == true)
        {
            int productId = Convert.ToInt32(Request.QueryString["ID"]);
            BalUtility.DeleteProdcutgallery(productId);
            File.Delete(Path.Combine(@"C:\inetpub\wwwroot\UploadFiles\Gallery\", productId.ToString()));
        }
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {

        try
        {
            var repository = new ProductRepository();
            int totalRecords;
            long PID = Convert.ToInt32(Request.QueryString["ID"]);

            Product prodct = new Product();

            var row = ((Button)sender).Parent.Parent as GridViewRow;
            if (row != null) ManageProductFeatures(Shared.Actions.Delete, row.RowIndex);


        }
        catch (Exception)
        {
            throw;
        }
    }

    public void UploadSize_Chart(long ProductId)
    {
        if (FileUpload2.HasFile)
        {
            //SqlDataSource1.Insert();
            //     string filename = Path.GetExtension(FileUpload1.FileName);
            string filename = ProductId + Path.GetExtension(FileUpload2.FileName);
            string fullpath = Server.MapPath("https://www.healthurwealth.com/UploadFiles/Size_Chart/") + FileUpload2.FileName; //Zip File Save On ServerSide.
            if (File.Exists(fullpath))
            {
                File.Delete(fullpath);
            }
            FileUpload2.SaveAs(fullpath);
            //ArrayList zippedList = UnZipFile(ProductId, fullpath);  //method for Extracted the Zip File.

            //string[] array1 = Directory.GetFiles(Server.MapPath("https://www.healthurwealth.com/UploadFiles/Gallery/Size_Chart/"));
            //string[] array2 = Directory.GetFiles(Server.MapPath("https://www.healthurwealth.com/UploadFiles/Gallery/" + ProductId + "/large/"));


            //List<ProductsGallery> gallery = new List<ProductsGallery>();
            //for (int i = 0; i < array1.Length; i++)
            //{
            //    gallery.Add(new ProductsGallery
            //    {
            //        ProductId = Convert.ToInt64(ProductId),
            //        ImgUrl = "https://www.healthurwealth.com/UploadFiles/Gallery/" + ProductId + "/small/" + Path.GetFileName(array1[i]),
            //        LargeImgUrl = "https://www.healthurwealth.com/UploadFiles/Gallery/" + ProductId + "/large/" + Path.GetFileName(array2[i]),
            //    });
            //}

            var repository = new ProductRepository();
            var currentProd = repository.Single(p => p.ProductId == ProductId);
            currentProd.SizeGuidePath = fullpath;
            repository.Update(currentProd);

            //   File.Delete(Server.MapPath("https://www.healthurwealth.com/UploadFiles/Size_Chart/") + filename);
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
                cmd.CommandText = "select * from Cashback_Coupons where is_Active=1";
                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        ListItem item = new ListItem();
                        var desc = sdr["Cashbackdesc"].ToString().Replace(" ", "_");
                        //item.Text = sdr["Coup_Code"].ToString();
                        item.Text = "&nbsp;<a href=\"#test\" title =" + desc + " style='COLOR: #16579c;' >" + sdr["Coup_Code"].ToString() + "</a>";
                        item.Text += "<br/>";
                        //item.Text = sdr["Coup_Code"].ToString();
                        item.Value = sdr["Coup_ID"].ToString();
                        item.Value += ',';
                        item.Value += sdr["Defaultid"].ToString();
                        var amount = sdr["Coup_Amount"].ToString();
                        if (amount != null && amount != "")
                        {
                            item.Text += "&nbsp;<label>" + sdr["Coup_Amount"].ToString() + "Rs<label/>";
                        }
                        else
                        {
                            item.Text += "&nbsp;<label>" + sdr["Coup_Percentage"].ToString().Replace(".00", "") + "%<label/>";

                        }

                        chkaccess.Items.Add(item);
                    }
                }
                conn.Close();
            }
        }
    }




}