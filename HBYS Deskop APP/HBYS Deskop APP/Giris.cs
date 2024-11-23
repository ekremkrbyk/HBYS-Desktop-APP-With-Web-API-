using HBYS_Deskop_APP.BilgiSýnýflari;
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
            timer = new Timer(); //Timer objesi oluþturuyoruz.
            timer.Interval = 1000; //1 saniye arttýrýyor. (1 saniye 1000 milisaniye.
            timer.Tick += Timer_Tick; //Tik arttýkça Timer_Tick çalýþacak.
            timer.Start(); //Form1 oluþturulduðu gibi timer çalýþýyor.
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;
            if (textBox1 is null || textBox1.Text == "" || textBox2 is null
                || textBox2.Text == "")
            {
                pictureBox1.Visible = false;
                MessageBox.Show("Kullanýcý adý veya þifre boþ olamaz");
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
                            MessageBox.Show("Geçersiz yanýt alýndý.");
                        pictureBox1.Visible = false;
                        AnaSayfa anaSayfa = new AnaSayfa(doctor, this);
                        anaSayfa.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Lütfen tekrar giriþ yapýnýz");
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
