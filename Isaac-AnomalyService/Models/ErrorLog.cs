using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Isaac_AnomalyService.Models
{
    public class ErrorLog
    {
        public int Id { get; set; }
        public DateTime ErrorOccurrence { get; set; }

        //public int SensorId { get; set; }

        public int Floor { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public string FlagReason { get; set; }

        public Status Status { get; set; }
    }

    public enum Status
    {
        Pending,
        Busy,
        Done
    }
}
