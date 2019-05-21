using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using BilBakalim.Data;
using BilBakalim.Data.Interfaces;
using BilBakalim.Web.App_Classes;


namespace BilBakalim.Web.Controllers
{
    public class KullaniciController : Controller
    {
        static BilBakalimContext db = new BilBakalimContext();
        // GET: Kullanici

        [HttpGet]
        public ActionResult Yetkiler(int id)
        {

            ViewbagModel a=new ViewbagModel();
            HttpResponseMessage respone = Global.client.GetAsync("/api/KullaniciApi/ViewBag/"+id.ToString()).Result;
            if (respone.IsSuccessStatusCode)
            {
                a = respone.Content.ReadAsAsync<ViewbagModel>().Result;
                ViewBag.Yetkileri = a.MenuRol.ToList();
                ViewBag.Menuler = a.Menu.ToList();
                return View(a.Rol);
            }
            else
            {
                TempData["hata"] = "Roller Listelenirken Bir Hata Oluştu. Lütfen Daha Sonra Tekrar Deneyiniz.";
                return View();
            }





            //Rol r = db.Rol.Where(x => x.ID == id).FirstOrDefault();

            //if (r == null)
            //{
            //    return RedirectToAction("Hata", "Admin");
            //}

            //ViewBag.Yetkileri = db.MenuRol.Where(x => x.RolId == r.ID).ToList();
            //ViewBag.Menuler = db.Menu.Where(x => x.Aktif == true).ToList();
            //return View(r);
        }

        [HttpPost]
        public ActionResult Yetkiler(int RolID, string menuler)
        {

            //try
            //{
            //    HttpResponseMessage respone = Global.client.DeleteAsync("/api/KullaniciApi/Yetkiler/" + RolID.ToString(),liste).Result;
            //    if (respone.IsSuccessStatusCode)
            //    {
            //        //TempData["GenelMesaj"] = "Kullanıcı Silme işlemi başarılı bir şekilde tamamlanmıştır.";
            //        return Json(true);
            //    }

            //    else
            //    {
            //        //TempData["hata"] = "Kullanici Silme İşleminde Bir hata oluştu.";
            //        return Json(false);
            //    }

            //}
            //catch (Exception)
            //{

            //    return Json("FK");
            //}





            try
            {
                //, MenuList list , IslemErisimList list2
                Rol r = db.Rol.Where(x => x.ID == RolID).FirstOrDefault(); // Düzenlenmek istenen Rolu bul

                if (r == null) // rol boş ise hata döndür
                {
                    return RedirectToAction("Hata", "Admin");
                }

                #region,Menü Rolleri update
                //Bu role ait tüm yetkileri 
                List<MenuRol> menuRol = db.MenuRol.Where(x => x.RolId == r.ID).ToList();


                // Menü rollerinin silinmesi
                foreach (var item in menuRol)
                {
                    db.MenuRol.Remove(item);
                }
                db.SaveChanges(); // roller sıfırlandı.

                //Tüm rolleri yeniden yükle ve değişiklikleri kayıt et.
                string[] Menuparts = menuler.Split('^');
                Array.Reverse(Menuparts);
                List<Menu> Eklenenmenuler = new List<Menu>();
                for (int i = 0; i < Menuparts.Length; i++)
                {
                    string s = Menuparts[i].ToString();
                    Menu alt = db.Menu.Where(x => x.Adi == s && x.Aktif == true).FirstOrDefault();
                    if (alt != null)
                    {
                        Eklenenmenuler.Add(alt);
                    }
                }
                foreach (Menu item in Eklenenmenuler)
                {
                    MenuRol rol = new MenuRol();
                    rol.MenuId = item.ID;
                    rol.RolId = RolID;
                    db.MenuRol.Add(rol);
                    db.SaveChanges();
                }
                // MenuList.RolKontrol(list, RolID);
                ViewBag.Yetkileri = db.MenuRol.Where(x => x.RolId == r.ID).ToList();
                ViewBag.Menuler = db.Menu.Where(x => x.Aktif == true).ToList();
                #endregion

                //Sayfayı geri yükle
                TempData["GenelMesaj"] = "Profil yetkileri başarılı bir şekilde güncellenmiştir.";
                return RedirectToAction("RolListesi", "Admin");
            }
            catch (Exception)
            {
                return Redirect("/Admin/Hata");
            }
        }


        [HttpGet]
        public ActionResult SinifEkle()
        {

            ViewbagModel a = new ViewbagModel();
            HttpResponseMessage respone = Global.client.GetAsync("/api/KullaniciApi/SinifEkleViewbag/").Result;
            if (respone.IsSuccessStatusCode)
            {
                a = respone.Content.ReadAsAsync<ViewbagModel>().Result;
                ViewBag.Dil = new SelectList(a.Dil.ToList(), "ID", "Adi");
                ViewBag.SinifKat = new SelectList(a.SinifKat.ToList(), "ID", "KategoriAdi");

                return View(new Sinif());
            }
            else
            {
                TempData["hata"] = "Bir Hata Oluştu. Lütfen Daha Sonra Tekrar Deneyiniz.";
                return RedirectToAction("Index","Home");
            }

        }

        [HttpPost]
        public ActionResult SinifEkle(Sinif k ,bool? gor, HttpPostedFileBase resimGelen)
        {
            Resim o = new Resim();
            ResimKategori l = new ResimKategori();

            if (resimGelen == null)
            {

                Resim b = db.Resim.Where(x => x.Url == "/Content/Resimler/SinifSoru/default.jpg").SingleOrDefault();
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
                    return View();
                }
                else if (yeniResimAdi == "boyut")
                {
                    ViewBag.Dil = new SelectList(db.Dİl.ToList(), "ID", "Adi");
                    ViewBag.SinifKat = new SelectList(db.SinifKategori.ToList(), "ID", "KategoriAdi");
                    ViewData["Hata"] = "En fazla 1MB boyutunda dosya girebilirsiniz.";
                    return View();
                }
                else
                {
                    
                  
                    l= db.ResimKategori.Where(x => x.KategoriAdi == "SinifSoru").SingleOrDefault();
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
            k.KullaniciID = c.ID;
            k.Durum = true;
            db.Sinif.Add(k);
            db.SaveChanges();

           
            Session["Sinif"] = k;
            return RedirectToAction("SoruEkle");

           
            //return View(new Sinif());
        }



        [HttpGet]
        public ActionResult SoruEkle(int ? id)
        {
            if (id!=null)
            {
                ViewBag.id = id;
            }
            return View();
        }



        [HttpPost]
    public ActionResult SoruEkle(Sorular sr , int DogruCont,bool? Odul, HttpPostedFileBase resimGelen, int? id)

        {
            Resim o = new Resim();
            ResimKategori l = new ResimKategori();

            if (resimGelen == null)
            {
                Resim b = db.Resim.Where(x => x.Url == "/Content/Resimler/SinifSoru/default.jpg").SingleOrDefault();
                sr.MedyaID = b.ID;
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
                    return View();
                }
                else if (yeniResimAdi == "boyut")
                {
                    ViewBag.Dil = new SelectList(db.Dİl.ToList(), "ID", "Adi");
                    ViewBag.SinifKat = new SelectList(db.SinifKategori.ToList(), "ID", "KategoriAdi");
                    ViewData["Hata"] = "En fazla 1MB boyutunda dosya girebilirsiniz.";
                    return View();
                }
                else
                {


                    l = db.ResimKategori.Where(x => x.KategoriAdi == "SinifSoru").SingleOrDefault();
                    o.ResimKategoriID = l.ID;
                    o.Url = yeniResimAdi;
                    db.Resim.Add(o);
                    db.SaveChanges();

                    sr.MedyaID = o.ID;

                }
            }


                db.Sorular.Add(sr);
            if (Odul != null)
            {
                sr.Odul = Odul;
            }
            else
            {
                sr.Odul = false;
            }
            if (id!=null)
            {
                sr.SinifID = id;
                ViewBag.id = id;
            }
            else
            {
                Sinif s = (Sinif)Session["Sinif"];
                sr.SinifID = s.ID;
                ViewBag.id = s.ID;
            }
        
            switch (DogruCont)
                {

                    case 1:
                        sr.DogruCevap = "Cevap1";
                        break;
                    case 2:
                        sr.DogruCevap = "Cevap2";
                        break;
                    case 3:
                        sr.DogruCevap = "Cevap3";
                        break;
                    case 4:
                        sr.DogruCevap = "Cevap4";
                        break;


                    default:
                        break;
                }
            int? deneme = sr.Sure;
            db.SaveChanges();           
            return View();

        }




        [HttpGet]
        public ActionResult KullaniciDetay(int id)
        {
            //ViewBag.Siniflar = ctx.Database.SqlQuery<Sinif>("SELECT TOP 5 *FROM Sinif order by OlusturmaTarihi desc").Where(x => x.Gorunurluk == true).ToList();
           
            Kullanici a = new Kullanici();
            HttpResponseMessage respone = Global.client.GetAsync("/api/KullaniciApi/KullaniciGetir/" + id.ToString()).Result;
            if (respone.IsSuccessStatusCode)
            {
                a = respone.Content.ReadAsAsync<Kullanici>().Result;
               
                return View(a);
            }
            else
            {
                TempData["hata"] = "Kullanici Listelenirken Bir Hata Oluştu. Lütfen Daha Sonra Tekrar Deneyiniz.";
                return RedirectToAction("Index","Home");
            }

        }


        [HttpPost]
        public ActionResult KUllaniciGuncelle(Kullanici z)
        {

                if (z != null)
                {
                    Kullanici k = db.Kullanici.Where(x => x.ID == z.ID).FirstOrDefault();
                    k.Adi = z.Adi;                 
                    k.Aciklama = z.Aciklama;
                    k.Soyadi = z.Soyadi;
                    k.KullaniciAdi = z.KullaniciAdi;
                    db.SaveChanges();
                    TempData["GenelMesaj"] = "Güncelleme Islemi Basarılı.";
                    return RedirectToAction("KullaniciDetay/"+z.ID);
                    
                }
            else
            {
                TempData["hata"] = "Boş Geçilemez!";
                return View("KullaniciDetay/"+z.ID);
            }
                                      

        }

        [HttpPost]
        public ActionResult KullaniciSifreResimGuncelle(string SifreTekrar, Kullanici k, HttpPostedFileBase resimGelen)
        {
           
                Kullanici c = db.Kullanici.Where(x => x.ID == k.ID).SingleOrDefault();

            if(SifreTekrar=="" && k.Sifre == null)
            {
                if (resimGelen != null)
                {
                    KullaniciResim kg = new KullaniciResim();
                    string yeniResimAdi = "";
                    ResimIslemleri r = new ResimIslemleri();
                    yeniResimAdi = r.Ekle(resimGelen, "Kullanici");
                    //yeniResimAdi = new ResimIslem().Ekle(resimGelen);

                    if (yeniResimAdi == "uzanti")
                    {
                        TempData["hata"] = "Lütfen .png veya .jpg uzantılı dosya giriniz.";
                        return RedirectToAction("KullaniciDetay/" + k.ID);
                    }
                    else if (yeniResimAdi == "boyut")
                    {

                        TempData["hata"] = "En fazla 1MB boyutunda dosya girebilirsiniz.";
                        return RedirectToAction("KullaniciDetay/" + k.ID);
                    }
                    else
                    {
                        new ResimIslemleri().Sil(resimGelen.ToString(), "Kullanici");


                        kg.Url = yeniResimAdi;
                        db.KullaniciResim.Add(kg);
                        c.ResimID = kg.ID;
                        db.SaveChanges();
                        TempData["GenelMesaj"] = "Başarılı";
                        return RedirectToAction("KullaniciDetay/" + k.ID);

                    }
                }
                else
                {
                    TempData["hata"] = "Boş Geçmeyiniz.";
                    return RedirectToAction("KullaniciDetay/" + k.ID);

                }

                
            }


                if (SifreTekrar != k.Sifre)
                {
                    TempData["hata"] = "Sifreler uyusmadı";
                    return RedirectToAction("KullaniciDetay/" + k.ID);
                }
                else
                {

                        if (resimGelen != null)
                        {
                            KullaniciResim kg = new KullaniciResim();
                            string yeniResimAdi = "";
                            ResimIslemleri r = new ResimIslemleri();
                            yeniResimAdi = r.Ekle(resimGelen, "Kullanici");
                    //yeniResimAdi = new ResimIslem().Ekle(resimGelen);

                    if (yeniResimAdi == "uzanti")
                    {
                        TempData["hata"] = "Lütfen .png veya .jpg uzantılı dosya giriniz.";
                        return RedirectToAction("KullaniciDetay/" + k.ID);
                    }
                    else if (yeniResimAdi == "boyut")
                    {

                        TempData["hata"] = "En fazla 1MB boyutunda dosya girebilirsiniz.";
                        return RedirectToAction("KullaniciDetay/" + k.ID);
                    }
                    else
                    {
                        new ResimIslemleri().Sil(resimGelen.ToString(), "Kullanici");


                        kg.Url = yeniResimAdi;
                        db.KullaniciResim.Add(kg);
                        c.ResimID = kg.ID;
                        using (MD5 md5Hash = MD5.Create())
                        {
                            string hash = Functions.Encrypt(k.Sifre);
                            try
                            {
                                c.Sifre = hash;

                                db.SaveChanges();
                                TempData["GenelMesaj"] = "Başarılı";
                                return RedirectToAction("KullaniciDetay/" + k.ID);

                            }

                            catch (Exception)
                            {
                                TempData["hata"] = "Başarısız";
                                return RedirectToAction("KullaniciDetay/" + k.ID);
                            }
                        }


                    }
                         }
                    else
                    {
                        using (MD5 md5Hash = MD5.Create())
                        {
                            string hash = Functions.Encrypt(k.Sifre);
                            try
                            {
                                c.Sifre = hash;

                                    db.SaveChanges();
                                    TempData["GenelMesaj"] = "Başarılı";
                                    return RedirectToAction("KullaniciDetay/" + k.ID);
                               

                            }


                            catch (Exception)
                            {
                                TempData["hata"] = "Başarısız";
                                return RedirectToAction("KullaniciDetay/" + k.ID);
                            }
                        }
                    }                   
                   
                }

            }

           

        [HttpGet]
        public ActionResult KullaniciGuncelle(int id)
        {
            HttpResponseMessage respone = Global.client.GetAsync("/api/KullaniciApi/KullaniciGetir/" + id.ToString()).Result;           
            if (respone.IsSuccessStatusCode)
            {
                return View(respone.Content.ReadAsAsync<Kullanici>().Result);
            }
            else
            {
                TempData["hata"] = "Bir Hata Oluştu.";
                return RedirectToAction("KullaniciListesi");
            }
        }


        [HttpGet]
        public ActionResult TakipListesi(int id)
        {
            List<Takip> liste = db.Takip.Where(x => x.TakipEdenID == id).ToList();
            List<Kullanici> liste2 = new List<Kullanici>();
            ViewBag.takip = db.Takip.Include("Kullanici").ToList();
            foreach (var item in liste)
            {
               List<Kullanici> list = db.Kullanici.Include("KullaniciResim").Include("Sinif").Include("Takip").Where(x => x.ID == item.TakipEdilenID).ToList();
                liste2.AddRange(list);
            }

            return View(liste2);
        }


        [HttpGet]
        public ActionResult OyunListesi(int id)
        {

            ViewBag.Kategori = db.SinifKategori.Include("Sinif").ToList();
            List<Sinif> liste= db.Sinif.Include("Resim").Include("Favori").Include("Sorular").Include("SinifKategori").Where(x => x.KullaniciID == id).ToList();
            return View(liste);

        }

        public ActionResult OyunKatListesi(int id)
        {
            ViewBag.Kategori = db.SinifKategori.Include("Sinif").ToList();
            ViewBag.SinifKat = db.Sinif.Include("Resim").Include("Favori").Include("Sorular").Where(x => x.SinifKategoriID == id).ToList();
            List<Sinif> liste = db.Sinif.Include("Resim").Include("Favori").Include("Sorular").Include("SinifKategori").Where(x => x.KullaniciID == id).ToList();
            return View("OyunListesi",liste);

        }

        [HttpGet]
        public ActionResult FavoriListesi(int id)
        {
            ViewBag.Kategori = db.SinifKategori.Include("Sinif").ToList();
            List<Favori> liste = db.Favori.Include("Sinif").Where(x => x.KullaniciID == id).ToList();

            List<Sinif> liste2 = new List<Sinif>();

            foreach (var item in liste)
            {
                List<Sinif> list = db.Sinif.Include("Resim").Include("Favori").Include("Sorular").Include("SinifKategori").Where(x => x.ID == item.Sinif.ID).ToList();
                liste2.AddRange(list);
            }

            return View(liste2);


        }


        [HttpPost]
        public ActionResult FavoriEkleSil(int id)
        {
            Favori k = new Favori();
            Sinif a = new Sinif();
            a = db.Sinif.Where(x => x.ID == id).SingleOrDefault();

            //k = db.Favori.Where(x => x.ID == id).SingleOrDefault();

            Kullanici c = (Kullanici)Session["Kullanici"];
            if (c == null)
            {
                return Json("Gir");
            }
            else
            {
                k = db.Favori.Where(x => x.KullaniciID == c.ID && x.SinifID == a.ID).SingleOrDefault();
                if (k != null)
                {
                    db.Favori.Remove(k);
                    db.SaveChanges();
                    return Json("cik");
                }
                else
                {
                    Favori h = new Favori();
                    h.KullaniciID = c.ID;
                    h.SinifID = a.ID;
                    db.Favori.Add(h);
                    db.SaveChanges();
                    return Json("ekle");
                }
            }

           

        }

        [HttpGet]
        public ActionResult Iletisim()
        {
            return View(new Iletisim());
        }

        [HttpPost]
        public ActionResult Iletisim(Iletisim i)
        {
            try
            {
                if (i == null)
                {
                    ViewBag.sonuc = "Hata";
                    return View();
                }  
                else
                {
                    i.Tarih = DateTime.Now;
                    i.Telefon = i.Telefon.ToString();
                    db.Iletisim.Add(i);
                    db.SaveChanges();
                    TempData["basarı"] = "Mesajınız alınmıştır";
                    return View();

                }
            }
            catch (Exception)
            {
                TempData["tehlikeli"] = "Hata olustu";
                return View();

            }
        }

        public ActionResult AnketListele(int id)
        {
            List<Anket> liste = db.Anket.Include("Resim").Include("AnketSoru").Where(x => x.KullaniciID == id).ToList();
            return View(liste);
        }

        public ActionResult SinifDuzenle(int id)
        {
            var dil = db.Dİl.ToList();
            ViewBag.dil = new SelectList(dil, "ID", "Adi");
            var kategori = db.SinifKategori.ToList();
            ViewBag.kategori = new SelectList(kategori, "ID", "KategoriAdi");
            return View(db.Sinif.Include("SinifKategori").Include("Resim").Where(x => x.ID == id).SingleOrDefault());
        }

        [HttpPost]
        public ActionResult SinifDuzenle(Sinif s, HttpPostedFileBase resimGelen)
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
                TempData["GenelMesaj"] = "Sınıf güncelleme işlemi başarılı bir şekilde tamamlanmıştır.";
                return RedirectToAction("SinifAyrinti", "Home", new { id = s.ID });
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
                    return RedirectToAction("SinifDuzenle", s.ID);

                }
            }


        }

    }

    }


