using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleERP.Domain
{
    public class PaymentMethod
    {
        public int id { get; set; }
        public String name { get; set; }
        public Manage.OrderManage manage { get; set; }
        public PaymentMethod() {
            manage = new Manage.OrderManage();
        }

        public void readAllPayments()
        {
            manage.readAllPayments();
        }

        public void readPayment()
        {
            manage.readPayment(this);
        }

        public override bool Equals(object obj)
        {
            return obj is PaymentMethod method &&
                   id == method.id;
        }
    }
}
