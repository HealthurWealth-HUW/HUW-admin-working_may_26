using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using BAL;
using Utility;
using System.Text;
using ClosedXML.Excel;
using System.Drawing;

public partial class Admin_DeliveryReport : System.Web.UI.Page
{
    private OleDbConnection Econ;
    private string constr;
    private string Query;
    private string Count;
    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["db_Zon_constr"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            if (!this.fupload.HasFile)
            {
                Response.Write("<script>alert('Please Select file for upload.');</script> ");
                return;
            }
            string excelPath = base.Server.MapPath(string.Concat("~/Admin/PDF-Files/", this.fupload.FileName));
            fupload.SaveAs(excelPath);
            //base.Response.Write(string.Concat("path=", excelPath));
            string lower = Path.GetExtension(this.fupload.FileName).ToLower();
            string fileName = this.fupload.PostedFile.FileName;
            if (lower.Trim() == ".xls")
            {
                this.constr = string.Concat("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=", excelPath, ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"");
                this.Econ = new OleDbConnection(this.constr);
                return;
            }
            if (lower.Trim() == ".xlsx")
            {
                //this.constr = string.Concat("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=", excelPath, ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"");
                this.constr = string.Concat("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=", excelPath, ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"");
                this.Econ = new OleDbConnection(this.constr);
            }
            constr = string.Format(constr, excelPath);
            using (OleDbConnection excel_con = new OleDbConnection(constr))
            {
                excel_con.Open();
                string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
                DataTable dtExcelData = new DataTable();
                dtExcelData.Columns.AddRange(new DataColumn[37] { new DataColumn("Order_id", typeof(string)),
                new DataColumn("Items_shipped", typeof(string)),
                new DataColumn("Waybill_num", typeof(string)),
                new DataColumn("Pickup_date", typeof(string)),
                new DataColumn("Package_type", typeof(string)),
                new DataColumn("Product_value", typeof(string)),
                new DataColumn("Status", typeof(string)),
                new DataColumn("Zone", typeof(string)),
                new DataColumn("Charged_weight", typeof(string)),
                new DataColumn("Gross_amount", typeof(string)),
                new DataColumn("charge_POD", typeof(string)),
                new DataColumn("charge_COVID", typeof(string)),
                new DataColumn("charge_FSC", typeof(string)),
                new DataColumn("charge_DL", typeof(string)),
                new DataColumn("charge_RTO", typeof(string)),
                new DataColumn("charge_DTO", typeof(string)),
                new DataColumn("charge_COD", typeof(string)),
                new DataColumn("charge_FS", typeof(string)),
                new DataColumn("charge_FOV", typeof(string)),
                new DataColumn("charge_CCOD", typeof(string)),
                new DataColumn("charge_WOD", typeof(string)),
                new DataColumn("charge_AIR", typeof(string)),
                new DataColumn("charge_pickup", typeof(string)),
                new DataColumn("charge_DEMUR", typeof(string)),
                new DataColumn("charge_LABEL", typeof(string)),
                new DataColumn("charge_REATTEMPT", typeof(string)),
                new DataColumn("charge_DOCUMENT", typeof(string)),
                new DataColumn("charge_ROV", typeof(string)),
                new DataColumn("IGST", typeof(string)),
                new DataColumn("CGST", typeof(string)),
                new DataColumn("SGST_UGST", typeof(string)),
                new DataColumn("total_amount", typeof(string)),
                new DataColumn("destination_pin", typeof(string)),
                new DataColumn("opin", typeof(string)),
                new DataColumn("dpin", typeof(string)),
                new DataColumn("rpin", typeof(string)),
                new DataColumn("date", typeof(string))});

                using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", excel_con))
                {
                    oda.Fill(dtExcelData);
                    System.Data.DataColumn newColumn = new System.Data.DataColumn("FileName", typeof(System.String));
                    newColumn.DefaultValue = fupload.FileName;
                    dtExcelData.Columns.Add(newColumn);
                }
                excel_con.Close();
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(conn))
                {
                    sqlBulkCopy.DestinationTableName = "Temp_Deliver_Excel_Record";
                    sqlBulkCopy.ColumnMappings.Add("Order_id", "Order_id");
                    sqlBulkCopy.ColumnMappings.Add("Items_shipped", "Items_shipped");
                    sqlBulkCopy.ColumnMappings.Add("Waybill_num", "Waybill_num");
                    sqlBulkCopy.ColumnMappings.Add("Pickup_date", "Pickup_date");
                    sqlBulkCopy.ColumnMappings.Add("Package_type", "Package_type");
                    sqlBulkCopy.ColumnMappings.Add("Product_value", "Product_value");
                    sqlBulkCopy.ColumnMappings.Add("Status", "Status");
                    sqlBulkCopy.ColumnMappings.Add("Zone", "Zone");
                    sqlBulkCopy.ColumnMappings.Add("Charged_weight", "Charged_weight");
                    sqlBulkCopy.ColumnMappings.Add("Gross_amount", "Gross_amount");
                    sqlBulkCopy.ColumnMappings.Add("charge_POD", "charge_POD");
                    sqlBulkCopy.ColumnMappings.Add("charge_COVID", "charge_COVID");
                    sqlBulkCopy.ColumnMappings.Add("charge_FSC", "charge_FSC");
                    sqlBulkCopy.ColumnMappings.Add("charge_DL", "charge_DL");
                    sqlBulkCopy.ColumnMappings.Add("charge_RTO", "charge_RTO");
                    sqlBulkCopy.ColumnMappings.Add("charge_DTO", "charge_DTO");
                    sqlBulkCopy.ColumnMappings.Add("charge_COD", "charge_COD");
                    sqlBulkCopy.ColumnMappings.Add("charge_FS", "charge_FS");
                    sqlBulkCopy.ColumnMappings.Add("charge_FOV", "charge_FOV");
                    sqlBulkCopy.ColumnMappings.Add("charge_CCOD", "charge_CCOD");
                    sqlBulkCopy.ColumnMappings.Add("charge_WOD", "charge_WOD");
                    sqlBulkCopy.ColumnMappings.Add("charge_AIR", "charge_AIR");
                    sqlBulkCopy.ColumnMappings.Add("charge_pickup", "charge_pickup");
                    sqlBulkCopy.ColumnMappings.Add("charge_DEMUR", "charge_DEMUR");
                    sqlBulkCopy.ColumnMappings.Add("charge_LABEL", "charge_LABEL");
                    sqlBulkCopy.ColumnMappings.Add("charge_REATTEMPT", "charge_REATTEMPT");
                    sqlBulkCopy.ColumnMappings.Add("charge_DOCUMENT", "charge_DOCUMENT");
                    sqlBulkCopy.ColumnMappings.Add("charge_ROV", "charge_ROV");
                    sqlBulkCopy.ColumnMappings.Add("IGST", "IGST");
                    sqlBulkCopy.ColumnMappings.Add("CGST", "CGST");
                    sqlBulkCopy.ColumnMappings.Add("SGST_UGST", "SGST_UGST");
                    sqlBulkCopy.ColumnMappings.Add("total_amount", "total_amount");
                    sqlBulkCopy.ColumnMappings.Add("destination_pin", "destination_pin");
                    sqlBulkCopy.ColumnMappings.Add("opin", "opin");
                    sqlBulkCopy.ColumnMappings.Add("dpin", "dpin");
                    sqlBulkCopy.ColumnMappings.Add("rpin", "rpin");
                    sqlBulkCopy.ColumnMappings.Add("date", "date");
                    conn.Open();
                    sqlBulkCopy.WriteToServer(dtExcelData);
                    conn.Close();
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

   
    protected void btninvidexcel_Click(object sender, EventArgs e)
    {
        string constr = ConfigurationManager.ConnectionStrings["db_Zon_constr"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("select Tder.*,Tid.Lengths,Tid.Width,Tid.Height,Tid.V_weight,Tid.Actual_Weight from Temp_Deliver_Excel_Record as Tder inner join Tbl_InpackingBox_Details as Tid on Tder.Order_id=Tid.OrderId"))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            wb.Worksheets.Add(dt, "Temp_Deliver_Excel_Record");

                            Response.Clear();
                            Response.Buffer = true;
                            Response.Charset = "";
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.AddHeader("content-disposition", "attachment;filename=DeliveryReport.xlsx");
                            using (MemoryStream MyMemoryStream = new MemoryStream())
                            {
                                wb.SaveAs(MyMemoryStream);
                                MyMemoryStream.WriteTo(Response.OutputStream);
                                Response.Flush();
                                CrearData();
                                Response.End();
                            }
                        }
                    }
                }
            }
        }
    }

    public void CrearData()
    {
        string constr = ConfigurationManager.ConnectionStrings["db_Zon_constr"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd1 = new SqlCommand("truncate table Temp_Deliver_Excel_Record"))
            {
                using (SqlDataAdapter sda1 = new SqlDataAdapter())
                {
                    cmd1.Connection = con;
                    sda1.SelectCommand = cmd1;
                    DataTable dt1 = new DataTable();
                    sda1.Fill(dt1);
                }
            }
        }
    }
}