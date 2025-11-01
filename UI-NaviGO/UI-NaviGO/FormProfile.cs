using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI_NaviGO
{
    public partial class FormProfileUser : Form
    {
        public FormProfileUser()
        {
            InitializeComponent();
            BuildUI();
            this.Opacity = 1;
            this.FormClosed += (s, e) => Application.Exit();
        }

        // 🔹 Efek transisi antar form (fade)
        private async void SwitchTo(Form nextForm)
        {
            for (double i = 1.0; i >= 0; i -= 0.1)
            {
                this.Opacity = i;
                await Task.Delay(25);
            }

            nextForm.Show();
            this.Hide();

            for (double i = 0; i <= 1; i += 0.1)
            {
                nextForm.Opacity = i;
                await Task.Delay(25);
            }
        }

        private void BuildUI()
        {
            // === FORM ===
            this.Text = "NaviGo - Ship Easily";
            this.Size = new Size(1280, 720);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.DoubleBuffered = true;

            // === BACKGROUND ===
            PictureBox bg = new PictureBox
            {
                Dock = DockStyle.Fill,
                Image = Properties.Resources.profile_bg,
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            this.Controls.Add(bg);

            // === OVERLAY ===
            Panel overlay = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(160, 255, 255, 255),
                Parent = bg
            };
            bg.Controls.Add(overlay);

            // === SIDEBAR ===
            Panel sidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 250,
                BackColor = Color.FromArgb(225, 238, 238)
            };
            this.Controls.Add(sidebar);
            sidebar.BringToFront();

            // Logo
            PictureBox logo = new PictureBox
            {
                Size = new Size(60, 60),
                Location = new Point(20, 20),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = Properties.Resources.logo_navigo
            };
            sidebar.Controls.Add(logo);

            Label lblLogo = new Label
            {
                Text = "NaviGo\nShip Easily",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 90, 100),
                Location = new Point(90, 25),
                AutoSize = true
            };
            sidebar.Controls.Add(lblLogo);

            // Tombol sidebar
            Button btnDashboard = CreateSidebarButton("Dashboard", 130);
            Button btnJadwal = CreateSidebarButton("Manajemen Jadwal", 180);
            Button btnRiwayat = CreateSidebarButton("Manajemen Pemesanan", 230);
            Button btnProfile = CreateSidebarButton("Profil Saya", 280);
            btnProfile.BackColor = Color.FromArgb(200, 230, 225);

            sidebar.Controls.AddRange(new Control[] { btnDashboard, btnJadwal, btnRiwayat, btnProfile });

            // Navigasi antar form
            btnDashboard.Click += (s, e) => SwitchTo(new UserDashboard());
            btnJadwal.Click += (s, e) => SwitchTo(new UserJadwal());
            btnRiwayat.Click += (s, e) => SwitchTo(new UserHistory());
            btnProfile.Click += (s, e) => MessageBox.Show("Anda sudah di halaman Profile.", "Info");

            // === HEADER ===
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 65,
                BackColor = Color.FromArgb(0, 90, 100)
            };
            this.Controls.Add(header);
            header.BringToFront();

            // Tombol Logout
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
            header.Controls.Add(btnLogout);

            // Tombol Profile di header
            Button btnHeaderProfile = new Button
            {
                Text = "Profile",
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(70, 30),
                Cursor = Cursors.Hand
            };
            btnHeaderProfile.FlatAppearance.BorderSize = 0;
            header.Controls.Add(btnHeaderProfile);

            // Label Halo user
            Label lblHalo = new Label
            {
                Text = "Halo, Felicia Angel",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.White,
                AutoSize = true
            };
            header.Controls.Add(lblHalo);

            // Fungsi posisi elemen header
            void PositionHeaderElements()
            {
                int marginRight = 20;
                btnLogout.Location = new Point(header.Width - btnLogout.Width - marginRight, 17);
                btnHeaderProfile.Location = new Point(btnLogout.Left - btnHeaderProfile.Width - 10, 17);
                lblHalo.Location = new Point(btnHeaderProfile.Left - lblHalo.Width - 20, 22);
            }

            PositionHeaderElements();
            header.Resize += (s, e) => PositionHeaderElements();

            // Event tombol
            btnHeaderProfile.Click += (s, e) => MessageBox.Show("Anda sudah di halaman Profile.", "Profile");
            btnLogout.Click += (s, e) =>
            {
                if (MessageBox.Show("Yakin ingin logout?", "Konfirmasi",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SwitchTo(new UserLogin());
                }
            };

            // === CONTENT PROFIL ===
            Panel content = new Panel
            {
                Size = new Size(700, 500),
                Location = new Point(400, 120),
                BackColor = Color.Transparent,
                Parent = overlay
            };
            overlay.Controls.Add(content);
            content.BringToFront();

            // Foto profil bundar
            PictureBox profilePic = new PictureBox
            {
                Size = new Size(130, 130),
                Location = new Point((content.Width - 130) / 2, 30),
                BackColor = Color.FromArgb(210, 240, 240),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            var gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddEllipse(0, 0, profilePic.Width, profilePic.Height);
            profilePic.Region = new Region(gp);
            profilePic.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (Pen p = new Pen(Color.FromArgb(0, 90, 100), 3))
                    e.Graphics.DrawEllipse(p, 1, 1, profilePic.Width - 3, profilePic.Height - 3);
            };
            content.Controls.Add(profilePic);

            // Tombol edit
            Label btnEdit = new Label
            {
                Text = "✎ Edit",
                ForeColor = Color.FromArgb(240, 90, 50),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                AutoSize = true,
                Location = new Point(content.Width - 90, 180)
            };
            btnEdit.Click += (s, e) => MessageBox.Show("Mode edit profil diaktifkan.", "Edit Profile");
            content.Controls.Add(btnEdit);

            // Field informasi user
            CreateInputField(content, "Nama", 200, "Felicia Angel");
            CreateInputField(content, "Email", 260, "felicia.angel@example.com");
            CreateInputField(content, "Nomor Telepon", 320, "+62 812-3456-7890");
            CreateInputField(content, "Password", 380, "••••••••");
        }

        private void CreateInputField(Panel parent, string label, int y, string placeholder = "")
        {
            Label lbl = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 40, 40),
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
    }
}
