using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviGo
{

    public class Schedule
    {
        public int ScheduleId { get; set; }
        public int ShipId { get; set; }
        public string? Departure { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureTime { get; set; }
        public decimal Price { get; set; }


        public Schedule(int scheduleId, int shipId, string departure, string destination, DateTime departureTime, decimal price)
        {
            ScheduleId = scheduleId;
            ShipId = shipId;
            Departure = departure;
            Destination = destination;
            DepartureTime = departureTime;
            Price = price;
        }

        public void UpdateSchedule(string newDeparture = null, string newDestination = null, DateTime? newDepartureTime = null, decimal? newPrice = null)
        {
            if (!string.IsNullOrEmpty(newDeparture))
                Departure = newDeparture;

            if (!string.IsNullOrEmpty(newDestination))
                Destination = newDestination;

            if (newDepartureTime.HasValue)
                DepartureTime = newDepartureTime.Value;

            if (newPrice.HasValue)
                Price = newPrice.Value;

            Console.WriteLine($"Schedule {ScheduleId} updated successfully");
        }
    }
}

