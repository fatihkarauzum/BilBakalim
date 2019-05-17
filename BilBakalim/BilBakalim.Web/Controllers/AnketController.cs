using BilBakalim.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BilBakalim.Web.Controllers
{
    public class AnketController : Controller
    {
        BilBakalimContext db = new BilBakalimContext();
        // GET: Anket
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AnketListele()
        {
            return View(db.Anket.Include("Resim").Include("Kullanici").Include("AnketSoru").ToList());
        }
    }
}