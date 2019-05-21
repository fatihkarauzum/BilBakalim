using BilBakalim.Api.App_Class;
using BilBakalim.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;

namespace BilBakalim.Api.Controllers
{

    [RoutePrefix("api/LoginApi")]
    [AllowAnonymous]
    public class LoginApiController : ApiController
    {
        BilBakalimContext db = new BilBakalimContext();


        [Route("Login")]
        [HttpGet]
        public IHttpActionResult GetLogin(string eposta,string sifre)
        {
            try
            {
               
                Kullanici kullanici = new Kullanici();
                using (MD5 md5Hash = MD5.Create())
                {

                    string hash = Functions.Encrypt(sifre);
                    kullanici = db.Kullanici.Include("Rol").Include("KullaniciResim").Where(x => x.Email == eposta && x.Sifre == hash).SingleOrDefault();
                }

                if (kullanici == null)
                {
                    return NotFound();
                }

                else
                {
                    if (kullanici.Durum != null)
                    {

                        return Ok(kullanici);

                    }
                    else
                    {
                        return BadRequest();
                    }
                }

            }
            catch (Exception)
            {
                return NotFound();
            }
        }



        [Route("KullaniciKayit")]
        [HttpPost]
        public IHttpActionResult PostKullanici(Kullanici k)
        {

            Kullanici c = db.Kullanici.Where(x => x.Email == k.Email).SingleOrDefault();

            if (c != null)
            {
                return NotFound();
            }
            else
            {
                using (MD5 md5Hash = MD5.Create())
                {
                    string hash = Functions.Encrypt(k.Sifre);
                    try
                    {
                        k.ResimID = 1;
                        k.RolID = 2;
                        k.Durum = true;
                        k.Sifre = hash;
                        k.OlusturmaTarihi = DateTime.Now;
                        db.Kullanici.Add(k);
                        db.SaveChanges();
                        return Ok();
                    }
                    catch (Exception)
                    {
                        return BadRequest();
                    }
                }
            }

        }

    }
}

