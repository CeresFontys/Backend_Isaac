using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Isaac_FloorService;
using Isaac_FloorService.Controllers;
using Isaac_FloorService.Data;
using Isaac_FloorService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace XUnit_FloorService
{
    public class FloorServiceTests
    {
        private readonly Floor floor;

        public FloorServiceTests()
        {
            floor = new Floor
            {
               Id = 0,
               Image = null,
               Length = 20,
               Width = 25,
               Name = "TestFloor"
            };
        }
        

        [Fact]
        public async void DeleteFloorTest()
        {
            var context = new Mock<FloorServiceContext>();
            context.Setup(x => x.Remove(It.IsAny<Floor>()));
            context.Setup(x => x.FindAsync<Floor>(It.IsAny<int>())).ReturnsAsync(floor);
            context.Setup(context => context.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            var floorController = new FloorController(context.Object);


            await floorController.DeleteFloor(0);
            

            context.VerifyAll();
        }

        [Fact]
        public async void GetFloorByIdTest()
        {
            //arrange
            var context = new Mock<FloorServiceContext>();
            context.Setup(x => x.FindAsync<Floor>(It.IsAny<int>())).ReturnsAsync(floor);
            var floorController = new FloorController(context.Object);



            //act
            await floorController.GetFloor(0);

            //assert
            context.VerifyAll();
        }
    }
}
