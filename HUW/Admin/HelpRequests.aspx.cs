using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;

public partial class Admin_HelpRequests : System.Web.UI.Page
{
    db_Zon_HuwEntities Entity = new db_Zon_HuwEntities();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDetails();
            BindComments();
        }
    }
    public void BindDetails()
    {
        if (!string.IsNullOrEmpty(Request.QueryString["ID"].ToString()))
        {
            helpId.InnerText = Request.QueryString["ID"].ToString();
            if (!string.IsNullOrEmpty(Request.QueryString["Name"].ToString()))
                personName.InnerText = Request.QueryString["Name"].ToString();
            if (!string.IsNullOrEmpty(Request.QueryString["Email"].ToString()))
                personEmail.InnerText = Request.QueryString["Email"].ToString();
            if (!string.IsNullOrEmpty(Request.QueryString["Phone"].ToString()))
                phone.InnerText = Request.QueryString["Phone"].ToString();
            if (!string.IsNullOrEmpty(Request.QueryString["Status"].ToString()))
            {
                ddlStatus.Items.FindByValue(Request.QueryString["Status"].ToString()).Selected = true;
            }
        }
        else
            Response.Redirect("NeedHelpRequests.aspx");
    }
    public void BindComments()
    {
        if (!string.IsNullOrEmpty(helpId.InnerText))
        {
            long hId = Convert.ToInt64(helpId.InnerText);
            List<Tbl_Need_Help_Comments> need_Help_Comments = Entity.Tbl_Need_Help_Comments.Where(x => x.Help_Id == hId).ToList();
            if (need_Help_Comments.Count() != 0 && need_Help_Comments != null)
            {
                StringBuilder str = new StringBuilder();
                str.Append(@"<h4 style='font-size: 18px;'>Replies</h4>");
                foreach (var item in need_Help_Comments)
                {
                    str.Append(@"<div class='bg-box clearfix'>
                       <div class='col-md-2 text-center'>
                            <img src='http://www.myiconfinder.com/uploads/iconsets/256-256-5644a46a81b2b9ab1557a2d7064680f5-user.png' class='img-responsive' alt='image' />
                            Admin
                        </div>
                        <div class='col-md-10 row'><div><p class='text-right' style='font-size: 12px; color: #16579c;font-weight: bold; margin-bottom:0'><i class='fa fa-calendar'></i> " + string.Format("{0:MMM dd yyyy}", item.Comment_Date) + @"</p></div>" + item.Comment + @"</div>
                    </div>");
                }
                divComment.InnerHtml = str.ToString();
                str.Clear();
            }
        }
    }
    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(helpId.InnerText))
        {
            long hId = Convert.ToInt64(helpId.InnerText);
            int st = Convert.ToInt32(ddlStatus.SelectedItem.Value);
            Tbl_Need_Help _Need_Help = Entity.Tbl_Need_Help.Where(x => x.Help_Id == hId).FirstOrDefault();
            _Need_Help.Status =st;
            int i = Entity.SaveChanges();
            if (i != 0)
            {
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("Status", st.ToString());
                string url = Request.Url.AbsolutePath;
                Response.Redirect(url + "?" + nameValues);
                //BindDetails();
                //if (st == 1)
                //    helpStatus.InnerText = "Pending";
                //else
                //    helpStatus.InnerText = "Closed";
                //ddlStatus.Items.FindByValue(st.ToString()).Selected = true;
            }
        }
    }

    protected void btnComment_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(helpId.InnerText) && !string.IsNullOrEmpty(txComment.InnerText))
        {
            long hId = Convert.ToInt64(helpId.InnerText);
            Tbl_Need_Help_Comments _Need_Help_Comments = new Tbl_Need_Help_Comments();
            _Need_Help_Comments.Comment = txComment.InnerText;
            _Need_Help_Comments.Help_Id = hId;
            _Need_Help_Comments.Comment_Date = DateTime.Now;
            _Need_Help_Comments.Status = true;
            Entity.Tbl_Need_Help_Comments.Add(_Need_Help_Comments);
            int i = Entity.SaveChanges();
            BindComments();
        }
    }
}