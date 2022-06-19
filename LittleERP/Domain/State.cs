using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleERP.Domain
{
    public class State
    {
        public int id { get; set; }
        public String name { get; set; }
        public Manage.CustomerManage manage { get; set; }


        public State()
        {
            manage = new Manage.CustomerManage();
        }

        public void chargeStates(Region region)
        {
            manage.chargeStates(region);
        }
        public void chargeStates(Zipcode zip)
        {
            manage.chargeStates(zip);
        }

        public override bool Equals(object obj)
        {
            return obj is State state &&
                   id == state.id;
        }
    }
}
