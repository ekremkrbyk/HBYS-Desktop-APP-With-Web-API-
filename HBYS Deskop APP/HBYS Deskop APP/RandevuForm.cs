using HBYS_Deskop_APP.BilgiSınıflari;
using Newtonsoft.Json;
using Timer = System.Windows.Forms.Timer;

namespace HBYS_Deskop_APP
{
    public partial class RandevuForm : Form
    {
        private Timer timer;
        private Doctor _doctor;
        private AnaSayfa _anaSayfa;
        private Giris _giris;

        public RandevuForm(Doctor doctor, AnaSayfa anaSayfa, Giris giris)
        {
            InitializeComponent();
            _doctor = doctor;
            _anaSayfa = anaSayfa;
            _giris = giris;
            dataGridView1.MultiSelect = false;
        }

        private async void RandevuForm_Load(object sender, EventArgs e)
        {
            timer = new Timer(); //Timer objesi oluşturuyoruz.
            timer.Interval = 1000; //1 saniye arttırıyor. (1 saniye 1000 milisaniye.
            timer.Tick += Timer_Tick; //Tik arttıkça Timer_Tick çalışacak.
            timer.Start(); //Form1 oluşturulduğu gibi timer çalışıyor.
            textBox1.Text = $"{_doctor.d_Isim} {_doctor.d_Soyisim}";
            using (HttpClient client = new HttpClient())
            {
                string uri = "https://localhost:7100/api/randevu/randevular";

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _doctor.AccessToken);

                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    List<Randevular> randevular = new List<Randevular>();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    IEnumerable<Randevular> randevuList = JsonConvert.DeserializeObject<IEnumerable<Randevular>>(responseBody);
                    randevuList.ToList();
                    List<Randevular> nowRandevous = new List<Randevular>();
                    foreach (var randev in randevuList)
                    {
                        if (randev.randevu_Tarihi == DateTime.Now.ToString("dd-M-yyyy"))
                            nowRandevous.Add(randev);
                    }
                    Yerlestir(nowRandevous);
                }
            }

        }

        private void button10_Click(object sender, EventArgs e)
        {//Hasta detay
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string tckno = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                HastaDetay hastaDetay = new HastaDetay(tckno, _doctor);
                hastaDetay.Show();
            }
            else
                MessageBox.Show("Lütfen hasta seçiniz");
        }

        private void Yerlestir(IEnumerable<Randevular> randevous)
        {
            if (!dataGridView1.Columns.Contains("HastaAdSoyad"))
            {
                string doc = $"{_doctor.d_Isim} {_doctor.d_Soyisim}";
           
                dataGridView1.DataSource = randevous;
                dataGridView1.Columns["id"].Visible = false;
                DataGridViewTextBoxColumn adSoyad = new DataGridViewTextBoxColumn();
                adSoyad.Name = "HastaAdSoyad";
                adSoyad.HeaderText = "Hasta Adı Soyadı";
                dataGridView1.Columns.Add(adSoyad);
                adSoyad.DisplayIndex = 2;
                foreach (DataGridViewRow satir in dataGridView1.Rows)
                {
                    string ad = satir.Cells["r_Isim"].Value?.ToString();
                    string soyad = satir.Cells["r_Soyisim"].Value?.ToString();

                    satir.Cells["HastaAdSoyad"].Value = $"{ad} {soyad}";
                }
                dataGridView1.Columns["r_Isim"].Visible = false;
                dataGridView1.Columns["r_Soyisim"].Visible = false;
                dataGridView1.Columns["polikinlik"].Visible = false;
                dataGridView1.Columns["doktor"].Visible = false;
                dataGridView1.Columns["tckno"].HeaderText = "Hasta TCKNO";
                dataGridView1.Columns["randevu_Tarihi"].HeaderText = "Randevu Tarihi";
                dataGridView1.Columns["sikayet"].HeaderText = "Hasta Şikayeti";
                dataGridView1.Columns["saat"].HeaderText = "Randevu Saati";
                dataGridView1.Columns["onay_Durumu"].HeaderText = "Randevu Onay Durumu";
            }
        }

        private void rANDEVULARToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AnaSayfa anaSayfa = new AnaSayfa(_doctor, _giris);
            anaSayfa.Show();
        }

        private void çIKIŞYAPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult exit = MessageBox.Show($"{_doctor.d_Isim} {_doctor.d_Soyisim} Çıkış yapmak istediğinize emin misiniz ?", "Çıkış Yap", MessageBoxButtons.YesNo);
            if (exit == DialogResult.Yes)
            {
                Giris giris = new Giris();
                giris.Show();
                Application.Exit();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {//Randevu onayla
            if (dataGridView1.SelectedRows.Count > 0)
            {//Onay
                string id = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();

                using (HttpClient client = new HttpClient())
                {
                    
                    string uri = $"https://localhost:7100/api/randevu/randevuSil?id={id}";

                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _doctor.AccessToken);

                    var response = client.DeleteAsync(uri).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        dataGridView1.CurrentRow.Cells[9].Value = "Onaylandı";
                        MessageBox.Show("Randevu Onaylandı !");
                    }
                    else
                    {
                        MessageBox.Show("Daha sonra tekrar deneyinzi !");
                    }
                }
            }
            else
                MessageBox.Show("Lütfen hasta seçiniz");
        }

        private async void button9_Click(object sender, EventArgs e)
        {//Yenile
            using (HttpClient client = new HttpClient())
            {
                string uri = "https://localhost:7100/api/randevu/randevular";

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _doctor.AccessToken);

                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    List<Randevular> randevular = new List<Randevular>();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    IEnumerable<Randevular> randevuList = JsonConvert.DeserializeObject<IEnumerable<Randevular>>(responseBody);
                    randevuList.ToList();
                    List<Randevular> nowRandevous = new List<Randevular>();
                    foreach (var randev in randevuList)
                    {
                        if (randev.randevu_Tarihi == DateTime.Now.ToString("dd-M-yyyy"))
                            nowRandevous.Add(randev);
                    }
                    Yerlestir(nowRandevous);
                    MessageBox.Show($"{_doctor.d_Isim} {_doctor.d_Soyisim} güncel randevularınız getirildi !");
                }
            }
            dataGridView1.Refresh();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime d = DateTime.Now;
            textBox7.Text = d.ToString();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            _anaSayfa.WindowState = FormWindowState.Normal;
        }
    }
}
