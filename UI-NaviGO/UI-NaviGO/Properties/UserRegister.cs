using System;
using System.Drawing;
using System.Windows.Forms;

namespace UI_NaviGO
{
    public partial class UserRegister : Form
    {
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

            TextBox txtName = new TextBox
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

            TextBox txtEmail = new TextBox
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

            TextBox txtPassword = new TextBox
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

            TextBox txtConfirm = new TextBox
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
            btnSignUp.Click += (s, e) =>
            {
                MessageBox.Show("Account created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                new UserLogin().Show();
            };
            this.Controls.Add(btnSignUp);
        }
    }
}
