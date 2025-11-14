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
        private ComboBox cbMetode;
        private TextBox txtBuktiPembayaran;
        private Button btnPesan;
        private Label lblTotalPembayaran;

        public UserPembayaran()
        {
            InitializeComponent();
            BuildUI();
            SetupEvents();
            UpdateDisplay();
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
                Size = new Size(900, 550),
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
            int x1 = 100, x2 = 500, y = 100;

            // Informasi Tiket
            Label lblInfoTiket = new Label()
            {
                Text = "Informasi Tiket:",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(x1, y),
                AutoSize = true
            };
            contentPanelBox.Controls.Add(lblInfoTiket);

            if (TiketManager.TiketDipilih != null)
            {
                Label lblDetailTiket = new Label()
                {
                    Text = $"{TiketManager.TiketDipilih.NamaKapal} - {TiketManager.TiketDipilih.Rute}",
                    Font = new Font("Segoe UI", 10),
                    Location = new Point(x1, y + 30),
                    AutoSize = true
                };
                contentPanelBox.Controls.Add(lblDetailTiket);

                Label lblTanggal = new Label()
                {
                    Text = $"Tanggal: {TiketManager.TiketDipilih.TanggalBerangkat:dd/MM/yyyy HH:mm}",
                    Font = new Font("Segoe UI", 10),
                    Location = new Point(x1, y + 55),
                    AutoSize = true
                };
                contentPanelBox.Controls.Add(lblTanggal);
            }

            y += 100;

            // Detail Penumpang
            Label lblPenumpang = new Label()
            {
                Text = "Detail Penumpang:",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(x1, y),
                AutoSize = true
            };
            contentPanelBox.Controls.Add(lblPenumpang);

            int penumpangY = y + 30;
            foreach (var penumpang in TiketManager.DaftarPenumpang)
            {
                Label lblPenumpangDetail = new Label()
                {
                    Text = $"{penumpang.Nama} ({penumpang.Kategori}) - NIK: {penumpang.NIK}",
                    Font = new Font("Segoe UI", 10),
                    Location = new Point(x1, penumpangY),
                    AutoSize = true
                };
                contentPanelBox.Controls.Add(lblPenumpangDetail);
                penumpangY += 25;
            }

            y = penumpangY + 20;

            // Informasi Pembayaran
            Label lblHargaTiket = new Label()
            {
                Text = $"Harga per tiket: {(TiketManager.TiketDipilih?.Harga.ToString("C0") ?? "Rp 0")}",
                Font = new Font("Segoe UI", 10),
                Location = new Point(x1, y),
                AutoSize = true
            };
            contentPanelBox.Controls.Add(lblHargaTiket);

            Label lblJumlahPenumpang = new Label()
            {
                Text = $"Jumlah penumpang: {TiketManager.DaftarPenumpang.Count} orang",
                Font = new Font("Segoe UI", 10),
                Location = new Point(x1, y + 25),
                AutoSize = true
            };
            contentPanelBox.Controls.Add(lblJumlahPenumpang);

            lblTotalPembayaran = new Label()
            {
                Text = $"Total Pembayaran: {TiketManager.TotalHarga.ToString("C0")}",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 85, 92),
                Location = new Point(x1, y + 55),
                AutoSize = true
            };
            contentPanelBox.Controls.Add(lblTotalPembayaran);

            // Metode Pembayaran
            Label lblMetode = new Label()
            {
                Text = "Metode Pembayaran",
                Location = new Point(x2, y),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            contentPanelBox.Controls.Add(lblMetode);

            cbMetode = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Size = new Size(200, 25),
                Location = new Point(x2, y + 25),
                Font = new Font("Segoe UI", 10)
            };
            cbMetode.Items.AddRange(new string[] { "Transfer Bank BCA", "Transfer Bank BRI", "Kartu Kredit", "Dana", "OVO", "Gopay" });
            contentPanelBox.Controls.Add(cbMetode);

            // Bukti Pembayaran
            Label lblBukti = new Label()
            {
                Text = "No. Referensi/Transaksi",
                Location = new Point(x2, y + 65),
                Font = new Font("Segoe UI", 10)
            };
            contentPanelBox.Controls.Add(lblBukti);

            txtBuktiPembayaran = new TextBox()
            {
                Size = new Size(200, 25),
                Location = new Point(x2, y + 90),
                Font = new Font("Segoe UI", 10)
            };
            // Setup placeholder manual
            SetupTextBoxPlaceholder(txtBuktiPembayaran, "Contoh: TRX123456789");
            contentPanelBox.Controls.Add(txtBuktiPembayaran);

            btnPesan = new Button()
            {
                Text = "Konfirmasi Pembayaran",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                BackColor = Color.FromArgb(255, 204, 153),
                ForeColor = Color.FromArgb(0, 102, 102),
                Size = new Size(250, 50),
                Location = new Point((contentPanelBox.Width - 250) / 2, y + 140),
                FlatStyle = FlatStyle.Flat
            };
            btnPesan.FlatAppearance.BorderSize = 0;
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

        private void SetupEvents()
        {
            // Event tombol sidebar
            var buttons = sidebarPanel.Controls;
            ((Button)buttons[1]).Click += (s, e) => { this.Hide(); new UserJadwal().Show(); };
            ((Button)buttons[2]).Click += (s, e) => { this.Hide(); new UserHistory().Show(); };

            // Event tombol header
            var topButtons = topPanel.Controls;
            ((Button)topButtons[2]).Click += (s, e) => { this.Hide(); new FormProfileUser().Show(); };
            ((Button)topButtons[3]).Click += (s, e) =>
            {
                if (MessageBox.Show("Yakin ingin logout?", "Konfirmasi",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Hide();
                    new UserLogin().Show();
                }
            };

            // Event tombol pesan
            btnPesan.Click += (s, e) =>
            {
                if (ValidateForm())
                {
                    ProsesPembayaran();
                }
            };

            // Event perubahan metode pembayaran
            cbMetode.SelectedIndexChanged += (s, e) => UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            // Update tampilan berdasarkan data terkini
            lblTotalPembayaran.Text = $"Total Pembayaran: {TiketManager.TotalHarga.ToString("C0")}";
        }

        private bool ValidateForm()
        {
            if (cbMetode.SelectedIndex == -1)
            {
                MessageBox.Show("Harap pilih metode pembayaran", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbMetode.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtBuktiPembayaran.Text) || txtBuktiPembayaran.Text == "Contoh: TRX123456789")
            {
                MessageBox.Show("Harap isi nomor referensi/transaksi", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBuktiPembayaran.Focus();
                return false;
            }

            return true;
        }

        private void ProsesPembayaran()
        {
            try
            {
                // Simpan data pembayaran
                TiketManager.MetodePembayaran = cbMetode.SelectedItem.ToString();
                TiketManager.BuktiPembayaran = txtBuktiPembayaran.Text;

                // Simulasi proses pembayaran
                MessageBox.Show("Pembayaran berhasil diproses!\n" +
                              $"Metode: {TiketManager.MetodePembayaran}\n" +
                              $"Total: {TiketManager.TotalHarga.ToString("C0")}\n" +
                              "Tiket akan dikirim ke email Anda.",
                              "Pembayaran Berhasil",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Simpan ke database (simulasi)
                SimpanKeDatabase();

                // Pindah ke halaman sukses
                this.Hide();
                new UserSuccess().Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SimpanKeDatabase()
        {
            // Simulasi penyimpanan ke database
            // Dalam implementasi nyata, ini akan menyimpan ke PostgreSQL
            Console.WriteLine($"Menyimpan tiket ke database:");
            Console.WriteLine($"- Kapal: {TiketManager.TiketDipilih?.NamaKapal}");
            Console.WriteLine($"- Rute: {TiketManager.TiketDipilih?.Rute}");
            Console.WriteLine($"- Total Harga: {TiketManager.TotalHarga}");
            Console.WriteLine($"- Jumlah Penumpang: {TiketManager.DaftarPenumpang.Count}");
            Console.WriteLine($"- Metode Bayar: {TiketManager.MetodePembayaran}");
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