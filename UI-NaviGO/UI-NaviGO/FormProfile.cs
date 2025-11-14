using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI_NaviGO
{
    public partial class FormProfileUser : Form
    {
        private UserProfile currentUser;
        private bool isEditMode = false;

        private Panel sidebar;
        private Panel topHeader;
        private Panel contentPanel;
        private Panel contentBox;

        private PictureBox profilePic;
        private Label lblEdit;
        private TextBox txtNama, txtEmail, txtTelepon, txtPassword;
        private Button btnSave, btnCancel;
        private Label lblHalo;

        private Image SetImageOpacity(Image image, float opacity)
        {
            Bitmap bmp = new Bitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                ColorMatrix matrix = new ColorMatrix();
                matrix.Matrix33 = opacity;
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                g.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height),
                    0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
            }
            return bmp;
        }

        public FormProfileUser()
        {
            InitializeComponent();
            InitializeUserData();
            BuildUI();
            SetupEvents();
        }

        private void InitializeUserData()
        {
            currentUser = new UserProfile
            {
                Nama = "Felicia Angel",
                Email = "test@example.com",
                Telepon = "+62 812-3456-7890",
                Password = "password123",
                FotoProfil = null
            };
        }

        private void BuildUI()
        {
            this.Text = "NaviGO - Profil";
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // SIDEBAR
            sidebar = new Panel()
            {
                BackColor = Color.FromArgb(225, 240, 240),
                Width = 250,
                Dock = DockStyle.Left
            };

            Panel logoPanel = new Panel()
            {
                Height = 120,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(200, 230, 225)
            };

            PictureBox logo = new PictureBox()
            {
                Size = new Size(60, 60),
                Location = new Point(20, 25),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent,
                Image = Properties.Resources.logo_navigo
            };

            Label lblSidebarTitle = new Label()
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

            logoPanel.Controls.AddRange(new Control[] { logo, lblSidebarTitle, lblLogoSubtitle });


            Button btnRiwayat = new Button()
            {
                Text = "  Riwayat Pemesanan",
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0)
            };
            btnRiwayat.FlatAppearance.BorderSize = 0;
            btnRiwayat.Click += (s, e) =>
            {
                this.Hide();
                new UserHistory().Show();
            };

            Button btnJadwal = new Button()
            {
                Text = "  Jadwal dan Rute",
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.White,
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

            // Hover effects
            foreach (Button btn in new[] { btnJadwal, btnRiwayat })
            {
                btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(240, 240, 240);
                btn.MouseLeave += (s, e) => btn.BackColor = Color.White;
            }

            sidebar.Controls.AddRange(new Control[] {  btnRiwayat, btnJadwal, logoPanel });

            // TOP HEADER
            topHeader = new Panel()
            {
                BackColor = Color.FromArgb(0, 85, 92),
                Height = 70,
                Dock = DockStyle.Top
            };

            Label lblHeaderTitle = new Label()
            {
                Text = "Profil Saya",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(280, 25)
            };

            lblHalo = new Label()
            {
                Text = $"Halo, {currentUser.Nama}",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10),
                AutoSize = true
            };

            Button btnProfile = new Button()
            {
                Text = "Profil",
                BackColor = Color.FromArgb(0, 85, 92),
                ForeColor = Color.White,
                Width = 80,
                Height = 35,
                Font = new Font("Segoe UI", 9),
                FlatStyle = FlatStyle.Flat
            };
            btnProfile.FlatAppearance.BorderSize = 1;
            btnProfile.FlatAppearance.BorderColor = Color.White;


            Button btnLogout = new Button()
            {
                Text = "Logout",
                BackColor = Color.FromArgb(210, 80, 60),
                Width = 80,
                Height = 35,
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

            // Position controls in topHeader
            topHeader.Controls.Add(lblHeaderTitle);
            topHeader.Controls.Add(lblHalo);
            topHeader.Controls.Add(btnProfile);
            topHeader.Controls.Add(btnLogout);

            topHeader.Resize += (s, e) =>
            {
                btnLogout.Location = new Point(topHeader.Width - 100, 18);
                btnProfile.Location = new Point(topHeader.Width - 190, 18);
                lblHalo.Location = new Point(topHeader.Width - 300, 25);
            };

            // CONTENT PANEL
            contentPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                BackgroundImageLayout = ImageLayout.Stretch,
                BackColor = Color.White
            };

            contentPanel.BackgroundImage = SetImageOpacity(Properties.Resources.tiket_bg, 0.3f);

            contentBox = new Panel()
            {
                Size = new Size(900, 480),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            contentPanel.Controls.Add(contentBox);
            contentPanel.Resize += (s, e) =>
            {
                contentBox.Location = new Point(
                    (contentPanel.Width - contentBox.Width) / 2,
                    (contentPanel.Height - contentBox.Height) / 2
                );
            };

            // FOTO PROFIL
            profilePic = new PictureBox()
            {
                Size = new Size(120, 120),
                Location = new Point(50, 30),
                BackColor = Color.FromArgb(245, 245, 245),
                SizeMode = PictureBoxSizeMode.Zoom,
                Cursor = Cursors.Hand,
                BorderStyle = BorderStyle.None
            };

            var gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddEllipse(0, 0, profilePic.Width, profilePic.Height);
            profilePic.Region = new Region(gp);

            profilePic.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (Pen p = new Pen(Color.FromArgb(0, 85, 92), 2))
                    e.Graphics.DrawEllipse(p, 1, 1, profilePic.Width - 3, profilePic.Height - 3);
            };

            SetDefaultProfilePicture();
            contentBox.Controls.Add(profilePic);

            // Label edit
            lblEdit = new Label()
            {
                Text = "✎ Edit Profil",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 85, 92),
                Cursor = Cursors.Hand,
                AutoSize = true,
                Location = new Point(50, 160)
            };
            contentBox.Controls.Add(lblEdit);

            // INPUT FIELDS - Layout lebih rapi seperti di gambar
            int leftLabel = 300;
            int leftField = 300;
            int startY = 30;
            int spacing = 60;

            // Nama Lengkap
            Label lblNama = new Label()
            {
                Text = "Nama Lengkap",
                Location = new Point(leftLabel, startY),
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                ForeColor = Color.FromArgb(80, 80, 80)
            };
            contentBox.Controls.Add(lblNama);

            txtNama = new TextBox()
            {
                Size = new Size(400, 35),
                Location = new Point(leftField, startY + 25),
                Text = currentUser.Nama,
                ReadOnly = true,
                BackColor = Color.FromArgb(250, 250, 250),
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10)
            };
            contentBox.Controls.Add(txtNama);

            // Email
            Label lblEmail = new Label()
            {
                Text = "Email",
                Location = new Point(leftLabel, startY + spacing),
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                ForeColor = Color.FromArgb(80, 80, 80)
            };
            contentBox.Controls.Add(lblEmail);

            txtEmail = new TextBox()
            {
                Size = new Size(400, 35),
                Location = new Point(leftField, startY + spacing + 25),
                Text = currentUser.Email,
                ReadOnly = true,
                BackColor = Color.FromArgb(250, 250, 250),
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10)
            };
            contentBox.Controls.Add(txtEmail);

            // Nomor Telepon
            Label lblTelepon = new Label()
            {
                Text = "Nomor Telepon",
                Location = new Point(leftLabel, startY + spacing * 2),
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                ForeColor = Color.FromArgb(80, 80, 80)
            };
            contentBox.Controls.Add(lblTelepon);

            txtTelepon = new TextBox()
            {
                Size = new Size(400, 35),
                Location = new Point(leftField, startY + spacing * 2 + 25),
                Text = currentUser.Telepon,
                ReadOnly = true,
                BackColor = Color.FromArgb(250, 250, 250),
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10)
            };
            contentBox.Controls.Add(txtTelepon);

            // Password
            Label lblPassword = new Label()
            {
                Text = "Password",
                Location = new Point(leftLabel, startY + spacing * 3),
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                ForeColor = Color.FromArgb(80, 80, 80)
            };
            contentBox.Controls.Add(lblPassword);

            txtPassword = new TextBox()
            {
                Size = new Size(400, 35),
                Location = new Point(leftField, startY + spacing * 3 + 25),
                Text = "••••••••",
                ReadOnly = true,
                BackColor = Color.FromArgb(250, 250, 250),
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10),
                UseSystemPasswordChar = true
            };
            contentBox.Controls.Add(txtPassword);

            // Show/Hide Password Button
            Button btnShowPassword = new Button()
            {
                Text = "👁",
                Size = new Size(40, 35),
                Location = new Point(leftField + 410, startY + spacing * 3 + 25),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Tag = txtPassword,
                Font = new Font("Segoe UI", 10)
            };
            btnShowPassword.FlatAppearance.BorderSize = 1;
            btnShowPassword.FlatAppearance.BorderColor = Color.LightGray;
            btnShowPassword.Click += BtnShow_Click;
            contentBox.Controls.Add(btnShowPassword);

            // BUTTON SAVE & CANCEL
            btnSave = new Button()
            {
                Text = "Simpan Perubahan",
                BackColor = Color.FromArgb(0, 85, 92),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(140, 40),
                Location = new Point(leftField, startY + spacing * 4 + 10),
                Visible = false,
                Font = new Font("Segoe UI", 10)
            };
            btnSave.FlatAppearance.BorderSize = 0;
            contentBox.Controls.Add(btnSave);

            btnCancel = new Button()
            {
                Text = "Batal",
                BackColor = Color.FromArgb(150, 150, 150),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 40),
                Location = new Point(leftField + 150, startY + spacing * 4 + 10),
                Visible = false,
                Font = new Font("Segoe UI", 10)
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            contentBox.Controls.Add(btnCancel);

            // TAMBAHKAN KE FORM dengan urutan yang benar
            this.Controls.Add(sidebar);
            this.Controls.Add(topHeader);
            this.Controls.Add(contentPanel);
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            TextBox txt = btn.Tag as TextBox;
            if (txt.UseSystemPasswordChar)
            {
                txt.UseSystemPasswordChar = false;
                btn.Text = "🙈";
                if (isEditMode) txt.Text = currentUser.Password;
            }
            else
            {
                txt.UseSystemPasswordChar = true;
                btn.Text = "👁";
                if (!isEditMode) txt.Text = "••••••••";
            }
        }

        private void SetupEvents()
        {
            lblEdit.Click += (s, e) => EnableEditMode();
            btnSave.Click += (s, e) => SaveProfile();
            btnCancel.Click += (s, e) => CancelEdit();
            profilePic.Click += (s, e) => ChangeProfilePicture();
        }

        private void SetDefaultProfilePicture()
        {
            Bitmap defaultPic = new Bitmap(profilePic.Width, profilePic.Height);
            using (Graphics g = Graphics.FromImage(defaultPic))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.FromArgb(245, 245, 245));
                using (Font font = new Font("Segoe UI", 35, FontStyle.Bold))
                using (Brush brush = new SolidBrush(Color.FromArgb(0, 85, 92)))
                    g.DrawString("👤", font, brush, new PointF(30, 25));
            }
            profilePic.Image = defaultPic;
        }

        private void ChangeProfilePicture()
        {
            if (!isEditMode)
            {
                MessageBox.Show("Masuk mode edit terlebih dahulu!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                ofd.Title = "Pilih Foto Profil";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Image img = Image.FromFile(ofd.FileName);
                        profilePic.Image = ResizeImage(img, profilePic.Width, profilePic.Height);
                        currentUser.FotoProfil = ofd.FileName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private Image ResizeImage(Image image, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, 0, 0, width, height);
            }
            return bmp;
        }

        private void EnableEditMode()
        {
            isEditMode = true;
            txtNama.ReadOnly = txtEmail.ReadOnly = txtTelepon.ReadOnly = txtPassword.ReadOnly = false;
            txtNama.BackColor = txtEmail.BackColor = txtTelepon.BackColor = txtPassword.BackColor = Color.White;
            txtPassword.Text = currentUser.Password;
            txtPassword.UseSystemPasswordChar = false;

            btnSave.Visible = true;
            btnCancel.Visible = true;
            lblEdit.Visible = false;
        }

        private void DisableEditMode()
        {
            isEditMode = false;
            txtNama.ReadOnly = txtEmail.ReadOnly = txtTelepon.ReadOnly = txtPassword.ReadOnly = true;
            txtNama.BackColor = txtEmail.BackColor = txtTelepon.BackColor = txtPassword.BackColor = Color.FromArgb(250, 250, 250);
            txtPassword.Text = "••••••••";
            txtPassword.UseSystemPasswordChar = true;

            btnSave.Visible = false;
            btnCancel.Visible = false;
            lblEdit.Visible = true;
        }

        private void SaveProfile()
        {
            if (string.IsNullOrWhiteSpace(txtNama.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Nama dan Email harus diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            currentUser.Nama = txtNama.Text.Trim();
            currentUser.Email = txtEmail.Text.Trim();
            currentUser.Telepon = txtTelepon.Text.Trim();
            currentUser.Password = txtPassword.Text;

            lblHalo.Text = $"Halo, {currentUser.Nama}";
            DisableEditMode();
            MessageBox.Show("Profil berhasil diperbarui!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CancelEdit()
        {
            txtNama.Text = currentUser.Nama;
            txtEmail.Text = currentUser.Email;
            txtTelepon.Text = currentUser.Telepon;
            txtPassword.Text = "••••••••";
            DisableEditMode();
        }
    }

    public class UserProfile
    {
        public string Nama { get; set; }
        public string Email { get; set; }
        public string Telepon { get; set; }
        public string Password { get; set; }
        public string FotoProfil { get; set; }
    }
}