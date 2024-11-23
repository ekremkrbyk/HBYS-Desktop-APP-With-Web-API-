using HBYS_Deskop_APP.BilgiS�n�flari;
using Newtonsoft.Json;
using Timer = System.Windows.Forms.Timer;

namespace HBYS_Deskop_APP
{
    public partial class Giris : Form
    {
        private Timer timer;

        public Giris()
        {
            InitializeComponent();
        }

        private void Giris_Load(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.Transparent;
            timer = new Timer(); //Timer objesi olu�turuyoruz.
            timer.Interval = 1000; //1 saniye artt�r�yor. (1 saniye 1000 milisaniye.
            timer.Tick += Timer_Tick; //Tik artt�k�a Timer_Tick �al��acak.
            timer.Start(); //Form1 olu�turuldu�u gibi timer �al���yor.
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;
            if (textBox1 is null || textBox1.Text == "" || textBox2 is null
                || textBox2.Text == "")
            {
                pictureBox1.Visible = false;
                MessageBox.Show("Kullan�c� ad� veya �ifre bo� olamaz");
            }

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string Uri = "https://localhost:7100/api/authentication/login/doctor";

                    var girisBilgi = new DoktorGiris
                    {
                        UserName = textBox1.Text,
                        Password = textBox2.Text,
                    };

                    string JsonData = JsonConvert.SerializeObject(girisBilgi);
                    StringContent content = new StringContent(JsonData, System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(Uri, content);

                    string responseBody = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        Doctor doctor = new Doctor();
                        doctor.userName = textBox1.Text;
                        Doctor doktor = JsonConvert.DeserializeObject<Doctor>(responseBody);
                        if (doktor != null)
                        {
                            doctor.AccessToken = doktor.AccessToken;
                            doctor.RefleshToken = doktor.RefleshToken;
                        }
                        else
                            MessageBox.Show("Ge�ersiz yan�t al�nd�.");
                        pictureBox1.Visible = false;
                        AnaSayfa anaSayfa = new AnaSayfa(doctor, this);
                        anaSayfa.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("L�tfen tekrar giri� yap�n�z");
                        pictureBox1.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    pictureBox1.Visible = false;
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime d = DateTime.Now;
            textBox3.Text = d.ToString();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
