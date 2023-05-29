using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TMDT_Nhom_05.Models;

namespace TMDT_Nhom_05.App_Start
{
    public class CAdminRole : AuthorizeAttribute
    {
        public int idRole { get; set; }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            AdminUser adminSession = (AdminUser)HttpContext.Current.Session["user"];
            if (adminSession != null)
            {
                #region
                //check quyền
                TMDTEntities db = new TMDTEntities();
                var count = db.Roles.Count(m => m.idAdmin == adminSession.ID && m.idFunction == idRole);
                if (count != 0)
                {
                    return;
                }
                else
                {
                    var returnUrl = filterContext.RequestContext.HttpContext.Request.RawUrl;
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new
                        {
                            controller = "ErrorPage",
                            action = "OverRole",
                            area = "Admin",
                            returnUrl = returnUrl.ToString()
                        }));
                }
                #endregion
                return; 
            }
            else
            {
                var returnUrl = filterContext.RequestContext.HttpContext.Request.RawUrl;
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new
                    {
                        controller = "ProductAdmin",
                        action = "LogIn",
                        area = "Admin",
                        returnUrl = returnUrl.ToString()
                    }));
            }
        }
    }
}