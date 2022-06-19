using LittleERP.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static LittleERP.resources.useful;

namespace LittleERP.View
{
    public partial class AUsers : Window
    {
        private User user;
        private operations op;
        private DataGrid dgvUsers;
        private int indexOriginalUser;

        /**
         * 
         * Constructor para añadir, no necesita ningun parametro, carga entera la lista de roles en allowed
         * NOTA: Mucho cuidado con la incoherencia de accesibilidad, se debe poner en public las clases (Ya que class asecas es private)
         */
        public AUsers(DataGrid dgv, User u)
        {
            InitializeComponent();

            dgvUsers = dgv;

            //Se indica que es una operacion de añadir, esto lo tendrá en cuenta el boton de ok
            op = operations.add; 

            //Carga todos los roles y los añade a la lista de allowed roles
            Role roleaux = new Role();
            roleaux.readAll();
            if(u.iduser != 0)
            {
                Role admin = new Role();
                admin.idrole = 1;
                roleaux.manage.roles.Remove(admin);
            }
            lstAllowedRoles.ItemsSource=roleaux.manage.roles;

            lstAssignedRoles.ItemsSource = new List<Role>(); //Tambien hay que inicializar assigned roles para poder añadir items
        }

        /**
         * Constructor para modificar modificar los roles de un usuario, por lo que desactiva el insertar el nombre
         */
        public AUsers(DataGrid dgv, User selectedUser, User u) {
            InitializeComponent();

            user = selectedUser;

            //El nomnbre no se va a poder editar, asi que va a ser solo de lectura
            txtName.Text = user.name;
            txtName.IsReadOnly = true;

            List<Role> assignedRoles = new List<Role>();
            List<Role> allowedRoles = new List<Role>();
            dgvUsers = dgv;
            indexOriginalUser=dgvUsers.SelectedItems.IndexOf(user); //Necesito el indice para luego modificarlo

            //Se indica que es una operacion de modificar
            op = operations.mod;


            //Ver los roles asignados de x usuario
            Role assignedRole = new Role();
            assignedRole.readRoles(user.iduser);            
            assignedRoles = assignedRole.manage.roles.ToList<Role>();


            //Ahora se debe de leer una lista entera de roles y eliminar los que esten asignados para dejarlos en allowed
            Role roleaux = new Role();
            roleaux.readAll();
            allowedRoles = roleaux.manage.roles.ToList<Role>(); //Copiamos la lista de roles a otra lista gracias al ToList, ya que si no solo apuntariamos a una sola lista y en el foreach provocaria problemas


            //Quitamos todos los roles asignados de la lista de allowed (El remove funciona en base al equals de la clase, si no hay equals compara el puntero, en este caso si tienen el mismo id de rol se elimina de la lista)
            foreach (Role assignRole in assignedRoles) {
                allowedRoles.Remove(assignRole);
            }

            //Si el usuario que esta modificando no es root que no se muestre el rol admin
            if (u.iduser != 0)
            {
                Role admin = new Role();
                admin.idrole = 1;
                assignedRole.manage.roles.Remove(admin);
                allowedRoles.Remove(admin);
            }

            //Se asigna esa lista
            lstAssignedRoles.ItemsSource = null;
            lstAllowedRoles.ItemsSource = allowedRoles;
            lstAssignedRoles.ItemsSource = null;
            lstAssignedRoles.ItemsSource = assignedRole.manage.roles;

        }

        /**
         * Constructor que solo se utiliza para el cambio de contraseña de un usuario dado por parametro, solo está activo el cambio de contraseña
         */
        public AUsers(User u) {
            InitializeComponent();

            user = u;

            txtName.Text = user.name;
            txtName.IsEnabled = false;

            //Solo el cambio de contraseña está disponible
            chkFirstSession.IsEnabled = false;
            btnAddAllowed.IsEnabled = false;
            btnAddAssigned.IsEnabled = false;
            lstAllowedRoles.IsEnabled = false;
            lstAssignedRoles.IsEnabled = false;


            op = operations.chpass;
        }

        /**
         * Pasan los elementos seleccionados de Assigned a Allowed
         */
        private void btnAddAssigned_Click(object sender, RoutedEventArgs e)
        {
            List<Role> assignedRoles = new List<Role>();
            List<Role> allowedRoles = new List<Role>();
            
            if (lstAssignedRoles.SelectedItems.Count > 0) {
                assignedRoles = (List<Role>)lstAssignedRoles.ItemsSource;
                allowedRoles = (List<Role>)lstAllowedRoles.ItemsSource;

                //Para cada item seleccionado de Assigned Roles se lo añadimos a Allowed Roles y despues lo eliminamos de Assigned
                foreach (Role assigRole in lstAssignedRoles.SelectedItems) {
                    allowedRoles.Add(assigRole);
                    assignedRoles.Remove(assigRole);
                }

                //Se vuelven a insertar las listas
                lstAllowedRoles.ItemsSource = null;
                lstAllowedRoles.ItemsSource = allowedRoles;

                lstAssignedRoles.ItemsSource = null;
                lstAssignedRoles.ItemsSource = assignedRoles;
            }

        }

        /**
         * Pasan los elementos seleccionados de Allowed a Assigned
         */
        private void btnAddAllowed_Click(object sender, RoutedEventArgs e)
        {
            List<Role> assignedRoles = new List<Role>();
            List<Role> allowedRoles = new List<Role>();

            if (lstAllowedRoles.SelectedItems.Count > 0)
            {
                assignedRoles = (List<Role>)lstAssignedRoles.ItemsSource;
                allowedRoles = (List<Role>)lstAllowedRoles.ItemsSource;

                //Para cada item seleccionado de Allowed Roles se lo añadimos a Assigned Roles y despues lo eliminamos de Allowed
                foreach (Role allowRole in lstAllowedRoles.SelectedItems)
                {
                    assignedRoles.Add(allowRole);
                    allowedRoles.Remove(allowRole);
                }

                //Se vuelven a insertar las listas
                lstAllowedRoles.ItemsSource = null;
                lstAllowedRoles.ItemsSource = allowedRoles;

                lstAssignedRoles.ItemsSource = null;
                lstAssignedRoles.ItemsSource = assignedRoles;
            }
        }

        /**
         * Accion del boton OK, dependiendo del tipo de operacion asignado añadirá o modificará un usuario
         */
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            switch (op) {
                case (operations.add):
                    addUser();
                    break;
                case (operations.mod):
                    modUser();
                    break;
                case (operations.chpass):
                    chUserPassword();
                    break;
            }
        }

        /**
         * Metodo para añadir usuario, comprueba que no existe ese usuario y que tiene al menos un rol asignado
         * Obliga al usuario al iniciar por primera vez sesión a cambiar contraseña
         * AÑADE EN LA TABLA USER Y USER_ROLES
         */
        public void addUser()
        {
            User user = new User();
            user.name = txtName.Text;

            //Si el nombre de usuario no existe que continue
            if (!user.checkName())
            {
                user.password = pwdPassword.Password;
                //Si la contraseña es igual a repetir contraseña
                if (pwdPassword.Password.Equals(pwdPasswordRepeat.Password))
                {
                    //Si hay al menos un rol asignado
                    if (lstAssignedRoles.Items.Count >= 1)
                    {
                        //Se inserta en la tabla de USERS y USER_ROLES
                        user.insert((List<Role>)lstAssignedRoles.ItemsSource);
                        MessageBox.Show("USER INSERTED");

                        //Tengo que añadirle la contraseña cifrada, si no se va a añadir a la tabla mal al salir
                        user.password = resources.useful.GetSHA256(pwdPassword.Password);

                        //Se inserta al datagrid para que no tenga que volver a hacer la consulta
                        List<User> data = (List<User>)dgvUsers.ItemsSource;
                        data.Add(user);
                        dgvUsers.ItemsSource = null;
                        dgvUsers.ItemsSource = data;

                        if (chkFirstSession.IsChecked.Value) {
                            user.markFirstSession(true);
                        }
                    }
                    else
                    {

                        MessageBox.Show("YOU MUST ASSIGN AT LEAST ONE ROLE");
                    }
                }
                else {
                    MessageBox.Show("YOU MUST CONFIRM THE PASSWORD");
                }


            }
        }

        /**
         * Metodo para modificar un usuario, comprueba que la contraseña es diferente a la que tiene actualmente
         * Obliga al usuario a cambiar contraseña en el siguiente inicio de sesión
         * Modifica el rol asignado y la contraseña, por lo que modifica tambien la tabla de USER_ROLES
         */
        public void modUser() {

            if (user.password != resources.useful.GetSHA256(pwdPassword.Password))
            {
                if (pwdPassword.Password.Equals(pwdPasswordRepeat.Password))
                {
                    //Si hay al menos un rol asignado
                    if (lstAssignedRoles.Items.Count >= 1)
                    {
                        //Modificar contraseña usuario
                        user.password = pwdPassword.Password;
                        //Modifica USER_ROLES
                        user.changeUser((List<Role>)lstAssignedRoles.ItemsSource);

                        //Modificar datagrid
                        List<User> data = (List<User>)dgvUsers.ItemsSource;
                        data[indexOriginalUser].password = resources.useful.GetSHA256(pwdPassword.Password);
                        dgvUsers.ItemsSource = null;
                        dgvUsers.ItemsSource = data;


                        if (chkFirstSession.IsChecked.Value)
                        {
                            user.markFirstSession(true);
                        }

                        MessageBox.Show("MODIFICATION COMPLETE");
                    }
                    else {
                        MessageBox.Show("YOU MUST ASSIGN AT LEAST ONE ROLE");
                    }
                }
                else {
                    MessageBox.Show("YOU MUST CONFIRM THE PASSWORD");
                }
            }
            else {
                MessageBox.Show("THE PASSWORD MUST BE DIFFERENT");
            }
        
        }

        public void chUserPassword()
        {
            if (user.password != resources.useful.GetSHA256(pwdPassword.Password))
            {
                if (pwdPassword.Password.Equals(pwdPasswordRepeat.Password))
                {
                    //Modificar contraseña usuario
                    user.password = pwdPassword.Password;
                    user.changePassword();

                    MessageBox.Show("THE PASSWORD HAS CHANGED");
                    user.markFirstSession(false);

                    this.Close();
                }
                else {
                    MessageBox.Show("YOU MUST CONFIRM THE PASSWORD");
                }
            }
            else {
                MessageBox.Show("THE PASSWORD MUST BE DIFFERENT");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
