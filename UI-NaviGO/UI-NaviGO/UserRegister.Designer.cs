using System.Windows.Forms;
using System.Drawing;

namespace UI_NaviGO
{
    partial class UserRegister
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
            this.SuspendLayout();
            // 
            // UserRegister
            // x
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 450);
            this.Name = "UserRegister";
            this.Text = "UserRegister";
            this.ResumeLayout(false);

        }
    }
}
