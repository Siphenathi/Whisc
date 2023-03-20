using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using WHSIC.BusinessLogic;
using WHSIC.Model;
using WHSIC.Data;

namespace WHSIC.Controllers
{
    public class TreasureController : Controller
    {
        DonationBusiness _db = new DonationBusiness();
        ExpenseBusiness exb = new ExpenseBusiness();
        Expense_PaidBusiness exp = new Expense_PaidBusiness();
        InternetTime it = new InternetTime();
        ApplicationDbContext db = new ApplicationDbContext();


        BaseFont f_cb = BaseFont.CreateFont("c:\\windows\\fonts\\calibrib.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        BaseFont f_cn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

        [Authorize(Roles = "Treasure")]
        public ActionResult FinancialReport()
        {
            FREportViewModel rep = new FREportViewModel();
            List<Expense_PaidViewModel> Expense_PaidList = new List<Expense_PaidViewModel>();
            List<DonationViewModel> DonationList = new List<DonationViewModel>();
            List<Asset_Rentalz> Rental = new List<Asset_Rentalz>();

            Expense_PaidList = (from e in exp.GetAllExpenses_Paid()
                                //where e.date_paid.Month == it.GetNistTime().Month
                                select e).ToList();

            DonationList = (from d in _db.GetAllDonation()
                            //where d.Date.Month == it.GetNistTime().Month
                            select d).ToList();

            Rental = (from r in db.Asset_Rentals.ToList()
                      where r.HireDate.Month == it.GetNistTime().Month
                      select r).ToList();


            rep.Income = DonationList;
            rep.Expense = Expense_PaidList;
            rep.Rental = Rental;

            string sdate = "01/" + 1+ "/" + it.GetNistTime().Year;
            rep.StartDate = Convert.ToDateTime(sdate);
            rep.EndDate = it.GetNistTime();
            return View(rep);
        }
        [HttpPost]
        public ActionResult FinancialReport(ReportViewModel model)
        {
            FREportViewModel rep = new FREportViewModel();
            List<Expense_PaidViewModel> Expense_PaidList = new List<Expense_PaidViewModel>();
            List<DonationViewModel> DonationList = new List<DonationViewModel>();

            try
            {
                Expense_PaidList = (from e in exp.GetAllExpenses_Paid()
                                    where e.date_paid >= model.StartDate && e.date_paid <= model.Endate
                                    select e).ToList();
                DonationList = (from d in _db.GetAllDonation()
                                where d.Date >= model.StartDate && d.Date <= model.Endate
                                select d).ToList();

                rep.Income = DonationList;
                rep.Expense = Expense_PaidList;

                rep.StartDate = model.StartDate;
                rep.EndDate = model.Endate;

                ViewBag.donation = DonationList;
                ViewBag.exp = Expense_PaidList;
            }
            catch (Exception c)
            {

            }
            return View(rep);
        }

        public ActionResult _SearchReportforchart()
        {
            return View();
        }
        [Authorize(Roles = "Treasure")]
        public ActionResult RenderChart()
        {
            FREportViewModel rep = new FREportViewModel();
            List<Expense_PaidViewModel> Expense_PaidList = new List<Expense_PaidViewModel>();
            List<DonationViewModel> DonationList = new List<DonationViewModel>();


            Expense_PaidList = (from e in exp.GetAllExpenses_Paid()
                                //where e.date_paid.Month == DateTime.Now.Month
                                select e).ToList();

            DonationList = (from d in _db.GetAllDonation()
                            //where d.Date.Month == DateTime.Now.Month
                            select d).ToList();


            rep.Income = DonationList;
            rep.Expense = Expense_PaidList;

            return View(rep);
        }
        [HttpPost]
        public ActionResult RenderChart(ReportViewModel model)
        {

            FREportViewModel rep = new FREportViewModel();
            List<Expense_PaidViewModel> Expense_PaidList = new List<Expense_PaidViewModel>();
            List<DonationViewModel> DonationList = new List<DonationViewModel>();

            try
            {
                Expense_PaidList = (from e in exp.GetAllExpenses_Paid()
                                    where e.date_paid >= model.StartDate && e.date_paid <= model.Endate
                                    select e).ToList();
                DonationList = (from d in _db.GetAllDonation()
                                where d.Date >= model.StartDate && d.Date <= model.Endate
                                select d).ToList();

                rep.Income = DonationList;
                rep.Expense = Expense_PaidList;

                ViewBag.donation = DonationList;
                ViewBag.exp = Expense_PaidList;
            }
            catch (Exception c)
            {

            }
            return View(rep);
        }

        
        public ActionResult _SearchReport()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        public FileStreamResult Report()
        {
            Document doc = new Document(PageSize.A4, 0, 0, 0, 0);
            var ms = new MemoryStream();

            PdfWriter writer = PdfWriter.GetInstance(doc, ms);

            doc.Open();
            PdfContentByte cb = writer.DirectContent;
            cb.AddTemplate(PdfFooter(cb), 30, 1);

            Font font_heading_3 = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 16, Font.BOLD, BaseColor.BLACK);
            Paragraph heading;
            heading = new Paragraph("WELCOME HOLY SPIRIT INTERNATIONAL CHURCH", font_heading_3);

            Font slogan_font = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 13, Font.ITALIC, BaseColor.BLUE);

            Paragraph slogan;
            slogan = new Paragraph("                 People with the mandate and influence of the holy spirit", slogan_font);

            Paragraph Datep;
            Datep = new Paragraph("Date");
            Paragraph Refp;
            Refp = new Paragraph("Reference");
            Paragraph Descp;
            Descp = new Paragraph("Description");
            Refp = new Paragraph("Reference");
            Paragraph Amountp;
            Amountp = new Paragraph("Amount");
            Paragraph Balp;
            Balp = new Paragraph("Outstanding Balance");

            Paragraph Expp;
            Expp = new Paragraph("Expense");

            DateTime d = DateTime.Now;

            Paragraph Dateprint;
            Dateprint = new Paragraph("Produced on : " + d);
            Paragraph timeprint;
            timeprint = new Paragraph("Time : " + d.ToShortTimeString());

            PdfPTable table1 = new PdfPTable(1);
            PdfPTable tableexp = new PdfPTable(1);

            PdfPTable tableGridIncome = new PdfPTable(5);
            PdfPTable tableGridExp = new PdfPTable(5);

            table1.DefaultCell.Border = Rectangle.NO_BORDER;
            PdfPCell cellheader = new PdfPCell(heading);
            cellheader.Colspan = 1;
            cellheader.HorizontalAlignment = 1;

            PdfPCell DateCell = new PdfPCell(Datep);
            PdfPCell RefCell = new PdfPCell(Refp);
            PdfPCell DescCell = new PdfPCell(Descp);
            PdfPCell AmtCell = new PdfPCell(Amountp);
            PdfPCell BalCell = new PdfPCell(Balp);
            PdfPCell cellexp = new PdfPCell(Expp);

            table1.AddCell(cellheader);
            table1.AddCell(slogan);
            table1.AddCell("   ");
            table1.AddCell("                                                          Financial Report of May 2016");
            table1.AddCell(Dateprint);
            table1.AddCell(timeprint);
            table1.AddCell("   ");
            table1.AddCell("Income");
            tableGridIncome.AddCell(DateCell);
            tableGridIncome.AddCell(RefCell);
            tableGridIncome.AddCell(DescCell);
            tableGridIncome.AddCell(AmtCell);
            tableGridIncome.AddCell(BalCell);

            tableGridIncome.AddCell("08 May 2016");
            tableGridIncome.AddCell("Mondli");
            tableGridIncome.AddCell("Offerings");
            tableGridIncome.AddCell("R5000,00");
            tableGridIncome.AddCell("");
            tableGridIncome.AddCell("15 May 2016");
            tableGridIncome.AddCell("Siwa");
            tableGridIncome.AddCell("Offerings");
            tableGridIncome.AddCell("R3333,00");
            tableGridIncome.AddCell("");
            tableGridIncome.AddCell("18 May 2016");
            tableGridIncome.AddCell("Siwa");
            tableGridIncome.AddCell("Pledge");
            tableGridIncome.AddCell("R10000,00");
            tableGridIncome.AddCell("");
            tableGridIncome.AddCell("21 May 2016");
            tableGridIncome.AddCell("Siwa");
            tableGridIncome.AddCell("Tithes");
            tableGridIncome.AddCell("R2300,00");
            tableGridIncome.AddCell("");
            tableGridIncome.AddCell("21 May 2016");
            tableGridIncome.AddCell("Siphenathi");
            tableGridIncome.AddCell("Tithes");
            tableGridIncome.AddCell("R2500,00");
            tableGridIncome.AddCell("");
            tableGridIncome.AddCell("21 May 2016");
            tableGridIncome.AddCell("Mondli Mkhanyiswa");
            tableGridIncome.AddCell("Tithes");
            tableGridIncome.AddCell("R2500,00");
            tableGridIncome.AddCell("");
            tableGridIncome.AddCell("21 May 2016");
            tableGridIncome.AddCell("Mondli Mkhanyiswa");
            tableGridIncome.AddCell("Tithes");
            tableGridIncome.AddCell("R10000,00");
            tableGridIncome.AddCell("");
            tableGridIncome.AddCell("21 May 2016");
            tableGridIncome.AddCell("Mondli Mkhanyiswa");
            tableGridIncome.AddCell("Tithes");
            tableGridIncome.AddCell("R10000,00");
            tableGridIncome.AddCell("");
            tableGridIncome.AddCell("21 May 2016");
            tableGridIncome.AddCell("Mkile Ishmael");
            tableGridIncome.AddCell("Tithes");
            tableGridIncome.AddCell("R2500,00");
            tableGridIncome.AddCell("");
            tableGridIncome.AddCell("Total Income");
            tableGridIncome.AddCell("");
            tableGridIncome.AddCell("");
            tableGridIncome.AddCell("R72999,00");
            tableGridIncome.AddCell("");

            tableexp.AddCell(cellexp);

            tableGridExp.AddCell("12 May 2016");
            tableGridExp.AddCell("Mondli");
            tableGridExp.AddCell("Water");
            tableGridExp.AddCell("R500,00");
            tableGridExp.AddCell("R5400,00");

            tableGridExp.AddCell("18 May 2016");
            tableGridExp.AddCell("Ishmael");
            tableGridExp.AddCell("Water");
            tableGridExp.AddCell("R1200,00");
            tableGridExp.AddCell("R4700,00");

            tableGridExp.AddCell("21 May 2016");
            tableGridExp.AddCell("Sipho");
            tableGridExp.AddCell("Tents");
            tableGridExp.AddCell("R11000,00");
            tableGridExp.AddCell("R1000,00");

            tableGridExp.AddCell("22 May 2016");
            tableGridExp.AddCell("Zama");
            tableGridExp.AddCell("Electricity");
            tableGridExp.AddCell("R500,00");
            tableGridExp.AddCell("R100,00");

            tableGridExp.AddCell("21 May 2016");
            tableGridExp.AddCell("Sipho");
            tableGridExp.AddCell("Water");
            tableGridExp.AddCell("R500,00");
            tableGridExp.AddCell("R5400,00");

            tableGridExp.AddCell("Total Amount");
            tableGridExp.AddCell("");
            tableGridExp.AddCell("");
            tableGridExp.AddCell("R13700,00");
            tableGridExp.AddCell("16600,00");

            tableGridExp.AddCell("Total Balance");
            tableGridExp.AddCell("R59299,00");
            tableGridExp.AddCell("");
            tableGridExp.AddCell("");
            tableGridExp.AddCell("");

            Phrase phrase = new Phrase(Environment.NewLine);


            doc.Add(phrase);
            doc.Add(table1);
            doc.Add(tableGridIncome);
            doc.Add(tableexp);
            doc.Add(tableGridExp);
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
    }
}