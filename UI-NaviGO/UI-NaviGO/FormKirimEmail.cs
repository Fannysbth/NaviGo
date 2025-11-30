using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SendGrid;
using SendGrid.Helpers.Mail;
using DotNetEnv;
using Npgsql;
using System.Collections.Generic;
using Font = System.Drawing.Font;


namespace UI_NaviGO
{
    public partial class FormKirimEmail : Form
    {
        private TextBox txtEmail;
        private Button btnKirim;
        private Button btnBatal;

        private int bookingId;
        private BookingData SelectedTicketData;

        public FormKirimEmail(int bookingId)
        {
            //string envPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", ".env");
            //DotNetEnv.Env.Load(envPath);
            DotNetEnv.Env.Load();
            this.bookingId = bookingId;
            InitializeComponent();
            BuildUI();
            LoadBookingData();
        }

        private void BuildUI()
        {
            this.Text = "Kirim E-Ticket via Email";
            this.Size = new Size(450, 250);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            Label lblTitle = new Label()
            {
                Text = "Kirim E-Ticket ke Email",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 85, 92),
                AutoSize = true,
                Location = new Point(120, 20)
            };

            Label lblEmail = new Label()
            {
                Text = "Alamat Email:",
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Location = new Point(50, 70)
            };

            txtEmail = new TextBox()
            {
                Location = new Point(150, 68),
                Size = new Size(250, 25),
                Font = new Font("Segoe UI", 10)
            };

            btnKirim = new Button()
            {
                Text = "Kirim E-Ticket",
                BackColor = Color.FromArgb(0, 120, 100),
                ForeColor = Color.White,
                Size = new Size(120, 35),
                Location = new Point(150, 120),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnKirim.Click += BtnKirim_Click;

            btnBatal = new Button()
            {
                Text = "Batal",
                BackColor = Color.FromArgb(200, 200, 200),
                ForeColor = Color.Black,
                Size = new Size(120, 35),
                Location = new Point(280, 120),
                Font = new Font("Segoe UI", 10)
            };
            btnBatal.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { lblTitle, lblEmail, txtEmail, btnKirim, btnBatal });
        }

        private void LoadBookingData()
        {
            try
            {
                using (var conn = new NpgsqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();

                    // Ambil data booking + user
                    string sqlBooking = @"
                SELECT b.booking_id, b.user_id, b.schedule_id, b.selected_class, b.total_price, b.payment_method,
                       b.booking_reference, b.class_surcharge,
                       u.name, u.email, b.payment_status
                FROM bookings b
                JOIN users u ON u.user_id = b.user_id
                WHERE b.booking_id = @bookingId";

                    using (var cmd = new NpgsqlCommand(sqlBooking, conn))
                    {
                        cmd.Parameters.AddWithValue("bookingId", bookingId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                SelectedTicketData = new BookingData
                                {
                                    BookingId = reader.GetInt32(0),
                                    ScheduleId = reader.GetInt32(2),
                                    SelectedClass = reader.GetString(3),
                                    TotalPrice = reader.GetDecimal(4),
                                    PaymentMethod = reader.IsDBNull(5) ? "" : reader.GetString(5),
                                    BookingReference = reader.IsDBNull(6) ? "" : reader.GetString(6),
                                    ClassSurcharge = reader.IsDBNull(7) ? 0 : reader.GetDecimal(7),
                                    Username = reader.GetString(8),
                                    PaymentStatus = reader.GetString(10),
                                    UserEmail = reader.GetString(9),
                                    Penumpang = new List<PassengerData>()
                                };
                                txtEmail.Text = SelectedTicketData.UserEmail;
                            }
                        }
                    }

                    // Ambil schedule + rute + kapal
                    string sqlSchedule = @"
                SELECT s.departure_date, s.departure_time, s.arrival_time,
                       r.route_name, r.departure_city, r.arrival_city, r.transit_info, r.base_price,
                       sh.ship_name
                FROM schedules s
                JOIN routes r ON r.route_id = s.route_id
                JOIN ships sh ON sh.ship_id = s.ship_id
                WHERE s.schedule_id = @scheduleId";

                    using (var cmd = new NpgsqlCommand(sqlSchedule, conn))
                    {
                        cmd.Parameters.AddWithValue("scheduleId", SelectedTicketData.ScheduleId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                SelectedTicketData.SelectedRute = new RouteData
                                {
                                    Rute = reader.GetString(3),
                                    DepartureCity = reader.GetString(4),
                                    ArrivalCity = reader.GetString(5),
                                    Transit = reader.IsDBNull(6) ? "-" : reader.GetString(6),
                                    Harga = reader.GetDecimal(7), // base price dari route
                                    Kapal = reader.GetString(8),
                                    Tanggal = reader.GetDateTime(0),
                                    JamBerangkat = reader.GetTimeSpan(1),
                                    JamTiba = reader.GetTimeSpan(2)
                                };
                            }
                        }
                    }

                    // Ambil data penumpang
                    string sqlPassengers = @"
                SELECT full_name, category, nik
                FROM passengers
                WHERE booking_id = @bookingId";

                    using (var cmd = new NpgsqlCommand(sqlPassengers, conn))
                    {
                        cmd.Parameters.AddWithValue("bookingId", bookingId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SelectedTicketData.Penumpang.Add(new PassengerData
                                {
                                    Nama = reader.GetString(0),
                                    Kategori = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                    NIK = reader.IsDBNull(2) ? "" : reader.GetString(2)
                                });
                            }
                        }
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal mengambil data booking: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async void BtnKirim_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Alamat email harus diisi!", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnKirim.Enabled = false;
                btnKirim.Text = "Mengirim...";

                byte[] pdfBytes = GenerateETicketPDF();
                await SendEmailWithSendGrid(email, pdfBytes);

                MessageBox.Show($"E-Ticket berhasil dikirim ke {email}", "Sukses",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal mengirim email: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnKirim.Enabled = true;
                btnKirim.Text = "Kirim E-Ticket";
            }
        }

        private byte[] GenerateETicketPDF()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 50, 50, 50, 50);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                // Gunakan fully qualified iTextSharp.text.Font
                iTextSharp.text.Font titleFont = FontFactory.GetFont("Arial", 18, iTextSharp.text.Font.BOLD, BaseColor.DARK_GRAY);
                iTextSharp.text.Font headerFont = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font normalFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font footerFont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.ITALIC, BaseColor.GRAY);

                var rute = SelectedTicketData.SelectedRute;

                document.Add(new Paragraph("E-TICKET NAVIGO", titleFont) { Alignment = Element.ALIGN_CENTER, SpacingAfter = 20f });
                document.Add(new Paragraph($"Booking Id: {bookingId}", normalFont) { Alignment = Element.ALIGN_CENTER, SpacingAfter = 20f });
                string statusColor = SelectedTicketData.PaymentStatus == "paid" ? "GREEN" : "RED";
                BaseColor color = SelectedTicketData.PaymentStatus == "paid" ? BaseColor.GREEN : BaseColor.RED;

                document.Add(new Paragraph($"STATUS: {SelectedTicketData.PaymentStatus.ToUpper()}",
                    FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.BOLD, color))
                {
                    Alignment = Element.ALIGN_RIGHT,
                    SpacingAfter = 20f
                });


                document.Add(new Paragraph("INFORMASI PERJALANAN", headerFont));
                document.Add(new Paragraph($"Rute: {rute.Rute}", normalFont));
                document.Add(new Paragraph($"Kapal: {rute.Kapal}", normalFont));
                document.Add(new Paragraph($"Tanggal: {rute.Tanggal:dd MMMM yyyy}", normalFont));
                document.Add(new Paragraph($"Waktu: {rute.JamBerangkat} - {rute.JamTiba}", normalFont));
                document.Add(new Paragraph($"Transit: {rute.Transit}", normalFont));
                document.Add(new Paragraph($"Kelas: {SelectedTicketData.SelectedClass}", normalFont));
                document.Add(new Paragraph($"Metode Pembayaran: {SelectedTicketData.PaymentMethod}", normalFont));
                document.Add(new Paragraph(" "));

                document.Add(new Paragraph("INFORMASI PENUMPANG", headerFont));
                foreach (var p in SelectedTicketData.Penumpang)
                {
                    document.Add(new Paragraph($"• {p.Nama} ({p.Kategori}) - NIK: {p.NIK}", normalFont));
                }

                document.Add(new Paragraph(" "));

                // Info pembayaran
                document.Add(new Paragraph("INFORMASI PEMBAYARAN", headerFont));
                decimal hargaTiketDewasa = SelectedTicketData.SelectedRute.Harga + SelectedTicketData.ClassSurcharge;


                int jumlahDewasa = 0, jumlahAnak = 0, jumlahBayi = 0;
                foreach (var p in SelectedTicketData.Penumpang)
                {
                    if (p.Kategori == "Dewasa") jumlahDewasa++;
                    else if (p.Kategori == "Anak") jumlahAnak++;
                    else if (p.Kategori == "Bayi") jumlahBayi++;
                }

                decimal totalDewasa = hargaTiketDewasa * jumlahDewasa;
                decimal totalAnak = hargaTiketDewasa * 0.75m * jumlahAnak;
                decimal totalBayi = hargaTiketDewasa * 0.25m * jumlahBayi;
                decimal totalKeseluruhan = totalDewasa + totalAnak + totalBayi;

                document.Add(new Paragraph($"Dewasa: {jumlahDewasa} x Rp {hargaTiketDewasa:N0} = Rp {totalDewasa:N0}", normalFont));
                if (jumlahAnak > 0)
                    document.Add(new Paragraph($"Anak: {jumlahAnak} x Rp {hargaTiketDewasa * 0.75m:N0} = Rp {totalAnak:N0}", normalFont));
                if (jumlahBayi > 0)
                    document.Add(new Paragraph($"Bayi: {jumlahBayi} x Rp {hargaTiketDewasa * 0.25m:N0} = Rp {totalBayi:N0}", normalFont));

                document.Add(new Paragraph($"TOTAL: Rp {totalKeseluruhan:N0}", headerFont));
                document.Add(new Paragraph(" "));

                document.Add(new Paragraph("Terima kasih telah menggunakan NaviGO. E-Ticket ini sah dan dapat digunakan untuk boarding.", footerFont) { Alignment = Element.ALIGN_CENTER });

                document.Close();
                return ms.ToArray();
            }
        }

        private async Task SendEmailWithSendGrid(string recipientEmail, byte[] pdfAttachment)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress("fanny.elisabeth17@gmail.com", "NaviGO Service");
            var to = new EmailAddress(recipientEmail);
            var subject = "E-Ticket NaviGO - Tiket Kapal Anda";

            var plainTextContent = $"Halo {SelectedTicketData.Username}, e-ticket Anda terlampir.";
            var htmlContent = $@"
<p>Halo <b>{SelectedTicketData.Username}</b>,</p>
<p>Terima kasih telah menggunakan NaviGO. E-ticket Anda terlampir.</p>
<p><b>Rute:</b> {SelectedTicketData.SelectedRute.Rute}</p>
<p><b>Tanggal:</b> {SelectedTicketData.SelectedRute.Tanggal:dd MMMM yyyy}</p>
<p>Salam hangat,<br>NaviGO</p>
";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var base64Pdf = Convert.ToBase64String(pdfAttachment);
            msg.AddAttachment($"E-Ticket-NaviGO-{SelectedTicketData.BookingId}.pdf", base64Pdf, "application/pdf");

            var response = await client.SendEmailAsync(msg);
            if (!response.IsSuccessStatusCode)
            {
                string body = await response.Body.ReadAsStringAsync();
                throw new Exception("SendGrid Error: " + body);
            }
        }
    }

    // Models sederhana untuk menampung data
    public class BookingData
    {
        public int BookingId;
        public int ScheduleId;
        public string SelectedClass;
        public decimal TotalPrice;
        public string PaymentMethod;
        public string BookingReference;
        public string Username;
        public string UserEmail;
        public string PaymentStatus;
        public decimal ClassSurcharge;
        public RouteData SelectedRute;
        public List<PassengerData> Penumpang;
    }

    public class PassengerData
    {
        public string Nama;
        public string Kategori;
        public string NIK;
    }

    public class RouteData
    {
        public string Rute;
        public string DepartureCity;
        public string ArrivalCity;
        public string Transit;
        public decimal Harga;
        public string Kapal;
        public DateTime Tanggal;
        public TimeSpan JamBerangkat;
        public TimeSpan JamTiba;
    }
}
