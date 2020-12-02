using InfluxDB.Client.Core;

namespace Isaac_DataService.Services
{
    [Measurement("sensorhumidity")]
    public class HumidityData : SensorData
    {
        public HumidityData(float value, string x, string y, string floor) : base(x, y, floor, SensorType.Humidity)
        {
            Value = value;
        }

        [Column("value")] public float Value { get; set; }
    }
}