namespace HBYS_Deskop_APP.BilgiSınıflari
{
    public record class Doctor
    {
        public string userName { get; set; }
        public string AccessToken { get; set; }
        public string RefleshToken { get; set; }
        public string d_Isim { get; set; }
        public string d_Soyisim { get; set; }
        public string d_Pol { get; set; }
        public string E_mail { get; set; }
        public string Tel_no { get; set; }
    }
}
