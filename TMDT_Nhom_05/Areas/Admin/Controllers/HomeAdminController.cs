using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TMDT_Nhom_05.Models;

namespace TMDT_Nhom_05.Areas.Admin.Controllers
{
    public class HomeAdminController : Controller
    {
        public ActionResult Index()
        {
            if(Session["user"] == null)
            {
                return RedirectToAction("LogIn");
            }
            
            else
                return View();
        }

        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(string user_name, string password)
        {
            TMDTEntities db = new TMDTEntities();
            var f_password = GetMD5(password);
            var userAdmin = db.AdminUsers.SingleOrDefault(n => n.NameUser.Equals(user_name) && n.PasswordUser.Equals(f_password));
            if (userAdmin != null)
            {
                Session["user"] = userAdmin;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Thông tin đăng nhập không chính xác";
                return View();
            }
        }

        public ActionResult LogOut()
        {
            Session.Remove("user");
            FormsAuthentication.SignOut();
            return RedirectToAction("LogIn");
        }
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

        TMDTEntities db = new TMDTEntities();
        [HttpGet]
        public ActionResult CreateRole()
        {
            ViewBag.AdminUsers = db.AdminUsers.Where(n=>n.ID != 1).ToList();
            ViewBag.Functions = db.Functions.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRole(int idAdmin, int idFunction, string note)
        {
            var existingRole = db.Roles.FirstOrDefault(r => r.idAdmin == idAdmin && r.idFunction == idFunction);

            if (existingRole != null)
            {
                ViewBag.ErrorMessage = "Người dùng đã có quyền này!";
                ViewBag.AdminUsers = db.AdminUsers.ToList();
                ViewBag.Functions = db.Functions.ToList();
                return View("CreateRole");
            }
            var role = new Role()
            {
                idAdmin = idAdmin,
                idFunction = idFunction,
                Note = note
            };
            ViewBag.SuccessMessage = "Phân quyền thành công";
            db.Roles.Add(role);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult RevokeRole(int id)
        //{
        //    var role = db.Roles.Find(id);
        //    db.Roles.Remove(role);
        //    db.SaveChanges();

        //    // Đặt thông báo vào ViewBag và trả về view 
        //    ViewBag.SuccessMessage = "Thu hồi quyền thành công";
        //    return View("Index");
        //}

    }
}