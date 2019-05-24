using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rotativa;
using BilBakalim.Data;
using BilBakalim.Web.App_Classes;

namespace BilBakalim.Web.Controllers
{
    public class AdminPrintController : Controller
    {
        BilBakalimContext db = new BilBakalimContext();
        // GET: AdminPrint

        [HttpGet]
        public ActionResult Sinif()
        {
            var kategori = db.SinifKategori.ToList();
            ViewBag.kategori = new SelectList(kategori, "ID", "KategoriAdi");
            return View();
        }

        public PartialViewResult altKategoriDropdown(int id)
        {
            var sinif = db.Sinif.Where(x => x.SinifKategoriID == id).ToList();
            ViewBag.sinif = new SelectList(sinif, "ID", "Ad");
            return PartialView();
        }
        public ActionResult SinifPrint(SinifFilter list)
        {

            try
            {
                List<Rapor> uc = db.Rapor.Include("Sinif").Include("Kullanici").Where(x => x.SınıfID == list.altKategoriID &&x.OyunZaman>=list.EklenmeTarihi &&x.KisiSayisi<list.OyunMiktarı).ToList();
                var report = new ViewAsPdf("SinifPrint", uc);
                return report;

            }
            catch (Exception)
            {
                return Redirect("/Admin/Hata");
            }

        }
    }
}