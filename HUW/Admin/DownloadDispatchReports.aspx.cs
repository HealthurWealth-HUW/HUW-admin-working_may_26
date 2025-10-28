using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_DownloadDispatchReports : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public void DownloadExcel(DateTime fromdate, DateTime todate)
    {
        todate = todate.AddDays
            (1);
        string constr = ConfigurationManager.ConnectionStrings["db_Zon_constr"].ConnectionString;
        //string query = "SELECT tpt.Payment_Transaction_Id, case when tpt.Delivered=1 then 'Deliverd' ELSE 'NOT' end as Delivered ,tui.Email_Id ,tui.Phone_Number ,tpi.Product_Name ,tsc.Super_Category_Name , tc.Category_Name ,tsubc.Sub_Category_Name ,tupt.Quantity,tupt.Product_Cost, (tupt.Quantity*tupt.Product_Cost) as cost , tpt.Txn_Status ,tpt.Payment_Mode , tpt.PGTxn_Id FROM [Tbl_Payment_Transactions] tpt join Tbl_User_Product_Transactions tupt on tpt.Payment_Transaction_Id =tupt.Payment_Transaction_Id join Tbl_Product_Info tpi on tpi.Product_Id =tupt.Product_Id left join Tbl_Super_Categories tsc on tsc.Super_Category_Id =tpi.Super_Category_Id left join Tbl_Categories tc on tc.Category_Id =tpi.Category_Id left join Tbl_Sub_Categories tsubc on tsubc.Sub_Category_Id =tpi.Sub_Category_Id join Tbl_User_Info tui on tui.User_Id =tpt.User_Id where tpt.Payment_Transaction_Id > "+ Orderid + "";
        //query += "SELECT tpt.Payment_Transaction_Id, case when tpt.Delivered=1 then 'Deliverd' ELSE 'NOT' end as Delivered ,tui.Email_Id ,tui.Phone_Number ,tui.User_Name ,TPT.Txn_Date, tpt.Txn_Status ,TPT.Txn_Amount,TPT.Payment_Mode , tpt.PGTxn_Id ,tpt.Txn_Amount ,tua.Address,Locality, LandMark,Pincode FROM [Tbl_Payment_Transactions] tpt join Tbl_User_Info tui on tui.User_Id =tpt.User_Id join Tbl_User_Address tua on tua.Address_Id =tpt.Address_Id where tpt.Payment_Transaction_Id >"+ Orderid + "";
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("Sp_DispatchedReports"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@startdate", SqlDbType.VarChar).Value = fromdate;
                cmd.Parameters.Add("@enddate", SqlDbType.VarChar).Value = todate;

                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);

                        //Set Name of DataTables.
                        ds.Tables[0].TableName = "DispatchOrders Single Product";
                        ds.Tables[1].TableName = "DispatchOrders Multiple Product";
                        ds.Tables[2].TableName = "DispatchedReturns";


                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            foreach (DataTable dt in ds.Tables)
                            {
                                //Add DataTable as Worksheet.
                                wb.Worksheets.Add(dt);
                            }

                            //Export the Excel file.
                            Response.Clear();
                            Response.Buffer = true;
                            Response.Charset = "";
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.AddHeader("content-disposition", "attachment;filename=Healthurwealthdispatchreport.xlsx");
                            using (MemoryStream MyMemoryStream = new MemoryStream())
                            {
                                wb.SaveAs(MyMemoryStream);
                                MyMemoryStream.WriteTo(Response.OutputStream);
                                Response.Flush();
                                Response.End();
                            }
                        }
                    }
                }
            }
        }
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        DownloadExcel(Convert.ToDateTime(txtFromDate.Value), Convert.ToDateTime(txtToDate.Value));
    }
}