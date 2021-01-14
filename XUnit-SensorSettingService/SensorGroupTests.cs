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
    public class SensorGroupTests
    {
        private readonly SensorGroupModel sensorGroup;
        private readonly Mock<DataContext> _context;
        private readonly Mock<DbSet<SensorGroupModel>> _dbSetMock;

        public SensorGroupTests()
        {
            _context = new Mock<DataContext>();
            _dbSetMock = new Mock<DbSet<SensorGroupModel>>();
            sensorGroup = new SensorGroupModel()
            {
                Id = 0,
                Name = "Sensor1",
                Floor = "3"
            };
        }
        [Fact]
        public void ShouldAddNewGroup()
        {
            //   context.Setup(x => x.GetSensor(sensor.Floor,sensor.X,sensor.Y)).Returns(sensor);

            _dbSetMock.Setup(x => x.Add(It.IsAny<SensorGroupModel>()));
            _context.Setup(x => x.Set<SensorGroupModel>()).Returns(_dbSetMock.Object);

            var groupService = new GroupService(_context.Object);
            groupService.AddGroup(sensorGroup);
            // Act
            _context.Verify(x => x.SaveChanges(), Times.Once);
          
        }

        [Fact]
        public void ShouldUpdateGroup()
        {
            _context.Setup(x => x.FindAsync<SensorGroupModel>(It.IsAny<int>())).ReturnsAsync(sensorGroup);
            _context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var groupService = new GroupService(_context.Object);

             groupService.UpdateGroup(sensorGroup);
            _context.VerifyAll();
        }

        [Fact]
        public async void ShouldDeleteGroup()
        {
            _context.Setup(x => x.FindAsync<SensorGroupModel>(It.IsAny<int>())).ReturnsAsync(sensorGroup);
            _context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var groupService = new GroupService(_context.Object);

             groupService.DeleteGroup(sensorGroup.Id);
            _context.VerifyAll();
        }
    }
}
