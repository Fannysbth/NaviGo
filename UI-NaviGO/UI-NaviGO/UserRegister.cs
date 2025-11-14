using System;
using System.Drawing;
using System.Windows.Forms;

namespace UI_NaviGO
{
    public partial class UserRegister : Form
    {
        private TextBox txtName;
        private TextBox txtEmail;
        private TextBox txtPassword;
        private TextBox txtConfirm;

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

            // ======== LEFT IMAGE PANEL ========
            PictureBox pictureBox = new PictureBox
            {
                Size = new Size(600, 700),
                Location = new Point(0, 0),
                Image = Properties.Resources.register_bg,
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            this.Controls.Add(pictureBox);

            // ======== TITLE ========
            Label lblTitle = new Label
            {
                Text = "Create Account",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(730, 120),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            Label lblSubtitle = new Label
            {
                Text = "Please complete to create your account",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.Gray,
                Location = new Point(700, 165),
                AutoSize = true
            };
            this.Controls.Add(lblSubtitle);

            // ======== NAME ========
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

            // ======== EMAIL ========
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

            // ======== PASSWORD ========
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

            // ======== CONFIRM PASSWORD ========
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

            // ======== SIGN UP BUTTON ========
            Button btnSignUp = new Button
            {
                Text = "Sign Up",
                Size = new Size(400, 45),
                Location = new Point(700, 530),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(20, 150, 130),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSignUp.FlatAppearance.BorderSize = 0;
            btnSignUp.Click += BtnSignUp_Click;
            this.Controls.Add(btnSignUp);

            // ======== BACK TO LOGIN LINK ========
            Label lblLogin = new Label
            {
                Text = "Already have an account?",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Black,
                Location = new Point(750, 590),
                AutoSize = true
            };
            this.Controls.Add(lblLogin);

            LinkLabel linkLogin = new LinkLabel
            {
                Text = "Sign In",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                LinkColor = Color.FromArgb(0, 120, 120),
                Location = new Point(920, 590),
                AutoSize = true,
                LinkBehavior = LinkBehavior.NeverUnderline
            };
            linkLogin.Click += (s, e) =>
            {
                this.Hide();
                new UserLogin().Show();
            };
            this.Controls.Add(linkLogin);
        }

        private void BtnSignUp_Click(object sender, EventArgs e)
        {
            // Validasi input
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtConfirm.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtPassword.Text != txtConfirm.Text)
            {
                MessageBox.Show("Password and confirmation do not match.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtPassword.Text.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters long.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Coba registrasi user
            bool success = UserManager.RegisterUser(
                txtName.Text.Trim(),
                txtEmail.Text.Trim(),
                txtPassword.Text
            );

            if (success)
            {
                MessageBox.Show("Account created successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                new UserLogin().Show();
            }
            else
            {
                MessageBox.Show("Email already registered. Please use a different email.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}