using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using Utility;
using DAL;
using System.IO;

public partial class Admin_NotifymeProducts : System.Web.UI.Page
{
    db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();
    string ApiUrl = System.Configuration.ConfigurationManager.AppSettings["ApiUrl"];
    protected void Page_Load(object sender, EventArgs e)
    {
       
        //Live Code
        //gdvNotifyme.DataSource = (from Nm in Entity.NoftifyMes
        //                          join Pr in Entity.Products
        //                          on Nm.ProductId equals Pr.ProductId
        //                          where Nm.IsActive == true
        //                          select new
        //                          {
        //                              Pr.ProductName,
        //                              Pr.ProductId,
        //                              Nm.UserName,
        //                              Nm.EmailId,
        //                              Nm.MobileNumber
        //                          }).ToList().Distinct().OrderByDescending(x => x.ProductId);

        //Test Code
        gdvNotifyme.DataSource = (from Nm in Entity.NoftifyMes
                                  join Pr in Entity.Products
                                  on Nm.ProductId equals Pr.ProductId
                                  where Nm.IsActive == true
                                  select new
                                  {
                                      Pr.ProductName,
                                      Pr.ProductId,
                                      Nm.UserName,
                                      Nm.EmailId,
                                      Nm.MobileNumber,
                                      Nm.CreatedOn
                                  }).ToList().Distinct().OrderByDescending(x => x.CreatedOn);
        gdvNotifyme.DataBind();
    }

    protected void gdvNotifyme_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "NotifyMe")
        {
            Utility.MailMessage ms = new Utility.MailMessage();
            ms.Subject = "Requested Product Available in HUW";

            long ProductID = Convert.ToInt64(e.CommandArgument.ToString());
            Product ProductInfo = Entity.Products.Where(x => x.ProductId == ProductID).First();
            List<NoftifyMe> Notify = Entity.NoftifyMes.Where(x => x.ProductId == ProductID && x.IsActive == true).ToList();

            if (ProductInfo.Quantity > 0)
            {
                foreach (NoftifyMe me in Notify)
                {
                    ms.To = me.EmailId;
                    //ms.Cc = "venkatakrishnateja.566@gmail.com";
                    string body = string.Empty;
                    string htmlPagePath = Shared.GethtmlPage(Shared.ProductStatusForSendMail.NotifyMeAvailable);
                    using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("" + htmlPagePath + "")))
                    {
                        body = reader.ReadToEnd();
                    }

                    body = body.Replace("##FirstName##", me.UserName);
                    body = body.Replace("##Product##", "<img height='100' src='" + ProductInfo.ProductImgUrl.Replace("~/", ApiUrl) + "' alt='" + ProductInfo.ProductName + "' />");
                    body = body.Replace("##ProductName##", ProductInfo.ProductName);
                    body = body.Replace("##UnitPrice##", ProductInfo.ProductCost.ToString());
                    ms.Body = body;
                    ms.IsBodyHtml = true;

                    try
                    {
                        ms.SendMail();
                    }
                    catch (Exception ex)
                    {

                    }

                    me.IsActive = false;
                    Entity.SaveChanges();

                    lblError.Text = "Users got mails with Health ur Wealth Regarding Product: " + ProductInfo.ProductName;
                    lblError.ForeColor = System.Drawing.Color.Green;
                }

            }
            else
            {
                lblError.Text = "Product Quantity is 0. Please Increase the Quanity of the Procduct and then Notify to Users.";
                lblError.ForeColor = System.Drawing.Color.Red;
            }

            gdvNotifyme.DataSource = (from Nm in Entity.NoftifyMes
                                      join Pr in Entity.Products
                                      on Nm.ProductId equals Pr.ProductId
                                      where Nm.IsActive == true
                                      select new
                                      {
                                          Pr.ProductName,
                                          Pr.ProductId,
                                          Nm.UserName,
                                          Nm.EmailId,
                                          Nm.MobileNumber,
                                          Nm.CreatedOn
                                      }).ToList().Distinct().OrderByDescending(x => x.CreatedOn);
            gdvNotifyme.DataBind();
        }
    }

    protected void btnNotify_Click(object sender, EventArgs e)
    {
        string Name = txtName.Text;
        string email = txtEmail.Text;
        string productName = txtProduct.Text;
        long productId = 0;
        if (txtProductId.Text != "")
        {
            productId = Convert.ToInt64(txtProductId.Text);
        }

        gdvNotifyme.DataSource = (from Nm in Entity.NoftifyMes
                                  join Pr in Entity.Products
                                  on Nm.ProductId equals Pr.ProductId
                                  where Nm.IsActive == true
                                  && (productId == 0 ? true : (Nm.ProductId == productId))
                                  && (Name == "" ? true : (Nm.UserName.Contains(Name)))
                                  && (email == "" ? true : (Nm.UserName.Contains(email)))
                                  && (productName == "" ? true : (Pr.ProductName.Contains(productName)))
                                  select new
                                  {
                                      Pr.ProductName,
                                      Pr.ProductId,
                                      Nm.UserName,
                                      Nm.EmailId,
                                      Nm.MobileNumber,
                                      Nm.CreatedOn
                                  }).ToList().Distinct().OrderByDescending(x => x.CreatedOn);
        gdvNotifyme.DataBind();
    }
}