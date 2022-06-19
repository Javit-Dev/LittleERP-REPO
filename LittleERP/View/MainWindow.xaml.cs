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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LittleERP.View
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            //Recogemos los datos del usuario
            User user = new User();
            user.name = txtLogin.Text;
            user.password = resources.useful.GetSHA256(pwdPassword.Password);

            Boolean exist = user.check(); //Comprobamos que existe en la base de datos
            if (exist)
            {
                user.readUserId(); //Se le asigna el id

                //Comprueba si es la primera sesion del usuario, en ese caso deberá cambiar su contraseña
                if (!user.isFirstSession())
                {
                    ControlPanel controlPanel = new ControlPanel(user);
                    controlPanel.Show();
                    this.Close();
                }
                else {
                    MessageBox.Show("You need to change your password");
                    AUsers aUsers = new AUsers(user);
                    aUsers.Show();
                }
            }
            else {
                MessageBox.Show("The User doesn't exist");
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
