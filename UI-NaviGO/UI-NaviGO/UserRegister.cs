using System;
using System.Drawing;
using System.Windows.Forms;
using Npgsql;

namespace UI_NaviGO
{
    public partial class UserRegister : Form
    {
        private TextBox txtName;
        private TextBox txtEmail;
        private TextBox txtPassword;
        private TextBox txtConfirm;

        // ===== CONNECTION STRING SUPABASE =====
        private string connString =
        "Host=aws-1-ap-southeast-1.pooler.supabase.com;" +
        "Port=6543;" +
        "Username=postgres.zsktvbvfquecdmndgyrz;" +
        "Password=agathahusna;" +
        "Database=postgres;" +
        "Ssl Mode=Require;" +
        "Trust Server Certificate=true;";

        public UserRegister()
        {
            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text = "NaviGo - Register";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // LEFT IMAGE
            PictureBox pictureBox = new PictureBox
            {
                Size = new Size(600, 700),
                Location = new Point(0, 0),
                Image = Properties.Resources.register_bg,
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            this.Controls.Add(pictureBox);

            // TITLE
            Label lblTitle = new Label
            {
                Text = "Create Account",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                Location = new Point(730, 120),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            Label lblSubtitle = new Label
            {
                Text = "Please complete to create your account",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                Location = new Point(700, 165),
                AutoSize = true
            };
            this.Controls.Add(lblSubtitle);

            // NAME
            Label lblName = new Label
            {
                Text = "Name",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(700, 220)
            };
            this.Controls.Add(lblName);

            txtName = new TextBox
            {
                Size = new Size(400, 35),
                Location = new Point(700, 245),
                BackColor = Color.FromArgb(175, 240, 220),
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtName);

            // EMAIL
            Label lblEmail = new Label
            {
                Text = "Email",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(700, 295)
            };
            this.Controls.Add(lblEmail);

            txtEmail = new TextBox
            {
                Size = new Size(400, 35),
                Location = new Point(700, 320),
                BackColor = Color.FromArgb(175, 240, 220),
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtEmail);

            // PASSWORD
            Label lblPassword = new Label
            {
                Text = "Password",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(700, 370)
            };
            this.Controls.Add(lblPassword);

            txtPassword = new TextBox
            {
                Size = new Size(400, 35),
                Location = new Point(700, 395),
                BackColor = Color.FromArgb(175, 240, 220),
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 10),
                PasswordChar = '*'
            };
            this.Controls.Add(txtPassword);

            // CONFIRM PASSWORD
            Label lblConfirm = new Label
            {
                Text = "Confirm Password",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(700, 445)
            };
            this.Controls.Add(lblConfirm);

            txtConfirm = new TextBox
            {
                Size = new Size(400, 35),
                Location = new Point(700, 470),
                BackColor = Color.FromArgb(175, 240, 220),
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 10),
                PasswordChar = '*'
            };
            this.Controls.Add(txtConfirm);

            // SIGN UP BUTTON
            Button btnSignUp = new Button
            {
                Text = "Sign Up",
                Size = new Size(400, 45),
                Location = new Point(700, 530),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(20, 150, 130),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSignUp.FlatAppearance.BorderSize = 0;
            btnSignUp.Click += BtnSignUp_Click;
            this.Controls.Add(btnSignUp);
        }

        // REGISTER FUNCTION
        private void BtnSignUp_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirm = txtConfirm.Text.Trim();

            if (string.IsNullOrEmpty(name) ||
                string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirm))
            {
                MessageBox.Show("Please fill in all fields.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirm)
            {
                MessageBox.Show("Passwords do not match.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();

                    // CHECK IF EMAIL EXISTS
                    string checkQuery = "SELECT COUNT(*) FROM users WHERE email = @e";
                    using (var checkCmd = new NpgsqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@e", email);
                        long exists = (long)checkCmd.ExecuteScalar();

                        if (exists > 0)
                        {
                            MessageBox.Show("Email already registered.",
                                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // INSERT NEW USER
                    string insertQuery =
                        "INSERT INTO users (name, email, password) VALUES (@n, @e, @p)";
                    using (var cmd = new NpgsqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@n", name);
                        cmd.Parameters.AddWithValue("@e", email);
                        cmd.Parameters.AddWithValue("@p", password);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Account created successfully!",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Hide();
                    new UserLogin().Show();
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