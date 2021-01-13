using System;
using Isaac_DataService.Services;

namespace Isaac_DataService.Controllers
{
    public class FullSensorData : ISensorData
    {
        public FullSensorData()
        {
            
        }
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        
        public DateTime Time { get; set; }
        public SensorType Type { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public string Floor { get; set; }
    }
}