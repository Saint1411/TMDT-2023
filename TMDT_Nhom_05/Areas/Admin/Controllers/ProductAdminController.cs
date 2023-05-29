using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TMDT_Nhom_05.Models;
using TMDT_Nhom_05.App_Start;

namespace TMDT_Nhom_05.Areas.Admin.Controllers
{
    public class ProductAdminController : Controller
    {
        TMDTEntities db = new TMDTEntities();
        [CAdminRole(idRole = 2)]
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {

            var listPro = new List<Product>();
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                listPro = db.Products.Where(n => n.NamePro.ToLower().Contains(searchString)).ToList();
            }
            else
            {
                listPro = db.Products.ToList();
            }
            ViewBag.CurrentFilter = searchString;
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            listPro = listPro.OrderByDescending(n => n.ProductID).ToList();
            return View(listPro.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult CreateProduct()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateProduct(Product product)
        {
            try
            {
                if (product.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(product.ImageUpload.FileName);
                    string extension = Path.GetExtension(product.ImageUpload.FileName);
                    fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                    product.ImagePro = fileName;
                    product.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Areas/Admin/Content/img/img_products/"), fileName));
                }
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.Message = "Thêm sản phẩm không thành công!!";
            }
            return View(product);
        }

        [HttpGet]
        public ActionResult EditProduct(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product pro = db.Products.Where(n => n.ProductID == id).FirstOrDefault();
            if (pro == null)
            {
                return HttpNotFound();
            }
            return View(pro);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditProduct(Product product, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                if (product.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(product.ImageUpload.FileName);
                    string extension = Path.GetExtension(product.ImageUpload.FileName);
                    fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                    product.ImagePro = fileName;
                    product.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Areas/Admin/Content/img/img_products/"), fileName));
                }
                else
                    product.ImagePro = form["oldimage"];
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }



        [HttpGet]
        public ActionResult DeleteProduct(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product pro = db.Products.Find(id);
            if (pro == null)
            {
                return HttpNotFound();
            }
            return View(pro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProduct(int id)
        {
            Product proDelete = db.Products.Find(id);
            try
            {
                if (proDelete == null)
                {
                    return HttpNotFound();
                }

                db.Products.Remove(proDelete);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View(proDelete);
            }
        }


        public ActionResult DetailProduct(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product pro = db.Products.Find(id);
            if (pro == null)
            {
                return HttpNotFound();
            }
            return View(pro);
        }

    }
}