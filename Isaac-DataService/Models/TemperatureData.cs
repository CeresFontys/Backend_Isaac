using InfluxDB.Client.Core;

namespace Isaac_DataService.Services
{
    [Measurement("sensortemperature")]
    public class TemperatureData : SensorData
    {
        public TemperatureData(float value, string x, string y, string floor) : base(x, y, floor, SensorType.Temperature)
        {
            Value = value;
        }

        [Column("value")] public float Value { get; set; }
    }
}