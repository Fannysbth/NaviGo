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
        private ComboBox cmbKategori;
        private TextBox txtNama;
        private TextBox txtNIK;
        private Button btnNext;

        public UserPenumpang()
        {
            InitializeComponent();
            BuildUI();
            SetupEvents();
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
                Size = new Size(850, 400),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            mainPanel.Controls.Add(card);

            Label lblDetail = new Label()
            {
                Text = "Detail Penumpang",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(300, 30)
            };
            card.Controls.Add(lblDetail);

            // Info Tiket yang dipilih
            if (TiketManager.TiketDipilih != null)
            {
                Label lblInfoTiket = new Label()
                {
                    Text = $"Kapal: {TiketManager.TiketDipilih.NamaKapal} | Rute: {TiketManager.TiketDipilih.Rute}",
                    Font = new Font("Segoe UI", 10),
                    ForeColor = Color.Gray,
                    Location = new Point(50, 70),
                    AutoSize = true
                };
                card.Controls.Add(lblInfoTiket);
            }

            cmbKategori = new ComboBox()
            {
                Location = new Point(60, 120),
                Size = new Size(180, 32),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbKategori.Items.AddRange(new string[] { "Dewasa", "Anak-anak", "Lansia" });
            cmbKategori.SelectedIndex = 0;
            card.Controls.Add(cmbKategori);

            Label lblKategori = new Label()
            {
                Text = "Kategori Penumpang",
                Location = new Point(60, 100),
                Font = new Font("Segoe UI", 9),
                AutoSize = true
            };
            card.Controls.Add(lblKategori);

            txtNama = new TextBox()
            {
                Location = new Point(280, 120),
                Size = new Size(200, 32),
                Font = new Font("Segoe UI", 10)
            };
            // Setup placeholder manual
            SetupTextBoxPlaceholder(txtNama, "Nama Lengkap Penumpang");
            card.Controls.Add(txtNama);

            Label lblNama = new Label()
            {
                Text = "Nama Lengkap",
                Location = new Point(280, 100),
                Font = new Font("Segoe UI", 9),
                AutoSize = true
            };
            card.Controls.Add(lblNama);

            txtNIK = new TextBox()
            {
                Location = new Point(520, 120),
                Size = new Size(250, 32),
                Font = new Font("Segoe UI", 10)
            };
            // Setup placeholder manual
            SetupTextBoxPlaceholder(txtNIK, "Nomor Induk Kependudukan");
            card.Controls.Add(txtNIK);

            Label lblNIK = new Label()
            {
                Text = "NIK",
                Location = new Point(520, 100),
                Font = new Font("Segoe UI", 9),
                AutoSize = true
            };
            card.Controls.Add(lblNIK);

            btnNext = new Button()
            {
                Text = "Next →",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Size = new Size(250, 45),
                Location = new Point(300, 200),
                BackColor = Color.FromArgb(255, 204, 153),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(0, 85, 92)
            };
            card.Controls.Add(btnNext);

            // Tombol tambah penumpang
            Button btnTambah = new Button()
            {
                Text = "+ Tambah Penumpang Lain",
                Font = new Font("Segoe UI", 10),
                Size = new Size(200, 35),
                Location = new Point(325, 260),
                BackColor = Color.FromArgb(200, 230, 225),
                FlatStyle = FlatStyle.Flat
            };
            card.Controls.Add(btnTambah);

            this.Controls.AddRange(new Control[] { mainPanel, topPanel, sidebarPanel });

            mainPanel.Resize += (s, e) =>
            {
                card.Location = new Point(
                    (mainPanel.Width - card.Width) / 2,
                    (mainPanel.Height - card.Height) / 2
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
            ((Button)buttons[1]).Click += (s, e) => { this.Hide(); new UserJadwal().Show(); }; // Jadwal
            ((Button)buttons[2]).Click += (s, e) => { this.Hide(); new UserHistory().Show(); }; // Riwayat

            // Event tombol header
            var topButtons = topPanel.Controls;
            ((Button)topButtons[2]).Click += (s, e) => { this.Hide(); new FormProfileUser().Show(); }; // Profile
            ((Button)topButtons[3]).Click += (s, e) =>
            {
                if (MessageBox.Show("Yakin ingin logout?", "Konfirmasi",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Hide();
                    new UserLogin().Show();
                }
            }; // Logout

            // Event tombol next
            btnNext.Click += (s, e) =>
            {
                if (ValidateForm())
                {
                    SimpanDataPenumpang();
                    this.Hide();
                    new UserPembayaran().Show();
                }
            };

            // Event tombol tambah penumpang
            var cardControls = ((Panel)((Panel)this.Controls[2]).Controls[0]).Controls;
            foreach (Control control in cardControls)
            {
                if (control is Button && control.Text == "+ Tambah Penumpang Lain")
                {
                    control.Click += (s, e) =>
                    {
                        TambahPenumpangLain();
                    };
                    break;
                }
            }
        }

        private void TambahPenumpangLain()
        {
            if (ValidateForm())
            {
                SimpanDataPenumpang();
                // Reset form untuk penumpang berikutnya
                txtNama.Text = "Nama Lengkap Penumpang";
                txtNama.ForeColor = Color.Gray;
                txtNIK.Text = "Nomor Induk Kependudukan";
                txtNIK.ForeColor = Color.Gray;

                MessageBox.Show($"Penumpang {TiketManager.DaftarPenumpang[TiketManager.DaftarPenumpang.Count - 1].Nama} berhasil ditambahkan!\nTotal penumpang: {TiketManager.DaftarPenumpang.Count}",
                              "Penumpang Ditambahkan", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtNama.Text) || txtNama.Text == "Nama Lengkap Penumpang")
            {
                MessageBox.Show("Harap isi nama penumpang", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNama.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNIK.Text) || txtNIK.Text == "Nomor Induk Kependudukan" || txtNIK.Text.Length != 16)
            {
                MessageBox.Show("NIK harus 16 digit", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNIK.Focus();
                return false;
            }

            // Validasi NIK hanya angka
            if (!long.TryParse(txtNIK.Text, out _))
            {
                MessageBox.Show("NIK harus berupa angka", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNIK.Focus();
                return false;
            }

            return true;
        }

        private void SimpanDataPenumpang()
        {
            // Add current passenger
            TiketManager.DaftarPenumpang.Add(new Penumpang
            {
                Nama = txtNama.Text,
                NIK = txtNIK.Text,
                Kategori = cmbKategori.SelectedItem?.ToString() ?? "Dewasa"
            });

            // Calculate total harga
            decimal hargaPerTiket = TiketManager.TiketDipilih?.Harga ?? 0;

            // Adjust price based on category
            foreach (var penumpang in TiketManager.DaftarPenumpang)
            {
                if (penumpang.Kategori == "Anak-anak")
                {
                    hargaPerTiket += (TiketManager.TiketDipilih?.Harga ?? 0) * 0.5m; // 50% for children
                }
                else if (penumpang.Kategori == "Lansia")
                {
                    hargaPerTiket += (TiketManager.TiketDipilih?.Harga ?? 0) * 0.7m; // 30% discount for seniors
                }
                else
                {
                    hargaPerTiket += TiketManager.TiketDipilih?.Harga ?? 0;
                }
            }

            TiketManager.TotalHarga = hargaPerTiket;
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