using System;
using System.Drawing;
using System.Windows.Forms;

namespace UI_NaviGO
{
    public partial class UserHistory : Form
    {
        public UserHistory()
        {
            InitializeUI();
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
            btnJadwalRute.Click += (s, e) =>
            {
                this.Hide();
                new UserDashboard().Show();
            };

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
            btnProfile.Click += (s, e) =>
            {
                this.Hide();
                new FormProfileUser().Show();
            };

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
            btnLogout.Click += (s, e) =>
            {
                this.Hide();
                new UserLogin().Show();
            };

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
                Text = "Tiket Anda saat ini",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                Location = new Point(8, 6),
                AutoSize = true
            };

            Label lblTicketCount = new Label
            {
                Text = "2",
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

            ComboBox cmbStatus = new ComboBox
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

            ComboBox cmbPeriode = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9F),
                Location = new Point(190, 34),
                Size = new Size(180, 25)
            };
            cmbPeriode.Items.AddRange(new object[] { "1 Bulan Terakhir", "3 Bulan Terakhir", "6 Bulan Terakhir", "1 Tahun" });
            cmbPeriode.SelectedIndex = 1;

            Label lblRute = new Label
            {
                Text = "Rute",
                Font = new Font("Segoe UI", 9F),
                Location = new Point(390, 14),
                AutoSize = true
            };

            TextBox txtRute = new TextBox
            {
                Font = new Font("Segoe UI", 9F),
                Text = "Cari Rute...",
                ForeColor = Color.Gray,
                Location = new Point(390, 34),
                Size = new Size(220, 25)
            };
            txtRute.GotFocus += (s, e) =>
            {
                if (txtRute.Text == "Cari Rute...") { txtRute.Text = ""; txtRute.ForeColor = Color.Black; }
            };
            txtRute.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtRute.Text)) { txtRute.Text = "Cari Rute..."; txtRute.ForeColor = Color.Gray; }
            };

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

            pnlFilters.Controls.AddRange(new Control[] { lblStatus, cmbStatus, lblPeriode, cmbPeriode, lblRute, txtRute, btnFilter });

            // ===== FLOW TICKET =====
            FlowLayoutPanel flowTickets = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(6)
            };

            flowTickets.Controls.Add(CreateTicketCard("NV2025001", "Jakarta - Batam", "Dipesan pada: 20 Oktober 2025", "15 Desember 2025", "08.00 WIB", "2 Dewasa, 1 Anak", "KM Sinar Harapan", "Ekonomi", "Rp 1.350.000", "Confirmed"));
            flowTickets.Controls.Add(CreateTicketCard("NV2024022", "Surabaya - Bali", "Dipesan pada: 16 Januari 2024", "28 Maret 2024", "19.00 WIB", "2 Dewasa", "KM Nusantara Jaya", "Bisnis", "Rp 2.200.000", "Selesai"));

            content.Controls.AddRange(new Control[] { flowTickets, pnlFilters, pnlTitle });

            // ===== FORM SETUP =====
            this.Text = "NaviGO - Riwayat Pemesanan";
            this.ClientSize = new Size(1200, 720);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Controls.Add(content);
            this.Controls.Add(header);
            this.Controls.Add(sidebar);
        }

        // === KARTU TIKET ===
        private Panel CreateTicketCard(string id, string route, string bookedDate, string departDate, string time, string passenger, string ship, string kelas, string total, string statusText)
        {
            Color cardHeaderBg = Color.FromArgb(95, 86, 86);
            Color cardInnerBg = Color.FromArgb(245, 242, 242);
            Color badgeConfirmed = Color.FromArgb(47, 160, 68);
            Color badgeDone = Color.FromArgb(77, 124, 133);

            Panel card = new Panel
            {
                Width = 820,
                Height = 170,
                Margin = new Padding(6),
                BackColor = Color.Transparent
            };

            Panel outer = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(0) };

            Panel header = new Panel { Height = 40, Dock = DockStyle.Top, BackColor = cardHeaderBg, Padding = new Padding(12, 6, 12, 6) };

            Label lblId = new Label { Text = id, ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold), AutoSize = true, Location = new Point(8, 8) };
            Label lblRoute = new Label { Text = route, ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold), AutoSize = true, Location = new Point(110, 8) };

            Panel badge = new Panel
            {
                Size = new Size(100, 24),
                BackColor = statusText.ToLower().Contains("confirm") ? badgeConfirmed : badgeDone,
                Location = new Point(700, 8),
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

            Label lblTanggal = new Label { Text = "TANGGAL KEBERANGKATAN\n" + departDate, Font = new Font("Segoe UI", 8F), ForeColor = Color.DimGray, Size = new Size(200, 36), Location = new Point(8, 6) };
            Label lblWaktu = new Label { Text = "WAKTU\n" + time, Font = new Font("Segoe UI", 8F), ForeColor = Color.DimGray, Size = new Size(130, 36), Location = new Point(220, 6) };
            Label lblPenumpang = new Label { Text = "PENUMPANG\n" + passenger, Font = new Font("Segoe UI", 8F), ForeColor = Color.DimGray, Size = new Size(190, 36), Location = new Point(360, 6) };
            Label lblKapal = new Label { Text = "KAPAL\n" + ship, Font = new Font("Segoe UI", 8F), ForeColor = Color.DimGray, Size = new Size(160, 36), Location = new Point(560, 6) };
            Label lblKelas = new Label { Text = "KELAS\n" + kelas, Font = new Font("Segoe UI", 8F), ForeColor = Color.DimGray, Size = new Size(80, 36), Location = new Point(740, 6) };

            Label lblTotal = new Label { Text = "Total: " + total, Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(34, 139, 34), AutoSize = true, Location = new Point(8, 70) };

            Button btnDownload = new Button { Text = "Download Tiket", Font = new Font("Segoe UI", 8.5F), Size = new Size(120, 28), Location = new Point(520, 66), BackColor = Color.FromArgb(173, 215, 237), FlatStyle = FlatStyle.Flat };
            btnDownload.FlatAppearance.BorderSize = 0;

            Button btnDetail = new Button { Text = "Detail", Font = new Font("Segoe UI", 8.5F), Size = new Size(80, 28), Location = new Point(650, 66), BackColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnDetail.FlatAppearance.BorderSize = 0;

            Button btnReschedule = new Button { Text = "Reschedule", Font = new Font("Segoe UI", 8.5F), Size = new Size(90, 28), Location = new Point(740, 66), BackColor = Color.FromArgb(253, 170, 109), FlatStyle = FlatStyle.Flat };
            btnReschedule.FlatAppearance.BorderSize = 0;

            Label lblBooked = new Label { Text = bookedDate, Font = new Font("Segoe UI", 8F), ForeColor = Color.LightGray, AutoSize = true, Location = new Point(8, 26) };

            inner.Controls.AddRange(new Control[] { lblTanggal, lblWaktu, lblPenumpang, lblKapal, lblKelas, lblTotal, btnDownload, btnDetail, btnReschedule, lblBooked });

            outer.Controls.Add(inner);
            outer.Controls.Add(header);
            card.Controls.Add(outer);

            return card;
        }
    }
}
