using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviGo
{

    public class Customer : User
    {
        private List<Booking> bookings;

        public Customer(int userId, string username, string email)
            : base(userId, username, email, UserRole.Customer)
        {
            bookings = new List<Booking>();
        }

        public void SearchSchedule()
        {
            Console.WriteLine($"Customer {Username} is searching for schedules");
            // Logic untuk mencari jadwal kapal
        }

        public void MakeBooking(Schedule schedule, List<string> passengerNames)
        {
            if (schedule == null || passengerNames == null || passengerNames.Count == 0)
            {
                Console.WriteLine("Invalid booking parameters");
                return;
            }

            var booking = new Booking(
                $"BK{DateTime.Now:yyyyMMddHHmmss}",
                DateTime.Now,
                BookingStatus.Pending,
                schedule.Price * passengerNames.Count
            );

            // Create tickets for each passenger
            foreach (var passengerName in passengerNames)
            {
                var ticket = new Ticket(
                    booking.Tickets.Count + 1,
                    passengerName,
                    DateTime.Now
                );
                booking.Tickets.Add(ticket);
            }

            bookings.Add(booking);
            Console.WriteLine($"Booking {booking.BookingId} created successfully for {passengerNames.Count} passengers");
        }

        public void ViewBookingHistory()
        {
            Console.WriteLine($"Booking history for {Username}:");
            foreach (var booking in bookings)
            {
                Console.WriteLine($"- Booking ID: {booking.BookingId}, Status: {booking.Status}, Total: {booking.TotalPrice:C}");
            }
        }

        public List<Booking> GetBookings()
        {
            return bookings;
        }
    }
}