using WHSIC.BusinessLogic;
using WHSIC.Data;
using WHSIC.Model;
using PagedList;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace WHSIC.Controllers
{
    public class TempUploadController : Controller
    {
        TempBusiness tb = new TempBusiness();
        ProgramBusiness pb = new ProgramBusiness();
        UploadBusiness ub = new UploadBusiness();
        RejectedBusiness rb = new RejectedBusiness();
        

        ApplicationDbContext db = new ApplicationDbContext();
        [Authorize(Roles = "Admin")]
        public ActionResult Index(int? page)
        {
           
            if(tb.GetAllTempUpload().Count()==0)
            {
                ViewBag.v = "No Uploaded Contents to be confirmed";
            }

            const int pageSize = 8;
            int pageNo = (page ?? 1);
            var list = tb.GetAllTempUpload();
            return View(list.ToPagedList(pageNo, pageSize));
        }


        [HttpPost]
        public async Task<ActionResult> Create(ProgramUploadViewModel model, HttpPostedFileBase File)
        {
            string f = "Content not uploaded ";
            ModelState.Clear();
            ViewBag.pc = new SelectList(pb.GetAllProgramies(), "PName", "PName");
            try
            {
                   
                    byte[] data = new byte[File.ContentLength];
                    File.InputStream.Read(data, 0, File.ContentLength);

                    model.Image = data;
                     tb.AddTempUpload(model);
                    f = "Content is Uploaded to the system and also notification is sent to Bishop to confirm ";
            }
            catch (Exception n)
            {
                f += n.Message;
            }


            ViewBag.pc = new SelectList(pb.GetAllProgramies(), "PName", "PName");
            
            return RedirectToAction("Create", "ProgramUpload", new { f});
        }

        public ActionResult Approved(int? id)
        {
            try
            {
                TempUploadViewModel tuv = tb.GetbyID(Convert.ToInt32(id));
                if (tuv != null)
                {
                    ub.AddProgramUpload(tuv);
                    tb.DeleteTempUpload(Convert.ToInt32(id));
                }
            }
            catch(Exception b)
            {
                Response.Write(b.Message);
            }

            return RedirectToAction("Index");
        }
        public ActionResult Declined(int ? id)
        {
            TempUploadViewModel tu = tb.GetbyID(Convert.ToInt32(id));
            ViewBag.c = tu.ProgramId.ToString();   
            return View(tu);
        }
        [HttpPost]
        public ActionResult Declined(FormCollection c,string id)
        {
            try
             {

                int ids = Convert.ToInt32(id);
                if (c["comment"].Equals(""))
                {
                    c["comment"] = " ";
                }
                else
                {
                    TempUploadViewModel tu = tb.GetbyID(Convert.ToInt32(id));
                    tu.comment = c["comment"];

                    rb.AddRejectedUpload(tu);
                    tb.DeleteTempUpload(Convert.ToInt32(id));
                }
                return RedirectToAction("Index", "TempUpload");
            }
            catch(Exception m)
            {
                Response.Write(m.Message);
            }
            return View();
        }


       

    }
}