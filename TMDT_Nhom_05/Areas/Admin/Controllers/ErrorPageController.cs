using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMDT_Nhom_05.Areas.Admin.Controllers
{
    public class ErrorPageController : Controller
    {
        // GET: Admin/ErrorPage
        public ActionResult OverRole()
        {
            return View();
        }
        public ActionResult FailedEdit()
        {
            return View();
        }
    }
}