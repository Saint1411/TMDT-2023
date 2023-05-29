using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TMDT_Nhom_05.Models;

namespace TMDT_Nhom_05.Controllers
{
    public class HistoryOrderController : Controller
    {
        // GET: HistoryOrder
        TMDTEntities db = new TMDTEntities();
        public ActionResult HistoryOrder()
        {
            Customer customer = Session["TaiKhoan"] as Customer;
            var orderProList = db.OrderProes.Where(n => n.IDCus == customer.IDCus).ToList();
            return View(orderProList);
        }
        public ActionResult DetailHistory(int? ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = db.OrderProes.Find(ID);
            if (order == null)
            {
                return HttpNotFound();
            }

            try
            {
                var listOderDetail = new List<OrderDetail>();
                listOderDetail = db.OrderDetails.Where(n => n.IDOrder == ID).ToList();
                return View(listOderDetail);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
    }
}