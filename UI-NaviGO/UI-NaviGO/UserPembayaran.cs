using Npgsql;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace UI_NaviGO
{
    public partial class UserPembayaran : Form
    {
        private Panel sidebarPanel;
        private Panel topPanel;
        private Panel contentPanelBox;
        private Panel mainContentPanel;
        private ComboBox cmbMetodePembayaran;
        private ComboBox cbKelas;
        private TextBox txtTotalKeseluruhan;

        private bool isEditMode;
        private RiwayatTiket tiketEdit;
        private decimal hargaAwal;
        private int bookingId;

        // Variabel untuk perhitungan harga
        private decimal hargaDasar;
        private int jumlahDewasa, jumlahAnak, jumlahBayi;

        // Controls untuk display harga
        private TextBox txtHargaAwal;
        private TextBox txtHargaBaru;
        private TextBox txtSelisih;
        private Label lblHargaAwal;
        private Label lblHargaBaru;
        private Label lblSelisih;

        // Tombol konfirmasi
        private Button btnPesan;

        public UserPembayaran(bool isEditMode = false)
        {
            this.isEditMode = isEditMode;
            this.bookingId = isEditMode ? SelectedTicketData.TiketReschedule.BookingID : 0;

            if (isEditMode && SelectedTicketData.TiketReschedule != null)
            {
                this.tiketEdit = SelectedTicketData.TiketReschedule;
                this.hargaAwal = SelectedTicketData.OriginalTotalPrice;
            }

            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            // ========== HITUNG TOTAL HARGA ==========
            dynamic r = isEditMode ? (dynamic)GetRouteDataFromBooking()
                       : (dynamic)SelectedTicketData.SelectedRute;
            // DEBUG: Cek nilai harga dasar
            Console.WriteLine($"Harga Dasar: {r.Harga}");
            Console.WriteLine($"Mode Edit: {isEditMode}");
            hargaDasar = r.Harga;
            decimal hargaKelas = SelectedTicketData.HargaKelas;

            if (isEditMode && (SelectedTicketData.Penumpang == null || SelectedTicketData.Penumpang.Count == 0))
            {
                // Jika data penumpang kosong di mode edit, ambil dari TiketReschedule
                if (SelectedTicketData.TiketReschedule != null && SelectedTicketData.TiketReschedule.Penumpang != null)
                {
                    SelectedTicketData.Penumpang = new List<PenumpangData>(SelectedTicketData.TiketReschedule.Penumpang);
                }
            }

            // Hitung jumlah per kategori
            jumlahDewasa = 0;
            jumlahAnak = 0;
            jumlahBayi = 0;

            if (SelectedTicketData.Penumpang != null)
            {
                foreach (var p in SelectedTicketData.Penumpang)
                {
                    if (p.Kategori == "Dewasa") jumlahDewasa++;
                    else if (p.Kategori == "Anak") jumlahAnak++;
                    else if (p.Kategori == "Bayi") jumlahBayi++;
                }
            }
            else
            {
                // Jika masih null, inisialisasi list kosong
                SelectedTicketData.Penumpang = new List<PenumpangData>();
            }

          

            // ========== FORM ==========
            this.Text = "NaviGo - Pembayaran";
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

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
                Text = isEditMode ? "Pembayaran Edit Pemesanan" : "Detail Pembayaran",
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

            mainContentPanel.BackgroundImage = SetImageOpacity(Properties.Resources.pembayaran_bg, 0.3f);

            // White box - diperbesar untuk menampung lebih banyak konten
            int contentHeight = isEditMode ? 1100 : 800;
            contentPanelBox = new Panel()
            {
                Size = new Size(900, contentHeight),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(20)
            };

            Label lblTitle = new Label()
            {
                Text = isEditMode ? "Pembayaran Edit Tiket" : "Detail Pembayaran Tiket",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(280, 30)
            };
            contentPanelBox.Controls.Add(lblTitle);

            int currentY = 80;

            // ========== INFORMASI RUTE ==========
            Panel panelRute = new Panel()
            {
                Size = new Size(800, 100),
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

            string ruteInfo = isEditMode ?
                $"{SelectedTicketData.UpdatedBookingData.DepartureCity} - {SelectedTicketData.UpdatedBookingData.ArrivalCity}" :
                r.Rute;

            string kapalInfo = isEditMode ?
                $"{SelectedTicketData.UpdatedBookingData.ShipName} | ID: {SelectedTicketData.UpdatedBookingData.ShipId}" :
                $"{r.Kapal} | ID: {r.ID}";

            string tanggalInfo = isEditMode ?
                $"Tanggal: {SelectedTicketData.UpdatedBookingData.DepartureDate:dd MMMM yyyy} | {SelectedTicketData.UpdatedBookingData.DepartureTime:hh\\:mm}" :
                $"Tanggal: {r.Tanggal:dd MMMM yyyy} | {r.JamBerangkat} - {r.JamTiba}";

            Label lblRuteDetail = new Label()
            {
                Text = $"Rute: {ruteInfo}",
                Font = new Font("Segoe UI", 10),
                Location = new Point(20, 35),
                AutoSize = true
            };

            Label lblKapalDetail = new Label()
            {
                Text = $"Kapal: {kapalInfo}",
                Font = new Font("Segoe UI", 10),
                Location = new Point(20, 55),
                AutoSize = true
            };

            Label lblTanggalDetail = new Label()
            {
                Text = tanggalInfo,
                Font = new Font("Segoe UI", 10),
                Location = new Point(20, 75),
                AutoSize = true
            };

            panelRute.Controls.AddRange(new Control[] { lblInfoRute, lblRuteDetail, lblKapalDetail, lblTanggalDetail });
            contentPanelBox.Controls.Add(panelRute);

            currentY += 120;

            // ========== PILIHAN KELAS ==========
            Label lblPilihKelas = new Label()
            {
                Text = "Pilih Kelas",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(50, currentY),
                AutoSize = true
            };

            cbKelas = new ComboBox()
            {
                Location = new Point(150, currentY),
                Width = 200,
                Font = new Font("Segoe UI", 10)
            };
            cbKelas.Items.AddRange(new string[] { "Ekonomi", "Bisnis", "VIP" });

            // Set kelas yang sudah dipilih sebelumnya jika ada
            if (!string.IsNullOrEmpty(SelectedTicketData.KelasDipilih))
                cbKelas.Text = SelectedTicketData.KelasDipilih;
            else if (isEditMode && !string.IsNullOrEmpty(SelectedTicketData.UpdatedBookingData.SelectedClass))
                cbKelas.Text = SelectedTicketData.UpdatedBookingData.SelectedClass;
            else
                cbKelas.SelectedIndex = 0;

            contentPanelBox.Controls.AddRange(new Control[] { lblPilihKelas, cbKelas });

            currentY += 50;

            // ========== DETAIL HARGA ==========
            Label lblDetailHarga = new Label()
            {
                Text = "Detail Harga",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(50, currentY),
                AutoSize = true
            };
            contentPanelBox.Controls.Add(lblDetailHarga);

            currentY += 40;

            int labelWidth = 150;
            int textBoxWidth = 120;
            int totalWidth = 100;

            // Header tabel
            Label lblHeaderKategori = new Label()
            {
                Text = "Kategori",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(50, currentY),
                Size = new Size(labelWidth, 25)
            };

            Label lblHeaderJumlah = new Label()
            {
                Text = "Jumlah",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(220, currentY),
                Size = new Size(textBoxWidth, 25)
            };

            Label lblHeaderHarga = new Label()
            {
                Text = "Harga Tiket",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(360, currentY),
                Size = new Size(textBoxWidth, 25)
            };

            Label lblHeaderTotal = new Label()
            {
                Text = "Total Harga",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(500, currentY),
                Size = new Size(totalWidth, 25)
            };

            contentPanelBox.Controls.AddRange(new Control[] { lblHeaderKategori, lblHeaderJumlah, lblHeaderHarga, lblHeaderTotal });

            // DEWASA
            currentY += 40;
            TextBox txtJumlahDewasa = new TextBox() { Text = jumlahDewasa.ToString(), Location = new Point(220, currentY), Size = new Size(textBoxWidth, 25), ReadOnly = true, TextAlign = HorizontalAlignment.Center };
            TextBox txtHargaDewasa = new TextBox() { Text = "", Location = new Point(360, currentY), Size = new Size(textBoxWidth, 25), ReadOnly = true, TextAlign = HorizontalAlignment.Right };
            TextBox txtTotalDewasa = new TextBox() { Text = "", Location = new Point(500, currentY), Size = new Size(totalWidth, 25), ReadOnly = true, TextAlign = HorizontalAlignment.Right, Font = new Font("Segoe UI", 10, FontStyle.Bold) };

            contentPanelBox.Controls.AddRange(new Control[] {
                new Label() { Text = "Dewasa", Font = new Font("Segoe UI", 10), Location = new Point(50, currentY), Size = new Size(labelWidth, 25) },
                txtJumlahDewasa, txtHargaDewasa, txtTotalDewasa
            });

            // ANAK
            currentY += 40;
            TextBox txtJumlahAnak = new TextBox() { Text = jumlahAnak.ToString(), Location = new Point(220, currentY), Size = new Size(textBoxWidth, 25), ReadOnly = true, TextAlign = HorizontalAlignment.Center };
            TextBox txtHargaAnak = new TextBox() { Text = "", Location = new Point(360, currentY), Size = new Size(textBoxWidth, 25), ReadOnly = true, TextAlign = HorizontalAlignment.Right };
            TextBox txtTotalAnak = new TextBox() { Text = "", Location = new Point(500, currentY), Size = new Size(totalWidth, 25), ReadOnly = true, TextAlign = HorizontalAlignment.Right, Font = new Font("Segoe UI", 10, FontStyle.Bold) };

            contentPanelBox.Controls.AddRange(new Control[] {
                new Label() { Text = "Anak-Anak", Font = new Font("Segoe UI", 10), Location = new Point(50, currentY), Size = new Size(labelWidth, 25) },
                txtJumlahAnak, txtHargaAnak, txtTotalAnak
            });

            // BAYI
            currentY += 40;
            TextBox txtJumlahBayi = new TextBox() { Text = jumlahBayi.ToString(), Location = new Point(220, currentY), Size = new Size(textBoxWidth, 25), ReadOnly = true, TextAlign = HorizontalAlignment.Center };
            TextBox txtHargaBayi = new TextBox() { Text = "", Location = new Point(360, currentY), Size = new Size(textBoxWidth, 25), ReadOnly = true, TextAlign = HorizontalAlignment.Right };
            TextBox txtTotalBayi = new TextBox() { Text = "", Location = new Point(500, currentY), Size = new Size(totalWidth, 25), ReadOnly = true, TextAlign = HorizontalAlignment.Right, Font = new Font("Segoe UI", 10, FontStyle.Bold) };

            contentPanelBox.Controls.AddRange(new Control[] {
                new Label() { Text = "Bayi", Font = new Font("Segoe UI", 10), Location = new Point(50, currentY), Size = new Size(labelWidth, 25) },
                txtJumlahBayi, txtHargaBayi, txtTotalBayi
            });

            // Garis pemisah
            currentY += 50;
            Panel garisPemisah = new Panel()
            {
                BackColor = Color.LightGray,
                Location = new Point(50, currentY),
                Size = new Size(650, 1)
            };
            contentPanelBox.Controls.Add(garisPemisah);

            // TOTAL KESELURUHAN
            currentY += 20;
            txtTotalKeseluruhan = new TextBox()
            {
                Text = "",
                Location = new Point(500, currentY),
                Size = new Size(150, 30),
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right,
                BackColor = Color.FromArgb(255, 255, 200),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.DarkRed
            };

            contentPanelBox.Controls.AddRange(new Control[] {
                new Label() { Text = "TOTAL PEMBAYARAN", Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Color.FromArgb(0, 85, 92), Location = new Point(50, currentY), Size = new Size(200, 30) },
                txtTotalKeseluruhan
            });

            currentY += 60;

            // ========== RINCIAN PERUBAHAN HARGA (Hanya untuk mode edit) ==========
            if (isEditMode)
            {
                Label lblRincianPerubahan = new Label()
                {
                    Text = "Rincian Perubahan Harga",
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    Location = new Point(50, currentY),
                    AutoSize = true,
                    ForeColor = Color.FromArgb(0, 85, 92)
                };
                contentPanelBox.Controls.Add(lblRincianPerubahan);

                currentY += 40;

                // Harga Awal
                lblHargaAwal = new Label()
                {
                    Text = "Harga Sebelumnya:",
                    Font = new Font("Segoe UI", 10),
                    Location = new Point(50, currentY),
                    Size = new Size(150, 25)
                };

                txtHargaAwal = new TextBox()
                {
                    Text = $"Rp {hargaAwal:N0}",
                    Location = new Point(220, currentY),
                    Size = new Size(150, 25),
                    ReadOnly = true,
                    TextAlign = HorizontalAlignment.Right,
                    BackColor = Color.FromArgb(240, 240, 240)
                };
                contentPanelBox.Controls.AddRange(new Control[] { lblHargaAwal, txtHargaAwal });

                currentY += 35;

                // Harga Baru
                lblHargaBaru = new Label()
                {
                    Text = "Harga Baru:",
                    Font = new Font("Segoe UI", 10),
                    Location = new Point(50, currentY),
                    Size = new Size(150, 25)
                };

                txtHargaBaru = new TextBox()
                {
                    Text = "",
                    Location = new Point(220, currentY),
                    Size = new Size(150, 25),
                    ReadOnly = true,
                    TextAlign = HorizontalAlignment.Right,
                    BackColor = Color.FromArgb(240, 240, 240)
                };
                contentPanelBox.Controls.AddRange(new Control[] { lblHargaBaru, txtHargaBaru });

                currentY += 35;

                // Selisih
                lblSelisih = new Label()
                {
                    Text = "Selisih yang Harus Dibayar:",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Location = new Point(50, currentY),
                    Size = new Size(180, 25)
                };

                txtSelisih = new TextBox()
                {
                    Text = "",
                    Location = new Point(220, currentY),
                    Size = new Size(150, 25),
                    ReadOnly = true,
                    TextAlign = HorizontalAlignment.Right,
                    BackColor = Color.FromArgb(255, 255, 200),
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    ForeColor = Color.DarkRed
                };
                contentPanelBox.Controls.AddRange(new Control[] { lblSelisih, txtSelisih });

                currentY += 50;

                // Informasi tambahan untuk mode edit
                Label lblInfoEdit = new Label()
                {
                    Text = "Catatan: Jika harga baru lebih rendah, selisih tidak dapat dikembalikan. " +
                          "Jika harga baru lebih tinggi, Anda perlu membayar selisihnya.",
                    Font = new Font("Segoe UI", 9),
                    ForeColor = Color.OrangeRed,
                    Location = new Point(50, currentY),
                    Size = new Size(800, 40),
                    TextAlign = ContentAlignment.MiddleLeft
                };
                contentPanelBox.Controls.Add(lblInfoEdit);

                currentY += 50;
            }

            // ========== METODE PEMBAYARAN ==========
            Label lblMetode = new Label()
            {
                Text = "Metode Pembayaran",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(50, currentY),
                Size = new Size(200, 25)
            };

            cmbMetodePembayaran = new ComboBox()
            {
                Location = new Point(250, currentY),
                Size = new Size(250, 25),
                Font = new Font("Segoe UI", 10)
            };
            cmbMetodePembayaran.Items.AddRange(new string[] {
                "Transfer Bank BCA",
                "Transfer Bank BRI",
                "Transfer Bank Mandiri",
                "Kartu Kredit",
                "E-Wallet (Gopay, OVO, Dana)",
                "Bayar di Tempat"
            });

            // Set metode pembayaran sebelumnya jika ada
            if (isEditMode && !string.IsNullOrEmpty(SelectedTicketData.UpdatedBookingData.PaymentMethod))
                cmbMetodePembayaran.Text = SelectedTicketData.UpdatedBookingData.PaymentMethod;
            else
                cmbMetodePembayaran.SelectedIndex = 0;

            contentPanelBox.Controls.AddRange(new Control[] { lblMetode, cmbMetodePembayaran });

            currentY += 60;

            // ========== KETERANGAN ==========
            Label lblKeterangan = new Label()
            {
                Text = "Keterangan:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(50, currentY),
                AutoSize = true
            };

            currentY += 25;
            Label lblKet1 = new Label()
            {
                Text = "• Harga anak-anak (3-12 tahun): 75% dari harga dewasa",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                Location = new Point(70, currentY),
                AutoSize = true
            };

            currentY += 20;
            Label lblKet2 = new Label()
            {
                Text = "• Harga bayi (0-2 tahun): 25% dari harga dewasa",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                Location = new Point(70, currentY),
                AutoSize = true
            };

            currentY += 20;
            Label lblKet3 = new Label()
            {
                Text = "• Tiket akan dikirim ke email setelah pembayaran dikonfirmasi",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                Location = new Point(70, currentY),
                AutoSize = true
            };

            contentPanelBox.Controls.AddRange(new Control[] { lblKeterangan, lblKet1, lblKet2, lblKet3 });

            // ================== TOMBOL AKSI ==================
            currentY += 50;

            Panel panelTombol = new Panel()
            {
                Size = new Size(800, 60),
                Location = new Point(50, currentY)
            };

            // Tombol Kembali
            Button btnKembali = new Button()
            {
                Text = "Kembali",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(255, 150, 150),
                ForeColor = Color.White,
                Size = new Size(150, 45),
                Location = new Point(100, 10),
                FlatStyle = FlatStyle.Flat
            };
            btnKembali.FlatAppearance.BorderSize = 0;
            btnKembali.Click += (s, e) =>
            {
                if (isEditMode)
                {
                    BackupDataSebelumKembali();
                    // Kembali ke FormEditPemesanan
                    this.Hide();
                    new FormEditPemesanan(bookingId).Show();
                }
                else
                {
                    // Kembali ke form sebelumnya
                    this.Hide();
                    new UserPenumpang().Show();
                }
            };

            // Tombol Konfirmasi - DIUBAH: Sekarang disimpan sebagai variabel class
            btnPesan = new Button()
            {
                Text = isEditMode ? "KONFIRMASI PERUBAHAN" : "KONFIRMASI PEMBAYARAN",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(255, 204, 153),
                ForeColor = Color.FromArgb(0, 102, 102),
                Size = new Size(300, 50),
                Location = new Point(450, 5),
                FlatStyle = FlatStyle.Flat
            };
            btnPesan.FlatAppearance.BorderSize = 0;
            btnPesan.Click += BtnPesan_Click;

            panelTombol.Controls.AddRange(new Control[] { btnKembali, btnPesan });
            contentPanelBox.Controls.Add(panelTombol);

            // Event handler untuk perubahan kelas
            cbKelas.SelectedIndexChanged += CbKelas_SelectedIndexChanged;

            // Inisialisasi harga pertama kali
            UpdateHargaBerdasarkanKelas();

            mainContentPanel.Controls.Add(contentPanelBox);
            this.Controls.Add(mainContentPanel);
            this.Controls.Add(topPanel);
            this.Controls.Add(sidebarPanel);

            mainContentPanel.Resize += (s, e) =>
            {
                contentPanelBox.Location = new Point(
                    (mainContentPanel.ClientSize.Width - contentPanelBox.Width) / 2,
                    Math.Max(20, (mainContentPanel.ClientSize.Height - contentPanelBox.Height) / 2)
                );
            };

            Panel spacer = new Panel()
            {
                Height = 50,
                Dock = DockStyle.Bottom,
                BackColor = Color.Transparent
            };
            mainContentPanel.Controls.Add(spacer);
        }

        private RuteData GetRouteDataFromBooking()
        {
            Console.WriteLine($"UpdatedBookingData TotalPrice: {SelectedTicketData.UpdatedBookingData.TotalPrice}");
            // Buat RuteData dari UpdatedBookingData untuk mode edit
            return new RuteData
            {
                Rute = $"{SelectedTicketData.UpdatedBookingData.DepartureCity} - {SelectedTicketData.UpdatedBookingData.ArrivalCity}",
                Kapal = SelectedTicketData.UpdatedBookingData.ShipName,
                ID = SelectedTicketData.UpdatedBookingData.ShipId,
                Tanggal = SelectedTicketData.UpdatedBookingData.DepartureDate,
                JamBerangkat = SelectedTicketData.UpdatedBookingData.DepartureTime.ToString(@"hh\:mm"),
                JamTiba = "00:00",
                Harga = SelectedTicketData.UpdatedBookingData.TotalPrice,
                ScheduleID = SelectedTicketData.UpdatedBookingData.ScheduleId
            };
        }

        private void CbKelas_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateHargaBerdasarkanKelas();
        }

        private void UpdateHargaBerdasarkanKelas()
        {
            Console.WriteLine($"hargaDasar: {hargaDasar}");
            Console.WriteLine($"Jumlah Dewasa: {jumlahDewasa}, Anak: {jumlahAnak}, Bayi: {jumlahBayi}");

            // Hitung tambahan harga berdasarkan kelas
            decimal tambahan = 0;
            if (cbKelas.Text == "Bisnis") tambahan = 50000;
            else if (cbKelas.Text == "VIP") tambahan = 120000;

            decimal newHargaDewasa = hargaDasar + tambahan;
            decimal newHargaAnak = newHargaDewasa * 0.75m;
            decimal newHargaBayi = newHargaDewasa * 0.25m;

            decimal newTotalDewasa = newHargaDewasa * jumlahDewasa;
            decimal newTotalAnak = newHargaAnak * jumlahAnak;
            decimal newTotalBayi = newHargaBayi * jumlahBayi;
            decimal newTotalKeseluruhan = newTotalDewasa + newTotalAnak + newTotalBayi;

            // Update semua TextBox harga
            UpdateTextBoxHarga("Dewasa", newHargaDewasa, newTotalDewasa);
            UpdateTextBoxHarga("Anak", newHargaAnak, newTotalAnak);
            UpdateTextBoxHarga("Bayi", newHargaBayi, newTotalBayi);

            txtTotalKeseluruhan.Text = $"Rp {newTotalKeseluruhan:N0}";

            // Update rincian perubahan harga untuk mode edit
            if (isEditMode)
            {
                txtHargaBaru.Text = $"Rp {newTotalKeseluruhan:N0}";

                // Hitung selisih (hanya positif, tidak ada refund)
                decimal selisih = newTotalKeseluruhan - hargaAwal;
                if (selisih < 0) selisih = 0;

                txtSelisih.Text = $"Rp {selisih:N0}";

                // UPDATE TOMBOL BERDASARKAN SELISIH
                UpdateTeksTombol(selisih);
            }

            // Simpan data kelas yang dipilih
            SelectedTicketData.KelasDipilih = cbKelas.Text;
            SelectedTicketData.HargaKelas = tambahan;
        }

        // METHOD BARU: Update teks tombol berdasarkan selisih
        private void UpdateTeksTombol(decimal selisih)
        {
            if (isEditMode)
            {
                if (selisih > 0)
                {
                    btnPesan.Text = "KONFIRMASI PEMBAYARAN";
                    btnPesan.BackColor = Color.FromArgb(255, 150, 100); // Warna oranye untuk pembayaran
                }
                else
                {
                    btnPesan.Text = "KONFIRMASI PERUBAHAN";
                    btnPesan.BackColor = Color.FromArgb(255, 204, 153); // Warna kuning untuk perubahan biasa
                }
            }
            else
            {
                btnPesan.Text = "KONFIRMASI PEMBAYARAN";
                btnPesan.BackColor = Color.FromArgb(255, 204, 153);
            }
        }

        private void UpdateTextBoxHarga(string kategori, decimal harga, decimal total)
        {
            foreach (Control control in contentPanelBox.Controls)
            {
                if (control is TextBox textBox)
                {
                    if (textBox.Location.X == 360 && control.Parent != null)
                    {
                        var label = control.Parent.Controls.OfType<Label>()
                            .FirstOrDefault(l => l.Location.X == 50 && Math.Abs(l.Location.Y - textBox.Location.Y) < 5);

                        if (label != null && label.Text.Contains(kategori))
                        {
                            textBox.Text = $"Rp {harga:N0}";
                        }
                    }
                    else if (textBox.Location.X == 500 && control.Parent != null)
                    {
                        var label = control.Parent.Controls.OfType<Label>()
                            .FirstOrDefault(l => l.Location.X == 50 && Math.Abs(l.Location.Y - textBox.Location.Y) < 5);

                        if (label != null && label.Text.Contains(kategori))
                        {
                            textBox.Text = $"Rp {total:N0}";
                        }
                    }
                }
            }
        }

        private void BtnPesan_Click(object sender, EventArgs e)
        {
            if (!ValidasiDataPenumpang())
            {
                return;
            }
            if (cmbMetodePembayaran.SelectedItem == null)
            {
                MessageBox.Show("Pilih metode pembayaran terlebih dahulu!", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string metodePembayaran = cmbMetodePembayaran.SelectedItem.ToString();
            SelectedTicketData.MetodePembayaran = metodePembayaran;

            decimal totalAkhir = HitungTotalAkhir();

            if (isEditMode)
            {
                decimal selisih = totalAkhir - hargaAwal;
                if (selisih < 0) selisih = 0;

                // PESAN KONFIRMASI YANG LEBIH INFORMATIF
                string message = "";
                if (selisih > 0)
                {
                    message = $"Konfirmasi pembayaran selisih?\n\n" +
                             $"Harga sebelumnya: Rp {hargaAwal:N0}\n" +
                             $"Harga baru: Rp {totalAkhir:N0}\n" +
                             $"Selisih yang harus dibayar: Rp {selisih:N0}\n\n" +
                             $"Metode pembayaran: {metodePembayaran}";
                }
                else
                {
                    message = $"Konfirmasi perubahan tiket?\n\n" +
                             $"Tidak ada tambahan biaya.\n" +
                             $"Harga tetap: Rp {hargaAwal:N0}\n\n" +
                             $"Metode: {metodePembayaran}";
                }

                DialogResult result = MessageBox.Show(message,
                    selisih > 0 ? "Konfirmasi Pembayaran Selisih" : "Konfirmasi Perubahan Tiket",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    KonfirmasiPembayaranEditMode();
                }
            }
            else
            {
                DialogResult result = MessageBox.Show(
                    $"Konfirmasi pembayaran sebesar Rp {totalAkhir:N0} via {metodePembayaran}?",
                    "Konfirmasi Pembayaran",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    KonfirmasiPembayaranNormal();
                }
            }
        }

        private decimal HitungTotalAkhir()
        {
            if (!ValidasiDataPenumpang())
            {
                return 0;
            }
            dynamic r = isEditMode ? (dynamic)GetRouteDataFromBooking()
                       : (dynamic)SelectedTicketData.SelectedRute;

            decimal hargaDasar = r.Harga;
            decimal tambahan = SelectedTicketData.HargaKelas;

            decimal hargaDewasa = hargaDasar + tambahan;
            decimal hargaAnak = hargaDewasa * 0.75m;
            decimal hargaBayi = hargaDewasa * 0.25m;

            int jumlahDewasa = SelectedTicketData.Penumpang.Count(p => p.Kategori == "Dewasa");
            int jumlahAnak = SelectedTicketData.Penumpang.Count(p => p.Kategori == "Anak");
            int jumlahBayi = SelectedTicketData.Penumpang.Count(p => p.Kategori == "Bayi");

            return (hargaDewasa * jumlahDewasa) +
                   (hargaAnak * jumlahAnak) +
                   (hargaBayi * jumlahBayi);
        }


private void KonfirmasiPembayaranNormal()
        {
            decimal totalAkhir = HitungTotalAkhir();
            string metodePembayaran = cmbMetodePembayaran.SelectedItem.ToString();

            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    int bookingCode = GenerateBookingID();

                    // INSERT BOOKING BARU
                    string insertBooking = @"
                        INSERT INTO bookings 
                        (booking_id, user_id, schedule_id, total_price, payment_method, 
                         payment_status, selected_class, class_surcharge, created_at)
                        VALUES 
                        (@booking_id, @user_id, @schedule_id, @total_price, @payment_method, 
                         'paid', @selected_class, @class_surcharge, CURRENT_TIMESTAMP)
                        RETURNING booking_id;";

                    string insertPassengerSql = @"
                        INSERT INTO passengers (
                            booking_id, full_name, category, nik
                        ) VALUES (
                            @booking_id, @full_name, @category, @nik
                        );";

                    using (var cmd = new NpgsqlCommand(insertBooking, conn))
                    {
                        cmd.Parameters.AddWithValue("booking_id", bookingCode);
                        cmd.Parameters.AddWithValue("user_id", SelectedTicketData.UserID);
                        cmd.Parameters.AddWithValue("schedule_id", SelectedTicketData.SelectedRute.ScheduleID);
                        cmd.Parameters.AddWithValue("total_price", totalAkhir);
                        cmd.Parameters.AddWithValue("payment_method", metodePembayaran);
                        cmd.Parameters.AddWithValue("selected_class", SelectedTicketData.KelasDipilih);
                        cmd.Parameters.AddWithValue("class_surcharge", SelectedTicketData.HargaKelas);

                        int bookingId = Convert.ToInt32(cmd.ExecuteScalar());

                        // UPDATE AVAILABLE SEATS
                        string updateSeats = @"
                            UPDATE schedules 
                            SET available_seats = available_seats - @penumpang_count
                            WHERE schedule_id = @schedule_id;";

                        using (var cmdSeats = new NpgsqlCommand(updateSeats, conn))
                        {
                            cmdSeats.Parameters.AddWithValue("penumpang_count", SelectedTicketData.Penumpang.Count);
                            cmdSeats.Parameters.AddWithValue("schedule_id", SelectedTicketData.SelectedRute.ScheduleID);
                            cmdSeats.ExecuteNonQuery();
                        }

                        // INSERT PENUMPANG
                        using (var trans = conn.BeginTransaction())
                        {
                            try
                            {
                                foreach (var p in SelectedTicketData.Penumpang)
                                {
                                    using (var insertP = new NpgsqlCommand(insertPassengerSql, conn, trans))
                                    {
                                        insertP.Parameters.AddWithValue("booking_id", bookingId);
                                        insertP.Parameters.AddWithValue("full_name", p.Nama);
                                        insertP.Parameters.AddWithValue("category", p.Kategori);
                                        insertP.Parameters.AddWithValue("nik", p.NIK ?? "");

                                        insertP.ExecuteNonQuery();
                                    }
                                }

                                trans.Commit();
                            }
                            catch
                            {
                                trans.Rollback();
                                throw;
                            }
                        }

                        MessageBox.Show("Pembayaran berhasil! Tiket Anda telah dikonfirmasi.",
                                        "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.Hide();
                        new UserSuccess(bookingId).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal memproses pembayaran:\n{ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BackupDataSebelumKembali()
        {
            try
            {
                // Pastikan data penumpang tersimpan di SelectedTicketData
                if (SelectedTicketData.TiketReschedule != null && SelectedTicketData.Penumpang != null)
                {
                    SelectedTicketData.TiketReschedule.Penumpang = new List<PenumpangData>(SelectedTicketData.Penumpang);
                }

                // Simpan juga di UpdatedBookingData jika ada
                if (SelectedTicketData.UpdatedBookingData != null && SelectedTicketData.Penumpang != null)
                {
                    // Anda bisa menambahkan logika penyimpanan tambahan di sini jika diperlukan
                }

                // Debug: Tampilkan jumlah penumpang yang disimpan
                Console.WriteLine($"Data disimpan - Jumlah penumpang: {SelectedTicketData.Penumpang?.Count ?? 0}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error backup data: {ex.Message}");
            }
        }

        // METHOD BARU: Validasi data penumpang
        private bool ValidasiDataPenumpang()
        {
            if (SelectedTicketData.Penumpang == null || SelectedTicketData.Penumpang.Count == 0)
            {
                if (isEditMode && SelectedTicketData.TiketReschedule != null)
                {
                    // Coba pulihkan dari TiketReschedule
                    SelectedTicketData.Penumpang = new List<PenumpangData>(SelectedTicketData.TiketReschedule.Penumpang);
                }

                if (SelectedTicketData.Penumpang == null || SelectedTicketData.Penumpang.Count == 0)
                {
                    MessageBox.Show("Data penumpang tidak ditemukan. Silakan ulangi proses edit.", "Data Tidak Valid",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            return true;
        }



        private void KonfirmasiPembayaranEditMode()
        {
            decimal totalAkhir = HitungTotalAkhir();
            string metodePembayaran = cmbMetodePembayaran.SelectedItem.ToString();

            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    // Hitung selisih harga
                    decimal selisih = totalAkhir - hargaAwal;
                    if (selisih < 0) selisih = 0;

                    // UPDATE BOOKING UNTUK EDIT
                    string updateBooking = @"
                        UPDATE bookings 
                        SET schedule_id = @schedule_id,
                            total_price = @total_price,
                            selected_class = @selected_class,
                            class_surcharge = @class_surcharge,
                            additional_payment = @additional_payment,
                            payment_method = @payment_method,
                            updated_date = CURRENT_TIMESTAMP
                        WHERE booking_id = @booking_id";

                    using (var cmd = new NpgsqlCommand(updateBooking, conn))
                    {
                        cmd.Parameters.AddWithValue("@schedule_id", SelectedTicketData.UpdatedBookingData.ScheduleId);
                        cmd.Parameters.AddWithValue("@total_price", totalAkhir);
                        cmd.Parameters.AddWithValue("@selected_class", SelectedTicketData.KelasDipilih);
                        cmd.Parameters.AddWithValue("@class_surcharge", SelectedTicketData.HargaKelas);
                        cmd.Parameters.AddWithValue("@additional_payment", selisih);
                        cmd.Parameters.AddWithValue("@payment_method", metodePembayaran);
                        cmd.Parameters.AddWithValue("@booking_id", bookingId);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Update data penumpang
                            UpdatePenumpangEditMode(conn);

                            string successMessage = selisih > 0 ?
                                $"Perubahan tiket dan Pembayaran berhasil!\n\nTiket Anda telah dikonfirmasi." :
                                "Perubahan tiket berhasil!\n\nTidak ada tambahan biaya.";

                            MessageBox.Show(successMessage, "Sukses",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Reset additional_payment jika sudah dibayar
                            if (selisih > 0)
                            {
                                ResetAdditionalPayment(conn);
                            }


                            this.Hide();
                            new UserSuccess(bookingId).Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal memproses perubahan: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdatePenumpangEditMode(NpgsqlConnection conn)
        {
            // Hapus penumpang lama
            string deleteSql = "DELETE FROM passengers WHERE booking_id = @booking_id";
            using (var cmdDelete = new NpgsqlCommand(deleteSql, conn))
            {
                cmdDelete.Parameters.AddWithValue("@booking_id", bookingId);
                cmdDelete.ExecuteNonQuery();
            }

            // Insert penumpang baru
            string insertSql = @"
                INSERT INTO passengers (booking_id, full_name, category, nik) 
                VALUES (@booking_id, @full_name, @category, @nik)";

            foreach (var penumpang in SelectedTicketData.Penumpang)
            {
                using (var cmdInsert = new NpgsqlCommand(insertSql, conn))
                {
                    cmdInsert.Parameters.AddWithValue("@booking_id", bookingId);
                    cmdInsert.Parameters.AddWithValue("@full_name", penumpang.Nama);
                    cmdInsert.Parameters.AddWithValue("@category", penumpang.Kategori);
                    cmdInsert.Parameters.AddWithValue("@nik", penumpang.NIK);
                    cmdInsert.ExecuteNonQuery();
                }
            }
        }

        private void ResetAdditionalPayment(NpgsqlConnection conn)
        {
            // Reset additional_payment ke 0 setelah pembayaran
            string resetSql = "UPDATE bookings SET additional_payment = 0 WHERE booking_id = @booking_id";
            using (var cmdReset = new NpgsqlCommand(resetSql, conn))
            {
                cmdReset.Parameters.AddWithValue("@booking_id", bookingId);
                cmdReset.ExecuteNonQuery();
            }
        }

        private int GenerateBookingID()
        {
            Random rnd = new Random();
            int bookingId;
            bool isUnique = false;

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                while (!isUnique)
                {
                    bookingId = rnd.Next(100000, 999999);
                    string checkSql = "SELECT COUNT(*) FROM bookings WHERE booking_id = @id";
                    using (var cmd = new NpgsqlCommand(checkSql, conn))
                    {
                        cmd.Parameters.AddWithValue("id", bookingId);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        if (count == 0)
                        {
                            isUnique = true;
                            return bookingId;
                        }
                    }
                }
            }
            return 0;
        }

        private Image SetImageOpacity(Image image, float opacity)
        {
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