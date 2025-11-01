using System;
using System.Windows.Forms;

namespace UI_NaviGO
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new UserLogin()); // mulai dari login
        }
    }
}
