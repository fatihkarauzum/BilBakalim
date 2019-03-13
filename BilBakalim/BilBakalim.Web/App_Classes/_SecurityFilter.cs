using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BilBakalim.Data;

namespace BilBakalim.Web.App_Classes
{
    public class _SecurityFilter : ActionFilterAttribute
    {
        BilBakalimContext db = new BilBakalimContext();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;
            string metot = filterContext.HttpContext.Request.RequestType;
            if (HttpContext.Current.Session["Kullanici"] == null)
            {
                if (controllerName == "Admin")
                {
                    filterContext.Result = new RedirectResult("/Login/Login");
                }
                else
                {
                    return;
                }
            }
            else
            {
                Kullanici p = (Kullanici)HttpContext.Current.Session["Kullanici"];
                if (controllerName == "Home" || controllerName == "Login")
                {
                    return;
                }
                else
                {
                    if (actionName == "MenuGetir")
                    {
                        return;
                    }

                    if (metot == "POST")
                    {
                        
                    }
                    else
                    {
                        Menu currentMenu = db.Menu.Where(x => x.Action == actionName && x.Controller == controllerName).SingleOrDefault();
                        if (currentMenu != null)
                        {
                            MenuRol rol = db.MenuRol.Where(x => x.RolId == p.RolID && x.MenuId == currentMenu.ID).SingleOrDefault();
                            if (rol == null)
                            {
                                if (actionName == "YetkiBulunamadi")
                                {
                                    return;
                                }
                                filterContext.Result = new RedirectResult("/Admin/YetkiBulunamadi");
                            }
                            else
                            {
                                if (controllerName != currentMenu.Controller && actionName != currentMenu.Action)
                                {
                                    filterContext.Result = new RedirectResult("/" + currentMenu.Controller + "/" + currentMenu.Action);
                                }
                                return;
                            }
                        }
                        else
                        {
                            return;
                        }
                    }


                }

            }
            base.OnActionExecuting(filterContext);
        }
    }
}