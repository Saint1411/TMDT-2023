using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TMDT_Nhom_05.App_Start;
using TMDT_Nhom_05.Models;

namespace TMDT_Nhom_05.Areas.Admin.Controllers
{
    public class CustomerController : Controller
    {
        TMDTEntities db = new TMDTEntities();
        [CAdminRole(idRole = 3)]

        public ActionResult CustomerList()
        {
            var cus = db.Customers.ToList();
            return View(cus);
        }
        public ActionResult CustomerAccount()
        {
            var cus = db.Customers.Where(n => !string.IsNullOrEmpty(n.UserCus) && !string.IsNullOrEmpty(n.PassCus)).ToList();
            return View(cus);
        }

        public ActionResult CreateCus()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateCus(Customer cus)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Customers.Add(cus);
                    db.SaveChanges();
                    return RedirectToAction("CustomerList");
                }
                catch (Exception)
                {
                    ViewBag.Message = "Thêm khách hàng thất bại!!";
                }
            }
            return View(cus);
        }

        [HttpGet]
        public ActionResult EditCus(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer cus = db.Customers.Where(n => n.IDCus == id).FirstOrDefault();
            if (cus == null)
            {
                return HttpNotFound();
            }
            return View(cus);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditCus(Customer cus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("CustomerList");
            }
            return View(cus);
        }
    }
}