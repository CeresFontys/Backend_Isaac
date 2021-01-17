using System;
using InfluxDB.Client.Core;

namespace Isaac_DataService.Services
{
    public abstract class SensorData : ISensorData, IComparable<SensorData>
    {
        [Column(IsTimestamp = true)] public DateTime Time { get; set; }
        public SensorType Type { get; set; }
        [Column("x", IsTag = true)] public string X { get; set; }
        [Column("y", IsTag = true)] public string Y { get; set; }
        [Column("floor", IsTag = true)] public string Floor { get; set; }

        protected SensorData(string x, string y, string floor, SensorType type, DateTime time = default)
        {
            Time = time == default ? DateTime.UtcNow : time;
            X = x;
            Y = y;
            Floor = floor;
            Type = type;
        }

        public SensorData()
        {
            
        }

        public int CompareTo(SensorData other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var xComparison = int.Parse(X).CompareTo(int.Parse(other.X));
            return xComparison != 0 ? xComparison : int.Parse(Y).CompareTo(int.Parse(other.Y));
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