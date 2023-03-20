
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WHSIC.Data;
using WHSIC.Model;

namespace WHSIC.Controllers
{
    public class DamagedController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        BaseFont f_cb = BaseFont.CreateFont("c:\\windows\\fonts\\calibrib.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        BaseFont f_cn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        public ActionResult DamagedList()
        {
            return View(db.Damaged_Assets.ToList());
        }
        public List<Asset> RentalList()
        {
            List<Asset> finalList = new List<Asset>();
            List<Asset> RentaLAss = (from a in db.Assets.ToList() where a.ForRental == true select a).ToList();
            foreach (var r in RentaLAss)
            {
                AssetCharge charge = db.AssetCharges.ToList().Find(x => x.Asset_Code == r.Asset_Code);
                if (charge != null)
                {
                    finalList.Add(r);
                }
            }
            return finalList;
        }

        public ActionResult EditDamaged(int?id)
        {
            ViewBag.Asset_code = new SelectList(RentalList(), "Asset_Code", "AName");
            ViewBag.Renter_Code = new SelectList(db.Asset_Renters.ToList(), "Renter_Code", "FullName");
            Damaged_Asset dam = db.Damaged_Assets.Find(id);

            Damaged_AssetViewModel dmg = new Damaged_AssetViewModel
            {
                Amount = dam.Amount,
                PayDate = dam.PayDate,
                Renter_code = dam.Renter_code,
                Quantity = dam.Quantity,
                Asset_Code = dam.Asset_Code,
                Damaged_ID= dam.Damaged_ID
            };
            ViewBag.qty = dam.Quantity;
            return View(dmg);
        }
        [HttpPost]
        public ActionResult EditDamaged(Damaged_AssetViewModel model,FormCollection form)
        {
            ViewBag.Asset_code = new SelectList(RentalList(), "Asset_Code", "AName");
            ViewBag.Renter_Code = new SelectList(db.Asset_Renters.ToList(), "Renter_Code", "FullName");
            string feed = null;
            int qty = Convert.ToInt32(form["txtqty"]);
            try
            {
                string pDate = model.PayDate.ToShortDateString();
                string dateNow = DateTime.Now.ToShortDateString();

                if (Convert.ToDateTime(pDate)<Convert.ToDateTime(dateNow))
                {
                    feed = "Invalid Pay Date, you cannot select the passed date for the pay date";
                }
                else
                {
                    if (model.Quantity <= 0)
                    {
                        feed = "Invalid number of quantity..! ";
                    }
                    else
                    {
                        Asset ast = db.Assets.ToList().Find(x => x.Asset_Code == model.Asset_Code);
                        ast.quantity += qty;
                        db.SaveChanges();

                        int lqty = 0;
                        Asset ass = db.Assets.ToList().Find(x => x.Asset_Code == model.Asset_Code);
                        lqty = ass.quantity - model.Quantity;

                        if (lqty < 0)
                        {

                            feed = "Entered number of damaged " + ass.AName + " is more than the number we have currently, firstly record returned quantity before recording the damaged quantity..!";
                        }
                        else
                        {
                            Damaged_Asset dam = db.Damaged_Assets.Find(model.Damaged_ID);

                            dam.Amount = ass.UnitPrice * model.Quantity;
                            dam.PayDate = model.PayDate;
                            dam.Renter_code = model.Renter_code;
                            dam.Quantity = model.Quantity;
                            dam.Asset_Code = model.Asset_Code;
                           
                            ass.quantity -= model.Quantity;
                            db.SaveChanges();

                            Damaged_Asset da = (from d in db.Damaged_Assets.ToList() where d.Asset_Code == model.Asset_Code && d.Renter_code == model.Renter_code select d).Last();
                            da.Quotation = ProvideQoute(da);
                            db.SaveChanges();

                            return RedirectToAction("DamagedList");
                        }

                    }
                }
            }
            catch (Exception v)
            {

            }
            ViewBag.Asset_code = new SelectList(RentalList(), "Asset_Code", "AName");
            ViewBag.Renter_Code = new SelectList(db.Asset_Renters.ToList(), "Renter_Code", "FullName");
            ViewBag.feed = feed;
            ViewBag.qty = qty;
            return View(model);
        }


        public ActionResult AddDamaged()
        {
            ViewBag.Asset_code = new SelectList(RentalList(), "Asset_Code", "AName");
            ViewBag.Renter_Code = new SelectList(db.Asset_Renters.ToList(), "Renter_Code", "FullName");
            return View();
        }
        [HttpPost]
        public ActionResult AddDamaged(Damaged_AssetViewModel model)
        {
            ViewBag.Asset_Code = new SelectList(db.Assets.ToList(), "Asset_Code", "AName");
            ViewBag.Renter_Code = new SelectList(db.Asset_Renters.ToList(), "Renter_Code", "FullName");
            string feed = null;
            try
            {
                string pDate = model.PayDate.ToShortDateString();
                string dateNow = DateTime.Now.ToShortDateString();

                if (Convert.ToDateTime(pDate) < Convert.ToDateTime(dateNow))
                {
                    feed = "Invalid Pay Date, you cannot select the passed date for the pay date";
                }
                else
                {
                    if (model.Quantity <= 0)
                    {
                        feed = "Invalid number of quantity..! ";
                    }
                    else
                    {
                        int lqty = 0;
                        Asset ass = db.Assets.ToList().Find(x => x.Asset_Code == model.Asset_Code);
                        lqty = ass.quantity - model.Quantity;

                        if (lqty<0)
                        {

                            feed = "Entered number of damaged "+ass.AName+" is more than the number we have currently, firstly record returned quantity before recording the damaged quantity..!";
                        }
                        else
                        {
                            Damaged_Asset dam = new Damaged_Asset
                            {
                                Amount = ass.UnitPrice * model.Quantity,
                                PayDate = model.PayDate,
                                Renter_code = model.Renter_code,
                                Quantity = model.Quantity,
                                Asset_Code = model.Asset_Code
                            };
                            ass.quantity -= model.Quantity;
                            db.Damaged_Assets.Add(dam);
                            db.SaveChanges();

                            Damaged_Asset da = (from d in db.Damaged_Assets.ToList() where d.Asset_Code == model.Asset_Code && d.Renter_code == model.Renter_code select d).Last();
                            da.Quotation = ProvideQoute(da);
                            db.SaveChanges();

                            return RedirectToAction("DamagedList");
                        }

                    }
                }
            }
            catch (Exception v)
            {

            }
            ViewBag.Asset_code = new SelectList(RentalList(), "Asset_Code", "AName");
            ViewBag.Renter_Code = new SelectList(db.Asset_Renters.ToList(), "Renter_Code", "FullName");
            ViewBag.feed = feed;
            return View();
        }
        public FileStreamResult ShowQuotation(int? id)
        {
            Stream output = new MemoryStream(getQuotation((int)id).Quotation);
            return new FileStreamResult(output, "application/pdf");
        }
        public Damaged_Asset getQuotation(int id)
        {
            return db.Damaged_Assets.Find(id);
        }

        public byte[] ProvideQoute(Damaged_Asset obj)
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
            slogan = new Paragraph("People with the mandate and influence of the holy spirit", slogan_font);

            Font boldstatement = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 11, Font.BOLD, BaseColor.BLACK);

            Font defaultfont = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 11, Font.NORMAL, BaseColor.BLACK);

            Font Qforpar = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 13, Font.BOLD, BaseColor.BLACK);

            Font quotedetails = FontFactory.GetFont(FontFactory.HELVETICA_BOLDOBLIQUE, 16, Font.BOLD, BaseColor.BLACK);

            //Qoutation details for formaTop table

            Paragraph damage;
            damage = new Paragraph("Damaged/Lost Asset", boldstatement);

            Paragraph Qvalid;
            Qvalid = new Paragraph("Valid until : " + obj.PayDate.ToShortDateString(), boldstatement);

            Paragraph Qfor;
            Qfor = new Paragraph("Quotation For : ", Qforpar);

            Paragraph Datepar;
            Datepar = new Paragraph("Date : " + DateTime.Now.ToShortDateString(), boldstatement);
            Paragraph quoteIDpar;
            quoteIDpar = new Paragraph("Quotation No. : " + "QU0" + obj.Damaged_ID, boldstatement);

            Paragraph CustIDpar;
            CustIDpar = new Paragraph("CustomerID : " + "RE0" + obj.Renter_code, boldstatement);

            Paragraph qoute;
            qoute = new Paragraph("Quotation", quotedetails);
            Font boldstatement1 = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 13, Font.BOLD, BaseColor.BLACK);
            //Comments
            Paragraph com1;
            com1 = new Paragraph("We are pleased to provide you with the following quote for the damaged asset.", boldstatement);

            PdfPCell comcell1 = new PdfPCell(com1);
            comcell1.Colspan = 1;
            comcell1.HorizontalAlignment = 1;
            comcell1.Border = Rectangle.NO_BORDER;


            Paragraph subconclusion;
            subconclusion = new Paragraph("If you have any questions concerning this quotation contact Zama Nyosi at 0784351232", defaultfont);

            PdfPCell subconclusioncell = new PdfPCell(subconclusion);
            subconclusioncell.Colspan = 1;
            subconclusioncell.HorizontalAlignment = 1;
            subconclusioncell.Border = Rectangle.NO_BORDER;

            Paragraph subconclusion1;
            subconclusion1 = new Paragraph("Thank you for your business!", boldstatement1);

            PdfPCell subconclusioncell1 = new PdfPCell(subconclusion1);
            subconclusioncell1.Colspan = 1;
            subconclusioncell1.HorizontalAlignment = 1;
            subconclusioncell1.Border = Rectangle.NO_BORDER;

            //suppliers
            //Font boldstatement1 = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 13, Font.BOLD, BaseColor.BLACK);
            Paragraph Name;
            Name = new Paragraph("NAME", boldstatement1);

            Paragraph Contact;
            Contact = new Paragraph("CONTACT", boldstatement1);

            Paragraph Email;
            Email = new Paragraph("EMAIL", boldstatement1);

            Paragraph Address;
            Address = new Paragraph("ADDRESS", boldstatement1);

            PdfPCell SupName = new PdfPCell(Name);
            SupName.Colspan = 1;
            SupName.HorizontalAlignment = 1;
            SupName.BackgroundColor = BaseColor.LIGHT_GRAY;

            PdfPCell SupContact = new PdfPCell(Contact);
            SupContact.Colspan = 1;
            SupContact.HorizontalAlignment = 1;
            SupContact.BackgroundColor = BaseColor.LIGHT_GRAY;

            PdfPCell SupEmail = new PdfPCell(Email);
            SupEmail.Colspan = 1;
            SupEmail.HorizontalAlignment = 1;
            SupEmail.BackgroundColor = BaseColor.LIGHT_GRAY;

            PdfPCell SupAddress = new PdfPCell(Address);
            SupAddress.Colspan = 1;
            SupAddress.HorizontalAlignment = 1;
            SupAddress.BackgroundColor = BaseColor.LIGHT_GRAY;

            //Asset
            Paragraph ADesc;
            ADesc = new Paragraph("DESCRIPTION", boldstatement1);

            Paragraph Aqty;
            Aqty = new Paragraph("QUANTITY", boldstatement1);

            Paragraph Aprice;
            Aprice = new Paragraph("UNIT PRICE", boldstatement1);

            Paragraph Aamt;
            Aamt = new Paragraph("AMOUNT", boldstatement1);

            PdfPCell ADescCell = new PdfPCell(ADesc);
            ADescCell.Colspan = 1;
            ADescCell.HorizontalAlignment = 1;
            ADescCell.BackgroundColor = BaseColor.LIGHT_GRAY;

            PdfPCell AqtyCell = new PdfPCell(Aqty);
            AqtyCell.Colspan = 1;
            AqtyCell.HorizontalAlignment = 1;
            AqtyCell.BackgroundColor = BaseColor.LIGHT_GRAY;

            PdfPCell ApriceCell = new PdfPCell(Aprice);
            ApriceCell.Colspan = 1;
            ApriceCell.HorizontalAlignment = 1;
            ApriceCell.BackgroundColor = BaseColor.LIGHT_GRAY;

            PdfPCell AamtCell = new PdfPCell(Aamt);
            AamtCell.Colspan = 1;
            AamtCell.HorizontalAlignment = 1;
            AamtCell.BackgroundColor = BaseColor.LIGHT_GRAY;


            PdfPTable table1 = new PdfPTable(1);
            PdfPTable table2 = new PdfPTable(1);
            PdfPTable table3 = new PdfPTable(1);
            PdfPTable tblSuppliers = new PdfPTable(4);
            PdfPTable formatTop = new PdfPTable(4);
            PdfPTable tblAsset = new PdfPTable(4);
            PdfPTable tblconclusion = new PdfPTable(1);

            table1.DefaultCell.Border = Rectangle.NO_BORDER;
            formatTop.DefaultCell.Border = Rectangle.NO_BORDER;
            table2.DefaultCell.Border = Rectangle.NO_BORDER;
            table3.DefaultCell.Border = Rectangle.NO_BORDER;
            tblconclusion.DefaultCell.Border = Rectangle.NO_BORDER;

            PdfPCell cellheader = new PdfPCell(heading);
            cellheader.Colspan = 1;
            cellheader.HorizontalAlignment = 1;
            cellheader.Border = Rectangle.NO_BORDER;

            PdfPCell cellslogan = new PdfPCell(slogan);
            cellslogan.Colspan = 1;
            cellslogan.HorizontalAlignment = 1;
            cellslogan.Border = Rectangle.NO_BORDER;

            PdfPCell cellqoute = new PdfPCell(qoute);
            cellqoute.Colspan = 1;
            cellqoute.HorizontalAlignment = 1;
            cellqoute.Border = Rectangle.NO_BORDER;

            PdfPCell test3 = new PdfPCell(new Paragraph(" "));
            test3.Colspan = 1;
            test3.HorizontalAlignment = 1;
            test3.Border = PdfPCell.LEFT_BORDER;

            PdfPCell test4 = new PdfPCell(new Paragraph(" "));
            test4.Colspan = 1;
            test4.HorizontalAlignment = 1;
            test4.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;

            PdfPCell test5 = new PdfPCell(new Paragraph(" "));
            test5.Colspan = 1;
            test5.HorizontalAlignment = 1;
            test5.Border = PdfPCell.TOP_BORDER;


            Asset ass = db.Assets.Find(obj.Asset_Code);

            Supplier sup = db.Suppliers.Find(ass.supply_code);


            //Supplier Values NB--------------------------------------------------------------------------------
            PdfPCell SupNamev = new PdfPCell(new Paragraph(sup.name));
            SupNamev.Colspan = 1;
            SupNamev.HorizontalAlignment = 1;


            PdfPCell SupContactv = new PdfPCell(new Paragraph(sup.contact));
            SupContactv.Colspan = 1;
            SupContactv.HorizontalAlignment = 1;


            PdfPCell SupEmailv = new PdfPCell(new Paragraph(sup.email));
            SupEmailv.Colspan = 1;
            SupEmailv.HorizontalAlignment = 1;


            PdfPCell SupAddresv = new PdfPCell(new Paragraph(sup.address));
            SupAddresv.Colspan = 1;
            SupAddresv.HorizontalAlignment = 1;




            //Asset Values NB-----------------------------------------------------------------------------------

            PdfPCell AssDesc = new PdfPCell(new Paragraph(ass.AName));
            AssDesc.Colspan = 1;
            AssDesc.HorizontalAlignment = 1;
            AssDesc.Border = PdfPCell.LEFT_BORDER;

            PdfPCell Assqty = new PdfPCell(new Paragraph(obj.Quantity.ToString()));
            Assqty.Colspan = 1;
            Assqty.HorizontalAlignment = 1;
            Assqty.Border = PdfPCell.LEFT_BORDER;

            PdfPCell Assuprice = new PdfPCell(new Paragraph(ass.UnitPrice.ToString("R00.00")));
            Assuprice.Colspan = 1;
            Assuprice.HorizontalAlignment = 1;
            Assuprice.Border = PdfPCell.LEFT_BORDER;

            PdfPCell Assamt = new PdfPCell(new Paragraph(obj.Amount.ToString("R00.00")));
            Assamt.Colspan = 1;
            Assamt.HorizontalAlignment = 1;
            Assamt.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;


            Asset_Renter renter = db.Asset_Renters.Find(obj.Renter_code);

            table1.AddCell(cellheader);
            table1.AddCell(cellslogan);
            table1.AddCell(cellqoute);
            table1.AddCell("   ");

            formatTop.AddCell(" ");
            formatTop.AddCell(" ");
            formatTop.AddCell(" ");
            formatTop.AddCell(Datepar);

            formatTop.AddCell(new Phrase("Address", boldstatement));
            formatTop.AddCell(" ");
            formatTop.AddCell(" ");
            formatTop.AddCell(quoteIDpar);

            formatTop.AddCell(new Phrase("21 Ntuzuma ", defaultfont));
            formatTop.AddCell(" ");
            formatTop.AddCell(" ");
            formatTop.AddCell(CustIDpar);

            formatTop.AddCell(new Phrase("Durban", defaultfont));
            formatTop.AddCell(" ");
            formatTop.AddCell(" ");
            formatTop.AddCell(" ");

            formatTop.AddCell(new Phrase("4001", defaultfont));
            formatTop.AddCell(" ");
            formatTop.AddCell(" ");
            formatTop.AddCell(" ");

            formatTop.AddCell(new Phrase("Phone : 0413459078", defaultfont));
            formatTop.AddCell(" ");
            formatTop.AddCell(" ");
            formatTop.AddCell(" ");

            formatTop.AddCell(" ");
            formatTop.AddCell(" ");
            formatTop.AddCell(" ");
            formatTop.AddCell(" ");

            formatTop.AddCell(Qfor);
            formatTop.AddCell(" ");
            formatTop.AddCell(" ");
            formatTop.AddCell(Qvalid);

            formatTop.AddCell(damage);
            formatTop.AddCell(" ");
            formatTop.AddCell(" ");
            formatTop.AddCell(" ");

            formatTop.AddCell(new Phrase(renter.FullName, defaultfont));
            formatTop.AddCell(" ");
            formatTop.AddCell(" ");
            formatTop.AddCell(" ");

            formatTop.AddCell(new Phrase(renter.Physical_Address, defaultfont));
            formatTop.AddCell(" ");
            formatTop.AddCell(" ");
            formatTop.AddCell(" ");

            formatTop.AddCell(new Phrase("Phone : " + renter.Contact, defaultfont));
            formatTop.AddCell(" ");
            formatTop.AddCell(" ");
            formatTop.AddCell(" ");

            formatTop.AddCell(" ");
            formatTop.AddCell(" ");
            formatTop.AddCell(" ");
            formatTop.AddCell(" ");

            table2.AddCell(comcell1);
            table2.AddCell(" ");
            table2.AddCell("Supplier Details");

            tblSuppliers.AddCell(SupName);
            tblSuppliers.AddCell(SupAddress);
            tblSuppliers.AddCell(SupEmail);
            tblSuppliers.AddCell(SupContact);

            tblSuppliers.AddCell(SupNamev);
            tblSuppliers.AddCell(SupAddresv);
            tblSuppliers.AddCell(SupEmailv);
            tblSuppliers.AddCell(SupContactv);
            table3.AddCell(" ");
            table3.AddCell("Asset Details");
            tblAsset.AddCell(ADescCell);
            tblAsset.AddCell(AqtyCell);
            tblAsset.AddCell(ApriceCell);
            tblAsset.AddCell(AamtCell);

            tblAsset.AddCell(AssDesc);
            tblAsset.AddCell(Assqty);
            tblAsset.AddCell(Assuprice);
            tblAsset.AddCell(Assamt);

            tblAsset.AddCell(test3);
            tblAsset.AddCell(test3);
            tblAsset.AddCell(test3);
            tblAsset.AddCell(test4);

            tblAsset.AddCell(test3);
            tblAsset.AddCell(test3);
            tblAsset.AddCell(test3);
            tblAsset.AddCell(test4);

            tblAsset.AddCell(test3);
            tblAsset.AddCell(test3);
            tblAsset.AddCell(test3);
            tblAsset.AddCell(test4);

            tblAsset.AddCell(test3);
            tblAsset.AddCell(test3);
            tblAsset.AddCell(test3);
            tblAsset.AddCell(test4);

            tblAsset.AddCell(test3);
            tblAsset.AddCell(test3);
            tblAsset.AddCell(test3);
            tblAsset.AddCell(test4);

            tblAsset.AddCell(test3);
            tblAsset.AddCell(test3);
            tblAsset.AddCell(test3);
            tblAsset.AddCell(test4);

            tblAsset.AddCell(test3);
            tblAsset.AddCell(test3);
            tblAsset.AddCell(test3);
            tblAsset.AddCell(test4);

            tblAsset.AddCell(test3);
            tblAsset.AddCell(test3);
            tblAsset.AddCell(test3);
            tblAsset.AddCell(test4);

            tblAsset.AddCell(test5);
            tblAsset.AddCell(test5);
            tblAsset.AddCell(test5);
            tblAsset.AddCell(test5);
            tblconclusion.AddCell(subconclusioncell);
            tblconclusion.AddCell(subconclusion1);
            Phrase phrase = new Phrase(Environment.NewLine);
            doc.Add(phrase);
            doc.Add(table1);
            doc.Add(formatTop);
            doc.Add(table2);
            //doc.Add(phrase);
            doc.Add(tblSuppliers);
            doc.Add(table3);
            doc.Add(tblAsset);
            doc.Add(tblconclusion);
            doc.Close();

            byte[] file = ms.ToArray();
            //var output = new MemoryStream();
            //output.Write(file, 0, file.Length);
            //output.Position = 0;
            //var f = new FileStreamResult(output, "application/pdf");
            return file;
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
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "infor@whsic.org".ToString(), 265, 45, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "www.whsic.org".ToString(), 265, 37, 0);
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