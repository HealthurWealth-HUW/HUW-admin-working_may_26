using BAL;
using DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_AddAWB : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
        
        }
    }

    //protected void btnSubmit_Click(object sender, EventArgs e)
    //{

    //}
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (fileAWB.PostedFile != null)
        {
            try
            {
                string folderPath = Server.MapPath("~/UploadFiles/AWB/");
                string path = folderPath+Guid.NewGuid() + Path.GetFileName(fileAWB.FileName);
                fileAWB.SaveAs(path);
                //string path = string.Concat(Server.MapPath("~/UploadFiles/AWB/" + Guid.NewGuid() + fileAWB.FileName));
                //fileAWB.SaveAs(path);
                // Connection String to Excel Workbook  
                string excelCS = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0", path);
                string constr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;""", path);
                OleDbConnection Econ = new OleDbConnection(constr);
                string Query = string.Format("Select [AWB_Number],[PaymentMode],[OrderId] FROM [{0}]", "Sheet1$");
                OleDbCommand Ecom = new OleDbCommand(Query, Econ);
                Econ.Open();

                DataSet ds = new DataSet();
                OleDbDataAdapter oda = new OleDbDataAdapter(Query, Econ);
                Econ.Close();
                oda.Fill(ds);
                DataTable Exceldt = ds.Tables[0];

                for (int i = Exceldt.Rows.Count - 1; i >= 0; i--)
                {
                    if (Exceldt.Rows[i]["AWB_Number"] == DBNull.Value || Exceldt.Rows[i]["PaymentMode"] == DBNull.Value || Exceldt.Rows[i]["OrderId"] == DBNull.Value)
                    {
                        Exceldt.Rows[i].Delete();
                    }
                    else
                    {
                        db_Zon_HuwEntities context = new db_Zon_HuwEntities();
                        string awb = Exceldt.Rows[i]["AWB_Number"].ToString().Trim();
                        if (context.Tbl_AWB.Where(x=>x.AWB_Number==awb&&x.Deliverytype==null).Count() != 0)
                        {
                            Exceldt.Rows[i].Delete();
                        }
                    }
                }
                Exceldt.AcceptChanges();
                string CS = ConfigurationManager.ConnectionStrings["db_Zon_constr"].ConnectionString;
                //string CS = "data source=184.168.194.70;initial catalog=db_Zon_Huw;user id=Healthurwealth;password=Zon@HUW2017";
                SqlConnection con = new SqlConnection(CS);
                //creating object of SqlBulkCopy      
                SqlBulkCopy objbulk = new SqlBulkCopy(con);
                //assigning Destination table name      
                objbulk.DestinationTableName = "[Healthurwealth].[Tbl_AWB]";
                //Mapping Table column    
                objbulk.ColumnMappings.Add("AWB_Number", "AWB_Number");
                objbulk.ColumnMappings.Add("OrderId", "OrderId");
                objbulk.ColumnMappings.Add("PaymentMode", "PaymentMode");
                con.Open();
                objbulk.WriteToServer(Exceldt);
                con.Close();
                lblMessage.InnerText = "Uploaded Successfully";
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = ex.Message.ToString();
            }
        }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        Response.ContentType = "application/xls";
        Response.AppendHeader("Content-Disposition", "attachment; filename=AWB_UploadExcel_Format.xls");
        Response.TransmitFile(Server.MapPath("~/UploadFiles/AWB/AWB_ExcelFormat/AWB_UploadExcel_Format.xls"));
        Response.End();
    }
} 
