namespace HBYS_Deskop_APP.BilgiSınıflari
{
    public record class Hasta
    {
        public int id { get; set; }
        public string tckno { get; set; }
        public string isim { get; set; }
        public string soyisim { get; set; }
        public string tel { get; set; }
        public string adres { get; set; }
        public string cinsiyet { get; set; }
        public string d_yeri { get; set; }
        public string d_tarihi { get; set; }
        public string sigorta { get; set; }
        public string gecirilen_hastaliklar { get; set; }
        public string kullanilan_ilaclar { get; set; }
    }
}
