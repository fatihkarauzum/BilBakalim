using BilBakalim.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BilBakalim.Web.Controllers
{
    public class HomeController : Controller
    {

        // GET: Home
        BilBakalimContext ctx = new BilBakalimContext();
        public ActionResult Index()
        {
            ViewBag.Siniflar = ctx.Sinif.Include("Resim").Where(x => x.Gorunurluk == true).ToList();
            ViewBag.Favori = ctx.Favori.ToList();
            ViewBag.SinifKategorileri = ctx.SinifKategori.Include("Resim").ToList();
            return View();
        }
        public ActionResult Gelistiriciler()
        {
            return View();
        }
        public PartialViewResult KategoriGetir()
        {
            ViewBag.Kategori=ctx.SinifKategori.ToList();
            return PartialView();
        }

        [HttpGet]
        public ActionResult KategoriAyrıntı(int id)
        {

            var ad = ctx.SinifKategori.Where(x => x.ID == id).FirstOrDefault();
            ViewBag.Kategori = ctx.SinifKategori.Include("Sinif").ToList();
            ViewBag.Oyunlar = ctx.Sinif.Include("Resim").Include("Favori").Include("Sorular").Where(x => x.SinifKategoriID == id && x.Gorunurluk==true);
            return View(ad);
        }

        
      [HttpGet]
        public ActionResult SinifAyrinti(int id)
        {
            return View( ctx.Sinif.Include("Resim").Include("Favori").Include("Sorular").Where(x => x.ID== id && x.Gorunurluk == true).ToList());
        }

    }
}