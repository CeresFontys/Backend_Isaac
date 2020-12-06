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
                Id = 1,
                Email = "Demirci.Emirhan@outlook.com",
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
            //var wrapper = new StaticWrapper();
            //var wm = new WrapperMethod(wrapper);
            //var output = wm.AddStaticUser(_users);
            var authService = new AuthService(context.Object);

            //var userTestList = new List<User>() { _users };

            //dbSetMock.As<IQueryable<User>>().Setup(x => x.Provider).Returns(userTestList.AsQueryable().Provider);
            //dbSetMock.As<IQueryable<User>>().Setup(x => x.Expression).Returns(userTestList.AsQueryable().Expression);
            //dbSetMock.As<IQueryable<User>>().Setup(x => x.ElementType).Returns(userTestList.AsQueryable().ElementType);
            //dbSetMock.As<IQueryable<User>>().Setup(x => x.GetEnumerator()).Returns(userTestList.AsQueryable().GetEnumerator());

            // Act
            authService.Create(_users);
            //wrapper.StaticUserAdd(output);

            // Assert
            context.Verify(x => x.SaveChanges(), Times.Once);
        }
        [Fact]
        public void Login_User()
        {
            var context = new Mock<ApplicationDbContext>();
            var dbSetMock = new Mock<DbSet<User>>();
            dbSetMock.Setup(x => x.Find(It.IsAny<User>()));
            context.Setup(x => x.Set<User>()).Returns(dbSetMock.Object);
            var authService = new AuthService(context.Object);

            //Act
            var ewa = authService.Authenticate(_users.Email, _users.Password);

            //Assert
            context.Verify(x => x.SaveChanges());
        }
    }
}
