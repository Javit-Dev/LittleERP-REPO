using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleERP.Domain
{
    public class OrderProducts
    {
        public Product product { get; set; }
        public String description { get; set; }
        public int AmountofSale { get; set; }
        public double priceofSale { get; set; }

        public Manage.OrderManage manage { get; set; }
        

        public OrderProducts()
        {
            manage = new Manage.OrderManage();
        }

        public void readOrderProducts(Order order)
        {
            manage.readOrderProducts(order);
        }

        public override bool Equals(object obj)
        {
            return obj is OrderProducts products &&
                   EqualityComparer<Product>.Default.Equals(product, products.product) &&
                   description == products.description;
        }
    }
}
