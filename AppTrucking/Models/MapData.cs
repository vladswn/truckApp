using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppTrucking.Models
{
    public class MapData
    {
        public int MapDataId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public double Distance { get; set; }
        //время отправки
        //public DateTime DispatchTime { get; set; }
        //Длительность
        public double Duration { get; set; }
        public int OrderId { get; set; }

        public virtual Order Order { get; set; }
    }
}