using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WPFMediaKit.DirectShow.Controls;
using ZXing;

namespace QRcode
{
    /// <summary>
    /// The interaction logic of ScanQRcode.xaml
    /// </summary>
    public partial class ScanQRcode : Window
    {

        //FilterInfoCollection filterInfoCollection;

        BarcodeReader codeReader = new BarcodeReader();
        //QRCodeDecoder decoder = new QRCodeDecoder();
        OpenFileDialog openFileDialog = new OpenFileDialog();

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
        /// Timer method
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
                var result = codeReader.Decode(btiMap);
                if (result != null)
                {
                    // Stop recognition
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
        /// Convert bitmap to ImageSource
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
                //Control the development of the camera
                vce.VideoCaptureSource = camera;
                cameraTimer.IsEnabled = false;
                cameraTimer.Interval = new TimeSpan(200); 
                cameraTimer.Tick += cameraTimer_Tick;
            }
        }

        private void btnScan_Click(object sender, RoutedEventArgs e)
        {
            if (openFileDialog.ShowDialog() == true)
            {
                BarcodeReader reader = new BarcodeReader { AutoRotate = true };
                Result result = reader.Decode(new Bitmap(openFileDialog.FileName));
                if (result != null)
                {
                    string decoded = result.ToString().Trim();
                    textQR.Text = decoded;
                }
            }
        }
    }
}
