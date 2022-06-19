using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleERP.Domain
{
    public class Customer
    {
        public int id { get; set; }
        public String NIF { get; set; }
        public String name { get; set; }
        public String surname { get; set; }

        public String email { get; set; }

        public string address { get; set; }
        public int phone { get; set; }

        public Zipcode zipcode { get; set; }
        public Region region { get; set; }
        public City city { get; set; }
        public State state { get; set; }

        public Manage.CustomerManage manage { get; set; }

        public Customer()
        {
            manage = new Manage.CustomerManage();
            id = -1;
        }

        public void insertCustomer()
        {
            manage.insertCustomer(this);
        }

        public void readAll()
        {
            manage.readAll();
        }

        public void readCustomer()
        {
            manage.readCustomer(this);
        }

        public void removeCustomer() {
            manage.removeCustomer(this);
        }

        public void modifyCustomer() {
            manage.modifyCustomer(this);
        }

        public override bool Equals(object obj)
        {
            return obj is Customer customer &&
                   id == customer.id;
        }
    }
}
