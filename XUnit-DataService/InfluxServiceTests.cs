using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Reactive;
using System.Threading.Tasks;
using Castle.Core.Internal;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Isaac_DataService.Components.Connections;
using Isaac_DataService.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace XUnit_DataService
{
    public class InfluxServiceTests
    {
        private Mock<IFluxConnection> _influxConnection;
        private Mock<ILogger<InfluxService>> _loggerMock;
        private PointData _point;

        public InfluxServiceTests()
        {
            _influxConnection = new Mock<IFluxConnection>();
            _loggerMock = new Mock<ILogger<InfluxService>>();
            _point = PointData.Measurement("measurement")
                .Tag("x", It.IsAny<long>().ToString())
                .Tag("y", It.IsAny<long>().ToString())
                .Tag("floor", It.IsAny<string>())
                .Field("value", It.IsAny<long>());
        }

        [Fact]
        public async Task UploadPointSuccessful()
        {
            //Setup valid connection
            Setup(true);
            var service = new InfluxService(_influxConnection.Object, _loggerMock.Object);

            //Upload valid point
            var result = await service.UploadPoint(_point);
            
            //Verify checked connection
            _influxConnection.Verify(connection => connection.ReadyAsync(), Times.Once);
            //Verify write requested point
            _influxConnection.Verify(connection => connection.WritePointAsync(_point), Times.Once);
            //Verify success
            Assert.True(result); 
        }
        
        [Fact]
        public async Task UploadPointConnectionFailed()
        {
            //Setup invalid connection
            Setup(false);
            var service = new InfluxService(_influxConnection.Object, _loggerMock.Object);

            //Upload valid point
            var result = await service.UploadPoint(_point);
            
            //Verify checked connection
            _influxConnection.Verify(connection => connection.ReadyAsync(), Times.Once);
            //Verify logged failure
            _loggerMock.Verify(logger => logger.Log(LogLevel.Warning, It.IsAny<EventId>(), It.IsAny<PointData>(),
                It.IsAny<Exception>(), It.IsAny<Func<PointData, Exception, string>>()), Times.Once);
            //Verify failure
            Assert.False(result); 
        }
        
        [Fact]
        public async Task UploadPointVerificationFailed()
        {
            //Setup valid connection
            Setup(true);
            var service = new InfluxService(_influxConnection.Object, _loggerMock.Object);
            
            //Upload invalid point
            var result = await service.UploadPoint(null);
            
            //Verify checked connection
            _influxConnection.Verify(connection => connection.ReadyAsync(), Times.Once);
            //Verify logged failure
            _loggerMock.Verify(logger => logger.Log(LogLevel.Warning, It.IsAny<EventId>(), It.IsAny<PointData>(),
                It.IsAny<Exception>(), It.IsAny<Func<PointData, Exception, string>>()), Times.Once);
            //Verify failure
            Assert.False(result); 
        }

        [Fact]
        public async Task BackupQueueFill()
        {
            //Setup invalid connection
            Setup(false);
            var service = new InfluxService(_influxConnection.Object, _loggerMock.Object);

            var pointData = PointData.Measurement("TEST");
            
            //Upload valid point
            var result = await service.UploadPoint(pointData);
            
            //Verify checked connection
            _influxConnection.Verify(connection => connection.ReadyAsync(), Times.Once);
            //Verify logged failure
            _loggerMock.Verify(logger => logger.Log(LogLevel.Warning, It.IsAny<EventId>(), It.IsAny<PointData>(),
                It.IsAny<Exception>(), It.IsAny<Func<PointData, Exception, string>>()), Times.Once);
            
            //Verify failure
            Assert.False(result);
            //Verify queue has been filled
            Assert.Contains(pointData, service.QueueData);
        }
        
        [Fact]
        public async Task BackupQueueEmpty()
        {
            //Setup valid connection
            Setup(true);
            
            var pointData = PointData.Measurement("TEST");
            
            var service = new InfluxService(_influxConnection.Object, _loggerMock.Object, new []{pointData});
            
            //Upload valid point
            var result = await service.UploadPoint(_point);
            
            //Verify checked connection twice
            _influxConnection.Verify(connection => connection.ReadyAsync(), Times.Exactly(2));
            //Verify write requested point
            _influxConnection.Verify(connection => connection.WritePointAsync(pointData), Times.Once);
            //Verify write backlog point
            _influxConnection.Verify(connection => connection.WritePointAsync(_point), Times.Once);
            
            //Verify success
            Assert.True(result);
            //Verify queue is empty
            Assert.Empty(service.QueueData);
        }

        private void Setup(bool ready)
        {
            _influxConnection.Setup(connection => connection.ReadyAsync())
                .ReturnsAsync(new Ready(ready ? Ready.StatusEnum.Ready : 0));
            _loggerMock.Setup(logger => logger.Log(LogLevel.Warning, It.IsAny<EventId>(), It.IsAny<PointData>(),
                It.IsAny<Exception>(), It.IsAny<Func<PointData, Exception, string>>()));
            _influxConnection.Setup(connection => connection.WritePointAsync(It.IsAny<PointData>()))
                .Returns(Task.CompletedTask);
        }
    }
}
