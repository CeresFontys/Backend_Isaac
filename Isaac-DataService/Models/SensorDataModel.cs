using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Isaac_DataService.Services
{
    public class SensorDataModel
    {
        public List<SensorData> Sensors = new List<SensorData>();

        public async Task UpdateSensor(SensorData data)
        {
            var old = Sensors.FirstOrDefault(sensorData => sensorData.Floor == data.Floor && sensorData.X == data.X && sensorData.Y == data.Y && sensorData.Type == data.Type);
            if (old != null && old.Time < data.Time)
            {
                Sensors.Remove(old);
                Sensors.Add(data);
            }
            else if (old == null)
            {
                Sensors.Add(data);
            }
        }
    }
}