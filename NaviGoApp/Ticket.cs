using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviGo
{

    public class Ticket
    {
        public int TicketId { get; set; }
        public string PassengerName { get; set; }
        public DateTime Date { get; set; }

        public Ticket(int ticketId, string passengerName, DateTime date)
        {
            TicketId = ticketId;
            PassengerName = passengerName;
            Date = date;
        }

        public void DownloadTicket()
        {
            Console.WriteLine($"Downloading ticket for {PassengerName} - Ticket ID: {TicketId}");
            // Logic untuk download ticket (bisa berupa PDF generation, dll)
        }
    }
}