using System;
using System.Threading;
using InfluxDB.Client.Api.Domain;
using Isaac_SensorSettingService.Compontents;
using Isaac_SensorSettingService.Data;
using Isaac_SensorSettingService.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace XUnit_SensorSettingService
{
    public class SensorTests
    {
        private readonly SensorModel sensor;
        private readonly Mock<DataContext> _context;
        private readonly Mock<DbSet<SensorModel>> _dbSetMock;

        public SensorTests()
        {
            _context = new Mock<DataContext>();
            _dbSetMock = new Mock<DbSet<SensorModel>>();
            sensor = new SensorModel
            {
                Id = 0,
                Name = "Sensor1",
                Floor = "3",
                GroupId = 1,
                X = 1,
                Y =2
            };
        }
        [Fact]
        public void ShouldAddNewSensor()
        {
            //   context.Setup(x => x.GetSensor(sensor.Floor,sensor.X,sensor.Y)).Returns(sensor);

            _dbSetMock.Setup(x => x.Add(It.IsAny<SensorModel>()));
            _context.Setup(x => x.Set<SensorModel>()).Returns(_dbSetMock.Object);

            var sensorService = new SensorService(_context.Object);

            // Act
            string response = sensorService.AddSensor(sensor);
            _context.Verify(x => x.SaveChanges(), Times.Once);
            Assert.True(response == "sensor added");
        }

        [Fact]
        public async void ShouldUpdateSensor()
        {
            _context.Setup(x => x.FindAsync<SensorModel>(It.IsAny<int>())).ReturnsAsync(sensor);
            _context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
           
            var sensorService = new SensorService(_context.Object);

            await sensorService.UpdateSensor(sensor);
            _context.VerifyAll();
        }

        [Fact]
        public async void ShouldDeleteSensor()
        {
            _context.Setup(x => x.FindAsync<SensorModel>(It.IsAny<int>())).ReturnsAsync(sensor);
            _context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var sensorService = new SensorService(_context.Object);

            await sensorService.DeleteSensor(sensor.Id);
            _context.VerifyAll();
        }

    }
}
