using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
 

namespace OnlineShopping
{
    public partial class details : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {            
            //var url = HttpContext.Current.Request.RawUrl;
            //Page pageRef = HttpContext.Current.Handler as Page;
            //// Response.AppendHeader("Cache-Control", "no-cache, no-store, must-revalidate"); // HTTP 1.1.
            ////Response.AppendHeader("Pragma", "no-cache"); // HTTP 1.0.
            ////Response.AppendHeader("Expires", "0"); // Proxies.

            //int productid = UrlParamsExtractor.GetInstance.getParam("productid", HttpContext.Current.Handler as Page);
            //string ProductName = UrlParamsExtractor.GetInstance.getParam("productname", HttpContext.Current.Handler as Page);
            //Session["ProductInputData"] = productid;
            //if (ProductName != "" && productid !=0)
            //{
            //    ProductName = ProductName.Replace("-", " ");
            //    this.Page.Title = "Buy " + ProductName + " Online - HealthurWealth";
            //    HtmlMeta keywords = new HtmlMeta();
            //    keywords.HttpEquiv = "keywords";
            //    keywords.Name = "keywords";
            //    keywords.Content = ProductName + ",  Buy " + ProductName + " online," + ProductName + " online, Buy" + ProductName + " online at best price";
            //    this.Page.Header.Controls.Add(keywords);

            //    HtmlMeta description = new HtmlMeta();
            //    description.HttpEquiv = "description";
            //    description.Name = "description";
            //    description.Content = "Buy " + ProductName + " Online at Best price in Hyderabad. Free shipping and cash on delivery available. ";
            //    this.Page.Header.Controls.Add(description);

            //    HtmlMeta SLURP = new HtmlMeta();
            //    SLURP.HttpEquiv = "SLURP";
            //    SLURP.Name = "SLURP";
            //    SLURP.Content = "INDEX, FOLLOW";
            //    this.Page.Header.Controls.Add(SLURP);

            //    HtmlMeta GOOGLEBOT = new HtmlMeta();
            //    GOOGLEBOT.HttpEquiv = "GOOGLEBOT";
            //    GOOGLEBOT.Name = "GOOGLEBOT";
            //    GOOGLEBOT.Content = "INDEX, FOLLOW";
            //    this.Page.Header.Controls.Add(GOOGLEBOT);

            //    HtmlMeta ROBOTS = new HtmlMeta();
            //    ROBOTS.HttpEquiv = "ROBOTS";
            //    ROBOTS.Name = "ROBOTS";
            //    ROBOTS.Content = "INDEX, FOLLOW";
            //    this.Page.Header.Controls.Add(ROBOTS);

            //    HtmlMeta MSNBOT = new HtmlMeta();
            //    MSNBOT.HttpEquiv = "MSNBOT";
            //    MSNBOT.Name = "MSNBOT";
            //    MSNBOT.Content = "INDEX, FOLLOW";
            //    this.Page.Header.Controls.Add(MSNBOT);

            //    HtmlMeta Rating = new HtmlMeta();
            //    Rating.HttpEquiv = "Rating";
            //    Rating.Name = "Rating";
            //    Rating.Content = "General";
            //    this.Page.Header.Controls.Add(Rating);

            //    HtmlMeta Distribution = new HtmlMeta();
            //    Distribution.HttpEquiv = "Distribution";
            //    Distribution.Name = "Distribution";
            //    Distribution.Content = "India";
            //    this.Page.Header.Controls.Add(Distribution);

            //    HtmlMeta Language = new HtmlMeta();
            //    Language.HttpEquiv = "MSNBOT";
            //    Language.Name = "Language";
            //    Language.Content = "English";
            //    this.Page.Header.Controls.Add(Language);

            //    HtmlMeta Author = new HtmlMeta();
            //    Author.HttpEquiv = "Author";
            //    Author.Name = "Author";
            //    Author.Content = "healthurwealth.com";
            //    this.Page.Header.Controls.Add(Author);

            //    HtmlMeta REVISIT = new HtmlMeta();
            //    REVISIT.HttpEquiv = "REVISIT-AFTER";
            //    REVISIT.Name = "REVISIT-AFTER";
            //    REVISIT.Content = "1 DAYS";
            //    this.Page.Header.Controls.Add(REVISIT);


            //    HtmlLink link = new HtmlLink();
            //    link.Attributes.Add("href", Request.Url.ToString());
            //    link.Attributes.Add("rel", "canonical");
            //    Page.Header.Controls.Add(link);
            //}
        }
    }
}
