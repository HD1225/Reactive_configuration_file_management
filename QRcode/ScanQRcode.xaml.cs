using Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
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
using System.Windows.Threading;
using System.Xml.Linq;
using WPFMediaKit.DirectShow.Controls;
using ZXing;

namespace QRcode
{
    /// <summary>
    /// ScanQRcode.xaml 的交互逻辑
    /// </summary>
    public partial class ScanQRcode : Window
    {

        //FilterInfoCollection filterInfoCollection;

        BarcodeReader codeReader = new BarcodeReader();

        /// <summary>
        /// Minuteur
        /// </summary>
        DispatcherTimer cameraTimer = new DispatcherTimer();
        private Logic.QRcode qrcode;

        public ScanQRcode(Logic.QRcode qrcode)
        {
            InitializeComponent();
            this.qrcode = qrcode;
            CameraList = MultimediaUtil.VideoInputNames.ToList();

            cb.ItemsSource = CameraList;
        }

        public List<string> CameraList
        {
            get;
            set;
        }
        /// <summary>
        /// Méthode de la minuterie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cameraTimer_Tick(object sender, EventArgs e)
        {
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)vce.ActualWidth, (int)vce.ActualHeight, 96, 96, PixelFormats.Default);
            vce.Measure(vce.RenderSize);
            vce.Arrange(new Rect(vce.RenderSize));
            bmp.Render(vce);
            BitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                Bitmap btiMap = new Bitmap(ms);
                var result = codeReader.Decode(btiMap);//Analyser le code-barres
                if (result != null)
                {
                    // 1:Arrêter la reconnaissance
                    cameraTimer.Stop();
                    vce.Play();
                    MessageBox.Show($"Le contenu d'identification est：{result}");

                }
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            cameraTimer.Start();
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            cameraTimer.Start();
        }

        /// <summary>
        /// Convertir de bitmap en ImageSource
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static ImageSource ChangeBitmapToImageSource(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();

            ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            if (!DeleteObject(hBitmap))
            {
                throw new System.ComponentModel.Win32Exception();
            }
            return wpfBitmap;
        }

        private static bool DeleteObject(IntPtr hBitmap)
        {
            throw new NotImplementedException();
        }


        private void cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var camera = cb.SelectedItem.ToString(); ;
            if (MultimediaUtil.VideoInputNames.Contains(camera))
            {
                //Contrôler le développement de la caméra
                vce.VideoCaptureSource = camera;
                cameraTimer.IsEnabled = false;
                cameraTimer.Interval = new TimeSpan(200); 
                cameraTimer.Tick += cameraTimer_Tick;
            }
        }
    }
}
