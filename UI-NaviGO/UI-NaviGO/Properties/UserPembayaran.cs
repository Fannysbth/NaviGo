using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace UI_NaviGO
{
    public partial class UserPembayaran : Form
    {
        private Panel sidebarPanel;
        private Panel topPanel;
        private Panel contentPanelBox;

        public UserPembayaran()
        {
            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            // ===== FORM SETTINGS =====
            this.Text = "NaviGo - Pembayaran";
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            // ===== SIDEBAR =====
            sidebarPanel = new Panel()
            {
                BackColor = Color.FromArgb(225, 240, 240),
                Width = 250,
                Dock = DockStyle.Left
            };

            // ===== LOGO PANEL =====
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
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.FromArgb(0, 85, 92),
                Location = new Point(97, 60),
                AutoSize = true
            };

            logoPanel.Controls.AddRange(new Control[] { logo, lblLogoTitle, lblLogoSubtitle });

            // ===== BUTTONS =====
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

            // 🔹 Tambahkan event untuk buka halaman UserHistory
            btnRiwayat.Click += (s, e) =>
            {
                this.Hide();
                new UserHistory().Show();
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
                Text = "Detail Pembayaran",
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
                FlatStyle = FlatStyle.Flat
            };
            btnProfile.FlatAppearance.BorderSize = 0;

            // 🔹 Tambahkan event untuk buka halaman Profile
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
                FlatStyle = FlatStyle.Flat
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

            // ===== CONTENT AREA =====
            Panel contentPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                BackgroundImageLayout = ImageLayout.Stretch,
                BackColor = Color.White
            };

            // Background dengan opacity
            Image original = Properties.Resources.pembayaran_bg;
            contentPanel.BackgroundImage = SetImageOpacity(original, 0.3f);

            // ===== WHITE BOX =====
            contentPanelBox = new Panel()
            {
                Size = new Size(900, 480),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // ===== TITLE =====
            Label lblTitle = new Label()
            {
                Text = "Form Pembayaran",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(310, 30),
                BackColor = Color.Transparent
            };
            contentPanelBox.Controls.Add(lblTitle);

            // ===== FORM =====
            int x1 = 100, x2 = 420, y = 120;

            AddLabelAndTextbox(contentPanelBox, "Dewasa (Pria)", x1, y, 200, "Masukkan nama");
            AddLabelAndTextbox(contentPanelBox, "Dewasa (Wanita)", x1, y + 60, 200, "Masukkan nama");
            AddLabelAndTextbox(contentPanelBox, "Harga Tiket", x2, y, 150);
            AddLabelAndTextbox(contentPanelBox, "Total Harga", x2 + 180, y, 150);
            AddLabelAndTextbox(contentPanelBox, "Total Pembayaran", x1, y + 130, 200);

            Label lblMetode = new Label()
            {
                Text = "Metode Pembayaran",
                Location = new Point(x2, y + 130),
                Font = new Font("Segoe UI", 10)
            };
            contentPanelBox.Controls.Add(lblMetode);

            ComboBox cbMetode = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Size = new Size(180, 25),
                Location = new Point(x2, y + 160),
                Font = new Font("Segoe UI", 10)
            };
            cbMetode.Items.AddRange(new string[] { "Transfer Bank", "Kartu Kredit", "Dana", "OVO", "Gopay" });
            contentPanelBox.Controls.Add(cbMetode);

            AddLabelAndTextbox(contentPanelBox, "Bukti Pembayaran", x2 + 200, y + 130, 150, "Masukkan Bukti");

            Button btnPesan = new Button()
            {
                Text = "Pesan Pelayaran",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                BackColor = Color.FromArgb(255, 204, 153),
                ForeColor = Color.FromArgb(0, 102, 102),
                Size = new Size(250, 50),
                Location = new Point((contentPanelBox.Width - 250) / 2, 350),
                FlatStyle = FlatStyle.Flat
            };
            btnPesan.FlatAppearance.BorderSize = 0;
            btnPesan.Click += (s, e) =>
            {
                this.Hide();
                new UserSuccess().Show();
            };
            contentPanelBox.Controls.Add(btnPesan);

            // ===== ADD PANELS TO FORM =====
            contentPanel.Controls.Add(contentPanelBox);
            this.Controls.Add(contentPanel);
            this.Controls.Add(topPanel);
            this.Controls.Add(sidebarPanel);

            // ===== CENTER BOX =====
            contentPanel.Resize += (s, e) =>
            {
                contentPanelBox.Location = new Point(
                    (contentPanel.Width - contentPanelBox.Width) / 2,
                    (contentPanel.Height - contentPanelBox.Height) / 2
                );
            };
        }

        private void AddLabelAndTextbox(Control parent, string label, int x, int y, int width, string placeholder = "")
        {
            Label lbl = new Label()
            {
                Text = label,
                Location = new Point(x, y),
                Font = new Font("Segoe UI", 10F)
            };
            parent.Controls.Add(lbl);

            TextBox txt = new TextBox()
            {
                Size = new Size(width, 25),
                Location = new Point(x, y + 25),
                Font = new Font("Segoe UI", 10F)
            };
            if (!string.IsNullOrEmpty(placeholder))
            {
                txt.Text = placeholder;
                txt.ForeColor = Color.Gray;
            }
            parent.Controls.Add(txt);
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
