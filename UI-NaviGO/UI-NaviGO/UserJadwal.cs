using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI_NaviGO
{
    public partial class UserJadwal : Form
    {
        private List<RuteKapal> daftarRuteAll;
        private Panel sidebar;
        private Panel topHeader;
        private bool userSudahPilihTanggal = false;
        private Panel contentPanel;
        private Panel contentBox;
        private DataGridView dgvRoutes;
        private ComboBox cbxDari;
        private ComboBox cbxKe;
        private DateTimePicker dtpTanggal;
        private UserProfile currentUser;


        public UserJadwal()
        {
            InitializeComponent();
            BuildUI();
            InitializeHardcodeData();
            LoadDataRute();
            SetupEventHandlers();
            InitializeUserData();
        }

        private void InitializeUserData()
        {
            currentUser = new UserProfile
            {
                Nama = "Felicia Angel",
                Email = "felicia.angel@example.com",
                Telepon = "+62 812-3456-7890",
                Password = "password123",
                FotoProfil = null
            };
        }

        private void BuildUI()
        {
            this.Text = "NaviGO - Jadwal&Rute";
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // SIDEBAR
            sidebar = new Panel()
            {
                BackColor = Color.FromArgb(225, 240, 240),
                Width = 250,
                Dock = DockStyle.Left
            };

            Panel logoPanel = new Panel()
            {
                Height = 120,
                Dock = DockStyle.Top
            };

            PictureBox logo = new PictureBox()
            {
                Size = new Size(60, 60),
                Location = new Point(20, 25),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent,
                Image = Properties.Resources.logo_navigo
            };

            Label lblSidebarTitle = new Label()
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

            logoPanel.Controls.AddRange(new Control[] { logo, lblSidebarTitle, lblLogoSubtitle });

            Button btnJadwal = new Button()
            {
                Text = "  Jadwal dan Rute    >",
                Dock = DockStyle.Top,
                Height = 45,
                BackColor = Color.FromArgb(200, 230, 225),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0)
            };

            Button btnRiwayat = new Button()
            {
                Text = "Riwayat Pemesanan",
                Dock = DockStyle.Top,
                Height = 45,
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0)
            };

            btnRiwayat.FlatAppearance.BorderSize = 0;
            btnJadwal.FlatAppearance.BorderSize = 0;

            btnRiwayat.Click += (s, e) =>
            {
                this.Hide();
                new UserHistory().Show();
            };

            sidebar.Controls.AddRange(new Control[]
            {
                btnRiwayat, btnJadwal, logoPanel
            });

            // HEADER
            topHeader = new Panel()
            {
                BackColor = Color.Teal,
                Height = 70,
                Dock = DockStyle.Top
            };

            Label lblHeaderTitle = new Label()
            {
                Text = "Jadwal & Rute Kapal",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(15, 20),
                AutoSize = true
            };

            Label lblUsername = new Label()
            {
                Text = $"Halo, User",
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

            topHeader.Resize += (s, e) =>
            {
                btnLogout.Location = new Point(topHeader.Width - 110, 18);
                btnProfile.Location = new Point(topHeader.Width - 210, 18);
                lblUsername.Location = new Point(topHeader.Width - 350, 22);
            };

            topHeader.Controls.AddRange(new Control[] { lblHeaderTitle, lblUsername, btnProfile, btnLogout });



            // CONTENT
            contentPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                BackgroundImageLayout = ImageLayout.Stretch,
                BackColor = Color.White
            };

            contentPanel.BackgroundImage = SetImageOpacity(Properties.Resources.tiket_bg, 0.3f);

            contentBox = new Panel()
            {
                Size = new Size(900, 480),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            contentPanel.Controls.Add(contentBox);

            contentPanel.Resize += (s, e) =>
            {
                contentBox.Location = new Point(
                    (contentPanel.Width - contentBox.Width) / 2,
                    (contentPanel.Height - contentBox.Height) / 2
                );
            };

            this.Controls.Add(contentPanel);
            this.Controls.Add(topHeader);
            this.Controls.Add(sidebar);

            // FILTERS
            Label lblDari = new Label()
            {
                Text = "Dari",
                Location = new Point(50, 30),
                Font = new Font("Segoe UI", 10)
            };

            cbxDari = new ComboBox()
            {
                Location = new Point(50, 55),
                Width = 250,
                Font = new Font("Segoe UI", 10)
            };
            cbxDari.Items.AddRange(new[] { "Semua", "Jakarta", "Surabaya", "Batam", "Bali" });
            cbxDari.SelectedIndex = 0;

            Label lblKe = new Label()
            {
                Text = "Ke",
                Location = new Point(350, 30),
                Font = new Font("Segoe UI", 10)
            };

            cbxKe = new ComboBox()
            {
                Location = new Point(350, 55),
                Width = 250,
                Font = new Font("Segoe UI", 10)
            };
            cbxKe.Items.AddRange(new[] { "Semua", "Jakarta", "Surabaya", "Batam", "Bali" });
            cbxKe.SelectedIndex = 0;

            Label lblTanggal = new Label()
            {
                Text = "Filter",
                Location = new Point(650, 30),
                Font = new Font("Segoe UI", 10)
            };

            dtpTanggal = new DateTimePicker()
            {
                Location = new Point(650, 55),
                Font = new Font("Segoe UI", 10)
            };

            dtpTanggal.ValueChanged += (s, e) =>
            {
                userSudahPilihTanggal = true;
                FilterData(null, null);
            };

            dgvRoutes = new DataGridView()
            {
                Dock = DockStyle.Bottom,
                Height = 350,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
            };

            dgvRoutes.Columns.Add("ID", "ID");
            dgvRoutes.Columns.Add("Kapal", "Kapal");
            dgvRoutes.Columns.Add("Rute", "Rute");
            dgvRoutes.Columns.Add("Tanggal", "Tanggal");
            dgvRoutes.Columns.Add("Pergi", "Pergi");
            dgvRoutes.Columns.Add("Tiba", "Tiba");
            dgvRoutes.Columns.Add("Transit", "Transit");
            dgvRoutes.Columns.Add("Harga", "Harga");

            DataGridViewButtonColumn pilihCol = new DataGridViewButtonColumn()
            {
                HeaderText = "Aksi",
                Text = "Pilih",
                UseColumnTextForButtonValue = true,
                Name = "colPilih"
            };
            dgvRoutes.Columns.Add(pilihCol);

            contentBox.Controls.AddRange(new Control[]
            {
                lblDari, cbxDari,
                lblKe, cbxKe,
                lblTanggal, dtpTanggal,
                dgvRoutes
            });
        }

        private Image SetImageOpacity(Image image, float opacity)
        {
            Bitmap bmp = new Bitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                ColorMatrix m = new ColorMatrix();
                m.Matrix33 = opacity;

                ImageAttributes ia = new ImageAttributes();
                ia.SetColorMatrix(m, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                g.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height),
                    0, 0, image.Width, image.Height,
                    GraphicsUnit.Pixel, ia);
            }
            return bmp;
        }

        private void InitializeHardcodeData()
        {
            daftarRuteAll = new List<RuteKapal>
            {
                new RuteKapal("KP001","Kapal Pelni","Jakarta - Batam",new DateTime(2025,11,20),"08:00","14:00","Langsung",750000),
                new RuteKapal("KP002","Kapal Jaya","Surabaya - Bali",new DateTime(2025,12,10),"07:00","12:00","Transit 1x",450000),
                new RuteKapal("KP003","Kapal Maritim","Bali - Surabaya",new DateTime(2025,12,18),"11:00","18:00","Langsung",350000)
            };
        }

        private void LoadDataRute()
        {
            dgvRoutes.Rows.Clear();

            foreach (var r in daftarRuteAll)
            {
                dgvRoutes.Rows.Add(
                    r.ID, r.Kapal, r.Rute,
                    r.Tanggal.ToString("dd MMM yyyy"),
                    r.JamBerangkat, r.JamTiba,
                    r.Transit, $"Rp {r.Harga:N0}"
                );
            }
        }

        private void SetupEventHandlers()
        {
            cbxDari.SelectedIndexChanged += FilterData;
            cbxKe.SelectedIndexChanged += FilterData;
            dtpTanggal.ValueChanged += FilterData;

            dgvRoutes.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == dgvRoutes.Columns["colPilih"].Index)
                {
                    string id = dgvRoutes.Rows[e.RowIndex].Cells[0].Value.ToString();
                    SelectedTicketData.SelectedRute = daftarRuteAll.Find(x => x.ID == id);

                    // Langsung ke UserPenumpang, bukan UserDashboard
                    this.Hide();
                    new UserPenumpang().Show();
                }
            };
        }

        private void FilterData(object sender, EventArgs e)
        {
            List<RuteKapal> filtered = new List<RuteKapal>(daftarRuteAll);

            // FILTER DARI
            if (cbxDari.Text != "Semua")
                filtered = filtered.FindAll(r => r.GetKotaAsal() == cbxDari.Text);

            // FILTER KE
            if (cbxKe.Text != "Semua")
                filtered = filtered.FindAll(r => r.GetKotaTujuan() == cbxKe.Text);

            // Filter tanggal hanya aktif jika user sudah pilih tanggal
            if (userSudahPilihTanggal)
            {
                filtered = filtered.FindAll(r =>
                    r.Tanggal.Month == dtpTanggal.Value.Month &&
                    r.Tanggal.Year == dtpTanggal.Value.Year
                );
            }

            dgvRoutes.Rows.Clear();

            foreach (var r in filtered)
            {
                dgvRoutes.Rows.Add(
                    r.ID, r.Kapal, r.Rute,
                    r.Tanggal.ToString("dd MMM yyyy"),
                    r.JamBerangkat, r.JamTiba,
                    r.Transit,
                    $"Rp {r.Harga:N0}"
                );
            }
        }
       
    }
}