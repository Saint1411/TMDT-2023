using Facebook;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TMDT_Nhom_05.Models;

namespace TMDT_Nhom_05.Controllers
{
    public class FacebookGoogleController : Controller
    {
        TMDTEntities db = new TMDTEntities();
        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                //client_id = ConfigurationManager.AppSettings["FbAppId"],
                //client_Secret = ConfigurationManager.AppSettings["FbAppSecret"],
                client_id = "1416446399128373",
                client_Secret = "ea521030315817c8713ae516aed8b613",
                //Redirect_uri = RedirectUri.AbsoluteUri,
                //reponse_type = "code",
                Redirect_uri = "https://localhost:44330/FacebookGoogle/FacebookCallback",
                scope = "public_profile,email",
                reponse_type = "code"
            });
            ViewBag.Url = loginUrl;
            return View();
        }
        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_Secret = ConfigurationManager.AppSettings["FbAppSecret"],
                Redirect_uri = "https://localhost:44330/FacebookGoogle/FacebookCallback",
                code = code
            });
            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string userName = me.userName;
                string firstname = me.first_Name;
                string lastname = me.last_Name;
                string middlename = me.middle_Name;
                var user = new Customer();
                user.EmailCus = email;
                user.NameCus = firstname + ' ' + middlename + ' ' + lastname;
                user.UserCus = userName;

                var resultInsert = new Dao().InsertForFaceboook(user);
                if (resultInsert > 0)
                {
                    ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                    Session["TaiKhoan"] = User;
                    return RedirectToAction("Index", "Home");
                }
            }
            return Redirect("/");
        }
       
    }
}