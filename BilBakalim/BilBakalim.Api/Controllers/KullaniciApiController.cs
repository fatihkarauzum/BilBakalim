using BilBakalim.Api.App_Class;
using BilBakalim.Data;
using BilBakalim.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;


namespace BilBakalim.Api.Controllers
{
    [RoutePrefix("api/KullaniciApi")]

    [AllowAnonymous]
    public class KullaniciApiController : ApiController
    {
        BilBakalimContext db = new BilBakalimContext();



        [Route("Kullanici")]
        public IEnumerable<Kullanici> GetKullanici()
        {

            return db.Kullanici.Include("Rol").ToList();

        }

        [Route("KullaniciGetir/{id:int}")]
        [HttpGet]
        public IHttpActionResult GetKullanici(int id)
        {
            Kullanici k = db.Kullanici.Include("Sinif").Include("Favori").Include("Takip").Include("KullaniciResim").Where(x => x.ID == id).FirstOrDefault();

            if (k != null)
            {
                return Ok(k);
            }
            else
            {
                return NotFound();
            }
        }

        [Route("KullaniciGuncelle")]
        [HttpPut]
        public IHttpActionResult PutKullanici([FromBody]Kullanici k)
        {
            try
            {
                Kullanici ku = db.Kullanici.Where(x => x.ID == k.ID).FirstOrDefault();
                if (ku == null)
                {
                    return NotFound();
                }
                else
                {
                    ku.Adi = k.Adi;
                    ku.Soyadi = k.Soyadi;
                    ku.Email = k.Email;
                    ku.Sifre = k.Sifre;
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception)
            {
                return BadRequest();

            }

        }


        [Route("KullaniciSil/{id:int}")]
        public IHttpActionResult DeleteKullanici(int id)
        {
            try
            {
                Kullanici b = db.Kullanici.Where(x => x.ID == id).SingleOrDefault();
                if (b == null)
                {
                    return NotFound();
                }
                else
                {
                    Rol k = db.Rol.Where(x => x.RolAdi == "Admin").SingleOrDefault();

                    if (b.RolID == k.ID)
                    {
                        return NotFound();
                    }
                    else
                    {
                        db.Kullanici.Remove(b);
                        db.SaveChanges();
                        return Ok();
                    }

                }

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [Route("RolListesi")]
        [HttpGet]
        public IEnumerable<Rol> GetRol()
        {
            return db.Rol.ToList();
        }

        [Route("RolGetir/{id:int}")]
        [HttpGet]
        public IHttpActionResult GetRol(int id)
        {
            Rol k = db.Rol.Where(x => x.ID == id).FirstOrDefault();

            if (k != null)
            {
                return Ok(k);
            }
            else
            {
                return NotFound();
            }
        }

        [Route("RolGuncelle")]
        [HttpPut]
        public IHttpActionResult PutRol([FromBody]Rol k)
        {
            try
            {
                Rol r = db.Rol.Where(x => x.ID == k.ID).FirstOrDefault();
                if (r == null)
                {
                    return NotFound();
                }
                else
                {
                    r.RolAdi = k.RolAdi;
                    r.Aciklama = k.Aciklama;
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception)
            {
                return BadRequest();

            }
        }

        [Route("Rolekle")]
        [HttpPost]
        public IHttpActionResult PostRol([FromBody]Rol k)
        {
            try
            {

                Rol r = db.Rol.Where(x => x.RolAdi == k.RolAdi).FirstOrDefault();
                if (r != null)
                {
                    return NotFound();
                }
                else
                {
                    db.Rol.Add(k);
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception)
            {
                return BadRequest();

            }
        }


        [Route("RolSil/{id:int}")]
        [HttpDelete]
        public IHttpActionResult DeleteRol(int id)
        {
            try
            {
                Rol b = db.Rol.Where(x => x.ID == id).SingleOrDefault();
                if (b == null)
                {
                    return BadRequest();
                }
                else
                {

                    if (b.RolAdi == "Admin")
                    {
                        return NotFound();
                    }
                    else
                    {
                        db.Rol.Remove(b);
                        db.SaveChanges();
                        return Ok();
                    }

                }
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [Route("SinifKategori")]
        [HttpGet]
        public IEnumerable<SinifKategori> GetSinifKategori()
        {
            return db.SinifKategori.Include("Resim").ToList();
        }

        [Route("SinifKategoriEkle")]
        [HttpPost]
        public IHttpActionResult PostSinifKategori([FromBody]BodyType k)
        {
            try
            {

                Resim o = new Resim();
                ResimKategori l = new ResimKategori();
                SinifKategori c = new SinifKategori();

                //MemoryStream ms = new MemoryStream(k.PicData);
                //Image returnImage = Image.FromStream(ms);

                if (k.PicData == null)
                {

                    Resim b = db.Resim.Where(x => x.Url == "/Content/Resimler/SinifSoru/default.png").SingleOrDefault();
                    c.ResimID = b.ID;
                }
                else
                {

                    string yeniResimAdi = "";
                    Resimislemleri r = new Resimislemleri();
                    // yeniResimAdi = r.Ekle((Convert.t)returnImage, "SinifSoru");
                    //yeniResimAdi = new ResimIslem().Ekle(resimGelen);

                    if (yeniResimAdi == "uzanti" || yeniResimAdi == "boyut")
                    {
                        return NotFound();
                    }

                    else
                    {

                        l = db.ResimKategori.Where(x => x.KategoriAdi == "SinifSoru").SingleOrDefault();
                        o.ResimKategoriID = l.ID;
                        o.Url = yeniResimAdi;
                        db.Resim.Add(o);
                        c.ResimID = o.ID;


                    }
                }

                SinifKategori kont = db.SinifKategori.Where(x => x.KategoriAdi.ToLower() == k.Ad.ToLower()).SingleOrDefault();
                if (kont != null)
                {
                    return BadRequest();
                }

                else
                {
                    if (k.Ad != null)
                    {
                        c.Aktif = true;
                        db.SinifKategori.Add(c);
                        db.SaveChanges();
                        return Ok();
                    }
                    else
                    {
                        return Conflict();
                    }
                }



            }
            catch (Exception)
            {
                return Conflict();

            }





        }


        [Route("SinifKategoriSil/{id:int}")]
        public IHttpActionResult DeleteSinifKategori(int id)
        {
            List<Sinif> Siniflar = db.Sinif.Where(x => x.SinifKategoriID == id).ToList();
            try
            {
                SinifKategori k = db.SinifKategori.Where(x => x.ID == id).SingleOrDefault();
                if (k != null)
                {
                    if (Siniflar.Count() != 0)
                    {
                        return BadRequest();
                    }

                    else
                    {
                        db.SinifKategori.Remove(k);
                        db.SaveChanges();
                        return Ok();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch
            {
                return NotFound();
            }
        }

        [Route("ResimKategori")]
        [HttpGet]
        public IHttpActionResult GetResimKategori()
        {
            List<ResimKategori> liste = new List<ResimKategori>();
            liste = db.ResimKategori.ToList();
            if (liste != null)
            {
                return Ok(liste);
            }
            else
            {
                return NotFound();
            }


        }


        [Route("Yetkiler/{id:int}")]
        [HttpPost]
        public IHttpActionResult PostYetkiler([FromUri]int id, [FromBody]string menuler)
        {

            try
            {
                //, MenuList list , IslemErisimList list2
                Rol r = db.Rol.Where(x => x.ID == id).FirstOrDefault(); // Düzenlenmek istenen Rolu bul

                if (r == null) // rol boş ise hata döndür
                {
                    return NotFound();
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
                    rol.RolId = id;
                    db.MenuRol.Add(rol);
                    db.SaveChanges();
                }
                // MenuList.RolKontrol(list, RolID);               
                #endregion

                //Sayfayı geri yükle
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }



        [Route("ViewBag/{id:int}")]
        [HttpGet]
        public IHttpActionResult GetYetkiler(int id)
        {

            ViewbagModel a = new ViewbagModel();
            Rol r = db.Rol.Where(x => x.ID == id).FirstOrDefault();
            if (r == null)
            {
                return NotFound();
            }
            a.Rol = r;
            a.Menu = db.Menu.Where(x => x.Aktif == true).ToList();
            a.MenuRol = db.MenuRol.Where(x => x.RolId == r.ID).ToList();


            return Ok(a);

        }

        [Route("SinifEkleViewbag")]
        [HttpGet]
        public IHttpActionResult GetSinifEkleViewbag()
        {
            ViewbagModel a = new ViewbagModel();
            a.SinifKat = db.SinifKategori.ToList();
            a.Dil = db.Dİl.ToList();
            return Ok(a);

        }

       

    }

}
