using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleERP.Domain
{
    public class Order
    {
        public long id { get; set; }

        public Customer customer { get; set; }

        public User user { get; set; }

        public DateTime dateTime { get; set; }

        public PaymentMethod payment { get; set; }
        public double total { get; set; }
        public double prepaid { get; set; }

        public int confirmed { get; set; }
        public int labeled { get; set; }
        public int sent { get; set; }
        public int invoiced { get; set; }

        public List<OrderProducts> orderProducts {get;set;}

        public Manage.OrderManage manage { get; set; }

        public Order() {
            manage = new Manage.OrderManage();
            orderProducts = new List<OrderProducts>();
        }

        public void readAllOrders()
        {
            manage.readAllOrders();
        }

        public void readOrderByDate()
        {
            manage.readOrderByDate(this);
        }

        public void readOrderByInvoice(Invoice i) {
            manage.readOrderByInvoice(i);
        }

        public void generateOrderID()
        {
            manage.generateOrderID(this);
        }
        public void updateOrderStatus()
        {
            manage.updateOrderStatus(this);
        }

        public void insertOrder()
        {
            manage.insertOrder(this);
        }

        public void deleteOrder()
        {
            manage.deleteOrder(this);
        }

        public override bool Equals(object obj)
        {
            return obj is Order order &&
                   id == order.id;
        }
    }
}
