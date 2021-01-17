//using System;
//using System.Collections.Generic;
//using System.Text;
//using Isaac_AnomalyService.Components.Logic.Algoritm;
//using Isaac_AnomalyService.Models;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using Moq;
//using Xunit;

//namespace XUnit_AnomalyService
//{
//    public class CheckBotLeafTest
//    {

//        public readonly List<SensorData> mockData = new List<SensorData>();

//        public List<SensorError> errorData = new List<SensorError>();


//        public OutlierLeaves _outlierLeaves;


//        public Mock<ILogger<OutlierLeaves>> _logger;


//        [Fact]
//        public void SetupTest()
//        {


//            var config = new ConfigurationBuilder()
//                .AddJsonFile("Config.json")
//                .Build();


//            _logger = new Mock<ILogger<OutlierLeaves>>();

//            //logger.Setup((mock) => mock.)


//            _outlierLeaves = new OutlierLeaves(config, _logger.Object);

//            //Bottom anomalys
//            mockData.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 14, X = 1, Y = 1, Type = DataType.Temperature });
//            mockData.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 16, X = 1, Y = 1, Type = DataType.Temperature });
//            mockData.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 9, X = 1, Y = 1, Type = DataType.Humidity });
//            mockData.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 11, X = 1, Y = 1, Type = DataType.Humidity });

//        }



//        [Fact]
//        public void TestBottomAnomaly()
//        {
//            SetupTest();
//            foreach (SensorData sensor in mockData)
//            {
//                _outlierLeaves.FillSensorList(sensor);
//            }
//            errorData = _outlierLeaves.RunAlgo();

//            var result = errorData.FindAll(x => x.Type == SensorError.ErrorType.ExtremeBottom);
//            Assert.Equal(2, result.Count);
//        }
//    }
//}
