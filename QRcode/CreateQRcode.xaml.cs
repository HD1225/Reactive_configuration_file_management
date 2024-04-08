using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml;
using ZXing;
using ZXing.QrCode;

namespace QRcode
{
    /// <summary>
    /// Interactive logic of CreateQRcode.xaml
    /// </summary>
    public partial class CreateQRcode : Window
    {
        //FilterInfoCollection filterInfoCollection;
        BarcodeReader codeReader = new BarcodeReader();
       
        public CreateQRcode Qrcode { get; }

        public CreateQRcode(CreateQRcode createQRcode)
        {
            InitializeComponent();
            Qrcode = createQRcode;
        }

        /// <summary>
        /// Set QR code properties
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        private QrCodeEncodingOptions getQrCodeEncodingOptions(int version)
        {
            //Set the specifications of the QR code
            QrCodeEncodingOptions qrEncodeOption = new QrCodeEncodingOptions();

            //Set the encoding format, otherwise the Chinese characters are garbled
            qrEncodeOption.CharacterSet = "UTF-8";
            //Set width and height
            qrEncodeOption.Height = 200;
            qrEncodeOption.Width = 200;
            //Set the surrounding margin
            qrEncodeOption.Margin = 1;
            //Set the version
            qrEncodeOption.QrVersion = version;

            return qrEncodeOption;
        }

        /// <summary>
        /// Image manipulation
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private Bitmap getQrCode(QrCodeEncodingOptions options)
        {
            if (options.QrVersion != null)
            {
                ZXing.BarcodeWriter qr = new BarcodeWriter();
                //QR code
                qr.Format = BarcodeFormat.QR_CODE;
                qr.Options = options;
                //create QR code
                Bitmap image = qr.Write(textBlock.Text);

                return image;
            }
            return null;           
        }

        /// <summary>
        /// Get the best version of the QR code
        /// </summary>
        /// <returns></returns>
        private int getOptimalVersion()
        {
            //Initialize the best version to 1
            int optimalVersion = 1;
            bool isOptimal = false;

            while (!isOptimal && optimalVersion <= 40)
            {
                try
                {
                    QrCodeEncodingOptions qrCodeEncodingOptions = getQrCodeEncodingOptions(optimalVersion);
                    Bitmap image = getQrCode(qrCodeEncodingOptions);
                    isOptimal = true;
                }
                catch
                {
                    isOptimal = false;
                    optimalVersion++;
                }
            }
            //If the selected version is not the best version, return 0
            return isOptimal ? optimalVersion:0;
        }

        /// <summary>
        /// Generate QR code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            int optimalVersion = getOptimalVersion();

            //When the best version is 0 or no version is selected
            if (0 == optimalVersion)
            {
                MessageBox.Show("We can't create the QR code", "ERROR");
            }
            else
            {
                //Select the QR code version in combobox
                int version = int.Parse(combobox1_version.Text);
                //If the selected version is less than the best version
                if (version < optimalVersion)
                {
                    //Prompt the best version
                    string message = "The optimal version is : " + optimalVersion;
                    MessageBox.Show(message, "INFO");
                }
                else
                {
                    try
                    {
                        QrCodeEncodingOptions qrCodeEncodingOptions = getQrCodeEncodingOptions(optimalVersion);
                        //Create QR code                  
                        Bitmap image = getQrCode(qrCodeEncodingOptions);
                        //Display QR code
                        pictureBox.Source = BitmapToBitmapImage(image);
                        string message = "Created successfully !";

                        //If the selected version is more than the best version
                        if (version > optimalVersion)
                        {
                            //Prompt the best version
                            message += "\nThe optimal version is : " + optimalVersion;
                        }                        
                        MessageBox.Show(message, "INFO");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "ERROR");
                    }
                }
            }

            /*              
            try
            {     
            
                //Set QR code encoding mode

                String encoding = combobox1_encoding.Text;
                if (encoding == "Numeric")               
                   qrEncodeOption.CharacterSet = "0-9";               
                else if (encoding == "AlphaNumeric")               
                  qrEncodeOption.CharacterSet = "Shift JIS";               
                else if (encoding == "Byte")               
                   qrEncodeOption.CharacterSet = "ISO - 8859 - 1";                
                else if (encoding == "Kanji")               
                   qrEncodeOption.CharacterSet = "Shift JIS";               
               
                if (textBlock.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Data must not be empty.");
                    return;
                }
                

                //Set QR code error correction level

                string errorCorrect = combobox4_correctionlevel.Text;
                if (errorCorrect == "L")
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                else if (errorCorrect == "M")
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                else if (errorCorrect == "Q")
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
                else if (errorCorrect == "H")
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;

            */
        }

        /*
         * 
        //Set QR code size       

        private void combobox_size(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                int scale = Convert.ToInt16(combobox2.Text);
                codeReader.QRCodeScale = scale;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid size!");
            }
        }

        */


        /// <summary>
        /// Bitmap --> BitmapImage
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        private BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png); 

                stream.Position = 0;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
                // Force the bitmap to load right now so we can dispose the stream.
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }


        /// <summary>
        /// Save the QR code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            //QR code image format
            save.Filter = "Image File ( *.jpg)|*.jpg |Image File ( *.png)|*.png|Image File ( *.bmp)|*.bmp| All Files | *.*";
            //Whether the save dialog box remembers the last opened directory
            save.RestoreDirectory = true; 

            if (save.ShowDialog() == true)
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)this.pictureBox.Source));
                using (FileStream stream = new FileStream(save.FileName, FileMode.Create))
                    encoder.Save(stream);
            }
        }

        /// <summary>
        /// Remove spaces in text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Removespaces_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //str1 is the text entered by the user in the TextBlock
                string str1 = textBlock.Text;
                str1 = str1.Trim();

                if (str1.StartsWith("<") && str1.EndsWith(">"))
                {
                    XmlDocument doc = new XmlDocument();
                    //Load the XML contained in the string
                    doc.LoadXml(str1);

                    StringWriter str2 = new StringWriter();
                    XmlTextWriter doc2 = new XmlTextWriter(str2);
                    doc.WriteTo(doc2);
                    string xml = str2.ToString();
                    string xml2 = Regex.Replace(xml, @"\s", "");
                    textBlock.Text = xml2;
                }
                else
                    MessageBox.Show(string.Format("You need to enter a xml text."));                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
            
        }

        /// <summary>
        /// Convert xml to json
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Convert_to_json_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string str = textBlock.Text;
                str = str.Trim();

                //If it is xml
                if (str.StartsWith("<") && str.EndsWith(">"))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(str);
                    //Convert to json
                    string json = JsonConvert.SerializeXmlNode(doc);
                    textBlock.Text = json;
                }
                else
                    MessageBox.Show(string.Format("You need to enter a xml text."));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }

        }        
    }
}
