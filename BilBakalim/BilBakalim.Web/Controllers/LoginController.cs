﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BilBakalim.Data;
using BilBakalim.Web.App_Classes;

namespace BilBakalim.Web.Controllers
{
    public class LoginController : Controller
    {
        BilBakalimContext db = new BilBakalimContext();
        // GET: Login
        [Route("Login")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Mail, string Sifre)
        {
            try
            {
                #region,Personel ile giriş kontrolu
                Kullanici kullanici = new Kullanici();
                using (MD5 md5Hash = MD5.Create())
                {
                    string hash = Functions.Encrypt(Sifre);
                    kullanici = db.Kullanici.Include("Rol").Include("KullaniciResim").FirstOrDefault(x => x.Email == Mail && x.Sifre == hash);
                }

                if (kullanici == null)
                {
                    ViewBag.Hata = "Girdiğiniz bilgilere ait bir kullanıcı bulunamadı";
                    return View();
                }

                else
                {
                    if (kullanici.Durum != null)
                    {

                        Session["Kullanici"] = kullanici;
                        return RedirectToAction("Index", "Home");
                        #endregion
                    }
                    else
                    {
                        ViewBag.Hata = "Girdiginiz Kullanıcı Akfit Değilidir.";
                        return View();
                    }
                }

            }
            catch (Exception)
            {
                return Redirect("/Home/Index");
            }
        }

        [Route("Register")]
        [HttpGet]
        public ActionResult Kayit()
        {


            return View();
        }

        [HttpPost]
        public ActionResult Kayit(Kullanici kullanici)
        {

            try
            {
                HttpResponseMessage respone = Global.client.PostAsJsonAsync("/api/LoginApi/KullaniciKayit/", kullanici).Result;
                if (respone.IsSuccessStatusCode)
                {
                    TempData["GenelMesaj"] = "Kayıt işlemi başarılı bir şekilde tamamlanmıştır.";
                    return RedirectToAction("Login");
                }

                else if (respone.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    TempData["hata"] = "Mail Adresi Kullanımda.";
                    return RedirectToAction("Kayit");
                }
                else
                {
                    TempData["hata"] = "Kayıt İşleminde Bir hata oluştu.";
                    return RedirectToAction("Kayit");
                }

            }
            catch (Exception)
            {

                TempData["hata"] = "Kayıt İşleminde Bir hata oluştu.";
                return RedirectToAction("Kayit");
            }


            //using (MD5 md5Hash = MD5.Create())
            //{
            //    string hash = Functions.Encrypt(kullanici.Sifre);
            //    try
            //    {
            //        kullanici.ResimID = 1;
            //        kullanici.RolID = 2;
            //        kullanici.Durum = true;
            //        kullanici.Sifre = hash;
            //        db.Kullanici.Add(kullanici);
            //        db.SaveChanges();
            //        TempData["GenelMesaj"] = "Kaydınız başarı ile tamamlanmıştır";
            //        return RedirectToAction("Login");
            //    }
            //    catch (Exception )
            //    {
            //        ViewBag.Hata = "Kaydınız yapılırken bir hata ortaya çıktı";
            //        return Redirect("/Home/Index");
            //    }
            //}

        }

        [Route("LogOut")]
        public ActionResult Cikis()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}