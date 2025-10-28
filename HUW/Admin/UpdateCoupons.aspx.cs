using BAL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;

public partial class Admin_UpdateCoupons : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Getcoupdetails();
        }
    }
    protected void ddlDiscountType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDiscountType.SelectedValue == "0")
        {
            lblError.Text = "Please select discount type";
        }
        else if (ddlDiscountType.SelectedValue == "1")
        {
            lblError.Text = "";
            divAmount.Visible = true;
            divPercentage.Visible = false;
            txtPercentage.Text = "0";
        }
        else if (ddlDiscountType.SelectedValue == "2")
        {
            lblError.Text = "";
            divAmount.Visible = false;
            divPercentage.Visible = true;
            txtAmount.Text = "0";
        }
    }
    private void Getcoupdetails()
    {

        
        using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
        {
            long UID = Convert.ToInt32(Request.QueryString["ID"]);
            Cashback_Coupons getcoups = context.Cashback_Coupons.Where(x => x.Coup_ID == UID).First();
           
                if (getcoups.Coup_Percentage != 0)
                {
                divPercentage.Visible = true;
                }
                else if (getcoups.Coup_Amount != 0)
                {
                divAmount.Visible = true;
                }
            TextBox3.Text = getcoups.Defaultid.ToString();
            txtCouponCode.Text = getcoups.Coup_Code;
            txtCartAMount.Text = getcoups.Min_Prod_Val.ToString();
            txtValidfrom.Text = getcoups.From_Date.ToString();
            txtValidTo.Text = getcoups.To_Date.ToString();
            var coupamount= (getcoups.Coup_Amount).ToString();
            var coupper= (getcoups.Coup_Percentage).ToString();
            txtAmount.Text = (coupamount).Replace(".00","");
            txtPercentage.Text = (coupper).Replace(".00", "");

            txtUsage.Text = getcoups.Total_Times_Usable.ToString();
            txtUserUsage.Text = getcoups.Total_Times_Usable_Per_User.ToString();
            TextBox1.Text = getcoups.Min_Qty.ToString();
            TextBox2.Text = getcoups.Max_Cashback.ToString();
            txtDescription.InnerText = (getcoups.Cashbackdesc).ToString();
            txttc.InnerText = (getcoups.TC).ToString();
            if (getcoups.New_User == true)
            {
                chknuser.Checked = true;
            }
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        long UID = Convert.ToInt32(Request.QueryString["ID"]);

        using (db_Zon_HuwEntities context = new db_Zon_HuwEntities())
        {
            if (ddlDiscountType.SelectedValue == "0")
            {
                lblError.Text = "Please select discount type";
            }
            else
            {
                if (ddlDiscountType.SelectedValue == "1")
                {
                    txtPercentage.Text = "0";
                }
                else if (ddlDiscountType.SelectedValue == "2")
                {
                    txtAmount.Text = "0";
                }
            }
           
                var repository = new CouponRepository();
                Cashback_Coupons def = new Cashback_Coupons();
                def.Coup_ID = Convert.ToInt32(UID);
                def.Is_Active = false;
                repository.Updatedata(def);
            CashbackRepository repositorys = new CashbackRepository();
            Cashback_Coupons cashbacknames = repositorys.First(u => u.Coup_Code == txtCouponCode.Text && u.Is_Active == true);
            if(txtAmount.Text!="0" || txtPercentage.Text != "0") { 
            if (cashbacknames == null)
            {
                    hiddenlbl.Visible = false;
                    Cashback_Coupons cback = new Cashback_Coupons();
                cback.Coup_Code = txtCouponCode.Text;
                cback.Min_Prod_Val = Convert.ToInt16(txtCartAMount.Text);
                string iDate = txtValidfrom.Text;
                DateTime oDate = DateTime.Parse(iDate);
                cback.From_Date = oDate;
                string pdate = txtValidTo.Text;
                DateTime atime = DateTime.Parse(pdate);
                cback.Defaultid = Convert.ToInt32(TextBox3.Text);
                cback.To_Date = atime;
                cback.Coup_Amount = Convert.ToInt64(txtAmount.Text);
                cback.Coup_Percentage = Convert.ToInt64(txtPercentage.Text);
                cback.Total_Times_Usable = Convert.ToInt16(txtUsage.Text);
                cback.Total_Times_Usable_Per_User = Convert.ToInt16(txtUserUsage.Text);
                cback.Min_Qty = Convert.ToInt16(TextBox1.Text);
                cback.Max_Cashback = Convert.ToInt64(TextBox2.Text);
                cback.Cashbackdesc = txtDescription.InnerText;
                cback.TC = txttc.InnerText;
                cback.New_User = chknuser.Checked;
                cback.Is_Active = true;
                cback.Created_Date = DateTime.Now;
                context.Cashback_Coupons.Add(cback);
                context.SaveChanges();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Updated sucessfully');window.location ='Couponslist.aspx';", true);
            }
                else
                {
                    def.Coup_ID = Convert.ToInt32(UID);
                    def.Is_Active = true;
                    repository.Updatedata(def);
                    hiddenlbl.Visible = true;
                    hiddenlbl.InnerHtml = "Coupon already exist";
                }
            }
            else
            {
                hiddenlbl.InnerHtml = "Amount or percentage should be not equal to 0";
                hiddenlbl.Visible = true;
            }
            
        }
        
    }
    }
