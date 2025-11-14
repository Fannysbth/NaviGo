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

        // Data dummy untuk autofill (simulasi pilihan user)
        private string[,] jadwalData = {
            {"Jakarta", "Batam", "KM Express", "Ekonomi", "Langsung", "15/09/2024", "08", "00"},
            {"Surabaya", "Bali", "KM Sejahtera", "Bisnis", "Transit 1x", "16/09/2024", "09", "30"},
            {"Semarang", "Makassar", "KM Bahari", "VIP", "Transit 2x", "17/09/2024", "07", "15"},
            {"Bali", "Lombok", "KM Pelangi", "Ekonomi", "Langsung", "18/09/2024", "10", "00"},
            {"Jakarta", "Surabaya", "KM Victory", "Bisnis", "Transit 1x", "19/09/2024", "06", "45"}
        };

        public UserDashboard()
        {
            InitializeComponent();
            BuildUI();
            SetupEvents();
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
            txtTanggal = new TextBox() { Width = 250, Location = new Point(120, 295), Font = new Font("Segoe UI", 10) };
            SetupTextBoxPlaceholder(txtTanggal, "DD/MM/YYYY");

            lblWaktu = new Label() { Text = "Waktu", Location = new Point(450, 270), Font = new Font("Segoe UI", 10) };
            txtJam = new TextBox() { Width = 60, Location = new Point(450, 295), Font = new Font("Segoe UI", 10) };
            SetupTextBoxPlaceholder(txtJam, "HH");

            lblSeparator = new Label() { Text = ":", Location = new Point(515, 298), Font = new Font("Segoe UI", 12, FontStyle.Bold) };
            txtMenit = new TextBox() { Width = 60, Location = new Point(535, 295), Font = new Font("Segoe UI", 10) };
            SetupTextBoxPlaceholder(txtMenit, "MM");

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

        private void SetupEvents()
        {
            // Event untuk placeholder text
            SetupPlaceholderEvents();

            // Event tombol sidebar
            btnJadwal.Click += (s, e) =>
            {
                this.Hide();
                UserJadwal jadwalForm = new UserJadwal();
                jadwalForm.FormClosed += (s2, e2) => this.Show();
                jadwalForm.Show();
            };

            btnRiwayat.Click += (s, e) =>
            {
                this.Hide();
                UserHistory historyForm = new UserHistory();
                historyForm.FormClosed += (s2, e2) => this.Show();
                historyForm.Show();
            };

            // Event tombol top panel
            btnProfile.Click += (s, e) =>
            {
                this.Hide();
                FormProfileUser profileForm = new FormProfileUser();
                profileForm.FormClosed += (s2, e2) => this.Show();
                profileForm.Show();
            };

            btnLogout.Click += (s, e) =>
            {
                if (MessageBox.Show("Yakin ingin logout?", "Konfirmasi",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Hide();
                    new UserLogin().Show();
                }
            };

            // Event tombol next
            btnNext.Click += (s, e) =>
            {
                if (ValidateForm())
                {
                    // Simpan data ke TiketManager
                    TiketManager.TiketDipilih = new Tiket
                    {
                        ID = "KJ01",
                        NamaKapal = txtNamaKapal.Text,
                        Rute = $"{txtDari.Text} - {txtKe.Text}",
                        Harga = CalculateHarga(cbKelas.Text, cbTransit.Text),
                        TanggalBerangkat = ParseTanggal(txtTanggal.Text, txtJam.Text, txtMenit.Text)
                    };

                    this.Hide();
                    new UserPenumpang().Show();
                }
            };

            // Event arrow icon untuk swap dari-ke
            arrowIcon.Click += (s, e) =>
            {
                string temp = txtDari.Text;
                txtDari.Text = txtKe.Text;
                txtKe.Text = temp;
            };

            // Auto-fill berdasarkan pilihan
            txtDari.TextChanged += (s, e) => AutoFillData();
            txtKe.TextChanged += (s, e) => AutoFillData();
        }

        private void SetupPlaceholderEvents()
        {
            // Tanggal
            txtTanggal.Enter += (s, e) => { if (txtTanggal.Text == "DD/MM/YYYY") { txtTanggal.Text = ""; txtTanggal.ForeColor = Color.Black; } };
            txtTanggal.Leave += (s, e) => { if (txtTanggal.Text == "") { txtTanggal.Text = "DD/MM/YYYY"; txtTanggal.ForeColor = Color.Gray; } };

            // Jam
            txtJam.Enter += (s, e) => { if (txtJam.Text == "HH") { txtJam.Text = ""; txtJam.ForeColor = Color.Black; } };
            txtJam.Leave += (s, e) => { if (txtJam.Text == "") { txtJam.Text = "HH"; txtJam.ForeColor = Color.Gray; } };

            // Menit
            txtMenit.Enter += (s, e) => { if (txtMenit.Text == "MM") { txtMenit.Text = ""; txtMenit.ForeColor = Color.Black; } };
            txtMenit.Leave += (s, e) => { if (txtMenit.Text == "") { txtMenit.Text = "MM"; txtMenit.ForeColor = Color.Gray; } };
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtDari.Text) || txtDari.Text == "DD/MM/YYYY")
            {
                MessageBox.Show("Harap isi kota asal", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtKe.Text) || txtKe.Text == "DD/MM/YYYY")
            {
                MessageBox.Show("Harap isi kota tujuan", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNamaKapal.Text))
            {
                MessageBox.Show("Harap isi nama kapal", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cbKelas.SelectedIndex == -1)
            {
                MessageBox.Show("Harap pilih kelas kapal", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (txtTanggal.Text == "DD/MM/YYYY" || !IsValidDate(txtTanggal.Text))
            {
                MessageBox.Show("Format tanggal tidak valid. Gunakan format DD/MM/YYYY", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private bool IsValidDate(string date)
        {
            try
            {
                DateTime.ParseExact(date, "dd/MM/yyyy", null);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private DateTime ParseTanggal(string tanggal, string jam, string menit)
        {
            try
            {
                string dateTimeStr = $"{tanggal} {jam.PadLeft(2, '0')}:{menit.PadLeft(2, '0')}";
                return DateTime.ParseExact(dateTimeStr, "dd/MM/yyyy HH:mm", null);
            }
            catch
            {
                return DateTime.Now.AddDays(1);
            }
        }

        private decimal CalculateHarga(string kelas, string transit)
        {
            decimal basePrice = 500000;

            // Tambah berdasarkan kelas
            if (kelas == "Bisnis") basePrice += 300000;
            else if (kelas == "VIP") basePrice += 600000;

            // Kurangi jika transit
            if (transit == "Transit 1x") basePrice -= 50000;
            else if (transit == "Transit 2x") basePrice -= 100000;

            return basePrice;
        }

        private void SetupTextBoxPlaceholder(TextBox textBox, string placeholder)
        {
            textBox.Text = placeholder;
            textBox.ForeColor = Color.Gray;

            textBox.Enter += (s, e) =>
            {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black;
                }
            };

            textBox.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholder;
                    textBox.ForeColor = Color.Gray;
                }
            };
        }

        private void AutoFillData()
        {
            for (int i = 0; i < jadwalData.GetLength(0); i++)
            {
                if (txtDari.Text.Contains(jadwalData[i, 0]) && txtKe.Text.Contains(jadwalData[i, 1]))
                {
                    txtNamaKapal.Text = jadwalData[i, 2];
                    cbKelas.Text = jadwalData[i, 3];
                    cbTransit.Text = jadwalData[i, 4];
                    txtTanggal.Text = jadwalData[i, 5];
                    txtJam.Text = jadwalData[i, 6];
                    txtMenit.Text = jadwalData[i, 7];

                    // Update warna text jika berisi data real
                    if (txtTanggal.Text != "DD/MM/YYYY") txtTanggal.ForeColor = Color.Black;
                    if (txtJam.Text != "HH") txtJam.ForeColor = Color.Black;
                    if (txtMenit.Text != "MM") txtMenit.ForeColor = Color.Black;

                    break;
                }
            }
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