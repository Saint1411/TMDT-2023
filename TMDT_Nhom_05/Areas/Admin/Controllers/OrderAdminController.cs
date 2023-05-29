using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TMDT_Nhom_05.App_Start;
using TMDT_Nhom_05.Models;

namespace TMDT_Nhom_05.Areas.Admin.Controllers
{
    public class OrderAdminController : Controller
    {
        TMDTEntities db = new TMDTEntities();
        [CAdminRole(idRole = 4)]
        public ActionResult OrderList(string fromDate = null, string toDate = null)
        {
            try
            {
                if (string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate))
                {
                    var lstOrder = db.OrderProes.OrderBy(o => o.DateOrder).ToList();
                    decimal totalPay = (decimal)lstOrder.Sum(o => o.TotalPay);
                    ViewBag.TotalPay = totalPay;
                    return View(lstOrder);
                }
                else
                {
                    DateTime fromDt = DateTime.MinValue;
                    DateTime toDt = DateTime.MaxValue;
                    if (!string.IsNullOrEmpty(fromDate))
                    {
                        fromDt = DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (!string.IsNullOrEmpty(toDate))
                    {
                        toDt = DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        toDt = toDt.AddDays(1);
                    }
                    var lstOrder = db.OrderProes.Where(o => o.DateOrder >= fromDt && o.DateOrder < toDt).OrderBy(o => o.DateOrder).ToList();
                    decimal totalPay = (decimal)lstOrder.Sum(o => o.TotalPay);
                    ViewBag.TotalPay = totalPay;
                    return View(lstOrder);
                }
            }
            catch
            {
                var lstOrder = db.OrderProes.OrderBy(o => o.DateOrder).ToList();
                decimal totalPay = (decimal)lstOrder.Sum(o => o.TotalPay);
                ViewBag.TotalPay = totalPay;
                ViewBag.ThongBao = "Đinh dạng ngày tháng không hợp lệ";
                return View(lstOrder);
            }   
        }
        [HttpGet]
        public ActionResult EditOrderList(int? ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderPro orderPro = db.OrderProes.Where(n => n.ID == ID).FirstOrDefault();
            if (orderPro == null)
            {
                return HttpNotFound();
            }
            return View(orderPro);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditOrderList(OrderPro orderPro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderPro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("OrderList");

            }
            return View(orderPro);
        }
        [CAdminRole(idRole = 4)]
        public ActionResult NotConfirmOrderList(string fromDate = null, string toDate = null)
        {
            try
            {
                if (string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate))
                {
                    var lstOrder = db.OrderProes.Where(n => n.OrderState == 0).OrderBy(o => o.DateOrder).ToList();
                    decimal totalPay = (decimal)lstOrder.Sum(o => o.TotalPay);
                    ViewBag.TotalPay = totalPay;
                    return View(lstOrder);
                }
                else
                {
                    DateTime fromDt = DateTime.MinValue;
                    DateTime toDt = DateTime.MaxValue;
                    if (!string.IsNullOrEmpty(fromDate))
                    {
                        fromDt = DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (!string.IsNullOrEmpty(toDate))
                    {
                        toDt = DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        toDt = toDt.AddDays(1);
                    }
                    var lstOrder = db.OrderProes.Where(o => o.DateOrder >= fromDt && o.DateOrder < toDt && o.OrderState == 0).OrderBy(o => o.DateOrder).ToList();
                    decimal totalPay = (decimal)lstOrder.Sum(o => o.TotalPay);
                    ViewBag.TotalPay = totalPay;
                    return View(lstOrder);
                }
            }
            catch
            {
                var lstOrder = db.OrderProes.Where(n => n.OrderState == 0).OrderBy(o => o.DateOrder).ToList();
                decimal totalPay = (decimal)lstOrder.Sum(o => o.TotalPay);
                ViewBag.TotalPay = totalPay;
                ViewBag.ThongBao = "Đinh dạng ngày tháng không hợp lệ";
                return View(lstOrder);
            }
        }
        [CAdminRole(idRole = 4)]
        public ActionResult DeliveringOrderList(string fromDate = null, string toDate = null)
        {
            try
            {
                if (string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate))
                {
                    var lstOrder = db.OrderProes.Where(n => n.OrderState == 1).OrderBy(o => o.DateOrder).ToList();
                    decimal totalPay = (decimal)lstOrder.Sum(o => o.TotalPay);
                    ViewBag.TotalPay = totalPay;
                    return View(lstOrder);
                }
                else
                {
                    DateTime fromDt = DateTime.MinValue;
                    DateTime toDt = DateTime.MaxValue;
                    if (!string.IsNullOrEmpty(fromDate))
                    {
                        fromDt = DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (!string.IsNullOrEmpty(toDate))
                    {
                        toDt = DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        toDt = toDt.AddDays(1);
                    }
                    var lstOrder = db.OrderProes.Where(o => o.DateOrder >= fromDt && o.DateOrder < toDt && o.OrderState == 1).OrderBy(o => o.DateOrder).ToList();
                    decimal totalPay = (decimal)lstOrder.Sum(o => o.TotalPay);
                    ViewBag.TotalPay = totalPay;
                    return View(lstOrder);
                }
            }
            catch
            {
                var lstOrder = db.OrderProes.Where(n => n.OrderState == 1).OrderBy(o => o.DateOrder).ToList();
                decimal totalPay = (decimal)lstOrder.Sum(o => o.TotalPay);
                ViewBag.TotalPay = totalPay;
                ViewBag.ThongBao = "Đinh dạng ngày tháng không hợp lệ";
                return View(lstOrder);
            }
        }
        [CAdminRole(idRole = 4)]
        public ActionResult IsRemoveOrderList(string fromDate = null, string toDate = null)
        {
            try
            {
                if (string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate))
                {
                    var lstOrder = db.OrderProes.Where(n => n.OrderState == 3).OrderBy(o => o.DateOrder).ToList();
                    decimal totalPay = (decimal)lstOrder.Sum(o => o.TotalPay);
                    ViewBag.TotalPay = totalPay;
                    return View(lstOrder);
                }
                else
                {
                    DateTime fromDt = DateTime.MinValue;
                    DateTime toDt = DateTime.MaxValue;
                    if (!string.IsNullOrEmpty(fromDate))
                    {
                        fromDt = DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (!string.IsNullOrEmpty(toDate))
                    {
                        toDt = DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        toDt = toDt.AddDays(1);
                    }
                    var lstOrder = db.OrderProes.Where(o => o.DateOrder >= fromDt && o.DateOrder < toDt && o.OrderState == 3).OrderBy(o => o.DateOrder).ToList();
                    decimal totalPay = (decimal)lstOrder.Sum(o => o.TotalPay);
                    ViewBag.TotalPay = totalPay;
                    return View(lstOrder);
                }
            }
            catch
            {
                var lstOrder = db.OrderProes.Where(n => n.OrderState == 3).OrderBy(o => o.DateOrder).ToList();
                decimal totalPay = (decimal)lstOrder.Sum(o => o.TotalPay);
                ViewBag.TotalPay = totalPay;
                ViewBag.ThongBao = "Đinh dạng ngày tháng không hợp lệ";
                return View(lstOrder);
            }
        }
        [CAdminRole(idRole = 4)]
        public ActionResult OrderDetail(int? ID)
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

        [HttpGet]
        public ActionResult EditOrderDetail(int? ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail orderDetail = db.OrderDetails.Find(ID);
            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            return View(orderDetail);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditOrderDetail(OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("OrderDetail", new {ID = orderDetail.IDOrder });
            }
            return View(orderDetail);
        }

        [HttpGet]
        public ActionResult DeleteOrderDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail cate = db.OrderDetails.Find(id);
            if (cate == null)
            {
                return HttpNotFound();
            }
            return View(cate);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteOrderDetail(int id)
        {
            OrderDetail cate = db.OrderDetails.Find(id);
            db.OrderDetails.Remove(cate);
            db.SaveChanges();
            return RedirectToAction("OrderList");
        }

        public ActionResult DetailOrderDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail cate = db.OrderDetails.Where(n => n.ID == id).FirstOrDefault();
            if (cate == null)
            {
                return HttpNotFound();
            }
            return View(cate);
        }
    }
}