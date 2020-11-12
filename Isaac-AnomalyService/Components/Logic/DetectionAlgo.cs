using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Isaac_AnomalyService.Models;
using Isaac_AnomalyService.Service;
using Microsoft.Extensions.Configuration;

namespace Isaac_AnomalyService.Logic
{
    public class DetectionAlgo
    {
        
        public DetectionAlgo(IConfiguration configuration)
        { 
            maxIncrease = configuration.GetValue<int>("AlgoConfig:MaxIncrease");
        }

        private int maxIncrease;

        private List<SensorData> temperatureSensorList = new List<SensorData>();
        private List<SensorData> humiditySensorList = new List<SensorData>();

        private double averageTemp;
        private double averageHum;

     
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

        public async Task GetAverageTemp()
        {
            double all = 0;
            int count = temperatureSensorList.Count;

            foreach (SensorData sensor in temperatureSensorList)
            {
                all += sensor.Value;
            }

            averageTemp = all / count;
        }

        public async Task GetAverageHum()
        {
            double all = 0;
            int count = humiditySensorList.Count;

            foreach (SensorData sensor in humiditySensorList)
            {
                all += sensor.Value;
            }

            averageHum = all / count;
        }

        public async Task CheckOutlierTemp()
        {

           
            foreach (SensorData sensor in temperatureSensorList)
            {
                SensorData SensorNext = temperatureSensorList.SkipWhile(x => x != sensor).Skip(1).DefaultIfEmpty(temperatureSensorList[0]).FirstOrDefault();
                SensorData SensorPrev = temperatureSensorList.TakeWhile(x => x != sensor).DefaultIfEmpty(temperatureSensorList[temperatureSensorList.Count - 1]).LastOrDefault();
                if ((sensor.Value + 5) >= SensorNext.Value)
                {
                    Console.WriteLine("Anomaly!");
                }

            }
        }

    }
}
