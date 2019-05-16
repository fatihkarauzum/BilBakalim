using BilBakalim.Data;
using BilBakalim.Web.App_Classes;
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
            return View(db.SinifKategori.Include("Resim").ToList());
        }
        public ActionResult Oyunlar(int id)
        {
            
            ViewBag.sorular = db.Sorular.ToList();
            List<Sinif> sinif = db.Sinif.Include("SinifKategori").Include("Kullanici").Where(x => x.SinifKategoriID == id).ToList();
            List<Sorular> soru= db.Sorular.ToList();
            
            return View(db.Sinif.Include("SinifKategori").Include("Kullanici").Where(x => x.SinifKategoriID == id).ToList());

        }

        [HttpGet]
        public ActionResult SinifEkle()
        {
            var dil = db.Dİl.ToList();
            ViewBag.dil = new SelectList(dil, "ID", "Adi");

            var kategori = db.SinifKategori.ToList();
            ViewBag.kategori = new SelectList(kategori, "ID", "KategoriAdi");

            return View(db.Dİl.ToList());
        }

        [HttpPost]
        public ActionResult SinifEkle(Sinif k,bool? gor, HttpPostedFileBase resimGelen)
        {
            //Sinif sinif = new Sinif();

            //sinif.Ad = k.Ad;
            //sinif.Aciklama = k.Aciklama;
            //sinif.Gorunurluk = k.Gorunurluk;
            //sinif.OlusturmaTarihi = DateTime.Now;
            //sinif.GoruntulenmeSayisi = 0;
            //sinif.LisanID = k.LisanID;
            ////resim ıd
            //sinif.KullaniciID = k.KullaniciID;
            //sinif.SinifKategoriID = k.SinifKategoriID;
            //db.Sinif.Add(sinif);



            //db.SaveChanges();
         

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
                    ViewBag.dil = new SelectList(db.Dİl.ToList(), "ID", "Adi");
                    ViewBag.kategori = new SelectList(db.SinifKategori.ToList(), "ID", "KategoriAdi");
                    ViewData["Hata"] = "Lütfen .png veya .jpg uzantılı dosya giriniz.";
                    return View();
                }
                else if (yeniResimAdi == "boyut")
                {
                    ViewBag.dil = new SelectList(db.Dİl.ToList(), "ID", "Adi");
                    ViewBag.kategori = new SelectList(db.SinifKategori.ToList(), "ID", "KategoriAdi");
                    ViewData["Hata"] = "En fazla 1MB boyutunda dosya girebilirsiniz.";
                    return View();
                }
                else
                {

                    l = db.ResimKategori.Where(x => x.KategoriAdi == "SinifSoru").SingleOrDefault();
                    o.ResimKategoriID = l.ID;
                    o.Url = yeniResimAdi;
                    k.GoruntulenmeSayisi = 0;
                    db.Resim.Add(o);
                    db.SaveChanges();

                    k.ResimID = o.ID;

                }
            }

            Kullanici c = (Kullanici)Session["Kullanici"];
            k.OlusturmaTarihi = DateTime.Now;           
            k.KullaniciID = c.ID;
            db.Sinif.Add(k);
            db.SaveChanges();
            return RedirectToAction("Index", "Sinif");
        }

        public ActionResult SinifDetay(int id)
        {
            Sinif u = db.Sinif.Include("Kullanici").Include("SinifKategori").Include("Resim").Where(x => x.ID == id).FirstOrDefault();
            return View(u);
        }
        public ActionResult SinifGuncelle(int id)
        {
            var dil = db.Dİl.ToList();
            ViewBag.dil = new SelectList(dil, "ID", "Adi");

            var kategori = db.SinifKategori.ToList();
            ViewBag.kategori = new SelectList(kategori, "ID", "KategoriAdi");
            return View(db.Sinif.Include("SinifKategori").Include("Resim").Where(x => x.ID == id).SingleOrDefault());
        }
        [HttpPost]
        public ActionResult SinifGuncelle(Sinif s,HttpPostedFileBase resimGelen)
        {      
            Sinif yeni = db.Sinif.Where(x => x.ID == s.ID).FirstOrDefault();
            yeni.Ad = s.Ad;
            yeni.Aciklama = s.Aciklama;
            yeni.Gorunurluk = s.Gorunurluk;
            yeni.LisanID = s.LisanID;
            //resim işlemi
            yeni.SinifKategoriID = s.SinifKategoriID;
            Resim o = new Resim();
            ResimKategori l = new ResimKategori();

            if (resimGelen == null)
            {
                db.SaveChanges();
               TempData["GenelMesaj"] = "Kullanıcı güncelleme işlemi başarılı bir şekilde tamamlanmıştır.";
               return RedirectToAction("SinifGuncelle",s.ID);
            }
            else
            {

                string yeniResimAdi = "";
                ResimIslemleri r = new ResimIslemleri();
                yeniResimAdi = r.Ekle(resimGelen, "SinifSoru");
                //yeniResimAdi = new ResimIslem().Ekle(resimGelen);

                if (yeniResimAdi == "uzanti")
                {
                    ViewBag.dil = new SelectList(db.Dİl.ToList(), "ID", "Adi");
                    ViewBag.kategori = new SelectList(db.SinifKategori.ToList(), "ID", "KategoriAdi");
                    ViewData["Hata"] = "Lütfen .png veya .jpg uzantılı dosya giriniz.";
                    return View();
                }
                else if (yeniResimAdi == "boyut")
                {
                    ViewBag.dil = new SelectList(db.Dİl.ToList(), "ID", "Adi");
                    ViewBag.kategori = new SelectList(db.SinifKategori.ToList(), "ID", "KategoriAdi");
                    ViewData["Hata"] = "En fazla 1MB boyutunda dosya girebilirsiniz.";
                    return View();
                }
                else
                {
                    new ResimIslemleri().Sil(resimGelen.ToString(), "SinifSoru");

                    l = db.ResimKategori.Where(x => x.KategoriAdi == "SinifSoru").SingleOrDefault();
                    o.ResimKategoriID = l.ID;
                    o.Url = yeniResimAdi;                   
                    db.Resim.Add(o);
                    db.SaveChanges();
                    yeni.ResimID = o.ID;

                    db.SaveChanges();
                    return RedirectToAction("SinifGuncelle", s.ID);

                }
            }

            
        }
        /*Sorular için gerekli işlemler*/
        public ActionResult Sorular(int id)
        {
            ViewBag.sorular = db.Sorular.Where(x => x.SinifID == id).ToList();
       
            return View(id);
        }


        public ActionResult SinifAktifEt(int id)
        {
            Sinif k = db.Sinif.Where(x => x.ID == id).SingleOrDefault();
            try
            {
                
                if (k != null)
                {
                    if (db.Sinif.Where(x => x.Durum == true).Where(x => x.Durum == true).Count() > 0)
                    {

                        k.Durum = Convert.ToBoolean(k.Durum) ? false : true;
                        if (k.Durum == true)
                        {
                            TempData["uyari"] = "Sınıf Aktif oldu";
                        }
                        else
                        {
                            TempData["uyari"] = "Sınıf Pasif oldu";
                        }
                        db.SaveChanges();

                        return RedirectToAction("Oyunlar", new { id = k.SinifKategoriID });
                    }
                    else
                    {
                        TempData["hata"] = "En az bir tane Sınıf olmali";
                        return RedirectToAction("Oyunlar", new {id= k.SinifKategoriID});
                    }
                }
                else
                {
                    TempData["hata"] = id + "li Sınıf bulunamamıştır.";
                    return RedirectToAction("Oyunlar", new { id = k.SinifKategoriID });
                }

            }
            catch
            {
                TempData["uyari"] = "Hata";
                return RedirectToAction("Oyunlar", new { id = k.SinifKategoriID });
            }
        }

        
        public ActionResult SoruDetay(int id)
        {
            var soru = db.Sorular.Include("Sinif").Where(x => x.ID == id).FirstOrDefault();
            ViewBag.sinif = db.Sinif.Include("Resim").Include("SinifKategori").Where(x => x.ID == soru.SinifID);
            return View(soru);
        }
        public ActionResult SoruSil(int id)
        {
           
            Sorular soru = db.Sorular.Where(x => x.ID == id).SingleOrDefault();
            int? git = soru.SinifID;
            db.Sorular.Remove(soru);
            db.SaveChanges();
            return RedirectToAction("Sorular", new { id = git});
        }

        public ActionResult SoruEkle(int id)
        {
            return View(db.Sinif.Where(x=>x.ID==id).SingleOrDefault());
        }

        [HttpPost]
        public ActionResult SoruEkle(Sorular s,int id)
        {
            Sorular kayit = new Sorular();
            kayit = s;
            db.Sorular.Add(kayit);
            db.SaveChanges();
            return RedirectToAction("Sorular",new { id=id});
        }
        public ActionResult SoruGuncelle(int id)
        {
            return View(db.Sorular.Include("Sinif").Where(x => x.ID == id).SingleOrDefault());
        }
        [HttpPost]
        public ActionResult SoruGuncelle(Sorular s)
        {
            Sorular soru = db.Sorular.Include("Sinif").Where(x => x.ID == s.ID).FirstOrDefault();
            soru.Soru = s.Soru;
            soru.Cevap1 = s.Cevap1;
            soru.Cevap2 = s.Cevap2;
            soru.Cevap3 = s.Cevap3;
            soru.Cevap4 = s.Cevap4;
            soru.Sure = s.Sure;
            soru.DogruCevap = s.DogruCevap;
            db.SaveChanges();
            return RedirectToAction("Sorular",new { id=soru.SinifID});
        }
      
    }


}