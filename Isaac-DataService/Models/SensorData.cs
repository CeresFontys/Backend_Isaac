using System;
using InfluxDB.Client.Core;

namespace Isaac_DataService.Services
{
    public abstract class SensorData : ISensorData
    {
        [Column(IsTimestamp = true)] public DateTime Time { get; set; }
        public SensorType Type { get; set; }
        [Column("x", IsTag = true)] public string X { get; set; }
        [Column("y", IsTag = true)] public string Y { get; set; }
        [Column("floor", IsTag = true)] public string Floor { get; set; }

        protected SensorData(string x, string y, string floor, SensorType type, DateTime time = default)
        {
            if (time == default)
            {
                Time = DateTime.UtcNow;
            }
            else
            {
                Time = time;
            }
            X = x;
            Y = y;
            Floor = floor;
            Type = type;
        }
    }

    public interface ISensorData
    {
        public DateTime Time { get; set; }
        public SensorType Type { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public string Floor { get; set; }
    }
}