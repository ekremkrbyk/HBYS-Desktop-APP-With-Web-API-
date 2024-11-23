using HBYS_Deskop_APP.BilgiSınıflari;
using Newtonsoft.Json;
using Timer = System.Windows.Forms.Timer;

namespace HBYS_Deskop_APP
{
    public partial class AnaSayfa : Form
    {
        private Doctor _doctor;
        private Timer timer;
        private Giris _giris;
        private RandevuForm _form;

        public AnaSayfa(Doctor doctor,Giris giris)
        {
            InitializeComponent();
            _doctor = doctor;
            _giris = giris;
        }

        private async void AnaSayfa_Load(object sender, EventArgs e)
        {
            timer = new Timer(); //Timer objesi oluşturuyoruz.
            timer.Interval = 1000; //1 saniye arttırıyor. (1 saniye 1000 milisaniye.
            timer.Tick += Timer_Tick; //Tik arttıkça Timer_Tick çalışacak.
            timer.Start(); //Form1 oluşturulduğu gibi timer çalışıyor.

            string accessToken = _doctor.AccessToken;
            string refleshToken = _doctor.RefleshToken;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string uri = $"https://localhost:7100/api/doktor/doktorTC?userName={_doctor.userName}";

                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _doctor.AccessToken);

                    var response = await client.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Doctor doctor = JsonConvert.DeserializeObject<Doctor>(responseBody);
                        _doctor = doctor;
                        textBox1.Text = _doctor.d_Isim;
                        textBox2.Text = _doctor.d_Soyisim;
                        textBox3.Text = _doctor.d_Pol;
                        textBox4.Text = _doctor.userName;
                        textBox5.Text = _doctor.E_mail;
                        textBox6.Text = _doctor.Tel_no;
                        _doctor.AccessToken = accessToken;
                        _doctor.RefleshToken = refleshToken;
                    }
                    else
                    {
                        if(_doctor is null)
                            MessageBox.Show("Geçersiz yanıt alındı !");
                        textBox1.Text = _doctor.d_Isim;
                        textBox2.Text = _doctor.d_Soyisim;
                        textBox3.Text = _doctor.d_Pol;
                        textBox4.Text = _doctor.userName;
                        textBox5.Text = _doctor.E_mail;
                        textBox6.Text = _doctor.Tel_no;
                        _doctor.AccessToken = accessToken;
                        _doctor.RefleshToken = refleshToken;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Bir hata oluştu ! {ex.Message}");
                }

            }
        }

        private void çIKIŞYAPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult exit = MessageBox.Show($"{_doctor.d_Isim} {_doctor.d_Soyisim} Çıkış yapmak istediğinize emin misiniz ?", "Çıkış Yap", MessageBoxButtons.YesNo);
            if (exit == DialogResult.Yes)
            {
                _giris.Close();
            }
        }

        private void rANDEVULARToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms != null)
            {
                foreach (Form acikFormlar in Application.OpenForms)
                {
                    if (acikFormlar is RandevuForm)
                    {
                        _form.Show();
                    }
                    else
                    {
                        RandevuForm randevuForm = new RandevuForm(_doctor, this, _giris);
                        _form = randevuForm;
                        randevuForm.Show();
                        this.WindowState = FormWindowState.Minimized;
                        break;
                    }
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime d = DateTime.Now;
            textBox7.Text = d.ToString();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Application.Exit();
        }
    }
}
