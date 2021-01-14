using InfluxDB.Client.Core;

namespace Isaac_DataService.Services
{
    [Measurement("sensorHumidity")]
    public class HumidityData : SensorData
    {
        public HumidityData(float value, string x, string y, string floor) : base(x, y, floor, SensorType.Humidity)
        {
            Value = value;
        }

        public HumidityData()
        {
            Type = SensorType.Humidity;
        }

        [Column("value")] public float Value { get; set; }
    }
}