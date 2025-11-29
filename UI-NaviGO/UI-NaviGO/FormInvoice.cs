using Npgsql;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace UI_NaviGO
{
    public partial class FormInvoice : Form
    {
        private int bookingId;
        private bool isEditMode;
        private decimal hargaAwal;
        private decimal hargaAkhir;
        private decimal selisihHarga;
        private string metodePembayaran;
        private Panel mainPanel;
        private Panel contentPanel;

        public FormInvoice(int bookingId, bool isEditMode = false, decimal hargaAwal = 0, decimal hargaAkhir = 0, decimal selisihHarga = 0)
        {
            this.bookingId = bookingId;
            this.isEditMode = isEditMode;
            this.hargaAwal = hargaAwal;
            this.hargaAkhir = hargaAkhir;
            this.selisihHarga = selisihHarga;

            InitializeComponent();
            BuildUI();
            LoadInvoiceData();
        }

        private void BuildUI()
        {
            this.Text = isEditMode ? "Invoice Perubahan Tiket" : "Invoice Tiket";
            this.Size = new Size(900, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Main panel dengan scroll
            mainPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(245, 245, 245)
            };

            // Content panel
            contentPanel = new Panel()
            {
                Size = new Size(800, 1000),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(20)
            };

            mainPanel.Controls.Add(contentPanel);
            this.Controls.Add(mainPanel);

            // Center content panel
            mainPanel.Resize += (s, e) =>
            {
                contentPanel.Location = new Point(
                    (mainPanel.ClientSize.Width - contentPanel.Width) / 2,
                    20
                );
            };
        }

        private void LoadInvoiceData()
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
    b.class_surcharge,
    s.departure_date,
    s.departure_time,
    r.route_name,
    r.departure_city,
    r.arrival_city,
    r.base_price,
    sh.ship_name,
    sh.ship_id,
    u.name as customer_name,
    u.email as customer_email,
    p.full_name,
    p.category,
    p.nik
FROM bookings b
JOIN schedules s ON b.schedule_id = s.schedule_id
JOIN routes r ON s.route_id = r.route_id
JOIN ships sh ON s.ship_id = sh.ship_id
JOIN users u ON b.user_id = u.user_id
LEFT JOIN passengers p ON b.booking_id = p.booking_id
WHERE b.booking_id = @bookingId";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@bookingId", bookingId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            bool firstRow = true;
                            var penumpangList = new List<PenumpangData>();
                            string customerName = "";
                            string customerEmail = "";
                            string routeInfo = "";
                            string shipInfo = "";
                            DateTime departureDate = DateTime.MinValue;
                            TimeSpan departureTime = TimeSpan.Zero;
                            string selectedClass = "";
                            decimal classSurcharge = 0;
                            decimal basePrice = 0;
                            DateTime bookingDate = DateTime.MinValue;
                            string paymentStatus = "";
                            string paymentMethod = "";

                            while (reader.Read())
                            {
                                if (firstRow)
                                {
                                    customerName = reader.GetString(reader.GetOrdinal("customer_name"));
                                    customerEmail = reader.GetString(reader.GetOrdinal("customer_email"));
                                    routeInfo = $"{reader.GetString(reader.GetOrdinal("departure_city"))} - {reader.GetString(reader.GetOrdinal("arrival_city"))}";
                                    shipInfo = reader.GetString(reader.GetOrdinal("ship_name"));
                                    departureDate = reader.GetDateTime(reader.GetOrdinal("departure_date"));
                                    departureTime = reader.GetTimeSpan(reader.GetOrdinal("departure_time"));
                                    selectedClass = reader.GetString(reader.GetOrdinal("selected_class"));
                                    classSurcharge = reader.GetDecimal(reader.GetOrdinal("class_surcharge"));
                                    basePrice = reader.GetDecimal(reader.GetOrdinal("base_price"));
                                    bookingDate = reader.GetDateTime(reader.GetOrdinal("booking_date"));
                                    paymentStatus = reader.GetString(reader.GetOrdinal("payment_status"));
                                    paymentMethod = reader.IsDBNull(reader.GetOrdinal("payment_method")) ?
                                        "Belum dibayar" : reader.GetString(reader.GetOrdinal("payment_method"));

                                    firstRow = false;
                                }

                                if (!reader.IsDBNull(reader.GetOrdinal("full_name")))
                                {
                                    penumpangList.Add(new PenumpangData
                                    {
                                        Nama = reader.GetString(reader.GetOrdinal("full_name")),
                                        Kategori = reader.GetString(reader.GetOrdinal("category")),
                                        NIK = reader.GetString(reader.GetOrdinal("nik"))
                                    });
                                }
                            }

                            // Update UI dengan data yang diambil
                            UpdateInvoiceUI(customerName, customerEmail, routeInfo, shipInfo,
                                          departureDate, departureTime, selectedClass,
                                          classSurcharge, basePrice, bookingDate,
                                          paymentStatus, paymentMethod, penumpangList);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading invoice data: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateInvoiceUI(string customerName, string customerEmail, string routeInfo,
                                   string shipInfo, DateTime departureDate, TimeSpan departureTime,
                                   string selectedClass, decimal classSurcharge, decimal basePrice,
                                   DateTime bookingDate, string paymentStatus, string paymentMethod,
                                   List<PenumpangData> penumpangList)
        {
            contentPanel.Controls.Clear();

            int currentY = 20;

            // Header
            Label lblTitle = new Label()
            {
                Text = isEditMode ? "INVOICE PERUBAHAN TIKET" : "INVOICE TIKET KAPAL",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 85, 92),
                AutoSize = true,
                Location = new Point(300, currentY)
            };
            contentPanel.Controls.Add(lblTitle);

            currentY += 50;

            // Informasi Customer
            Panel panelCustomer = new Panel()
            {
                Location = new Point(50, currentY),
                Size = new Size(700, 80),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(250, 250, 250)
            };

            Label lblCustomerInfo = new Label()
            {
                Text = "Informasi Pelanggan",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            Label lblCustomerName = new Label()
            {
                Text = $"Nama: {customerName}",
                Font = new Font("Segoe UI", 10),
                Location = new Point(10, 35),
                AutoSize = true
            };

            Label lblCustomerEmail = new Label()
            {
                Text = $"Email: {customerEmail}",
                Font = new Font("Segoe UI", 10),
                Location = new Point(10, 55),
                AutoSize = true
            };

            Label lblBookingId = new Label()
            {
                Text = $"ID Booking: {bookingId}",
                Font = new Font("Segoe UI", 10),
                Location = new Point(350, 35),
                AutoSize = true
            };

            Label lblBookingDate = new Label()
            {
                Text = $"Tanggal Booking: {bookingDate:dd MMMM yyyy}",
                Font = new Font("Segoe UI", 10),
                Location = new Point(350, 55),
                AutoSize = true
            };

            panelCustomer.Controls.AddRange(new Control[] { lblCustomerInfo, lblCustomerName, lblCustomerEmail, lblBookingId, lblBookingDate });
            contentPanel.Controls.Add(panelCustomer);

            currentY += 100;

            // Informasi Perjalanan
            Panel panelJourney = new Panel()
            {
                Location = new Point(50, currentY),
                Size = new Size(700, 100),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(250, 250, 250)
            };

            Label lblJourneyInfo = new Label()
            {
                Text = "Informasi Perjalanan",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            Label lblRoute = new Label()
            {
                Text = $"Rute: {routeInfo}",
                Font = new Font("Segoe UI", 10),
                Location = new Point(10, 35),
                AutoSize = true
            };

            Label lblShip = new Label()
            {
                Text = $"Kapal: {shipInfo}",
                Font = new Font("Segoe UI", 10),
                Location = new Point(10, 55),
                AutoSize = true
            };

            Label lblDeparture = new Label()
            {
                Text = $"Keberangkatan: {departureDate:dd MMMM yyyy} - {departureTime:hh\\:mm}",
                Font = new Font("Segoe UI", 10),
                Location = new Point(10, 75),
                AutoSize = true
            };

            Label lblClass = new Label()
            {
                Text = $"Kelas: {selectedClass}",
                Font = new Font("Segoe UI", 10),
                Location = new Point(350, 35),
                AutoSize = true
            };

            panelJourney.Controls.AddRange(new Control[] { lblJourneyInfo, lblRoute, lblShip, lblDeparture, lblClass });
            contentPanel.Controls.Add(panelJourney);

            currentY += 120;

            // Detail Penumpang
            Label lblPassengerTitle = new Label()
            {
                Text = "Detail Penumpang",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(50, currentY),
                AutoSize = true
            };
            contentPanel.Controls.Add(lblPassengerTitle);

            currentY += 30;

            DataGridView dgvPenumpang = new DataGridView()
            {
                Location = new Point(50, currentY),
                Size = new Size(700, 150),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            dgvPenumpang.Columns.Add("No", "No");
            dgvPenumpang.Columns.Add("Nama", "Nama Penumpang");
            dgvPenumpang.Columns.Add("Kategori", "Kategori");
            dgvPenumpang.Columns.Add("NIK", "NIK");

            for (int i = 0; i < penumpangList.Count; i++)
            {
                dgvPenumpang.Rows.Add(i + 1, penumpangList[i].Nama, penumpangList[i].Kategori, penumpangList[i].NIK);
            }

            contentPanel.Controls.Add(dgvPenumpang);

            currentY += 180;

            // Detail Harga
            Label lblPriceTitle = new Label()
            {
                Text = "Rincian Harga",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(50, currentY),
                AutoSize = true
            };
            contentPanel.Controls.Add(lblPriceTitle);

            currentY += 30;

            Panel panelPrice = new Panel()
            {
                Location = new Point(50, currentY),
                Size = new Size(700, isEditMode ? 200 : 150),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(250, 250, 250)
            };

            int priceY = 10;

            if (isEditMode)
            {
                // Tampilkan perbandingan harga untuk mode edit
                Label lblHargaLama = new Label()
                {
                    Text = $"Harga Sebelumnya: Rp {hargaAwal:N0}",
                    Font = new Font("Segoe UI", 10),
                    Location = new Point(20, priceY),
                    AutoSize = true
                };
                panelPrice.Controls.Add(lblHargaLama);
                priceY += 25;

                Label lblHargaBaru = new Label()
                {
                    Text = $"Harga Setelah Perubahan: Rp {hargaAkhir:N0}",
                    Font = new Font("Segoe UI", 10),
                    Location = new Point(20, priceY),
                    AutoSize = true
                };
                panelPrice.Controls.Add(lblHargaBaru);
                priceY += 25;

                // Garis pemisah
                Panel garis = new Panel()
                {
                    BackColor = Color.LightGray,
                    Location = new Point(20, priceY),
                    Size = new Size(660, 1)
                };
                panelPrice.Controls.Add(garis);
                priceY += 10;

                Label lblSelisih = new Label()
                {
                    Text = $"Selisih Harga: Rp {selisihHarga:N0}",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    ForeColor = selisihHarga >= 0 ? Color.Green : Color.Red,
                    Location = new Point(20, priceY),
                    AutoSize = true
                };
                panelPrice.Controls.Add(lblSelisih);
                priceY += 25;

                if (selisihHarga > 0)
                {
                    Label lblInfoTambahan = new Label()
                    {
                        Text = "Anda perlu membayar tambahan biaya di atas",
                        Font = new Font("Segoe UI", 9, FontStyle.Italic),
                        ForeColor = Color.OrangeRed,
                        Location = new Point(20, priceY),
                        AutoSize = true
                    };
                    panelPrice.Controls.Add(lblInfoTambahan);
                    priceY += 20;
                }
                else if (selisihHarga < 0)
                {
                    Label lblInfoPengembalian = new Label()
                    {
                        Text = "Selisih akan dikembalikan melalui metode pembayaran yang sama",
                        Font = new Font("Segoe UI", 9, FontStyle.Italic),
                        ForeColor = Color.Green,
                        Location = new Point(20, priceY),
                        AutoSize = true
                    };
                    panelPrice.Controls.Add(lblInfoPengembalian);
                    priceY += 20;
                }
            }

            // Hitung total berdasarkan penumpang
            int jumlahDewasa = penumpangList.Count(p => p.Kategori == "Dewasa");
            int jumlahAnak = penumpangList.Count(p => p.Kategori == "Anak");
            int jumlahBayi = penumpangList.Count(p => p.Kategori == "Bayi");

            decimal hargaDewasa = basePrice + classSurcharge;
            decimal hargaAnak = hargaDewasa * 0.75m;
            decimal hargaBayi = hargaDewasa * 0.25m;

            decimal totalDewasa = hargaDewasa * jumlahDewasa;
            decimal totalAnak = hargaAnak * jumlahAnak;
            decimal totalBayi = hargaBayi * jumlahBayi;
            decimal totalHarga = totalDewasa + totalAnak + totalBayi;

            Label lblBasePrice = new Label()
            {
                Text = $"Harga Dasar: Rp {basePrice:N0}",
                Font = new Font("Segoe UI", 10),
                Location = new Point(20, priceY),
                AutoSize = true
            };
            panelPrice.Controls.Add(lblBasePrice);
            priceY += 25;

            if (classSurcharge > 0)
            {
                Label lblSurcharge = new Label()
                {
                    Text = $"Tambahan Kelas {selectedClass}: Rp {classSurcharge:N0}",
                    Font = new Font("Segoe UI", 10),
                    Location = new Point(20, priceY),
                    AutoSize = true
                };
                panelPrice.Controls.Add(lblSurcharge);
                priceY += 25;
            }

            Label lblTotal = new Label()
            {
                Text = $"Total Harga: Rp {totalHarga:N0}",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 85, 92),
                Location = new Point(20, priceY),
                AutoSize = true
            };
            panelPrice.Controls.Add(lblTotal);

            contentPanel.Controls.Add(panelPrice);

            currentY += (isEditMode ? 210 : 160);

            // Informasi Pembayaran
            Panel panelPayment = new Panel()
            {
                Location = new Point(50, currentY),
                Size = new Size(700, 80),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(250, 250, 250)
            };

            Label lblPaymentInfo = new Label()
            {
                Text = "Informasi Pembayaran",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            Label lblPaymentMethod = new Label()
            {
                Text = $"Metode Pembayaran: {paymentMethod}",
                Font = new Font("Segoe UI", 10),
                Location = new Point(10, 35),
                AutoSize = true
            };

            Label lblPaymentStatus = new Label()
            {
                Text = $"Status: {paymentStatus}",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = GetStatusColor(paymentStatus),
                Location = new Point(10, 55),
                AutoSize = true
            };

            panelPayment.Controls.AddRange(new Control[] { lblPaymentInfo, lblPaymentMethod, lblPaymentStatus });
            contentPanel.Controls.Add(panelPayment);

            currentY += 100;

            // Tombol
            Button btnClose = new Button()
            {
                Text = "Tutup",
                Size = new Size(120, 40),
                Location = new Point(390, currentY),
                BackColor = Color.FromArgb(0, 85, 92),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();

            Button btnPrint = new Button()
            {
                Text = "Cetak Invoice",
                Size = new Size(120, 40),
                Location = new Point(260, currentY),
                BackColor = Color.FromArgb(255, 204, 153),
                ForeColor = Color.FromArgb(0, 102, 102),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnPrint.FlatAppearance.BorderSize = 0;
            btnPrint.Click += (s, e) => CetakInvoice();

            contentPanel.Controls.Add(btnPrint);
            contentPanel.Controls.Add(btnClose);

            // Update height content panel
            contentPanel.Height = currentY + 80;
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

        private void CetakInvoice()
        {
            MessageBox.Show("Fitur cetak invoice akan segera tersedia!", "Informasi",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}