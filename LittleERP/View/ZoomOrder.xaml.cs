using LittleERP.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LittleERP.View
{
    /// <summary>
    /// Lógica de interacción para ZoomOrder.xaml
    /// </summary>
    public partial class ZoomOrder : Window
    {
        private ControlPanel controlPanel;
        public Customer customer { get; set; }
        public Product product { get; set; }

        public Order order { get; set; }

        public Invoice invoice { get; set; }

        private Boolean withInvoice;

        private readonly Regex regex = new Regex("[^0-9]+");

        /**
         * Constructor para crear un nuevo order, se da por parametro el ControlPanel
         */
        public ZoomOrder(ControlPanel originalcontrolPanel, Boolean withInvoice)
        {
            InitializeComponent();

            controlPanel = originalcontrolPanel;
            order = new Order();
            customer = new Customer();
            product = new Product();
            this.withInvoice = withInvoice;

            //Se cargan los los metodos de pago
            PaymentMethod paymentMethod = new PaymentMethod();
            paymentMethod.readAllPayments();
            cboPaymentM.ItemsSource = null;
            cboPaymentM.ItemsSource = paymentMethod.manage.GenericList;

            //Se genera un orderID
            order.generateOrderID();
            
            //Si se va a hacer con un invoice se genera el invoiceID
            if (withInvoice)
            {
                this.Title = "ZoomInvoice";
                invoice = new Invoice();
                invoice.generateInvoiceID();
                txtIdOrder.Text = invoice.id.ToString();
            }
            else {
                txtIdOrder.Text = order.id.ToString();
            }
            txtIdOrder.IsEnabled = false;

            txtProductNameOrder.IsEnabled = false;
        }

        /**
         * Constructor para ver la información de un order dado por parametro
         * no se permite la modificación por lo que se desactivan todos los campos
         * Se da por parametro el order a ver y el ControlPanel
         */
        public ZoomOrder(Order orderV, ControlPanel originalcontrolPanel)
        {
            InitializeComponent();

            controlPanel = originalcontrolPanel;
            order = orderV;
            customer = orderV.customer;
            this.withInvoice = false;

            dgvOrderProducts.ItemsSource = null;
            dgvOrderProducts.ItemsSource = orderV.orderProducts;

            PaymentMethod paymentMethod = new PaymentMethod();
            paymentMethod.readAllPayments();
            cboPaymentM.ItemsSource = null;
            cboPaymentM.ItemsSource = paymentMethod.manage.GenericList;
            int payIndex = paymentMethod.manage.GenericList.IndexOf(orderV.payment);

            cboPaymentM.SelectedIndex = payIndex;
            txtClient.Text = customer.name.ToString();
            txtIdOrder.Text = orderV.id.ToString();
            txtPrepaid.Text = orderV.prepaid.ToString();


            txtIdOrder.IsEnabled = false;
            txtClient.IsEnabled = false;
            txtProductNameOrder.IsEnabled = false;
            txtProductPriceOrder.IsEnabled = false;
            txtProductQuantityOrder.IsEnabled = false;
            txtPrepaid.IsEnabled = false;
            cboPaymentM.IsEnabled = false;
            imgChooseClient.IsEnabled = false;
            imgChooseProduct.IsEnabled = false;
            btnSaveOrderProduct.IsEnabled = false;
            btnSaveOrder.IsEnabled = false;
            btnAddOrderProduct.IsEnabled = false;
            btnDelOrderProduct.IsEnabled = false;
            btnOthOrderProduct.IsEnabled = false;
            btnUpQuantity.IsEnabled = false;
            btnDownQuantity.IsEnabled = false;
            dgvOrderProducts.IsEnabled = false;
            

        }

        public ZoomOrder(Order orderV, Invoice invoiceV, ControlPanel originalcontrolPanel)
        {
            InitializeComponent();

            controlPanel = originalcontrolPanel;
            order = orderV;
            invoice = invoiceV;
            customer = orderV.customer;
            this.withInvoice = true;

            dgvOrderProducts.ItemsSource = null;
            dgvOrderProducts.ItemsSource = orderV.orderProducts;

            PaymentMethod paymentMethod = new PaymentMethod();
            paymentMethod.readAllPayments();
            cboPaymentM.ItemsSource = null;
            cboPaymentM.ItemsSource = paymentMethod.manage.GenericList;
            int payIndex = paymentMethod.manage.GenericList.IndexOf(orderV.payment);

            cboPaymentM.SelectedIndex = payIndex;
            txtClient.Text = customer.name.ToString();
            txtIdOrder.Text = invoice.id.ToString();
            txtPrepaid.Text = orderV.prepaid.ToString();


            txtIdOrder.IsEnabled = false;
            txtClient.IsEnabled = false;
            txtProductNameOrder.IsEnabled = false;
            txtProductPriceOrder.IsEnabled = false;
            txtProductQuantityOrder.IsEnabled = false;
            txtPrepaid.IsEnabled = false;
            cboPaymentM.IsEnabled = false;
            imgChooseClient.IsEnabled = false;
            imgChooseProduct.IsEnabled = false;
            btnSaveOrderProduct.IsEnabled = false;
            btnSaveOrder.IsEnabled = false;
            btnAddOrderProduct.IsEnabled = false;
            btnDelOrderProduct.IsEnabled = false;
            btnOthOrderProduct.IsEnabled = false;
            btnUpQuantity.IsEnabled = false;
            btnDownQuantity.IsEnabled = false;
            dgvOrderProducts.IsEnabled = false;

            this.Title = "ZoomInvoice";

        }

        /**
         * Evento de click para elegir un customer, deja el ControlPanel solo con la parte de customers y le da esta ventana como atributo (Ya que funciona como un objeto)
         */
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            controlPanel.tbiProducts.Visibility = Visibility.Collapsed;
            controlPanel.tbiOrders.Visibility = Visibility.Collapsed;
            controlPanel.tbiSuppliers.Visibility = Visibility.Collapsed;
            controlPanel.tbHome.Visibility = Visibility.Collapsed;
            controlPanel.tbUsers.Visibility = Visibility.Collapsed;
            controlPanel.tbProfile.Visibility = Visibility.Collapsed;
            controlPanel.tbiCustomers.Visibility = Visibility.Visible;

            controlPanel.tbiCustomers.IsSelected = true;

            controlPanel.Hide();
            controlPanel.Show();

            controlPanel.zoomOrder = this;
        }

        /**
         * Evento de click para elegir un producto, deja el ControlPanel solo con la parte de products y le da esta ventana como atributo (Ya que funciona como objeto)
         */
        private void imgChooseProduct_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            controlPanel.tbiCustomers.Visibility = Visibility.Collapsed;
            controlPanel.tbiOrders.Visibility = Visibility.Collapsed;
            controlPanel.tbiSuppliers.Visibility = Visibility.Collapsed;
            controlPanel.tbHome.Visibility = Visibility.Collapsed;
            controlPanel.tbUsers.Visibility = Visibility.Collapsed;
            controlPanel.tbProfile.Visibility = Visibility.Collapsed;
            controlPanel.tbiProducts.Visibility = Visibility.Visible;

            controlPanel.tbiProducts.IsSelected = true;

            controlPanel.Hide();
            controlPanel.Show();

            controlPanel.zoomOrder = this;
        }

        /**
         * Evento al cerrar esta ventana, deja el ControlPanel visible y le quita de atributo esta ventana
         */
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            controlPanel.tbiProducts.Visibility = Visibility.Visible;
            controlPanel.tbiCustomers.Visibility = Visibility.Visible;
            controlPanel.tbiOrders.Visibility = Visibility.Visible;
            controlPanel.tbiSuppliers.Visibility = Visibility.Visible;
            controlPanel.tbHome.Visibility = Visibility.Visible;
            controlPanel.tbUsers.Visibility = Visibility.Visible;
            controlPanel.tbProfile.Visibility = Visibility.Visible;

            if (withInvoice)
            {
                controlPanel.tbiInvoices.IsSelected = true;
            } else
            {
                controlPanel.tbiOrders.IsSelected = true;
            }

            controlPanel.zoomOrder = null;
            controlPanel.Show();
        }

        /**
         * Evento de boton para guardar un OrderProduct, si es un GenericProduct se deducirá por su descripción
         */
        private void btnSaveOrderProduct_Click(object sender, RoutedEventArgs e)
        {
            OrderProducts orderProduct = new OrderProducts();
            //Comprueba si los campos estan rellenados
            if (txtProductNameOrder.Text.Length > 0 && txtProductPriceOrder.Text.Length > 0 && txtProductQuantityOrder.Text.Length > 0)
            {
                orderProduct.product = product;
                //Si el campo del nombre del producto está activado quiere decir que tiene que coger "descripcion"
                if (txtProductNameOrder.IsEnabled)
                {
                    orderProduct.description = txtProductNameOrder.Text;
                } else
                {
                    orderProduct.description = null;
                }
                orderProduct.AmountofSale = Convert.ToInt32(txtProductQuantityOrder.Text);
                orderProduct.priceofSale = Convert.ToDouble(txtProductPriceOrder.Text);

                //Si ya existia el producto en los orderProducts de order, que lo machaque
                int index = order.orderProducts.IndexOf(orderProduct);
                if (index != -1)
                {
                    order.orderProducts.RemoveAt(index);
                    order.orderProducts.Add(orderProduct);
                }
                else
                {
                    order.orderProducts.Add(orderProduct);
                }

                dgvOrderProducts.ItemsSource = null;
                dgvOrderProducts.ItemsSource = order.orderProducts;
            }
            
          
        }

        private void btnUpQuantity_Click(object sender, RoutedEventArgs e)
        {
            int number = 1;
            if (txtProductQuantityOrder.Text.Length > 0)
            {
                number = Convert.ToInt32(txtProductQuantityOrder.Text) + 1;
                txtProductQuantityOrder.Text = number.ToString();
            }
            else {
                txtProductQuantityOrder.Text = number.ToString();
            }
        }

        private void btnDownQuantity_Click(object sender, RoutedEventArgs e)
        {
            int number = Convert.ToInt32(txtProductQuantityOrder.Text);
            if (number > 1)
            {
                number = number - 1;
                txtProductQuantityOrder.Text = number.ToString();
            }
        }
        /**
         * Evento de boton para añadir un nuevo producto al order, limpia los campos y habilita lo necesario para insertar un producto
         */
        private void btnAddOrderProduct_Click(object sender, RoutedEventArgs e)
        {
            product = new Product();
            txtProductNameOrder.Text = "";
            txtProductNameOrder.IsEnabled = false;
            txtProductNameOrder.Height = 25;
            imgChooseProduct.Visibility = Visibility.Visible;
            imgChooseProduct.IsEnabled = true;
            lblProduct.Content = "Product:";
            txtProductPriceOrder.Text = "";
            txtProductQuantityOrder.Text = "1";

        }

        /**
         * Evento de boton de borrado de producto al order, borra en caso de que haya un producto seleccionado del datagrid
         * borra independientemente de si es generic o no
         */
        private void btnDelOrderProduct_Click(object sender, RoutedEventArgs e)
        {
            if (dgvOrderProducts.SelectedIndex != -1)
            {
                order.orderProducts.Remove((OrderProducts)dgvOrderProducts.SelectedItem);
                dgvOrderProducts.ItemsSource = null;
                dgvOrderProducts.ItemsSource = order.orderProducts;
            }
        }

        /**
         * Evento de boton para guardar el order entero una vez se ha decidido todo y lo inserta en la tabla
         */
        private void btnSaveOrder_Click(object sender, RoutedEventArgs e)
        {
            //Comprueba que hay elementos en la lista de productos(Order products),
            // un cliente y un metodo de pago
            //Tambien se comprueba que haya una cantidad en prepaid

            if(order.orderProducts.Count > 0 && customer.id != -1 && cboPaymentM.SelectedIndex != -1 && txtPrepaid.Text.Length > 0)
            {
                order.payment = (PaymentMethod)cboPaymentM.SelectedItem;
                order.customer = customer;
                order.user = controlPanel.u;

                double total = 0;
                double prepaid = 0;
                foreach (OrderProducts op in order.orderProducts)
                {
                    total = total + (op.priceofSale * op.AmountofSale);
                }
                
                prepaid = Convert.ToDouble(txtPrepaid.Text);
                if (prepaid < total)
                {
                    order.total = total - prepaid;
                    order.prepaid = prepaid;

                    //Cuando hay un invoice se deja en confirmado todos los estados y se inserta en la base de datos un invoice
                    if (withInvoice)
                    {
                        order.confirmed = -1;
                        order.labeled = -1;
                        order.sent = -1;
                        order.invoiced = -1;

                        invoice.accounted = 0;
                        order.insertOrder();
                        invoice.insertInvoice(order);

                        //Se actualiza el datagrid de invoices
                        List<Invoice> invoices = controlPanel.dgvInvoices.ItemsSource.Cast<Invoice>().ToList();
                        invoices.Insert(0,invoice); 
                        controlPanel.dgvInvoices.ItemsSource = null;
                        controlPanel.dgvInvoices.ItemsSource = invoices;
                    }
                    else {
                        //Si es cash on delivery, se confirma automaticamente
                        if (order.payment.id == 2)
                        {
                            order.confirmed = -1;
                        }
                        else
                        {
                            order.confirmed = 0;
                        }
                        order.labeled = 0;
                        order.sent = 0;
                        order.invoiced = 0;

                        order.insertOrder();
                    }

                    //Actualiza el stock de los productos en el datagrid (En el proceso de insertOrder ya se han actualizado)
                    List<Product> products = controlPanel.dgvProducts.ItemsSource.Cast<Product>().ToList();
                    foreach(OrderProducts op in order.orderProducts)
                    {
                        if (op.product.id != -1) {
                            products[products.IndexOf(op.product)] = op.product;
                        }

                    }
                    controlPanel.dgvProducts.ItemsSource = null;
                    controlPanel.dgvProducts.ItemsSource = products;

                    //Actualiza el datagrid de orders añadiendo el order creado
                    List<Order> orders = controlPanel.dgvOrders.ItemsSource.Cast<Order>().ToList();
                    //Se inserta en la lista como el primero ya que tienen un orden de fecha descente
                    orders.Insert(0,order);
                    controlPanel.dgvOrders.ItemsSource = null;
                    controlPanel.dgvOrders.ItemsSource = orders;

                    MessageBox.Show("The order has been inserted");
                    this.Close();
                }
                else {
                    MessageBox.Show("The prepaid is bigger than the total");
                }

                
            }

        }

        private void btnCancelOrder_Click(object sender, RoutedEventArgs e)
        {
            //Activa tambien el evento de window closing
            this.Close();
        }

        /**
         * Evento de boton que habilita los campos necesarios para insertar un producto generico (Con ID -1 y una descripcion) a Orderproducts
         */
        private void btnOthOrderProduct_Click(object sender, RoutedEventArgs e)
        {
            product = new Product();
            txtProductNameOrder.Text = "";
            txtProductNameOrder.IsEnabled = true;
            txtProductNameOrder.Height = 50;
            lblProduct.Content = "Description:";
            imgChooseProduct.Visibility = Visibility.Hidden;
            imgChooseProduct.IsEnabled = false;
            txtProductPriceOrder.Text = "";
            txtProductQuantityOrder.Text = "1";
        }

        /**
         * Evento que antes de que se introduzca texto en el textbox compruebe que el input es un numero mediante un regex
         */
        private void txtProductQuantityOrder_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txtPrepaid_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txtProductPriceOrder_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = regex.IsMatch(e.Text);
        }

        /**
         * Al seleccionar un OrderProduct del datagrid se carga a la derecha con sus datos por si se quiere ver en detalle o machacarlo con otros detalles
         */
        private void dgvOrderProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgvOrderProducts.SelectedIndex != -1) {
                OrderProducts orderProduct = (OrderProducts)dgvOrderProducts.SelectedItem;
                product = orderProduct.product;
                if (orderProduct.product.id != -1) {
                    txtProductNameOrder.Text = orderProduct.product.name;
                    txtProductNameOrder.IsEnabled = false;
                    txtProductNameOrder.Height = 25;
                    imgChooseProduct.Visibility = Visibility.Visible;
                    imgChooseProduct.IsEnabled = true;
                } else
                {
                    txtProductNameOrder.Text = orderProduct.description;
                    txtProductNameOrder.IsEnabled = true;
                    txtProductNameOrder.Height = 50;
                    lblProduct.Content = "Description:";
                    imgChooseProduct.Visibility = Visibility.Hidden;
                    imgChooseProduct.IsEnabled = false;
                }

                txtProductPriceOrder.Text = orderProduct.priceofSale.ToString();
                txtProductQuantityOrder.Text = orderProduct.AmountofSale.ToString();

            }
        }
    }
}
