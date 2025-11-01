using System;
using System.Drawing;
using System.Windows.Forms;

namespace UI_NaviGO
{
    public partial class AdminProfile : Form
    {
        public AdminProfile()
        {
            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            // === FORM ===
            this.Text = "NaviGo - Admin Profile";
            this.Size = new Size(1280, 720);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(250, 240, 240); // background solid lembut
            this.DoubleBuffered = true;

            // === SIDEBAR ===
            Panel sidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 250,
                BackColor = Color.FromArgb(225, 238, 238)
            };
            this.Controls.Add(sidebar);

            PictureBox logo = new PictureBox
            {
                Size = new Size(60, 60),
                Location = new Point(20, 20),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = Properties.Resources.logo_navigo // logo dari resources
            };
            sidebar.Controls.Add(logo);

            // === LABEL LOGO TEXT ===
            Label lblLogo = new Label
            {
                Text = "NaviGo Admin",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 90, 100),
                Location = new Point(90, 25),
                AutoSize = true
            };
            sidebar.Controls.Add(lblLogo);

            Label lblSub = new Label
            {
                Text = "Dashboard Management",
                Font = new Font("Segoe UI", 8, FontStyle.Regular),
                ForeColor = Color.FromArgb(60, 100, 100),
                Location = new Point(92, 52),
                AutoSize = true
            };
            sidebar.Controls.Add(lblSub);

            // === SIDEBAR BUTTONS ===
            Button btnDashboard = CreateSidebarButton("Dashboard", 130);
            Button btnJadwal = CreateSidebarButton("Manajemen Jadwal", 180);
            Button btnRiwayat = CreateSidebarButton("Manajemen Pemesanan", 230);
            btnRiwayat.BackColor = Color.FromArgb(200, 230, 225);
            sidebar.Controls.AddRange(new Control[] { btnDashboard, btnJadwal, btnRiwayat });

            // === HEADER ===
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 65,
                BackColor = Color.FromArgb(0, 90, 100)
            };
            this.Controls.Add(header);
            header.BringToFront();

            Button btnLogout = new Button
            {
                Text = "Logout",
                BackColor = Color.FromArgb(240, 90, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(70, 30),
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += BtnLogout_Click;
            header.Controls.Add(btnLogout);

            Button btnProfile = new Button
            {
                Text = "Profile",
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(70, 30),
                Cursor = Cursors.Hand
            };
            btnProfile.FlatAppearance.BorderSize = 0;
            btnProfile.Click += BtnProfile_Click;
            header.Controls.Add(btnProfile);

            Label lblHalo = new Label
            {
                Text = "Halo, Felicia Angel",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.White,
                AutoSize = true
            };
            header.Controls.Add(lblHalo);

            void PositionHeaderElements()
            {
                int marginRight = 20;
                btnLogout.Location = new Point(header.Width - btnLogout.Width - marginRight, 17);
                btnProfile.Location = new Point(btnLogout.Left - btnProfile.Width - 10, 17);
                lblHalo.Location = new Point(btnProfile.Left - lblHalo.Width - 20, 22);
            }

            PositionHeaderElements();
            header.Resize += (s, e) => PositionHeaderElements();

            // === AREA PROFIL ===
            Panel content = new Panel
            {
                Size = new Size(700, 500),
                Location = new Point(400, 120),
                BackColor = Color.Transparent
            };
            this.Controls.Add(content);

            // === FOTO PROFIL BUNDAR ===
            PictureBox profilePic = new PictureBox
            {
                Size = new Size(130, 130),
                Location = new Point((content.Width - 130) / 2, 20),
                BackColor = Color.FromArgb(200, 230, 230),
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            var gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddEllipse(0, 0, profilePic.Width, profilePic.Height);
            profilePic.Region = new Region(gp);
            profilePic.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (Pen p = new Pen(Color.Black, 4))
                    e.Graphics.DrawEllipse(p, 1, 1, profilePic.Width - 3, profilePic.Height - 3);
            };
            content.Controls.Add(profilePic);

            // === TOMBOL EDIT ===
            Label btnEdit = new Label
            {
                Text = "Edit",
                ForeColor = Color.FromArgb(240, 90, 50),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                AutoSize = true,
                Location = new Point(content.Width - 90, 160)
            };
            btnEdit.Click += (s, e) => MessageBox.Show("Mode edit profil diaktifkan.", "Edit Profile");
            content.Controls.Add(btnEdit);

            // === FIELD PROFIL ===
            CreateInputField(content, "Nama", 200, "Felicia Angel");
            CreateInputField(content, "Email", 260, "felicia.angel@example.com");
            CreateInputField(content, "Nomor telepon", 320, "+62 812-3456-7890");
            CreateInputField(content, "Password", 380, "••••••••");
        }

        private Button CreateSidebarButton(string text, int top)
        {
            Button btn = new Button
            {
                Text = text,
                Width = 200,
                Height = 40,
                Location = new Point(25, top),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(0, 90, 100),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(190, 225, 225);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.White;
            return btn;
        }

        private void CreateInputField(Panel parent, string label, int y, string placeholder = "")
        {
            Label lbl = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(100, y),
                AutoSize = true
            };
            parent.Controls.Add(lbl);

            TextBox txt = new TextBox
            {
                Size = new Size(500, 32),
                Location = new Point(100, y + 22),
                Font = new Font("Segoe UI", 10),
                Text = placeholder,
                ReadOnly = true,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            parent.Controls.Add(txt);
        }

        private void BtnProfile_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Anda sudah di halaman Profile.", "Profile");
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Yakin ingin logout?", "Konfirmasi",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                this.Close();
        }
    }
}
