using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TMDT_Nhom_05.Models;

namespace TMDT_Nhom_05.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        TMDTEntities db = new TMDTEntities();
        public ActionResult Product()
        {
            var listPro = db.Products.Where(n => n.Quantity > 0 && n.display == true).ToList();
            return View(listPro);
        }

        public ActionResult ProductInCategory(string IDCate)
        {
            if (string.IsNullOrEmpty(IDCate))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                var listProInCate = db.Products.Where(n => n.Category == IDCate && n.Quantity>0 && n.display == true).ToList();
                return View(listProInCate);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }


        public ActionResult Detail(int? id)
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

        public ActionResult Search(string searchString)
        {
            var listPro = db.Products.Where(n => n.Quantity > 0 && n.display == true).ToList();
            ViewBag.result = listPro;
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                listPro = listPro.Where(b => b.NamePro.ToLower().Contains(searchString)).ToList();
            }
            return View(listPro);
        }
    }
}