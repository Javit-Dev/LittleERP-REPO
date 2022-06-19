using LittleERP.Domain;
using LittleERP.Domain.Manage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using static LittleERP.resources.useful;

namespace LittleERP.View
{
    /// <summary>
    /// Lógica de interacción para ControlPanel.xaml
    /// </summary>
    public partial class ControlPanel : Window
    {
        public User u { get; set; }
        private Customer customer;
        private Product product;
        private int invoiceIndex;
        private Boolean canCharge;
        private Boolean canEditCustom;
        private Boolean canEditProd;
        private Boolean canEditLabeled;
        private Boolean canEditConfirmed;
        private Boolean canEditSent;
        private Boolean canEditInvoiced;
        private Boolean canEditAccounted;

        public ZoomOrder zoomOrder { get; set; }
        public ControlPanel(User user)
        {
            InitializeComponent();

            u = user;

            //Carga los privilegios del usuario en su atributo de permisos
            u.Permissions = new List<Permission>();
            u.chargePrivileges();

            lblNameUser.Content = u.name;
            lblDateTime.Content = DateTime.Now.ToString("dd/MM/yyyy");

            //Comprobacion de permisos

            //Permisos show user
            if (u.Permissions.Contains(new Permission(1, "Show_user")))
            {
                //Lee todos los usuarios menos root y el propio usuario y se insertan en el datagrid
                u.manage = new UserManage();
                u.readAllMinus();
                dgvUsers.ItemsSource = null;
                dgvUsers.ItemsSource = u.manage.list;
            }
            else
            {
                //Si no tienen el permiso de show, no se muestra el tab directamente
                //Se usa Collapsed en vez de hidden para que el control no ocupe espacio
                tbUsers.Visibility = Visibility.Collapsed;
            }

            //Permisos edit user
            if (!u.Permissions.Contains(new Permission(2, "Edit_user")))
            {
                btnMod.IsEnabled = false;
                btnDel.IsEnabled = false;
            }

            //Permisos add user
            if (!u.Permissions.Contains(new Permission(3, "Add_user")))
            {
                btnAdd.IsEnabled = false;
            }

            //Permisos show client
            if (u.Permissions.Contains(new Permission(4, "Show_client")))
            {

                //Se prepara el objeto customer para la pestaña customer
                customer = new Customer();

                //Cargar lista de customers en el datagrid
                Customer customerL = new Customer();
                customerL.readAll();
                dgvCustomers.ItemsSource = null;
                dgvCustomers.ItemsSource = customerL.manage.GenericList;


                //Carga de todas las regiones al combobox de regiones para la pestaña customer
                Region reg = new Region();
                reg.chargeRegions();
                cboRegions.ItemsSource = reg.manage.GenericList;
                canCharge = true; //Valor boolean para los selection change de la pestaña customer


                //Se desactivan todos los campos relacionados con el cliente (Si quiere que se activen tiene que darle a new o seleccionar un cliente para editarlo)
                txtNameCustomer.IsEnabled = false;
                txtNIF.IsEnabled = false;
                txtSurname.IsEnabled = false;
                txtAddress.IsEnabled = false;
                txtEmail.IsEnabled = false;
                txtPhone.IsEnabled = false;
                txtZipcode.IsEnabled = false;
                cboRegions.IsEnabled = false;
                cboStates.IsEnabled = false;
                cboCities.IsEnabled = false;
                cboZipcodes.IsEnabled = false;
                btnSaveCustomer.IsEnabled = false;

            }
            else
            {
                tbiCustomers.Visibility = Visibility.Collapsed;
            }

            //Permisos edit client
            if (!u.Permissions.Contains(new Permission(5, "Edit_client")))
            {
                btnDelCustomer.IsEnabled = false;
                canEditCustom = false;

            }
            else
            {
                canEditCustom = true;
            }

            //Permisos add client
            if (!u.Permissions.Contains(new Permission(6, "Add_client")))
            {
                btnNewCustomer.IsEnabled = false;
            }

            //Permisos edit role
            if (!u.Permissions.Contains(new Permission(8, "Edit_role")))
            {
                btnRolP.IsEnabled = false;
            }

            //Permisos show product
            if (u.Permissions.Contains(new Permission(10, "Show_product")))
            {
                //Carga de productos para la pestaña products
                product = new Product();
                //Cargar lista de products en el datagrid
                Product productL = new Product();
                productL.readAllProducts();
                dgvProducts.ItemsSource = null;
                dgvProducts.ItemsSource = productL.manage.GenericList;
                //Carga de todas las marcas(brands)
                Brand brand = new Brand();
                brand.chargeBrands();
                cboProductBrand.ItemsSource = brand.manage.GenericList;
                //Carga de todos los tamaños(sizes)
                SizeDrive size = new SizeDrive();
                size.chargeSizes();
                cboProductSize.ItemsSource = size.manage.GenericList;

                //Se desactivan y una vez le des a new o selecciones un producto para editarlo se activarán
                txtProductName.IsEnabled = false;
                txtProductName.IsEnabled = false;
                txtProductPrice.IsEnabled = false;
                txtProductStock.IsEnabled = false;
                cboProductBrand.IsEnabled = false;
                cboProductSize.IsEnabled = false;
                btnProductSave.IsEnabled = false;
            }
            else
            {
                tbiProducts.Visibility = Visibility.Collapsed;
            }

            //Permisos edit product
            if (!u.Permissions.Contains(new Permission(11, "Edit_product")))
            {
                btnProductDelete.IsEnabled = false;
                canEditProd = false;

            }
            else
            {
                canEditProd = true;
            }

            //Permisos add product
            if (!u.Permissions.Contains(new Permission(12, "Add_product")))
            {
                btnProductNew.IsEnabled = false;
            }

            //Permisos de orders
            if (u.Permissions.Contains(new Permission(13, "Show_order")))
            {
                //Carga de orders para la pestaña orders
                Order orderL = new Order();
                orderL.readAllOrders();
                dgvOrders.ItemsSource = orderL.manage.GenericList;
            }
            else
            {
                tbiOrders.Visibility = Visibility.Collapsed;
            }

            if (!u.Permissions.Contains(new Permission(14, "Edit_order")))
            {
                //Habrán permisos a parte para editar los estados
                btnOrderDelete.IsEnabled = false;
            }

            if (!u.Permissions.Contains(new Permission(15, "Add_order")))
            {
                btnOrderNew.IsEnabled = false;
            }

            canEditLabeled = u.Permissions.Contains(new Permission(16, "Edit_labeled"));
            canEditSent = u.Permissions.Contains(new Permission(17, "Edit_Sent"));
            canEditConfirmed = u.Permissions.Contains(new Permission(18, "Edit_Confirmed"));
            canEditInvoiced = u.Permissions.Contains(new Permission(19, "Edit_Invoiced"));

            //Permisos de invoice
            if (u.Permissions.Contains(new Permission(20, "Show_Invoice")))
            {
                //Carga de invoices para la pestaña invoices
                Invoice invoiceL = new Invoice();
                invoiceL.readAll();
                dgvInvoices.ItemsSource = null;
                dgvInvoices.ItemsSource = invoiceL.manage.list;

                invoiceIndex = 0;
                Boolean encontrado = true;
                foreach (Invoice i in invoiceL.manage.list) if (encontrado)
                    {
                        if (i.accounted == -1)
                        {
                            encontrado = false;
                        }
                        else
                        {
                            invoiceIndex++;
                        }
                    }


                invoiceIndex--;
            }
            else
            {
                tbiInvoices.Visibility = Visibility.Collapsed;
            }

            canEditAccounted = u.Permissions.Contains(new Permission(21, "Edit_Accounted"));
            if (!canEditAccounted) {
                btnInvoiceDelete.IsEnabled = false;
            }



            //Carga de roles del usuario para la pestaña profile
            Role roleaux = new Role();
            roleaux.readRoles(u.iduser);
            lstRolesProfile.ItemsSource = null;
            lstRolesProfile.ItemsSource = roleaux.manage.roles;


            //Carga de suppliers para la pestaña suppliers
            try
            {
                Supplier supplierL = new Supplier();
                supplierL.readAll();
                dgvSuppliers.ItemsSource = supplierL.manage.list;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }





        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AUsers aUsers = new AUsers(dgvUsers, u);
            aUsers.Show();
        }

        /**
         * Boton de borrar usuarios (Borrado logico)
         */
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            List<User> data = new List<User>();

            if (dgvUsers.SelectedItems.Count > 0)
            {
                data = (List<User>)dgvUsers.ItemsSource;
                //Por cada usuario seleccionado, lo borramos de la base de datos y del datagrid
                foreach (User user in dgvUsers.SelectedItems)
                {
                    //Borrado en la base de datos
                    user.delete();
                    //Borrado en el datagrid
                    data.Remove(user);
                }

                dgvUsers.ItemsSource = null;
                dgvUsers.ItemsSource = data;
            }
            else
            {
                MessageBox.Show("You must select at least one row");
            }
        }

        private void btnMod_Click(object sender, RoutedEventArgs e)
        {
            //Comprueba que solo hay un usuario seleccionado y que no es el propio usuario con el que ha iniciado sesion
            if (dgvUsers.SelectedItems.Count == 1)
            {
                AUsers aUsers;
                aUsers = new AUsers(dgvUsers, (User)dgvUsers.SelectedItems[0], u);
                aUsers.Show();
            }
            else
            {
                MessageBox.Show("You must select one row");
            }
        }

        private void btnRolP_Click(object sender, RoutedEventArgs e)
        {
            RolP rolP = new RolP();
            rolP.Show();
        }

        /**
         * Boton para cambiar la contraseña del propio usuario
         */
        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            String prev = resources.useful.GetSHA256(pwdPrevious.Password);
            if (prev.Equals(u.password))
            {
                if (!pwdPrevious.Password.Equals(pwdNewPassword.Password))
                {
                    if (pwdNewPassword.Password.Equals(pwdRepeatPassword.Password))
                    {
                        u.password = pwdNewPassword.Password;
                        u.changePassword();
                        MessageBox.Show("The password has changed");

                        pwdNewPassword.Clear();
                        pwdPrevious.Clear();
                        pwdRepeatPassword.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Repeat password invalid");
                    }
                }
                else
                {
                    MessageBox.Show("The new password must be different");
                }
            }
            else
            {
                MessageBox.Show("The previous password is invalid");
            }
        }


        /**
         * Boton de busqueda
         */
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            User aux = new User();
            aux.readSelectUsers(txtSearch.Text);

            dgvUsers.ItemsSource = null;
            dgvUsers.ItemsSource = aux.manage.list;


        }

        /**
         * Selection changed de regions, al elegirlo carga states
         */
        private void cboRegions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboRegions.SelectedIndex != -1 && canCharge) {
                State state = new State();
                state.chargeStates((Region)cboRegions.SelectedItem);

                cboStates.ItemsSource = state.manage.GenericList;

                cboCities.ItemsSource = null;
                cboZipcodes.ItemsSource = null;
            }
        }

        /**
         * Selection changed de states, al elegirlo carga Cities
         */
        private void cboStates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboStates.SelectedIndex != -1 && canCharge) {
                City city = new City();
                city.chargeCities((State)cboStates.SelectedItem);

                cboCities.ItemsSource = city.manage.GenericList;

                cboZipcodes.ItemsSource = null;
            }
        }

        /**
         * Selection changed de cities, al elegirlo carga Zipcodes
         */
        private void cboCities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboCities.SelectedIndex != -1  && canCharge) {
                Zipcode zipcode = new Zipcode();
                zipcode.chargeZipcodes((City)cboCities.SelectedItem, (State)cboStates.SelectedItem);

                cboZipcodes.ItemsSource = zipcode.manage.GenericList;
            }
        }

        /**
         * Evento que al pulsar una tecla comprueba el tamaño del textbox de los zipcodes, si es igual a 5 quiere decir que se ha introducido un zipcode y se cargan
         * el resto de campos en base al zipcode
         */
        private void txtZipcode_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtZipcode.Text.Length == 5) {
                Zipcode zipcode = new Zipcode();
                zipcode.searchZipcode(txtZipcode.Text.ToString());

                //Una vez encontrado un zipcode correcto, se cargan los campos
                if (zipcode.manage.GenericList.Count > 0) {


                    zipcode = (Zipcode)zipcode.manage.GenericList[0];

                    //Cargar Region, State, City... en base al zipcode
                    City city = new City();
                    city.chargeCities(zipcode);

                    State state = new State();
                    state.chargeStates(zipcode);

                    Region region = new Region();
                    region.chargeRegions(zipcode);



                    //El cbo de regiones ya está cargado y despues de seleccionar un indice, se activará el evento selection changed y se cargará el de states, por lo que podremos elegir tambien un indice para el y así sucesivamente
                    List<Region> regions = cboRegions.ItemsSource.Cast<Region>().ToList();
                    cboRegions.SelectedIndex = regions.IndexOf((Region)region.manage.GenericList[0]); //Activa su selection change y carga el cbo de states

                    List<State> states = cboStates.ItemsSource.Cast<State>().ToList();
                    cboStates.SelectedIndex = states.IndexOf((State)state.manage.GenericList[0]); //Activa su selection change y carga el cbo de cities

                    List<City> cities = cboCities.ItemsSource.Cast<City>().ToList();
                    cboCities.SelectedIndex = cities.IndexOf((City)city.manage.GenericList[0]); //Activa su selection change y carga el cbo de zipcodes

                    List<Zipcode> zipcodes = cboZipcodes.ItemsSource.Cast<Zipcode>().ToList();
                    cboZipcodes.SelectedIndex = zipcodes.IndexOf(zipcode);

                }
            }

        }

        /**
         * Evento que añade un usuario a la base de datos en base a los campos que se han rellenado
         */
        private void btnSaveCustomer_Click(object sender, RoutedEventArgs e)
        {
            //Comprobacion de que todos los campos han sido rellenados

            if (txtNIF.Text.Length != 0 && txtNameCustomer.Text.Length != 0 && txtSurname.Text.Length != 0 && txtPhone.Text.Length != 0 && txtAddress.Text.Length != 0 && txtEmail.Text.Length != 0 &&
                cboRegions.SelectedIndex != -1 && cboStates.SelectedIndex != -1 && cboCities.SelectedIndex != -1 && cboZipcodes.SelectedIndex != -1)
            {
                customer.NIF = txtNIF.Text.ToString();
                customer.name = txtNameCustomer.Text.ToString();
                customer.surname = txtSurname.Text.ToString();
                customer.phone = Convert.ToInt32(txtPhone.Text);
                customer.address = txtAddress.Text.ToString();
                customer.email = txtEmail.Text.ToString();
                customer.zipcode = ((Zipcode)cboZipcodes.SelectedItem);
                customer.city = ((City)cboCities.SelectedItem);
                customer.state = ((State)cboStates.SelectedItem);
                customer.region = ((Region)cboRegions.SelectedItem);


                //Si no tiene el id a -1 quiere decir que el usuario existe y fue elegido en el datagrid, por lo tanto hay que modificarlo
                if (customer.id != -1)
                {
                    customer.modifyCustomer();


                    List<Customer> customers = dgvCustomers.ItemsSource.Cast<Customer>().ToList();
                    customers[customers.IndexOf(customer)] = customer;
                    dgvCustomers.ItemsSource = null;
                    dgvCustomers.ItemsSource = customers;

                }
                else
                {
                    //Si tiene el id a -1 se inserta ese usuario en la base de datos y en el datagrid
                    customer.insertCustomer();

                    List<Customer> customers = dgvCustomers.ItemsSource.Cast<Customer>().ToList();
                    customers.Add(customer);
                    dgvCustomers.ItemsSource = null;
                    dgvCustomers.ItemsSource = customers;

                    customer = new Customer();
                }
            }
            else {
                MessageBox.Show("You have not filled all the fields");
            }


        }

        /**
         * Evento que al seleccionar un customer del datagrid de customers, se cargan sus datos en los campos de la derecha
         */
        private void dgvCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgvCustomers.SelectedIndex != -1)
            {

                txtNIF.IsEnabled = false;

                customer = (Customer) dgvCustomers.SelectedItem;

                txtNameCustomer.Text = customer.name;
                txtNIF.Text = customer.NIF;
                txtSurname.Text = customer.surname;
                txtAddress.Text = customer.address;
                txtEmail.Text = customer.email;
                txtPhone.Text = Convert.ToString(customer.phone);

                //El cbo de regiones ya está cargado y despues de seleccionar un indice, se activará el evento selection changed y se cargará el de states, por lo que podremos elegir tambien un indice para el y así sucesivamente
                List<Region> regions = cboRegions.ItemsSource.Cast<Region>().ToList();
                cboRegions.SelectedIndex = regions.IndexOf(customer.region); //Activa su selection change y carga el cbo de states

                List<State> states = cboStates.ItemsSource.Cast<State>().ToList();
                cboStates.SelectedIndex = states.IndexOf(customer.state); //Activa su selection change y carga el cbo de cities

                List<City> cities = cboCities.ItemsSource.Cast<City>().ToList();
                cboCities.SelectedIndex = cities.IndexOf(customer.city); //Activa su selection change y carga el cbo de zipcodes

                List<Zipcode> zipcodes = cboZipcodes.ItemsSource.Cast<Zipcode>().ToList();
                cboZipcodes.SelectedIndex = zipcodes.IndexOf(customer.zipcode);


                //Si no tiene permisos para editar entonces se desactivan todos los textbox y botones
                if (!canEditCustom)
                {
                    txtNameCustomer.IsEnabled = false;
                    txtNIF.IsEnabled = false;
                    txtSurname.IsEnabled = false;
                    txtAddress.IsEnabled = false;
                    txtEmail.IsEnabled = false;
                    txtPhone.IsEnabled = false;
                    txtZipcode.IsEnabled = false;
                    cboRegions.IsEnabled = false;
                    cboStates.IsEnabled = false;
                    cboCities.IsEnabled = false;
                    cboZipcodes.IsEnabled = false;
                    btnSaveCustomer.IsEnabled = false;
                } else
                {
                    txtNameCustomer.IsEnabled = true;
                    txtNIF.IsEnabled = true;
                    txtSurname.IsEnabled = true;
                    txtAddress.IsEnabled = true;
                    txtEmail.IsEnabled = true;
                    txtPhone.IsEnabled = true;
                    txtZipcode.IsEnabled = true;
                    cboRegions.IsEnabled = true;
                    cboStates.IsEnabled = true;
                    cboCities.IsEnabled = true;
                    cboZipcodes.IsEnabled = true;
                    btnSaveCustomer.IsEnabled = true;
                }

            }

        }

        /**
         * Boton de nuevo cliente, vacia los campos y crea un nuevo objeto Cliente
         */
        private void btnNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            customer = new Customer();
            //Se activan todos los campos
            txtNIF.IsEnabled = true;
            txtNameCustomer.IsEnabled = true;
            txtNIF.IsEnabled = true;
            txtSurname.IsEnabled = true;
            txtAddress.IsEnabled = true;
            txtEmail.IsEnabled = true;
            txtPhone.IsEnabled = true;
            txtZipcode.IsEnabled = true;
            cboRegions.IsEnabled = true;
            cboStates.IsEnabled = true;
            cboCities.IsEnabled = true;
            cboZipcodes.IsEnabled = true;
            btnSaveCustomer.IsEnabled = true;

            txtNIF.Text = "";
            txtNameCustomer.Text = "";
            txtAddress.Text = "";
            txtEmail.Text = "";
            txtSurname.Text = "";
            txtPhone.Text = "";
            cboRegions.SelectedIndex = -1;
            //Se deja el resto en null
            cboStates.ItemsSource = null;
            cboCities.ItemsSource = null;
            cboZipcodes.ItemsSource = null;

        }

        /**
         * Evento de boton que al tener seleccionado un customer y darle a eliminar se borra el customer seleccionado tanto de la base de datos como de el datagrid
         */
        private void btnDelCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (dgvCustomers.SelectedItems.Count > 0) {
                List<Customer> customers = dgvCustomers.ItemsSource.Cast<Customer>().ToList();

                foreach(Customer c in dgvCustomers.SelectedItems)
                {
                    //Se elimina de la base de datos
                    c.removeCustomer();
                    //Se elimina de la copia de la lista
                    customers.Remove(c);
                }

                dgvCustomers.ItemsSource = null;
                dgvCustomers.ItemsSource = customers;


            }
        }

        /**
         * Evento que al seleccionar un producto del datagrid de productos, se cargan sus datos en los campos de la derecha
         */
        private void dgvProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgvProducts.SelectedIndex != -1) {
                product = (Product)dgvProducts.SelectedItem;

                txtProductName.Text = product.name;
                txtProductPrice.Text = Convert.ToString(product.price);
                txtProductStock.Text = Convert.ToString(product.stock);

                List<Brand> brands = cboProductBrand.ItemsSource.Cast<Brand>().ToList();
                cboProductBrand.SelectedIndex = brands.IndexOf(product.brand);

                List<SizeDrive> sizes = cboProductSize.ItemsSource.Cast<SizeDrive>().ToList();
                cboProductSize.SelectedIndex = sizes.IndexOf(product.size);


                if (!canEditProd)
                {
                    txtProductName.IsEnabled = false;
                    txtProductName.IsEnabled = false;
                    txtProductPrice.IsEnabled = false;
                    txtProductStock.IsEnabled = false;
                    cboProductBrand.IsEnabled = false;
                    cboProductSize.IsEnabled = false;
                    btnProductSave.IsEnabled = false;
                } else {
                    txtProductName.IsEnabled = true;
                    txtProductName.IsEnabled = true;
                    txtProductPrice.IsEnabled = true;
                    txtProductStock.IsEnabled = true;
                    cboProductBrand.IsEnabled = true;
                    cboProductSize.IsEnabled = true;
                    btnProductSave.IsEnabled = true;
                }

            }
        }

        /**
         * Boton de guardar producto en la base de datos
         */
        private void btnProductSave_Click(object sender, RoutedEventArgs e)
        {
            //Comprobacion de que todos los campos han sido rellenados
            if (txtProductName.Text.Length != 0 && txtProductPrice.Text.Length != 0 && cboProductSize.SelectedIndex != -1 && cboProductBrand.SelectedIndex != -1)
            {
                product.size = (SizeDrive)cboProductSize.SelectedItem;
                product.brand = (Brand)cboProductBrand.SelectedItem;
                product.name = txtProductName.Text;
                product.stock = Convert.ToInt32(txtProductStock.Text);
                product.price = Convert.ToDouble(txtProductPrice.Text);

                if (product.id != -1)
                {
                    product.modifyProduct();

                    List<Product> products = dgvProducts.ItemsSource.Cast<Product>().ToList();
                    products[products.IndexOf(product)] = product;
                    dgvProducts.ItemsSource = null;
                    dgvProducts.ItemsSource = products;

                }
                else
                {
                    product.insertProduct();

                    List<Product> products = dgvProducts.ItemsSource.Cast<Product>().ToList();
                    products.Add(product);
                    dgvProducts.ItemsSource = null;
                    dgvProducts.ItemsSource = products;

                    product = new Product();
                }
            } else

            {
                MessageBox.Show("You have not filled all the fields");
            }

        }

        /**
         * Boton de nuevo producto, vacia los campos y crea un nuevo objeto Producto
         */
        private void btnProductNew_Click(object sender, RoutedEventArgs e)
        {
            product = new Product();

            txtProductName.Text = "";
            txtProductPrice.Text = "";
            cboProductBrand.SelectedIndex = -1;
            cboProductSize.SelectedIndex = -1;

            txtProductName.IsEnabled = true;
            txtProductName.IsEnabled = true;
            txtProductPrice.IsEnabled = true;
            txtProductStock.IsEnabled = true;
            cboProductBrand.IsEnabled = true;
            cboProductSize.IsEnabled = true;
            btnProductSave.IsEnabled = true;
        }

        /**
         * Boton de borrar producto elegido del datagrid
         */
        private void btnProductDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgvProducts.SelectedItems.Count > 0) {
                List<Product> products = dgvProducts.ItemsSource.Cast<Product>().ToList();

                foreach (Product p in dgvProducts.SelectedItems)
                {
                    //Se elimina de la base de datos
                    p.removeProduct();
                    //Se elimina de la copia de la lista
                    products.Remove(p);
                }

                dgvProducts.ItemsSource = null;
                dgvProducts.ItemsSource = products;
            }
        }

        private void txtSearchCustomer_KeyUp(object sender, KeyEventArgs e)
        {

            Customer customerL = new Customer();
            TextBlock text = (TextBlock)cboCustomerCategory.SelectedItem;
            if (text.Text.Equals("NAME"))
            {
                customerL.name = txtSearchCustomer.Text;
            }
            else if (text.Text.Equals("NIF")){
                customerL.NIF = txtSearchCustomer.Text;
            }

            customerL.readCustomer();

            dgvCustomers.ItemsSource = null;
            dgvCustomers.ItemsSource = customerL.manage.GenericList;
        }

        private void txtSearchProduct_KeyUp(object sender, KeyEventArgs e)
        {
            Product productL = new Product();
            productL.name = txtSearchProduct.Text;
            productL.readProductName();

            dgvProducts.ItemsSource = null;
            dgvProducts.ItemsSource = productL.manage.GenericList;
        }

        /**
         * Evento para cuando le des a una celda doble click, cambia su color y muestra el numero de columna (Temporal)
         * se utilizará mas tarde para controlar los estados de orders
         */
        private void cell_Click(object sender, RoutedEventArgs e)
        {
            DataGridCell columnCell = (DataGridCell)sender;

            if(dgvOrders.SelectedIndex != -1)
            {
                Order order = (Order)dgvOrders.SelectedItem;
                //Se comprueba el nombre de cabecera al que le ha dado doble click, se cambia ese atributo a 0 y se pone en verde esa celda
                //Ademas se actualiza el estado del pedido
                switch (columnCell.Column.Header.ToString())
                {
                    case "CONF":
                        if (canEditConfirmed)
                        {
                            order.confirmed = -1;
                        }
                        break;
                    case "LAB":
                        if (canEditLabeled)
                        {
                            //Solo puede revertirse labeled
                            if (order.labeled == 0)
                            {
                                order.labeled = -1;
                            } else
                            {
                                order.labeled = 0;
                            }
                           
                        }
                        break;
                    case "SENT":
                        if (canEditSent)
                        {
                            order.sent = -1;
                        }
                        break;
                    case "INV":
                        if (canEditInvoiced && order.invoiced == 0)
                        {
                            order.invoiced = -1;

                            //Se hace un invoice
                            Invoice invoice = new Invoice();
                            invoice.generateInvoiceID();
                            invoice.accounted = 0;
                            invoice.insertInvoice(order);

                            //Actualizamos la tabla
                            List<Invoice> invoices = dgvInvoices.ItemsSource.Cast<Invoice>().ToList();
                            //Se inserta como el primero de la lista, ya que tiene un orden de fecha descendente (Mas reciente a menos)
                            invoices.Insert(0,invoice);
                            
                            dgvInvoices.ItemsSource = null;
                            dgvInvoices.ItemsSource = invoices;

                            invoiceIndex++;

                            //Se muestra el invoice
                            PrintInvoice pi = new PrintInvoice(order, invoice);
                            pi.Show();
                        }
                        break;
                }
                order.updateOrderStatus();

                //Actualizamos la tabla (Reinsertamos su lista)
                List<Order> orders = dgvOrders.ItemsSource.Cast<Order>().ToList();
                dgvOrders.ItemsSource = null;
                dgvOrders.ItemsSource = orders;
            }



            //MessageBox.Show(Convert.ToString(columnCell.Column.DisplayIndex)); //Muestra la posicion de la columna
            //columnCell.Background = new SolidColorBrush(System.Windows.Media.Colors.Green);
            //String a = ((TextBlock)columnCell.Content).Text;
            //MessageBox.Show(a);
        }

        /**
         * Evento de boton para abrir una ventana donde se creará un order
         */
        private void btnOrderNew_Click(object sender, RoutedEventArgs e)
        {
            ZoomOrder zoomOrder = new ZoomOrder(this,false);
            zoomOrder.Show();
            this.Hide();
        }

        /**
         * Evento que al seleccionar un supplier del datagrid de suppliers, se cargan sus campos de la derecha
         */
        private void dgvSuppliers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgvSuppliers.SelectedIndex != -1) {
                Supplier supplier = (Supplier)dgvSuppliers.SelectedItem;

                txtSupplierName.Text = supplier.Name;
                txtSupplierSurname.Text = supplier.Surname;
                txtSupplierPhone.Text = Convert.ToString(supplier.Phone);
                txtSupplierPage.Text = supplier.page;
                txtSupplierAddress.Text = supplier.Address;
            }
        }
        /**
         * Evento que al hacer doble click al datagrid y si hay un producto seleccionado y si la ventana de ZoomOrder está abierta traerá el producto hasta esa ventana
         */
        private void dgvProducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            if (dgvProducts.SelectedIndex != -1 && zoomOrder != null)
            {
                zoomOrder.product = product;
                zoomOrder.txtProductNameOrder.Text = product.name;
                zoomOrder.txtProductPriceOrder.Text = product.price.ToString();
                zoomOrder.Hide();
                zoomOrder.Show();

                this.Hide();
            }
        }
        /**
        * Evento que al hacer doble click al datagrid y si hay un customer seleccionado y si la ventana de ZoomOrder está abierta traerá el customer hasta esa ventana
        */
        private void dgvCustomers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //En caso de que ZoomOrder esté abierto, que se lleve customer
            if (dgvCustomers.SelectedIndex != -1 && zoomOrder != null)
            {
                zoomOrder.customer = customer;
                zoomOrder.txtClient.Text = customer.name;
                zoomOrder.Hide();
                zoomOrder.Show();

                this.Hide();
            }
        }

        /**
         * Evento de boton que al darle click abrirá la ventana de ZoomOrder para ver detalladamente la información del order seleccionado
         */
        private void btnOrderZoom_Click(object sender, RoutedEventArgs e)
        {

            if (dgvOrders.SelectedIndex != -1)
            {
                Order order = (Order)dgvOrders.SelectedItem;
                ZoomOrder zoomOrder = new ZoomOrder(order, this);
                zoomOrder.Show();
                this.Hide();
            }
        }

        /**
         * Evento de boton que al darle click eliminará un order seleccionado del datagrid
         */
        private void btnOrderDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgvOrders.SelectedItems.Count > 0)
            {
                List<Order> orders = dgvOrders.ItemsSource.Cast<Order>().ToList();

                Invoice invoice;
                foreach (Order o in dgvOrders.SelectedItems)
                {
                    //Se elimina de la base de datos
                    o.deleteOrder();
                    //Se elimina de la copia de la lista
                    orders.Remove(o);

                    //Si tiene una factura tambien la elimina de la base de datos y del datagrid
                    if (o.invoiced == -1) {
                        invoice = new Invoice();
                        invoice.readInvoiceByOrder(o);
                        invoice = invoice.manage.list[0];
                        invoice.deleteInvoice();

                        List<Invoice> invoices = dgvInvoices.ItemsSource.Cast<Invoice>().ToList();
                        invoices.Remove(invoice);
                        dgvInvoices.ItemsSource = null;
                        dgvInvoices.ItemsSource = invoices;
                    }
                }

                dgvOrders.ItemsSource = null;
                dgvOrders.ItemsSource = orders;
            }

        }
        /**
         * Evento de datepicker que al cambiar de fecha cargará los orders de esa fecha
         */
        private void dtpFechaOrder_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtpFechaOrder.SelectedDate != null)
            {
                Order orderL = new Order();
                orderL.dateTime = (DateTime) dtpFechaOrder.SelectedDate;
                orderL.readOrderByDate();
                dgvOrders.ItemsSource = null;
                dgvOrders.ItemsSource = orderL.manage.GenericList;
            }
        }

        /**
         * Evento de boton para limpiar el datepicker y leer de nuevo todos los orders
         */
        private void btnClearDtpOrder_Click(object sender, RoutedEventArgs e)
        {
            dtpFechaOrder.SelectedDate = null;
            Order orderL = new Order();
            orderL.readAllOrders();
            dgvOrders.ItemsSource = null;
            dgvOrders.ItemsSource = orderL.manage.GenericList;
        }


        private void btnInvoiceNew_Click(object sender, RoutedEventArgs e)
        {
            ZoomOrder zoomOrder = new ZoomOrder(this, true);
            zoomOrder.Show();
            this.Hide();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (zoomOrder != null)
            {
                e.Cancel = true;
            }
        }

        private void btnInvoicePrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgvInvoices.SelectedIndex != -1) {
                Invoice invoice = (Invoice)dgvInvoices.SelectedItem;
                
                Order order = new Order();
                order.readOrderByInvoice(invoice);
                order = (Order)order.manage.GenericList[0];

                PrintInvoice pi = new PrintInvoice(order,invoice);
                pi.Show();

                


            }
        }

        private void dgvInvoices_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgvInvoices.SelectedIndex != -1 && canEditAccounted)
            {
                if (dgvInvoices.SelectedIndex == invoiceIndex || dgvInvoices.SelectedIndex == invoiceIndex + 1)
                {
                    int accounted = 0;
                    Invoice invoice = (Invoice)dgvInvoices.SelectedItem;

                    if (invoice.accounted == 0)
                    {
                        accounted = -1; //Activa
                        if (invoiceIndex > -1)
                        {
                            invoiceIndex--;
                        }
                    }
                    else
                    {
                        invoiceIndex++;
                    }

                    invoice.accounted = accounted;

                    //Se actualiza en la base de datos
                    invoice.updateAccounted();

                    //Actualizamos la tabla (Reinsertamos su lista)
                    List<Invoice> invoices = dgvInvoices.ItemsSource.Cast<Invoice>().ToList();
                    dgvInvoices.ItemsSource = null;
                    dgvInvoices.ItemsSource = invoices;

                }
            }
        }

        private void btnInvoiceDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgvInvoices.SelectedItems.Count > 0)
            {
                List<Invoice> invoices = dgvInvoices.ItemsSource.Cast<Invoice>().ToList();
                List<Order> orders = dgvOrders.ItemsSource.Cast<Order>().ToList();

                Order o;
                foreach (Invoice i in dgvInvoices.SelectedItems)
                {
                    o = new Order();
                    //Se elimina de la base de datos
                    i.deleteInvoice();
                    //Se encuentra su respectivo order y se actualiza su estado de invoiced tanto en la base de datos como en el datagrid
                    o.readOrderByInvoice(i);
                    o = (Order)o.manage.GenericList[0];
                    o.invoiced = 0;
                    orders[orders.IndexOf(o)].invoiced = 0;
                    o.updateOrderStatus();

                    //Se elimina de la copia de la lista
                    invoices.Remove(i);
                }

                if (invoiceIndex > -1) {
                    invoiceIndex--;
                }

                //Actualizacion de ambos datagrid

                dgvOrders.ItemsSource = null;
                dgvOrders.ItemsSource = orders;

                dgvInvoices.ItemsSource = null;
                dgvInvoices.ItemsSource = invoices;
            }
        }

        private void btnInvoiceZoom_Click(object sender, RoutedEventArgs e)
        {
            if (dgvInvoices.SelectedIndex != -1)
            {
                Invoice invoice = (Invoice)dgvInvoices.SelectedItem;
                Order order = new Order();
                order.readOrderByInvoice(invoice);
                order = (Order)order.manage.GenericList[0];

                ZoomOrder zoomOrder = new ZoomOrder(order,invoice, this);
                zoomOrder.Show();
                this.Hide();
            }
        }

        private void dtpFechaInvoice_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtpFechaInvoice.SelectedDate != null)
            {
                Invoice invoiceL = new Invoice();
                invoiceL.dateTime = (DateTime)dtpFechaInvoice.SelectedDate;
                invoiceL.readInvoiceByDate();
                dgvInvoices.ItemsSource = null;
                dgvInvoices.ItemsSource = invoiceL.manage.list;

                //Se evita las operaciones de accounted
                canEditAccounted = false;
            }
        }

        private void btnClearDtpInvoice_Click(object sender, RoutedEventArgs e)
        {
            dtpFechaInvoice.SelectedDate = null;
            Invoice invoiceL = new Invoice();
            invoiceL.readAll();
            dgvInvoices.ItemsSource = null;
            dgvInvoices.ItemsSource = invoiceL.manage.list;

            canEditAccounted = u.Permissions.Contains(new Permission(21, "Edit_Accounted"));

        }
    }
}
