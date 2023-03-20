using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WHSIC.Data;
namespace WHSIC.Controllers
{
    public class PenaltyController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult GetPenalties(int?page)
        {
            const int pageSize = 5;
            int pageNo = (page ?? 1);
            var list = db.Penalties.ToList();
            return View(list.ToPagedList(pageNo, pageSize));
        }
    }
}