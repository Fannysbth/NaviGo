using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviGo
{
    internal class Admin : User
    {
        // Constructor lengkap: langsung set Role Admin
        public Admin(int userId, string username, string email)
            : base(userId, username, email, UserRole.Admin)
        {
        }

        // Jika mau constructor kosong, beri nilai default untuk base
        public Admin() 
            : base(0, "defaultAdmin", "admin@example.com", UserRole.Admin)
        {
        }

        public void ManageSchedule()
        {
            Console.WriteLine($"Admin {Username} is managing schedules.");
        }

        public void ManageBooking()
        {
            Console.WriteLine($"Admin {Username} is managing bookings.");
        }
    }
}
