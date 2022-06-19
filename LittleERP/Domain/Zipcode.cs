using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleERP.Domain
{
    public class Zipcode
    {
        public int id { get; set; }
        public String name { get; set; }
        public Manage.CustomerManage manage { get; set; }


        public Zipcode()
        {
            manage = new Manage.CustomerManage();
        }

        public void chargeZipcodes(City city, State state)
        {
            manage.chargeZipcodes(city,state);
        }

        public void searchZipcode(String zip) {
            manage.searchZipcode(zip);
        }

        public override bool Equals(object obj)
        {
            return obj is Zipcode zipcode &&
                   id == zipcode.id;
        }
    }
}
