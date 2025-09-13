using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviGo
{

    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Cancelled
    }

    public class Booking
    {
        public string BookingId { get; set; }
        public DateTime BookingDate { get; set; }
        public BookingStatus Status { get; set; }
        public decimal TotalPrice { get; set; }
        public List<Ticket> Tickets { get; set; }

        public Booking(string bookingId, DateTime bookingDate, BookingStatus status, decimal totalPrice)
        {
            BookingId = bookingId;
            BookingDate = bookingDate;
            Status = status;
            TotalPrice = totalPrice;
            Tickets = new List<Ticket>();
        }

        public void ConfirmBooking()
        {
            Status = BookingStatus.Confirmed;
            Console.WriteLine($"Booking {BookingId} confirmed successfully");
        }

        public void CancelBooking()
        {
            Status = BookingStatus.Cancelled;
            Console.WriteLine($"Booking {BookingId} cancelled successfully");
        }

        public void RescheduleBooking(Schedule newSchedule)
        {
            if (newSchedule != null && Status == BookingStatus.Confirmed)
            {
                Console.WriteLine($"Booking {BookingId} rescheduled to new schedule {newSchedule.ScheduleId}");
                // Logic untuk reschedule bisa ditambahkan di sini
            }
            else
            {
                Console.WriteLine("Cannot reschedule booking. Invalid schedule or booking not confirmed.");
            }
        }
    }
}
