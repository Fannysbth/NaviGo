using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NaviGo_CD
{
    public enum ScheduleStatus
    {
        Berlayar,
        Bersandar,
        Cancelled
    }

    public class Schedule
    {
        public string ScheduleId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public double Price { get; set; }
        public ScheduleStatus Status { get; set; }

        public void UpdateSchedule() { }
    }
}

