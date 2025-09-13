using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NaviGo
{

    public class Route
    {
        public int RouteId { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Duration { get; set; }

        public Route(int routeId, string origin, string destination, string duration)
        {
            RouteId = routeId;
            Origin = origin;
            Destination = destination;
            Duration = duration;
        }
    }
}

