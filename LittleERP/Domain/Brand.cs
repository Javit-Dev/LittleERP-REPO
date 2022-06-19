using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleERP.Domain
{
    public class Brand
    {
        public int id { get; set; }
        public String name { get; set; }

        public Manage.ProductManage manage;
        public Brand()
        {
            manage = new Manage.ProductManage();
        }

        public void chargeBrands() {
            manage.chargeBrands();
        }

        public override bool Equals(object obj)
        {
            return obj is Brand brand &&
                   id == brand.id;
        }
    }
}
