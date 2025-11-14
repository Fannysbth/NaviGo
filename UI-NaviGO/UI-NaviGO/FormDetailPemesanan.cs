using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace UI_NaviGO
{
    public partial class FormDetailPemesanan : Form
    {
        private RiwayatTiket tiket;

        private Panel sidebarPanel;
        private Panel topPanel;
        private Panel contentPanelBox;
        private Panel mainContentPanel;

        public FormDetailPemesanan(RiwayatTiket tiket)
        {
            this.tiket = tiket;
            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            // ========== FORM ==========
            this.Text = "NaviGo - Detail Pelayaran";
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            // ================== SIDEBAR ==================
            sidebarPanel = new Panel()
            {
                BackColor = Color.FromArgb(225, 240, 240),
                Width = 250,
                Dock = DockStyle.Left
            };

            // Logo panel
            Panel logoPanel = new Panel()
            {
                Height = 120,
                Dock = DockStyle.Top
            };

            PictureBox logo = new PictureBox()
            {
                Size = new Size(60, 60),
                Location = new Point(20, 25),
                Image = Properties.Resources.logo_navigo,
                SizeMode = PictureBoxSizeMode.StretchImage
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

            // Sidebar buttons
            Button btnJadwal = new Button()
            {
                Text = "  Jadwal dan Rute     >",
                BackColor = Color.FromArgb(200, 230, 225),
                Dock = DockStyle.Top,
                Height = 45,
                ForeColor = Color.FromArgb(0, 85, 92),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0)
            };
            btnJadwal.FlatAppearance.BorderSize = 0;
            btnJadwal.Click += (s, e) =>
            {
                this.Hide();
                new UserJadwal().Show();
            };

            Button btnRiwayat = new Button()
            {
                Text = "Riwayat Pemesanan",
                Dock = DockStyle.Top,
                Height = 45,
                ForeColor = Color.FromArgb(0, 85, 92),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                BackColor = Color.White // aktif
            };
            btnRiwayat.FlatAppearance.BorderSize = 0;
            btnRiwayat.Click += (s, e) =>
            {
                this.Hide();
                new UserHistory().Show();
            };

            // Add in order so Riwayat appears on top (active)
            sidebarPanel.Controls.AddRange(new Control[] { btnRiwayat, btnJadwal, logoPanel });

            // ================== TOP PANEL ==================
            topPanel = new Panel()
            {
                BackColor = Color.Teal,
                Height = 70,
                Dock = DockStyle.Top
            };

            Label lblHeaderTitle = new Label()
            {
                Text = "Detail Pelayaran",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(15, 22)
            };

            Label lblUsername = new Label()
            {
                Text = $"Halo, {SelectedTicketData.Username}",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11),
                AutoSize = true
            };

            Button btnProfile = new Button()
            {
                Text = "Profile",
                Width = 90,
                Height = 35,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9),
                FlatStyle = FlatStyle.Flat
            };
            btnProfile.FlatAppearance.BorderSize = 0;
            btnProfile.Click += (s, e) =>
            {
                var p = new FormProfileUser();
                p.Closed += (s2, e2) => this.Close();
                p.Show();
                this.Hide();
            };

            Button btnLogout = new Button()
            {
                Text = "Logout",
                Width = 90,
                Height = 35,
                BackColor = Color.FromArgb(210, 80, 60),
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

            // ================== MAIN CONTENT PANEL (SCROLLABLE) ==================
            mainContentPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackgroundImageLayout = ImageLayout.Stretch
            };

            // set background image with opacity if resource exists
            try
            {
                mainContentPanel.BackgroundImage = SetImageOpacity(Properties.Resources.pembayaran_bg, 0.18f);
            }
            catch
            {
                // jika resource tidak ada, abaikan
                mainContentPanel.BackgroundImage = null;
            }

            // White card di tengah
            contentPanelBox = new Panel()
            {
                Size = new Size(900, 600),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(20)
            };

            // Judul kartu
            Label lblTitle = new Label()
            {
                Text = "Detail Pelayaran",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(280, 20),
                ForeColor = Color.FromArgb(0, 85, 92)
            };
            contentPanelBox.Controls.Add(lblTitle);

            int currentY = 80;

            // ---------- SECTION: Informasi Perjalanan ----------
            Panel panelRute = new Panel()
            {
                Size = new Size(800, 120),
                Location = new Point(50, currentY),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(245, 245, 245)
            };

            Label lblInfoRute = new Label()
            {
                Text = "Informasi Perjalanan",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 85, 92),
                Location = new Point(20, 10),
                AutoSize = true
            };

            Label lblRuteDetail = new Label()
            {
                Text = $"Rute: {tiket.Rute}",
                Font = new Font("Segoe UI", 10),
                Location = new Point(20, 40),
                AutoSize = true
            };

            Label lblKapalDetail = new Label()
            {
                Text = $"Kapal: {tiket.Kapal} | ID: {tiket.ID}",
                Font = new Font("Segoe UI", 10),
                Location = new Point(20, 60),
                AutoSize = true
            };

            Label lblTanggalDetail = new Label()
            {
                Text = $"Tanggal: {tiket.TanggalBerangkat:dd MMMM yyyy} | {tiket.Waktu}",
                Font = new Font("Segoe UI", 10),
                Location = new Point(20, 80),
                AutoSize = true
            };

            panelRute.Controls.AddRange(new Control[] { lblInfoRute, lblRuteDetail, lblKapalDetail, lblTanggalDetail });
            contentPanelBox.Controls.Add(panelRute);

            currentY += 140;

            // ---------- SECTION: Detail Tiket (readonly textboxes) ----------
            Label lblSectionTiket = new Label()
            {
                Text = "Detail Tiket",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(50, currentY),
                AutoSize = true
            };
            contentPanelBox.Controls.Add(lblSectionTiket);

            currentY += 40;

            int labelX = 50;
            int valueX = 260;
            int labelWidth = 180;
            int valueWidth = 520;
            int lineHeight = 34;

            void AddReadOnlyField(string label, string value)
            {
                Label l = new Label()
                {
                    Text = label + ":",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Location = new Point(labelX, currentY),
                    Size = new Size(labelWidth, 25),
                    TextAlign = ContentAlignment.MiddleLeft
                };
                TextBox tb = new TextBox()
                {
                    Text = value,
                    Font = new Font("Segoe UI", 10),
                    Location = new Point(valueX, currentY),
                    Size = new Size(valueWidth, 25),
                    ReadOnly = true,
                    BackColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle,
                    ForeColor = Color.Black
                };
                contentPanelBox.Controls.Add(l);
                contentPanelBox.Controls.Add(tb);
                currentY += lineHeight;
            }

            AddReadOnlyField("ID Tiket", tiket.ID);
            AddReadOnlyField("Kelas", tiket.Kelas);
            AddReadOnlyField("Penumpang", tiket.Penumpang);
            AddReadOnlyField("Tanggal Pemesanan", tiket.TanggalPesan.ToString("dd MMMM yyyy"));

            // Status with colored panel
            currentY += 5;
            Panel statusPanel = new Panel()
            {
                Location = new Point(valueX, currentY),
                Size = new Size(140, 30),
                BackColor = GetStatusColor(tiket.Status),
                BorderStyle = BorderStyle.FixedSingle
            };
            Label lblStatus = new Label()
            {
                Text = tiket.Status,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            statusPanel.Controls.Add(lblStatus);
            contentPanelBox.Controls.Add(statusPanel);

            currentY += 45;

            // ---------- SECTION: Informasi Pembayaran ----------
            Label lblSectionBayar = new Label()
            {
                Text = "Informasi Pembayaran",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(50, currentY),
                AutoSize = true,
                ForeColor = Color.FromArgb(0, 85, 92)
            };
            contentPanelBox.Controls.Add(lblSectionBayar);

            currentY += 40;

            AddReadOnlyField("Total Harga", tiket.TotalHarga.ToString("C0"));
            if (!string.IsNullOrEmpty(tiket.MetodePembayaran))
            {
                AddReadOnlyField("Metode Pembayaran", tiket.MetodePembayaran);
            }

            // Spacer
            currentY += 20;

            // ---------- Tombol Kembali (di tengah) ----------
            Button btnKembali = new Button()
            {
                Text = "Kembali",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(255, 204, 153),
                ForeColor = Color.FromArgb(0, 102, 102),
                Size = new Size(200, 45),
                Location = new Point((contentPanelBox.Width - 200) / 2, currentY + 20),
                FlatStyle = FlatStyle.Flat
            };
            btnKembali.FlatAppearance.BorderSize = 0;
            btnKembali.Click += (s, e) =>
            {
                this.Hide();
                new UserHistory().Show();
            };
            contentPanelBox.Controls.Add(btnKembali);

            // Add card to main
            mainContentPanel.Controls.Add(contentPanelBox);
            this.Controls.Add(mainContentPanel);
            this.Controls.Add(topPanel);
            this.Controls.Add(sidebarPanel);

            // Center the card inside mainContentPanel on resize
            mainContentPanel.Resize += (s, e) =>
            {
                contentPanelBox.Location = new Point(
                    (mainContentPanel.ClientSize.Width - contentPanelBox.Width) / 2,
                    Math.Max(20, (mainContentPanel.ClientSize.Height - contentPanelBox.Height) / 2)
                );
            };

            // trigger initial layout
            mainContentPanel.PerformLayout();
            mainContentPanel.Refresh();
        }

        private Color GetStatusColor(string status)
        {
            if (string.IsNullOrEmpty(status)) return Color.Gray;

            switch (status.ToLower())
            {
                case "confirmed":
                case "konfirmasi":
                case "terkonfirmasi":
                    return Color.FromArgb(47, 160, 68); // hijau
                case "selesai":
                case "completed":
                    return Color.FromArgb(77, 124, 133); // biru tua
                case "cancelled":
                case "dibatalkan":
                case "cancel":
                    return Color.FromArgb(209, 100, 58); // orange/merah
                default:
                    return Color.Gray;
            }
        }

        private Image SetImageOpacity(Image image, float opacity)
        {
            if (image == null) return null;
            Bitmap bmp = new Bitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                ColorMatrix matrix = new ColorMatrix();
                matrix.Matrix33 = opacity;
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(matrix);
                g.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height),
                    0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
            }
            return bmp;
        }
    }
}
