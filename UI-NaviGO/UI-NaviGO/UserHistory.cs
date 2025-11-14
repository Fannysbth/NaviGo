using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace UI_NaviGO
{
    public partial class UserHistory : Form
    {
        // Data dummy untuk riwayat pemesanan
        private List<RiwayatTiket> daftarRiwayat;
        private FlowLayoutPanel flowTickets;
        private ComboBox cmbStatus;
        private ComboBox cmbPeriode;
        private TextBox txtRute;
        private Label lblTicketCount;

        public UserHistory()
        {
            InitializeComponent();
            InitializeData();
            InitializeUI();
            SetupEvents();
            RefreshTampilan();
        }

        private void InitializeData()
        {
            // Inisialisasi data dummy riwayat pemesanan
            daftarRiwayat = new List<RiwayatTiket>
            {
                new RiwayatTiket
                {
                    ID = "NV2025001",
                    Rute = "Jakarta - Batam",
                    TanggalPesan = new DateTime(2024, 10, 20),
                    TanggalBerangkat = new DateTime(2024, 12, 15),
                    Waktu = "08:00 WIB",
                    Penumpang = "2 Dewasa, 1 Anak",
                    Kapal = "KM Sinar Harapan",
                    Kelas = "Ekonomi",
                    TotalHarga = 1350000,
                    Status = "Confirmed"
                },
                new RiwayatTiket
                {
                    ID = "NV2024022",
                    Rute = "Surabaya - Bali",
                    TanggalPesan = new DateTime(2024, 1, 16),
                    TanggalBerangkat = new DateTime(2024, 3, 28),
                    Waktu = "19:00 WIB",
                    Penumpang = "2 Dewasa",
                    Kapal = "KM Nusantara Jaya",
                    Kelas = "Bisnis",
                    TotalHarga = 2200000,
                    Status = "Selesai"
                },
                new RiwayatTiket
                {
                    ID = "NV2024033",
                    Rute = "Semarang - Makassar",
                    TanggalPesan = new DateTime(2024, 2, 10),
                    TanggalBerangkat = new DateTime(2024, 4, 5),
                    Waktu = "07:15 WIB",
                    Penumpang = "1 Dewasa",
                    Kapal = "KM Bahari",
                    Kelas = "Ekonomi",
                    TotalHarga = 1200000,
                    Status = "Cancelled"
                },
                new RiwayatTiket
                {
                    ID = "NV2024044",
                    Rute = "Bali - Lombok",
                    TanggalPesan = new DateTime(2024, 3, 1),
                    TanggalBerangkat = new DateTime(2024, 5, 20),
                    Waktu = "10:00 WIB",
                    Penumpang = "3 Dewasa, 2 Anak",
                    Kapal = "KM Pelangi",
                    Kelas = "VIP",
                    TotalHarga = 3500000,
                    Status = "Confirmed"
                }
            };
        }

        private void InitializeUI()
        {
            // === WARNA UTAMA ===
            Color mainBlue = Color.FromArgb(0, 85, 92);       // teal NaviGO
            Color sidebarActive = Color.FromArgb(191, 224, 215);
            Color pageBg = Color.FromArgb(250, 246, 246);
            Color cardHeaderBg = Color.FromArgb(95, 86, 86);
            Color cardInnerBg = Color.FromArgb(245, 242, 242);

            this.components = new System.ComponentModel.Container();

            // === PANEL SIDEBAR ===
            Panel sidebar = new Panel
            {
                BackColor = Color.FromArgb(243, 247, 246),
                Dock = DockStyle.Left,
                Width = 260,
                Padding = new Padding(18, 18, 0, 0)
            };

            PictureBox picLogo = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Location = new Point(20, 12),
                Size = new Size(56, 56),
                Image = Properties.Resources.logo_navigo
            };

            Label lblLogo = new Label
            {
                Text = "NaviGO",
                Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold),
                ForeColor = mainBlue,
                Location = new Point(90, 22),
                AutoSize = true
            };

            Label lblSubtitle = new Label
            {
                Text = "Ship Easily",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                Location = new Point(90, 44),
                AutoSize = true
            };

            Button btnJadwalRute = new Button
            {
                Text = "Jadwal & Rute",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                ForeColor = mainBlue,
                BackColor = Color.White,
                Size = new Size(260, 44),
                Location = new Point(0, 100),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(24, 0, 0, 0)
            };
            btnJadwalRute.FlatAppearance.BorderSize = 0;

            Button btnRiwayat = new Button
            {
                Text = "Riwayat Pemesanan",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold),
                ForeColor = mainBlue,
                BackColor = sidebarActive,
                Size = new Size(260, 44),
                Location = new Point(0, 146),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(24, 0, 0, 0)
            };
            btnRiwayat.FlatAppearance.BorderSize = 0;

            sidebar.Controls.AddRange(new Control[] { btnRiwayat, btnJadwalRute, lblSubtitle, lblLogo, picLogo });

            // === HEADER ===
            Panel header = new Panel
            {
                BackColor = mainBlue,
                Dock = DockStyle.Top,
                Height = 72,
                Padding = new Padding(18, 8, 18, 8)
            };

            Label lblWelcome = new Label
            {
                Text = "Halo, Felicia Angel",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10.5F),
                AutoSize = true,
                Location = new Point(0, 22)
            };

            Button btnProfile = new Button
            {
                Text = "Profile",
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(80, 28),
                Location = new Point(650, 18),
                Font = new Font("Segoe UI", 9F),
                ForeColor = mainBlue
            };
            btnProfile.FlatAppearance.BorderSize = 0;

            Button btnLogout = new Button
            {
                Text = "Logout",
                BackColor = Color.FromArgb(209, 100, 58),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(80, 28),
                Location = new Point(790, 18),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.White
            };
            btnLogout.FlatAppearance.BorderSize = 0;

            header.Controls.AddRange(new Control[] { lblWelcome, btnProfile, btnLogout });

            // === CONTENT AREA ===
            Panel content = new Panel
            {
                BackColor = pageBg,
                Dock = DockStyle.Fill,
                Padding = new Padding(28)
            };

            // ===== TITLE PANEL =====
            Panel pnlTitle = new Panel
            {
                Dock = DockStyle.Top,
                Height = 90,
                Padding = new Padding(8),
                BackColor = Color.Transparent
            };

            Label lblTitle = new Label
            {
                Text = "Riwayat Pemesanan",
                Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold),
                ForeColor = mainBlue,
                AutoSize = true,
                Location = new Point(6, 18)
            };

            Panel pnlTicketCount = new Panel
            {
                Size = new Size(120, 60),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(this.ClientSize.Width - 360, 15),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Padding = new Padding(8)
            };

            Label lblTicketCountLabel = new Label
            {
                Text = "Total Tiket",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                Location = new Point(8, 6),
                AutoSize = true
            };

            lblTicketCount = new Label
            {
                Text = "0",
                Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold),
                ForeColor = mainBlue,
                Location = new Point(50, 20),
                AutoSize = true
            };

            pnlTicketCount.Controls.AddRange(new Control[] { lblTicketCountLabel, lblTicketCount });
            pnlTitle.Controls.AddRange(new Control[] { lblTitle, pnlTicketCount });

            // ===== FILTER PANEL =====
            Panel pnlFilters = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                Padding = new Padding(6),
                BackColor = Color.Transparent
            };

            Label lblStatus = new Label
            {
                Text = "Status",
                Font = new Font("Segoe UI", 9F),
                Location = new Point(10, 14),
                AutoSize = true
            };

            cmbStatus = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9F),
                Location = new Point(10, 34),
                Size = new Size(160, 25)
            };
            cmbStatus.Items.AddRange(new object[] { "Semua Status", "Confirmed", "Selesai", "Cancelled" });
            cmbStatus.SelectedIndex = 0;

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

            pnlFilters.Controls.AddRange(new Control[] { lblStatus, cmbStatus, lblPeriode, cmbPeriode, lblRute, txtRute, btnFilter, btnReset });

            // ===== FLOW TICKET =====
            flowTickets = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(6)
            };

            content.Controls.AddRange(new Control[] { flowTickets, pnlFilters, pnlTitle });

            // ===== FORM SETUP =====
            this.Text = "NaviGO - Riwayat Pemesanan";
            this.ClientSize = new Size(1200, 720);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Controls.Add(content);
            this.Controls.Add(header);
            this.Controls.Add(sidebar);
        }

        private void SetupEvents()
        {
            // Event tombol sidebar
            var sidebarButtons = ((Panel)this.Controls[2]).Controls;
            ((Button)sidebarButtons[1]).Click += (s, e) =>
            {
                this.Hide();
                new UserDashboard().Show();
            }; // Jadwal & Rute

            // Event tombol header
            var headerButtons = ((Panel)this.Controls[1]).Controls;
            ((Button)headerButtons[1]).Click += (s, e) =>
            {
                this.Hide();
                new FormProfileUser().Show();
            }; // Profile

            ((Button)headerButtons[2]).Click += (s, e) =>
            {
                if (MessageBox.Show("Yakin ingin logout?", "Konfirmasi",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Hide();
                    new UserLogin().Show();
                }
            }; // Logout

            // Event filter
            var filterButtons = ((Panel)((Panel)this.Controls[0]).Controls[1]).Controls;
            foreach (Control control in filterButtons)
            {
                if (control is Button)
                {
                    if (control.Text == "Filter")
                    {
                        control.Click += (s, e) => RefreshTampilan();
                    }
                    else if (control.Text == "Reset")
                    {
                        control.Click += (s, e) => ResetFilter();
                    }
                }
            }

            // Event perubahan combobox
            cmbStatus.SelectedIndexChanged += (s, e) => RefreshTampilan();
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

            // Update jumlah tiket
            lblTicketCount.Text = jumlahTiket.ToString();

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
                // Filter status
                if (cmbStatus.SelectedIndex > 0 && tiket.Status != cmbStatus.SelectedItem.ToString())
                    continue;

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
            cmbStatus.SelectedIndex = 0;
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

            Panel card = new Panel
            {
                Width = 820,
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
            if (tiket.Status == "Selesai") badgeColor = badgeDone;
            else if (tiket.Status == "Cancelled") badgeColor = badgeCancelled;

            Panel badge = new Panel
            {
                Size = new Size(100, 24),
                BackColor = badgeColor,
                Location = new Point(700, 8),
                Padding = new Padding(6, 2, 6, 2)
            };
            Label lblBadge = new Label
            {
                Text = tiket.Status,
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
            Label lblPenumpang = new Label { Text = $"PENUMPANG\n{tiket.Penumpang}", Font = new Font("Segoe UI", 8F), ForeColor = Color.DimGray, Size = new Size(190, 36), Location = new Point(360, 6) };
            Label lblKapal = new Label { Text = $"KAPAL\n{tiket.Kapal}", Font = new Font("Segoe UI", 8F), ForeColor = Color.DimGray, Size = new Size(160, 36), Location = new Point(560, 6) };
            Label lblKelas = new Label { Text = $"KELAS\n{tiket.Kelas}", Font = new Font("Segoe UI", 8F), ForeColor = Color.DimGray, Size = new Size(80, 36), Location = new Point(740, 6) };

            Label lblTotal = new Label { Text = $"Total: {tiket.TotalHarga.ToString("C0")}", Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(34, 139, 34), AutoSize = true, Location = new Point(8, 70) };

            // Tombol Download Tiket
            Button btnDownload = new Button
            {
                Text = "Download Tiket",
                Font = new Font("Segoe UI", 8.5F),
                Size = new Size(120, 28),
                Location = new Point(520, 66),
                BackColor = Color.FromArgb(173, 215, 237),
                FlatStyle = FlatStyle.Flat,
                Tag = tiket.ID // Simpan ID tiket di Tag
            };
            btnDownload.FlatAppearance.BorderSize = 0;
            btnDownload.Click += (s, e) => DownloadTiket(btnDownload.Tag.ToString());

            // Tombol Detail
            Button btnDetail = new Button
            {
                Text = "Detail",
                Font = new Font("Segoe UI", 8.5F),
                Size = new Size(80, 28),
                Location = new Point(650, 66),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Tag = tiket.ID
            };
            btnDetail.FlatAppearance.BorderSize = 0;
            btnDetail.Click += (s, e) => TampilkanDetail(btnDetail.Tag.ToString());

            // Tombol Reschedule (hanya untuk tiket dengan status Confirmed)
            Button btnReschedule = new Button
            {
                Text = "Reschedule",
                Font = new Font("Segoe UI", 8.5F),
                Size = new Size(90, 28),
                Location = new Point(740, 66),
                BackColor = tiket.Status == "Confirmed" ? Color.FromArgb(253, 170, 109) : Color.LightGray,
                FlatStyle = FlatStyle.Flat,
                Tag = tiket.ID,
                Enabled = tiket.Status == "Confirmed"
            };
            btnReschedule.FlatAppearance.BorderSize = 0;
            btnReschedule.Click += (s, e) => RescheduleTiket(btnReschedule.Tag.ToString());

            Label lblBooked = new Label { Text = $"Dipesan pada: {tiket.TanggalPesan:dd MMMM yyyy}", Font = new Font("Segoe UI", 8F), ForeColor = Color.LightGray, AutoSize = true, Location = new Point(8, 100) };

            inner.Controls.AddRange(new Control[] { lblTanggal, lblWaktu, lblPenumpang, lblKapal, lblKelas, lblTotal, btnDownload, btnDetail, btnReschedule, lblBooked });

            outer.Controls.Add(inner);
            outer.Controls.Add(header);
            card.Controls.Add(outer);

            return card;
        }

        private void DownloadTiket(string idTiket)
        {
            // Simulasi download tiket
            var tiket = daftarRiwayat.Find(t => t.ID == idTiket);
            if (tiket != null)
            {
                MessageBox.Show($"Download e-ticket untuk {idTiket}\n" +
                              $"Rute: {tiket.Rute}\n" +
                              $"File PDF akan disimpan di folder Downloads",
                              "Download Tiket",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Di sini bisa ditambahkan logika untuk generate PDF
                // GeneratePDFTicket(tiket);
            }
        }

        private void TampilkanDetail(string idTiket)
        {
            var tiket = daftarRiwayat.Find(t => t.ID == idTiket);
            if (tiket != null)
            {
                string detail = $"Detail Tiket:\n\n" +
                              $"ID: {tiket.ID}\n" +
                              $"Rute: {tiket.Rute}\n" +
                              $"Tanggal Berangkat: {tiket.TanggalBerangkat:dd MMMM yyyy}\n" +
                              $"Waktu: {tiket.Waktu}\n" +
                              $"Kapal: {tiket.Kapal}\n" +
                              $"Kelas: {tiket.Kelas}\n" +
                              $"Penumpang: {tiket.Penumpang}\n" +
                              $"Total: {tiket.TotalHarga.ToString("C0")}\n" +
                              $"Status: {tiket.Status}\n" +
                              $"Dipesan pada: {tiket.TanggalPesan:dd MMMM yyyy}";

                MessageBox.Show(detail, "Detail Pemesanan",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RescheduleTiket(string idTiket)
        {
            var tiket = daftarRiwayat.Find(t => t.ID == idTiket);
            if (tiket != null)
            {
                DialogResult result = MessageBox.Show($"Apakah Anda ingin reschedule tiket {idTiket}?\n" +
                                                    $"Rute: {tiket.Rute}\n\n" +
                                                    "Anda akan diarahkan ke halaman jadwal untuk memilih tanggal baru.",
                                                    "Konfirmasi Reschedule",
                                                    MessageBoxButtons.YesNo,
                                                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Simpan data tiket yang akan di-reschedule
                    TiketManager.TiketDipilih = new Tiket
                    {
                        ID = tiket.ID,
                        NamaKapal = tiket.Kapal,
                        Rute = tiket.Rute,
                        Harga = tiket.TotalHarga,
                        TanggalBerangkat = tiket.TanggalBerangkat
                    };

                    MessageBox.Show("Silakan pilih jadwal baru di halaman berikutnya",
                                  "Reschedule",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Hide();
                    new UserJadwal().Show();
                }
            }
        }
    }

    // Class untuk menyimpan data riwayat tiket
    public class RiwayatTiket
    {
        public string ID { get; set; }
        public string Rute { get; set; }
        public DateTime TanggalPesan { get; set; }
        public DateTime TanggalBerangkat { get; set; }
        public string Waktu { get; set; }
        public string Penumpang { get; set; }
        public string Kapal { get; set; }
        public string Kelas { get; set; }
        public decimal TotalHarga { get; set; }
        public string Status { get; set; } // Confirmed, Selesai, Cancelled
    }
}