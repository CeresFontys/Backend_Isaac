using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Isaac_AnomalyService.Models;
using Isaac_AnomalyService.Service;
using Microsoft.Extensions.Configuration;

namespace Isaac_AnomalyService.Logic
{
    public class OutlierAlgo
    {
        //Get Config parameters
        public OutlierAlgo(IConfiguration configuration)
        { 
            maxDifferenceTemp = configuration.GetValue<double>("AlgoConfig:MaxDifferenceTemp");
            maxDifferenceHum = configuration.GetValue<double>("AlgoConfig:MaxDifferenceHum");
            parameterExtremeTopTemp = configuration.GetValue<int>("AlgoConfig:ParameterExtremeTopTemp");
            parameterExtremeBottomTemp = configuration.GetValue<int>("AlgoConfig:ParameterExtremeBottomTemp");
            parameterExtremeTopHum = configuration.GetValue<int>("AlgoConfig:ParameterExtremeTopHum");
            parameterExtremeBottomHum = configuration.GetValue<int>("AlgoConfig:ParameterExtremeBottomHum");
            parameterTopTemp = configuration.GetValue<int>("AlgoConfig:ParameterTopTemp");
            parameterBottomTemp = configuration.GetValue<int>("AlgoConfig:ParameterBottomTemp");
            parameterTopHum = configuration.GetValue<int>("AlgoConfig:ParameterTopHum");
            parameterBottomHum = configuration.GetValue<int>("AlgoConfig:ParameterBottomHum");
        }

        private double maxDifferenceTemp;
        private double maxDifferenceHum;
        private int parameterExtremeTopTemp;
        private int parameterExtremeBottomTemp;
        private int parameterExtremeTopHum;
        private int parameterExtremeBottomHum;
        private int parameterTopTemp;
        private int parameterBottomTemp;
        private int parameterTopHum;
        private int parameterBottomHum;

        private List<SensorData> temperatureSensorList = new List<SensorData>();
        private List<SensorData> humiditySensorList = new List<SensorData>();

        private double averageTemp;
        private double averageHum;

        //Sort sensor data on value
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

        //Get average sensor temp
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

        //Get average sensor hum
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

        public async Task RunOutlierAlgo()
        {
            await GetAverageTemp();
            await GetAverageHum();
            await CheckExtremity(temperatureSensorList, parameterExtremeTopTemp, parameterExtremeBottomTemp, parameterTopTemp, parameterBottomTemp, maxDifferenceTemp);
            await CheckExtremity(humiditySensorList, parameterExtremeTopHum, parameterExtremeBottomHum, parameterTopHum, parameterBottomHum, maxDifferenceHum);
        }


        //Check outlier Anomaly's
        private async Task CheckOutlierTemp(SensorData sensor, List<SensorData> sensorDataList, double maxDifference)
        {
            SensorData SensorNext;
            SensorData SensorPrev;
            double relativeDifference = maxDifference;
           

            
                //Get all the data from 1 sensor in the list
                var condition = sensorDataList.Where(pos => pos.X == sensor.X && pos.Y == sensor.Y).ToList();

                //Check if the sensor is not the last
                if (sensor != condition[^1])
                {
                    //Get next sensor
                    SensorNext = condition.SkipWhile(x => x != sensor).Skip(1).DefaultIfEmpty(condition[0]).FirstOrDefault();

                    //Calc minute dif
                    var minuteDif = (SensorNext.DateTime - sensor.DateTime).TotalMinutes;

                    //Flag sensor
                    if (!IsValueValid(sensor.Value, SensorNext.Value, minuteDif, maxDifference))
                    {
                        string writeLine = "Type: " + sensor.Type + "My X and Y: " + sensor.X + "," + sensor.Y + " My value: " + sensor.Value + " My Date time: " + sensor.DateTime + " difference with next sensor is to big!";
                        Console.WriteLine(writeLine);
                    }
                    
                }
                //check if the sensor is not the first
                if (sensor != condition[0])
                {
                    //get prev sensor
                    SensorPrev = condition.TakeWhile(x => x != sensor).DefaultIfEmpty(sensorDataList[condition.Count - 1]).LastOrDefault();

                    //Calc minute dif
                    var minuteDif = (sensor.DateTime - SensorPrev.DateTime ).TotalMinutes;

                    //Flag sensor
                    if (!IsValueValid(sensor.Value, SensorPrev.Value, minuteDif, maxDifference))
                    {
                        string writeLine = "Type: " + sensor.Type + "My X and Y: " + sensor.X + "," + sensor.Y + " My value: " + sensor.Value + " My Date time: " + sensor.DateTime + " difference with previous sensor is to big!";
                        Console.WriteLine(writeLine);
                    }
                }
        }

        //check if sensor exceeds extreme parameters 
        public async Task CheckExtremity(List<SensorData> sensorDataList, int parameterExtremeTop, int parameterExtremeBottom, int parameterTop,int parameterBottom, double maxDifference)
        {
            foreach (SensorData sensor in sensorDataList)
            {
                if (sensor.Value >= parameterExtremeTop || sensor.Value <= parameterExtremeBottom)
                {
                    string writeLine ="Type: "+ sensor.Type +"My X and Y: " + sensor.X +","+ sensor.Y + " My value: " + sensor.Value + " My Date time: " + sensor.DateTime + " EXTREME Anomaly!";
                    Console.WriteLine(writeLine);
                }else if (sensor.Value >= parameterTop || sensor.Value <= parameterBottom)
                {
                    string writeLine = "Type: " + sensor.Type + "My X and Y: " + sensor.X + "," + sensor.Y + " My value: " + sensor.Value + " My Date time: " + sensor.DateTime + " normal Anomaly!";
                    Console.WriteLine(writeLine);
                }
                else
                {
                   await CheckOutlierTemp(sensor, sensorDataList, maxDifference);
                }
            }
        }


        //Calculate the relative difference 2 sensors may differ 
        public bool IsValueValid(double first, double second, double minuteDif, double maxDifference)
        {
            var relativeDifference = maxDifference * minuteDif;

            return !(Math.Abs(first - second) >= relativeDifference);

        }
    }
}
