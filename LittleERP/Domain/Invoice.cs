using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleERP.Domain
{
    public class Invoice
    {
        public int id { get; set; }
        public DateTime dateTime { get; set; }
        public int accounted { get; set; }

        public Manage.InvoiceManage manage { get; set; }

        public Invoice() {
            manage = new Manage.InvoiceManage();
        }


        public void readAll() {
            manage.readAll();
        }

        public void readInvoiceByOrder(Order o) {
            manage.readInvoiceByOrder(o);
        }

        public void readInvoiceByDate()
        {
            manage.readInvoiceByDate(this);
        }

        public void generateInvoiceID()
        {
            manage.generateInvoiceID(this);
        }

        public void generateInvoiceInfo() {
            manage.generateInvoiceInfo(this);
        }

        public void insertInvoice(Order o) {
            manage.insertInvoice(this,o);
        }

        public void updateAccounted() {
            manage.updateAccounted(this);
        }

        public void deleteInvoice()
        {
            manage.deleteInvoice(this);
        }


        public override bool Equals(object obj)
        {
            return obj is Invoice invoice &&
                   id == invoice.id;
        }

    }
}
