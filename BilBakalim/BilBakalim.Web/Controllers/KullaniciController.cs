using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BilBakalim.Data;
using BilBakalim.Web.App_Classes;

namespace BilBakalim.Web.Controllers
{
    public class KullaniciController : Controller
    {
        private BilBakalimContext db = new BilBakalimContext();
        // GET: Kullanici
        [HttpGet]
        public ActionResult Yetkiler(int id)
        {
            Rol r = db.Rol.Where(x => x.ID == id).FirstOrDefault();

            if (r == null)
            {
                return RedirectToAction("Hata", "Admin");
            }

            ViewBag.Yetkileri = db.MenuRol.Where(x => x.RolId == r.ID).ToList();
            ViewBag.Menuler = db.Menu.ToList();
            return View(r);
        }

        [HttpPost]
        public ActionResult Yetkiler(int RolID, string menuler)
        {

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
                    Menu alt = db.Menu.Where(x => x.Adi == s).FirstOrDefault();
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
                ViewBag.Menuler = db.Menu.ToList();
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

            ViewBag.Dil = new SelectList(db.Dİl.ToList(), "ID", "Adi");
            ViewBag.SinifKat = new SelectList(db.SinifKategori.ToList(), "ID", "KategoriAdi");           
            return View(new Sinif());
        }

        [HttpPost]
        public ActionResult SinifEkle(Sinif k ,bool? gor, HttpPostedFileBase resimGelen)
        {
            Resim o = new Resim();
            ResimKategori l = new ResimKategori();

            if (resimGelen == null)
            {
              
                //l = db.ResimKategori.Where(x => x.KategoriAdi == "SinifSoru").SingleOrDefault();
                //o.ResimKategoriID = l.ID;
                //o.Url = "bos.jpg";
                //db.Resim.Add(o);
                //db.SaveChanges();

                //k.ResimID = o.ID;
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
            db.Sinif.Add(k);
            db.SaveChanges();

           
            Session["Sinif"] = k;
            return RedirectToAction("SoruEkle");

           
            //return View(new Sinif());
        }



        [HttpGet]
        public ActionResult SoruEkle()

        {

            return View(new Sorular());
        }



        [HttpPost]
    public ActionResult SoruEkle(Sorular sr , int DogruCont,bool? Odul, HttpPostedFileBase resimGelen)

        {
            Resim o = new Resim();
            ResimKategori l = new ResimKategori();

            if (resimGelen == null)
            {
                //sr.Sinif.Resim.Url = "bos.png";
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
            if (Odul != false)
            {
                sr.Odul = Odul;
            }
            else
            {
                sr.Odul = false;
            }
            Sinif s = (Sinif)Session["Sinif"];
            sr.SinifID = s.ID;
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
            db.SaveChanges();

            return View();

        }



    }






    }


