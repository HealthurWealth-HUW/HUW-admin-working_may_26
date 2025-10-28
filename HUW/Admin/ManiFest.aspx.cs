using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;

public partial class Admin_ManiFest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
                ManifestData();
                ConverttoPDF();           
        }
    }

    private void ManifestData()
    {
        List<Manifesto> objmanifest = (List<Manifesto>)Session["Data"];
        gdvManifest.DataSource = objmanifest;
        gdvManifest.DataBind();
    }

    private void ConverttoPDF()
    {
        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition", "attachment;filename=Manifest.pdf");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        this.Page.RenderControl(hw);
        StringReader sr = new StringReader(sw.ToString());
        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 30f, 0f);
        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        StyleSheet bodySize = new StyleSheet();
        Hashtable props = new Hashtable();
        props.Add(ElementTags.SIZE, "0.8");
        htmlparser.Style.LoadTagStyle("table", props);
        PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        pdfDoc.Open();
        htmlparser.Parse(sr);
        pdfDoc.Close();
        Response.Write(pdfDoc);
        Response.End();
    }

}