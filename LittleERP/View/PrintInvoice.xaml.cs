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

namespace LittleERP.View
{
    /// <summary>
    /// Lógica de interacción para PrintInvoice.xaml
    /// </summary>
    public partial class PrintInvoice : Window
    {
        private Order o;
        private Invoice i;
        public PrintInvoice(Order order, Invoice invoice)
        {
            InitializeComponent();

            o = order;
            i = invoice;

        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            CrystalReport1 miReporte = new CrystalReport1();
            miReporte.Database.Tables["OrderProducts"].SetDataSource(o.manage.torderproducts);
            miReporte.Database.Tables["InfoInvoice"].SetDataSource(i.manage.tinfinvoices);
            Cr1.ViewerCore.ReportSource = miReporte;
        }
    }
}
