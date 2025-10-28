using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;

public partial class Admin_prescriptionupdatenew : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
            {
                DDlstatus.AutoPostBack = true;
                long transid = Convert.ToInt64(Request.QueryString["transid"]);
                Tbl_Prescriptionorders data = context.Tbl_Prescriptionorders.Where(x => x.OrderId == transid&&x.OrderplacedUrl=="mmh").FirstOrDefault();
                txtid.Text = data.OrderId.ToString();
                txtaddress.Text = data.Address;
                txtdeliverytype.Text = data.Deliverytype;
                txtName.Text = data.Name;
                txtlocality.Text = data.Locality;
                txtMobile.Text = data.Mobile;
                txtorderitems.Text = data.Orderitems;
                txtpincode.Text = data.Pincode;
                txtAmount.Text = data.Amount.ToString();
                string[] images = data.Prescriptionurl.Split(',');
                RptImages.DataSource = images;
                RptImages.DataBind();
                //txtimage.ImageUrl = data.Prescriptionurl;
                Admincomments.InnerText = data.AdminComments;
                Usercomments.InnerText = data.UserComments;
                DDlstatus.SelectedValue = data.Status.ToString();
            }
        }
        
    }

    protected void btn_Submit_Click(object sender, EventArgs e)
    {
        using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
        {
            long transid = Convert.ToInt64(txtid.Text);
            
            Tbl_Prescriptionorders orders = context.Tbl_Prescriptionorders.Where(x => x.OrderId == transid).FirstOrDefault();
            string Message = "";
            if (DDlstatus.SelectedValue == "2") { 
             Message = "Your order " + transid + " is accepted by Mukesh Medical Hall note: All medicine stock available.Please check status here https://healthurwealth.com/home/mmh?orderid="+ orders .OrderId+ " ,for any queries call 9440689551 - Mukesh medical hall (SONAL ENTERPRISE)";
            }
            //else if(DDlstatus.SelectedValue == "2")
            //{
            //    Message = "your order " + transid + " is Ready for pickup by Mukesh Medical Hall note: All medicine stock available.Please check status here https://healthurwealth.com/home/mmh ,for any queries call 9440689551 - Mukesh medical hall";

            //}
            //else if (DDlstatus.SelectedValue == "3")
            //{
            //    Message = "your order " + transid + " is Ready for pickup by Mukesh Medical Hall note: All medicine stock available.Please check status here https://healthurwealth.com/home/mmh ,for any queries call 9440689551 - Mukesh medical hall";

            //}
            string Url = WebConfigurationManager.AppSettings["SmsUrl"].ToString();
            string UserName = WebConfigurationManager.AppSettings["SmsId"].ToString();
            string password = WebConfigurationManager.AppSettings["SmsPwd"].ToString();
            string Status = Utility.MailMessage.SendSms(Url, UserName, password, Convert.ToDecimal(orders.Mobile), Message, "N", "1707161520393287160");
            orders.Status = Convert.ToInt16(DDlstatus.SelectedValue);
            orders.AdminComments = Admincomments.InnerText;
            orders.UserComments = Usercomments.InnerText;
            orders.Amount = Convert.ToDecimal(txtAmount.Text);
            context.SaveChanges();
                }
    }
}