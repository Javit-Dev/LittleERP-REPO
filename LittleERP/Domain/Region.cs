using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleERP.Domain
{
    public class Region
    {
        public int id { get; set; }
        public String name { get; set; }
        public Manage.CustomerManage manage { get; set; }


        public Region() {
            manage = new Manage.CustomerManage();
        }

        public void chargeRegions()
        {
            manage.chargeRegions();
        }

        public void chargeRegions(Zipcode zip)
        {
            manage.chargeRegions(zip);
        }

        public override bool Equals(object obj)
        {
            return obj is Region region &&
                   id == region.id;
        }
    }
}
