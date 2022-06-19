using LittleERP.Domain;
using LittleERP.Domain.Manage;
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

namespace LittleERP.View
{
    /// <summary>
    /// Lógica de interacción para RolP.xaml
    /// </summary>
    public partial class RolP : Window
    {
        public RolP()
        {
            InitializeComponent();

            //Cargar Lista de roles y añadirla al combobox
            Role aux = new Role();
            aux.readAll();
            cboRoles.ItemsSource = null;
            cboRoles.ItemsSource = aux.manage.roles;

        }

        private void cboRoles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Permission> assignedPermissions = new List<Permission>();
            List<Permission> allowedPermissions = new List<Permission>();

            //Al seleccionar carga los permisos de ese rol

            //Carga permisos disponibles
            Permission aux = new Permission();
            aux.readAll();
            allowedPermissions=aux.manage.permissions.ToList<Permission>();

            //Carga permisos assignados de el rol elegido
            Permission aux2 = new Permission();
            Role role = (Role)cboRoles.SelectedItem;
            aux2.readPermissions(role);
            assignedPermissions=aux2.manage.permissions.ToList<Permission>();
            
            //Elimina de permisos disponibles los permisos ya asignados de ese rol
            foreach (Permission perm in assignedPermissions) {
                allowedPermissions.Remove(perm);
            }

            lstAllowedP.ItemsSource = null;
            lstAllowedP.ItemsSource = allowedPermissions;
            lstAssignedP.ItemsSource = null;
            lstAssignedP.ItemsSource = assignedPermissions;


            


            

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAddAssigned_Click(object sender, RoutedEventArgs e)
        {
            List<Permission> assignedPermissions = new List<Permission>();
            List<Permission> allowedPermissions = new List<Permission>();

            if (lstAssignedP.SelectedItems.Count > 0)
            {
                assignedPermissions = (List<Permission>)lstAssignedP.ItemsSource;
                allowedPermissions = (List<Permission>)lstAllowedP.ItemsSource;

                //Para cada item seleccionado de Assigned Permissions se lo añadimos a Allowed Permissions y despues lo eliminamos de Assigned
                foreach (Permission assigPerm in lstAssignedP.SelectedItems)
                {
                    allowedPermissions.Add(assigPerm);
                    assignedPermissions.Remove(assigPerm);
                }

                //Se vuelven a insertar las listas
                lstAllowedP.ItemsSource = null;
                lstAllowedP.ItemsSource = allowedPermissions;

                lstAssignedP.ItemsSource = null;
                lstAssignedP.ItemsSource = assignedPermissions;
            }
        }

        private void btnAddAllowed_Click(object sender, RoutedEventArgs e)
        {
            List<Permission> assignedPermissions = new List<Permission>();
            List<Permission> allowedPermissions = new List<Permission>();

            if (lstAllowedP.SelectedItems.Count > 0)
            {
                assignedPermissions = (List<Permission>)lstAssignedP.ItemsSource;
                allowedPermissions = (List<Permission>)lstAllowedP.ItemsSource;

                //Para cada item seleccionado de Allowed Permissions se lo añadimos a Assigned Permissions y despues lo eliminamos de Allowed
                foreach (Permission assigPerm in lstAllowedP.SelectedItems)
                {
                    assignedPermissions.Add(assigPerm);
                    allowedPermissions.Remove(assigPerm);
                }

                //Se vuelven a insertar las listas
                lstAllowedP.ItemsSource = null;
                lstAllowedP.ItemsSource = allowedPermissions;

                lstAssignedP.ItemsSource = null;
                lstAssignedP.ItemsSource = assignedPermissions;
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            //Se cambian los permisos de ese rol en base a la lista de asignados
            Role role = (Role)cboRoles.SelectedItem;
            role.changeRole((List<Permission>)lstAssignedP.ItemsSource);

            MessageBox.Show("THE ROLE HAS CHANGED");
        }
    }
}
