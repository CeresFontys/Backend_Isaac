using InfluxDB.Client.Core;

namespace Isaac_DataService.Services
{
    [Measurement("sensoruptime")]
    public class UptimeData : SensorData
    {
        public UptimeData(long value, string x, string y, string floor) : base(x, y, floor, SensorType.Uptime)
        {
            Value = value;
        }

        public UptimeData()
        {
            
        }

        [Column("value")] public long Value { get; set; }
    }
}