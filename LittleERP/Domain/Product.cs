using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleERP.Domain
{
    public class Product
    {
        public int id { get; set; }

        public String name { get; set; }

        public int stock { get; set; }
        public double price { get; set; }

        public Brand brand { get; set; }
        public SizeDrive size { get; set; }

        public Manage.ProductManage manage { get; set; }

        public Product() {
            manage = new Manage.ProductManage();
            id = -1;
        }

        public void readAllProducts() {
            manage.readAllProducts();
        }

        public void readProductName()
        {
            manage.readProductName(this);
        }

        public void readProduct()
        {
            manage.readProduct(this);
        }

        public void insertProduct()
        {
            manage.insertProduct(this);
        }

        public void modifyProduct() {
            manage.modifyProduct(this);
        }

        public void removeProduct()
        {
            manage.removeProduct(this);
        }

        public override bool Equals(object obj)
        {
            return obj is Product product &&
                   id == product.id;
        }
    }
}
