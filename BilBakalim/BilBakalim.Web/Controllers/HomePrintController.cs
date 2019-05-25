using BilBakalim.Data;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BilBakalim.Web.Controllers
{
    public class HomePrintController : Controller
    {
        BilBakalimContext db = new BilBakalimContext();
        // GET: HomePrint
        public ActionResult Raporlar(int id)
        {
            return View(db.Rapor.Include("Kullanici").Include("Sinif").Where(x => x.KullaniciID == id).ToList());
        }

        public ActionResult OyunPrint(int id)
        {

            try
            {
                Rapor uc = db.Rapor.Include("Sinif").Include("Kullanici").Where(x => x.ID == id).SingleOrDefault();
                var report = new ViewAsPdf("OyunPrint", uc);
                return report;

            }
            catch (Exception)
            {
                return Redirect("/Admin/Hata");
            }

        }
    }
}