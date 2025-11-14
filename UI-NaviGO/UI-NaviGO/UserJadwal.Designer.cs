namespace UI_NaviGO
{
    partial class UserJadwal
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnProfile = new System.Windows.Forms.Button();
            this.lblHalo = new System.Windows.Forms.Label();
            this.lblLogo = new System.Windows.Forms.Label();
            this.panelSubHeader = new System.Windows.Forms.Panel();
            this.lblJadwalRute = new System.Windows.Forms.Label();
            this.panelContent = new System.Windows.Forms.Panel();
            this.pictureBoxBackground = new System.Windows.Forms.PictureBox();
            this.dgvRoutes = new System.Windows.Forms.DataGridView();
            this.panelFilter = new System.Windows.Forms.Panel();
            this.dtpTanggal = new System.Windows.Forms.DateTimePicker();
            this.lblTanggal = new System.Windows.Forms.Label();
            this.cbxKe = new System.Windows.Forms.ComboBox();
            this.lblKe = new System.Windows.Forms.Label();
            this.cbxDari = new System.Windows.Forms.ComboBox();
            this.lblDari = new System.Windows.Forms.Label();
            this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colKapal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRute = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTanggal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPergi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTiba = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTransit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHarga = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelHeader.SuspendLayout();
            this.panelSubHeader.SuspendLayout();
            this.panelContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBackground)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoutes)).BeginInit();
            this.panelFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(17, 76, 97);
            this.panelHeader.Controls.Add(this.btnLogout);
            this.panelHeader.Controls.Add(this.btnProfile);
            this.panelHeader.Controls.Add(this.lblHalo);
            this.panelHeader.Controls.Add(this.lblLogo);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Height = 65;
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(255, 107, 53);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.Location = new System.Drawing.Point(1230, 18);
            this.btnLogout.Size = new System.Drawing.Size(75, 30);
            this.btnLogout.Text = "Logout";
            // 
            // btnProfile
            // 
            this.btnProfile.BackColor = System.Drawing.Color.White;
            this.btnProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProfile.FlatAppearance.BorderSize = 0;
            this.btnProfile.Location = new System.Drawing.Point(1140, 18);
            this.btnProfile.Size = new System.Drawing.Size(75, 30);
            this.btnProfile.Text = "Profile";
            // 
            // lblHalo
            // 
            this.lblHalo.AutoSize = true;
            this.lblHalo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblHalo.ForeColor = System.Drawing.Color.White;
            this.lblHalo.Location = new System.Drawing.Point(950, 23);
            this.lblHalo.Text = "Halo, Felicia Angel";
            // 
            // lblLogo
            // 
            this.lblLogo.AutoSize = true;
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblLogo.ForeColor = System.Drawing.Color.White;
            this.lblLogo.Location = new System.Drawing.Point(20, 18);
            this.lblLogo.Text = "NaviGo";
            // 
            // panelSubHeader
            // 
            this.panelSubHeader.BackColor = System.Drawing.Color.FromArgb(17, 76, 97);
            this.panelSubHeader.Controls.Add(this.lblJadwalRute);
            this.panelSubHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSubHeader.Height = 45;
            // 
            // lblJadwalRute
            // 
            this.lblJadwalRute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblJadwalRute.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblJadwalRute.ForeColor = System.Drawing.Color.White;
            this.lblJadwalRute.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblJadwalRute.Text = "Jadwal dan Rute";
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.dgvRoutes);
            this.panelContent.Controls.Add(this.panelFilter);
            this.panelContent.Controls.Add(this.pictureBoxBackground);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // pictureBoxBackground
            // 
            this.pictureBoxBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxBackground.Image = global::UI_NaviGO.Properties.Resources.profile_bg;
            this.pictureBoxBackground.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // dgvRoutes
            // 
            this.dgvRoutes.AllowUserToAddRows = false;
            this.dgvRoutes.AllowUserToDeleteRows = false;
            this.dgvRoutes.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.dgvRoutes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRoutes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRoutes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colID, this.colKapal, this.colRute, this.colTanggal,
                this.colPergi, this.colTiba, this.colTransit, this.colHarga });
            this.dgvRoutes.Location = new System.Drawing.Point(350, 200);
            this.dgvRoutes.Size = new System.Drawing.Size(900, 300);
            this.dgvRoutes.ReadOnly = true;
            this.dgvRoutes.RowHeadersVisible = false;
            // 
            // panelFilter
            // 
            this.panelFilter.BackColor = System.Drawing.Color.FromArgb(245, 250, 250);
            this.panelFilter.Controls.Add(this.dtpTanggal);
            this.panelFilter.Controls.Add(this.lblTanggal);
            this.panelFilter.Controls.Add(this.cbxKe);
            this.panelFilter.Controls.Add(this.lblKe);
            this.panelFilter.Controls.Add(this.cbxDari);
            this.panelFilter.Controls.Add(this.lblDari);
            this.panelFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilter.Height = 80;
            // 
            // Filter controls
            // 
            this.lblDari.Location = new System.Drawing.Point(400, 15);
            this.lblDari.Text = "Dari";
            this.cbxDari.Location = new System.Drawing.Point(400, 40);
            this.cbxDari.Items.AddRange(new object[] { "Pilih Asal Kota / Pelabuhan", "Jakarta", "Surabaya" });
            this.lblKe.Location = new System.Drawing.Point(640, 15);
            this.lblKe.Text = "Ke";
            this.cbxKe.Location = new System.Drawing.Point(640, 40);
            this.cbxKe.Items.AddRange(new object[] { "Pilih Tujuan Kota / Pelabuhan", "Batam", "Bali" });
            this.lblTanggal.Location = new System.Drawing.Point(900, 15);
            this.lblTanggal.Text = "Tanggal Bulan Keberangkatan";
            this.dtpTanggal.Location = new System.Drawing.Point(900, 40);
            // 
            // Column Headers
            // 
            this.colID.HeaderText = "ID";
            this.colKapal.HeaderText = "Kapal";
            this.colRute.HeaderText = "Rute";
            this.colTanggal.HeaderText = "Tanggal";
            this.colPergi.HeaderText = "Pergi";
            this.colTiba.HeaderText = "Tiba";
            this.colTransit.HeaderText = "Transit";
            this.colHarga.HeaderText = "Harga";
            // 
            // UserJadwal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1575, 1000);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelSubHeader);
            this.Controls.Add(this.panelHeader);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NaviGo - Ship Easily";
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnProfile;
        private System.Windows.Forms.Label lblHalo;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Panel panelSubHeader;
        private System.Windows.Forms.Label lblJadwalRute;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.PictureBox pictureBoxBackground;
        private System.Windows.Forms.Panel panelFilter;
        private System.Windows.Forms.Label lblDari;
        private System.Windows.Forms.ComboBox cbxDari;
        private System.Windows.Forms.Label lblKe;
        private System.Windows.Forms.ComboBox cbxKe;
        private System.Windows.Forms.Label lblTanggal;
        private System.Windows.Forms.DateTimePicker dtpTanggal;
        private System.Windows.Forms.DataGridView dgvRoutes;
        private System.Windows.Forms.DataGridViewTextBoxColumn colID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKapal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRute;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTanggal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPergi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTiba;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTransit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHarga;
    }
}