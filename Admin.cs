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
        public Admin() : base()
        {
            Role = Role.Admin;
        }

        public Admin(int userId, string username, string email)
            : base(userId, username, email, Role.Admin)
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
