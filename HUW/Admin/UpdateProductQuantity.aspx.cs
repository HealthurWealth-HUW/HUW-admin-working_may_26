using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using System.Text;
using Utility;

public partial class Admin_UpdateProductQuantity : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
     
    {
        if (!Page.IsPostBack)
        {
           if (Request.QueryString["ID"] == null)
            {
                Response.Redirect("EditProduct.aspx");
            }
            var ID = Request.QueryString["ID"];
            var data = BalUtility.GetProductInfo(Convert.ToInt32(Request.QueryString["ID"]));
            lblSubprdctsTitle.Visible = false;
            if (data != null)
            {
                txtProductId.Text = Convert.ToString(data.ProductId);
                txtProductQuantity.Text = Convert.ToString(data.Quantity);
                if (data.SubProducts.Count != 0)
                {
                    lblSubprdctsTitle.Visible = true;
                    GVSubProducts.DataSource = data.SubProducts;
                    GVSubProducts.DataBind();
                }
            }
       }
    }
    protected void btnUpdateProduct_Click(object sender, EventArgs e)
    {
        try
        {
            int Qty = 0;
            var repository = new ProductRepository();
            Product prodct = new Product();

            prodct.ProductId = Convert.ToInt32(Request.QueryString["ID"]);
            //prodct.Quantity = Convert.ToInt32(txtProductQuantity.Text);
            prodct.Quantity =Convert.ToInt32(txtProductQuantity.Text);
            prodct.SubProducts = (from GridViewRow row in GVSubProducts.Rows
                                  select new SubProduct
                                  {
                                      ProductId = prodct.ProductId,
                                      SubProductId = Convert.ToInt32(((TextBox)row.FindControl("txtSubProductId")).Text),
                                      SPName = (((TextBox)row.FindControl("txtSPName")).Text),
                                      Quantity = Convert.ToInt32(((TextBox)row.FindControl("txtSubProductQuantity")).Text),

                                  }).ToList();

            foreach (var item in prodct.SubProducts)
            {
                var qty1 = item.Quantity;
                Qty = Qty + qty1;
                prodct.Quantity = Qty;
            }
            repository.UpdateProductQtyAndInsert(prodct);

            //ProductInventory PrdctQtyInvntry = new ProductInventory();
            var InvntryRepostry = new ProductInventoryRepository();
            List<ProductInventory> ProductInventoryList = new List<ProductInventory>();
            ProductInventoryList = (from GridViewRow row in GVSubProducts.Rows
                                    select new ProductInventory
                                    {
                                        ProductId = prodct.ProductId,
                                        ProductQuantity = Convert.ToInt32(((TextBox)row.FindControl("txtSubProductQuantity")).Text),
                                        SubProductId = Convert.ToInt32(((TextBox)row.FindControl("txtSubProductId")).Text),
                                    }).ToList();
           // InvntryRepostry.InsertPrdctInventory(ProductInventoryList);
           InvntryRepostry.Insert(ProductInventoryList);
            lblMsg.ForeColor = System.Drawing.Color.Green;
            lblMsg.Text = "You have Successfully Updated Product(s) Quantity.";
        }
        catch(Exception ex)
        {
            throw ex;
        }
       
    }

  
}