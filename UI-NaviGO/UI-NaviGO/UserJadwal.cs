using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace UI_NaviGO
{
    public partial class UserJadwal : Form
    {
        // Data rute kapal hardcode
        private List<JadwalKapal> daftarJadwal;

        public UserJadwal()
        {
            InitializeComponent();
            SetupEvents();
            LoadDataRute();
            SetupFilterEvents();
        }

        private void LoadDataRute()
        {
            // Inisialisasi data jadwal
            daftarJadwal = new List<JadwalKapal>
            {
                new JadwalKapal { ID = "KJ01", NamaKapal = "KM Express", Dari = "Jakarta", Ke = "Batam",
                                 Tanggal = "15 Sep 2024", Pergi = "08:00", Tiba = "14:00",
                                 Transit = "Langsung", Harga = 750000 },
                new JadwalKapal { ID = "KS02", NamaKapal = "KM Sejahtera", Dari = "Surabaya", Ke = "Bali",
                                 Tanggal = "16 Sep 2024", Pergi = "09:30", Tiba = "16:45",
                                 Transit = "Transit 1x", Harga = 550000 },
                new JadwalKapal { ID = "KM03", NamaKapal = "KM Bahari", Dari = "Semarang", Ke = "Makassar",
                                 Tanggal = "17 Sep 2024", Pergi = "07:15", Tiba = "19:30",
                                 Transit = "Transit 2x", Harga = 1200000 },
                new JadwalKapal { ID = "KP04", NamaKapal = "KM Pelangi", Dari = "Bali", Ke = "Lombok",
                                 Tanggal = "18 Sep 2024", Pergi = "10:00", Tiba = "12:30",
                                 Transit = "Langsung", Harga = 350000 },
                new JadwalKapal { ID = "KV05", NamaKapal = "KM Victory", Dari = "Jakarta", Ke = "Surabaya",
                                 Tanggal = "19 Sep 2024", Pergi = "06:45", Tiba = "20:15",
                                 Transit = "Transit 1x", Harga = 900000 }
            };

            // Isi combobox filter
            cbxDari.Items.AddRange(new string[] { "Semua Kota", "Jakarta", "Surabaya", "Semarang", "Bali" });
            cbxKe.Items.AddRange(new string[] { "Semua Kota", "Batam", "Bali", "Makassar", "Lombok", "Surabaya" });

            cbxDari.SelectedIndex = 0;
            cbxKe.SelectedIndex = 0;

            RefreshDataGrid();
        }

        private void RefreshDataGrid()
        {
            dgvRoutes.Rows.Clear();

            foreach (var jadwal in daftarJadwal)
            {
                // Filter data
                if (cbxDari.SelectedIndex > 0 && jadwal.Dari != cbxDari.SelectedItem.ToString())
                    continue;

                if (cbxKe.SelectedIndex > 0 && jadwal.Ke != cbxKe.SelectedItem.ToString())
                    continue;

                if (dtpTanggal.Value.Date != DateTime.Today &&
                    DateTime.Parse(jadwal.Tanggal).Date != dtpTanggal.Value.Date)
                    continue;

                dgvRoutes.Rows.Add(
                    jadwal.ID,
                    jadwal.NamaKapal,
                    $"{jadwal.Dari} - {jadwal.Ke}",
                    jadwal.Tanggal,
                    jadwal.Pergi,
                    jadwal.Tiba,
                    jadwal.Transit,
                    jadwal.Harga.ToString("C0")
                );
            }
        }

        private void SetupEvents()
        {
            // Event untuk tombol di header
            btnLogout.Click += (s, e) =>
            {
                if (MessageBox.Show("Yakin ingin logout?", "Konfirmasi",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Hide();
                    new UserLogin().Show();
                }
            };

            btnProfile.Click += (s, e) =>
            {
                this.Hide();
                new FormProfileUser().Show();
            };

            // Double click pada row untuk memilih
            dgvRoutes.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    PilihJadwal(e.RowIndex);
                }
            };

            // Enter key untuk memilih
            dgvRoutes.KeyPress += (s, e) =>
            {
                if (e.KeyChar == (char)13 && dgvRoutes.CurrentRow != null) // Enter key
                {
                    PilihJadwal(dgvRoutes.CurrentRow.Index);
                }
            };
        }

        private void SetupFilterEvents()
        {
            cbxDari.SelectedIndexChanged += (s, e) => RefreshDataGrid();
            cbxKe.SelectedIndexChanged += (s, e) => RefreshDataGrid();
            dtpTanggal.ValueChanged += (s, e) => RefreshDataGrid();
        }

        private void PilihJadwal(int rowIndex)
        {
            var selectedRow = dgvRoutes.Rows[rowIndex];
            string idKapal = selectedRow.Cells["colID"].Value?.ToString() ?? "";

            var jadwalTerpilih = daftarJadwal.Find(j => j.ID == idKapal);

            if (jadwalTerpilih != null)
            {
                // Simpan data tiket yang dipilih
                TiketManager.TiketDipilih = new Tiket
                {
                    ID = jadwalTerpilih.ID,
                    NamaKapal = jadwalTerpilih.NamaKapal,
                    Rute = $"{jadwalTerpilih.Dari} - {jadwalTerpilih.Ke}",
                    Harga = jadwalTerpilih.Harga,
                    TanggalBerangkat = DateTime.Parse(jadwalTerpilih.Tanggal + " " + jadwalTerpilih.Pergi)
                };

                MessageBox.Show($"Tiket {jadwalTerpilih.NamaKapal} dipilih!\n" +
                              $"Rute: {jadwalTerpilih.Dari} - {jadwalTerpilih.Ke}\n" +
                              $"Harga: {jadwalTerpilih.Harga.ToString("C0")}",
                              "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Hide();
                new UserPenumpang().Show();
            }
        }
    }

    // Class untuk menyimpan data jadwal
    public class JadwalKapal
    {
        public string ID { get; set; }
        public string NamaKapal { get; set; }
        public string Dari { get; set; }
        public string Ke { get; set; }
        public string Tanggal { get; set; }
        public string Pergi { get; set; }
        public string Tiba { get; set; }
        public string Transit { get; set; }
        public decimal Harga { get; set; }
    }

    // 🔹 CLASS UNTUK MANAGER TIKET (Tetap sama)
    public static class TiketManager
    {
        public static Tiket TiketDipilih { get; set; }
        public static List<Penumpang> DaftarPenumpang { get; set; } = new List<Penumpang>();
        public static decimal TotalHarga { get; set; }
        public static string MetodePembayaran { get; set; }
        public static string BuktiPembayaran { get; set; }
    }

    public class Tiket
    {
        public string ID { get; set; }
        public string NamaKapal { get; set; }
        public string Rute { get; set; }
        public decimal Harga { get; set; }
        public DateTime TanggalBerangkat { get; set; }
    }

    public class Penumpang
    {
        public string Nama { get; set; }
        public string NIK { get; set; }
        public string Kategori { get; set; } // Dewasa / Anak
    }
}