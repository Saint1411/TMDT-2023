using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMDT_Nhom_05.Models
{
    public class MuaMatHang
    {
        TMDTEntities db = new TMDTEntities();
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public string HinhAnh { get; set; }
        public double DonGia { get; set; }
        public int Soluong { get; set; }

        public double ThanhTien()
        {
            return Soluong * DonGia;
        }

        public MuaMatHang(int MaSP)
        {
            this.MaSP = MaSP;
            var pro = db.Products.Single(p => p.ProductID == MaSP);
            this.TenSP = pro.NamePro;
            this.HinhAnh = pro.ImagePro;
            this.DonGia = double.Parse(pro.Price.ToString());
            this.Soluong = 1;
        }
    }
}