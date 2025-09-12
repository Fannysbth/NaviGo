using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviGo
{
    internal class Customer : User
    {
        public List<Booking> Bookings { get; set; }

        public Customer() : base()
        {
            Role = Role.Customer;
            Bookings = new List<Booking>();
        }

        public Customer(int userId, string username, string email)
            : base(userId, username, email, Role.Customer)
        {
            Bookings = new List<Booking>();
        }

        public void SearchSchedule()
        {
            Console.WriteLine($"Customer {Username} is searching for schedules.");
        }

        public void MakeBooking()
        {
            Console.WriteLine($"Customer {Username} is making a booking.");
        }

        public void ViewBookingHistory()
        {
            Console.WriteLine($"Customer {Username} is viewing booking history.");
            if (Bookings.Count == 0)
            {
                Console.WriteLine("No bookings found.");
            }
            else
            {
                foreach (var booking in Bookings)
                {
                    Console.WriteLine($"Booking ID: {booking.BookingId}, Date: {booking.BookingDate}, Status: {booking.Status}");
                }
            }
        }
    }
}