namespace NaviGo
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

        private void InitializeComponent()
        {
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
            this.panelHeader.SuspendLayout();
            this.panelSubHeader.SuspendLayout();
            this.panelContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBackground)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoutes)).BeginInit();
            this.panelFilter.SuspendLayout();
            this.SuspendLayout();

            // panelHeader
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(76)))), ((int)(((byte)(97)))));
            this.panelHeader.Controls.Add(this.btnLogout);
            this.panelHeader.Controls.Add(this.btnProfile);
            this.panelHeader.Controls.Add(this.lblHalo);
            this.panelHeader.Controls.Add(this.lblLogo);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Height = 60;
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1400, 60);
            this.panelHeader.TabIndex = 0;

            // lblLogo
            this.lblLogo.AutoSize = true;
            this.lblLogo.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold);
            this.lblLogo.ForeColor = System.Drawing.Color.White;
            this.lblLogo.Location = new System.Drawing.Point(20, 15);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(80, 28);
            this.lblLogo.TabIndex = 0;
            this.lblLogo.Text = "NaviGo";

            // lblHalo
            this.lblHalo.AutoSize = true;
            this.lblHalo.Font = new System.Drawing.Font("Arial", 10F);
            this.lblHalo.ForeColor = System.Drawing.Color.White;
            this.lblHalo.Location = new System.Drawing.Point(900, 20);
            this.lblHalo.Name = "lblHalo";
            this.lblHalo.Size = new System.Drawing.Size(150, 18);
            this.lblHalo.TabIndex = 1;
            this.lblHalo.Text = "Halo, Felicia Angel";

            // btnProfile
            this.btnProfile.BackColor = System.Drawing.Color.White;
            this.btnProfile.Font = new System.Drawing.Font("Arial", 9F);
            this.btnProfile.ForeColor = System.Drawing.Color.Black;
            this.btnProfile.Location = new System.Drawing.Point(1110, 15);
            this.btnProfile.Name = "btnProfile";
            this.btnProfile.Size = new System.Drawing.Size(70, 30);
            this.btnProfile.TabIndex = 2;
            this.btnProfile.Text = "Profile";
            this.btnProfile.UseVisualStyleBackColor = false;
            this.btnProfile.Click += new System.EventHandler(this.btnProfile_Click);

            // btnLogout
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(107)))), ((int)(((byte)(53)))));
            this.btnLogout.Font = new System.Drawing.Font("Arial", 9F);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(1190, 15);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(70, 30);
            this.btnLogout.TabIndex = 3;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);

            // panelSubHeader
            this.panelSubHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(76)))), ((int)(((byte)(97)))));
            this.panelSubHeader.Controls.Add(this.lblJadwalRute);
            this.panelSubHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSubHeader.Height = 50;
            this.panelSubHeader.Name = "panelSubHeader";
            this.panelSubHeader.Size = new System.Drawing.Size(1400, 50);
            this.panelSubHeader.TabIndex = 1;

            // lblJadwalRute
            this.lblJadwalRute.AutoSize = true;
            this.lblJadwalRute.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lblJadwalRute.ForeColor = System.Drawing.Color.White;
            this.lblJadwalRute.Location = new System.Drawing.Point(350, 12);
            this.lblJadwalRute.Name = "lblJadwalRute";
            this.lblJadwalRute.Size = new System.Drawing.Size(145, 20);
            this.lblJadwalRute.TabIndex = 0;
            this.lblJadwalRute.Text = "Jadwal dan Rute";

            // panelContent
            this.panelContent.BackColor = System.Drawing.Color.White;
            this.panelContent.Controls.Add(this.pictureBoxBackground);
            this.panelContent.Controls.Add(this.dgvRoutes);
            this.panelContent.Controls.Add(this.panelFilter);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(1400, 690);
            this.panelContent.TabIndex = 2;

            // panelFilter
            this.panelFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            this.panelFilter.Controls.Add(this.dtpTanggal);
            this.panelFilter.Controls.Add(this.lblTanggal);
            this.panelFilter.Controls.Add(this.cbxKe);
            this.panelFilter.Controls.Add(this.lblKe);
            this.panelFilter.Controls.Add(this.cbxDari);
            this.panelFilter.Controls.Add(this.lblDari);
            this.panelFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilter.Height = 100;
            this.panelFilter.Name = "panelFilter";
            this.panelFilter.Size = new System.Drawing.Size(1400, 100);
            this.panelFilter.TabIndex = 0;

            // lblDari
            this.lblDari.AutoSize = true;
            this.lblDari.Font = new System.Drawing.Font("Arial", 10F);
            this.lblDari.Location = new System.Drawing.Point(350, 15);
            this.lblDari.Name = "lblDari";
            this.lblDari.Size = new System.Drawing.Size(28, 18);
            this.lblDari.TabIndex = 0;
            this.lblDari.Text = "Dari";

            // cbxDari
            this.cbxDari.FormattingEnabled = true;
            this.cbxDari.Items.AddRange(new object[] {
            "Pilih Asal Kota / Pelabuhan",
            "Jakarta",
            "Surabaya"});
            this.cbxDari.Location = new System.Drawing.Point(350, 35);
            this.cbxDari.Name = "cbxDari";
            this.cbxDari.Size = new System.Drawing.Size(300, 24);
            this.cbxDari.TabIndex = 1;
            this.cbxDari.SelectedIndex = 0;
            this.cbxDari.SelectedIndexChanged += new System.EventHandler(this.cbxDari_SelectedIndexChanged);

            // lblKe
            this.lblKe.AutoSize = true;
            this.lblKe.Font = new System.Drawing.Font("Arial", 10F);
            this.lblKe.Location = new System.Drawing.Point(720, 15);
            this.lblKe.Name = "lblKe";
            this.lblKe.Size = new System.Drawing.Size(23, 18);
            this.lblKe.TabIndex = 2;
            this.lblKe.Text = "Ke";

            // cbxKe
            this.cbxKe.FormattingEnabled = true;
            this.cbxKe.Items.AddRange(new object[] {
            "Pilih Tujuan Kota / Pelabuhan",
            "Batam",
            "Bali"});
            this.cbxKe.Location = new System.Drawing.Point(720, 35);
            this.cbxKe.Name = "cbxKe";
            this.cbxKe.Size = new System.Drawing.Size(300, 24);
            this.cbxKe.TabIndex = 3;
            this.cbxKe.SelectedIndex = 0;
            this.cbxKe.SelectedIndexChanged += new System.EventHandler(this.cbxKe_SelectedIndexChanged);

            // lblTanggal
            this.lblTanggal.AutoSize = true;
            this.lblTanggal.Font = new System.Drawing.Font("Arial", 10F);
            this.lblTanggal.Location = new System.Drawing.Point(1100, 15);
            this.lblTanggal.Name = "lblTanggal";
            this.lblTanggal.Size = new System.Drawing.Size(235, 18);
            this.lblTanggal.TabIndex = 4;
            this.lblTanggal.Text = "Tanggal Bulan Keberangkatan";

            // dtpTanggal
            this.dtpTanggal.Location = new System.Drawing.Point(1100, 35);
            this.dtpTanggal.Name = "dtpTanggal";
            this.dtpTanggal.Size = new System.Drawing.Size(250, 22);
            this.dtpTanggal.TabIndex = 5;
            this.dtpTanggal.ValueChanged += new System.EventHandler(this.dtpTanggal_ValueChanged);

            // dgvRoutes
            this.dgvRoutes.AllowUserToAddRows = false;
            this.dgvRoutes.AllowUserToDeleteRows = false;
            this.dgvRoutes.BackgroundColor = System.Drawing.Color.White;
            this.dgvRoutes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dgvRoutes.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dgvRoutes.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.dgvRoutes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRoutes.Location = new System.Drawing.Point(350, 120);
            this.dgvRoutes.Name = "dgvRoutes";
            this.dgvRoutes.Size = new System.Drawing.Size(1000, 200);
            this.dgvRoutes.TabIndex = 1;
            this.dgvRoutes.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRoutes_CellClick);

            // Setup columns
            this.dgvRoutes.Columns.Add("ID", "ID");
            this.dgvRoutes.Columns.Add("Kapal", "Kapal");
            this.dgvRoutes.Columns.Add("Rute", "Rute");
            this.dgvRoutes.Columns.Add("Tanggal", "Tanggal");
            this.dgvRoutes.Columns.Add("Pergi", "Pergi");
            this.dgvRoutes.Columns.Add("Tiba", "Tiba");
            this.dgvRoutes.Columns.Add("Transit", "Transit");
            this.dgvRoutes.Columns.Add("Harga", "Harga");

            this.dgvRoutes.Columns["ID"].Width = 50;
            this.dgvRoutes.Columns["Kapal"].Width = 100;
            this.dgvRoutes.Columns["Rute"].Width = 140;
            this.dgvRoutes.Columns["Tanggal"].Width = 100;
            this.dgvRoutes.Columns["Pergi"].Width = 70;
            this.dgvRoutes.Columns["Tiba"].Width = 70;
            this.dgvRoutes.Columns["Transit"].Width = 70;
            this.dgvRoutes.Columns["Harga"].Width = 100;

            // pictureBoxBackground
            this.pictureBoxBackground.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pictureBoxBackground.Location = new System.Drawing.Point(350, 330);
            this.pictureBoxBackground.Name = "pictureBoxBackground";
            this.pictureBoxBackground.Size = new System.Drawing.Size(1000, 370);
            this.pictureBoxBackground.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxBackground.TabIndex = 2;
            this.pictureBoxBackground.TabStop = false;

            // UserJadwal
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 800);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelSubHeader);
            this.Controls.Add(this.panelHeader);
            this.Name = "UserJadwal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NaviGo - Ship Easily";
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelSubHeader.ResumeLayout(false);
            this.panelSubHeader.PerformLayout();
            this.panelContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBackground)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoutes)).EndInit();
            this.panelFilter.ResumeLayout(false);
            this.panelFilter.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Label lblHalo;
        private System.Windows.Forms.Button btnProfile;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Panel panelSubHeader;
        private System.Windows.Forms.Label lblJadwalRute;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Panel panelFilter;
        private System.Windows.Forms.Label lblDari;
        private System.Windows.Forms.ComboBox cbxDari;
        private System.Windows.Forms.Label lblKe;
        private System.Windows.Forms.ComboBox cbxKe;
        private System.Windows.Forms.Label lblTanggal;
        private System.Windows.Forms.DateTimePicker dtpTanggal;
        private System.Windows.Forms.DataGridView dgvRoutes;
        private System.Windows.Forms.PictureBox pictureBoxBackground;
    }
}