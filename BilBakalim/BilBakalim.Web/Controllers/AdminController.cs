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

        [HttpGet]
        public ActionResult KullaniciGuncelle(int id)
        {
            ViewBag.Roller = db.Rol.ToList();
            Kullanici k = db.Kullanici.Where(x => x.ID == id).FirstOrDefault();
            return View(k);

        }

        [HttpPost]
        public ActionResult KullaniciGuncelle(Kullanici k)
        {
            try
            {

                Kullanici ku = db.Kullanici.Where(x => x.ID==k.ID).FirstOrDefault();
                if (ku == null)
                {
                    return RedirectToAction("Hata", "Admin");
                }
                ku.Adi = k.Adi;
                ku.Soyadi = k.Soyadi;
                ku.Email = k.Email;
                ku.Sifre = k.Sifre;              
                db.SaveChanges();
                TempData["GenelMesaj"] = "Kullanıcı güncelleme işlemi başarılı bir şekilde tamamlanmıştır.";
                return RedirectToAction("KullaniciListesi");
            }
            catch (Exception)
            {
                return Redirect("/Admin/Hata");
            }
        }
        [HttpPost]
        public ActionResult KullaniciSil(int id)
        {
            Kullanici b = db.Kullanici.Where(x => x.ID == id).SingleOrDefault();
            if (b == null)
            {
                return Json(false);
            }
            else
            {
                try
                {
                    db.Kullanici.Remove(b);
                    db.SaveChanges();
                    return Json(true);
                }
                catch (Exception)
                {
                    return Json("FK");
                }

            }
        }
    }
}