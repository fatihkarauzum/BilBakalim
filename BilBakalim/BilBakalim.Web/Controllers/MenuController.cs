using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity.Migrations;
using System.Reflection;
using BilBakalim.Data;
using BilBakalim.Web.App_Classes;

namespace BilBakalim.Web.Controllers
{
    public class MenuController : Controller
    {
        BilBakalimContext db = new BilBakalimContext();

        // GET: Menu
        public ActionResult MenuListesi()
        {
            var menus = db.Menu.Where(x => x.Aktif == true).ToList();
            return View(menus);
        }

        [HttpPost]
        public ActionResult Sil(int id)
        {
            // Kullanici aktifKullanici = (Personel)Session["Kullanici"];

            Menu menu = db.Menu.Where(x => x.ID == id).SingleOrDefault();

            // var dateAndTime = DateTime.Now;
            // var date = dateAndTime.Date;

            if (menu == null)
            {
                return Json(false);
            }
            else
            {
                try
                {
                    List<MenuRol> menuRol = db.MenuRol.Where(x => x.MenuId == menu.ID).ToList();
                    List<Menu> menuler = db.Menu.Where(x => x.ParentMenuID == menu.ID).ToList();
                    if (menuRol.Count != 0)
                    {
                        return Json("menuRol");
                    }
                    else if (menuler.Count != 0)
                    {
                        return Json("menus");
                    }

                    menu.Aktif = false;
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
        public ActionResult MenuEkle()
        {
            var menus = db.Menu.Where(x => x.Aktif == true && x.Controller == null && x.Action == null).ToList();
            ViewBag.menus = menus;

            MvcHelper helper = new MvcHelper();

            var controllerList = helper.GetControllerNames().ToList();
            //var actionList = new ReflectedControllerDescriptor(typeof(ActionResult)).GetCanonicalActions();

            ViewBag.controllers = controllerList;

            //ViewBag.actions = actionList;

            return View();
        }

        [HttpPost]
        public ActionResult Ekle(Menu menu)
        {
            try
            {
                if (menu.Controller == "default")
                {
                    menu.Controller = null;
                }
                else if (menu.Controller != null)
                {
                    menu.Controller = menu.Controller.Substring(0, menu.Controller.Length - 10);
                }
                if (menu.Action == null)
                {

                }
                if (menu.ParentMenuID != null)
                {
                    Menu parentMenu = db.Menu.FirstOrDefault(x => x.ID == menu.ParentMenuID);
                    parentMenu.AcilirMenu = true;
                    menu.AcilirMenu = false;
                }
                else
                {
                    menu.AcilirMenu = false;
                }

                menu.Aktif = true;
                db.Menu.AddOrUpdate(menu);
                db.SaveChanges();
                TempData["GenelMesaj"] = "Menü ekleme işlemi başarılı bir şekilde tamamlanmıştır.";
                return RedirectToAction("MenuListesi");
            }
            catch (Exception)
            {
                return Redirect("/Admin/Hata");
            }
        }
        public ActionResult Duzenle(int id)
        {
            var menu = db.Menu.FirstOrDefault(x => x.Aktif == true && x.ID == id);
            var menus = db.Menu.Where(x => x.Aktif == true && x.Controller == null && x.Action == null).ToList();

            MvcHelper helper = new MvcHelper();

            var controllerList = helper.GetControllerNames().ToList();
            //var actionList = new ReflectedControllerDescriptor(typeof(ActionResult)).GetCanonicalActions();

            ViewBag.controllers = controllerList;
            ViewBag.menus = menus;

            //ViewBag.actions = actionList;

            return View(menu);
        }

        [HttpPost]
        public ActionResult Duzenle(Menu menu)
        {
            try
            {
                if (menu.Controller == "default")
                {
                    menu.Controller = null;
                }
                else if(menu.Controller != null)
                {
                    menu.Controller = menu.Controller.Substring(0, menu.Controller.Length - 10);
                }
                //if (menu.Action == null)
                //{

                //}
                if (menu.ParentMenuID != null)
                {
                    Menu parentMenu = db.Menu.FirstOrDefault(x => x.ID == menu.ParentMenuID);
                    parentMenu.AcilirMenu = true;
                }

                db.Menu.AddOrUpdate(menu);
                db.SaveChanges();
                TempData["GenelMesaj"] = "Menü Güncelleme işlemi başarılı bir şekilde tamamlanmıştır.";
                return RedirectToAction("MenuListesi");
            }
            catch (Exception)
            {
                return Redirect("/Admin/Hata");
            }
        }

    }
}