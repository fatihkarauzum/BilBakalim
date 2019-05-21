using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rotativa;
using BilBakalim.Data;

namespace BilBakalim.Web.Controllers
{
    public class AdminPrintController : Controller
    {
        BilBakalimContext db = new BilBakalimContext();
        // GET: AdminPrint
        public ActionResult SoruRapor(int id)
        {
            try
            {
                List<Rapor> uc = db.Rapor.Where(x => x.SınıfID == id).ToList();
                var report = new ViewAsPdf("SoruRapor", uc);               
                return report;

            }
            catch (Exception)
            {
                return Redirect("/Admin/Hata");
            }

        }
    }
}