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
            var oldData = Sensors.FirstOrDefault(sensorData =>
                sensorData.Floor == data.Floor && sensorData.X == data.X && sensorData.Y == data.Y &&
                sensorData.Type == data.Type);

            //Determine if upgrade of data is needed
            if (oldData != null && oldData.Time < data.Time)
            {
                Sensors.Remove(oldData);
                Sensors.Add(data);
            }
            else if (oldData == null)
            {
                Sensors.Add(data);
            }
        }
    }
}