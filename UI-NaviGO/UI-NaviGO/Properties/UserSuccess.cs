using System;
using System.Drawing;
using System.Windows.Forms;

namespace UI_NaviGO
{
    public partial class UserSuccess : Form
    {
        public UserSuccess()
        {
            InitializeComponent();
            BuildUI();
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
                Size = new Size(600, 420),
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

            // Jika punya gambar success di Resources
            try
            {
                icon.Image = Properties.Resources.success_icon; // pastikan ada di Resources
            }
            catch
            {
                // fallback kalau belum ada gambar
                Bitmap bmp = new Bitmap(120, 120);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);
                    using (Pen p = new Pen(Color.Green, 8))
                    {
                        g.DrawEllipse(p, 10, 10, 100, 100);
                        g.DrawLine(p, 40, 70, 60, 90);
                        g.DrawLine(p, 60, 90, 90, 50);
                    }
                }
                icon.Image = bmp;
            }

            mainPanel.Controls.Add(icon);

            // ===== TITLE =====
            Label lblTitle = new Label()
            {
                Text = "Pembayaran Berhasil!",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 120, 90),
                AutoSize = true,
                Location = new Point((mainPanel.Width - 270) / 2, 180)
            };
            mainPanel.Controls.Add(lblTitle);

            // ===== MESSAGE =====
            Label lblMessage = new Label()
            {
                Text = "Terima kasih telah menggunakan NaviGO.\nPesanan Anda sedang diproses.",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.Gray,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point((mainPanel.Width - 350) / 2, 230)
            };
            mainPanel.Controls.Add(lblMessage);

            // ===== BUTTON KEMBALI KE DASHBOARD =====
            Button btnDashboard = new Button()
            {
                Text = "Kembali ke Dashboard",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(255, 204, 153),
                ForeColor = Color.FromArgb(0, 85, 92),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(250, 50),
                Location = new Point((mainPanel.Width - 260) / 2, 310)
            };
            btnDashboard.FlatAppearance.BorderSize = 0;

            btnDashboard.Click += (s, e) =>
            {
                this.Hide();
                new UserDashboard().Show();
            };
            mainPanel.Controls.Add(btnDashboard);

            // ===== BUTTON LIHAT JADWAL PENGGUNA =====
            Button btnJadwal = new Button()
            {
                Text = "Lihat Jadwal Pengguna",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(200, 230, 225),
                ForeColor = Color.FromArgb(0, 85, 92),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(250, 50),
                Location = new Point((mainPanel.Width - 260) / 2, 370)
            };
            btnJadwal.FlatAppearance.BorderSize = 0;

            btnJadwal.Click += (s, e) =>
            {
                this.Hide();
                new UserJadwal().Show();
            };
            mainPanel.Controls.Add(btnJadwal);
        }
    }
}
