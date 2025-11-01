using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace UI_NaviGO
{
    public partial class UserPenumpang : Form
    {
        private Panel sidebarPanel;
        private Panel topPanel;

        public UserPenumpang()
        {
            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text = "NaviGo - Detail Penumpang";
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // ===== SIDEBAR =====
            sidebarPanel = new Panel()
            {
                BackColor = Color.FromArgb(225, 240, 240),
                Width = 250,
                Dock = DockStyle.Left
            };

            Panel logoPanel = new Panel()
            {
                Height = 120,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(225, 240, 240)
            };

            PictureBox logo = new PictureBox()
            {
                Size = new Size(60, 60),
                Location = new Point(20, 25),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = Properties.Resources.logo_navigo
            };

            Label lblLogoTitle = new Label()
            {
                Text = "NaviGO",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 85, 92),
                Location = new Point(95, 35),
                AutoSize = true
            };

            Label lblLogoSubtitle = new Label()
            {
                Text = "Ship Easily",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(0, 85, 92),
                Location = new Point(97, 60),
                AutoSize = true
            };

            logoPanel.Controls.AddRange(new Control[] { logo, lblLogoTitle, lblLogoSubtitle });

            // 🔹 tombol jadwal
            Button btnJadwal = new Button()
            {
                Text = "  Jadwal dan Rute     >",
                BackColor = Color.FromArgb(200, 230, 225),
                Dock = DockStyle.Top,
                Height = 45,
                ForeColor = Color.FromArgb(0, 85, 92),
                TextAlign = ContentAlignment.MiddleLeft,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                Padding = new Padding(20, 0, 0, 0)
            };
            btnJadwal.FlatAppearance.BorderSize = 0;
            btnJadwal.Click += (s, e) =>
            {
                this.Hide();
                new UserDashboard().Show();
            };

            // 🔹 tombol riwayat
            Button btnRiwayat = new Button()
            {
                Text = "Riwayat Pemesanan",
                Dock = DockStyle.Top,
                Height = 45,
                ForeColor = Color.FromArgb(0, 85, 92),
                TextAlign = ContentAlignment.MiddleLeft,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                Padding = new Padding(20, 0, 0, 0),
                BackColor = Color.White
            };
            btnRiwayat.FlatAppearance.BorderSize = 0;

            // 👉 arahkan ke UserHistory
            btnRiwayat.Click += (s, e) =>
            {
                this.Hide();
                new UserHistory().Show();
            };

            sidebarPanel.Controls.AddRange(new Control[] { btnRiwayat, btnJadwal, logoPanel });

            // ===== HEADER =====
            topPanel = new Panel()
            {
                BackColor = Color.Teal,
                Height = 70,
                Dock = DockStyle.Top
            };

            Label lblHeaderTitle = new Label()
            {
                Text = "Detail Penumpang",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(15, 20)
            };

            Label lblUsername = new Label()
            {
                Text = "Halo, Felicia Angel",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11),
                AutoSize = true
            };

            Button btnProfile = new Button()
            {
                Text = "Profile",
                BackColor = Color.White,
                Width = 90,
                Height = 35,
                Font = new Font("Segoe UI", 9),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnProfile.FlatAppearance.BorderSize = 0;
            btnProfile.Click += (s, e) =>
            {
                FormProfileUser profileForm = new FormProfileUser();
                profileForm.Closed += (s2, e2) => this.Close();
                profileForm.Show();
                this.Hide();
            };

            Button btnLogout = new Button()
            {
                Text = "Logout",
                BackColor = Color.FromArgb(210, 80, 60),
                Width = 90,
                Height = 35,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9),
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += (s, e) =>
            {
                this.Hide();
                new UserLogin().Show();
            };

            topPanel.Resize += (s, e) =>
            {
                btnLogout.Location = new Point(topPanel.Width - 110, 18);
                btnProfile.Location = new Point(topPanel.Width - 210, 18);
                lblUsername.Location = new Point(topPanel.Width - 350, 22);
            };

            topPanel.Controls.AddRange(new Control[] { lblHeaderTitle, lblUsername, btnProfile, btnLogout });

            // ===== MAIN CONTENT =====
            Panel mainPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                BackgroundImageLayout = ImageLayout.Stretch
            };
            mainPanel.BackgroundImage = SetImageOpacity(Properties.Resources.penumpang_bg, 0.3f);

            Panel card = new Panel()
            {
                Size = new Size(850, 330),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            mainPanel.Controls.Add(card);

            Label lblDetail = new Label()
            {
                Text = "Detail Penumpang",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(300, 20)
            };
            card.Controls.Add(lblDetail);

            ComboBox cmbKategori = new ComboBox()
            {
                Location = new Point(60, 120),
                Size = new Size(180, 32),
                Font = new Font("Segoe UI", 10),
                Text = "Pilih Kategori"
            };
            cmbKategori.Items.AddRange(new string[] { "Dewasa", "Anak-anak", "Lansia" });
            card.Controls.Add(cmbKategori);

            TextBox txtNama = new TextBox()
            {
                Text = "Nama Penumpang 1",
                Location = new Point(280, 120),
                Size = new Size(200, 32),
                Font = new Font("Segoe UI", 10)
            };
            card.Controls.Add(txtNama);

            TextBox txtNIK = new TextBox()
            {
                Text = "Masukkan NIK",
                ForeColor = Color.Gray,
                Location = new Point(520, 120),
                Size = new Size(250, 32),
                Font = new Font("Segoe UI", 10)
            };
            txtNIK.Enter += (s, e) => { if (txtNIK.Text == "Masukkan NIK") { txtNIK.Text = ""; txtNIK.ForeColor = Color.Black; } };
            txtNIK.Leave += (s, e) => { if (string.IsNullOrWhiteSpace(txtNIK.Text)) { txtNIK.Text = "Masukkan NIK"; txtNIK.ForeColor = Color.Gray; } };
            card.Controls.Add(txtNIK);

            Button btnNext = new Button()
            {
                Text = "Next",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Size = new Size(250, 45),
                Location = new Point(300, 220),
                BackColor = Color.FromArgb(250, 180, 150),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(0, 85, 92)
            };
            btnNext.Click += (s, e) =>
            {
                this.Hide();
                new UserPembayaran().Show();
            };
            card.Controls.Add(btnNext);

            this.Controls.AddRange(new Control[] { mainPanel, topPanel, sidebarPanel });

            mainPanel.Resize += (s, e) =>
            {
                card.Location = new Point(
                    (mainPanel.Width - card.Width) / 2,
                    (mainPanel.Height - card.Height) / 2
                );
            };
        }

        private Image SetImageOpacity(Image image, float opacity)
        {
            Bitmap bmp = new Bitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                ColorMatrix matrix = new ColorMatrix();
                matrix.Matrix33 = opacity;
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                g.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height),
                    0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
            }
            return bmp;
        }
    }
}
