using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using Utility;
using System.IO;
using System.Collections;
using System.Drawing;
using ICSharpCode.SharpZipLib.Zip;
using System.Data.SqlClient;
using System.Configuration;
using DAL;
using System.Text;

public partial class Admin_AddProducts : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            Getaccess();
            BindGST();
            BindSuperCategories();
            BindProductFeaturesCategories();
            ManageProductFeatures(Shared.Actions.Read);
            ManageProductSpecifications(Shared.Actions.Read);
            ManageSubProducts(Shared.Actions.Read);
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
            BindSubCategories();
        }
    }
    public void BindGST()
    {
        DDLgst.Items.Clear();
        DDLgst.DataTextField = "GSTPer";
        DDLgst.DataValueField = "GSTPer";
        var repository = new GSTRepository();
        int totalRecords;
        var data = repository.FetchAllByPage(p => p.GstID, out totalRecords, 0, 0, null, c => new { c.GstID, c.GSTPer });

        DDLgst.DataSource = data;
        DDLgst.DataBind();
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
        }
    }

    private void BindProductFeaturesCategories()
    {
        //      var repository = new FeaturesSubCategoryRepository();
        //     int totalRecords;
        //     int categoryId = Convert.ToInt32(ddlCategory.SelectedItem.Value);
        ////     var data = repository.FetchAllByPage(p => p.CategoryId, out  totalRecords, 0, 0, (p => p.IsActive && p.IsDeleted == false && p.CategoryId == categoryId), c => new { c.CategoryId, c.CategoryName, c.IsActive });

        //     var data = repository.FetchAllByPage(p => p.FeaturesSubCategoryId, out totalRecords,0,0,
        //                                        null, null, "");
        //    // foreach (var subCategory in data)
        //      //   subCategory.CategoryName = subCategory.Category.CategoryName;



        //     ddlSubCategory.DataSource = data;
        //     ddlSubCategory.DataBind();
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

    protected void btnDeleteProductSpecification_Click(object sender, EventArgs e)
    {
        var row = ((Button)sender).Parent.Parent as GridViewRow;
        if (row != null) ManageProductSpecifications(Shared.Actions.Delete, row.RowIndex);

    }

    protected void btnAddProductSpecification_Click(object sender, EventArgs e)
    {
        ManageProductSpecifications(Shared.Actions.Create);
    }

    private void ManageProductSpecifications(Shared.Actions action, int rowId = -1)
    {
        var productSpecification = (from GridViewRow row in GVProductSpecifications.Rows
                                    select new ProductSpecification
                                    {
                                        ProductSpecificationName = ((TextBox)row.FindControl("txtProductSpecificationName")).Text,
                                        ProductSpecificationNameValues = ((TextBox)row.FindControl("txtProductSpecificationValue")).Text,
                                        SpecificationTypeId = Convert.ToInt32(((DropDownList)row.FindControl("ddlProductSpecificationTypes")).SelectedItem.Value)
                                    }).ToList();
        switch (action)
        {
            case Shared.Actions.Create:
                productSpecification.Add(new ProductSpecification());
                break;
            case Shared.Actions.Delete:
                if (rowId != -1) productSpecification.RemoveAt(rowId);
                break;
        }
        GVProductSpecifications.DataSource = productSpecification;
        GVProductSpecifications.DataBind();

        foreach (GridViewRow row in GVProductSpecifications.Rows)
        {
            var dropdown = row.FindControl("ddlProductSpecificationTypes") as DropDownList;
            if (dropdown != null)
            {
                //   dropdown.SelectedItem.Value = productSpecification[row.RowIndex].SpecificationTypeId.ToString();
                dropdown.SelectedIndex = dropdown.Items.IndexOf(dropdown.Items.FindByValue(productSpecification[row.RowIndex].SpecificationTypeId.ToString()));

            }
        }
    }

    protected void GVProductSpecifications_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var dropdown = e.Row.FindControl("ddlProductSpecificationTypes") as DropDownList;
            if (dropdown != null)
            {
                dropdown.DataSource = Enumeration.GetAll<Shared.SpecificationTypes>();
                dropdown.DataTextField = "Value";
                dropdown.DataValueField = "Key";
                dropdown.DataBind();

            }
        }
    }

    //Sub Products
    protected void btnDeleteSubProducts_Click(object sender, EventArgs e)
    {
        var row = ((Button)sender).Parent.Parent as GridViewRow;
        if (row != null) ManageSubProducts(Shared.Actions.Delete, row.RowIndex);


        txtProductQuantity.Enabled = GVSubProducts.Rows.Count == 0 ? true : false;
    }

    protected void btnAddSubProducts_Click(object sender, EventArgs e)
    {
        // txtProductQuantity.Enabled = false;
        // txtProductQuantity.ReadOnly = true;
        ManageSubProducts(Shared.Actions.Create);
    }

    private void ManageSubProducts(Shared.Actions action, int rowId = -1)
    {
        var subProducts = (from GridViewRow row in GVSubProducts.Rows

                           select new SubProduct
                           {
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

    protected void btnSaveProduct_Click(object sender, EventArgs e)
    {
        try
        {

            Product product = new Product();

            ProductGallery products = new ProductGallery();
            // GVFeatureSubCat  hiddnFeatureSubCatId   txtFeaturesSubCatDesc
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

            List<ProductFeature> ProductFeatureList = new List<ProductFeature>();
            //foreach (GridViewRow grow in GVProductFeatures.Rows)
            //{
            //    GridView gv = (GridView)grow.FindControl("GVFeatureSubCat");
            //    ProductFeatureList.AddRange((from GridViewRow row in gv.Rows
            //                                 select new ProductFeature
            //                                 {
            //                                     FeatureInfo = ((TextBox)row.FindControl("txtFeaturesSubCatDesc")).Text,
            //                                     FeaturesSubCategoryId = Convert.ToInt32(((HiddenField)row.FindControl("hiddnFeatureSubCatId")).Value),
            //                                     CreatedOn = System.DateTime.Now,
            //                                     UpdatedOn = System.DateTime.Now,
            //                                 }).ToList());
            //}
            try
            {

                var subproduct = new SubProduct();

                product.IsFeaturedProduct = chkIsFeatureProduct.Checked;
                product.CanCompare = chkBoxCompare.Checked;
                product.HasReviews = chkBoxHasReviews.Checked;
                product.IsPresciption = CheckBox1.Checked;
                product.CanReviewsNeedPermission = chkBoxAdminPerissionsOnReviews.Checked;

                product.Occasion = "";
                product.ProductCode = "";
                product.ProductColor = "";
                product.ProductDescription = txtProductDescription.Text;
                product.ShortDescription = elm1.InnerHtml;

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
                product.HSNCode = HSNCode.Text;
                product.ProductOriginalCost = ProductCost;
                product.ProductCost = FinalAount;
                product.ProductDiscountPercentage = ProductDiscount;
                product.ProductDiscountCost = (ProductCost - FinalAount);
                product.Quantity = Convert.ToInt32(txtProductQuantity.Text);
                product.ProductImgUrl = "";
                product.ProductImgUrl2 = "";
                product.ProductName = txtProductName.Text;
                product.Brand = txtProductBrand.Text.ToUpper();
                product.ProductFeaturesContent = ha.InnerHtml;
                product.H2Tag = tcth2tag.Text;
                product.SubCategoryId = Convert.ToInt32(ddlSubCategory.SelectedItem.Value);
                product.ProductFeatures = ProductFeatureList;
                product.ProductSpecifications = (from GridViewRow row in GVProductSpecifications.Rows
                                                 select new ProductSpecification
                                                 {
                                                     ProductSpecificationName = ((TextBox)row.FindControl("txtProductSpecificationName")).Text,
                                                     ProductSpecificationNameValues = ((TextBox)row.FindControl("txtProductSpecificationValue")).Text,
                                                     SpecificationTypeId = Convert.ToInt32(((DropDownList)row.FindControl("ddlProductSpecificationTypes")).SelectedItem.Value)
                                                 }).ToList();
                product.SubProducts = (from GridViewRow row in GVSubProducts.Rows
                                       select new SubProduct
                                       {
                                           SPName = (((TextBox)row.FindControl("txtSPName")).Text),
                                           Quantity = Convert.ToInt32(((TextBox)row.FindControl("txtSubProductQuantity")).Text),
                                           ProductOriginalCost = Convert.ToDecimal(((TextBox)row.FindControl("txtSubProductOriginalCost")).Text),
                                           ProductDiscountPercentage = Convert.ToDecimal(((TextBox)row.FindControl("txtSubProductDiscountPercentage")).Text),
                                           ProductDiscountCost = ((Convert.ToDecimal(((TextBox)row.FindControl("txtSubProductOriginalCost")).Text)) * (Convert.ToDecimal(((TextBox)row.FindControl("txtSubProductDiscountPercentage")).Text)) / 100),
                                           ProductCost = Convert.ToDecimal(((TextBox)row.FindControl("txtSubProductProductCost")).Text)
                                       }).ToList();

                product.ProductImgs = ProductImage.PostedFile;
                product.ProductImgss = FileUpload3.PostedFile;

                if (FileSize != 0)
                {
                    product.SizeGuidePath = @"C:\inetpub\wwwroot\HUW_live\UploadFiles/Size_Chart/" + FileUpload2.PostedFile.FileName;
                    //  ProductGalleryImgs = Request.Files,
                }
                List<Product> freeProductItems = (List<Product>)BalUtility.GetSession(Shared.Sessions.FreeProductList) ?? new List<Product>();

                foreach (Product p in freeProductItems)
                {
                    product.Free_Product_ID = p.ProductId;
                    product.Is_Free_Product_Active = true;
                }

                var RelatedProductItems = (List<RelatedProduct>)BalUtility.GetSession(Shared.Sessions.RelatedProductList) ??
                             new List<RelatedProduct>();

                var repository = new ProductRepository();
                product.IsActive = true;
                product.CreatedOn = DateTime.Now;
                product.UpdatedOn = DateTime.Now;
                product.Page_Title = "Buy " + txtProductName.Text + " Online - HealthurWealth";
                product.Meta_Keywords = txtProductName.Text + ", Buy " + txtProductName.Text + " online, " + txtProductName.Text + " online, Buy " + txtProductName.Text + " online at best price";
                product.Meta_Description = "Buy " + txtProductName.Text + " Online at Best price in Hyderabad. Free shipping and cash on delivery available";
                product.GST = Convert.ToDecimal(DDLgst.SelectedItem.Value);
                repository.Save(product);
                Tbl_AdditionalInfo additionalInfo = new Tbl_AdditionalInfo();
                additionalInfo.ProductId = product.ProductId;
                additionalInfo.Marketerdetails = Marketerdetails.Text;
                additionalInfo.Manufacturer_Date = Convert.ToDateTime(Manufacturer_Date.Text);
                additionalInfo.Best_Before_Date = Convert.ToInt32(Best_Before_Date.Text);
                additionalInfo.Country_Of_Origin = Country_Of_Origin.Text;
                additionalInfo.GTIN = GTIN.Text;
                additionalInfo.Manufacturer_Details = Manufacturer.Text;
                additionalInfo.BatchNo = txtBatch.Text;
                repository.UpdateAdditionalinfo(additionalInfo);
                var y = BalUtility.Logmaintainance((int)Shared.OrderStatus.Cashbackassigned, "Cashbackassigned", "", product.ProductId.ToString());
                CBC_Data_Tree def = new CBC_Data_Tree();
                foreach (ListItem item in chkaccess.Items)
                {
                    if (item.Selected == true)
                    {
                        db_Zon_HuwEntities context = new db_Zon_HuwEntities();
                        def.Prod_ID = (int)product.ProductId;
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
                RelatedProductItems.ForEach(p => p.ProductId = product.ProductId);
                var relatedProductRepository = new RelatedProductRepository();
                relatedProductRepository.Insert(RelatedProductItems);
                BalUtility.ClearSession(Shared.Sessions.RelatedProductList);
                if (product.ProductId != 0)
                {
                    var pfrepository = new ProductFeatureRepository();
                    foreach (var item in ProductFeatureList)
                        item.ProductId = product.ProductId;
                    //var sss = pfrepository.Insert(ProductFeatureList);

                    UploadGallery(product.ProductId.ToString());
                    UploadSize_Chart(product.ProductId.ToString());
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alert", "Product has been saved Product Id :" + product.ProductId + "", true)
                        ;
                    //txtGalleryPath
                }
            }

            catch (System.Data.SqlTypes.SqlTypeException sdex)
            {
                var data1 = sdex.Message;
                var data = sdex.StackTrace;
                lblErrorCatch.Text = sdex.Message + "---" + sdex.StackTrace;
            }

            catch (Exception ex)
            {
                lblErrorCatch.Text = ex.Message + "---" + ex.StackTrace;
                throw;
            }
        }
        catch (Exception ex)
        {
            lblErrorCatch.Text = ex.Message + "---" + ex.StackTrace;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alert", ex.Message + "---" + ex.StackTrace, true);
        }
        Response.Write("<script>alert('Record Inserted Successfully..');window.location = 'home.aspx';</script>");
        //Response.Redirect("home.aspx");
    }

    public void UploadGallery(string ProductId)
    {
        var galleryPath = @"C:\inetpub\wwwroot\HUW_live\UploadFiles\Gallery\";
        if (FileUpload1.HasFile)
        {
            //SqlDataSource1.Insert();
            //     string filename = Path.GetExtension(FileUpload1.FileName);
            string filename = ProductId + Path.GetExtension(FileUpload1.FileName);
            string fullpath = Path.Combine(galleryPath, filename); //Zip File Save On ServerSide.
            if (File.Exists(fullpath))
            {
                File.Delete(fullpath);
            }
            FileUpload1.SaveAs(fullpath);
            ArrayList zippedList = UnZipFile(ProductId, fullpath);  //method for Extracted the Zip File.

            string[] array1 = Directory.GetFiles(galleryPath + ProductId + "/small/");
            string[] array2 = Directory.GetFiles(galleryPath + ProductId + "/large/");


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
            File.Delete(Path.Combine(galleryPath, filename));
        }
    }

    public void UploadSize_Chart(string ProductId)
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

            //var repository = new ProductsGalleryRepository();
            //repository.Insert(gallery);
            //File.Delete(Server.MapPath("https://www.healthurwealth.com/UploadFiles/Size_Chart/") + filename);
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

                    ZipEntry theEntry; ZipConstants.DefaultCodePage = 850;
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
                                    pathList.Add(Path.Combine(@"C:\inetpub\wwwroot\HUW_live\UploadFiles\Gallery\" + ProductId + @"/", theEntry.Name));
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
    private void Getaccess()
    {
        //List<Cashback_Coupons> data = new List<Cashback_Coupons>;
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager
                    .ConnectionStrings["db_Zon_constr"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "select * from Cashback_Coupons where Is_Active=1";
                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        ListItem item = new ListItem();
                        var desc = sdr["Cashbackdesc"].ToString().Replace(" ", "_");
                        //item.Text = sdr["Coup_Code"].ToString();
                        //item.Text = "&nbsp;<div>";
                        item.Text = "&nbsp;<a href=\"#test\" title =" + desc + " style='COLOR: #16579c;' >" + sdr["Coup_Code"].ToString() + "</a>";
                        item.Text += "<br/>";
                        item.Value = sdr["Coup_ID"].ToString();
                        item.Value += ',';
                        item.Value += sdr["Defaultid"].ToString();
                        var amount = sdr["Coup_Amount"].ToString();
                        if (amount != null && amount != "")
                            item.Text += "&nbsp;<label>" + sdr["Coup_Amount"].ToString() + "Rs<label/>";
                        else
                            item.Text += "&nbsp;<label>" + sdr["Coup_Percentage"].ToString().Replace(".00", "") + "%<label/>";

                        //item.Text += "&nbsp;</div>";
                        // item.Text += "&nbsp;<input type=\"text\" class=\"tooltiptext\" onclick =\"adddata()\"/>";
                        chkaccess.Items.Add(item);

                    }
                }
                conn.Close();
            }
        }
    }

    //public void cost()
    //{
    //    decimal ProductOriginalCost = 0;
    //    decimal ProductDiscountPercentage = 0;


    //    if ((((TextBox)row.FindControl("txtSubProductOriginalCost")).Text) != "")
    //        try
    //        {
    //            ProductOriginalCost = Convert.ToDecimal(txtSubProductOriginalCost.Text);
    //        }
    //        catch { }

    //    if (txtSubProductDiscountPercentage.Text != "")
    //        try
    //        {
    //            ProductDiscountPercentage = Convert.ToDecimal(txtSubProductDiscountPercentage.Text);
    //        }
    //        catch { }


    //    decimal ProductCost = ProductOriginalCost - (ProductOriginalCost * ProductDiscountPercentage / 100);
    //    decimal ProductDiscountCost = (ProductOriginalCost - ProductCost);

    //}
}