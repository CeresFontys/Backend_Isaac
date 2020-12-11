using Isaac_AuthorizationService.Data;
using Isaac_AuthorizationService.Helpers;
using Isaac_AuthorizationService.Models;
using Isaac_AuthorizationService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace XUnit_AuthorizationService
{
    [TestClass]
    public class AuthServiceTest
    {
        private readonly User _users;

        public AuthServiceTest()
        {

            _users = new User
            {
                Email = "Demirci.Emirhan@outlook.com",
                Id = 1,
                Password = "test1234"
            };
        }

        [Fact] 
        public void Create_User()
        {
            var context = new Mock<ApplicationDbContext>();
            var dbSetMock = new Mock<DbSet<User>>();
            dbSetMock.Setup(x => x.Add(It.IsAny<User>()));
            context.Setup(x => x.Set<User>()).Returns(dbSetMock.Object);

            var authService = new AuthService(context.Object);

            // Act
            authService.Create(_users);
            context.Verify(x => x.SaveChanges(), Times.Once);
        }
        [Fact]
        public void Login_User()
        {
            var wrapper = new StaticWrapper();

            var wm = new WrapperMethod(wrapper);

            var output = wm.AuthenticateUser(_users);

            //Assert
            Xunit.Assert.Equal(1, output.Id);
        }
    }
}
