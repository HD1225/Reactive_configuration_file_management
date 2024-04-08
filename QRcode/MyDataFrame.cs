namespace QRcode
{
    public class MyDataFrame
    {
        //public xml xml { get; set; }
        public KIT_BCH KIT_BCH { get; set; }
    }


    public class xml
    {
        public string version { get; set; }
        public string encoding { get; set; }
    }
    public class KIT_BCH
    {
        //public string xsi { get; set; }
        public PRODUCT_INFO PRODUCT_INFO { get; set; }
        public CM_IMMUNO_INFO CM_IMMUNO_INFO { get; set; }
    }

    public class PRODUCT_INFO
    {
        public string Key_Code_Barre { get; set; }
        public string Lot_Number { get; set; }
        public string Lot_Expiry_Date { get; set; }
        public string Type_De_Produit { get; set; }
        public string OnBoardStability { get; set; }
        public string OnBoardStabilityType { get; set; }
        public string Stability { get; set; }
        public string StabilityType { get; set; }
        public string LagTime { get; set; }
        public string MixingTime { get; set; }
        public string APPLICATIONS_RESTRICTION { get; set; }
        public CASSETTE_IMMUNO CASSETTE_IMMUNO { get; set; }
        public ALLOWED_APP[] ALLOWED_APP { get; set; }

    }

    public class CASSETTE_IMMUNO
    {
        public string CM_ID { get; set; }
        public string Mean_Min { get; set; }
        public string Mean_Max { get; set; }
        public string SD_Max { get; set; }
        public string Name { get; set; }
        public string K { get; set; }
        public string Pretreatment { get; set; }
        public string Division_Stabilite { get; set; }
        public POSITION_CASSETTE[] POSITION_CASSETTE { get; set; }
    }

    public class POSITION_CASSETTE
    {
        public string Position { get; set; }
        public string TypeProduitCassette { get; set; }
        public REAGENT_CASSETTE REAGENT_CASSETTE { get; set; }
    }

    public class REAGENT_CASSETTE
    {
        public string Contenant { get; set; }
        public string PID { get; set; }
        public string Volume_Initial { get; set; }
        public string Volume_Ideal { get; set; }
    }

    public class ALLOWED_APP
    {
        public string AppId { get; set; }
    }

    public class CM_IMMUNO_INFO
    {
        public string C_Val { get; set; }
        public string CM_ID { get; set; }
        public string d_Val { get; set; }
        public string NSB_Val { get; set; }
        public string YMax_Val { get; set; }
        public string APPLICATIONS_RESTRICTION { get; set; }
        public ALLOWED_APP[] ALLOWED_APP { get; set; }
    }
}
