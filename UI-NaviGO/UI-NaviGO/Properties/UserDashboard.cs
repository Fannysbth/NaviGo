using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace UI_NaviGO
{
    public partial class UserDashboard : Form
    {
        private Panel sidebarPanel;
        private Button btnJadwal;
        private Button btnRiwayat;
        private Panel topPanel;
        private Label lblUsername;
        private Button btnProfile;
        private Button btnLogout;
        private Panel contentPanelBox;

        private Label lblTitle;
        private Label lblDari;
        private TextBox txtDari;
        private PictureBox arrowIcon;
        private Label lblKe;
        private TextBox txtKe;
        private Label lblNamaKapal;
        private TextBox txtNamaKapal;
        private Label lblKelas;
        private ComboBox cbKelas;
        private Label lblTransit;
        private ComboBox cbTransit;
        private Label lblTanggal;
        private TextBox txtTanggal;
        private Label lblWaktu;
        private TextBox txtJam;
        private Label lblSeparator;
        private TextBox txtMenit;
        private Button btnNext;

        public UserDashboard()
        {
            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            // ===== FORM SETTINGS =====
            this.Text = "NaviGO - Pemesanan Tiket";
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

            // ===== LOGO DAN TEKS NAVIGO =====
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
                BackColor = Color.Transparent,
                Image = Properties.Resources.logo_navigo // pastikan ada di Resources
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

            // ===== BUTTON SIDEBAR =====
            btnJadwal = new Button()
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

            btnRiwayat = new Button()
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

            // 🔹 EVENT KLIK RIWAYAT
            btnRiwayat.Click += (s, e) =>
            {
                this.Hide();
                UserHistory historyForm = new UserHistory();
                historyForm.FormClosed += (s2, e2) => this.Show();
                historyForm.Show();
            };

            sidebarPanel.Controls.AddRange(new Control[] { btnRiwayat, btnJadwal, logoPanel });

            // ===== TOP PANEL =====
            topPanel = new Panel()
            {
                BackColor = Color.Teal,
                Height = 70,
                Dock = DockStyle.Top
            };

            Label lblHeaderTitle = new Label()
            {
                Text = "Pemesanan Tiket",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(15, 20)
            };

            lblUsername = new Label()
            {
                Text = "Halo, Felicia Angel",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11),
                AutoSize = true
            };

            btnProfile = new Button()
            {
                Text = "Profile",
                BackColor = Color.White,
                Width = 90,
                Height = 35,
                Font = new Font("Segoe UI", 9),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnProfile.FlatAppearance.BorderSize = 0;

            // 🔹 EVENT PROFILE
            btnProfile.Click += (s, e) =>
            {
                this.Hide();
                FormProfileUser profileForm = new FormProfileUser();
                profileForm.FormClosed += (s2, e2) => this.Show();
                profileForm.Show();
            };

            btnLogout = new Button()
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

            // ===== CONTENT =====
            Panel contentPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                BackgroundImageLayout = ImageLayout.Stretch,
                BackColor = Color.White
            };

            Image original = Properties.Resources.tiket_bg;
            contentPanel.BackgroundImage = SetImageOpacity(original, 0.3f);

            contentPanelBox = new Panel()
            {
                Size = new Size(900, 480),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            lblTitle = new Label()
            {
                Text = "Detail Tiket Kapal",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(320, 30),
                BackColor = Color.Transparent
            };

            lblDari = new Label() { Text = "Dari", Location = new Point(120, 110), Font = new Font("Segoe UI", 10) };
            txtDari = new TextBox() { Width = 250, Location = new Point(120, 135), Font = new Font("Segoe UI", 10) };

            arrowIcon = new PictureBox()
            {
                Size = new Size(40, 30),
                Location = new Point(390, 135),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent,
                Image = Properties.Resources.swap_icon
            };

            lblKe = new Label() { Text = "Ke", Location = new Point(450, 110), Font = new Font("Segoe UI", 10) };
            txtKe = new TextBox() { Width = 250, Location = new Point(450, 135), Font = new Font("Segoe UI", 10) };

            lblNamaKapal = new Label() { Text = "Nama Kapal", Location = new Point(120, 190), Font = new Font("Segoe UI", 10) };
            txtNamaKapal = new TextBox() { Width = 250, Location = new Point(120, 215), Font = new Font("Segoe UI", 10) };

            lblKelas = new Label() { Text = "Kelas Kapal", Location = new Point(450, 190), Font = new Font("Segoe UI", 10) };
            cbKelas = new ComboBox() { Width = 250, Location = new Point(450, 215), Font = new Font("Segoe UI", 10) };
            cbKelas.Items.AddRange(new string[] { "Ekonomi", "Bisnis", "VIP" });

            lblTransit = new Label() { Text = "Transit", Location = new Point(720, 190), Font = new Font("Segoe UI", 10) };
            cbTransit = new ComboBox() { Width = 130, Location = new Point(720, 215), Font = new Font("Segoe UI", 10) };
            cbTransit.Items.AddRange(new string[] { "Langsung", "Transit 1x", "Transit 2x" });

            lblTanggal = new Label() { Text = "Tanggal", Location = new Point(120, 270), Font = new Font("Segoe UI", 10) };
            txtTanggal = new TextBox() { Width = 250, Location = new Point(120, 295), ForeColor = Color.Gray, Text = "DD/MM/YYYY", Font = new Font("Segoe UI", 10) };
            txtTanggal.Enter += (s, e) => { if (txtTanggal.Text == "DD/MM/YYYY") { txtTanggal.Text = ""; txtTanggal.ForeColor = Color.Black; } };
            txtTanggal.Leave += (s, e) => { if (txtTanggal.Text == "") { txtTanggal.Text = "DD/MM/YYYY"; txtTanggal.ForeColor = Color.Gray; } };

            lblWaktu = new Label() { Text = "Waktu", Location = new Point(450, 270), Font = new Font("Segoe UI", 10) };
            txtJam = new TextBox() { Width = 60, Location = new Point(450, 295), ForeColor = Color.Gray, Text = "HH", Font = new Font("Segoe UI", 10) };
            txtJam.Enter += (s, e) => { if (txtJam.Text == "HH") { txtJam.Text = ""; txtJam.ForeColor = Color.Black; } };
            txtJam.Leave += (s, e) => { if (txtJam.Text == "") { txtJam.Text = "HH"; txtJam.ForeColor = Color.Gray; } };

            lblSeparator = new Label() { Text = ":", Location = new Point(515, 298), Font = new Font("Segoe UI", 12, FontStyle.Bold) };
            txtMenit = new TextBox() { Width = 60, Location = new Point(535, 295), ForeColor = Color.Gray, Text = "MM", Font = new Font("Segoe UI", 10) };
            txtMenit.Enter += (s, e) => { if (txtMenit.Text == "MM") { txtMenit.Text = ""; txtMenit.ForeColor = Color.Black; } };
            txtMenit.Leave += (s, e) => { if (txtMenit.Text == "") { txtMenit.Text = "MM"; txtMenit.ForeColor = Color.Gray; } };

            btnNext = new Button()
            {
                Text = "Next",
                BackColor = Color.PeachPuff,
                Width = 300,
                Height = 50,
                Location = new Point(300, 380),
                ForeColor = ColorTranslator.FromHtml("#33767C"),
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            btnNext.Click += (s, e) =>
            {
                this.Hide();
                new UserPenumpang().Show();
            };

            contentPanelBox.Controls.AddRange(new Control[]
            {
                lblTitle, lblDari, txtDari, arrowIcon, lblKe, txtKe,
                lblNamaKapal, txtNamaKapal, lblKelas, cbKelas,
                lblTransit, cbTransit, lblTanggal, txtTanggal,
                lblWaktu, txtJam, lblSeparator, txtMenit, btnNext
            });

            contentPanel.Controls.Add(contentPanelBox);
            this.Controls.AddRange(new Control[] { contentPanel, topPanel, sidebarPanel });

            contentPanel.Resize += (s, e) =>
            {
                contentPanelBox.Location = new Point(
                    (contentPanel.Width - contentPanelBox.Width) / 2,
                    (contentPanel.Height - contentPanelBox.Height) / 2
                );
            };
        }

        // ===== FUNGSI UNTUK MENGATUR OPACITY GAMBAR =====
        private Image SetImageOpacity(Image image, float opacity)
        {
            Bitmap bmp = new Bitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                ColorMatrix matrix = new ColorMatrix();
                matrix.Matrix33 = opacity;
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                g.DrawImage(image,
                    new Rectangle(0, 0, bmp.Width, bmp.Height),
                    0, 0, image.Width, image.Height,
                    GraphicsUnit.Pixel,
                    attributes);
            }
            return bmp;
        }
    }
}
