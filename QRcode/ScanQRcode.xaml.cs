using Microsoft.Win32;
using Newtonsoft.Json;
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
using System.Xml;
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
        /// Timer
        /// </summary>
        DispatcherTimer cameraTimer = new DispatcherTimer();
        
        private ScanQRcode qrcode;

        public ScanQRcode(ScanQRcode scanQRcode)
        {
            InitializeComponent();
            this.qrcode = scanQRcode;

            //Get the camera list
            CameraList = MultimediaUtil.VideoInputNames.ToList();
            cb.ItemsSource = CameraList;
        }

        /// <summary>
        /// Get the camera list
        /// </summary>
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
                    //Stop recognition
                    cameraTimer.Stop();
                    vce.Play();
                    MessageBox.Show($"Le contenu d'identification est：{result}");
                }
            }
        }

        /// <summary>
        /// Click the "start" button to scan the QR code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            cameraTimer.Start();
        }

        /// <summary>
        /// Click the "stop" button to scan the QR code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            cameraTimer.Stop();
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

        /// <summary>
        /// Select the QR code to be scanned
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (openFileDialog.ShowDialog() == true)
            {
                BarcodeReader reader = new BarcodeReader { AutoRotate = true };
                Result result = reader.Decode(new Bitmap(openFileDialog.FileName));
                if (result != null)
                {
                    string decoded = result.ToString().Trim();
                    //Display scan results in TextBox
                    textQR.Text = decoded;
                }
            }
        }

        /// <summary>
        /// Parse the string obtained after scanning the QR code and save it in the corresponding text format
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            string str1 = textQR.Text;
            str1 = str1.Trim();
            
            try
            {
                //If it is xml, the default save format is xml
                XmlDocument xdoc = new XmlDocument();            
                xdoc.LoadXml(textQR.Text);
                save.Filter = "XML File ( *.xml)|*.xml|JSON File ( *.json)|*.json|TEXT File ( *.txt)|*.txt";
                if (save.ShowDialog() == true)
                {
                    //Console.WriteLine(save.FilterIndex);
                    if(save.FilterIndex == 2)
                    {
                        string jsonText = JsonConvert.SerializeXmlNode(xdoc);
                        File.WriteAllText(save.FileName, jsonText);
                    }
                    else
                    {
                        xdoc.Save(save.FileName);
                    }                                   
                }
            }
            catch
            {
                try
                {
                    //If it is json, the default save format is json
                    JsonConvert.SerializeObject(str1);
                    save.Filter = "JSON File ( *.json)|*.json|XML File ( *.xml)|*.xml|TEXT File ( *.txt)|*.txt";
                    if (save.ShowDialog() == true)
                    {    
                        if(save.FilterIndex == 2)
                        {
                            XmlDocument xmlText = JsonConvert.DeserializeXmlNode(str1);
                            xmlText.Save(save.FileName);
                        }
                        else
                        {
                            File.WriteAllText(save.FileName, str1);
                        }                        
                    }
                }
                catch
                {
                    //The rest are saved in text format
                    save.Filter = "TEXT File ( *.txt)|*.txt";
                    if (save.ShowDialog() == true)                        
                    {                       
                        File.WriteAllText(save.FileName, str1);
                    }
                }
            }

        }

        /// <summary>
        /// Create and display the Cartridge window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cartridge_Click(object sender, RoutedEventArgs e)
        {
            //Declare json string
            string jsonStr;
            try
            {
                //Try to parse it in XML and convert it to JSON
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(textQR.Text);
                jsonStr = JsonConvert.SerializeXmlNode(xdoc);
            }
            catch
            {
                //If it fails, it means that it was originally json, so assign it directly
                jsonStr = textQR.Text;
            }
            //Remove the'@' symbol and parse the json
            MyDataFrame data = JsonConvert.DeserializeObject<MyDataFrame>(jsonStr.Replace("@",""));
            //Create a new window and pass data to the window
            Cartridge cartridgeWindow = new Cartridge(data);
            //Show window
            cartridgeWindow.ShowDialog();
        }

    }
}
