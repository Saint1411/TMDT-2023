using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT_Nhom_05.Models;
using TMDT_Nhom_05.Others;

namespace TMDT_Nhom_05.Controllers
{
    public class GioHangController : Controller
    {
        public List<MuaMatHang> LayGioHang()
        {
            List<MuaMatHang> gioHang = Session["GioHang"] as List<MuaMatHang>;
            if(gioHang == null)
            {
                gioHang = new List<MuaMatHang>();
                Session["GioHang"] = gioHang;
            }
            return gioHang;
        }
        private int TinhTongSL()
        {
            int tong = 0;
            List<MuaMatHang> gionHang = LayGioHang();
            if (gionHang != null)
                tong = gionHang.Sum(sp => sp.Soluong);
            return tong;
        }
        private double TinhTongTien()
        {
            double tong = 0;
            List<MuaMatHang> gionHang = LayGioHang();
            if (gionHang != null)
                tong = gionHang.Sum(sp => sp.ThanhTien());
            return tong;
        }

        public ActionResult HienThiGioHang()
        {
            if (TempData["ThongBao"] != null)
            {
                ViewBag.ThongBao = TempData["ThongBao"].ToString();
            }
            List<MuaMatHang> gionHang = LayGioHang();
            if (gionHang == null || gionHang.Count == 0)
            {
                return RedirectToAction("Product", "Product");
            }
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return View(gionHang);
        }
        public ActionResult ThemSPVaoGioHang(int MaSP)
        {
            List<MuaMatHang> gioHang = LayGioHang();

            MuaMatHang sanPham = gioHang.FirstOrDefault(s => s.MaSP == MaSP);
            if(sanPham == null)
            {
                sanPham = new MuaMatHang(MaSP);
                gioHang.Add(sanPham);
            }
            else
            {
                sanPham.Soluong++;
            }
            TempData["ThongBao"] = "Đã thêm sản phẩm vào giỏ hàng thành công!";
            //return RedirectToAction("Detail", "Product", new { id = MaSP });
            return RedirectToAction("HienThiGioHang");
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }
        public ActionResult XoaMatHang(int MaSP)
        {
            List<MuaMatHang> gioHang = LayGioHang();
            var pro = gioHang.FirstOrDefault(s => s.MaSP == MaSP);
            if (pro != null)
            {
                gioHang.RemoveAll(s => s.MaSP == MaSP);
                return RedirectToAction("HienThiGioHang");
            }
            if (gioHang.Count == 0)
                return RedirectToAction("Product", "Product");
            return RedirectToAction("HienThiGioHang");
        }
        public ActionResult CapNhatMatHang(int? MaSP, int SoLuong)
        {
            List<MuaMatHang> gioHang = LayGioHang();
            var pro = gioHang.FirstOrDefault(s => s.MaSP == MaSP);
            if (pro != null)
            {
                pro.Soluong = SoLuong;
            }
            return RedirectToAction("HienThiGioHang");
        }
        public ActionResult DatHang()
        {
            List<MuaMatHang> gioHang = LayGioHang();
            if (gioHang == null || gioHang.Count == 0)
                return RedirectToAction("Product", "Product");
            double giamGia = TinhTongTien();
            if(Session["TaiKhoan"] as Customer != null)
            {
                var customer = Session["TaiKhoan"] as Customer;
                customer = db.Customers.Find(customer.IDCus);
                if (customer.TypeCus == 2)
                {
                    giamGia -= (giamGia * 20 / 100);
                    ViewBag.GiamGia = giamGia;
                }
                if (customer.TypeCus == 1)
                {
                    giamGia -= (giamGia * 10 / 100);
                    ViewBag.GiamGia = giamGia;
                }
            }
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return View(gioHang);
        }

        TMDTEntities db = new TMDTEntities();
        public ActionResult DongYDatHang()
        {
            Customer customer = Session["TaiKhoan"] as Customer;
            customer = db.Customers.Find(customer.IDCus);
            List<MuaMatHang> gioHang = LayGioHang();
            OrderPro orderPro = new OrderPro();
            orderPro.IDCus = customer.IDCus;
            orderPro.DatePay = DateTime.Now;
            orderPro.DateOrder = DateTime.Now;
            orderPro.Pay = false;
            decimal tinhTongTien  = (decimal)TinhTongTien();
            if (customer.TypeCus == 2)
            {
                tinhTongTien -= (tinhTongTien * 20 / 100);
            }
            if (customer.TypeCus == 1)
            {
                tinhTongTien -= (tinhTongTien * 10 / 100);
            }
            orderPro.TotalPay = tinhTongTien;
            orderPro.WhyNot = "";
            orderPro.OrderState = 0;
            db.OrderProes.Add(orderPro);
            db.SaveChanges();

            foreach(var pro in gioHang)
            {
                OrderDetail orderDetail = new OrderDetail();
                var product = db.Products.SingleOrDefault(s => s.ProductID == pro.MaSP);
                if (product != null)
                {
                    product.Quantity -= pro.Soluong;
                    orderDetail.IDOrder = orderPro.ID;
                    orderDetail.IDProduct = pro.MaSP;
                    orderDetail.Quantity = pro.Soluong;
                    orderDetail.UnitPrice = (double)pro.DonGia;
                    db.OrderDetails.Add(orderDetail);
                }
            }
            if (orderPro.TotalPay >= 100000)
            {
                // Cập nhật thông tin điểm tích lũy của khách hàng
                customer.ScorePay += 10;
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("HoanThanhDonHang");
        }

        [HttpPost]
        public ActionResult DongYDatHang(Customer customer)
        {
            customer.TypeCus = 0;
            db.Customers.Add(customer);
            db.SaveChanges();
            List<MuaMatHang> gioHang = LayGioHang();
            OrderPro orderPro = new OrderPro();
            orderPro.IDCus = customer.IDCus;
            orderPro.DatePay = DateTime.Now;
            orderPro.DateOrder = DateTime.Now;
            orderPro.Pay = false;
            orderPro.TotalPay = (decimal)TinhTongTien();
            orderPro.WhyNot = "";
            orderPro.OrderState = 0;
            db.OrderProes.Add(orderPro);
            db.SaveChanges();

            foreach (var pro in gioHang)
            {
                OrderDetail orderDetail = new OrderDetail();
                var product = db.Products.SingleOrDefault(s => s.ProductID == pro.MaSP);
                if (product != null)
                {
                    product.Quantity -= pro.Soluong;
                    orderDetail.IDOrder = orderPro.ID;
                    orderDetail.IDProduct = pro.MaSP;
                    orderDetail.Quantity = pro.Soluong;
                    orderDetail.UnitPrice = (double)pro.DonGia;
                    db.OrderDetails.Add(orderDetail);
                }
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("HoanThanhDonHang");
        }

        public ActionResult HoanThanhDonHang()
        {
            return View();
        }

        public ActionResult Payment()
        {
            decimal tinhTongTien = (decimal)TinhTongTien();
            string url = ConfigurationManager.AppSettings["Url"];
            string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
            string hashSecret = ConfigurationManager.AppSettings["HashSecret"];

            PayLib pay = new PayLib();

            pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_Amount", (tinhTongTien * 100).ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_IpAddr", Util.GetIpAddress()); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang"); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); //mã hóa đơn

            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);

            return Redirect(paymentUrl);
        }

        public ActionResult PaymentConfirm()
        {
            if (Request.QueryString.Count > 0)
            {
                string hashSecret = ConfigurationManager.AppSettings["HashSecret"];
                var vnpayData = Request.QueryString;
                PayLib pay = new PayLib();

                foreach (string s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        pay.AddResponseData(s, vnpayData[s]);
                    }
                }

                long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode");
                string vnp_SecureHash = Request.QueryString["vnp_SecureHash"];

                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret);

                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        ViewBag.Message = "Payment successful for order " + orderId + " | Transaction ID: " + vnpayTranId;

                        Customer customer = Session["TaiKhoan"] as Customer;
                        customer = db.Customers.Find(customer.IDCus);
                        List<MuaMatHang> gioHang = LayGioHang();

                        // Create a new OrderPro
                        OrderPro orderPro = new OrderPro();
                        orderPro.IDCus = customer.IDCus;
                        orderPro.DatePay = DateTime.Now;
                        orderPro.DateOrder = DateTime.Now;
                        orderPro.Pay = true;
                        orderPro.OrderState = 0;
                        db.OrderProes.Add(orderPro);
                        db.SaveChanges();

                        foreach (var pro in gioHang)
                        {
                            OrderDetail orderDetail = new OrderDetail();
                            var product = db.Products.SingleOrDefault(s => s.ProductID == pro.MaSP);
                            if (product != null)
                            {
                                product.Quantity -= pro.Soluong;
                                orderDetail.IDOrder = orderPro.ID;
                                orderDetail.IDProduct = pro.MaSP;
                                orderDetail.Quantity = pro.Soluong;
                                orderDetail.UnitPrice = (double)pro.DonGia;
                                db.OrderDetails.Add(orderDetail);
                            }
                        }
                        db.SaveChanges();
                    }
                    else
                    {
                        // Payment failed. Error code: vnp_ResponseCode
                        ViewBag.Message = "An error occurred while processing order " + orderId + " | Transaction ID: " + vnpayTranId + " | Error code: " + vnp_ResponseCode;
                    }
                }
                else
                {
                    ViewBag.Message = "An error occurred during processing.";
                }
            }
            Session["GioHang"] = null;
            return RedirectToAction("HoanThanhDonHang");
        }

    }
}