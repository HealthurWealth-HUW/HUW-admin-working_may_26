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
using DAL;
using Utility;

public partial class Admin_Medicine_Excell_Upload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (fileAWB.PostedFile != null)
        {
            //try
            //{
            string folderPath = Server.MapPath("~/UploadFiles");
            string path = folderPath + Guid.NewGuid() + Path.GetFileName(fileAWB.FileName);
            fileAWB.SaveAs(path);
            
            // Connection String to Excel Workbook  
            string excelCS = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0", path);
            string constr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;""", path);
            OleDbConnection Econ = new OleDbConnection(constr);
            //string Query = string.Format("Select [AWB_Number],[PaymentMode],[OrderId] FROM [{0}]", "Sheet1$");
            string Query = string.Format("Select * FROM [{0}]", "Sheet1$");
            OleDbCommand Ecom = new OleDbCommand(Query, Econ);
            Econ.Open();

            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(Query, Econ);
            Econ.Close();
            oda.Fill(ds);
            DataTable Exceldt = ds.Tables[0];


            for (int i = 0; i <= Exceldt.Rows.Count - 2; i++)
            {
                db_Zon_HuwEntities context = new db_Zon_HuwEntities();


                Medicines_tbl med_obj = new Medicines_tbl();

                med_obj.ItemID = Exceldt.Rows[i]["ItemID"].ToString().Trim();
                med_obj.company = Exceldt.Rows[i]["Company"].ToString().Trim();
                med_obj.Item_Code = Exceldt.Rows[i]["ItemCode"].ToString().Trim();
                med_obj.Med_Name = Exceldt.Rows[i]["Name"].ToString().Trim();
                med_obj.HSNCode = Exceldt.Rows[i]["HSNCode"].ToString().Trim();
                med_obj.LocalTax = Exceldt.Rows[i]["LocalTax"].ToString().Trim();
                if (Exceldt.Rows[i]["SGST"].ToString().Trim() != "")
                {
                    med_obj.SGST = Convert.ToDecimal(Exceldt.Rows[i]["SGST"].ToString().Trim());
                }
                else
                {
                    med_obj.SGST = null;
                }
                if (Exceldt.Rows[i]["CGST"].ToString().Trim() != "")
                {
                    med_obj.CGST = Convert.ToDecimal(Exceldt.Rows[i]["CGST"].ToString().Trim());
                }
                else
                {
                    med_obj.CGST = null;
                }
                if (Exceldt.Rows[i]["IGST"].ToString().Trim() != "")
                {
                    med_obj.IGST = Convert.ToDecimal(Exceldt.Rows[i]["IGST"].ToString().Trim());
                }
                else
                {
                    med_obj.IGST = null;
                }
                if (Exceldt.Rows[i]["Rate"].ToString().Trim() != "")
                {
                    med_obj.Rate = Convert.ToDecimal(Exceldt.Rows[i]["Rate"].ToString().Trim());
                }
                else
                {
                    med_obj.Rate = null;
                }
                if (Exceldt.Rows[i]["MRP"].ToString().Trim() != "")
                {
                    med_obj.MRP = Convert.ToDecimal(Exceldt.Rows[i]["MRP"].ToString().Trim());
                }
                else
                {
                    med_obj.MRP = null;
                }
                if (Exceldt.Rows[i]["Stock"].ToString().Trim() != "")
                {
                    var x = Exceldt.Rows[i]["Stock"].ToString().Trim();
                    med_obj.stock = Convert.ToDecimal(Exceldt.Rows[i]["Stock"].ToString().Trim());
                }
                else
                {
                    med_obj.stock = null;
                }

                context.Medicines_tbl.Add(med_obj);
                context.SaveChanges();
            }
        }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        Response.ContentType = "application/xls";
        Response.AppendHeader("Content-Disposition", "attachment; filename=AWB_UploadExcel_Format.xls");
        Response.TransmitFile(Server.MapPath("~/UploadFiles/AWB_UploadExcel_Format.xls"));
        Response.End();
    }
    protected void pending_records_bind(object sender, EventArgs e)
    {
    }
}
