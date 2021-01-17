//using System;
//using System.Collections.Generic;
//using System.Text;
//using Isaac_AnomalyService.Components.Logic;
//using Isaac_AnomalyService.Components.Logic.Algoritm;
//using Isaac_AnomalyService.Models;
//using Microsoft.EntityFrameworkCore.Infrastructure;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using Moq;
//using Xunit;

//namespace XUnit_AnomalyService
//{
//     public class CheckETopLeafTest
//    {


//        private readonly List<SensorData> mockData = new List<SensorData>();
//        private List<SensorError> errorData = new List<SensorError>();

//       private OutlierLeaves _outlierLeaves;


//       private Mock<ILogger<OutlierLeaves>> logger;



//       [Fact]
//        public void SetupTest() 
//            {


//                var config = new ConfigurationBuilder()
//                    .AddJsonFile("Config.json")
//                    .Build();


//                logger = new Mock<ILogger<OutlierLeaves>>();

//                //logger.Setup((mock) => mock.)


//                _outlierLeaves = new OutlierLeaves(config, logger.Object);

//                //Bottom anomalys
//                mockData.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 36, X = 1, Y = 1, Type = DataType.Temperature });
//                mockData.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 34, X = 1, Y = 1, Type = DataType.Temperature });
//                mockData.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 91, X = 1, Y = 1, Type = DataType.Humidity });
//                mockData.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value =  89, X = 1, Y = 1, Type = DataType.Humidity });

//        }

        
//        [Fact]
//        public void TestExtremeTopAnomaly()
//        {
//            SetupTest();
//            foreach (SensorData sensor in mockData)
//            {
//                _outlierLeaves.FillSensorList(sensor);
//            }
//            errorData = _outlierLeaves.RunAlgo();

//            var result = errorData.FindAll(x => x.Type == SensorError.ErrorType.ExtremeTop);
//            Assert.Equal(2, result.Count);
//        }
//    }
//}
