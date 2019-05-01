using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilBakalim.Data.Interfaces
{
   public class ViewbagModel
    {
        public ViewbagModel()
        {
            
        }
      
        public List<SinifKategori> SinifKat { get; set; }
        public List<Dİl> Dil { get; set; }
        public List<Menu> Menu { get; set; }
        public List<MenuRol> MenuRol { get; set; }
        public Rol Rol { get; set; }

    }
}
