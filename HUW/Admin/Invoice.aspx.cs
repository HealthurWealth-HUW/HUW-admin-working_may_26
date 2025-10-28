using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using BAL;

public partial class Admin_Invoice : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int trnsid = Convert.ToInt32(Request.QueryString["id"]);
        var result = BalUtility.GetInvoice(trnsid);
        // Create a Document object
        var document = new Document(PageSize.A4, 50, 50, 45, 25);

        // Create a new PdfWrite object, writing the output to a MemoryStream
        var output = new MemoryStream();
        var writer = PdfWriter.GetInstance(document, output);

        // Open the Document for writing
        document.Open();

        //contents = contents.Replace("[ITEMS]", itemsTable);

        var parsedHtmlElements = HTMLWorker.ParseToList(new StringReader(result.Result.ToString()), null);
        foreach (var htmlElement in parsedHtmlElements)
            document.Add(htmlElement as IElement);

        //   You can add additional elements to the document. Let's add an image in the upper right corner



        var logo = iTextSharp.text.Image.GetInstance(Server.MapPath("../skin/frontend/default/megashop-pink/images/HUWLogo2015.png"));
        logo.SetAbsolutePosition(40, 750);
        logo.ScaleAbsolute(150f,60f);
        document.Add(logo);

        document.Close();

        Response.ContentType = "application/pdf";
        Response.AddHeader("Content-Disposition", string.Format("attachment;filename=Receipt-{0}.pdf", result.Message));
        Response.BinaryWrite(output.ToArray());
    }
}