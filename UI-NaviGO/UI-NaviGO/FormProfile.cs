using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI_NaviGO
{
    public partial class FormProfileUser : Form
    {
        // Data user (sementara hardcode, nanti bisa dari database)
        private UserProfile currentUser;
        private bool isEditMode = false;
        private TextBox txtNama, txtEmail, txtTelepon, txtPassword;
        private PictureBox profilePic;
        private Button btnSave, btnCancel;
        private Label btnEdit;

        public FormProfileUser()
        {
            // HAPUS: InitializeComponent(); // Karena sudah ada di designer
            InitializeUserData();
            BuildCustomUI(); // Ganti nama method untuk hindari duplikasi
            SetupEvents();
            this.Opacity = 1;
            this.FormClosed += (s, e) => Application.Exit();
        }

        private void InitializeUserData()
        {
            // Data user dummy - nanti bisa diganti dengan data dari database
            currentUser = new UserProfile
            {
                Nama = "Felicia Angel",
                Email = "felicia.angel@example.com",
                Telepon = "+62 812-3456-7890",
                Password = "password123", // Dalam real app, ini harus hash
                FotoProfil = null
            };
        }

        // 🔹 Efek transisi antar form (fade) - GUNAKAN NAMA BERBEDA
        private async void NavigateToForm(Form nextForm)
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

        private void BuildCustomUI()
        {
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
            Button btnDashboard = CreateSidebarButtonCustom("Dashboard", 130);
            Button btnJadwal = CreateSidebarButtonCustom("Jadwal dan Rute", 180);
            Button btnRiwayat = CreateSidebarButtonCustom("Riwayat Pemesanan", 230);
            Button btnProfile = CreateSidebarButtonCustom("Profil Saya", 280);
            btnProfile.BackColor = Color.FromArgb(200, 230, 225);

            sidebar.Controls.AddRange(new Control[] { btnDashboard, btnJadwal, btnRiwayat, btnProfile });

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
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9)
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
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9)
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

            // === CONTENT PROFIL ===
            Panel content = new Panel
            {
                Size = new Size(700, 550),
                Location = new Point(400, 100),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Parent = overlay
            };
            overlay.Controls.Add(content);
            content.BringToFront();

            // Title
            Label lblTitle = new Label
            {
                Text = "Profil Saya",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 90, 100),
                AutoSize = true,
                Location = new Point(300, 20)
            };
            content.Controls.Add(lblTitle);

            // Foto profil bundar
            profilePic = new PictureBox
            {
                Size = new Size(130, 130),
                Location = new Point((content.Width - 130) / 2, 60),
                BackColor = Color.FromArgb(210, 240, 240),
                SizeMode = PictureBoxSizeMode.Zoom,
                Cursor = Cursors.Hand,
                BorderStyle = BorderStyle.None
            };

            // Buat foto profil circular
            var gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddEllipse(0, 0, profilePic.Width, profilePic.Height);
            profilePic.Region = new Region(gp);

            // Gambar border circular
            profilePic.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (Pen p = new Pen(Color.FromArgb(0, 90, 100), 3))
                    e.Graphics.DrawEllipse(p, 1, 1, profilePic.Width - 3, profilePic.Height - 3);
            };

            // Default profile picture
            SetDefaultProfilePicture();
            content.Controls.Add(profilePic);

            // Label upload foto
            Label lblUpload = new Label
            {
                Text = "Klik untuk ganti foto",
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point((content.Width - 100) / 2, 195),
                Cursor = Cursors.Hand
            };
            content.Controls.Add(lblUpload);

            // Tombol edit
            btnEdit = new Label
            {
                Text = "✎ Edit Profil",
                ForeColor = Color.FromArgb(0, 90, 100),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                AutoSize = true,
                Location = new Point(content.Width - 120, 220),
                BackColor = Color.Transparent
            };
            content.Controls.Add(btnEdit);

            // Field informasi user
            txtNama = CreateInputFieldCustom(content, "Nama Lengkap", 250, currentUser.Nama);
            txtEmail = CreateInputFieldCustom(content, "Email", 310, currentUser.Email);
            txtTelepon = CreateInputFieldCustom(content, "Nomor Telepon", 370, currentUser.Telepon);
            txtPassword = CreateInputFieldCustom(content, "Password", 430, "••••••••", true);

            // Tombol Simpan dan Batal (awalnya hidden)
            btnSave = new Button
            {
                Text = "Simpan Perubahan",
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(120, 35),
                Location = new Point(450, 490),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Visible = false
            };
            btnSave.FlatAppearance.BorderSize = 0;
            content.Controls.Add(btnSave);

            btnCancel = new Button
            {
                Text = "Batal",
                BackColor = Color.FromArgb(240, 90, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(80, 35),
                Location = new Point(580, 490),
                Font = new Font("Segoe UI", 9),
                Visible = false
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            content.Controls.Add(btnCancel);
        }

        private void SetupEvents()
        {
            // Event tombol sidebar
            var sidebar = this.Controls[2]; // Panel sidebar
            var buttons = sidebar.Controls;

            ((Button)buttons[1]).Click += (s, e) => { this.Hide(); new UserDashboard().Show(); }; // Dashboard
            ((Button)buttons[2]).Click += (s, e) => { this.Hide(); new UserJadwal().Show(); }; // Jadwal
            ((Button)buttons[3]).Click += (s, e) => { this.Hide(); new UserHistory().Show(); }; // Riwayat
            ((Button)buttons[4]).Click += (s, e) => { /* Already in profile */ }; // Profile

            // Event tombol header
            var header = this.Controls[1]; // Panel header
            var headerButtons = header.Controls;

            ((Button)headerButtons[1]).Click += (s, e) => { /* Already in profile */ }; // Profile
            ((Button)headerButtons[2]).Click += (s, e) =>
            {
                if (MessageBox.Show("Yakin ingin logout?", "Konfirmasi",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Hide();
                    new UserLogin().Show();
                }
            }; // Logout

            // Event foto profil
            profilePic.Click += (s, e) => ChangeProfilePicture();

            // Cari label upload foto dan tambahkan event
            foreach (Control control in ((Panel)this.Controls[0].Controls[0]).Controls)
            {
                if (control is Panel contentPanel && control.Text == "")
                {
                    foreach (Control contentControl in contentPanel.Controls)
                    {
                        if (contentControl is Label label && label.Text == "Klik untuk ganti foto")
                        {
                            label.Click += (s, e) => ChangeProfilePicture();
                            break;
                        }
                    }
                    break;
                }
            }

            // Event tombol edit
            btnEdit.Click += (s, e) => EnableEditMode();

            // Event tombol simpan dan batal
            btnSave.Click += (s, e) => SaveProfile();
            btnCancel.Click += (s, e) => CancelEdit();
        }

        private TextBox CreateInputFieldCustom(Panel parent, string label, int y, string value, bool isPassword = false)
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
                Location = new Point(100, y + 25),
                Font = new Font("Segoe UI", 10),
                Text = value,
                ReadOnly = true,
                BackColor = Color.FromArgb(250, 250, 250),
                BorderStyle = BorderStyle.FixedSingle,
                ForeColor = Color.Black
            };

            if (isPassword)
            {
                txt.UseSystemPasswordChar = true;
                // Tambahkan tombol show/hide password
                Button btnShowPassword = new Button
                {
                    Text = "👁",
                    Size = new Size(40, 32),
                    Location = new Point(605, y + 25),
                    BackColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10),
                    Cursor = Cursors.Hand,
                    Tag = txt
                };
                btnShowPassword.FlatAppearance.BorderSize = 1;
                btnShowPassword.Click += (s, e) => TogglePasswordVisibility(btnShowPassword);
                parent.Controls.Add(btnShowPassword);
            }

            parent.Controls.Add(txt);
            return txt;
        }

        private void TogglePasswordVisibility(Button btn)
        {
            TextBox txtPassword = btn.Tag as TextBox;
            if (txtPassword != null)
            {
                if (txtPassword.UseSystemPasswordChar)
                {
                    txtPassword.UseSystemPasswordChar = false;
                    btn.Text = "👁";
                }
                else
                {
                    txtPassword.UseSystemPasswordChar = true;
                    btn.Text = "👁";
                }
            }
        }

        private Button CreateSidebarButtonCustom(string text, int top)
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
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(190, 225, 225);
            btn.MouseLeave += (s, e) =>
            {
                if (btn.Text != "Profil Saya")
                    btn.BackColor = Color.White;
            };
            return btn;
        }

        private void SetDefaultProfilePicture()
        {
            // Buat gambar profil default
            Bitmap defaultPic = new Bitmap(profilePic.Width, profilePic.Height);
            using (Graphics g = Graphics.FromImage(defaultPic))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.FromArgb(210, 240, 240));

                // Gambar icon orang
                using (Font font = new Font("Segoe UI", 40, FontStyle.Bold))
                using (Brush brush = new SolidBrush(Color.FromArgb(0, 90, 100)))
                {
                    g.DrawString("👤", font, brush, new PointF(25, 25));
                }
            }
            profilePic.Image = defaultPic;
        }

        private void ChangeProfilePicture()
        {
            if (!isEditMode)
            {
                MessageBox.Show("Silakan masuk ke mode edit terlebih dahulu untuk mengubah foto profil.", "Info",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                openFileDialog.Title = "Pilih Foto Profil";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Load dan resize image
                        Image originalImage = Image.FromFile(openFileDialog.FileName);
                        Image resizedImage = ResizeImage(originalImage, profilePic.Width, profilePic.Height);

                        profilePic.Image = resizedImage;
                        currentUser.FotoProfil = openFileDialog.FileName;

                        MessageBox.Show("Foto profil berhasil diubah!", "Sukses",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}", "Error",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private Image ResizeImage(Image image, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, 0, 0, width, height);
            }
            return result;
        }

        private void EnableEditMode()
        {
            isEditMode = true;

            // Enable textboxes
            txtNama.ReadOnly = false;
            txtNama.BackColor = Color.White;

            txtEmail.ReadOnly = false;
            txtEmail.BackColor = Color.White;

            txtTelepon.ReadOnly = false;
            txtTelepon.BackColor = Color.White;

            txtPassword.ReadOnly = false;
            txtPassword.BackColor = Color.White;
            txtPassword.Text = currentUser.Password; // Show actual password in edit mode
            txtPassword.UseSystemPasswordChar = false;

            // Show save and cancel buttons
            btnSave.Visible = true;
            btnCancel.Visible = true;

            // Hide edit button
            btnEdit.Visible = false;

            MessageBox.Show("Mode edit diaktifkan. Anda dapat mengubah data profil.", "Edit Mode",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DisableEditMode()
        {
            isEditMode = false;

            // Disable textboxes
            txtNama.ReadOnly = true;
            txtNama.BackColor = Color.FromArgb(250, 250, 250);

            txtEmail.ReadOnly = true;
            txtEmail.BackColor = Color.FromArgb(250, 250, 250);

            txtTelepon.ReadOnly = true;
            txtTelepon.BackColor = Color.FromArgb(250, 250, 250);

            txtPassword.ReadOnly = true;
            txtPassword.BackColor = Color.FromArgb(250, 250, 250);
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.Text = "••••••••";

            // Hide save and cancel buttons
            btnSave.Visible = false;
            btnCancel.Visible = false;

            // Show edit button
            btnEdit.Visible = true;
        }

        private void SaveProfile()
        {
            if (!ValidateInput())
                return;

            // Update user data
            currentUser.Nama = txtNama.Text.Trim();
            currentUser.Email = txtEmail.Text.Trim();
            currentUser.Telepon = txtTelepon.Text.Trim();

            // Only update password if changed
            if (txtPassword.Text != currentUser.Password && txtPassword.Text != "••••••••")
            {
                currentUser.Password = txtPassword.Text;
            }

            // Simpan ke database (simulasi)
            SimpanKeDatabase();

            // Update header name
            UpdateHeaderName();

            DisableEditMode();

            MessageBox.Show("Profil berhasil diperbarui!", "Sukses",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CancelEdit()
        {
            // Reset fields to original values
            txtNama.Text = currentUser.Nama;
            txtEmail.Text = currentUser.Email;
            txtTelepon.Text = currentUser.Telepon;
            txtPassword.Text = "••••••••";

            DisableEditMode();

            MessageBox.Show("Perubahan dibatalkan.", "Info",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtNama.Text))
            {
                MessageBox.Show("Nama lengkap harus diisi!", "Validasi Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNama.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Format email tidak valid!", "Validasi Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTelepon.Text))
            {
                MessageBox.Show("Nomor telepon harus diisi!", "Validasi Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTelepon.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text) || txtPassword.Text == "••••••••")
            {
                MessageBox.Show("Password harus diisi!", "Validasi Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }

            if (txtPassword.Text.Length < 6)
            {
                MessageBox.Show("Password harus minimal 6 karakter!", "Validasi Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }

            return true;
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

        private void SimpanKeDatabase()
        {
            // Simulasi penyimpanan ke database
            // Dalam implementasi nyata, ini akan menyimpan ke PostgreSQL
            Console.WriteLine($"Menyimpan profil user ke database:");
            Console.WriteLine($"- Nama: {currentUser.Nama}");
            Console.WriteLine($"- Email: {currentUser.Email}");
            Console.WriteLine($"- Telepon: {currentUser.Telepon}");
            Console.WriteLine($"- Password: {currentUser.Password}");
            Console.WriteLine($"- Foto: {currentUser.FotoProfil}");
        }

        private void UpdateHeaderName()
        {
            // Update nama di header
            var header = this.Controls[1]; // Panel header
            foreach (Control control in header.Controls)
            {
                if (control is Label label && label.Text.StartsWith("Halo,"))
                {
                    label.Text = $"Halo, {currentUser.Nama}";
                    break;
                }
            }
        }
    }

    // Class untuk menyimpan data profil user
    public class UserProfile
    {
        public string Nama { get; set; }
        public string Email { get; set; }
        public string Telepon { get; set; }
        public string Password { get; set; }
        public string FotoProfil { get; set; }
    }
}