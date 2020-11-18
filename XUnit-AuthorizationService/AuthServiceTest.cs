using Isaac_AuthorizationService.Data;
using Isaac_AuthorizationService.Models;
using Isaac_AuthorizationService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
        public async Task Create_User()
        {
            var dbSetMock = new Mock<DbSet<User>>();
            dbSetMock.Setup(s => s.FindAsync(It.IsAny<User>())).ReturnsAsync(_users);
            dbSetMock.Setup(x => x.Add(It.IsAny<User>()));
            var context = new Mock<ApplicationDbContext>();
            context.Setup(x => x.Set<User>()).Returns(dbSetMock.Object);

            var authService = new AuthService(context.Object);

            // Act
            authService.Create(_users);

            // Assert
            context.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
