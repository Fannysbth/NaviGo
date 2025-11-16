using Npgsql;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI_NaviGO
{
    public partial class FormProfileUser : Form
    {
        private List<TextBox> inputFields = new List<TextBox>();
        private Button btnSave;
        private PictureBox profilePic;
        private Label btnEdit;
        private List<PictureBox> avatarChoices = new List<PictureBox>();
        private bool isEditMode = false;
        private Panel avatarPanel;

        private string connString =
            "Host=aws-1-ap-southeast-1.pooler.supabase.com;" +
            "Port=5432;" +
            "Username=postgres.zsktvbvfquecdmndgyrz;" +
            "Password=agathahusna;" +
            "Database=postgres;" +
            "Ssl Mode=Require;" +
            "Trust Server Certificate=true;" +
            "Pooling=true;" +
            "Timeout=30;";

        public FormProfileUser()
        {
            InitializeComponent();
            BuildUI();
            this.Opacity = 1;
            this.FormClosed += (s, e) => Application.Exit();
        }

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
            // ========== FORM ==========
            this.Text = "NaviGo - Profile";
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // ================== SIDEBAR ==================
            Panel sidebarPanel = new Panel()
            {
                BackColor = Color.FromArgb(225, 240, 240),
                Width = 250,
                Dock = DockStyle.Left
            };

            // Logo panel
            Panel logoPanel = new Panel()
            {
                Height = 120,
                Dock = DockStyle.Top
            };

            PictureBox logo = new PictureBox()
            {
                Size = new Size(60, 60),
                Location = new Point(20, 25),
                Image = Properties.Resources.logo_navigo,
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            Label lblLogoTitle = new Label()
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

            logoPanel.Controls.AddRange(new Control[] { logo, lblLogoTitle, lblLogoSubtitle });

            // Sidebar buttons
            Button btnJadwal = new Button()
            {
                Text = "  Jadwal dan Rute",
                BackColor = Color.White,
                Dock = DockStyle.Top,
                Height = 45,
                ForeColor = Color.FromArgb(0, 85, 92),
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

            Button btnRiwayat = new Button()
            {
                Text = "Riwayat Pemesanan",
                Dock = DockStyle.Top,
                Height = 45,
                ForeColor = Color.FromArgb(0, 85, 92),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                BackColor = Color.White
            };
            btnRiwayat.FlatAppearance.BorderSize = 0;
            btnRiwayat.Click += (s, e) =>
            {
                this.Hide();
                new UserHistory().Show();
            };

            sidebarPanel.Controls.AddRange(new Control[] { btnRiwayat, btnJadwal, logoPanel });

            // ================== TOP PANEL ==================
            Panel topPanel = new Panel()
            {
                BackColor = Color.Teal,
                Height = 70,
                Dock = DockStyle.Top
            };

            Label lblHeaderTitle = new Label()
            {
                Text = "Profil Saya",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(15, 22)
            };

            Label lblUsername = new Label()
            {
                Text = "Halo, " + UserSession.Name,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.White,
                AutoSize = true
            };

            Button btnProfile = new Button()
            {
                Text = "Profile",
                Width = 90,
                Height = 35,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9),
                FlatStyle = FlatStyle.Flat
            };
            btnProfile.FlatAppearance.BorderSize = 0;
            btnProfile.Click += (s, e) =>
            {
                var p = new FormProfileUser();
                p.Closed += (s2, e2) => this.Close();
                p.Show();
                this.Hide();
            };

            Button btnLogout = new Button()
            {
                Text = "Logout",
                Width = 90,
                Height = 35,
                BackColor = Color.FromArgb(210, 80, 60),
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

            topPanel.Resize += (s, e) =>
            {
                btnLogout.Location = new Point(topPanel.Width - 110, 18);
                btnProfile.Location = new Point(topPanel.Width - 210, 18);
                lblUsername.Location = new Point(topPanel.Width - 350, 22);
            };

            topPanel.Controls.AddRange(new Control[] { lblHeaderTitle, lblUsername, btnProfile, btnLogout });

            // CONTENT
            Panel content = new Panel
            {
                Dock = DockStyle.Fill,
                BackgroundImageLayout = ImageLayout.Stretch,
                BackColor = Color.White
            };

            content.BackgroundImage = SetImageOpacity(Properties.Resources.tiket_bg, 0.3f);

            Panel contentBox = new Panel()
            {
                Size = new Size(900, 550),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            content.Controls.Add(contentBox);

            content.Resize += (s, e) =>
            {
                contentBox.Location = new Point(
                    (content.Width - contentBox.Width) / 2,
                    (content.Height - contentBox.Height) / 2
                );
            };

            this.Controls.Add(content);
            this.Controls.Add(topPanel);
            this.Controls.Add(sidebarPanel);
            content.BringToFront();

            // Header
            Label lblheader = new Label()
            {
                Text = "Welcome To Navigo!!",
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                AutoSize = true,
                Location = new Point((contentBox.Width - 250) / 2, 30)
            };
            contentBox.Controls.Add(lblheader);

            // Profile Picture
            profilePic = new PictureBox
            {
                Size = new Size(130, 130),
                Location = new Point((contentBox.Width - 130) / 2, 80),
                BackColor = Color.FromArgb(210, 240, 240),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BorderStyle = BorderStyle.FixedSingle
            };
            MakeCircle(profilePic);
            contentBox.Controls.Add(profilePic);

            // Edit Label
            btnEdit = new Label
            {
                Text = "✎ Edit",
                ForeColor = Color.FromArgb(240, 90, 50),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                AutoSize = true,
                Location = new Point(contentBox.Width - 220, 250)
            };
            contentBox.Controls.Add(btnEdit);
            btnEdit.Click += BtnEdit_Click;

            // Avatar Selection Panel (hidden by default)
            avatarPanel = new Panel()
            {
                Size = new Size(330, 80),
                Location = new Point((contentBox.Width - 300) / 2, 220),
                BackColor = Color.FromArgb(245, 245, 245),
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };
            contentBox.Controls.Add(avatarPanel);

            // Create avatar choices
            CreateAvatarChoices();

            // INPUT FIELDS
            CreateInputField(contentBox, "Nama", 300, UserSession.Name);
            CreateInputField(contentBox, "Email", 360, UserSession.Email);
            CreateInputField(contentBox, "Nomor Telepon", 420, UserSession.Phone);

            // Save Button
            btnSave = new Button
            {
                Text = "Save",
                Size = new Size(120, 40),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(0, 120, 100),
                ForeColor = Color.White,
                Location = new Point((contentBox.Width - 120) / 2, 480),
                Visible = false
            };
            btnSave.Click += BtnSave_Click;
            contentBox.Controls.Add(btnSave);

            // Load user data
            LoadUserData();
        }

        private void CreateInputField(Panel parent, string label, int y, string value)
        {
            Label lbl = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(40, 40, 40),
                Location = new Point(200, y),
                AutoSize = true
            };
            parent.Controls.Add(lbl);

            TextBox txt = new TextBox
            {
                Size = new Size(500, 32),
                Location = new Point(200, y + 22),
                Font = new Font("Segoe UI", 10),
                Text = value,
                ReadOnly = true,
                BackColor = Color.FromArgb(175, 240, 220),
                BorderStyle = BorderStyle.None,

            };
            parent.Controls.Add(txt);

            inputFields.Add(txt);
        }

        private void CreateAvatarChoices()
        {
            Image[] images = new Image[]
            {
                Properties.Resources.avatar1,
                Properties.Resources.avatar2,
                Properties.Resources.avatar3,
                Properties.Resources.avatar4
            };

            int startX = 20;
            int startY = 10;
            int size = 60;
            int spacing = 20;

            for (int i = 0; i < images.Length; i++)
            {
                PictureBox pb = new PictureBox
                {
                    Size = new Size(size, size),
                    Location = new Point(startX + i * (size + spacing), startY),
                    Image = images[i],
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    BorderStyle = BorderStyle.FixedSingle,
                    Cursor = Cursors.Hand,
                    Tag = i
                };
                MakeCircle(pb);
                pb.Click += Avatar_Click;
                avatarPanel.Controls.Add(pb);
                avatarChoices.Add(pb);
            }
        }

        private void Avatar_Click(object sender, EventArgs e)
        {
            if (!isEditMode) return;

            PictureBox clicked = sender as PictureBox;
            int avatarId = (int)clicked.Tag;

            // Update profile picture
            profilePic.Image = clicked.Image;
            profilePic.Tag = avatarId;

            // Highlight selected avatar
            foreach (var pb in avatarChoices)
            {
                if (pb == clicked)
                {
                    pb.BackColor = Color.Teal;
                    pb.BorderStyle = BorderStyle.Fixed3D;
                }
                else
                {
                    pb.BackColor = Color.Transparent;
                    pb.BorderStyle = BorderStyle.FixedSingle;
                }
            }
        }

        private void LoadUserData()
        {
            try
            {
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    string query = "SELECT name, email, phone, avatar_id FROM users WHERE user_id = @id";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", UserSession.UserId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Update input fields
                                if (inputFields.Count >= 3)
                                {
                                    inputFields[0].Text = reader["name"].ToString();
                                    inputFields[1].Text = reader["email"].ToString();
                                    inputFields[2].Text = reader["phone"] != DBNull.Value ? reader["phone"].ToString() : "";
                                }

                                // Load avatar
                                int avatarId = reader["avatar_id"] != DBNull.Value ? Convert.ToInt32(reader["avatar_id"]) : 0;
                                LoadAvatar(avatarId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAvatar(int avatarId)
        {
            Image[] images = new Image[]
            {
                Properties.Resources.avatar1,
                Properties.Resources.avatar2,
                Properties.Resources.avatar3,
                Properties.Resources.avatar4
            };

            if (avatarId >= 0 && avatarId < images.Length)
            {
                profilePic.Image = images[avatarId];
                profilePic.Tag = avatarId;
            }
            else
            {
                profilePic.Image = images[0];
                profilePic.Tag = 0;
            }

            // Highlight selected avatar
            for (int i = 0; i < avatarChoices.Count; i++)
            {
                if (i == avatarId)
                {
                    avatarChoices[i].BackColor = Color.Teal;
                    avatarChoices[i].BorderStyle = BorderStyle.Fixed3D;
                }
                else
                {
                    avatarChoices[i].BackColor = Color.Transparent;
                    avatarChoices[i].BorderStyle = BorderStyle.FixedSingle;
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            isEditMode = !isEditMode;

            if (isEditMode)
            {
                btnEdit.Text = "X Cancel";
                foreach (var txt in inputFields)
                {
                    txt.ReadOnly = false;
                    BackColor = Color.FromArgb(175, 240, 220);
               
                }

                avatarPanel.Visible = true;
                btnSave.Visible = true;
            }
            else
            {
                // Cancel edit mode
                btnEdit.Text = "✎ Edit";
                foreach (var txt in inputFields)
                {
                    txt.ReadOnly = true;
                    txt.BackColor = Color.FromArgb(175, 240, 220);
                }

                avatarPanel.Visible = false;
                btnSave.Visible = false;

                // Reload original data
                LoadUserData();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string newName = inputFields[0].Text;
            string newEmail = inputFields[1].Text;
            string newPhone = inputFields[2].Text;
            int avatarId = profilePic.Tag != null ? (int)profilePic.Tag : 0;

            // Validation
            if (string.IsNullOrWhiteSpace(newName) || string.IsNullOrWhiteSpace(newEmail))
            {
                MessageBox.Show("Nama dan Email tidak boleh kosong!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    string query = @"
                        UPDATE users SET 
                            name = @n, email = @e, phone = @p, avatar_id = @a
                        WHERE user_id = @id";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@n", newName);
                        cmd.Parameters.AddWithValue("@e", newEmail);
                        cmd.Parameters.AddWithValue("@p", newPhone);
                        cmd.Parameters.AddWithValue("@a", avatarId);
                        cmd.Parameters.AddWithValue("@id", UserSession.UserId);
                        cmd.ExecuteNonQuery();
                    }
                }

                // Update session
                UserSession.Name = newName;
                UserSession.Email = newEmail;
                UserSession.Phone = newPhone;
                UserSession.AvatarId = avatarId;

                // Exit edit mode
                isEditMode = false;
                btnEdit.Text = "✎ Edit";
                foreach (var txt in inputFields)
                {
                    txt.ReadOnly = true;
                    BackColor = Color.FromArgb(175, 240, 220);
                }

                avatarPanel.Visible = false;
                btnSave.Visible = false;

                MessageBox.Show("Profil berhasil diperbarui!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating profile: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MakeCircle(PictureBox pb)
        {
            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(0, 0, pb.Width - 1, pb.Height - 1);
            pb.Region = new Region(gp);
        }

        private Image SetImageOpacity(Image image, float opacity)
        {
            Bitmap bmp = new Bitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                ColorMatrix m = new ColorMatrix();
                m.Matrix33 = opacity;

                ImageAttributes ia = new ImageAttributes();
                ia.SetColorMatrix(m, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                g.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height),
                    0, 0, image.Width, image.Height,
                    GraphicsUnit.Pixel, ia);
            }
            return bmp;
        }
    }
}