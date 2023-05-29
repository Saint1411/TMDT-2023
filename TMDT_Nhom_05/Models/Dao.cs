using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace TMDT_Nhom_05.Models
{
    public class Dao
    {
        TMDTEntities db = new TMDTEntities();
        public Dao() { 
            db = new TMDTEntities();
        }
        public long InsertForFaceboook(Customer entity)
        {
            var user = db.Customers.SingleOrDefault(x => x.NameCus == entity.NameCus);
            if (user == null)
            {
                db.Customers.Add(entity);
                db.SaveChanges();
                return entity.IDCus;
            }
            else
            {
                return user.IDCus;
            }
           
        }
    }
}