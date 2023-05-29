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
    public class CategoryController : Controller
    {
        TMDTEntities db = new TMDTEntities();
        [CAdminRole(idRole = 1)]
        public ActionResult Index()
        {
            var listCategory = db.Categories.ToList();
            return View(listCategory);
        }

        public ActionResult CreateCate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateCate(Category category)
        {
            TMDTEntities db = new TMDTEntities();
            if (ModelState.IsValid)
            {
                try
                {

                    db.Categories.Add(category);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ViewBag.Message = "không thành công!!";
                }

            }
            return View(category);
        }

        [HttpGet]
        public ActionResult EditCate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category cate = db.Categories.Where(n => n.Id == id).FirstOrDefault();
            if (cate == null)
            {
                return HttpNotFound();
            }
            return View(cate);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditCate(Category cate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(cate);
        }

        [HttpGet]
        public ActionResult DeleteCate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category cate = db.Categories.Where(n => n.Id == id).FirstOrDefault();
            if (cate == null)
            {
                return HttpNotFound();
            }
            return View(cate);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCate(int id)
        {
            Category cate = db.Categories.Where(n => n.Id == id).FirstOrDefault();
            db.Categories.Remove(cate);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DetailCate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category cate = db.Categories.Where(n => n.Id == id).FirstOrDefault();
            if (cate == null)
            {
                return HttpNotFound();
            }
            return View(cate);
        }
    }
}