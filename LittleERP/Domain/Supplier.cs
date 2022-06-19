using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleERP.Domain
{
    public class Supplier
    {
        public int id { get; set; }
        public String Name { get; set; }

        public String Surname { get; set; }

        public String Address { get; set; }

        public int Phone { get; set; }

        public String page { get; set; }

        public Manage.SupplierManage manage;
        public Supplier()
        {
            manage = new Manage.SupplierManage();
        }

        public void readAll() {
            manage.readAll();
        }

    }
}
