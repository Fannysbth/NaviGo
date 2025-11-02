using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI_NaviGO
{
    // ===============================
    // 🔹 KELAS MODEL OOP
    // ===============================

    // 🧱 Base Class: Encapsulation
    public class RuteKapal
    {
        private string id;
        private string kapal;
        private string rute;
        private DateTime tanggal;
        private decimal harga;

        public string ID { get => id; set => id = value; }
        public string Kapal { get => kapal; set => kapal = value; }
        public string Rute { get => rute; set => rute = value; }
        public DateTime Tanggal { get => tanggal; set => tanggal = value; }
        public decimal Harga
        {
            get => harga;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Harga tidak boleh negatif!");
                harga = value;
            }
        }

        public RuteKapal(string id, string kapal, string rute, DateTime tanggal, decimal harga)
        {
            ID = id;
            Kapal = kapal;
            Rute = rute;
            Tanggal = tanggal;
            Harga = harga;
        }

        // 🔸 Virtual agar bisa di-override oleh subclass (Polymorphism)
        public virtual string GetInfo()
        {
            return $"{ID} - {Kapal} ({Rute}) pada {Tanggal:dd MMM yyyy} - Rp {Harga:N0}";
        }
    }

    // 🧱 Subclass: Inheritance + Polymorphism
    public class RuteVIP : RuteKapal
    {
        public string FasilitasVIP { get; set; }

        public RuteVIP(string id, string kapal, string rute, DateTime tanggal, decimal harga, string fasilitas)
            : base(id, kapal, rute, tanggal, harga)
        {
            FasilitasVIP = fasilitas;
        }

        public override string GetInfo()
        {
            return base.GetInfo() + $" | Fasilitas: {FasilitasVIP}";
        }
    }

    // ===============================
    // 🔹 FORM USERJADWAL
    // ===============================

    public partial class UserJadwal : Form
    {
        private DataGridView dgv;

        public UserJadwal()
        {
            BuildUI();
            this.Opacity = 1;
            this.FormClosed += (s, e) => Application.Exit();

            // 🔸 Panggil method untuk load data (OOP)
            LoadDataRute();
        }

        // 🔹 Method menggunakan OOP (Polymorphism)
        private void LoadDataRute()
        {
            List<RuteKapal> daftarRute = new List<RuteKapal>
            {
                new RuteKapal("KJ02", "Kapal Jetsar", "Jakarta - Batam", new DateTime(2025, 9, 15), 750000),
                new RuteVIP("KK01", "Kapal Kastar", "Surabaya - Bali", new DateTime(2025, 12, 12), 300000, "Snack + Lounge")
            };

            dgv.Rows.Clear();

            foreach (var rute in daftarRute)
            {
                dgv.Rows.Add(
                    rute.ID,
                    rute.Kapal,
                    rute.Rute,
                    rute.Tanggal.ToString("dd MMM yyyy"),
                    "-", "-", "-", $"Rp {rute.Harga:N0}"
                );
                Console.WriteLine(rute.GetInfo());
            }
        }

        // 🔹 Animasi transisi antar form (fade in/out)
        private async void SwitchTo(Form nextForm)
        {
            for (double i = 1.0; i >= 0; i -= 0.1)
            {
                this.Opacity = i;
                await Task.Delay(25);
            }

            nextForm.Show();
            this.Hide();

            for (double i = 0; i <= 1; i += 0.1)
            {
                nextForm.Opacity = i;
                await Task.Delay(25);
            }
        }

        private void BuildUI()
        {
            this.Text = "NaviGo - Ship Easily";
            this.Size = new Size(1280, 720);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.BackColor = Color.White;

            PictureBox bg = new PictureBox();
            bg.Dock = DockStyle.Fill;
            bg.Image = MakeSemiTransparent(Properties.Resources.sunset_picture, 0.5f);
            bg.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(bg);

            Panel sidebar = new Panel();
            sidebar.Dock = DockStyle.Left;
            sidebar.Width = 250;
            sidebar.BackColor = Color.FromArgb(230, 240, 240);
            this.Controls.Add(sidebar);
            sidebar.BringToFront();

            PictureBox logo = new PictureBox();
            logo.Size = new Size(60, 60);
            logo.Location = new Point(20, 25);
            logo.SizeMode = PictureBoxSizeMode.StretchImage;
            logo.Image = Properties.Resources.logo_navigo;
            sidebar.Controls.Add(logo);

            Label lblLogo = new Label();
            lblLogo.Text = "NaviGo\nShip Easily";
            lblLogo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblLogo.ForeColor = Color.FromArgb(0, 90, 100);
            lblLogo.Location = new Point(90, 30);
            lblLogo.AutoSize = true;
            sidebar.Controls.Add(lblLogo);

            Button btnJadwal = CreateSidebarButton("Jadwal  &  Rute", 150);
            btnJadwal.BackColor = Color.FromArgb(200, 230, 225);
            Button btnRiwayat = CreateSidebarButton("Riwayat Pemesanan", 210);
            sidebar.Controls.Add(btnJadwal);
            sidebar.Controls.Add(btnRiwayat);

            btnRiwayat.Click += (s, e) => SwitchTo(new UserHistory());
            btnJadwal.MouseEnter += (s, e) => btnJadwal.BackColor = Color.FromArgb(180, 220, 220);
            btnJadwal.MouseLeave += (s, e) => btnJadwal.BackColor = Color.FromArgb(200, 230, 225);
            btnRiwayat.MouseEnter += (s, e) => btnRiwayat.BackColor = Color.FromArgb(220, 240, 240);
            btnRiwayat.MouseLeave += (s, e) => btnRiwayat.BackColor = Color.White;

            Panel header = new Panel();
            header.Dock = DockStyle.Top;
            header.Height = 65;
            header.BackColor = Color.FromArgb(0, 90, 100);
            this.Controls.Add(header);
            header.BringToFront();

            Label lblTitle = new Label();
            lblTitle.Text = "Jadwal dan Rute";
            lblTitle.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(280, 20);
            header.Controls.Add(lblTitle);

            Button btnLogout = new Button();
            btnLogout.Text = "Logout";
            btnLogout.BackColor = Color.FromArgb(240, 90, 50);
            btnLogout.ForeColor = Color.White;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Size = new Size(70, 30);
            btnLogout.Cursor = Cursors.Hand;
            header.Controls.Add(btnLogout);

            Button btnProfile = new Button();
            btnProfile.Text = "Profile";
            btnProfile.BackColor = Color.White;
            btnProfile.FlatStyle = FlatStyle.Flat;
            btnProfile.FlatAppearance.BorderSize = 0;
            btnProfile.Size = new Size(70, 30);
            btnProfile.Cursor = Cursors.Hand;
            header.Controls.Add(btnProfile);

            Label lblHalo = new Label();
            lblHalo.Text = "Halo, Felicia Angel";
            lblHalo.Font = new Font("Segoe UI", 10);
            lblHalo.ForeColor = Color.White;
            lblHalo.AutoSize = true;
            header.Controls.Add(lblHalo);

            void PositionRightElements()
            {
                int marginRight = 20;
                btnLogout.Location = new Point(header.Width - btnLogout.Width - marginRight, 17);
                btnProfile.Location = new Point(btnLogout.Left - btnProfile.Width - 10, 17);
                lblHalo.Location = new Point(btnProfile.Left - lblHalo.Width - 20, 22);
            }

            PositionRightElements();
            header.Resize += (s, e) => PositionRightElements();

            btnProfile.Click += (s, e) => SwitchTo(new FormProfileUser());
            btnLogout.Click += (s, e) =>
            {
                if (MessageBox.Show("Yakin ingin logout?", "Konfirmasi",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SwitchTo(new UserLogin());
                }
            };

            Panel card = new Panel();
            card.Size = new Size(950, 500);
            card.Location = new Point(280, 120);
            card.BackColor = Color.White;
            card.Padding = new Padding(20);
            this.Controls.Add(card);
            card.BringToFront();

            Label lblDari = new Label();
            lblDari.Text = "Dari";
            lblDari.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblDari.Location = new Point(20, 10);
            card.Controls.Add(lblDari);

            ComboBox cbxDari = new ComboBox();
            cbxDari.Location = new Point(20, 35);
            cbxDari.Width = 200;
            cbxDari.Font = new Font("Segoe UI", 9);
            cbxDari.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxDari.Items.AddRange(new string[] { "Pilih Asal Kota / Pelabuhan", "Jakarta", "Surabaya" });
            cbxDari.SelectedIndex = 0;
            card.Controls.Add(cbxDari);

            Label lblKe = new Label();
            lblKe.Text = "Ke";
            lblKe.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblKe.Location = new Point(280, 10);
            card.Controls.Add(lblKe);

            ComboBox cbxKe = new ComboBox();
            cbxKe.Location = new Point(280, 35);
            cbxKe.Width = 200;
            cbxKe.Font = new Font("Segoe UI", 9);
            cbxKe.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxKe.Items.AddRange(new string[] { "Pilih Tujuan Kota / Pelabuhan", "Batam", "Bali" });
            cbxKe.SelectedIndex = 0;
            card.Controls.Add(cbxKe);

            Label lblTanggal = new Label();
            lblTanggal.Text = "Tanggal Bulan Keberangkatan";
            lblTanggal.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblTanggal.Location = new Point(510, 10);
            card.Controls.Add(lblTanggal);

            DateTimePicker dtpTanggal = new DateTimePicker();
            dtpTanggal.Location = new Point(510, 35);
            dtpTanggal.Width = 250;
            dtpTanggal.Font = new Font("Segoe UI", 9);
            card.Controls.Add(dtpTanggal);

            dgv = new DataGridView();
            dgv.Location = new Point(20, 90);
            dgv.Size = new Size(900, 360);
            dgv.BackgroundColor = Color.White;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            dgv.BorderStyle = BorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.EnableHeadersVisualStyles = false;
            dgv.RowTemplate.Height = 32;
            dgv.GridColor = Color.FromArgb(230, 230, 230);

            dgv.Columns.Add("ID", "ID");
            dgv.Columns.Add("Kapal", "Kapal");
            dgv.Columns.Add("Rute", "Rute");
            dgv.Columns.Add("Tanggal", "Tanggal");
            dgv.Columns.Add("Pergi", "Pergi");
            dgv.Columns.Add("Tiba", "Tiba");
            dgv.Columns.Add("Transit", "Transit");
            dgv.Columns.Add("Harga", "Harga");

            card.Controls.Add(dgv);
        }

        private Button CreateSidebarButton(string text, int top)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Width = 200;
            btn.Height = 40;
            btn.Location = new Point(25, top);
            btn.BackColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.ForeColor = Color.FromArgb(0, 90, 100);
            btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            return btn;
        }

        private Bitmap MakeSemiTransparent(Image original, float opacity)
        {
            Bitmap bmp = new Bitmap(original.Width, original.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                var cm = new ColorMatrix();
                cm.Matrix33 = opacity;
                using (var ia = new ImageAttributes())
                {
                    ia.SetColorMatrix(cm);
                    g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                        0, 0, original.Width, original.Height, GraphicsUnit.Pixel, ia);
                }
            }
            return bmp;
        }
    }
}
