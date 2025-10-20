using System;
using System.Windows.Forms;

namespace NaviGo
{
    public partial class UserJadwal : Form
    {
        public UserJadwal()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            // Load data ke DataGridView
            dgvRoutes.Rows.Add("KJ02", "Kapal Jetsar", "Jakarta - Batam", "15 Sep 2025", "07:00", "10:00", "Direct", "Rp 750,000");
            dgvRoutes.Rows.Add("KK01", "Kapal Kastar", "Surabaya - Bali", "12 Des 2025", "12:00", "13:00", "Transit", "Rp 300,000");
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Buka halaman profile");
            // Tambahkan logic profile di sini
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Yakin mau logout?", "Konfirmasi", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                // Tambahkan logic logout di sini
                this.Close();
            }
        }

        private void cbxDari_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Event handler saat memilih asal
        }

        private void cbxKe_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Event handler saat memilih tujuan
        }

        private void dtpTanggal_ValueChanged(object sender, EventArgs e)
        {
            // Event handler saat tanggal berubah
        }

        private void dgvRoutes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Event handler saat klik cell di grid
            if (e.RowIndex >= 0)
            {
                string idRute = dgvRoutes.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                MessageBox.Show($"Rute dipilih: {idRute}");
            }
        }
    }
}