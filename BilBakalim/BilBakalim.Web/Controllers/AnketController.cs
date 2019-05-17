using BilBakalim.Data;
using BilBakalim.Web.App_Classes;
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
            return View(db.Anket.Include("Resim").Include("Kullanici").Include("AnketSoru").Where(x => x.Durum == true).ToList());
        }

        public ActionResult AnketEkle()
        {
            ViewBag.Dil = new SelectList(db.Dİl.ToList(), "ID", "Adi");
            return View();
        }

        [HttpPost]
        public ActionResult AnketEkle(Anket k, bool? gor, HttpPostedFileBase resimGelen)
        {
            Resim o = new Resim();
            ResimKategori l = new ResimKategori();

            if (resimGelen == null)
            {

                Resim b = db.Resim.Where(x => x.Url == "/Content/Resimler/Anket/default.png").SingleOrDefault();
                k.ResimID = b.ID;
            }
            else
            {

                string yeniResimAdi = "";
                ResimIslemleri r = new ResimIslemleri();
                yeniResimAdi = r.Ekle(resimGelen, "Anket");
                //yeniResimAdi = new ResimIslem().Ekle(resimGelen);

                if (yeniResimAdi == "uzanti")
                {
                    ViewBag.Dil = new SelectList(db.Dİl.ToList(), "ID", "Adi");
                    ViewData["Hata"] = "Lütfen .png veya .jpg uzantılı dosya giriniz.";
                    return View();
                }
                else if (yeniResimAdi == "boyut")
                {
                    ViewBag.Dil = new SelectList(db.Dİl.ToList(), "ID", "Adi");
                    ViewData["Hata"] = "En fazla 1MB boyutunda dosya girebilirsiniz.";
                    return View();
                }
                else
                {


                    l = db.ResimKategori.Where(x => x.KategoriAdi == "Anket").SingleOrDefault();
                    o.ResimKategoriID = l.ID;
                    o.Url = yeniResimAdi;
                    db.Resim.Add(o);
                    db.SaveChanges();

                    k.ResimID = o.ID;
                    //k.Resim.Url = yeniResimAdi;

                }
            }

            k.GoruntulenmeSayisi = 0;
            Kullanici c = (Kullanici)Session["Kullanici"];
            k.OlusturmaTarihi = DateTime.Now;
            if (gor != null)
            {
                k.Gorunurluk = gor;
            }
            else
            {
                k.Gorunurluk = false;
            }
            k.Durum = true;
            k.KullaniciID = c.ID;
            db.Anket.Add(k);
            db.SaveChanges();


            Session["Anket"] = k;
            return RedirectToAction("AnketSoruEkle");
        }


        public ActionResult AnketSoruEkle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AnketSoruEkle(AnketSoru sr, HttpPostedFileBase resimGelen)
        {
            Resim o = new Resim();
            ResimKategori l = new ResimKategori();

            if (resimGelen == null)
            {
                Resim b = db.Resim.Where(x => x.Url == "/Content/Resimler/Anket/default.png").SingleOrDefault();
                sr.MedyaID = b.ID;
            }

            else
            {

                string yeniResimAdi = "";
                ResimIslemleri r = new ResimIslemleri();
                yeniResimAdi = r.Ekle(resimGelen, "Anket");
                //yeniResimAdi = new ResimIslem().Ekle(resimGelen);

                if (yeniResimAdi == "uzanti")
                {
                    ViewBag.Dil = new SelectList(db.Dİl.ToList(), "ID", "Adi");
                    ViewData["Hata"] = "Lütfen .png veya .jpg uzantılı dosya giriniz.";
                    return View();
                }
                else if (yeniResimAdi == "boyut")
                {
                    ViewBag.Dil = new SelectList(db.Dİl.ToList(), "ID", "Adi");
                    ViewData["Hata"] = "En fazla 1MB boyutunda dosya girebilirsiniz.";
                    return View();
                }
                else
                {


                    l = db.ResimKategori.Where(x => x.KategoriAdi == "Anket").SingleOrDefault();
                    o.ResimKategoriID = l.ID;
                    o.Url = yeniResimAdi;
                    db.Resim.Add(o);
                    db.SaveChanges();

                    sr.MedyaID = o.ID;

                }
            }


            db.AnketSoru.Add(sr);

            Anket s = (Anket)Session["Anket"];
            sr.SinifID = s.ID;

            db.SaveChanges();
            ViewBag.id = s.ID;
            return View();

        }


        public ActionResult AnketDetay(int id)
        {
            ViewBag.soru = db.AnketSoru.Where(x => x.SinifID == id).ToList();
            return View(db.Anket.Include("Resim").Include("Kullanici").Where(x => x.ID == id).SingleOrDefault());
        }


        public ActionResult AnketDuzenle(int id)
        {
            var dil = db.Dİl.ToList();
            ViewBag.dil = new SelectList(dil, "ID", "Adi");

            return View(db.Anket.Include("AnketSoru").Include("Resim").Where(x => x.ID == id).SingleOrDefault());
        }

        [HttpPost]
        public ActionResult AnketDuzenle(Anket s, HttpPostedFileBase resimGelen)
        {
            Anket yeni = db.Anket.Where(x => x.ID == s.ID).SingleOrDefault();
            yeni.Ad = s.Ad;
            yeni.Aciklama = s.Aciklama;
            yeni.Gorunurluk = s.Gorunurluk;
            yeni.LisanID = s.LisanID;
            //resim işlemi

            Resim o = new Resim();
            ResimKategori l = new ResimKategori();

            if (resimGelen == null)
            {
                db.SaveChanges();
                TempData["GenelMesaj"] = "Anket güncelleme işlemi başarılı bir şekilde tamamlanmıştır.";
                return RedirectToAction("AnketDetay", s.ID);
            }
            else
            {

                string yeniResimAdi = "";
                ResimIslemleri r = new ResimIslemleri();
                yeniResimAdi = r.Ekle(resimGelen, "Anket");
                //yeniResimAdi = new ResimIslem().Ekle(resimGelen);

                if (yeniResimAdi == "uzanti")
                {
                    ViewBag.dil = new SelectList(db.Dİl.ToList(), "ID", "Adi");

                    ViewData["Hata"] = "Lütfen .png veya .jpg uzantılı dosya giriniz.";
                    return View();
                }
                else if (yeniResimAdi == "boyut")
                {
                    ViewBag.dil = new SelectList(db.Dİl.ToList(), "ID", "Adi");

                    ViewData["Hata"] = "En fazla 1MB boyutunda dosya girebilirsiniz.";
                    return View();
                }
                else
                {
                    new ResimIslemleri().Sil(resimGelen.ToString(), "Anket");

                    l = db.ResimKategori.Where(x => x.KategoriAdi == "Anket").SingleOrDefault();
                    o.ResimKategoriID = l.ID;
                    o.Url = yeniResimAdi;
                    db.Resim.Add(o);
                    db.SaveChanges();
                    yeni.ResimID = o.ID;

                    db.SaveChanges();
                    return RedirectToAction("AnketDetay", new { id = s.ID });

                }
            }

        }
    }
}