using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;



namespace UI_NaviGO
{
    public partial class UserHistory : Form
    {
        // Data dummy untuk riwayat pemesanan
        private List<RiwayatTiket> daftarRiwayat;
        private FlowLayoutPanel flowTickets;

        private ComboBox cmbPeriode;
        private TextBox txtRute;
        private Label lblTicketCount;

        public UserHistory()
        {
            InitializeComponent();
            if (DatabaseHelper.TestConnection())
            {
                LoadRiwayatDariDatabase(UserSession.UserId);
                //LoadRiwayatDariDatabase(1);// ganti dengan userId yang login
            }

            InitializeUI();
            SetupEvents();
            RefreshTampilan();
        }

        private void LoadRiwayatDariDatabase(int userId)
        {
            daftarRiwayat = new List<RiwayatTiket>();

            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    // Query dengan payment_status
                    string sql = @"
                SELECT 
                    b.booking_id,
                    b.booking_reference,
                    b.booking_date,
                    r.route_name,
                    s.departure_date,
                    s.departure_time,
                    u.name AS user_name,
                    b.selected_class,
                    b.total_price,
                    p.payment_method,
                    b.payment_status,  
                    STRING_AGG(pass.full_name || 
                              CASE WHEN pass.category IS NOT NULL AND pass.category != '' 
                                   THEN ' (' || pass.category || ')' 
                                   ELSE '' 
                              END, ', ') AS penumpang_list
                FROM bookings b
                JOIN schedules s ON b.schedule_id = s.schedule_id
                JOIN routes r ON s.route_id = r.route_id
                JOIN users u ON b.user_id = u.user_id
                LEFT JOIN payments p ON b.booking_id = p.booking_id
                LEFT JOIN passengers pass ON b.booking_id = pass.booking_id
                WHERE b.user_id = @userId
                GROUP BY b.booking_id, b.booking_reference, r.route_name, s.departure_date, 
                         s.departure_time, u.name, b.selected_class, b.total_price, 
                         p.payment_method, b.payment_status  -- TAMBAHKAN payment_status DI GROUP BY
                ORDER BY b.booking_date DESC;
            ";

                    using (var cmd = new Npgsql.NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("userId", userId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Ambil string agregasi penumpang
                                string penumpangAgg = reader.IsDBNull(reader.GetOrdinal("penumpang_list"))
                                    ? ""
                                    : reader.GetString(reader.GetOrdinal("penumpang_list"));

                                // Parsing string → List<PenumpangData>
                                var listPenumpang = new List<PenumpangData>();

                                if (!string.IsNullOrWhiteSpace(penumpangAgg))
                                {
                                    // Format: "Budi (Dewasa), Siti (Anak)"
                                    var split = penumpangAgg.Split(',');

                                    foreach (var item in split)
                                    {
                                        string trimmed = item.Trim();
                                        if (trimmed.Length == 0) continue;

                                        string nama = trimmed;
                                        string kategori = "Lainnya";

                                        // Cari teks dalam tanda kurung
                                        int idx = trimmed.LastIndexOf("(");
                                        if (idx > 0)
                                        {
                                            nama = trimmed.Substring(0, idx).Trim();
                                            kategori = trimmed.Substring(idx + 1).Replace(")", "").Trim();
                                        }

                                        listPenumpang.Add(new PenumpangData
                                        {
                                            Nama = nama,
                                            Kategori = kategori
                                        });
                                    }
                                }

                                var tiket = new RiwayatTiket
                                {
                                    BookingID = reader.GetInt32(reader.GetOrdinal("booking_id")),
                                    ID = reader.IsDBNull(reader.GetOrdinal("booking_reference"))
                                        ? $"BK-{reader.GetInt32(reader.GetOrdinal("booking_id"))}"
                                        : reader.GetString(reader.GetOrdinal("booking_reference")),
                                    Rute = reader.GetString(reader.GetOrdinal("route_name")),
                                    TanggalPesan = reader.GetDateTime(reader.GetOrdinal("booking_date")),
                                    TanggalBerangkat = reader.GetDateTime(reader.GetOrdinal("departure_date")),
                                    Waktu = reader.GetTimeSpan(reader.GetOrdinal("departure_time")).ToString(@"hh\:mm"),
                                    Penumpang = listPenumpang,
                                    Kelas = reader.GetString(reader.GetOrdinal("selected_class")),
                                    TotalHarga = reader.GetDecimal(reader.GetOrdinal("total_price")),
                                    MetodePembayaran = reader.IsDBNull(reader.GetOrdinal("payment_method"))
                                        ? ""
                                        : reader.GetString(reader.GetOrdinal("payment_method")),
                                    PaymentStatus = reader.IsDBNull(reader.GetOrdinal("payment_status"))
                                        ? "unknown"
                                        : reader.GetString(reader.GetOrdinal("payment_status")), // TAMBAHKAN INI
                                    Kapal = "Belum Tersedia",
                                    Status = "paid"
                                };

                                daftarRiwayat.Add(tiket);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal mengambil data riwayat: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        


        private void InitializeUI()
        {
            this.Text = "NaviGO - Riwayat Pemesanan";
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            Color mainBlue = Color.FromArgb(0, 85, 92); // teal NaviGO


            // SIDEBAR
            Panel sidebar = new Panel()
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
                Text = "  Jadwal dan Rute ",
                Dock = DockStyle.Top,
                Height = 45,
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0)
            };

            Button btnRiwayat = new Button()
            {
                Text = "Riwayat Pemesanan ",
                Dock = DockStyle.Top,
                Height = 45,
                BackColor = Color.FromArgb(200, 230, 225),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0)
            };

            btnJadwal.Click += (s, e) =>
            {
                this.Hide();
                new UserJadwal().Show();
            };

            btnRiwayat.FlatAppearance.BorderSize = 0;
            btnJadwal.FlatAppearance.BorderSize = 0;


            sidebar.Controls.AddRange(new Control[]
            {
                btnRiwayat, btnJadwal, logoPanel
            });

            // HEADER
            Panel topHeader = new Panel()
            {
                BackColor = Color.Teal,
                Height = 70,
                Dock = DockStyle.Top
            };

            Label lblHeaderTitle = new Label()
            {
                Text = "Riwayat Pemesanan",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(15, 20),
                AutoSize = true
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



            // === CONTENT AREA ===
            Panel content = new Panel
            {
                BackColor = Color.White,
                Dock = DockStyle.Fill,
                Padding = new Padding(28),
                BorderStyle = BorderStyle.FixedSingle,
            };
            content.BackgroundImage = SetImageOpacity(Properties.Resources.tiket_bg, 0.3f);

            // ===== TITLE PANEL =====


            // ===== FILTER PANEL =====
            Panel pnlFilters = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                Padding = new Padding(6),
                BackColor = Color.Transparent
            };




            Label lblPeriode = new Label
            {
                Text = "Periode",
                Font = new Font("Segoe UI", 9F),
                Location = new Point(190, 14),
                AutoSize = true
            };

            cmbPeriode = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9F),
                Location = new Point(190, 34),
                Size = new Size(180, 25)
            };
            cmbPeriode.Items.AddRange(new object[] { "Semua Periode", "1 Bulan Terakhir", "3 Bulan Terakhir", "6 Bulan Terakhir", "1 Tahun" });
            cmbPeriode.SelectedIndex = 0;

            Label lblRute = new Label
            {
                Text = "Rute",
                Font = new Font("Segoe UI", 9F),
                Location = new Point(390, 14),
                AutoSize = true
            };

            txtRute = new TextBox
            {
                Font = new Font("Segoe UI", 9F),
                Location = new Point(390, 34),
                Size = new Size(220, 25)
            };
            SetupTextBoxPlaceholder(txtRute, "Cari Rute...");

            Button btnFilter = new Button
            {
                Text = "Filter",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(199, 219, 212),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(80, 28),
                Location = new Point(630, 33)
            };
            btnFilter.FlatAppearance.BorderSize = 0;

            // Tombol reset filter
            Button btnReset = new Button
            {
                Text = "Reset",
                Font = new Font("Segoe UI", 9F),
                BackColor = Color.FromArgb(230, 230, 230),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(80, 28),
                Location = new Point(720, 33)
            };
            btnReset.FlatAppearance.BorderSize = 0;

            pnlFilters.Controls.AddRange(new Control[] {  lblPeriode, cmbPeriode, lblRute, txtRute, btnFilter, btnReset });

            // ===== FLOW TICKET =====
            flowTickets = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(6)
            };

            content.Controls.Add(flowTickets);      // Dock = Fill, tapi harus ditambahkan terakhir
            content.Controls.Add(pnlFilters);       // Dock = Top        // Dock = Top







            this.Controls.AddRange(new Control[] { content,  topHeader, sidebar });
        }

        private void SetupEvents()
        {
            // Event filter
            var filterButtons = ((Panel)((Panel)this.Controls[0]).Controls[1]).Controls;
            foreach (Control control in filterButtons)
            {
                if (control is Button btn)
                {
                    if (btn.Text == "Filter")
                    {
                        btn.Click += (s, e) => RefreshTampilan();
                    }
                    else if (btn.Text == "Reset")
                    {
                        btn.Click += (s, e) => ResetFilter();
                    }
                }
            }

            // Event perubahan combobox
            cmbPeriode.SelectedIndexChanged += (s, e) => RefreshTampilan();

            // Event textbox rute (filter saat tekan Enter)
            txtRute.KeyPress += (s, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    RefreshTampilan();
                    e.Handled = true;
                }
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

        private void RefreshTampilan()
        {
            flowTickets.Controls.Clear();

            var tiketTersaring = FilterTiket();
            int jumlahTiket = 0;

            foreach (var riwayat in tiketTersaring)
            {
                flowTickets.Controls.Add(CreateTicketCard(riwayat));
                jumlahTiket++;
            }


            // Tampilkan pesan jika tidak ada tiket
            if (jumlahTiket == 0)
            {
                Label lblNoData = new Label
                {
                    Text = "Tidak ada tiket yang sesuai dengan filter",
                    Font = new Font("Segoe UI", 12F),
                    ForeColor = Color.Gray,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    AutoSize = false,
                    Height = 100
                };
                flowTickets.Controls.Add(lblNoData);
            }
        }

        private List<RiwayatTiket> FilterTiket()
        {
            var hasil = new List<RiwayatTiket>();

            foreach (var tiket in daftarRiwayat)
            {

                // Filter periode
                if (cmbPeriode.SelectedIndex > 0)
                {
                    DateTime batasWaktu = DateTime.Now;
                    switch (cmbPeriode.SelectedIndex)
                    {
                        case 1: batasWaktu = batasWaktu.AddMonths(-1); break;
                        case 2: batasWaktu = batasWaktu.AddMonths(-3); break;
                        case 3: batasWaktu = batasWaktu.AddMonths(-6); break;
                        case 4: batasWaktu = batasWaktu.AddYears(-1); break;
                    }

                    if (tiket.TanggalPesan < batasWaktu)
                        continue;
                }

                // Filter rute
                if (txtRute.Text != "Cari Rute..." && !string.IsNullOrWhiteSpace(txtRute.Text) &&
                    !tiket.Rute.ToLower().Contains(txtRute.Text.ToLower()))
                    continue;

                hasil.Add(tiket);
            }

            return hasil;
        }

        private void ResetFilter()
        {
            cmbPeriode.SelectedIndex = 0;
            txtRute.Text = "Cari Rute...";
            txtRute.ForeColor = Color.Gray;
            RefreshTampilan();
        }

        // === KARTU TIKET ===
        private Panel CreateTicketCard(RiwayatTiket tiket)
        {
            Color cardHeaderBg = Color.FromArgb(95, 86, 86);
            Color cardInnerBg = Color.FromArgb(245, 242, 242);
            Color badgeConfirmed = Color.FromArgb(47, 160, 68);
            Color badgeDone = Color.FromArgb(77, 124, 133);
            Color badgeCancelled = Color.FromArgb(209, 100, 58);
            Color badgeRefunded = Color.FromArgb(150, 150, 150); // Warna untuk status refunded

            Panel card = new Panel
            {
                Width = 920,
                Height = 170,
                Margin = new Padding(6),
                BackColor = Color.Transparent
            };

            Panel outer = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(0) };

            Panel header = new Panel { Height = 40, Dock = DockStyle.Top, BackColor = cardHeaderBg, Padding = new Padding(12, 6, 12, 6) };

            Label lblId = new Label { Text = tiket.ID, ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold), AutoSize = true, Location = new Point(8, 8) };
            Label lblRoute = new Label { Text = tiket.Rute, ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold), AutoSize = true, Location = new Point(110, 8) };

            // Tentukan warna badge berdasarkan status
            Color badgeColor = badgeConfirmed;
            string statusText = tiket.PaymentStatus;

            if (tiket.PaymentStatus?.ToLower() == "refunded")
            {
                badgeColor = badgeRefunded;
                statusText = "Refunded";
            }
            else if (tiket.PaymentStatus?.ToLower() == "refunded")
                badgeColor = badgeDone;

            Panel badge = new Panel
            {
                Size = new Size(100, 24),
                BackColor = badgeColor,
                Location = new Point(800, 8),
                Padding = new Padding(6, 2, 6, 2)
            };
            Label lblBadge = new Label
            {
                Text = statusText,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            badge.Controls.Add(lblBadge);
            header.Controls.AddRange(new Control[] { lblId, lblRoute, badge });

            Panel inner = new Panel { Dock = DockStyle.Fill, BackColor = cardInnerBg, Padding = new Padding(14) };

            Label lblTanggal = new Label { Text = $"TANGGAL KEBERANGKATAN\n{tiket.TanggalBerangkat:dd MMMM yyyy}", Font = new Font("Segoe UI", 8F), ForeColor = Color.DimGray, Size = new Size(200, 36), Location = new Point(8, 6) };
            Label lblWaktu = new Label { Text = $"WAKTU\n{tiket.Waktu}", Font = new Font("Segoe UI", 8F), ForeColor = Color.DimGray, Size = new Size(130, 36), Location = new Point(220, 6) };
            Label lblPenumpang = new Label { Text = $"PENUMPANG\n{tiket.RingkasanPenumpang}", Font = new Font("Segoe UI", 8F), ForeColor = Color.DimGray, Size = new Size(190, 36), Location = new Point(360, 6) };
            Label lblKapal = new Label { Text = $"KAPAL\n{tiket.Kapal}", Font = new Font("Segoe UI", 8F), ForeColor = Color.DimGray, Size = new Size(160, 36), Location = new Point(560, 6) };
            Label lblKelas = new Label { Text = $"KELAS\n{tiket.Kelas}", Font = new Font("Segoe UI", 8F), ForeColor = Color.DimGray, Size = new Size(80, 36), Location = new Point(740, 6) };

            Label lblTotal = new Label { Text = $"Total: {tiket.TotalHarga.ToString("C0")}", Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(34, 139, 34), AutoSize = true, Location = new Point(8, 70) };

            // TOMBOL DOWNLOAD TIKET
            Button btnDownload = new Button
            {
                Text = "Download E-Ticket",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Size = new Size(150, 35),
                Location = new Point(530, 65),
                BackColor = Color.FromArgb(255, 204, 153),
                ForeColor = Color.FromArgb(0, 85, 92),
                FlatStyle = FlatStyle.Flat,
                Tag = tiket
            };
            btnDownload.FlatAppearance.BorderSize = 0;

            btnDownload.Click += (s, e) =>
            {
                var selectedTiket = (RiwayatTiket)((Button)s).Tag;
                FormKirimEmail formKirimEmail = new FormKirimEmail(selectedTiket.BookingID);
                formKirimEmail.ShowDialog();
            };

            // TOMBOL DETAIL
            Button btnDetail = new Button
            {
                Text = "Lihat Detail",
                Font = new Font("Segoe UI", 9F),
                Size = new Size(100, 35),
                Location = new Point(690, 65),
                BackColor = Color.FromArgb(180, 220, 215),
                ForeColor = Color.FromArgb(0, 85, 92),
                FlatStyle = FlatStyle.Flat,
                Tag = tiket
            };
            btnDetail.FlatAppearance.BorderSize = 0;
            btnDetail.Click += (s, e) =>
            {
                var selectedTiket = (RiwayatTiket)((Button)s).Tag;
                FormDetailPemesanan formdetail = new FormDetailPemesanan(selectedTiket.BookingID);
                formdetail.ShowDialog();
            };

            // TOMBOL EDIT TIKET
            Button btnEdit = new Button
            {
                Text = "Edit Ticket",
                Font = new Font("Segoe UI", 9F),
                Size = new Size(100, 35),
                Location = new Point(810, 65),
                BackColor = Color.FromArgb(255, 230, 180),
                ForeColor = Color.FromArgb(120, 60, 0),
                FlatStyle = FlatStyle.Flat,
                Tag = tiket,
                Visible = (tiket.PaymentStatus?.ToLower() != "refunded")
            };
            btnEdit.FlatAppearance.BorderSize = 0;

            btnEdit.Click += (s, e) =>
            {
                var selectedTiket = (RiwayatTiket)((Button)s).Tag;
                FormEditPemesanan fr = new FormEditPemesanan(selectedTiket.BookingID);
                this.Hide();
                fr.Show();
            };

            // TOMBOL REFUND - HANYA MUNCUL JIKA PAYMENT_STATUS = "paid"
            Button btnRefund = new Button
            {
                Text = "Refund",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Size = new Size(100, 35),
                Location = new Point(410, 65), // Posisi sama dengan tombol edit
                BackColor = Color.FromArgb(220, 80, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Tag = tiket,
                Visible = (tiket.PaymentStatus?.ToLower() == "paid") // Hanya tampil jika status paid
            };
            btnRefund.FlatAppearance.BorderSize = 0;

            btnRefund.Click += (s, e) =>
            {
                var selectedTiket = (RiwayatTiket)((Button)s).Tag;

                // Konfirmasi refund
                DialogResult result = MessageBox.Show(
                    $"Apakah Anda yakin ingin melakukan refund untuk tiket {selectedTiket.ID}?\n\nTotal yang akan direfund: {selectedTiket.TotalHarga.ToString("C0")}",
                    "Konfirmasi Refund",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    UpdatePaymentStatusToRefund(selectedTiket.BookingID);
                    RefreshTampilan(); // Refresh untuk memperbarui tampilan
                }
            };

            // TOMBOL DELETE - HANYA MUNCUL JIKA STATUS REFUNDED ATAU TIDAK ADA TOMBOL REFUND
            Button btnDelete = new Button
            {
                Text = "Delete",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Size = new Size(100, 35),
                Location = new Point(810, 65), // Posisi yang sama dengan tombol refund
                BackColor = Color.FromArgb(150, 150, 150),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Tag = tiket,
                Visible = (tiket.PaymentStatus?.ToLower() == "refunded") // Hanya tampil jika status refunded
            };
            btnDelete.FlatAppearance.BorderSize = 0;

            btnDelete.Click += (s, e) =>
            {
                var selectedTiket = (RiwayatTiket)((Button)s).Tag;

                DialogResult result = MessageBox.Show(
                    $"Apakah Anda yakin ingin menghapus tiket {selectedTiket.ID}?\nTiket yang sudah dihapus tidak dapat dikembalikan.",
                    "Konfirmasi Hapus",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    DeleteBooking(selectedTiket.BookingID);
                    // Hapus dari daftarRiwayat
                    daftarRiwayat.RemoveAll(t => t.BookingID == selectedTiket.BookingID);
                    RefreshTampilan(); // Refresh untuk memperbarui tampilan
                }
            };



            Label lblBooked = new Label { Text = $"Dipesan pada: {tiket.TanggalPesan:dd MMMM yyyy}", Font = new Font("Segoe UI", 8F), ForeColor = Color.LightGray, AutoSize = true, Location = new Point(8, 100) };

            inner.Controls.AddRange(new Control[] { lblTanggal, lblWaktu, lblPenumpang, lblKapal, lblKelas, lblTotal, btnDownload, btnDetail, btnEdit, btnRefund, btnDelete, lblBooked });

            outer.Controls.Add(inner);
            outer.Controls.Add(header);
            card.Controls.Add(outer);

            return card;
        }

        private void UpdatePaymentStatusToRefund(int bookingId)
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string sql = "UPDATE bookings SET payment_status = 'refunded' WHERE booking_id = @bookingId";

                    using (var cmd = new Npgsql.NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("bookingId", bookingId);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Update juga di local data
                            var tiket = daftarRiwayat.FirstOrDefault(t => t.BookingID == bookingId);
                            if (tiket != null)
                            {
                                tiket.PaymentStatus = "refunded";
                                tiket.Status = "refunded";
                            }
                            MessageBox.Show("Refund berhasil! Status pembayaran telah diupdate.", "Success",
                                          MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Gagal update status refund.", "Error",
                                          MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error update status refund: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteBooking(int bookingId)
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    // Mulai transaction
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Hapus data dari tabel terkait terlebih dahulu (untuk menghindari constraint violation)
                            string[] deleteQueries = {
                        "DELETE FROM passengers WHERE booking_id = @bookingId",
                        "DELETE FROM payments WHERE booking_id = @bookingId",
                        "DELETE FROM bookings WHERE booking_id = @bookingId"
                    };

                            foreach (string sql in deleteQueries)
                            {
                                using (var cmd = new Npgsql.NpgsqlCommand(sql, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("bookingId", bookingId);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            // Commit transaction
                            transaction.Commit();

                            MessageBox.Show("Tiket berhasil dihapus.", "Success",
                                          MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception($"Gagal menghapus tiket: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error menghapus tiket: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
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