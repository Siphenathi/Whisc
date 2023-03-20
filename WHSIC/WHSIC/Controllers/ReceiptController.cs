using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WHSIC.BusinessLogic;
using WHSIC.Model;

namespace WHSIC.Controllers
{
    public class ReceiptController : Controller
    {
        readonly BaseFont _fCb = BaseFont.CreateFont("c:\\windows\\fonts\\calibrib.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        readonly BaseFont _fCn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

        DonorBusiness donorb = new DonorBusiness();
        DonorTypeBusiness dontypeb = new DonorTypeBusiness();
        DonationBusiness donationb = new DonationBusiness();
        DonationTypeBusiness donationTypeb = new DonationTypeBusiness();


        DonationViewModel donV = new DonationViewModel();
        DonorViewModel donorV = new DonorViewModel();
        DonorTypeViewModel donotypeV = new DonorTypeViewModel();
        DonationTypeViewModel donationtypeV = new DonationTypeViewModel();
        

           public ActionResult Index()
           {

            DonationViewModel Don = (from obj in donationb.GetAllDonation() select obj).LastOrDefault();

                return View(Don);
           }
            public FileStreamResult Print()
            {

            DonationViewModel Don = (from obj in donationb.GetAllDonation() select obj).LastOrDefault();


            string donor_name = "", donortype = "", donation_type = "", amt = "", date, time = "";

             donV = donationb.GetbyID(Don.DonationID);
             donorV = donorb.GetAllDonor().Find(x => x.Donor_ID == donV.Donor_ID);
             donotypeV = dontypeb.GetAllDonarType().Find(x => x.DonortypeID == donorV.DonortypeID);
             donationtypeV = donationTypeb.GetAllDonationType().Find(x => x.DonationtypeID == donV.DonationtypeID);

            

            donor_name = donorV.Name;
            donortype = donotypeV.Description;
            donation_type = donationtypeV.Description;
            date = donV.Date.ToShortDateString();
            time = donV.Date.ToShortTimeString();
            amt = donV.Amount.ToString("R0.00");

            

            string imagepath = Server.MapPath("~/Images");


                Document doc = new Document(PageSize.A4, 0, 0, 0, 0);
                var ms = new MemoryStream();

                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                writer.CloseStream = false;

                doc.Open();
                PdfContentByte cb = writer.DirectContent;
                cb.AddTemplate(PdfFooter(cb), 30, 1);

                
                Font address_font = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, Font.ITALIC, BaseColor.BLACK);
                //Text Settings
                string text = "Payment Details";
                Font georgia = FontFactory.GetFont("georgia", 24f);
                georgia.Color = BaseColor.BLACK;
                Chunk beginning = new Chunk(text, georgia);




                var fontHeading3 = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 16, Font.BOLD, BaseColor.MAGENTA);
                var fontBody = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 13, Font.BOLDITALIC, BaseColor.BLUE);
                var headingAddress = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 13, Font.BOLD, BaseColor.BLACK);


            

                // Create the heading paragraph with the headig font
                Paragraph heading;
                heading = new Paragraph("WELCOME HOLY SPIRIT INTERNATIONAL CHURCH", fontHeading3);

                Paragraph thankf;
                thankf = new Paragraph("--------------Thank you for your Donation have a nice day and God bless you---------------", fontBody);

                Paragraph address;
                address = new Paragraph("Head Office", headingAddress);
                Paragraph StreetNo;
                StreetNo = new Paragraph("E1210 UMBALANE ROAD", address_font);
                Paragraph location;
                location = new Paragraph("Ntuzuma", address_font);

                Paragraph city;
                city = new Paragraph("Durban", address_font);

                Paragraph code;
                code = new Paragraph("4360", address_font);

                var table = new PdfPTable(2);
                table.DefaultCell.Border = Rectangle.NO_BORDER;


                var cell = new PdfPCell(new Phrase(beginning));

                var table1 = new PdfPTable(1);
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
                table.AddCell(donor_name);
                table.AddCell("              ");
                table.AddCell("              ");
                table.AddCell("Donation For");
                table.AddCell(donation_type);
                table.AddCell("              ");
                table.AddCell("              ");
                table.AddCell("Donated As");
                table.AddCell(donortype);
                table.AddCell("              ");
                table.AddCell("              ");
                table.AddCell("Date");
                table.AddCell(date);
                table.AddCell("              ");
                table.AddCell("              ");
                table.AddCell("Time");
                table.AddCell(time);
                table.AddCell("              ");
                table.AddCell("              ");
                table.AddCell("Amount");
                table.AddCell(amt);
                table.AddCell("              ");
                table.AddCell("              ");
            if(donorV.tax_no!=null)
            {
                table.AddCell("Tax Number");
                table.AddCell(donorV.tax_no.ToString());
            }


            //Add Image
            var jpg = Image.GetInstance(imagepath + "/SCAN0008.JPG");
                jpg.ScaleToFit(300f, 150f);
                jpg.Alignment = Element.ALIGN_CENTER;
                jpg.Border = Rectangle.BOX;
                jpg.BorderColor = BaseColor.LIGHT_GRAY;
                jpg.BorderColorTop = BaseColor.WHITE;
                jpg.BorderWidth = 3f;

                var phrase = new Phrase(Environment.NewLine);
                var phrase1 = new Phrase(Environment.NewLine);
                var phrase2 = new Phrase(Environment.NewLine);


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
            private PdfTemplate PdfFooter(PdfContentByte cb)
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
                tmpFooter.SetFontAndSize(_fCn, 8);
                // Write out details from the payee table
                tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Head Office", 0, 53, 0);
                tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "21 May Street", 0, 45, 0);
                tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Ntuzuma", 0, 37, 0);
                tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Durban", 0, 29, 0);
                tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "4100", 0, 21, 0);
                // Bold text for ther headers

                //string Phone= "Phone", mail, Web, Legal;
                //Font font_footer = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, Font.BOLD, BaseColor.RED);
                //font_footer.Color = BaseColor.BLUE;
                //Chunk begin = new Chunk(Phone, font_footer);

                tmpFooter.SetFontAndSize(_fCb, 8);
                tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Phone", 215, 53, 0);
                tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Mail", 215, 45, 0);
                tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Web", 215, 37, 0);
                tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Legal info", 400, 53, 0);
                // Regular text for infomation fields
                tmpFooter.SetFontAndSize(_fCn, 8);
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
        }
    }
