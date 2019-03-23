using BilBakalim.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BilBakalim.Web.Models;
using BilBakalim.Web.App_Classes;

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
                Rol kont = db.Rol.Where(x => x.RolAdi.ToLower() == k.RolAdi.ToLower()).SingleOrDefault();
                if (kont != null)
                {
                    ViewData["Hata"] = "Kayıtlı bir Rol Eklediniz!";
                    return RedirectToAction("RolListesi");
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
                        return RedirectToAction("RolListesi");
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

        [HttpGet]
        public ActionResult SinifKategori()
        {
            return View(db.SinifKategori.ToList());

        }

        [HttpPost]
        public ActionResult SinifKategoriEkle(SinifKategori k, HttpPostedFileBase resimGelen)
        {

            try
            {
                Resim o = new Resim();
                ResimKategori l = new ResimKategori();

                if (resimGelen == null)
                {

                    Resim b = db.Resim.Where(x => x.Url == "/Content/Resimler/SinifSoru/default.png").SingleOrDefault();
                    k.ResimID = b.ID;
                }
                else
                {

                    string yeniResimAdi = "";
                    ResimIslemleri r = new ResimIslemleri();
                    yeniResimAdi = r.Ekle(resimGelen, "SinifSoru");
                    //yeniResimAdi = new ResimIslem().Ekle(resimGelen);

                    if (yeniResimAdi == "uzanti")
                    {
                        ViewBag.Dil = new SelectList(db.Dİl.ToList(), "ID", "Adi");
                        ViewBag.SinifKat = new SelectList(db.SinifKategori.ToList(), "ID", "KategoriAdi");
                        ViewData["Hata"] = "Lütfen .png veya .jpg uzantılı dosya giriniz.";
                        return RedirectToAction("SinifKategori");
                    }
                    else if (yeniResimAdi == "boyut")
                    {
                        ViewBag.Dil = new SelectList(db.Dİl.ToList(), "ID", "Adi");
                        ViewBag.SinifKat = new SelectList(db.SinifKategori.ToList(), "ID", "KategoriAdi");
                        ViewData["Hata"] = "En fazla 1MB boyutunda dosya girebilirsiniz.";
                        return RedirectToAction("SinifKategori");
                    }
                    else
                    {

                        l = db.ResimKategori.Where(x => x.KategoriAdi == "SinifSoru").SingleOrDefault();
                        o.ResimKategoriID = l.ID;
                        o.Url = yeniResimAdi;
                        db.Resim.Add(o);
                        k.ResimID = o.ID;
                       

                    }
                }

                    SinifKategori kont = db.SinifKategori.Where(x => x.KategoriAdi.ToLower() == k.KategoriAdi.ToLower()).SingleOrDefault();
                    if (kont != null)
                    {
                        TempData["Hata"] = "Kayıtlı bir Kategori Eklediniz!";
                        return RedirectToAction("SinifKategori");
                }

                    else
                    {
                        if (k != null)
                        {
                        k.Aktif = true;
                        db.SinifKategori.Add(k);
                            db.SaveChanges();
                            TempData["uyari"] = k.KategoriAdi + " isimli Kategori basarı ile eklenmiştir";
                            return RedirectToAction("SinifKategori");
                        }
                        else
                        {
                            ViewData["Hata"] = "Boş Geçmeyiniz";
                            return RedirectToAction("SinifKategori");
                    }
                    }                

                

            }
            catch (Exception)
            {
                TempData["tehlikeli"] = "Kategori eklerken hata olustu";
                return RedirectToAction("SinifKategori");

            }

        }

        [HttpGet]
        public ActionResult SinifKategoriDuzenle(int id)
        {

           
            SinifKategori r = db.SinifKategori.Where(x => x.ID == id).SingleOrDefault();
            return View(r);

        }

        [HttpPost]
        public ActionResult SinifKategoriDuzenle(SinifKategori k, HttpPostedFileBase resimGelen)
        {

            try
            {

                SinifKategori ob = db.SinifKategori.Where(x => x.ID == k.ID).SingleOrDefault();
                Resim o = new Resim();
                ResimKategori l = new ResimKategori();

                if (resimGelen == null)
                {

                    Resim b = db.Resim.Where(x => x.Url == "/Content/Resimler/SinifSoru/default.png").SingleOrDefault();
                    ob.ResimID = b.ID;
                }
                else
                {


                    string yeniResimAdi = "";
                    ResimIslemleri r = new ResimIslemleri();
                    yeniResimAdi = r.Ekle(resimGelen, "SinifSoru");
                    //yeniResimAdi = new ResimIslem().Ekle(resimGelen);

                    if (yeniResimAdi == "uzanti")
                    {
                        ViewBag.Dil = new SelectList(db.Dİl.ToList(), "ID", "Adi");
                        ViewBag.SinifKat = new SelectList(db.SinifKategori.ToList(), "ID", "KategoriAdi");
                        ViewData["Hata"] = "Lütfen .png veya .jpg uzantılı dosya giriniz.";
                        return RedirectToAction("SinifKategori");
                    }
                    else if (yeniResimAdi == "boyut")
                    {
                        ViewBag.Dil = new SelectList(db.Dİl.ToList(), "ID", "Adi");
                        ViewBag.SinifKat = new SelectList(db.SinifKategori.ToList(), "ID", "KategoriAdi");
                        ViewData["Hata"] = "En fazla 1MB boyutunda dosya girebilirsiniz.";
                        return RedirectToAction("SinifKategori");
                    }
                    else
                    {
                        if (ob.Resim.Url != null)
                        {
                            new ResimIslemleri().Sil(resimGelen.ToString(),"SinifSoru");

                        }

                        l = db.ResimKategori.Where(x => x.KategoriAdi == "SinifSoru").SingleOrDefault();
                        o.ResimKategoriID = l.ID;
                        o.Url = yeniResimAdi;
                        db.Resim.Add(o);
                        ob.ResimID = o.ID;
                    }
                }

                SinifKategori kont = db.SinifKategori.Where(x => x.KategoriAdi.ToLower() == k.KategoriAdi.ToLower()).SingleOrDefault();
                if (kont != null)
                {
                    TempData["Hata"] = "Kayıtlı bir Kategori Eklediniz!";
                    return RedirectToAction("SinifKategori");
                }

                else
                {
                    if (k != null)
                    {

                        ob.KategoriAdi = k.KategoriAdi;                        
                        db.SaveChanges();
                        TempData["uyari"] = k.KategoriAdi + " isimli Kategori basarı ile eklenmiştir";
                        return RedirectToAction("SinifKategori");
                    }
                    else
                    {
                        ViewData["Hata"] = "Boş Geçmeyiniz";
                        return RedirectToAction("SinifKategori");
                    }
                }


            }
            catch (Exception)
            {
                TempData["Hata"] = "Kategori eklerken hata olustu";
                return RedirectToAction("SinifKategori");

            }

        }


        public ActionResult SinifKatAktif(int id)
        {
            try
            {
                SinifKategori k = db.SinifKategori.Where(x => x.ID == id).SingleOrDefault();
                if (k != null)
                {
                    if (db.SinifKategori.Where(x => x.Aktif == true).Where(x => x.Aktif == true).Count() > 0)
                    {

                        k.Aktif = Convert.ToBoolean(k.Aktif) ? false : true;
                        if (k.Aktif == true)
                        {
                            TempData["uyari"] = "Kategori Aktif oldu";
                        }
                        else
                        {
                            TempData["uyari"] = "Kategori Pasif oldu";
                        }
                        db.SaveChanges();

                        return RedirectToAction("SinifKategori");
                    }
                    else
                    {
                        TempData["hata"] = "En az bir tane Kategori olmali";
                        return RedirectToAction("SinifKategori");
                    }
                }
                else
                {
                    TempData["hata"] = id + "li kategori bulunamamıştır.";
                    return RedirectToAction("SinifKategori");
                }

            }
            catch
            {
                TempData["uyari"] = "Kullanıcı silerken hata olustu";
                return RedirectToAction("SinifKategori");
            }


        }


        public ActionResult SinifKategoriSil(int id)
        {
            List<Sinif> Siniflar = db.Sinif.Where(x => x.SinifKategoriID == id).ToList();
            try
            {
                SinifKategori k = db.SinifKategori.Where(x => x.ID == id).SingleOrDefault();
                if (k != null)
                {
                    if (Siniflar.Count()!= 0)
                    {
                        TempData["hata"] = "Kategoriye ait siniflar mevcut. Lütfen silmek yerine pasif duruma alın.";
                        return RedirectToAction("SinifKategori");
                    }

                    else
                    {
                        db.SinifKategori.Remove(k);
                        db.SaveChanges();
                        TempData["Uyari"] = k.KategoriAdi + "İsimli Kategori başarılı şekilde silinmiştir.";
                        return RedirectToAction("SinifKategori");
                    }                   
                }
                else
                {
                    TempData["hata"] = id + "li kategori bulunamamıştır.";
                    return RedirectToAction("SinifKategori");
                }          
            }
            catch
            {
                TempData["hata"] = "Kullanıcı silerken hata olustu";
                return RedirectToAction("SinifKategori");
            }

        }


        public ActionResult ResimKategori()
        {
            return View(db.ResimKategori.ToList());


        }


        public ActionResult Hata()
        {
            return View();
        }
    }
}