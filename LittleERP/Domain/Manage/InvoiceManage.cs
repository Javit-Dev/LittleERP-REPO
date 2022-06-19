using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleERP.Domain.Manage
{
    public class InvoiceManage
    {
        public List<Invoice> list { get; set; }

        public DataTable tinfinvoices { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="InvoiceManage"/> class.
        /// </summary>
        public InvoiceManage()
        {
            list = new List<Invoice>();
            tinfinvoices = new DataTable();
        }

        /// <summary>
        /// Retrieves all the invoices from the database
        /// </summary>
        public void readAll() {
            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();

            data = search.getData("SELECT IDINVOICE, DATETIME, ACCOUNTED FROM INVOICES WHERE DELETED=0 " +
                                    "ORDER BY DATETIME DESC ", "invoices");

            DataTable table = data.Tables["invoices"];

            Invoice aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new Invoice();
                aux.id = Convert.ToInt32(row["IDINVOICE"]);
                aux.dateTime = Convert.ToDateTime(row["DATETIME"]);
                aux.accounted = Convert.ToInt32(row["ACCOUNTED"]);
                aux.generateInvoiceInfo();
                list.Add(aux);
            }
        }

        /// <summary>
        /// Retrieves a invoice from the database given an order by parameter
        /// </summary>
        /// <param name="o">The o.</param>
        public void readInvoiceByOrder(Order o) {
            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();

            data = search.getData("SELECT I.IDINVOICE,DATETIME,ACCOUNTED FROM INVOICES I, ORDER_INVOICE OI "+
                                    "WHERE I.IDINVOICE = OI.IDINVOICE "+
                                    "AND OI.IDORDER ="+o.id + " AND DELETED=0" , "invoices");

            DataTable table = data.Tables["invoices"];

            Invoice aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new Invoice();
                aux.id = Convert.ToInt32(row["IDINVOICE"]);
                aux.dateTime = Convert.ToDateTime(row["DATETIME"]);
                aux.accounted = Convert.ToInt32(row["ACCOUNTED"]);
                aux.generateInvoiceInfo();
                list.Add(aux);
            }

        }

        /// <summary>
        /// Retrieves invoices from the database given a date by parameter.
        /// </summary>
        /// <param name="i">The i.</param>
        public void readInvoiceByDate(Invoice i) {
            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();

            data = search.getData("SELECT IDINVOICE, DATETIME, ACCOUNTED FROM INVOICES WHERE DELETED=0 AND TO_CHAR(DATETIME,'DD/MM/YYYY') = TO_DATE('"+i.dateTime.ToString("dd/MM/yyyy")+"') "+
                                    "ORDER BY DATETIME DESC ", "invoices");

            DataTable table = data.Tables["invoices"];

            Invoice aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new Invoice();
                aux.id = Convert.ToInt32(row["IDINVOICE"]);
                aux.dateTime = Convert.ToDateTime(row["DATETIME"]);
                aux.accounted = Convert.ToInt32(row["ACCOUNTED"]);
                aux.generateInvoiceInfo();
                list.Add(aux);
            }
        }

        /// <summary>
        /// Generates the invoice information.
        /// </summary>
        /// <param name="i">The i.</param>
        public void generateInvoiceInfo(Invoice i)
        {
            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();
            

            data = search.getData("SELECT I.IDINVOICE ID, I.DATETIME, C.NAME || ' ' || C.SURNAME WNAME, C.PHONE, C.NIF, O.TOTAL, O.PREPAID, PA.PAYMENTMETHOD,  C.ADDRESS || ', ' || CI.CITY || ', '|| S.STATE || ', ' || Z.ZIPCODE WADDRESS "+
                                "FROM CUSTOMERS C, ORDERS O, ZIPCODESCITIES ZC, CITIES CI, ZIPCODES Z, STATES S, PAYMENTMETHODS PA, INVOICES I, ORDER_INVOICE OI "+
                                "WHERE C.IDCUSTOMER = O.REFCUSTOMER "+
                                "AND ZC.IDZIPCODESCITIES = C.REFZIPCODESCITIES "+
                                "AND ZC.REFCITY = CI.IDCITY "+
                                "AND ZC.REFZIPCODE = Z.IDZIPCODE "+
                                "AND ZC.REFSTATE = S.IDSTATE "+
                                "AND O.REFPAYMENTMETHOD = PA.IDPAYMENTMETHOD "+
                                "AND OI.IDORDER = O.IDORDER "+
                                "AND OI.IDINVOICE = I.IDINVOICE "+
                                "AND I.IDINVOICE =  "+i.id, "infinvoices");

            DataTable table = data.Tables["infinvoices"];
            
            tinfinvoices.Columns.Add("ID", Type.GetType("System.String"));
            tinfinvoices.Columns.Add("DATETIME", Type.GetType("System.String"));
            tinfinvoices.Columns.Add("TOTAL", Type.GetType("System.String"));
            tinfinvoices.Columns.Add("PREPAID", Type.GetType("System.String"));
            tinfinvoices.Columns.Add("PAYMENTMETHOD", Type.GetType("System.String"));
            tinfinvoices.Columns.Add("WNAME", Type.GetType("System.String"));
            tinfinvoices.Columns.Add("WADDRESS", Type.GetType("System.String"));
            tinfinvoices.Columns.Add("PHONE", Type.GetType("System.String"));
            tinfinvoices.Columns.Add("NIF", Type.GetType("System.String"));

            DataRow row = table.Rows[0];
            tinfinvoices.Rows.Add(new object[] { row["ID"], row["DATETIME"], row["TOTAL"],row["PREPAID"], row["PAYMENTMETHOD"],row["WNAME"],row["WADDRESS"], row["PHONE"],row["NIF"] });
            



        }

        /// <summary>
        /// Inserts the invoice in the database
        /// </summary>
        /// <param name="i">The i.</param>
        /// <param name="o">The o.</param>
        public void insertInvoice(Invoice i, Order o) {
            ConnectOracle search = new ConnectOracle();

            i.dateTime = Convert.ToDateTime(search.DLookUp("SYSDATE", "DUAL", ""));
            //Inserta el invoice
            search.setData("INSERT INTO INVOICES(IDINVOICE,DATETIME,ACCOUNTED, DELETED) VALUES(" + i.id + ",SYSDATE,"+i.accounted + ",0"+ ")");

            //Inserta en order_invoice el id del order y el id del invoice
            int maximum = Convert.ToInt32("0" + search.DLookUp("max(ID_ORDER_INVOICE)", "ORDER_INVOICE", "")) + 1;
            search.setData("INSERT INTO ORDER_INVOICE(ID_ORDER_INVOICE,IDINVOICE,IDORDER) VALUES("+maximum+","+i.id+","+o.id+")");

            //Genera la facturacion
            i.generateInvoiceInfo();

            //Se actualiza el id
            int newcount = Convert.ToInt32(search.DLookUp("NUM", "COUNTINVOICE", "")) +1;
            search.setData("UPDATE COUNTINVOICE SET NUM=" + newcount);

        }

        /// <summary>
        /// Generates the invoice identifier.
        /// </summary>
        /// <param name="i">The i.</param>
        public void generateInvoiceID(Invoice i) {
            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();
            int id = 0;

            data = search.getData("SELECT AGE, NUM FROM COUNTINVOICE", "Countinvoice");

            DataTable table = data.Tables["Countinvoice"];

            DataRow row = table.Rows[0];
            id = Convert.ToInt32(row["AGE"])*10000;
            id = id+Convert.ToInt32(row["NUM"]);
            i.id = id;


        }

        /// <summary>
        /// Updates the state accounted.
        /// </summary>
        /// <param name="i">The i.</param>
        public void updateAccounted(Invoice i) {
            ConnectOracle search = new ConnectOracle();
            search.setData("UPDATE INVOICES SET ACCOUNTED="+i.accounted+" WHERE IDINVOICE="+i.id);
        }

        /// <summary>
        /// Deletes the invoice from the database.
        /// </summary>
        /// <param name="i">The i.</param>
        public void deleteInvoice(Invoice i) {
            ConnectOracle search = new ConnectOracle();
            search.setData("UPDATE INVOICES SET DELETED= -1" + " WHERE IDINVOICE=" + i.id);
        }

    }
}
