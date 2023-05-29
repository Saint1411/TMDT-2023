using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Facebook;
using System.Web.Mvc;
using System.Web.Security;
using TMDT_Nhom_05.Models;
using System.Configuration;
using System.Threading.Tasks;

namespace TMDT_Nhom_05.Controllers
{
    public class UserController : Controller
    {
        TMDTEntities db = new TMDTEntities();
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Customer cus)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(cus.NameCus))
                    ModelState.AddModelError(string.Empty, "Họ tên không được trống!");
                if (string.IsNullOrEmpty(cus.UserCus))
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được trống!");
                if (string.IsNullOrEmpty(cus.PassCus))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được trống!");
                if (string.IsNullOrEmpty(cus.EmailCus))
                    ModelState.AddModelError(string.Empty, "Email không được trống!");
                if (string.IsNullOrEmpty(cus.PhoneCus))
                    ModelState.AddModelError(string.Empty, "Điện thoại không được trống!");

                var customer = db.Customers.FirstOrDefault(k => k.UserCus == cus.UserCus);
                if(customer != null)
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập đã có người đăng ký!");
                if (ModelState.IsValid)
                {
                    cus.PassCus = GetMD5(cus.PassCus);
                    cus.ScorePay = 0;
                    cus.TypeCus = 1;
                    //db.Configuration.ValidateOnSaveEnabled = false;
                    db.Customers.Add(cus);
                    db.SaveChanges();
                    return RedirectToAction("LogIn");
                }
                else
                {
                    return View();
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(Customer cus)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(cus.UserCus))
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được trống!");
                if (string.IsNullOrEmpty(cus.PassCus))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được trống!");
                if (ModelState.IsValid)
                {
                    var Pass = GetMD5(cus.PassCus);
                    var customer = db.Customers.FirstOrDefault(k => k.UserCus == cus.UserCus && k.PassCus.Equals(Pass));
                    if (customer != null)
                    {
                        ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                        Session["TaiKhoan"] = customer;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                        ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không chính xác";
                }
            }
            return View();
        }

        public ActionResult LogOut()
        {
            Session.Remove("TaiKhoan");
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

        [HttpGet]
        public ActionResult EditUserAccount(int? IDCus)
        {
            try
            {
                if (IDCus == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Customer cus = db.Customers.Find(IDCus);
                return View(cus);
            }
            catch
            {
                return RedirectToAction("LogIn");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserAccount(Customer model, string OldPassword, string NewPassword)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingCustomer = db.Customers.Find(model.IDCus);

                    // Update customer properties
                    existingCustomer.NameCus = model.NameCus;
                    existingCustomer.PhoneCus = model.PhoneCus;
                    existingCustomer.EmailCus = model.EmailCus;
                    if (!string.IsNullOrEmpty(OldPassword) && !string.IsNullOrEmpty(NewPassword))
                    {
                        var OldPass = GetMD5(OldPassword);
                        if (existingCustomer.PassCus.Trim().Equals(OldPass.Trim()))
                        {
                            // Update password
                            ViewBag.SuccessMessage = "Đổi mật khẩu thành công!";
                            existingCustomer.PassCus = GetMD5(NewPassword);
                        }
                        else
                        {
                            ViewBag.OldPasswordError = "Mật khẩu hiện tại không chính xác";
                            return View(model);
                        }
                    }
                    db.Entry(existingCustomer).State = EntityState.Modified;
                    db.SaveChanges();

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Không thành công: " + ex.Message;
            }

            return View(model);
        }

        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        

    }
}