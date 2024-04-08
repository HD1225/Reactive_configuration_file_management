using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace QRcode
{
    /// <summary>
    /// Interactive logic of Cartridge.xaml
    /// </summary>
    public partial class Cartridge : Window
    {
        //ObservableCollection<Appli> Applis = new ObservableCollection<Appli>();

        public Cartridge(MyDataFrame data)
        {
            InitializeComponent();
            //Assign value to all information
            setValues(data);
        }

        public class Application
        {
            public string applications { get; set; }
        }

        public class PositionCassette
        {
            public string position { get; set; }
            public string data { get; set; }
        }

        /// <summary>
        /// Assign value to all information
        /// </summary>
        /// <param name="data">Information parsed from the QR code</param>
        private void setValues(MyDataFrame data)
        {
            //col:1
            TB_KeyCodeBarre.Text = data.KIT_BCH.PRODUCT_INFO.Key_Code_Barre;
            TB_LotNumber.Text = data.KIT_BCH.PRODUCT_INFO.Lot_Number;
            TB_MeanMin.Text = data.KIT_BCH.PRODUCT_INFO.CASSETTE_IMMUNO.Mean_Min;
            TB_MeanMax.Text = data.KIT_BCH.PRODUCT_INFO.CASSETTE_IMMUNO.Mean_Max;

            //col:2
            TB_LotExpiryDate.Text = data.KIT_BCH.PRODUCT_INFO.Lot_Expiry_Date;
            TB_CMID.Text = data.KIT_BCH.PRODUCT_INFO.CASSETTE_IMMUNO.CM_ID;
            TB_SDMax.Text = data.KIT_BCH.PRODUCT_INFO.CASSETTE_IMMUNO.SD_Max;
            TB_Name.Text = data.KIT_BCH.PRODUCT_INFO.CASSETTE_IMMUNO.Name;
            TB_TypeDeProduit.Text = data.KIT_BCH.PRODUCT_INFO.Type_De_Produit;

            //col:3
            TB_K.Text = data.KIT_BCH.PRODUCT_INFO.CASSETTE_IMMUNO.K;
            TB_OnBoardStability.Text = data.KIT_BCH.PRODUCT_INFO.OnBoardStability;
            TB_OnBoardStabilityType.Text = data.KIT_BCH.PRODUCT_INFO.OnBoardStabilityType;
            TB_LagTime.Text = data.KIT_BCH.PRODUCT_INFO.LagTime;
            TB_MixingTime.Text = data.KIT_BCH.PRODUCT_INFO.MixingTime;
        
            //col:4
            TB_Pretreatment.Text = data.KIT_BCH.PRODUCT_INFO.CASSETTE_IMMUNO.Pretreatment;
            TB_Stability.Text = data.KIT_BCH.PRODUCT_INFO.Stability;
            TB_StabilityType.Text = data.KIT_BCH.PRODUCT_INFO.StabilityType;
            TB_DivisionStabilite.Text = data.KIT_BCH.PRODUCT_INFO.CASSETTE_IMMUNO.Division_Stabilite;
            TB_ApplicationsRestriction.Text = data.KIT_BCH.PRODUCT_INFO.APPLICATIONS_RESTRICTION;

            //listView1
            IList<PositionCassette> list1 = new ObservableCollection<PositionCassette>()
                {
                    new PositionCassette(){position = data.KIT_BCH.PRODUCT_INFO.CASSETTE_IMMUNO.POSITION_CASSETTE[0].Position, data = "T=" + data.KIT_BCH.PRODUCT_INFO.CASSETTE_IMMUNO.POSITION_CASSETTE[0].TypeProduitCassette + ";" + "PID=" + data.KIT_BCH.PRODUCT_INFO.CASSETTE_IMMUNO.POSITION_CASSETTE[0].REAGENT_CASSETTE.PID + ";" + "C=" + data.KIT_BCH.PRODUCT_INFO.CASSETTE_IMMUNO.POSITION_CASSETTE[0].REAGENT_CASSETTE.Contenant},
                    new PositionCassette(){position = data.KIT_BCH.PRODUCT_INFO.CASSETTE_IMMUNO.POSITION_CASSETTE[1].Position, data = "T=" + data.KIT_BCH.PRODUCT_INFO.CASSETTE_IMMUNO.POSITION_CASSETTE[1].TypeProduitCassette + ";" + "PID=" + data.KIT_BCH.PRODUCT_INFO.CASSETTE_IMMUNO.POSITION_CASSETTE[1].REAGENT_CASSETTE.PID + ";" + "C=" + data.KIT_BCH.PRODUCT_INFO.CASSETTE_IMMUNO.POSITION_CASSETTE[1].REAGENT_CASSETTE.Contenant},
                    new PositionCassette(){position = data.KIT_BCH.PRODUCT_INFO.CASSETTE_IMMUNO.POSITION_CASSETTE[2].Position, data = "T=" + data.KIT_BCH.PRODUCT_INFO.CASSETTE_IMMUNO.POSITION_CASSETTE[2].TypeProduitCassette + ";" + "PID=" + data.KIT_BCH.PRODUCT_INFO.CASSETTE_IMMUNO.POSITION_CASSETTE[2].REAGENT_CASSETTE.PID + ";" + "C=" + data.KIT_BCH.PRODUCT_INFO.CASSETTE_IMMUNO.POSITION_CASSETTE[2].REAGENT_CASSETTE.Contenant}
                };
            this.ListView1.ItemsSource = list1;           

            //ListView2         
            IList<Application> list2 = new ObservableCollection<Application>()
                {
                    new Application(){applications = data.KIT_BCH.CM_IMMUNO_INFO.ALLOWED_APP[0].AppId},
                    new Application(){applications = data.KIT_BCH.CM_IMMUNO_INFO.ALLOWED_APP[1].AppId},
                    new Application(){applications = data.KIT_BCH.CM_IMMUNO_INFO.ALLOWED_APP[2].AppId}
                };
            this.ListView2.ItemsSource = list2;
        }

        /// <summary>
        /// Add allowed application ID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddAppid_Click(object sender, RoutedEventArgs e)
        {          
            try
            {
                //The app id is the content entered by the user in the text box
                new Application() { applications = TB_AppID.Text };
                MessageBox.Show("Added data successfully！");
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        /// <summary>
        /// Delete the selected app id
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteAppid_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (ListView2.SelectedItem != null)
                    ListView2.Items.Remove(ListView2.SelectedItems[0]);
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }
    }
}


