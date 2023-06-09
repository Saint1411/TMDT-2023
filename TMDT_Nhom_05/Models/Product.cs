﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TMDT_Nhom_05.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }

        [Required]
        public int ProductID { get; set; }
        [Required(ErrorMessage = "Vui long nhap ten san pham!")]
        public string NamePro { get; set; }
        [Required]
        public string DecriptionPro { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Giá nhập vào phải lớn hơn 0.")]
        public Nullable<decimal> Price { get; set; }
        public string ImagePro { get; set; }

        [NotMapped]
        public System.Web.HttpPostedFileBase ImageUpload { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng nhập vào phải lớn hơn 0.")]
        public Nullable<int> Quantity { get; set; }
        public Nullable<bool> display { get; set; }
        public virtual Category Category1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
