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
using WHSIC.Data;

namespace WHSIC.Controllers
{
    public class FinancialReportController : Controller
    {
        DonationBusiness _db = new DonationBusiness();
        ExpenseBusiness exb = new ExpenseBusiness();
        Expense_PaidBusiness exp = new Expense_PaidBusiness();
        DonationTypeBusiness dontype = new DonationTypeBusiness(); 
        InternetTime it = new InternetTime();
        ExpenseViewModel evm = new ExpenseViewModel();
        ApplicationDbContext db = new ApplicationDbContext();


        BaseFont f_cb = BaseFont.CreateFont("c:\\windows\\fonts\\calibrib.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        BaseFont f_cn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

        //[Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            List<object> income = new List<object>();
            List<Expense_PaidViewModel> Expense_PaidList = new List<Expense_PaidViewModel>();
            List<DonationViewModel> DonationList = new List<DonationViewModel>();
            List<Asset_Rentalz> Rental = new List<Asset_Rentalz>();

            Expense_PaidList = (from e in exp.GetAllExpenses_Paid()
                                where e.date_paid.Month == DateTime.Now.Month
                                select e).ToList();

            DonationList = (from d in _db.GetAllDonation()
                            where d.Date.Month == DateTime.Now.Month
                            select d).ToList();

            income.Add(DonationList);
            income.Add(Expense_PaidList);

            return View(income);
        }
        [HttpPost]
        public ActionResult Index(ReportViewModel model)
        {

            List<object> income = new List<object>();
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


                income.Add(DonationList);
                income.Add(Expense_PaidList);

                ViewBag.donation = DonationList;
                ViewBag.exp = Expense_PaidList;
            }
            catch (Exception c)
            {

            }
            return View(income);
        }
        public ActionResult _LineGraph()
        {
            return View();
        }
        [Authorize(Roles = "Treasure")]
        public ActionResult LineGraph()
        {
            return View();
        }
        public ActionResult _SearchReportforchart()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult RenderChart()
        {
            var income = new List<object>();
            List<Expense_PaidViewModel> Expense_PaidList = new List<Expense_PaidViewModel>();
            List<DonationViewModel> DonationList = new List<DonationViewModel>();


            Expense_PaidList = (from e in exp.GetAllExpenses_Paid()
                                where e.date_paid.Month == DateTime.Now.Month
                                select e).ToList();

            DonationList = (from d in _db.GetAllDonation()
                            where d.Date.Month == DateTime.Now.Month
                            select d).ToList();

            income.Add(DonationList);
            income.Add(Expense_PaidList);

            return View(income);
        }
        [HttpPost]
        public ActionResult RenderChart(ReportViewModel model)
        {

            List<object> income = new List<object>();
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


                income.Add(DonationList);
                income.Add(Expense_PaidList);

                ViewBag.donation = DonationList;
                ViewBag.exp = Expense_PaidList;
            }
            catch (Exception c)
            {

            }
            return View(income);
        }

        public ActionResult Chart()
        {
            return View();
        }
        public ActionResult _SearchReport()
        {
            return View();
        }
        public ActionResult _ProfandLost()
        {
            return View();
        }
        public ActionResult _Income()
        {
            return View();
        }
        public ActionResult _Expenses()
        {
            return View();
        }

        public FileStreamResult Report(FREportViewModel model)
        {
            List<Expense_PaidViewModel> ListExpPaid = new List<Expense_PaidViewModel>();
            List<Asset_Rentalz> RentalList = new List<Asset_Rentalz>(); 
            List<DonationViewModel> listDonation = new List<DonationViewModel>();
            decimal totDon = 0, totExp = 0;
            foreach (var obj in model.Income)
            {
                DonationViewModel donation = _db.GetAllDonation().Find(x => x.DonationID == obj.DonationID);
                listDonation.Add(donation);
                totDon += donation.Amount;
            }

            foreach (var ex in model.Expense)
            {
                Expense_PaidViewModel epv = exp.GetAllExpenses_Paid().Find(x => x.paid_exp_id == ex.paid_exp_id);
                ListExpPaid.Add(epv);
            }
            foreach (var ex in model.Rental)
            {
                Asset_Rentalz epv = db.Asset_Rentals.Find(ex.Rental_code);
                RentalList.Add(epv);
            }

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


            Font boldtext = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 14, Font.BOLD, BaseColor.BLACK);

            Paragraph Expp;
            Expp = new Paragraph("EXPENSES",boldtext);
            Paragraph totExpp;
            totExpp = new Paragraph("                  Total Expenses", boldtext);

            Paragraph Netinc;
            Netinc = new Paragraph("Net Income", boldtext);

            Paragraph Rev;
            Rev = new Paragraph("REVENUE", boldtext);
            Paragraph totRev;
            totRev = new Paragraph("                  Total Revenue", boldtext);

            Paragraph Dateprint;
            Dateprint = new Paragraph("Produced on : " + ReportDate(it.GetNistTime()));
            Paragraph timeprint;
            timeprint = new Paragraph("Time : " + it.GetNistTime().ToShortTimeString());

            PdfPTable table1 = new PdfPTable(1);
            //PdfPTable tableexp = new PdfPTable(1);
            PdfPTable tableGridIncome = new PdfPTable(3);
            PdfPTable tableGridExp = new PdfPTable(3);


            //tableexp.DefaultCell.Border = Rectangle.NO_BORDER;
            //tableGridIncome.DefaultCell.Border = Rectangle.NO_BORDER;
            table1.DefaultCell.Border = Rectangle.NO_BORDER;
            //tableGridExp.DefaultCell.Border = Rectangle.NO_BORDER;

            PdfPCell cellheader = new PdfPCell(heading);
            cellheader.Colspan = 1;
            cellheader.HorizontalAlignment = 1;

            Font boldstatement = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 15, Font.BOLD, BaseColor.BLACK);

            Paragraph istatement;
            istatement = new Paragraph("                                                Income Statement ", boldstatement);

            Paragraph dstatement;
            dstatement = new Paragraph("                       For " + ReportDate(model.StartDate) + " to " + ReportDate(model.EndDate), boldstatement);

            PdfPCell cellexp = new PdfPCell(Expp);

            table1.AddCell(cellheader);
            table1.AddCell(slogan);
            table1.AddCell("   ");
            table1.AddCell(istatement);
            table1.AddCell(dstatement);
            table1.AddCell("  ");
            table1.AddCell(Dateprint);
            table1.AddCell(timeprint);
            table1.AddCell("   ");

            tableGridIncome.AddCell(Rev);
            tableGridIncome.AddCell(" ");
            tableGridIncome.AddCell(" ");
            decimal rental = 0;
            foreach (var type in dontype.GetAllDonationType())
            {
                decimal Amttype = 0;
                tableGridIncome.AddCell("           "+type.Description);

                List<DonationViewModel> ListType = listDonation.FindAll(x => x.DonationtypeID == type.DonationtypeID);

                foreach (var don in ListType)
                {
 
                    Amttype +=don.Amount;
                    
                }
                tableGridIncome.AddCell(Amttype.ToString("R00.00"));
                tableGridIncome.AddCell(" ");   
            }
            foreach (var don in RentalList)
            {
                rental += don.Amount;

            }
            tableGridIncome.AddCell("           Rentals");
            tableGridIncome.AddCell(rental.ToString("R00.00"));
            tableGridIncome.AddCell(" ");
            totDon += rental;

            Paragraph totamtRev;
            totamtRev = new Paragraph(totDon.ToString("R00.00"), boldtext);

            tableGridIncome.AddCell(totRev);
            tableGridIncome.AddCell("");

            tableGridIncome.AddCell(totamtRev);

            tableGridIncome.AddCell(cellexp);
            tableGridIncome.AddCell(" ");
            tableGridIncome.AddCell(" ");

            foreach (var exp in exb.GetAllExpenses())
            {
                decimal Amtpaid = 0, rem_amount = 0;
                tableGridExp.AddCell("           "+exp.desc);
                List<Expense_PaidViewModel> ExpList = ListExpPaid.FindAll(x=>x.exp_id==exp.exp_id);
                foreach (var obj in ExpList)
                {

                    //rem_amount += exp.amount - obj.amount_paid;
                    Amtpaid += obj.amount_paid;
                    
                }
                tableGridExp.AddCell(Amtpaid.ToString("R00.00"));
                tableGridExp.AddCell(" ");
                //tableGridExp.AddCell(rem_amount.ToString("R00.00"));
                totExp += Amtpaid;
            }

            Paragraph totamtExp;
            totamtExp = new Paragraph(totExp.ToString("R0.00"), boldtext);


            var profnloss = totDon - totExp;
            Paragraph totnet;
            totnet = new Paragraph(profnloss.ToString("R00.00"), boldtext);

            tableGridExp.AddCell(totExpp);
            tableGridExp.AddCell(" ");
            tableGridExp.AddCell(totamtExp);
            //tableGridExp.AddCell(totexpRemain.ToString("00.00"));

            tableGridExp.AddCell(Netinc);
            tableGridExp.AddCell(" ");
            tableGridExp.AddCell(totnet);
           
            //tableGridExp.AddCell("");
            //tableGridExp.AddCell("");

            Phrase phrase = new Phrase(Environment.NewLine);


            doc.Add(phrase);
            doc.Add(table1);
            doc.Add(tableGridIncome);
            //doc.Add(tableexp);
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
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "0780329830", 265, 53, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "infor@whsic.org", 265, 45, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "www.whsic.org", 265, 37, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "xtrainfo", 400, 45, 0);
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

            if(day.ToString().Length==1)
            {
                return "0"+day + " " + findate + " " + year;
            }

            return day + " " + findate + " " + year;
        }
    }
}