using BAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;

public partial class Admin_Medicineorderspending : System.Web.UI.Page
{
    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["db_Zon_constr"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand("INSERT_SM_TEMP_REPORT", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@fromdate", txtfromdate.Text);
        cmd.Parameters.AddWithValue("@todate", txttodate.Text);
        SqlDataAdapter sda = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        sda.Fill(dt);
        string excelfile = Convert.ToDateTime(txtfromdate.Text).ToString("dd-MM-yyyy") + " - " + Convert.ToDateTime(txttodate.Text).ToString("dd-MM-yyyy");
        //var repository = new ReportRepository();
        //var dt = ToDataTable.LINQResultToDataTable<SM_Temp_Report>(repository.GetReport().AsEnumerable());
        if (dt.Rows.Count > 0)
            { 
                Response.ClearContent(); 
                string FileName = excelfile+".xls";
                Response.AddHeader("content-disposition", string.Format("attachment; filename=" + FileName));
                Response.ContentType = "application/vnd.ms-excel";
                string space = "";
                foreach (DataColumn dcolumn in dt.Columns)
                {
                    Response.Write(space + dcolumn.ColumnName);
                    space = "\t";
                }
                Response.Write("\n");
                int countcolumn;
                foreach (DataRow dr in dt.Rows)
                {
                    space = "";
                    for (countcolumn = 0; countcolumn < dt.Columns.Count; countcolumn++)
                    {
                        Response.Write(space + dr[countcolumn].ToString());
                        space = "\t";
                    }
                    Response.Write("\n");
                }
                Response.End();
            }
    }
}