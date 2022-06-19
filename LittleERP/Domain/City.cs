using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleERP.Domain
{
    public class City
    {
        public int id { get; set; }
        public String name { get; set; }
        public Manage.CustomerManage manage { get; set; }


        public City()
        {
            manage = new Manage.CustomerManage();
        }

        public void chargeCities(State state)
        {
            manage.chargeCities(state);
        }

        public void chargeCities(Zipcode zip)
        {
            manage.chargeCities(zip);
        }

        public override bool Equals(object obj)
        {
            return obj is City city &&
                   id == city.id;
        }
    }
}
