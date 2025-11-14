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



namespace UI_NaviGO
{
    public partial class FormKirimEmail : Form
    {
        
        private TextBox txtEmail;
        private Button btnKirim;
        private Button btnBatal;

        public FormKirimEmail()
        {
            DotNetEnv.Env.Load();
            InitializeComponent();
            BuildUI();
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
                Font = new System.Drawing.Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 85, 92),
                AutoSize = true,
                Location = new Point(120, 20)
            };

            Label lblEmail = new Label()
            {
                Text = "Alamat Email:",
                Font = new System.Drawing.Font("Segoe UI", 10),
                AutoSize = true,
                Location = new Point(50, 70)
            };

            txtEmail = new TextBox()
            {
                Location = new Point(150, 68),
                Size = new Size(250, 25),
                Font = new System.Drawing.Font("Segoe UI", 10)
            };

            if (!string.IsNullOrEmpty(SelectedTicketData.Email))
            {
                txtEmail.Text = SelectedTicketData.Email;
            }

            btnKirim = new Button()
            {
                Text = "Kirim E-Ticket",
                BackColor = Color.FromArgb(0, 120, 100),
                ForeColor = Color.White,
                Size = new Size(120, 35),
                Location = new Point(150, 120),
                Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnKirim.Click += BtnKirim_Click;

            btnBatal = new Button()
            {
                Text = "Batal",
                BackColor = Color.FromArgb(200, 200, 200),
                ForeColor = Color.Black,
                Size = new Size(120, 35),
                Location = new Point(280, 120),
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            btnBatal.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { lblTitle, lblEmail, txtEmail, btnKirim, btnBatal });
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

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Format email tidak valid!", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnKirim.Enabled = false;
                btnKirim.Text = "Mengirim...";

                byte[] pdfBytes = GenerateETicketPDF();

                // Pakai SendGrid Web API
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

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private byte[] GenerateETicketPDF()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 50, 50, 50, 50);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                iTextSharp.text.Font titleFont = FontFactory.GetFont("Arial", 18, iTextSharp.text.Font.BOLD, BaseColor.DARK_GRAY);
                Paragraph title = new Paragraph("E-TICKET NAVIGO", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20f
                };
                document.Add(title);

                iTextSharp.text.Font statusFont = FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.BOLD, BaseColor.GREEN);
                Paragraph status = new Paragraph("STATUS: LUNAS", statusFont)
                {
                    Alignment = Element.ALIGN_RIGHT,
                    SpacingAfter = 20f
                };
                document.Add(status);

                iTextSharp.text.Font headerFont = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font normalFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                var rute = SelectedTicketData.SelectedRute;

                document.Add(new Paragraph("INFORMASI PERJALANAN", headerFont));
                document.Add(new Paragraph($"Rute: {rute.Rute}", normalFont));
                document.Add(new Paragraph($"Kapal: {rute.Kapal}", normalFont));
                document.Add(new Paragraph($"ID Tiket: {rute.ID}", normalFont));
                document.Add(new Paragraph($"Tanggal: {rute.Tanggal:dd MMMM yyyy}", normalFont));
                document.Add(new Paragraph($"Waktu: {rute.JamBerangkat} - {rute.JamTiba}", normalFont));
                document.Add(new Paragraph($"Transit: {rute.Transit}", normalFont));
                document.Add(new Paragraph($"Kelas: {SelectedTicketData.KelasDipilih}", normalFont));
                document.Add(new Paragraph($"Metode Pembayaran: {SelectedTicketData.KelasDipilih}", normalFont));
                document.Add(new Paragraph(" "));

                document.Add(new Paragraph("INFORMASI PENUMPANG", headerFont));
                foreach (var penumpang in SelectedTicketData.Penumpang)
                {
                    document.Add(new Paragraph($"• {penumpang.Nama} ({penumpang.Kategori}) - NIK: {penumpang.NIK}", normalFont));
                }
                document.Add(new Paragraph(" "));

                document.Add(new Paragraph("INFORMASI PEMBAYARAN", headerFont));

                decimal hargaDasar = rute.Harga;
                decimal hargaKelas = SelectedTicketData.HargaKelas;
                decimal hargaTiketDewasa = hargaDasar + hargaKelas;

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



                iTextSharp.text.Font footerFont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.ITALIC, BaseColor.GRAY);
                Paragraph footer = new Paragraph("Terima kasih telah menggunakan NaviGO. E-Ticket ini sah dan dapat digunakan untuk boarding.", footerFont)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                document.Add(footer);

                document.Close();
                return ms.ToArray();
            }
        }

        private async Task SendEmailWithSendGrid(string recipientEmail, byte[] pdfAttachment)
        {
            // Pakai API Key Free Trial
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress("fanny.elisabeth17@gmail.com", "NaviGO Service"); // verified sender
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

            // Lampirkan PDF
            var base64Pdf = Convert.ToBase64String(pdfAttachment);
            msg.AddAttachment($"E-Ticket-NaviGO-{SelectedTicketData.SelectedRute.ID}.pdf", base64Pdf, "application/pdf");

            var response = await client.SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
            {
                string body = await response.Body.ReadAsStringAsync();
                throw new Exception("SendGrid Error: " + body);
            }
        }
    }
}
