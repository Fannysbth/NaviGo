using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviGo
{

    public class Ship
    {
        public int ShipId { get; set; }
        public string ShipName { get; set; }
        public int Capacity { get; set; }
        public string ShipType { get; set; }

        public Ship(int shipId, string shipName, int capacity, string shipType)
        {
            ShipId = shipId;
            ShipName = shipName;
            Capacity = capacity;
            ShipType = shipType;
        }
    }

// Route Class


}
