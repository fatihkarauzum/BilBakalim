using BilBakalim.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BilBakalim.Web.Controllers
{
    public class SinifController : Controller
    {
        // GET: Sinif
        BilBakalimContext db = new BilBakalimContext();
        public ActionResult Index()
        {
            return View(db.SinifKategori.ToList());
        }
        public ActionResult Oyunlar(int id)
        {
            return View(db.Sinif.Where(x => x.SinifKategoriID == id).ToList());
        }
        public ActionResult SinifEkle()
        {
            return View(db.Dİl.ToList());
        }
        public ActionResult SinifDetay(int id)
        {
            Sinif u = db.Sinif.Where(x => x.ID == id).FirstOrDefault();
            return View(u);
        }
      
    }


}