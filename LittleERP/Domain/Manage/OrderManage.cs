using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleERP.Domain.Manage
{
    public class OrderManage
    {
        public List<Object> GenericList { get; set; }
        public DataTable torderproducts { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderManage"/> class.
        /// </summary>
        public OrderManage()
        {
            torderproducts = new DataTable();
            GenericList = new List<Object>();
        }

        /**
         * Lee todos los orders y los añade a una lista generica
         */

        /// <summary>
        /// Retrieves all the orders from the database
        /// </summary>
        public void readAllOrders()
        {
            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();
            //Para no traer tantos datos de una sola vez, se traerán solo los id de referencias de otras tablas y se leerán a parte con sus respectivos metodos
            data = search.getData("SELECT O.IDORDER, O.DATETIME,O.REFCUSTOMER,O.REFUSER,O.REFPAYMENTMETHOD,O.TOTAL,O.PREPAID,CONFIRMED,LABELED,SENT,INVOICED FROM ORDERS O, ORDERS_STATUS OS " +
                                    "WHERE O.IDORDER = OS.REFORDER AND " +
                                    "O.DELETED = 0 " +
                                    "ORDER BY O.DATETIME DESC", "Orders");

            DataTable table = data.Tables["Orders"];

            Order aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new Order();
                aux.id = Convert.ToInt64(row["IDORDER"]);
                aux.dateTime = Convert.ToDateTime(row["DATETIME"]);

                //Se lee un customer a partir de su id
                Customer customer = new Customer();
                customer.id = Convert.ToInt32(row["REFCUSTOMER"]);
                customer.readCustomer();
                aux.customer = (Customer)customer.manage.GenericList[0];

                //Se lee un usuario a partir de su id
                User user = new User();
                user.iduser = Convert.ToInt32(row["REFUSER"]);
                user.readUser();
                aux.user = (User)user.manage.list[0];

                //Se lee un paymentmethod a partir de su id
                PaymentMethod paymentMethod = new PaymentMethod();
                paymentMethod.id = Convert.ToInt32(row["REFPAYMENTMETHOD"]);
                paymentMethod.readPayment();
                aux.payment = (PaymentMethod)paymentMethod.manage.GenericList[0];


                aux.total = Convert.ToDouble(row["TOTAL"]);
                aux.prepaid = Convert.ToDouble(row["PREPAID"]);
                aux.confirmed = Convert.ToInt32(row["CONFIRMED"]);
                aux.labeled = Convert.ToInt32(row["LABELED"]);
                aux.sent = Convert.ToInt32(row["SENT"]);
                aux.invoiced = Convert.ToInt32(row["INVOICED"]);

                //Se lee un orderproduct a partir del id de un order
                OrderProducts orderProducts = new OrderProducts();
                orderProducts.readOrderProducts(aux);

                //Se recoge torderproducts leido
                aux.manage.torderproducts = orderProducts.manage.torderproducts;

                aux.orderProducts = orderProducts.manage.GenericList.Cast<OrderProducts>().ToList();

                GenericList.Add(aux);
            }

        }

        /**
         * Lee todos los order por una fecha especifica
         */
        /// <summary>
        /// Retrieves orders from the database given a date by parameter.
        /// </summary>
        /// <param name="order">The order.</param>
        public void readOrderByDate(Order order)
        {
            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();

            data = search.getData("SELECT O.IDORDER, O.DATETIME,O.REFCUSTOMER,O.REFPAYMENTMETHOD,O.TOTAL,O.PREPAID,CONFIRMED,LABELED,SENT,INVOICED FROM ORDERS O, ORDERS_STATUS OS " +
                                    "WHERE O.IDORDER = OS.REFORDER AND " +
                                    "O.DELETED = 0 AND TO_CHAR(O.DATETIME,'DD/MM/YYYY') = TO_DATE('"+order.dateTime.ToString("dd/MM/yyyy")+"','DD/MM/YYYY')" +
                                    " ORDER BY O.DATETIME DESC", "Orders");

            DataTable table = data.Tables["Orders"];

            Order aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new Order();
                aux.id = Convert.ToInt64(row["IDORDER"]);
                aux.dateTime = Convert.ToDateTime(row["DATETIME"]);

                Customer customer = new Customer();
                customer.id = Convert.ToInt32(row["REFCUSTOMER"]);
                customer.readCustomer();
                aux.customer = (Customer)customer.manage.GenericList[0];

                PaymentMethod paymentMethod = new PaymentMethod();
                paymentMethod.id = Convert.ToInt32(row["REFPAYMENTMETHOD"]);
                paymentMethod.readPayment();
                aux.payment = (PaymentMethod)paymentMethod.manage.GenericList[0];


                aux.total = Convert.ToDouble(row["TOTAL"]);
                aux.prepaid = Convert.ToDouble(row["PREPAID"]);
                aux.confirmed = Convert.ToInt32(row["CONFIRMED"]);
                aux.labeled = Convert.ToInt32(row["LABELED"]);
                aux.sent = Convert.ToInt32(row["SENT"]);
                aux.invoiced = Convert.ToInt32(row["INVOICED"]);

                OrderProducts orderProducts = new OrderProducts();
                orderProducts.readOrderProducts(aux);

                //Se recoge torderproducts leido
                aux.manage.torderproducts = orderProducts.manage.torderproducts;

                aux.orderProducts = orderProducts.manage.GenericList.Cast<OrderProducts>().ToList();

                GenericList.Add(aux);
            }
        }

        /// <summary>
        /// Retrieves orders from the database given an invoice by parameter
        /// </summary>
        /// <param name="i">The i.</param>
        public void readOrderByInvoice(Invoice i) {
            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();
            //Para no traer tantos datos de una sola vez, se traerán solo los id de referencias de otras tablas y se leerán a parte con sus respectivos metodos
            data = search.getData("SELECT O.IDORDER, O.DATETIME,O.REFCUSTOMER,O.REFUSER,O.REFPAYMENTMETHOD,O.TOTAL,O.PREPAID,CONFIRMED,LABELED,SENT,INVOICED FROM ORDERS O, ORDERS_STATUS OS, ORDER_INVOICE OI " +
                                    "WHERE O.IDORDER = OS.REFORDER AND OI.IDINVOICE ="+i.id +" AND "+
                                    " OI.IDORDER = O.IDORDER AND "+
                                    "O.DELETED = 0 ", "Orders");

            DataTable table = data.Tables["Orders"];

            Order aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new Order();
                aux.id = Convert.ToInt64(row["IDORDER"]);
                aux.dateTime = Convert.ToDateTime(row["DATETIME"]);

                //Se lee un customer a partir de su id
                Customer customer = new Customer();
                customer.id = Convert.ToInt32(row["REFCUSTOMER"]);
                customer.readCustomer();
                aux.customer = (Customer)customer.manage.GenericList[0];

                //Se lee un usuario a partir de su id
                User user = new User();
                user.iduser = Convert.ToInt32(row["REFUSER"]);
                user.readUser();
                aux.user = (User)user.manage.list[0];

                //Se lee un paymentmethod a partir de su id
                PaymentMethod paymentMethod = new PaymentMethod();
                paymentMethod.id = Convert.ToInt32(row["REFPAYMENTMETHOD"]);
                paymentMethod.readPayment();
                aux.payment = (PaymentMethod)paymentMethod.manage.GenericList[0];


                aux.total = Convert.ToDouble(row["TOTAL"]);
                aux.prepaid = Convert.ToDouble(row["PREPAID"]);
                aux.confirmed = Convert.ToInt32(row["CONFIRMED"]);
                aux.labeled = Convert.ToInt32(row["LABELED"]);
                aux.sent = Convert.ToInt32(row["SENT"]);
                aux.invoiced = Convert.ToInt32(row["INVOICED"]);

                //Se lee un orderproduct a partir del id de un order
                OrderProducts orderProducts = new OrderProducts();
                orderProducts.readOrderProducts(aux);

                //Se recoge torderproducts leido
                aux.manage.torderproducts = orderProducts.manage.torderproducts;

                aux.orderProducts = orderProducts.manage.GenericList.Cast<OrderProducts>().ToList();

                GenericList.Add(aux);
            }
        }

        /**
         * Genera un orderID nuevo ya que contiene la fecha y está numerado (YYYYMMDD0000)
         * Consulta la fecha a la base de datos
         */

        /// <summary>
        /// Generates the order identifier.
        /// </summary>
        /// <param name="order">The order.</param>
        public void generateOrderID(Order order)
        {
            ConnectOracle search = new ConnectOracle();
            //Se saca la fecha con el formato adecuado de la base de datos
            long fecha = Convert.ToInt64(search.DLookUp("to_char(sysdate,'yyyymmdd')", "DUAL",""));
            //Comprobamos si ya hay un id que empiece por esa fecha, si devuelve 0 quiere decir que es el primer order que hay que insertar
            long id = Convert.ToInt64("0"+search.DLookUp("max(IDORDER)", "ORDERS", "IDORDER LIKE ('" + fecha.ToString() + "%')"));

            if (id == 0) {
                id = (fecha * 10000) + 1;
            } else
            {
                id = id + 1;
            }
            order.id = id;
        }

        /**
         * Inserta un order dado por parametro a la base de datos, se le da la fecha actual de cuando se ha insertado en la base de datos
         */
        /// <summary>
        /// Inserts the order in the database.
        /// </summary>
        /// <param name="order">The order.</param>
        public void insertOrder(Order order)
        {
            ConnectOracle search = new ConnectOracle();

            order.dateTime = Convert.ToDateTime(search.DLookUp("SYSDATE", "DUAL", ""));

            search.setData("INSERT INTO ORDERS(IDORDER,REFCUSTOMER,REFUSER,DATETIME,REFPAYMENTMETHOD,TOTAL,PREPAID,DELETED) VALUES(" + order.id + "," + order.customer.id + ","+ order.user.iduser + ",SYSDATE" + "," + order.payment.id + "," + order.total + "," + order.prepaid + ",0" + ")");

            search.setData("INSERT INTO ORDERS_STATUS(REFORDER,CONFIRMED,LABELED,SENT,INVOICED) VALUES(" + order.id + "," + order.confirmed + "," + order.labeled + "," + order.sent + "," + order.invoiced + ")");
            foreach(OrderProducts op in order.orderProducts)
            {
                //Si el id de producto es -1 quiere decir que no tiene un producto asignado y por lo tanto es un producto generico
                if (op.product.id != -1)
                {
                    //Se inserta en OrdersProducts
                    int maximum = Convert.ToInt32("0" + search.DLookUp("max(IDORDERPRODUCT)", "ORDERSPRODUCTS", "")) + 1;
                    search.setData("INSERT INTO ORDERSPRODUCTS(IDORDERPRODUCT,REFORDER,REFPRODUCT,AMOUNT,PRICESALE) VALUES(" + maximum + "," + order.id + "," + op.product.id + "," + op.AmountofSale + "," + op.priceofSale + ")");

                    //Actualiza el stock del producto en base a la cantidad del pedido
                    op.product.stock = op.product.stock - op.AmountofSale;
                    op.product.modifyProduct();
                } else
                {
                    //Se inserta en GenericProducts
                    int maximum = Convert.ToInt32("0" + search.DLookUp("max(IDORDERGENERIC)", "GENERICPRODUCTS", "")) + 1;
                    search.setData("INSERT INTO GENERICPRODUCTS(IDORDERGENERIC,REFORDER,AMOUNT,PRICESALE,DESCRIPTION) VALUES(" + maximum + "," + order.id + "," + op.AmountofSale + "," + op.priceofSale + ",'" + op.description + "')");
                }
               
            }

        }

        /**
         * Elimina un order dado por parametro
         */
        /// <summary>
        /// Deletes the order from the database.
        /// </summary>
        /// <param name="order">The order.</param>
        public void deleteOrder(Order order) {
            ConnectOracle search = new ConnectOracle();

            search.setData("UPDATE ORDERS SET DELETED=-1 WHERE IDORDER=" + order.id);
        }


        /**
         * Lee todos los metodos de pago y los inserta en una lista generica
         */
        /// <summary>
        /// Retrieves all the payment methods from the database.
        /// </summary>
        public void readAllPayments()
        {
            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();

            data = search.getData("SELECT IDPAYMENTMETHOD,PAYMENTMETHOD FROM PAYMENTMETHODS WHERE DELETED=0", "PaymentMethods");

            DataTable table = data.Tables["PaymentMethods"];

            PaymentMethod aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new PaymentMethod();
                aux.name = Convert.ToString(row["PAYMENTMETHOD"]);
                aux.id = Convert.ToInt32(row["IDPAYMENTMETHOD"]);
                GenericList.Add(aux);
            }

        }

        /**
         * Lee un metodo de pago dado por parametro
         */

        /// <summary>
        /// Retrieves a payment method from the database given an id by parameter.
        /// </summary>
        /// <param name="payment">The payment.</param>
        public void readPayment(PaymentMethod payment)
        {
            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();

            data = search.getData("SELECT IDPAYMENTMETHOD,PAYMENTMETHOD FROM PAYMENTMETHODS WHERE IDPAYMENTMETHOD = " + payment.id + " AND DELETED=0", "PaymentMethods");

            DataTable table = data.Tables["PaymentMethods"];

            PaymentMethod aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new PaymentMethod();
                aux.name = Convert.ToString(row["PAYMENTMETHOD"]);
                aux.id = Convert.ToInt32(row["IDPAYMENTMETHOD"]);
                GenericList.Add(aux);
            }
        }

        /**
         * Lee los orderproducts de un order especifico, hay que hacer union con la tabla genericproducts por si hay un producto generico
         */

        /// <summary>
        /// Retrieves all order products from the database given an order id by parameter.
        /// </summary>
        /// <param name="order">The order.</param>
        public void readOrderProducts(Order order)
        {
            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();

            data = search.getData("SELECT REFPRODUCT, NAME DESCRIPTION, AMOUNT, PRICESALE FROM ORDERSPRODUCTS OP, PRODUCTS P " +
                                    "WHERE REFORDER = " + order.id + " AND OP.REFPRODUCT = P.IDPRODUCT "+
                                                " UNION " +
                                    "SELECT - 1, DESCRIPTION, AMOUNT, PRICESALE FROM GENERICPRODUCTS " +
                                     "WHERE REFORDER = " + order.id, "OrderProducts");

            DataTable table = data.Tables["OrderProducts"];

            //Se añaden las columnas a torderproducts
            torderproducts.Columns.Add("DESCRIPTION", Type.GetType("System.String"));
            torderproducts.Columns.Add("AMOUNTOFSALE", Type.GetType("System.String"));
            torderproducts.Columns.Add("PRICEOFSALE", Type.GetType("System.String"));

            OrderProducts aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new OrderProducts();
                //Si la fila da -1 quiere decir que es un producto generico
                if (Convert.ToInt32(row["REFPRODUCT"]) != -1)
                {
                    Product product = new Product();
                    product.id = Convert.ToInt32(row["REFPRODUCT"]);
                    product.readProduct();
                    aux.product = (Product)product.manage.GenericList[0];
                }
                else
                {
                    aux.product = new Product(); //Se genera un nuevo producto el cual tiene de id -1 por defecto
                    aux.description = Convert.ToString(row["DESCRIPTION"]);
                }

                aux.AmountofSale = Convert.ToInt32(row["AMOUNT"]);
                aux.priceofSale = Convert.ToDouble(row["PRICESALE"]);

                //Se añaden las filas con sus respectivos datos
                
                torderproducts.Rows.Add(new object[] { row["DESCRIPTION"], row["AMOUNT"], row["PRICESALE"] });


                GenericList.Add(aux);
            }
        }

        /**
         * Actualiza el estado de un order dado por parametro
         */

        /// <summary>
        /// Updates the order status.
        /// </summary>
        /// <param name="order">The order.</param>
        public void updateOrderStatus(Order order)
        {
            ConnectOracle search = new ConnectOracle();
            search.setData("UPDATE ORDERS_STATUS SET CONFIRMED="+order.confirmed+",LABELED="+order.labeled+",SENT="+order.sent+",INVOICED="+order.invoiced+" WHERE REFORDER="+order.id);
        }
    }
}
