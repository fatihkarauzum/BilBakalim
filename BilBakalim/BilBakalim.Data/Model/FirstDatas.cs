using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilBakalim.Data.Model
{
    public class FirstDatas : IDatabaseInitializer<BilBakalimContext>
    {
        public void InitializeDatabase(BilBakalimContext context)
        {
            if (context.Menu.FirstOrDefault(x => x.Action == "MenuListesi" && x.Controller == "Menu") == null)
            {
                List<Menu> menu = new List<Menu>()
                {
                    new Menu()
                    {
                        Adi = "Menü Listesi", AcilirMenu = false, Action = "MenuListesi", Controller = "Menu",
                        Aktif = true, Icon = "icon-group", ParentMenuID = null
                    }
                };
                context.Menu.AddRange(menu);
                context.SaveChanges();               
            }

            if (context.Rol.FirstOrDefault(x => x.RolAdi == "Admin") == null)
            {
                List<Rol> rol = new List<Rol>()
                {
                    new Rol()
                    {
                        RolAdi = "Admin"
                    }
                };
                context.Rol.AddRange(rol);
                context.SaveChanges();
            }

            Rol admin = context.Rol.FirstOrDefault(x => x.RolAdi == "Admin");
            Menu menuListesi = context.Menu.FirstOrDefault(x => x.Adi == "Menü Listesi");
            if (context.MenuRol.FirstOrDefault(x => x.RolId == admin.ID && x.MenuId == menuListesi.ID) == null)
            {

                MenuRol rol = new MenuRol()
                {
                    RolId = context.Rol.First(x => x.RolAdi == "Admin").ID,
                    MenuId = context.Menu.First(x => x.Action == "MenuListesi" && x.Controller == "Menu").ID                
                };

                context.MenuRol.Add(rol);
                context.SaveChanges();
            }
            
        }
    }
}
