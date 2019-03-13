﻿using BilBakalim.Data;
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
            TempData["KullaniciID"] = id;
            Rol k = db.Rol.Where(x => x.ID == id).SingleOrDefault();
            return View(k);
        }

        [HttpPost]
        public ActionResult RolDuzenle(Rol k)
        {
            int id = (int)TempData["KullaniciID"];
            Rol c = db.Rol.Where(x => x.ID ==id ).SingleOrDefault();

            try
            {
                if (k.RolAdi!=null)
                {
                    if (c.RolAdi == k.RolAdi)
                    {
                        ViewData["hata"] = " aynı isimli Rol eklemeyiniz";
                        return View();
                    }
                    else
                    {
                         c.RolAdi = k.RolAdi;
                         c.Aciklama = k.Aciklama;
                         db.SaveChanges();
                        TempData["uyari"] = k.RolAdi + " isimli Rol basarı ile guncellenmiştir";
                        return RedirectToAction("RolListesi");
                    }

                }
                else
                {
                    ViewData["Hata"] = "Lütfen Rol adı giriniz.";
                    return View();
                }

               
            }
            catch (Exception)
            {
                ViewData["Hata"] = "Hata oluştu";
                return View();
                
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

        [HttpGet]
        public ActionResult RolSil(int id)
        {
            if (id != null)
            {
                Rol k = db.Rol.Where(x => x.ID == id).SingleOrDefault();

                db.Rol.Remove(k);
                db.SaveChanges();
                TempData["uyari"] = k.RolAdi + " isimli Rol basarı ile silinmiştir";
                return RedirectToAction("RolListesi");
            }
            else
            {
                TempData["tehlikeli"] = "Kullanıcı silerken hata olustu";
                return View();
            }
        }





    }
}