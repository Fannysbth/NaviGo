using Npgsql;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace UI_NaviGO
{
    public partial class FormDetailPemesanan : Form
    {
        private int bookingId;
        private Panel sidebarPanel;
        private Panel topPanel;
        private Panel contentPanelBox;
        private Panel mainContentPanel;

        // Simpan referensi ke kontrol untuk akses mudah
        private Label lblRuteDetail, lblKapalDetail, lblTanggalDetail;
        private TextBox txtIdTiket, txtKelas, txtPenumpang, txtTanggalPesan, txtTotalHarga, txtMetodeBayar;
        private Panel statusPanel;
        private Label lblStatus;
        private DataGridView dgvPenumpang;

        public FormDetailPemesanan(int bookingId)
        {
            this.bookingId = bookingId;
            InitializeComponent();
            BuildUI();
            LoadPassengerData();
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
                Text = "  Jadwal dan Rute",
                BackColor = Color.White,
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
                Font = new Font("Segoe UI", 10),
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
                Text = "Halo, " + UserSession.Name,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.White,
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

            mainContentPanel.BackgroundImage = SetImageOpacity(Properties.Resources.sunset_picture, 0.3f);

            // White card di tengah - TINGGI DINAIIKAN UNTUK MENAMPUNG TABEL
            contentPanelBox = new Panel()
            {
                Size = new Size(900, 900),
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

            // Data rute akan diisi dari database berdasarkan bookingId
            lblRuteDetail = new Label()
            {
                Text = "Rute: ",
                Font = new Font("Segoe UI", 10),
                Location = new Point(20, 40),
                AutoSize = true
            };

            lblKapalDetail = new Label()
            {
                Text = "Kapal: ",
                Font = new Font("Segoe UI", 10),
                Location = new Point(20, 60),
                AutoSize = true
            };

            lblTanggalDetail = new Label()
            {
                Text = "Tanggal: ",
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

            // Simpan semua TextBox dalam list untuk akses mudah
            var textBoxes = new List<TextBox>();

            void AddReadOnlyField(string label, string value, string name = "")
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
                    Name = name,
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
                textBoxes.Add(tb);
                currentY += lineHeight;
            }

            // Data akan diisi dari database
            AddReadOnlyField("ID Tiket", "", "txtIdTiket");
            AddReadOnlyField("Kelas", "", "txtKelas");
            AddReadOnlyField("Penumpang", "", "txtPenumpang");
            AddReadOnlyField("Tanggal Pemesanan", "", "txtTanggalPesan");

            // Assign ke variabel global
            txtIdTiket = textBoxes[0];
            txtKelas = textBoxes[1];
            txtPenumpang = textBoxes[2];
            txtTanggalPesan = textBoxes[3];

            // Status dengan panel berwarna
            currentY += 5;
            statusPanel = new Panel()
            {
                Location = new Point(valueX, currentY),
                Size = new Size(140, 30),
                BackColor = Color.Gray, // Default color
                BorderStyle = BorderStyle.FixedSingle
            };
            lblStatus = new Label()
            {
                Text = "",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            statusPanel.Controls.Add(lblStatus);
            contentPanelBox.Controls.Add(statusPanel);

            currentY += 45;

            // ---------- SECTION: Detail Penumpang (Tabel) ----------
            Label lblSectionPenumpang = new Label()
            {
                Text = "Detail Penumpang",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(50, currentY),
                AutoSize = true,
                ForeColor = Color.FromArgb(0, 85, 92)
            };
            contentPanelBox.Controls.Add(lblSectionPenumpang);

            currentY += 40;

            // Buat DataGridView untuk menampilkan data penumpang
            dgvPenumpang = new DataGridView()
            {
                Location = new Point(50, currentY),
                Size = new Size(800, 150),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                ScrollBars = ScrollBars.Vertical,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                ReadOnly = true
            };

            // Tambahkan kolom
            dgvPenumpang.Columns.Add("No", "No");
            dgvPenumpang.Columns.Add("Nama", "Nama Penumpang");
            dgvPenumpang.Columns.Add("Kategori", "Kategori");
            dgvPenumpang.Columns.Add("NIK", "NIK");

            contentPanelBox.Controls.Add(dgvPenumpang);

            currentY += 160;

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

            // TextBox untuk pembayaran
            AddReadOnlyField("Total Harga", "", "txtTotalHarga");
            AddReadOnlyField("Metode Pembayaran", "", "txtMetodeBayar");

            // Assign ke variabel global
            txtTotalHarga = textBoxes[4];
            txtMetodeBayar = textBoxes[5];

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
            int bottomPadding = 50; // spasi putih di bawah
            mainContentPanel.Resize += (s, e) =>
            {
                contentPanelBox.Location = new Point(
                    (mainContentPanel.ClientSize.Width - contentPanelBox.Width) / 2,
                    Math.Max(20, (mainContentPanel.ClientSize.Height - contentPanelBox.Height - bottomPadding) / 2)
                );
            };
            Panel spacer = new Panel()
            {
                Height = 50, // jarak putih
                Dock = DockStyle.Bottom,
                BackColor = Color.Transparent
            };
            mainContentPanel.Controls.Add(spacer);

            // trigger initial layout
            mainContentPanel.PerformLayout();
            mainContentPanel.Refresh();
        }

        private void LoadPassengerData()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    // Query untuk mengambil data booking dan penumpang
                    string query = @"
                        SELECT 
                            b.booking_id,
                            b.booking_date,
                            b.total_price,
                            b.payment_method,
                            b.payment_status,
                            b.selected_class,
                            s.departure_date,
                            s.departure_time,
                            r.route_name,
                            r.departure_city,
                            r.arrival_city,
                            sh.ship_name,
                            sh.ship_id,
                            p.full_name,
                            p.category,
                            p.nik
                        FROM bookings b
                        JOIN schedules s ON b.schedule_id = s.schedule_id
                        JOIN routes r ON s.route_id = r.route_id
                        JOIN ships sh ON s.ship_id = sh.ship_id
                        LEFT JOIN passengers p ON b.booking_id = p.booking_id
                        WHERE b.booking_id = @bookingId";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@bookingId", bookingId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            bool firstRow = true;
                            int passengerNumber = 1;

                            while (reader.Read())
                            {
                                if (firstRow)
                                {
                                    // Isi data booking (hanya sekali)
                                    UpdateBookingInfo(reader);
                                    firstRow = false;
                                }

                                // Isi data penumpang ke tabel
                                if (!reader.IsDBNull(reader.GetOrdinal("full_name")))
                                {
                                    string nama = reader.GetString(reader.GetOrdinal("full_name"));
                                    string kategori = reader.GetString(reader.GetOrdinal("category"));
                                    string nik = reader.GetString(reader.GetOrdinal("nik"));

                                    dgvPenumpang.Rows.Add(passengerNumber, nama, kategori, nik);
                                    passengerNumber++;
                                }
                            }

                            // Jika tidak ada penumpang, tampilkan pesan
                            if (passengerNumber == 1)
                            {
                                dgvPenumpang.Rows.Add(1, "Tidak ada data penumpang", "", "");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading passenger data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateBookingInfo(NpgsqlDataReader reader)
        {
            try
            {
                // Update informasi rute
                string routeName = reader.GetString(reader.GetOrdinal("route_name"));
                string departureCity = reader.GetString(reader.GetOrdinal("departure_city"));
                string arrivalCity = reader.GetString(reader.GetOrdinal("arrival_city"));
                string shipName = reader.GetString(reader.GetOrdinal("ship_name"));
                string shipid = reader.GetString(reader.GetOrdinal("ship_id"));
                DateTime departureDate = reader.GetDateTime(reader.GetOrdinal("departure_date"));
                TimeSpan departureTime = reader.GetTimeSpan(reader.GetOrdinal("departure_time"));

                lblRuteDetail.Text = $"Rute: {departureCity} - {arrivalCity} ({routeName})";
                lblKapalDetail.Text = $"Kapal: {shipName} | ID: {shipid}";
                lblTanggalDetail.Text = $"Tanggal: {departureDate:dd MMMM yyyy} | {departureTime:hh\\:mm}";

                // Update detail tiket
                DateTime bookingDate = reader.GetDateTime(reader.GetOrdinal("booking_date"));
                decimal totalPrice = reader.GetDecimal(reader.GetOrdinal("total_price"));
                string selectedClass = reader.GetString(reader.GetOrdinal("selected_class"));
                string paymentStatus = reader.GetString(reader.GetOrdinal("payment_status"));
                string paymentMethod = reader.IsDBNull(reader.GetOrdinal("payment_method")) ?
                    "Belum dibayar" : reader.GetString(reader.GetOrdinal("payment_method"));

                // Hitung jumlah penumpang dari query terpisah
                int passengerCount = GetPassengerCount(bookingId);

                txtIdTiket.Text = bookingId.ToString();
                txtKelas.Text = selectedClass;
                txtPenumpang.Text = $"{passengerCount} orang";
                txtTanggalPesan.Text = bookingDate.ToString("dd MMMM yyyy");
                txtTotalHarga.Text = totalPrice.ToString("C0");
                txtMetodeBayar.Text = paymentMethod;

                // Update status
                lblStatus.Text = paymentStatus;
                statusPanel.BackColor = GetStatusColor(paymentStatus);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating booking info: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetPassengerCount(int bookingId)
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM passengers WHERE booking_id = @bookingId";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@bookingId", bookingId);
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error counting passengers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }

        private Color GetStatusColor(string status)
        {
            if (string.IsNullOrEmpty(status)) return Color.Gray;

            switch (status.ToLower())
            {
                case "confirmed":
                case "konfirmasi":
                case "terkonfirmasi":
                case "paid":
                    return Color.FromArgb(47, 160, 68); // hijau
                case "selesai":
                case "completed":
                    return Color.FromArgb(77, 124, 133); // biru tua
                case "cancelled":
                case "dibatalkan":
                case "cancel":
                    return Color.FromArgb(209, 100, 58); // orange/merah
                case "pending":
                    return Color.FromArgb(255, 193, 7); // kuning
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