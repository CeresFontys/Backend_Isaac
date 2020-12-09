//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Threading.Tasks;
//using Isaac_AnomalyService.Components.Logic;
//using Isaac_AnomalyService.Components.Logic.Algoritm;
//using Isaac_AnomalyService.Models;
//using Isaac_AnomalyService.Service;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;

//namespace Isaac_AnomalyService.Logic
//{
//    public class OutlierAlgo
//    {

//        private readonly ILogger<OutlierAlgo> _logger;
//        private readonly CheckExtremeTopLeaf _outlierLeaf;


//        //Get Config parameters
//        public OutlierAlgo(IConfiguration configuration, ILogger<OutlierAlgo> logger, CheckExtremeTopLeaf outlierLeaf)
//        {
//            _logger = logger;
//            _outlierLeaf = outlierLeaf;
//            _maxDifferenceTemp = configuration.GetValue<double>("AlgoConfig:MaxDifferenceTemp");
//            _maxDifferenceHum = configuration.GetValue<double>("AlgoConfig:MaxDifferenceHum");
//            _parameterExtremeTopTemp = configuration.GetValue<int>("AlgoConfig:ParameterExtremeTopTemp");
//            _parameterExtremeBottomTemp = configuration.GetValue<int>("AlgoConfig:ParameterExtremeBottomTemp");
//            _parameterExtremeTopHum = configuration.GetValue<int>("AlgoConfig:ParameterExtremeTopHum");
//            _parameterExtremeBottomHum = configuration.GetValue<int>("AlgoConfig:ParameterExtremeBottomHum");
//            _parameterTopTemp = configuration.GetValue<int>("AlgoConfig:ParameterTopTemp");
//            _parameterBottomTemp = configuration.GetValue<int>("AlgoConfig:ParameterBottomTemp");
//            _parameterTopHum = configuration.GetValue<int>("AlgoConfig:ParameterTopHum");
//            _parameterBottomHum = configuration.GetValue<int>("AlgoConfig:ParameterBottomHum");
//        }

//        public OutlierAlgo(double maxDifferenceTemp, double maxDifferenceHum, int parameterExtremeTopTemp, int parameterExtremeBottomTemp, int parameterExtremeTopHum, int parameterExtremeBottomHum, int parameterTopTemp, int parameterBottomTemp, int parameterTopHum, int parameterBottomHum, ILogger<OutlierAlgo> logger, CheckExtremeTopLeaf outlierLeaf)
//        {
//            _maxDifferenceTemp = maxDifferenceTemp;
//            _maxDifferenceHum = maxDifferenceHum;
//            _parameterExtremeTopTemp = parameterExtremeTopTemp;
//            _parameterExtremeBottomTemp = parameterExtremeBottomTemp;
//            _parameterExtremeTopHum = parameterExtremeTopHum;
//            _parameterExtremeBottomHum = parameterExtremeBottomHum;
//            _parameterTopTemp = parameterTopTemp;
//            _parameterBottomTemp = parameterBottomTemp;
//            _parameterTopHum = parameterTopHum;
//            _parameterBottomHum = parameterBottomHum;
//            _logger = logger;
//            _outlierLeaf = outlierLeaf;
//        }


//        private readonly double _maxDifferenceTemp;
//        private readonly double _maxDifferenceHum;
//        private readonly int _parameterExtremeTopTemp;
//        private readonly int _parameterExtremeBottomTemp;
//        private readonly int _parameterExtremeTopHum;
//        private readonly int _parameterExtremeBottomHum;
//        private readonly int _parameterTopTemp;
//        private readonly int _parameterBottomTemp;
//        private readonly int _parameterTopHum;
//        private readonly int _parameterBottomHum;

//        public readonly List<SensorData> temperatureSensorList = new List<SensorData>();
//        public readonly List<SensorData> humiditySensorList = new List<SensorData>();
//        public readonly List<SensorError> sensorErrorList = new List<SensorError>();
//        private readonly ValidationComposite validate = new ValidationComposite();


//        private double weatherApiTemp;
//        private int weatherApiHum;

//        private double averageTemp;
//        private double averageHum;

//        //Sort sensor data on value
//        public void SortData(SensorData sensor)
//        {
//            if (sensor.Type == DataType.Temperature)
//            {
//                temperatureSensorList.Add(sensor);
//            }
//            else
//            {
//                humiditySensorList.Add(sensor);
//            }
//        }

//        public void SetWeatherApiData(WeatherApiWield weatherApiWield)
//        {
//            weatherApiTemp = weatherApiWield.ApiTemp;
//            weatherApiHum = weatherApiWield.ApiHum;
//            _logger.LogWarning("Api temp is:" + weatherApiTemp);
//            _logger.LogWarning("Api hum is:" + weatherApiHum);
//        }

//        //Get average sensor temp
//        private void GetAverageTemp()
//        {
//            double all = 0;
//            var count = temperatureSensorList.Count;

//            foreach (SensorData sensor in temperatureSensorList)
//            {
//                all += sensor.Value;
//            }

//            averageTemp = all / count;
//        }

//        //Get average sensor hum
//        private void GetAverageHum()
//        {
//            double all = 0;
//            var count = humiditySensorList.Count;

//            foreach (SensorData sensor in humiditySensorList)
//            {
//                all += sensor.Value;
//            }

//            averageHum = all / count;
//        }

//        public List<SensorError> RunOutlierAlgo()
//        {
//            //MOCK
//            temperatureSensorList.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 150, X = 1, Y = 1, Type = DataType.Temperature });
//            temperatureSensorList.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 20, X = 1, Y = 1, Type = DataType.Temperature });

//            List<SensorError> sensorErrorList = new List<SensorError>();
//            GetAverageTemp();
//            GetAverageHum();
//            sensorErrorList.AddRange(CheckExtremity(temperatureSensorList, _parameterExtremeTopTemp, _parameterExtremeBottomTemp, _parameterTopTemp, _parameterBottomTemp, _maxDifferenceTemp));
//            sensorErrorList.AddRange(CheckExtremity(humiditySensorList, _parameterExtremeTopHum, _parameterExtremeBottomHum, _parameterTopHum, _parameterBottomHum, _maxDifferenceHum));
//            return sensorErrorList;
//        }


//        //Check outlier Anomaly's
//        private void CheckOutlierTemp(List<SensorError> sensorErrorList, SensorData sensor, List<SensorData> sensorDataList, double maxDifference)
//        {

//            //Get all the data from 1 sensor in the list
//            var condition = sensorDataList.Where(pos => pos.X == sensor.X && pos.Y == sensor.Y).ToList();

//            //Check if the sensor is not the last
//            if (sensor != condition[^1])
//            {

//                //Get next sensor
//                SensorData sensorNext = condition.SkipWhile(x => x != sensor).Skip(1).DefaultIfEmpty(condition[0]).FirstOrDefault();

//                //Calc minute dif
//                var minuteDif = (sensorNext.DateTime - sensor.DateTime).TotalMinutes;

//                //Flag sensor
//                if (!validate.IsValueValid(sensor.Value, sensorNext.Value, minuteDif, maxDifference))
//                {
//                    SensorError sensorError = new SensorError(sensor.X, sensor.Y, sensor.Floor.ToString(), "Difference with next sensor is to big: " + sensor.Value, sensor.DateTime, SensorError.ErrorType.NextDif, sensor.Value, sensorNext.Value);
//                    string writeLine = "Type: " + sensor.Type + " My X and Y: " + sensor.X + "," + sensor.Y + " My value: " + sensor.Value + " My Date time: " + sensor.DateTime + " difference with next sensor is to big!";
//                    Console.WriteLine(writeLine);
//                    sensorErrorList.Add(sensorError);
//                }

//            }
//            //check if the sensor is not the first
//            if (sensor != condition[0])
//            {
//                //get prev sensor
//                SensorData sensorPrev = condition.TakeWhile(x => x != sensor).DefaultIfEmpty(sensorDataList[condition.Count - 1]).LastOrDefault();

//                //Calc minute dif
//                var minuteDif = (sensor.DateTime - sensorPrev.DateTime).TotalMinutes;

//                //Flag sensor
//                if (!validate.IsValueValid(sensor.Value, sensorPrev.Value, minuteDif, maxDifference))
//                {
//                    SensorError sensorError = new SensorError(sensor.X, sensor.Y, sensor.Floor.ToString(), "Difference with previous sensor is to bigs: " + sensor.Value, sensor.DateTime, SensorError.ErrorType.PrevDif, sensorPrev.Value, sensor.Value);
//                    string writeLine = "Type: " + sensor.Type + " My X and Y: " + sensor.X + "," + sensor.Y + " My value: " + sensor.Value + " My Date time: " + sensor.DateTime + " difference with previous sensor is to big!";
//                    Console.WriteLine(writeLine);
//                    sensorErrorList.Add(sensorError);
//                }
//            }
//        }

//        //check if sensor exceeds extreme parameters 
//        private List<SensorError> CheckExtremity(List<SensorData> sensorDataList, int parameterExtremeTop, int parameterExtremeBottom, int parameterTop, int parameterBottom, double maxDifference)
//        {


//            foreach (SensorData sensor in sensorDataList)
//            {
//                if (sensor.Value >= parameterExtremeTop || sensor.Value <= parameterExtremeBottom)
//                {
//                    //check Extreme Top




//                    //if (sensor.Value >= parameterExtremeTop)
//                    //{
//                    //    SensorError sensorError = new SensorError(sensor.X, sensor.Y, sensor.Floor.ToString(), "Sensor exceeds extreme given parameters: " + sensor.Value, sensor.DateTime, SensorError.ErrorType.ExtremeTop, sensor.Value);
//                    //    sensorErrorList.Add(sensorError);
//                    //}

//                    ////Check Extreme Bottom
//                    //else
//                    //{
//                    //    SensorError sensorError = new SensorError(sensor.X, sensor.Y, sensor.Floor.ToString(), "Sensor exceeds extreme given parameters: " + sensor.Value, sensor.DateTime, SensorError.ErrorType.ExtremeBottom, sensor.Value);
//                    //    sensorErrorList.Add(sensorError);
//                    //}
//                    string writeLine = "Type: " + sensor.Type + " My X and Y: " + sensor.X + "," + sensor.Y + " My value: " + sensor.Value + " My Date time: " + sensor.DateTime + " EXTREME Anomaly!";
//                    Console.WriteLine(writeLine);

//                }
//                else if (sensor.Value >= parameterTop || sensor.Value <= parameterBottom)
//                {
//                    //Check Normal top
//                    if (sensor.Value >= parameterTop)
//                    {
//                        SensorError sensorError = new SensorError(sensor.X, sensor.Y, sensor.Floor.ToString(), "Sensor exceeds normal given parameters: " + sensor.Value, sensor.DateTime, SensorError.ErrorType.NormalTop, sensor.Value);
//                        sensorErrorList.Add(sensorError);

//                    }
//                    //check normal bottom
//                    else
//                    {
//                        SensorError sensorError = new SensorError(sensor.X, sensor.Y, sensor.Floor.ToString(), "Sensor exceeds normal given parameters: " + sensor.Value, sensor.DateTime, SensorError.ErrorType.NormalBottom, sensor.Value);
//                        sensorErrorList.Add(sensorError);
//                    }
//                    string writeLine = "Type: " + sensor.Type + " My X and Y: " + sensor.X + "," + sensor.Y + " My value: " + sensor.Value + " My Date time: " + sensor.DateTime + " normal Anomaly!";
//                    Console.WriteLine(writeLine);

//                }
//                else
//                {
//                    CheckOutlierTemp(sensorErrorList, sensor, sensorDataList, maxDifference);
//                }


//            }

//            return sensorErrorList;
//        }
//    }
//}
