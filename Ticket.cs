using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviGo
{
    internal class Ticket
    {
        public int TicketId { get; set; }
        public string PassengerName { get; set; }
        public DateTime Date { get; set; }
        public Booking Booking { get; set; } // Relasi ke Booking

        public Ticket()
        {
        }

        public Ticket(int ticketId, string passengerName, DateTime date)
        {
            TicketId = ticketId;
            PassengerName = passengerName;
            Date = date;
        }

        public void DownloadTicket()
        {
            Console.WriteLine($"Downloading ticket for {PassengerName}...");
            Console.WriteLine($"Ticket ID: {TicketId}");
            Console.WriteLine($"Date: {Date:yyyy-MM-dd HH:mm}");
            Console.WriteLine("Ticket downloaded successfully.");
        }
    }
}