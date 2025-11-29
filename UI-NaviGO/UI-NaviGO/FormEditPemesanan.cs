using Npgsql;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace UI_NaviGO
{
    public partial class FormEditPemesanan : Form
    {
        private int bookingId;
        private Panel sidebarPanel;
        private Panel topPanel;
        private Panel contentPanelBox;
        private Panel mainContentPanel;

        // Simpan referensi ke kontrol untuk akses mudah
        private Label lblRuteDetail, lblKapalDetail, lblTanggalDetail;
        private DataGridView dgvPenumpang;

        // Data sementara untuk edit
        private BookingDataa tempBookingData;
        private List<PenumpangData> tempPenumpangData;
        private decimal originalTotalPrice;

        public FormEditPemesanan(int bookingId)
        {
            InitializeComponent();
            this.bookingId = bookingId;
            this.tempBookingData = new BookingDataa();
            this.tempPenumpangData = new List<PenumpangData>();

            BuildUI();
            LoadBookingData();
        }

        private void BuildUI()
        {
            // ========== FORM ==========
            this.Text = "NaviGo - Edit Pemesanan";
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
                BackColor = Color.White
            };
            btnRiwayat.FlatAppearance.BorderSize = 0;
            btnRiwayat.Click += (s, e) =>
            {
                this.Hide();
                new UserHistory().Show();
            };

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
                Text = "Edit Pemesanan",
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

            // White card di tengah
            contentPanelBox = new Panel()
            {
                Size = new Size(900, 700),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(20)
            };

            // Judul kartu
            Label lblTitle = new Label()
            {
                Text = "Edit Pemesanan",
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

            // === Tombol Edit Detail Tiket ===
            Button btnEditTiket = new Button()
            {
                Text = "Edit",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(255, 200, 150),
                ForeColor = Color.FromArgb(80, 40, 0),
                Location = new Point(700, 80),
                FlatStyle = FlatStyle.Flat
            };
            btnEditTiket.FlatAppearance.BorderSize = 0;

            btnEditTiket.Click += (s, e) =>
            {
                EditInformasiPerjalanan();
            };

            panelRute.Controls.AddRange(new Control[] { lblInfoRute, lblRuteDetail, lblKapalDetail, lblTanggalDetail, btnEditTiket });
            contentPanelBox.Controls.Add(panelRute);

            currentY += 140;

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

            // === Tombol Edit Penumpang ===
            Button btnEditPenumpang = new Button()
            {
                Text = "Edit",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(255, 200, 150),
                ForeColor = Color.FromArgb(80, 40, 0),
                Location = new Point(750, currentY - 5),
                FlatStyle = FlatStyle.Flat
            };
            btnEditPenumpang.FlatAppearance.BorderSize = 0;
            btnEditPenumpang.Click += (s, e) =>
            {
                EditDataPenumpang();
            };
            contentPanelBox.Controls.Add(btnEditPenumpang);

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

            currentY += 180;

            // ---------- Tombol Aksi ----------
            Panel panelAksi = new Panel()
            {
                Size = new Size(800, 60),
                Location = new Point(50, currentY)
            };

            // Tombol Kembali (Batal Edit)
            Button btnKembali = new Button()
            {
                Text = "Kembali (Batal)",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(255, 150, 150),
                ForeColor = Color.White,
                Size = new Size(180, 45),
                Location = new Point(100, 10),
                FlatStyle = FlatStyle.Flat
            };
            btnKembali.FlatAppearance.BorderSize = 0;
            btnKembali.Click += (s, e) =>
            {
                var result = MessageBox.Show("Batalkan semua perubahan dan kembali?", "Konfirmasi",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    this.Hide();
                    new UserHistory().Show();
                }
            };

            // Tombol Next (Simpan Sementara)
            Button btnNext = new Button()
            {
                Text = "Next →",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(100, 200, 100),
                ForeColor = Color.White,
                Size = new Size(180, 45),
                Location = new Point(520, 10),
                FlatStyle = FlatStyle.Flat
            };
            btnNext.FlatAppearance.BorderSize = 0;
            btnNext.Click += (s, e) =>
            {
                LanjutKePembayaran();
            };

            panelAksi.Controls.AddRange(new Control[] { btnKembali, btnNext });
            contentPanelBox.Controls.Add(panelAksi);

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
        }

        private void LoadBookingData()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    string query = @"
                SELECT 
                    b.booking_id,
                    b.booking_date,
                    b.total_price,
                    b.payment_method,
                    b.payment_status,
                    b.selected_class,
                    s.schedule_id,
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

                            // Bersihkan DataGridView dulu
                            dgvPenumpang.Rows.Clear();
                            tempPenumpangData.Clear();

                            while (reader.Read())
                            {
                                if (firstRow)
                                {
                                    // Isi data booking (hanya sekali)
                                    UpdateBookingInfo(reader);
                                    firstRow = false;
                                }

                                // Isi data penumpang ke tabel dan list sementara
                                if (!reader.IsDBNull(reader.GetOrdinal("full_name")))
                                {
                                    string nama = reader.GetString(reader.GetOrdinal("full_name"));
                                    string kategori = reader.GetString(reader.GetOrdinal("category"));
                                    string nik = reader.GetString(reader.GetOrdinal("nik"));

                                    dgvPenumpang.Rows.Add(passengerNumber, nama, kategori, nik);

                                    // Simpan ke data sementara
                                    tempPenumpangData.Add(new PenumpangData
                                    {
                                        Nama = nama,
                                        Kategori = kategori,
                                        NIK = nik
                                    });

                                    passengerNumber++;
                                }
                            }

                            // Jika tidak ada penumpang, tampilkan pesan
                            if (passengerNumber == 1)
                            {
                                dgvPenumpang.Rows.Add(1, "Tidak ada data penumpang", "", "");
                            }

                            SelectedTicketData.TiketReschedule = new RiwayatTiket
                            {
                                BookingID = bookingId,
                                Penumpang = new List<PenumpangData>(tempPenumpangData),
                                TotalHarga = originalTotalPrice
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading booking data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                decimal totalPrice = reader.GetDecimal(reader.GetOrdinal("total_price"));
                decimal hargaDasar = CalculateBasePrice(reader);

                lblRuteDetail.Text = $"Rute: {departureCity} - {arrivalCity} ({routeName})";
                lblKapalDetail.Text = $"Kapal: {shipName} | ID: {shipid}";
                lblTanggalDetail.Text = $"Tanggal: {departureDate:dd MMMM yyyy} | {departureTime:hh\\:mm}";

                // Simpan data sementara
                tempBookingData.BookingId = bookingId;
                tempBookingData.ScheduleId = reader.GetInt32(reader.GetOrdinal("schedule_id"));
                tempBookingData.RouteName = routeName;
                tempBookingData.DepartureCity = departureCity;
                tempBookingData.ArrivalCity = arrivalCity;
                tempBookingData.ShipName = shipName;
                tempBookingData.ShipId = shipid;
                tempBookingData.DepartureDate = departureDate;
                tempBookingData.DepartureTime = departureTime;
                tempBookingData.SelectedClass = reader.GetString(reader.GetOrdinal("selected_class"));
           
                tempBookingData.TotalPrice = hargaDasar;
                tempBookingData.PaymentMethod = reader.IsDBNull(reader.GetOrdinal("payment_method")) ?
                    "Belum dibayar" : reader.GetString(reader.GetOrdinal("payment_method"));

                // Simpan harga asli
                originalTotalPrice = totalPrice;
                SelectedTicketData.OriginalTotalPrice = originalTotalPrice;
                SelectedTicketData.UpdatedBookingData = tempBookingData;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating booking info: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private decimal CalculateBasePrice(NpgsqlDataReader reader)
        {
            try
            {
                // Coba ambil harga dasar dari routes table melalui schedule
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    string query = @"
                SELECT r.base_price 
                FROM routes r
                JOIN schedules s ON r.route_id = s.route_id
                WHERE s.schedule_id = @scheduleId";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@scheduleId", reader.GetInt32(reader.GetOrdinal("schedule_id")));
                        var result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToDecimal(result);
                        }
                    }
                }

                // Fallback: jika tidak ada base_price, hitung dari total_price dan penumpang
                decimal totalPrice = reader.GetDecimal(reader.GetOrdinal("total_price"));
                string selectedClass = reader.GetString(reader.GetOrdinal("selected_class"));

                // Hitung faktor kelas
                decimal classFactor = 1.0m;
                if (selectedClass == "Bisnis") classFactor = 1.5m;
                else if (selectedClass == "VIP") classFactor = 2.0m;

                // Ambil jumlah penumpang dari database
                int totalPassengers = GetPassengerCount(bookingId);

                if (totalPassengers > 0)
                {
                    // Harga dasar = total_price / (jumlah_penumpang * faktor_kelas)
                    return totalPrice / (totalPassengers * classFactor);
                }

                // Final fallback
                return 100000; // Harga default reasonable
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error CalculateBasePrice: {ex.Message}");
                return 100000; // Harga default
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
                Console.WriteLine($"Error GetPassengerCount: {ex.Message}");
                return 1; // Default 1 penumpang
            }
        }
        private void EditInformasiPerjalanan()
        {
            using (var formEdit = new FormEditTicket(bookingId))
            {
                if (formEdit.ShowDialog() == DialogResult.OK)
                {
                    // Update data sementara dengan data baru dari FormEditTicket
                    if (formEdit.UpdatedBookingData != null)
                    {
                        tempBookingData = formEdit.UpdatedBookingData;

                        // Update tampilan
                        lblRuteDetail.Text = $"Rute: {tempBookingData.DepartureCity} - {tempBookingData.ArrivalCity} ({tempBookingData.RouteName})";
                        lblKapalDetail.Text = $"Kapal: {tempBookingData.ShipName} | ID: {tempBookingData.ShipId}";
                        lblTanggalDetail.Text = $"Tanggal: {tempBookingData.DepartureDate:dd MMMM yyyy} | {tempBookingData.DepartureTime:hh\\:mm}";

                        MessageBox.Show("Informasi perjalanan berhasil diubah (sementara).", "Sukses",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        SelectedTicketData.UpdatedBookingData = tempBookingData;
                    }
                }
            }
        }

        private void EditDataPenumpang()
        {
            SelectedTicketData.TiketReschedule = new RiwayatTiket
            {
                BookingID = bookingId,
                Penumpang = new List<PenumpangData>(tempPenumpangData),
                TotalHarga = originalTotalPrice
            };

            using (var formPenumpang = new UserPenumpang(true))
            {
                // Set data penumpang sementara ke SelectedTicketData
                SelectedTicketData.Penumpang = new List<PenumpangData>(tempPenumpangData);
                SelectedTicketData.IsEdit = true;

                if (formPenumpang.ShowDialog() == DialogResult.OK)
                {
                    // Update data sementara dengan data baru dari form penumpang
                    tempPenumpangData = new List<PenumpangData>(SelectedTicketData.Penumpang);
                    SelectedTicketData.TiketReschedule.Penumpang = new List<PenumpangData>(tempPenumpangData);

                    // Update tampilan DataGridView
                    UpdateTampilanPenumpang();

                    MessageBox.Show("Data penumpang berhasil diubah (sementara).", "Sukses",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Jika batal, kembalikan data semula
                    SelectedTicketData.Penumpang = new List<PenumpangData>(tempPenumpangData);
                }
            }
        }

        private void UpdateTampilanPenumpang()
        {
            dgvPenumpang.Rows.Clear();
            int passengerNumber = 1;

            foreach (var penumpang in tempPenumpangData)
            {
                dgvPenumpang.Rows.Add(passengerNumber, penumpang.Nama, penumpang.Kategori, penumpang.NIK);
                passengerNumber++;
            }

            if (passengerNumber == 1)
            {
                dgvPenumpang.Rows.Add(1, "Tidak ada data penumpang", "", "");
            }
        }

        private void BackupDataSebelumKePembayaran()
        {
            try
            {
                // Pastikan data tersimpan dengan benar sebelum ke pembayaran
                if (tempPenumpangData != null)
                {
                    SelectedTicketData.Penumpang = new List<PenumpangData>(tempPenumpangData);
                }

                if (tempBookingData != null)
                {
                    SelectedTicketData.UpdatedBookingData = tempBookingData;
                }

                // Simpan juga di TiketReschedule
                SelectedTicketData.TiketReschedule = new RiwayatTiket
                {
                    BookingID = bookingId,
                    Penumpang = new List<PenumpangData>(tempPenumpangData),
                    TotalHarga = originalTotalPrice
                };

                Console.WriteLine($"Backup data - Penumpang: {tempPenumpangData.Count}, Booking: {tempBookingData != null}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error backup data: {ex.Message}");
            }
        }

        

        private void LanjutKePembayaran()
        {
            
            try
            {
                BackupDataSebelumKePembayaran();
                // Simpan data sementara ke SelectedTicketData untuk diproses di form pembayaran
                SelectedTicketData.TiketReschedule = new RiwayatTiket
                {
                    BookingID = bookingId,
                    Penumpang = tempPenumpangData,
                    TotalHarga = originalTotalPrice
                };

                SelectedTicketData.UpdatedBookingData = tempBookingData;
                SelectedTicketData.OriginalTotalPrice = originalTotalPrice;
                SelectedTicketData.IsEdit = true;

                // Buka form pembayaran dalam mode edit
                this.Close();
                new UserPembayaran(true).Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

    // Class untuk menyimpan data booking sementara
    public class BookingDataa
    {
        public int BookingId { get; set; }
        public int ScheduleId { get; set; }
        public string RouteName { get; set; }
        public string DepartureCity { get; set; }
        public string ArrivalCity { get; set; }
        public string ShipName { get; set; }
        public string ShipId { get; set; }
        public DateTime DepartureDate { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public string SelectedClass { get; set; }
        public decimal TotalPrice { get; set; }
        public string PaymentMethod { get; set; }
    }
}