using BilBakalim.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BilBakalim.Web.Models;

namespace BilBakalim.Web.Controllers
{
    public class AdminController : Controller
    {
        BilBakalimContext db=new BilBakalimContext();
        // GET: Admin
        public ActionResult Index()
        {
            
            return View();
                
        }
        [HttpGet]
        public ActionResult KullaniciListesi()
        {
            return View(db.Kullanici.ToList());
        }

        public PartialViewResult MenuGetir()
        {
            Kullanici p = (Kullanici)Session["Kullanici"];

            MenuControl k = new MenuControl();
            k.menuler = db.Menu.ToList();
            k.roller = db.MenuRol.Where(x => x.RolId == p.RolID).ToList();
            return PartialView(k);
        }

        [HttpGet]
        public ActionResult YetkiBulunamadi()
        {
            return View();
        }
    }
}