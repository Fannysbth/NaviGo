using Npgsql;
using SendGrid.Helpers.Mail;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace UI_NaviGO
{
    public partial class FormEditTicket : Form
    {
        private int bookingId;
        private int oldScheduleId;
        private decimal oldPrice;

        private Panel sidebarPanel;
        private Panel topPanel;
        private Panel mainContentPanel;
        private Panel contentPanelBox;

        // Property untuk mengembalikan data yang diupdate
        public BookingDataa UpdatedBookingData { get; private set; }

        public FormEditTicket(int bookingId)
        {
            InitializeComponent();
            this.bookingId = bookingId;
            this.UpdatedBookingData = new BookingDataa();

            BuildUI();
            LoadOldTicketData();
            LoadNewSchedules();
        }

        Panel panelDetail;
        DataGridView dgvSchedule;

        // ==================== BUILD UI ====================
        private void BuildUI()
        {
            this.Text = "NaviGo - Edit Tiket";
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
                Text = "Edit Informasi Tiket",
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

            // ================== MAIN CONTENT PANEL ==================
            mainContentPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackgroundImageLayout = ImageLayout.Stretch
            };

            mainContentPanel.BackgroundImage = SetImageOpacity(Properties.Resources.tiket_bg, 0.3f);
            // White card di tengah
            contentPanelBox = new Panel()
            {
                Size = new Size(900, 600),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(20)
            };

            Label lblTitle = new Label()
            {
                Text = "Edit Booking Ticket",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(280, 20),
                ForeColor = Color.FromArgb(0, 85, 92)
            };
            contentPanelBox.Controls.Add(lblTitle);

            // PANEL DETAIL TICKET LAMA
            panelDetail = new Panel()
            {
                Location = new Point(50, 80),
                Size = new Size(800, 100),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(245, 245, 245)
            };
            contentPanelBox.Controls.Add(panelDetail);

            Label lblJadwalBaru = new Label()
            {
                Text = "Pilih Jadwal Baru",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(50, 200),
                ForeColor = Color.FromArgb(0, 85, 92)
            };
            contentPanelBox.Controls.Add(lblJadwalBaru);

            // DATAGRID JADWAL BARU
            dgvSchedule = new DataGridView()
            {
                Location = new Point(50, 240),
                Size = new Size(800, 250),
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White
            };

            dgvSchedule.Columns.Add("ID", "Schedule ID");
            dgvSchedule.Columns.Add("Kapal", "Kapal");
            dgvSchedule.Columns.Add("Rute", "Rute");
            dgvSchedule.Columns.Add("Tanggal", "Tanggal");
            dgvSchedule.Columns.Add("Berangkat", "Berangkat");
            dgvSchedule.Columns.Add("Tiba", "Tiba");
            dgvSchedule.Columns.Add("Harga", "Harga");

            DataGridViewButtonColumn pilihBtn = new DataGridViewButtonColumn()
            {
                HeaderText = "Aksi",
                Text = "Pilih",
                UseColumnTextForButtonValue = true,
                Name = "colPilih"
            };
            dgvSchedule.Columns.Add(pilihBtn);

            dgvSchedule.CellClick += DgvSchedule_CellClick;

            contentPanelBox.Controls.Add(dgvSchedule);

            // Tombol Batal
            Button btnCancel = new Button()
            {
                Text = "Batal",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(120, 40),
                Location = new Point(650, 510),
                BackColor = Color.FromArgb(255, 150, 150),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += BtnCancel_Click;
            contentPanelBox.Controls.Add(btnCancel);

            mainContentPanel.Controls.Add(contentPanelBox);
            this.Controls.Add(mainContentPanel);
            this.Controls.Add(topPanel);
            this.Controls.Add(sidebarPanel);

            // Center the card
            mainContentPanel.Resize += (s, e) =>
            {
                contentPanelBox.Location = new Point(
                    (mainContentPanel.ClientSize.Width - contentPanelBox.Width) / 2,
                    Math.Max(20, (mainContentPanel.ClientSize.Height - contentPanelBox.Height) / 2)
                );
            };
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // ==================== LOAD DATA BOOKING LAMA ====================
        private void LoadOldTicketData()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                        SELECT b.booking_id, b.schedule_id, s.departure_date,
                               s.departure_time, sh.ship_name,
                               r.departure_city, r.arrival_city, r.route_name,
                               r.base_price
                        FROM bookings b
                        JOIN schedules s ON b.schedule_id = s.schedule_id
                        JOIN ships sh ON s.ship_id = sh.ship_id
                        JOIN routes r ON s.route_id = r.route_id
                        WHERE b.booking_id = @id
                    ";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", bookingId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                oldScheduleId = Convert.ToInt32(reader["schedule_id"]);
                                oldPrice = Convert.ToDecimal(reader["base_price"]);

                                // Tampilkan detail di panel
                                panelDetail.Controls.Clear();

                                panelDetail.Controls.Add(CreateLabel($"Booking ID : {bookingId}", 20, 15));
                                panelDetail.Controls.Add(CreateLabel($"Kapal : {reader["ship_name"]}", 20, 40));
                                panelDetail.Controls.Add(CreateLabel(
                                    $"Rute : {reader["departure_city"]} - {reader["arrival_city"]}", 20, 65));
                                panelDetail.Controls.Add(CreateLabel(
                                    $"Tanggal : {Convert.ToDateTime(reader["departure_date"]).ToString("dd MMM yyyy")}", 400, 15));
                                panelDetail.Controls.Add(CreateLabel($"Berangkat : {reader["departure_time"]}", 400, 40));
                                panelDetail.Controls.Add(CreateLabel($"Harga Lama : Rp {oldPrice:N0}", 400, 65));

                                // Simpan data lama ke UpdatedBookingData
                                UpdatedBookingData.BookingId = bookingId;
                                UpdatedBookingData.ScheduleId = oldScheduleId;
                                UpdatedBookingData.ShipName = reader["ship_name"].ToString();
                                UpdatedBookingData.DepartureCity = reader["departure_city"].ToString();
                                UpdatedBookingData.ArrivalCity = reader["arrival_city"].ToString();
                                UpdatedBookingData.RouteName = reader["route_name"].ToString();
                                UpdatedBookingData.DepartureDate = Convert.ToDateTime(reader["departure_date"]);
                                UpdatedBookingData.DepartureTime = (TimeSpan)reader["departure_time"];
                                UpdatedBookingData.TotalPrice = oldPrice;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data ticket lama: " + ex.Message);
            }
        }

        private Label CreateLabel(string text, int x, int y)
        {
            return new Label()
            {
                Text = text,
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Location = new Point(x, y)
            };
        }

        // ==================== LOAD JADWAL BARU (Kecuali jadwal lama) ====================
        private void LoadNewSchedules()
        {
            dgvSchedule.Rows.Clear();

            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                        SELECT s.schedule_id, sh.ship_name, sh.ship_id,
                               r.route_name, r.departure_city, r.arrival_city,
                               s.departure_date, s.departure_time,
                               s.arrival_time, r.base_price
                        FROM schedules s
                        JOIN ships sh ON s.ship_id = sh.ship_id
                        JOIN routes r ON s.route_id = r.route_id
                        WHERE s.departure_date >= CURRENT_DATE
                AND s.schedule_id != @old
                        ORDER BY s.departure_date ASC
                    ";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@old", oldScheduleId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                dgvSchedule.Rows.Add(
                                    reader["schedule_id"],
                                    reader["ship_name"],
                                    $"{reader["departure_city"]} - {reader["arrival_city"]}",
                                    Convert.ToDateTime(reader["departure_date"]).ToString("dd MMM yyyy"),
                                    reader["departure_time"],
                                    reader["arrival_time"],
                                    $"Rp {Convert.ToDecimal(reader["base_price"]):N0}"
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat jadwal baru: " + ex.Message);
            }
        }

        // ==================== PILIH JADWAL BARU ====================
        private void DgvSchedule_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvSchedule.Columns["colPilih"].Index)
            {
                int newScheduleId = Convert.ToInt32(dgvSchedule.Rows[e.RowIndex].Cells[0].Value);

                // Ambil data lengkap jadwal baru
                var newScheduleData = GetScheduleData(newScheduleId);

                if (newScheduleData != null)
                {
                    decimal newPrice = newScheduleData.BasePrice;

                    // Hitung selisih (hanya tambahan, tidak ada refund)
                    decimal selisih = newPrice - oldPrice;
                    if (selisih < 0) selisih = 0;

                    DialogResult ask = MessageBox.Show(
                        $"Yakin ingin mengubah jadwal?\n\n" +
                        $"Jadwal Baru:\n" +
                        $"Kapal: {newScheduleData.ShipName}\n" +
                        $"Rute: {newScheduleData.DepartureCity} - {newScheduleData.ArrivalCity}\n" +
                        $"Tanggal: {newScheduleData.DepartureDate:dd MMM yyyy}\n" +
                        $"Berangkat: {newScheduleData.DepartureTime:hh\\:mm}\n\n" +
                        $"Tambahan biaya: Rp {selisih:N0}",
                        "Konfirmasi",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (ask == DialogResult.Yes)
                    {
                        // Update data sementara
                        UpdatedBookingData.ScheduleId = newScheduleId;
                        UpdatedBookingData.ShipName = newScheduleData.ShipName;
                        UpdatedBookingData.ShipId = newScheduleData.ShipId;
                        UpdatedBookingData.DepartureCity = newScheduleData.DepartureCity;
                        UpdatedBookingData.ArrivalCity = newScheduleData.ArrivalCity;
                        UpdatedBookingData.RouteName = newScheduleData.RouteName;
                        UpdatedBookingData.DepartureDate = newScheduleData.DepartureDate;
                        UpdatedBookingData.DepartureTime = newScheduleData.DepartureTime;
                        UpdatedBookingData.TotalPrice = newPrice;

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
        }

        private ScheduleData GetScheduleData(int scheduleId)
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string sql = @"
                        SELECT s.schedule_id, sh.ship_name, sh.ship_id,
                               r.route_name, r.departure_city, r.arrival_city,
                               s.departure_date, s.departure_time,
                               s.arrival_time, r.base_price
                        FROM schedules s
                        JOIN ships sh ON s.ship_id = sh.ship_id
                        JOIN routes r ON s.route_id = r.route_id
                        WHERE s.schedule_id = @id";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", scheduleId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new ScheduleData
                                {
                                    ScheduleId = scheduleId,
                                    ShipName = reader["ship_name"].ToString(),
                                    ShipId = reader["ship_id"].ToString(),
                                    RouteName = reader["route_name"].ToString(),
                                    DepartureCity = reader["departure_city"].ToString(),
                                    ArrivalCity = reader["arrival_city"].ToString(),
                                    DepartureDate = Convert.ToDateTime(reader["departure_date"]),
                                    DepartureTime = (TimeSpan)reader["departure_time"],
                                    ArrivalTime = (TimeSpan)reader["arrival_time"],
                                    BasePrice = Convert.ToDecimal(reader["base_price"])
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal mengambil data jadwal: " + ex.Message);
            }
            return null;
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

    // Class untuk data jadwal
    public class ScheduleData
    {
        public int ScheduleId { get; set; }
        public string ShipName { get; set; }
        public string ShipId { get; set; }
        public string RouteName { get; set; }
        public string DepartureCity { get; set; }
        public string ArrivalCity { get; set; }
        public DateTime DepartureDate { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public decimal BasePrice { get; set; }
    }

}