using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using BilBakalim.Data;

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
                    }else if (menuler.Count != 0)
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
    }
}