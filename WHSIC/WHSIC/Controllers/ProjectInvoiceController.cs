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
    public class ProjectInvoiceController : Controller
    {
        ProjectInvoicesBusiness pib = new ProjectInvoicesBusiness();
        ProjectBusiness pb = new ProjectBusiness();
        public ActionResult GetProjectInvoices()
        {
            return View();
        }
        public FileStreamResult Showfile(int? id)
        {
            Stream output = new MemoryStream(getContract((int)id).Invoice);
            return new FileStreamResult(output, "application/pdf");
        }
        public ProjectInvoiceViewModel getContract(int id)
        {
            return pib.GetbyID(id);
        }
        private byte[] readFileContents(HttpPostedFileBase file)
        {
            Stream fileStream = file.InputStream;
            var mStreamer = new MemoryStream();
            mStreamer.SetLength(fileStream.Length);
            fileStream.Read(mStreamer.GetBuffer(), 0, (int)fileStream.Length);
            mStreamer.Seek(0, SeekOrigin.Begin);
            byte[] fileBytes = mStreamer.GetBuffer();
            return fileBytes;
        }

        public ActionResult AddProjectInvoice(int projectID, int ConstructorID)
        {
            ViewBag.feed = null;
            ViewBag.ConID = ConstructorID;
            ViewBag.ProjID = projectID;
            
            return View();
        }
        [HttpPost]
        public ActionResult AddProjectInvoice(ProjectInvoiceViewModel model, HttpPostedFileBase file,FormCollection form)
        {
            ViewBag.ConID = form["ConID"];
            ViewBag.ProjID = form["ProjID"];
            try
            {
               
                    ProjectViewModel project = pb.GetbyID(Convert.ToInt32(form["ProjID"]));
                   

                    model.Invoice = readFileContents(file);
                    model.ProjectID = Convert.ToInt32(form["ProjID"]);
                    pib.AddProjectInvoice(model);

                    project.Amount = project.Amount + Convert.ToDecimal(model.Amount);
                    pb.UpdateProject(project);
                    ViewBag.feed = "Invoice is uploaded successfully";
            }
            catch(Exception v)
            {

            }
            ViewBag.ConID = form["ConID"];
            ViewBag.ProjID = form["ProjID"];
            return View();
        }
    }
}