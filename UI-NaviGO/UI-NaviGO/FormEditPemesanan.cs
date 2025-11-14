using System;
using System.Drawing;
using System.Windows.Forms;

namespace UI_NaviGO
{
    public partial class FormEditPemesanan : Form
    {
        private RiwayatTiket tiket;

        public FormEditPemesanan(RiwayatTiket tiket)
        {
            this.tiket = tiket;
            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text = "Edit Pemesanan - NaviGO";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            Panel mainPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            Label lblTitle = new Label()
            {
                Text = "Edit Pemesanan",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 85, 92),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            // Tombol Edit Penumpang
            Button btnEditPenumpang = new Button()
            {
                Text = "Edit Data Penumpang",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(180, 220, 215),
                ForeColor = Color.FromArgb(0, 85, 92),
                Size = new Size(300, 50),
                Location = new Point(100, 100),
                FlatStyle = FlatStyle.Flat
            };
            btnEditPenumpang.FlatAppearance.BorderSize = 0;
            btnEditPenumpang.Click += (s, e) =>
            {
                EditPenumpang();
                this.Close();
            };

            // Tombol Reschedule
            Button btnReschedule = new Button()
            {
                Text = "Reschedule Jadwal",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(255, 204, 153),
                ForeColor = Color.FromArgb(0, 85, 92),
                Size = new Size(300, 50),
                Location = new Point(100, 170),
                FlatStyle = FlatStyle.Flat
            };
            btnReschedule.FlatAppearance.BorderSize = 0;
            btnReschedule.Click += (s, e) =>
            {
                RescheduleTiket();
                this.Close();
            };

            // Tombol Ubah Kelas
            Button btnUbahKelas = new Button()
            {
                Text = "Ubah Kelas",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(200, 230, 225),
                ForeColor = Color.FromArgb(0, 85, 92),
                Size = new Size(300, 50),
                Location = new Point(100, 240),
                FlatStyle = FlatStyle.Flat
            };
            btnUbahKelas.FlatAppearance.BorderSize = 0;
            btnUbahKelas.Click += (s, e) =>
            {
                UbahKelas();
                this.Close();
            };

            // Tombol Kembali
            Button btnKembali = new Button()
            {
                Text = "Kembali",
                Font = new Font("Segoe UI", 11),
                BackColor = Color.FromArgb(150, 150, 150),
                ForeColor = Color.White,
                Size = new Size(120, 40),
                Location = new Point(190, 320),
                FlatStyle = FlatStyle.Flat
            };
            btnKembali.FlatAppearance.BorderSize = 0;
            btnKembali.Click += (s, e) => this.Close();

            mainPanel.Controls.AddRange(new Control[] {
                lblTitle, btnEditPenumpang, btnReschedule, btnUbahKelas, btnKembali
            });

            this.Controls.Add(mainPanel);
        }

        private void EditPenumpang()
        {
            // Isi data penumpang ke SelectedTicketData
            SelectedTicketData.TiketReschedule = tiket;

            // Buka form penumpang dalam mode edit
            UserPenumpang formPenumpang = new UserPenumpang(true); // mode edit
            formPenumpang.Show();
        }

        private void RescheduleTiket()
        {
            SelectedTicketData.TiketReschedule = tiket;
            UserJadwal formJadwal = new UserJadwal();
            formJadwal.Show();
        }

        private void UbahKelas()
        {
            SelectedTicketData.TiketReschedule = tiket;

            // Buka form pembayaran dalam mode edit kelas
            UserPembayaran formPembayaran = new UserPembayaran(true); // mode edit
            formPembayaran.Show();
        }
    }
}