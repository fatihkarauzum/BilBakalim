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
                    new Menu(){Adi="Menü Listesi", AcilirMenu=false, Action="MenuListesi", Controller="Menu", Aktif=true, Icon="icon-group", ParentMenuID=null},
                };
                context.Menu.AddRange(menu);
                context.SaveChanges();
            }
        }
    }
}
