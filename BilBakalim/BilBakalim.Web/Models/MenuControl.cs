using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BilBakalim.Data;

namespace BilBakalim.Web.Models
{
    public class MenuControl
    {
        public List<Menu> menuler { get; set; }
        public List<MenuRol> roller { get; set; }
    }
}