using Logic;
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

namespace QRcode
{
    /// <summary>
    /// Interaction logic of MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Logic.QRcode qrcode;

        /// <summary>
        /// Afficher l'interface de la fenêtre principale
        /// </summary>
        public MainWindow()
        {
            this.qrcode = new Logic.QRcode();
            InitializeComponent();
        }

        private void DragGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btn_CreateQR(object sender, RoutedEventArgs e)
        {
            CreateQRcode gocreate = new CreateQRcode(qrcode);
            gocreate.Show();
        }

        private void btn_ScanQR(object sender, RoutedEventArgs e)
        {
            ScanQRcode goscan = new ScanQRcode(qrcode);
            goscan.Show();
        }
    }
}
