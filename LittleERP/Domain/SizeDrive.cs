using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleERP.Domain
{
    public class SizeDrive
    {
        public int id { get; set; }
        public String name { get; set; }

        public Manage.ProductManage manage;
        public SizeDrive() {
            manage = new Manage.ProductManage();
        }

        public void chargeSizes()
        {
            manage.chargeSizes();
        }

        public override bool Equals(object obj)
        {
            return obj is SizeDrive drive &&
                   id == drive.id;
        }
    }
}
