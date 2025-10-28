using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using Utility;

public partial class Admin_putmedicinesintoproducttable : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        db_Zon_HuwEntities context = new db_Zon_HuwEntities();
        List<Product> prodtbl = new List<Product>();
        Product prodobj = new Product();
        var medlist = context.Medicines_tbl.ToList();
        foreach (var item in medlist)
        {
            decimal rate = 999999;
            decimal mrp = 999999;
            decimal stock = 999999;
            if (item.Rate != null)
            {
                rate = Convert.ToDecimal(item.Rate);
            }
            if (item.MRP != null)
            {
                mrp = Convert.ToDecimal(item.MRP);
            }
            if (item.stock != null)
            {
                stock = Convert.ToDecimal(item.stock);
                if (stock < 0)
                {
                    stock = stock * -1;
                }
            }
            prodobj.Brand = item.company;
            prodobj.ProductCode = item.Item_Code;
            prodobj.ProductName = item.Med_Name;
            prodobj.ProductOriginalCost = rate;
            prodobj.ProductCost = mrp;
            prodobj.Quantity = Convert.ToInt32(stock);


            //prodobj.SubCategoryId=
            prodobj.IsFeaturedProduct = false;
            prodobj.ProductDescription = "medicines";
            prodobj.ProductDiscountPercentage = 0; //discount percentage here
            prodobj.ProductDiscountCost = 0;       //discount amount
            prodobj.ProductImgUrl = "";            //default image location
            prodobj.IsActive = true;
            prodobj.IsDeleted = false;
            prodobj.CreatedOn = DateTime.Now;
            prodobj.UpdatedOn = DateTime.Now;
            prodobj.IsSold = false;                 //presumming out of stock
            prodobj.CanCompare = false;
            prodobj.HasReviews = false;
            prodobj.CanReviewsNeedPermission = false;
            prodobj.ShortDescription = "medicines";

            prodobj.SubCategoryId = 203;
            prodobj.IsPresciption = true;

            context.Products.Add(prodobj);
            context.SaveChanges();
        }
    }
}