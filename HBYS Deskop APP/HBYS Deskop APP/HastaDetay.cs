using HBYS_Deskop_APP.BilgiSınıflari;
using Newtonsoft.Json;
using Timer = System.Windows.Forms.Timer;

namespace HBYS_Deskop_APP
{
    public partial class HastaDetay : Form
    {
        private Timer timer;
        private Doctor _doctor;
        private Hasta _hasta;
        private string _tckno;

        public HastaDetay(string tckno,Doctor doctor)
        {
            InitializeComponent();
            _tckno = tckno;
            _doctor = doctor;
        }

        private async void HastaDetay_Load(object sender, EventArgs e)
        {
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();

            using (HttpClient client = new HttpClient())
            {
                string uri = $"https://localhost:7100/api/hasta/hastaTC?tckno={_tckno}";

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _doctor.AccessToken);

                var response = await client.GetAsync(uri);

                if(response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Hasta hasta = JsonConvert.DeserializeObject<Hasta>(responseBody);
                    _hasta = hasta;
                    Yerlestir(_hasta);
                }
            }
        }

        private void Yerlestir(Hasta hast)
        {
            textBox2.Text = _hasta.tckno;
            textBox3.Text = $"{_hasta.isim} {_hasta.soyisim}";
            textBox4.Text = _hasta.cinsiyet;
            textBox5.Text = _hasta.tel;
            textBox6.Text = _hasta.gecirilen_hastaliklar;
            textBox7.Text = _hasta.kullanilan_ilaclar;
            textBox8.Text = _hasta.d_tarihi;
            textBox9.Text = _hasta.d_yeri;
            textBox10.Text = _hasta.sigorta;
            richTextBox1.Text = _hasta.adres;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime d = DateTime.Now;
            textBox1.Text = d.ToString();
        }

    }
}
