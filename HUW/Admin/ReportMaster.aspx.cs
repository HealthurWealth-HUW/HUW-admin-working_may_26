using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using BAL;

using Utility;
using System.Globalization;

public partial class Admin_ReportMaster : System.Web.UI.Page
    {
    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["db_Zon_constr"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindMonth();
            BindYear();
        }
    }
    void BindMonth()
    {
        for (int month = 1; month <= 12; month++)
        {
            string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
            ddlMonth.Items.Add(new ListItem(monthName, month.ToString().PadLeft(2, '0')));
        }
    }
    void BindYear()
    {
        for (int i = 2010; i <= 2025; i++)
        {
            ddlyear.Items.Add(i.ToString());
        }
    }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            Response.Redirect("FileUload.aspx");
        }

    protected void btnDownload_Click(object sender, EventArgs e)
    {

        string fromdate = ddlMonth.SelectedValue+ '/'+ ddlyear.SelectedValue;
        DateTime firstDay = DateTime.Parse(fromdate);
        string monthyear = firstDay.ToString("MMM  yyyy");//Feb2022

        var lastDay = firstDay.AddMonths(1);
        SqlCommand cmd = new SqlCommand("GET_Final_REPORT", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@fromdate", firstDay);
        cmd.Parameters.AddWithValue("@todate", lastDay);
        cmd.Parameters.AddWithValue("@mothyear", monthyear);

        SqlDataAdapter sda = new SqlDataAdapter(cmd);

        DataSet ds = new DataSet();
        sda.Fill(ds);
        //string excelfile = Convert.ToDateTime(ddlMonth.SelectedValue).ToString("dd-MM-yyyy");
        if (ds.Tables.Count > 0)
        {
            string attachment = "attachment; filename=Test.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            string tab = "";
            foreach (DataTable dt in ds.Tables)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    Response.Write(tab+ dc.ColumnName  );
                    tab = "\t";
                }
                Response.Write("\n");
                int i;
                foreach (DataRow dr in dt.Rows)
                {
                    tab = "";
                    for (i = 0; i < dt.Columns.Count; i++)
                    {
                        Response.Write(tab + dr[i].ToString());
                        tab = "\t";
                    }
                    Response.Write("\n");
                }
                Response.Write("\n");
            }
            Response.End();
        }
    }

    protected void DDlstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        //bindPin();
    }
}