using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml;
using ZXing;

namespace QRcode
{
    /// <summary>
    /// Interactive logic of CreateQRcode.xaml
    /// </summary>
    public partial class CreateQRcode : Window
    {
        //FilterInfoCollection filterInfoCollection;

        BarcodeReader codeReader = new BarcodeReader();

        /// <summary>
        /// Minuteur
        /// </summary>
        public Logic.QRcode Qrcode { get; }

        public CreateQRcode(Logic.QRcode qrcode)
        {
            InitializeComponent();
            Qrcode = qrcode;
        }

        /// <summary>
        /// Generate QR code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            //Set the specifications of the QR code
            ZXing.QrCode.QrCodeEncodingOptions qrEncodeOption = new ZXing.QrCode.QrCodeEncodingOptions();
            //Set the encoding format, otherwise the Chinese characters are garbled
            qrEncodeOption.CharacterSet = "UTF-8";
            //Set width and height
            qrEncodeOption.Height = 200;
            qrEncodeOption.Width = 200;
            //Set the surrounding margin
            qrEncodeOption.Margin = 1;
            ZXing.BarcodeWriter qr = new BarcodeWriter();
            //QR code
            qr.Format = BarcodeFormat.QR_CODE;
            qr.Options = qrEncodeOption;
            //create QR code
            Bitmap image = qr.Write(textBlock.Text);
            image.Save("toto.jpg");
            //Display
            pictureBox.Source = new BitmapImage(new Uri(@"C:\Users\dan.hou\OneDrive - IDS PLC\Bureau\QRcode\QRcode\bin\Debug\toto.jpg"));
        }


        /// <summary>
        /// Save the QR code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            //save.Filter = "Image Files ( *.png, *.bmp, *.jpg)|*.bmp;*.png;*.jpg | All Files | *.*"; //Image Format
            save.Filter = "Image File ( *.jpg)|*.jpg |Image File ( *.png)|*.png|Image File ( *.bmp)|*.bmp| All Files | *.*"; //Image Format
            save.RestoreDirectory = true; //Whether the save dialog box remembers the last opened directory
            if (save.ShowDialog() == true)
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)this.pictureBox.Source));
                using (FileStream stream = new FileStream(save.FileName, FileMode.Create))
                    encoder.Save(stream);
            }
        }

        private void Removespaces_Click(object sender, RoutedEventArgs e)
        {
            string xml = textBlock.Text;
            string xml2 = xml.Trim();
            //Console.WriteLine(xml);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml2);
            MemoryStream mstream = new MemoryStream(1024);
            XmlTextWriter writer = new XmlTextWriter(mstream, null);
            writer.Formatting = System.Xml.Formatting.Indented;
            doc.WriteTo(writer);
            writer.Flush();
            writer.Close();
            Encoding encoding = Encoding.GetEncoding("utf-8");
            string result = encoding.GetString(mstream.ToArray());
            textBlock.Clear();
            textBlock.Text = result;
            mstream.Close();
        }

        private void Convert_to_json_Click(object sender, RoutedEventArgs e)
        {
            string xml = textBlock.Text;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            string json = JsonConvert.SerializeXmlNode(doc);
            textBlock.Text = json;
            //Console.WriteLine(json);
        }
    }
}
