using System;
using System.Drawing;
using System.Windows.Forms;

namespace UI_NaviGO
{
    public partial class UserSuccess : Form
    {
        private Button btnDashboard;
        private Button btnJadwal;

        public UserSuccess()
        {
            InitializeComponent();
            BuildUI();
            SetupEvents();
        }

        private void BuildUI()
        {
            // ===== FORM SETTINGS =====
            this.Text = "NaviGo - Pembayaran Berhasil";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(235, 246, 247);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // ===== PANEL UTAMA (CARD PUTIH) =====
            Panel mainPanel = new Panel()
            {
                Size = new Size(600, 500),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(mainPanel);

            // Tengah-tengah layar
            this.Resize += (s, e) =>
            {
                mainPanel.Location = new Point(
                    (this.ClientSize.Width - mainPanel.Width) / 2,
                    (this.ClientSize.Height - mainPanel.Height) / 2
                );
            };

            // ===== ICON SUCCESS =====
            PictureBox icon = new PictureBox()
            {
                Size = new Size(120, 120),
                SizeMode = PictureBoxSizeMode.Zoom,
                Location = new Point((mainPanel.Width - 120) / 2, 40),
                BackColor = Color.Transparent
            };

            // Buat icon success sederhana
            Bitmap successIcon = new Bitmap(120, 120);
            using (Graphics g = Graphics.FromImage(successIcon))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);

                // Lingkaran hijau
                using (Brush greenBrush = new SolidBrush(Color.FromArgb(76, 175, 80)))
                {
                    g.FillEllipse(greenBrush, 10, 10, 100, 100);
                }

                // Centang putih
                using (Pen whitePen = new Pen(Color.White, 8))
                {
                    g.DrawLine(whitePen, 35, 65, 55, 85);
                    g.DrawLine(whitePen, 55, 85, 85, 45);
                }
            }
            icon.Image = successIcon;
            mainPanel.Controls.Add(icon);

            // ===== TITLE =====
            Label lblTitle = new Label()
            {
                Text = "Pembayaran Berhasil!",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 120, 90),
                AutoSize = true,
                Location = new Point((mainPanel.Width - 220) / 2, 180)
            };
            mainPanel.Controls.Add(lblTitle);

            // ===== MESSAGE =====
            Label lblMessage = new Label()
            {
                Text = "Terima kasih telah menggunakan NaviGO.\n" +
                      "Pesanan Anda sedang diproses.\n" +
                      "E-ticket akan dikirim ke email Anda.",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.Gray,
                AutoSize = false,
                Size = new Size(400, 60),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point((mainPanel.Width - 400) / 2, 230)
            };
            mainPanel.Controls.Add(lblMessage);

            // ===== DETAIL TIKET =====
            if (TiketManager.TiketDipilih != null && TiketManager.DaftarPenumpang.Count > 0)
            {
                Panel detailPanel = new Panel()
                {
                    Size = new Size(500, 100),
                    Location = new Point(50, 300),
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = Color.FromArgb(250, 250, 250)
                };

                Label lblDetailTitle = new Label()
                {
                    Text = "Detail Pemesanan:",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Location = new Point(10, 10),
                    AutoSize = true
                };
                detailPanel.Controls.Add(lblDetailTitle);

                Label lblDetail = new Label()
                {
                    Text = $"{TiketManager.TiketDipilih.NamaKapal}\n" +
                          $"{TiketManager.TiketDipilih.Rute}\n" +
                          $"{TiketManager.DaftarPenumpang.Count} penumpang • {TiketManager.TotalHarga.ToString("C0")}",
                    Font = new Font("Segoe UI", 9),
                    Location = new Point(10, 35),
                    AutoSize = true
                };
                detailPanel.Controls.Add(lblDetail);

                mainPanel.Controls.Add(detailPanel);
            }

            // ===== BUTTON KEMBALI KE DASHBOARD =====
            btnDashboard = new Button()
            {
                Text = "Kembali ke Dashboard",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(255, 204, 153),
                ForeColor = Color.FromArgb(0, 85, 92),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(250, 50),
                Location = new Point((mainPanel.Width - 250) / 2, 420)
            };
            btnDashboard.FlatAppearance.BorderSize = 0;
            mainPanel.Controls.Add(btnDashboard);

            // ===== BUTTON LIHAT JADWAL PENGGUNA =====
            btnJadwal = new Button()
            {
                Text = "Lihat Jadwal Lainnya",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(200, 230, 225),
                ForeColor = Color.FromArgb(0, 85, 92),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(250, 50),
                Location = new Point((mainPanel.Width - 250) / 2, 480)
            };
            btnJadwal.FlatAppearance.BorderSize = 0;
            mainPanel.Controls.Add(btnJadwal);
        }

        private void SetupEvents()
        {
            btnDashboard.Click += (s, e) =>
            {
                this.Hide();
                new UserDashboard().Show();
            };

            btnJadwal.Click += (s, e) =>
            {
                this.Hide();
                new UserJadwal().Show();
            };
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            // Clear data tiket setelah form ditutup
            TiketManager.TiketDipilih = null;
            TiketManager.DaftarPenumpang.Clear();
            TiketManager.TotalHarga = 0;
        }
    }
}