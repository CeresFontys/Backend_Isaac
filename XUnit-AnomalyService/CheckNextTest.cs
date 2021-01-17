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
//    public class CheckNextTest
//    {
//        private readonly List<SensorData> mockData = new List<SensorData>();
//        private List<SensorError> errorData = new List<SensorError>();

//        private OutlierLeaves _outlierLeaves;


//        private Mock<ILogger<OutlierLeaves>> logger;



//        [Fact]
//        public void SetupTest()
//        {


//            var config = new ConfigurationBuilder()
//                .AddJsonFile("Config.json")
//                .Build();


//            logger = new Mock<ILogger<OutlierLeaves>>();

//            //logger.Setup((mock) => mock.)


//            _outlierLeaves = new OutlierLeaves(config, logger.Object);

//            //Bottom anomalys
//            mockData.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 20, X = 1, Y = 1, Type = DataType.Temperature });
//            mockData.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 25, X = 1, Y = 1, Type = DataType.Temperature });
//            mockData.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 20, X = 1, Y = 1, Type = DataType.Temperature });
//            mockData.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 40, X = 1, Y = 1, Type = DataType.Humidity });
//            mockData.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 45, X = 1, Y = 1, Type = DataType.Humidity });
//            mockData.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 40, X = 1, Y = 1, Type = DataType.Humidity });


//        }


//        [Fact]
//        public void NextFlagTest()
//        {
//            SetupTest();
//            foreach (SensorData sensor in mockData)
//            {
//                _outlierLeaves.FillSensorList(sensor);
//            }
//            errorData = _outlierLeaves.RunAlgo();

//            var result = errorData.FindAll(x => x.Type == SensorError.ErrorType.NextDif);
//            Assert.Equal(4, result.Count);
//        }

//    }
//}
