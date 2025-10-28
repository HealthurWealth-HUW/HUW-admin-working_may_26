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

public partial class Admin_FileUload : System.Web.UI.Page
    {
        private OleDbConnection Econ;
        private string constr;
        private string Query;
        private string Count;
    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["db_Zon_constr"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                binddocument();
                bindPdf();
            }
        }

        void binddocument()
        {
        var repository = new DocumnetMonthRepository();
        rptDetailsTbl.DataSource = repository.GetDocuemnts();
        rptDetailsTbl.DataBind();
    }
        void bindPdf()
        {
        var repository = new DocumnetPdfRepository();
        rptDetailsTbl1.DataSource = repository.GetPdfDocuemnts();
        rptDetailsTbl1.DataBind();
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
                string excelPath = base.Server.MapPath(string.Concat("~/Admin/Files/", this.fupload.FileName));
                fupload.SaveAs(excelPath);
                base.Response.Write(string.Concat("path=", excelPath));
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
                    this.constr = string.Concat("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=", excelPath, ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"");
                    this.Econ = new OleDbConnection(this.constr);
                }
                constr = string.Format(constr, excelPath);
                using (OleDbConnection excel_con = new OleDbConnection(constr))
                {
                    excel_con.Open();
                    string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
                    DataTable dtExcelData = new DataTable();
                    dtExcelData.Columns.AddRange(new DataColumn[8] { new DataColumn("Order_id", typeof(string)),
                new DataColumn("waybill_num", typeof(string)),
                new DataColumn("package_type", typeof(string)),
                new DataColumn("status", typeof(string)),
                new DataColumn("total_amount", typeof(string)),
                new DataColumn("dpin", typeof(string)),
                new DataColumn("rpin", typeof(string)),
                new DataColumn("items_shipped", typeof(string))});

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
                        sqlBulkCopy.DestinationTableName = "dbo.Temp_File_Data";
                        sqlBulkCopy.ColumnMappings.Add("order_id", "Order_id");
                        sqlBulkCopy.ColumnMappings.Add("waybill_num", "waybill_no");
                        sqlBulkCopy.ColumnMappings.Add("package_type", "package_type");
                        sqlBulkCopy.ColumnMappings.Add("status", "status");
                        sqlBulkCopy.ColumnMappings.Add("total_amount", "Sum");
                        sqlBulkCopy.ColumnMappings.Add("dpin", "D_Pin");
                        sqlBulkCopy.ColumnMappings.Add("rpin", "R_Pin");
                        sqlBulkCopy.ColumnMappings.Add("items_shipped", "Item_Shipped");
                        sqlBulkCopy.ColumnMappings.Add("FileName", "File_Name");
                        conn.Open();
                        sqlBulkCopy.WriteToServer(dtExcelData);
                        conn.Close();
                    }

                }
            string Filename = fupload.FileName;
            var repository = new DocumentNameRepository();
            var dt11 = repository.GetName(Filename);
            if (dt11.Count > 0)
                {
                    Response.Write("<script>alert('Sorry! this File Alrady Exists.');</script> ");
                }
                else
                {
                //string Currentdate = DateTime.Now.ToString("MMM  yyyy");

                DateTime date = Convert.ToDateTime(exceluploadeddate.Text);
                string Currentdate = date.ToString("MMM yyyy");
                conn.Open();
                SqlCommand cmd1 = new SqlCommand();
                cmd1.Connection = conn;
                cmd1.CommandType = CommandType.Text;
                cmd1.CommandText = "INSERT INTO Delivery_Invoice_Excel_Upload_Details(File_Date,File_Name) values( '" + Currentdate + "', (SELECT DISTINCT File_Name FROM Temp_File_Data WHERE File_Name NOT IN(SELECT File_Name FROM Delivery_Invoice_Excel_Upload_Details)))";
                int rowsAffected = cmd1.ExecuteNonQuery();
                conn.Close();
                if (rowsAffected == 1)
                {
                    Response.Write("<script>alert('added successfully...');</script> ");
                }
                else
                {
                    Response.Write("<script>alert('Something wrong with File adding... please try again');</script> "); 
                }
            }

                SqlCommand cmd = new SqlCommand("[INSERT_UPDATE_DELIVERY_INVOICE_EXCEL]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                int aaa = cmd.ExecuteNonQuery();
                conn.Close();
                if (aaa > 0)
                {
                    Response.Write("<script>alert('Record Updated successfull');</script>");
                }

                Response.Redirect("RecordMaster.aspx");
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Sorry!!');</script>");
            }
        }

        protected void btnpdf_Click(object sender, EventArgs e)
        {
            if (!this.Pdfuload.HasFile)
            {
                Response.Write("<script>alert('Please Select file for upload.');</script> ");
                return;
            }
            else
            {
            string Pdfname = Pdfuload.FileName;
            var repository = new PdfNameRepository();
            var dt11 = repository.GetPdfName(Pdfname);
            if (dt11.Count > 0)
                {
                    Response.Write("<script>alert('Sorry! this PDF Alrady Exists.');</script> ");
                }
                else
                {
                DateTime date = Convert.ToDateTime(pdfuploadeddate.Text);
                string Currentdate = date.ToString("MMM yyyy");
                string str = base.Server.MapPath(string.Concat("~/Admin/Files/", this.Pdfuload.FileName));
                    this.Pdfuload.SaveAs(str);
                conn.Open();
                SqlCommand cmd1 = new SqlCommand();
                cmd1.Connection = conn;
                cmd1.CommandType = CommandType.Text;
                cmd1.CommandText = "INSERT INTO Delivery_Invoice_Pdf(Pdf_Name,Pdf_Date) values( '" + Pdfuload.FileName + "','" + Currentdate + "')";
                int rowsAffected = cmd1.ExecuteNonQuery();
                conn.Close();
                if (rowsAffected == 1)
                {
                    Response.Write("<script>alert('added successfully...');</script> ");
                    bindPdf();
                }
                else
                {
                    Response.Write("<script>alert('Something wrong with File adding... please try again');</script> ");
                }
            }
            }


        }

        protected void btnback_Click(object sender, EventArgs e)
        {
            Response.Redirect("RecordMaster.aspx");
        }
    }
