using BilBakalim.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}