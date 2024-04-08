using System.Windows;
using System.Windows.Input;

namespace QRcode
{
    /// <summary>
    /// Interaction logic of MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CreateQRcode createQRcode;
        private readonly ScanQRcode scanQRcode;

        /// <summary>
        /// Show main window interface
        /// </summary>
        public MainWindow()
        {            
            InitializeComponent();
        }

        private void DragGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        /// <summary>
        /// Open the window of "CreateQRcode" 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CreateQR(object sender, RoutedEventArgs e)
        {
        
            CreateQRcode gocreate = new CreateQRcode(createQRcode);
            gocreate.Show();
        }

        /// <summary>
        /// Open the window of "ScanQRcode"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ScanQR(object sender, RoutedEventArgs e)
        {
            ScanQRcode goscan = new ScanQRcode(scanQRcode);
            goscan.Show();
        }
    }
}
