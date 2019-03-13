using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BilBakalim.Data;

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

        
    }


}