using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Isaac_AnomalyService.Models
{
    public class SensorData
    {
        public string Id { get; set; }
        public DateTime DateTime { get; set; }
        public double Value { get; set; }
        public int Floor { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public DataType Type { get; set; }


    }




    public enum DataType
    {
        Temperature,
        Humidity
    }
}
