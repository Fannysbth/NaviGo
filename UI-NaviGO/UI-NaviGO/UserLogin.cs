using System;
using System.Drawing;
using System.Windows.Forms;
using Npgsql;

namespace UI_NaviGO
{
    public partial class UserLogin : Form
    {
        // ===== KONFIGURASI DATABASE SUPABASE =====
        private string connString =
        "Host=aws-1-ap-southeast-1.pooler.supabase.com;" +
        "Port=5432;" +
        "Username=postgres.zsktvbvfquecdmndgyrz;" +
        "Password=agathahusna;" +
        "Database=postgres;" +
        "Ssl Mode=Require;" +
        "Trust Server Certificate=true;";

        private TextBox txtEmail;
        private TextBox txtPassword;

        public UserLogin()
        {
            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text = "NaviGo - User Login";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // ===== LEFT IMAGE PANEL =====
            PictureBox pictureBox = new PictureBox
            {
                Size = new Size(600, 700),
                Location = new Point(0, 0),
                Image = Properties.Resources.login_bg,
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            this.Controls.Add(pictureBox);

            // ===== TITLE =====
            Label lblTitle = new Label
            {
                Text = "User Login",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(780, 150),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            Label lblSubtitle = new Label
            {
                Text = "Welcome back! Please sign in to continue.",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                Location = new Point(740, 195),
                AutoSize = true
            };
            this.Controls.Add(lblSubtitle);

            // ===== EMAIL =====
            Label lblEmail = new Label
            {
                Text = "Email",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(740, 250)
            };
            this.Controls.Add(lblEmail);

            txtEmail = new TextBox
            {
                Size = new Size(350, 35),
                Location = new Point(740, 275),
                BackColor = Color.FromArgb(175, 240, 220),
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtEmail);

            // ===== PASSWORD =====
            Label lblPassword = new Label
            {
                Text = "Password",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(740, 330)
            };
            this.Controls.Add(lblPassword);

            txtPassword = new TextBox
            {
                Size = new Size(350, 35),
                Location = new Point(740, 355),
                BackColor = Color.FromArgb(175, 240, 220),
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 10),
                PasswordChar = '*'
            };
            this.Controls.Add(txtPassword);

            // ===== LOGIN BUTTON =====
            Button btnLogin = new Button
            {
                Text = "Login",
                Size = new Size(350, 45),
                Location = new Point(740, 420),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(20, 150, 130),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;
            this.Controls.Add(btnLogin);

            // ===== REGISTER LINK =====
            Label lblRegister = new Label
            {
                Text = "No Account Yet?",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Black,
                Location = new Point(800, 480),
                AutoSize = true
            };
            this.Controls.Add(lblRegister);

            LinkLabel linkRegister = new LinkLabel
            {
                Text = "Create Account",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                LinkColor = Color.FromArgb(0, 120, 120),
                Location = new Point(915, 480),
                AutoSize = true,
                LinkBehavior = LinkBehavior.NeverUnderline
            };
            linkRegister.Click += (s, e) =>
            {
                this.Hide();
                new UserRegister().Show();
            };
            this.Controls.Add(linkRegister);
        }

        // ================= LOGIN FUNCTION ==================
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both email and password.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();

                    // Cek user di Supabase
                    string query = "SELECT * FROM users WHERE email = @e AND password = @p";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@e", email);
                        cmd.Parameters.AddWithValue("@p", password);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                reader.Close();

                                // Catat login ke login_history
                                string logQuery = "INSERT INTO login_history (email, login_time) VALUES (@e, CURRENT_TIMESTAMP)";
                                using (var cmdLog = new NpgsqlCommand(logQuery, conn))
                                {
                                    cmdLog.Parameters.AddWithValue("@e", email);
                                    cmdLog.ExecuteNonQuery();
                                }

                                MessageBox.Show("Login successful!",
                                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                this.Hide();
                                new UserJadwal().Show();
                            }
                            else
                            {
                                MessageBox.Show("Invalid email or password.",
                                    "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database connection failed:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}