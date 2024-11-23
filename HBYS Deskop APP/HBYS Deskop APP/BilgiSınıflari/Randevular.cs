namespace HBYS_Deskop_APP.BilgiSınıflari
{
    public record class Randevular
    {
        public int id { get; set; }
        public string tckno { get; set; }
        public string r_Isim { get; set; }
        public string r_Soyisim { get; set; }
        public string randevu_Tarihi { get; set; }
        public string polikinlik { get; set; }
        public string doktor { get; set; }
        public string sikayet { get; set; }
        public string saat { get; set; }
        public string onay_Durumu { get; set; }
    }
}
