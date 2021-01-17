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
//    public class ChecktopLeafTest
//    {


//        private readonly List<SensorData> mockTopData = new List<SensorData>();

//        private List<SensorError> errorData = new List<SensorError>();


//        private OutlierLeaves _outlierLeaveTop;


//        private Mock<ILogger<OutlierLeaves>> _logger;


//        [Fact]
//        public void testTopSetup()
//        {


//            var config = new ConfigurationBuilder()
//                .AddJsonFile("Config.json")
//                .Build();


//            _logger = new Mock<ILogger<OutlierLeaves>>();

//            //logger.Setup((mock) => mock.)


//            _outlierLeaveTop = new OutlierLeaves(config, _logger.Object);

//            //Bottom anomalys
//            mockTopData.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 29, X = 1, Y = 1, Type = DataType.Temperature });
//            mockTopData.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 27, X = 1, Y = 1, Type = DataType.Temperature });
//            mockTopData.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 59, X = 1, Y = 1, Type = DataType.Humidity });
//            mockTopData.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 61, X = 1, Y = 1, Type = DataType.Humidity });

//        }




//        [Fact]
//        public void TestTopAnomaly()
//        {
//            testTopSetup();
//            foreach (SensorData sensor in mockTopData)
//            {
//                _outlierLeaveTop.FillSensorList(sensor);
//            }
//            errorData = _outlierLeaveTop.RunAlgo();

//            var result = errorData.FindAll(x => x.Type == SensorError.ErrorType.NormalTop);
//            Assert.Equal(2, result.Count);
//        }
//    }
//}
