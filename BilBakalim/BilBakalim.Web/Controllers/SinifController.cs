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
            return View(db.SinifKategori.ToList());
        }
        public ActionResult Oyunlar(int id)
        {
            return View(db.Sinif.Where(x => x.SinifKategoriID == id).ToList());
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
            Sinif sinif = new Sinif();

            sinif.Ad = k.Ad;
            sinif.Aciklama = k.Aciklama;
            sinif.Gorunurluk = k.Gorunurluk;
            sinif.OlusturmaTarihi = DateTime.Now;
            sinif.GoruntulenmeSayisi = 0;
            sinif.LisanID = k.LisanID;
            //resim ıd
            sinif.KullaniciID = k.KullaniciID;
            sinif.SinifKategoriID = k.SinifKategoriID;
          

            db.Sinif.Add(sinif);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        public ActionResult SinifDetay(int id)
        {
            Sinif u = db.Sinif.Where(x => x.ID == id).FirstOrDefault();
            return View(u);
        }
        public ActionResult SinifGuncelle(int id)
        {
            var dil = db.Dİl.ToList();
            ViewBag.dil = new SelectList(dil, "ID", "Adi");

            var kategori = db.SinifKategori.ToList();
            ViewBag.kategori = new SelectList(kategori, "ID", "KategoriAdi");
            return View(db.Sinif.Where(x => x.ID == id).SingleOrDefault());
        }
        [HttpPost]
        public ActionResult SinifGuncelle(Sinif s,int id)
        {
            Sinif yeni = db.Sinif.Where(x => x.ID == s.ID).FirstOrDefault();
            yeni.Ad = s.Ad;
            yeni.Aciklama = s.Aciklama;
            yeni.Gorunurluk = s.Gorunurluk;
            yeni.LisanID = s.LisanID;
            //resim işlemi
            yeni.SinifKategoriID = s.SinifKategoriID;
            db.SaveChanges();
            return RedirectToAction("Oyunlar", new { id = id });
        }
        /*Sorular için gerekli işlemler*/
        public ActionResult Sorular(int id)
        {
            ViewBag.sorular = db.Sorular.Where(x => x.SinifID == id).ToList();
       
            return View(id);
        }
        public ActionResult SoruDetay(int id)
        {
            return View(db.Sorular.Where(x => x.ID == id).FirstOrDefault());
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
            return View(db.Sorular.Where(x => x.ID == id).SingleOrDefault());
        }
        [HttpPost]
        public ActionResult SoruGuncelle(Sorular s,int id)
        {
            Sorular soru = db.Sorular.Where(x => x.ID == s.ID).FirstOrDefault();
            soru.Soru = s.Soru;
            soru.Cevap1 = s.Cevap1;
            soru.Cevap2 = s.Cevap2;
            soru.Cevap3 = s.Cevap3;
            soru.Cevap4 = s.Cevap4;
            soru.Sure = s.Sure;
            soru.DogruCevap = s.DogruCevap;
            db.SaveChanges();
            return RedirectToAction("Sorular",new { id=id});
        }
      
    }


}