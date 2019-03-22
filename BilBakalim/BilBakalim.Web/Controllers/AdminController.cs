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
            k.menuler = db.Menu.Where(x => x.Aktif == true).ToList();
            k.roller = db.MenuRol.Where(x => x.RolId == p.RolID).ToList();
            return PartialView(k);
        }

        [HttpGet]
        public ActionResult YetkiBulunamadi()
        {
            return View();
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

        [HttpGet]
        public ActionResult RolListesi()
        {
            

            return View(db.Rol.ToList());
        }

        [HttpGet]
        public ActionResult RolDuzenle(int id)
        {

            Rol r = db.Rol.Where(x => x.ID == id).FirstOrDefault();
            return View(r);
        }

        [HttpPost]
        public ActionResult RolDuzenle(Rol k)
        {
            try
            {
                Rol r = db.Rol.Where(x => x.ID == k.ID).FirstOrDefault();
                if (r == null)
                {
                    return RedirectToAction("Hata", "Admin");
                }
                r.RolAdi = k.RolAdi;
                r.Aciklama = k.Aciklama;
                db.SaveChanges();
                TempData["GenelMesaj"] = "Profil bilgileri başarılı bir şekilde güncellenmiştir.";
                return RedirectToAction("RolListesi");
            }
            catch (Exception)
            {
                return Redirect("/Admin/Hata");
            }

        }

        [HttpGet]
        public ActionResult RolEkle()
        {

            return View();
        }

        [HttpPost]
        public ActionResult RolEkle(Rol k)
        {
            try
            {
                Rol kont = db.Rol.Where(x => x.RolAdi == k.RolAdi).SingleOrDefault();
                if (kont != null)
                {
                    ViewData["Hata"] = "Kayıtlı bir Rol Eklediniz!";
                    return View();
                }

                else
                {
                    if (k != null)
                    {
                        db.Rol.Add(k);
                        db.SaveChanges();
                        TempData["uyari"] = k.RolAdi + " isimli Rol basarı ile eklenmiştir";
                        return RedirectToAction("RolListesi");
                    }
                    else
                    {
                        ViewData["Hata"] = "Boş Geçmeyiniz";
                        return View();
                    }
                }
            }
            catch (Exception)
            {
                TempData["tehlikeli"] = "Rol eklerken hata olustu";
                return RedirectToAction("RolListesi");
                
            }

        }

        [HttpPost]
        public ActionResult RolSil(int id)
        {
            Rol b = db.Rol.Where(x => x.ID == id).SingleOrDefault();
            if (b == null)
            {
                return Json(false);
            }
            else
            {
                try
                {
                    if (b.RolAdi == "Admin")
                    {
                        return Json("admin");
                    }                   
                    db.Rol.Remove(b);
                    db.SaveChanges();
                    return Json(true);
                }
                catch (Exception)
                {
                    return Json("FK");
                }

            }
        }

        public ActionResult Hata()
        {
            return View();
        }
    }
}