using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Isaac_AnomalyService.Models;
using Isaac_AnomalyService.Service;

namespace Isaac_AnomalyService.Logic
{
    public class DetectionAlgo
    {

   

        private List<SensorData> temperatureSensorList = new List<SensorData>();
        private List<SensorData> humiditySensorList = new List<SensorData>();

     
        public async Task SortData(SensorData sensor)
        {
            if (sensor.Type == DataType.Temperature)
            {
                temperatureSensorList.Add(sensor);
            }
            else
            {
                humiditySensorList.Add(sensor);
            }
        }

    }
}
