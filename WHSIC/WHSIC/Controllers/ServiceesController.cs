using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WHSIC.Data;
using WHSIC.Model;
using WHSIC.BusinessLogic;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;

namespace WHSIC.Controllers
{
    public class ServiceesController : Controller
    {
        BaseFont f_cb = BaseFont.CreateFont("c:\\windows\\fonts\\calibrib.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        BaseFont f_cn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

        InternetTime it = new InternetTime();
        ServiceeBusiness sb = new ServiceeBusiness();
        ServiceTypeBusiness st = new ServiceTypeBusiness();

        public List<ServiceTypeViewModel>sType()
        {
            List<ServiceTypeViewModel> typeList = (from s in st.GetAllServiceType() where s.dismiss == true select s).ToList();
            return typeList;
        }

        [Authorize(Roles = "Treasure")]
        public ActionResult Index()
        {
            return View(sb.GetAllService());
        }

        [Authorize]
        public ActionResult Create()
        {
            ViewBag.feed = null;
            ViewBag.m = new SelectList(sType(), "TypeID", "Description");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServiceeViewModel servicee)
        {
            ViewBag.m = new SelectList(sType(), "TypeID", "Description");
            if (ModelState.IsValid)
            {
                string email = User.Identity.Name;
                if (servicee.Quantity < 1)
                {
                    ViewBag.feed = "Invalid quantity, enter correct quantity..!";
                }
                else
                {
                    DateTime newdate = Convert.ToDateTime(servicee.Date.ToShortDateString());
                    DateTime netdate = Convert.ToDateTime(it.GetNistTime().ToShortDateString());

                    if (newdate < netdate)
                    {
                        ViewBag.feed = "Date entered has passed, enter correct date..!";
                    }
                    else
                    {
                        servicee.email = email;
                        servicee.Status = false;
                        sb.AddService(servicee);
                        ModelState.Clear();
                        ViewBag.feed = "Your service is successfully saved, Thank you for your service..!";
                        return RedirectToAction("GetAllMyDonations","ViewMyDonations" );
                    }
                }
            }
            ViewBag.m = new SelectList(sType(), "TypeID", "Description");

            return View();
        }
        [Authorize(Roles = "Treasure")]
        public ActionResult Edit(int? id)
        {
            ViewBag.m = new SelectList(sType(), "TypeID", "Description");
            ViewBag.feed = null;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceeViewModel servicee = sb.GetbyID(Convert.ToInt32(id));
            if (servicee == null)
            {
                return HttpNotFound();
            }
            ViewBag.name = servicee.email;
            ViewBag.m = new SelectList(sType(), "TypeID", "Description");
            return View(servicee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ServiceeViewModel servicee,FormCollection form)
        {
            ViewBag.m = new SelectList(sType(), "TypeID", "Description");
            if (ModelState.IsValid)
            {
                servicee.email = form["txtname"];
                if(servicee.Status==true)
                {
                    sb.UpdateService(servicee);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.feed = "Service is not updated, change service status..!!";
                }
               
            }
            ViewBag.name = form["txtname"];
            ViewBag.m = new SelectList(sType(), "TypeID", "Description");
            return View(servicee);
        }


        public FileStreamResult PrintService(int id)
        {

            ServiceeViewModel serv = sb.GetbyID(id);
            ServiceTypeViewModel type = st.GetbyID(serv.TypeID);
            

            string imagepath = Server.MapPath("~/Images");


            Document doc = new Document(PageSize.A4, 0, 0, 0, 0);
            var ms = new MemoryStream();

            PdfWriter writer = PdfWriter.GetInstance(doc, ms);

            doc.Open();
            PdfContentByte cb = writer.DirectContent;
            cb.AddTemplate(PdfFooterExt(cb), 30, 1);

            Font address_font = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, Font.ITALIC, BaseColor.BLACK);
            //Text Settings
            string text = "Payment Details";
            Font georgia = FontFactory.GetFont("georgia", 24f);
            georgia.Color = BaseColor.BLACK;
            Chunk beginning = new Chunk(text, georgia);




            Font font_heading_3 = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 16, Font.BOLD, BaseColor.MAGENTA);
            Font font_body = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 13, Font.BOLDITALIC, BaseColor.BLUE);
            Font headingaddress = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 13, Font.BOLD, BaseColor.BLACK);




            // Create the heading paragraph with the headig font
            Paragraph heading;
            heading = new Paragraph("WELCOME HOLY SPIRIT INTERNATIONAL CHURCH", font_heading_3);

            Paragraph thankf;
            thankf = new Paragraph("--------------Thank you for your Donation have a nice day and God bless you---------------", font_body);

            Paragraph address;
            address = new Paragraph("Head Office", headingaddress);
            Paragraph StreetNo;
            StreetNo = new Paragraph("E1210 UMBALANE ROAD", address_font);
            Paragraph location;
            location = new Paragraph("Ntuzuma", address_font);

            Paragraph city;
            city = new Paragraph("Durban", address_font);

            Paragraph code;
            code = new Paragraph("4360", address_font);

            PdfPTable table = new PdfPTable(2);
            table.DefaultCell.Border = Rectangle.NO_BORDER;


            PdfPCell cell = new PdfPCell(new Phrase(beginning));

            PdfPTable table1 = new PdfPTable(1);
            table1.DefaultCell.Border = Rectangle.NO_BORDER;

            PdfPCell cell2 = new PdfPCell(heading);
            PdfPCell cellt = new PdfPCell(thankf);

            cell2.Colspan = 1;
            cell2.HorizontalAlignment = 1;

            //PdfPCell cell1 = new PdfPCell(new Phrase(Ending));
            cell.Colspan = 2;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right



            PdfPTable table2 = new PdfPTable(2);
            table2.DefaultCell.Border = Rectangle.NO_BORDER;


            PdfPTable tbthank = new PdfPTable(1);
            tbthank.DefaultCell.Border = Rectangle.NO_BORDER;

            Font slogan_font = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 13, Font.ITALIC, BaseColor.BLUE);

            Paragraph slogan;
            slogan = new Paragraph("                 People with the mandate and influence of the holy spirit", slogan_font);


            tbthank.AddCell(thankf);
            table1.AddCell(cell2);
            table1.AddCell(slogan);
            table1.AddCell(address);
            table1.AddCell(StreetNo);
            table1.AddCell(location);
            table1.AddCell(city);
            table1.AddCell(code);
            table.AddCell(cell);
            //table.AddCell(cell1);
            table.AddCell("              ");
            table.AddCell("              ");

            table.AddCell("Donated by");
            table.AddCell(serv.email);
            table.AddCell("              ");
            table.AddCell("              ");

            table.AddCell("Donation Type");
            table.AddCell("Service");
            table.AddCell("              ");
            table.AddCell("              ");

            table.AddCell("Type of Service");
            table.AddCell(type.Description);
            table.AddCell("              ");
            table.AddCell("              ");

            table.AddCell("Service Name");
            table.AddCell(serv.Service_name);
            table.AddCell("              ");
            table.AddCell("              ");

            table.AddCell("Quantity");
            table.AddCell(serv.Quantity.ToString());
            table.AddCell("              ");
            table.AddCell("              ");

            table.AddCell("Date");
            table.AddCell(ReportDate(serv.Date));
            table.AddCell("              ");
            table.AddCell("              ");

            table.AddCell("Time");
            table.AddCell(it.GetNistTime().ToShortTimeString());
            table.AddCell("              ");
            table.AddCell("              ");
            



            //Add Image
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imagepath + "/SCAN0008.JPG");
            jpg.ScaleToFit(300f, 150f);
            jpg.Alignment = Element.ALIGN_CENTER;
            jpg.Border = Rectangle.BOX;
            jpg.BorderColor = BaseColor.LIGHT_GRAY;
            jpg.BorderColorTop = BaseColor.WHITE;
            jpg.BorderWidth = 3f;

            Phrase phrase = new Phrase(Environment.NewLine);
            Phrase phrase1 = new Phrase(Environment.NewLine);
            Phrase phrase2 = new Phrase(Environment.NewLine);


            //doc.Add();
            doc.Add(phrase1);
            doc.Add(table1);
            doc.Add(jpg);
            doc.Add(phrase2);
            doc.Add(table2);
            doc.Add(phrase);
            doc.Add(table);
            doc.Add(phrase1);
            doc.Add(tbthank);
            doc.Close();


            byte[] file = ms.ToArray();
            var output = new MemoryStream();
            output.Write(file, 0, file.Length);
            output.Position = 0;
            var f = new FileStreamResult(output, "application/pdf");
            return f;
        }
        private PdfTemplate PdfFooterExt(PdfContentByte cb)
        {
            // Create the template and assign height
            PdfTemplate tmpFooter = cb.CreateTemplate(580, 70);

            // Move to the bottom left corner of the template
            tmpFooter.MoveTo(1, 1);
            // Place the footer content
            tmpFooter.Stroke();
            // Begin writing the footer
            tmpFooter.BeginText();
            // Set the font and size
            tmpFooter.SetFontAndSize(f_cn, 8);
            // Write out details from the payee table
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Head Office", 0, 53, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "21 May Street", 0, 45, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Ntuzuma", 0, 37, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Durban".ToString(), 0, 29, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "4100", 0, 21, 0);
            // Bold text for ther headers

            //string Phone= "Phone", mail, Web, Legal;
            //Font font_footer = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, Font.BOLD, BaseColor.RED);
            //font_footer.Color = BaseColor.BLUE;
            //Chunk begin = new Chunk(Phone, font_footer);

            tmpFooter.SetFontAndSize(f_cb, 8);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Phone", 215, 53, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Mail", 215, 45, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Web", 215, 37, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Legal info", 400, 53, 0);
            // Regular text for infomation fields
            tmpFooter.SetFontAndSize(f_cn, 8);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "0780329830".ToString(), 265, 53, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "infor@whsic.co.za".ToString(), 265, 45, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "www.whsic.co.za".ToString(), 265, 37, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "xtrainfo".ToString(), 400, 45, 0);
            // End text
            tmpFooter.EndText();
            // Stamp a line above the page footer
            cb.SetLineWidth(0f);
            cb.MoveTo(30, 60);
            cb.LineTo(570, 60);
            cb.Stroke();
            // Return the footer template
            return tmpFooter;
        }

        public string ReportDate(DateTime date)
        {
            string findate = null;
            int day = date.Day;
            int month = date.Month;
            int year = date.Year;

            if (month == 1)
            {
                findate += "January";
            }
            else if (month == 2)
            {
                findate += "February";
            }
            else if (month == 3)
            {
                findate += "March";
            }
            else if (month == 4)
            {
                findate += "April";
            }
            else if (month == 5)
            {
                findate += "May";
            }
            else if (month == 6)
            {
                findate += "June";
            }
            else if (month == 7)
            {
                findate += "july";
            }
            else if (month == 8)
            {
                findate += "August";
            }
            else if (month == 9)
            {
                findate += "September";
            }
            else if (month == 10)
            {
                findate += "October";
            }
            else if (month == 11)
            {
                findate += "November";
            }
            else if (month == 12)
            {
                findate += "December";
            }
            return day + " " + findate + " " + year;
        }
    }
}
