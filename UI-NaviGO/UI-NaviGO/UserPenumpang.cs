using Npgsql;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace UI_NaviGO
{
    public partial class UserPenumpang : Form
    {
        private Panel sidebarPanel;
        private Panel topPanel;
        private Panel card;
        private FlowLayoutPanel panelList;
        private Button btnTambah;
        private Button btnNext;
        private Panel mainPanel;

        private bool isEditMode;
        private RiwayatTiket tiketEdit;

        public UserPenumpang(bool isEditMode = false)
        {
            this.isEditMode = isEditMode;

            if (isEditMode)
            {
                // PERBAIKAN: Ambil data dari SelectedTicketData.TiketReschedule
                this.tiketEdit = SelectedTicketData.TiketReschedule;
            }

            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            // ===== FORM SETTINGS =====
            this.Text = "NaviGo - Detail Penumpang";
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

            Button btnJadwal = new Button()
            {
                Text = "  Jadwal dan Rute",
                BackColor = Color.White,
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
                new UserJadwal().Show();
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
                Text = "Detail Penumpang",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(15, 20)
            };

            Label lblUsername = new Label()
            {
                Text = "Halo, " + UserSession.Name,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.White,
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

            // ===== MAIN CONTENT =====
            mainPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                BackgroundImageLayout = ImageLayout.Stretch,
                AutoScroll = true
            };

            try
            {
                mainPanel.BackgroundImage = SetImageOpacity(Properties.Resources.penumpang_bg, 0.18f);
            }
            catch { }

            // ===== CENTER CARD =====
            card = new Panel()
            {
                Size = new Size(920, 520),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(20)
            };

            Label lblDetail = new Label()
            {
                Text = isEditMode ? "Edit Data Penumpang" : "Detail Penumpang",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                AutoSize = true,
                Location = new Point((card.Width / 2) - 110, 20)
            };
            card.Controls.Add(lblDetail);

            // ===== header =====
            Panel headerPanel = new Panel()
            {
                Location = new Point(80, 60),
                Size = new Size(860, 25),
                BackColor = Color.Transparent
            };

            Label lblKategori = new Label() { Text = "Kategori", Location = new Point(0, 0), Size = new Size(140, 20), Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            Label lblNama = new Label() { Text = "Nama Penumpang", Location = new Point(160, 0), Size = new Size(260, 20), Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            Label lblNIK = new Label() { Text = "NIK", Location = new Point(440, 0), Size = new Size(220, 20), Font = new Font("Segoe UI", 10, FontStyle.Bold) };

            headerPanel.Controls.AddRange(new Control[] { lblKategori, lblNama, lblNIK });
            card.Controls.Add(headerPanel);

            // ===== panelList =====
            panelList = new FlowLayoutPanel()
            {
                Location = new Point(80, 70),
                Size = new Size(860, 330),
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };
            card.Controls.Add(panelList);

            // ===== BUTTONS =====
            btnTambah = new Button()
            {
                Text = "Tambah Penumpang",
                Width = 200,
                Height = 40,
                Location = new Point(40, card.Height - 80),
                BackColor = Color.FromArgb(230, 245, 240),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            btnTambah.FlatAppearance.BorderSize = 0;
            btnTambah.Click += (s, e) => TambahFormPenumpang();

            btnNext = new Button()
            {
                Text = isEditMode ? "Simpan Perubahan" : "Lanjut Pembayaran",
                Width = 260,
                Height = 45,
                Location = new Point(card.Width - 320, card.Height - 85),
                BackColor = Color.FromArgb(250, 180, 150),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 85, 92)
            };
            btnNext.FlatAppearance.BorderSize = 0;
            btnNext.Click += (s, e) => SimpanDanKembali();

            card.Controls.AddRange(new Control[] { btnTambah, btnNext });
            mainPanel.Controls.Add(card);

            this.Controls.AddRange(new Control[] { mainPanel, topPanel, sidebarPanel });

            // Center card
            mainPanel.Resize += (s, e) =>
            {
                card.Location = new Point(
                    (mainPanel.ClientSize.Width - card.Width) / 2,
                    Math.Max(20, (mainPanel.ClientSize.Height - card.Height) / 2)
                );
            };

            // ===== PERBAIKAN: Load data penumpang =====
            LoadDataPenumpang();
        }

        // METHOD BARU: Load data penumpang berdasarkan mode
        private void LoadDataPenumpang()
        {
            panelList.Controls.Clear();

            if (isEditMode)
            {
                // PERBAIKAN: Cek beberapa sumber data untuk mode edit
                if (tiketEdit != null && tiketEdit.Penumpang != null && tiketEdit.Penumpang.Count > 0)
                {
                    // Sumber 1: Dari TiketReschedule
                    foreach (var p in tiketEdit.Penumpang)
                    {
                        TambahFormPenumpang(p.Kategori, p.Nama, p.NIK);
                    }
                }
                else if (SelectedTicketData.Penumpang != null && SelectedTicketData.Penumpang.Count > 0)
                {
                    // Sumber 2: Dari SelectedTicketData.Penumpang
                    foreach (var p in SelectedTicketData.Penumpang)
                    {
                        TambahFormPenumpang(p.Kategori, p.Nama, p.NIK);
                    }
                }
                else
                {
                    // Sumber 3: Load dari database berdasarkan booking ID
                    LoadPenumpangDariDatabase();
                }
            }
            else
            {
                // Mode normal (booking baru)
                for (int i = 0; i < Math.Max(1, SelectedTicketData.JumlahDewasa); i++)
                {
                    TambahFormPenumpang("Dewasa");
                }
            }
        }

        // METHOD BARU: Load data penumpang dari database untuk mode edit
        private void LoadPenumpangDariDatabase()
        {
            try
            {
                if (SelectedTicketData.TiketReschedule?.BookingID == null) return;

                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT full_name, category, nik 
                        FROM passengers 
                        WHERE booking_id = @bookingId";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@bookingId", SelectedTicketData.TiketReschedule.BookingID);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string nama = reader.GetString(reader.GetOrdinal("full_name"));
                                string kategori = reader.GetString(reader.GetOrdinal("category"));
                                string nik = reader.GetString(reader.GetOrdinal("nik"));

                                TambahFormPenumpang(kategori, nama, nik);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading passenger data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============== Tambah row penumpang ==============
        private void TambahFormPenumpang(string kategoriDefault = "Dewasa", string namaDefault = "", string nikDefault = "")
        {
            Panel row = new Panel()
            {
                Width = panelList.Width - 25,
                Height = 40,
                Margin = new Padding(0, 0, 0, 5)
            };

            ComboBox cmbKategori = new ComboBox()
            {
                Location = new Point(0, 16),
                Width = 140,
                Font = new Font("Segoe UI", 9)
            };
            cmbKategori.Items.AddRange(new string[] { "Dewasa", "Anak", "Bayi" });
            cmbKategori.Text = kategoriDefault;
            row.Controls.Add(cmbKategori);

            TextBox txtNama = new TextBox()
            {
                Location = new Point(160, 15),
                Width = 260,
                Font = new Font("Segoe UI", 9),
                Text = namaDefault
            };
            row.Controls.Add(txtNama);

            TextBox txtNIK = new TextBox()
            {
                Location = new Point(440, 15),
                Width = 220,
                Font = new Font("Segoe UI", 9),
                Text = nikDefault
            };
            row.Controls.Add(txtNIK);

            Button btnHapus = new Button()
            {
                Text = "Hapus",
                Location = new Point(680, 12),
                Size = new Size(70, 30),
                BackColor = Color.FromArgb(230, 90, 90),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9)
            };
            btnHapus.FlatAppearance.BorderSize = 0;
            btnHapus.Click += (s, e) =>
            {
                panelList.Controls.Remove(row);
            };
            row.Controls.Add(btnHapus);

            panelList.Controls.Add(row);
        }

        // ============== SIMPAN & KEMBALI ==============
        private void SimpanDanKembali()
        {
            if (!ValidasiDataPenumpang())
                return;

            // Simpan data form ke SelectedTicketData
            SimpanDataPenumpangDariUI();

            if (isEditMode)
            {
                // Untuk mode edit, tutup form dengan DialogResult.OK
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                // Mode normal (booking baru) - lanjut ke pembayaran
                this.Hide();
                new UserPembayaran(false).Show();
            }
        }

        private bool ValidasiDataPenumpang()
        {
            // First save the data from UI to validate
            SimpanDataPenumpangDariUI();

            foreach (var p in SelectedTicketData.Penumpang)
            {
                if (string.IsNullOrWhiteSpace(p.Nama))
                {
                    MessageBox.Show("Nama penumpang tidak boleh kosong!", "Validasi Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(p.NIK) || p.NIK.Length < 16)
                {
                    MessageBox.Show("NIK harus 16 digit!", "Validasi Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            return true;
        }

        private void SimpanDataPenumpangDariUI()
        {
            SelectedTicketData.Penumpang.Clear();

            foreach (Control c in panelList.Controls)
            {
                if (c is Panel row)
                {
                    ComboBox kategori = null;
                    TextBox nama = null;
                    TextBox nik = null;

                    foreach (Control child in row.Controls)
                    {
                        if (child is ComboBox) kategori = (ComboBox)child;
                        else if (child is TextBox)
                        {
                            if (nama == null) nama = (TextBox)child;
                            else nik = (TextBox)child;
                        }
                    }

                    if (kategori != null && nama != null && nik != null)
                    {
                        SelectedTicketData.Penumpang.Add(new PenumpangData()
                        {
                            Kategori = kategori.Text,
                            Nama = nama.Text.Trim(),
                            NIK = nik.Text.Trim()
                        });
                    }
                }
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