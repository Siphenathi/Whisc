using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace WHSIC.Controllers
{
    public class SimplePdfController : Controller
    {
        // GET: SimplePDF
        public ActionResult Index()
        {
            return View();
        }

        public FileStreamResult CreatePdf()
        {
            var doc = new Document(PageSize.A4, 0, 0, 0, 0);
            var ms = new MemoryStream();


            var writer = PdfWriter.GetInstance(doc, ms);
            //writer.CloseStream = false;

            doc.Open();
            //PdfContentByte cb = writer.DirectContent;
            //cb.AddTemplate(PdfFooter(cb), 30, 1);

            var imagepath = Server.MapPath("~/Images");

            var jpg = Image.GetInstance(imagepath + "/SCAN0008.JPG");
            jpg.ScaleToFit(300f, 150f);
            jpg.Alignment = Element.ALIGN_CENTER;
            jpg.Border = Rectangle.BOX;
            jpg.BorderColor = BaseColor.LIGHT_GRAY;
            jpg.BorderColorTop = BaseColor.WHITE;
            jpg.BorderWidth = 3f;

            doc.Add(jpg);
            doc.Close();

            var file = ms.ToArray();
            var output = new MemoryStream();
            output.Write(file, 0, file.Length);
            output.Position = 0;
            var f = new FileStreamResult(output, "application/pdf");
            return f;
        }
    }
}