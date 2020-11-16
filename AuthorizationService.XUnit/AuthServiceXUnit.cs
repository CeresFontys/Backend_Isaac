using Isaac_AuthorizationService.Data;
using Isaac_AuthorizationService.Models;
using Isaac_AuthorizationService.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AuthorizationService.XUnit
{
    public class AuthServiceXUnit
    {
        private readonly User _users;

        public AuthServiceXUnit()
        {
            _users = new User
            {
                Id = 1,
                Email = "Furkan.Demirci@outlook.com",
                Password = "test1234"
            };
        }
        [Fact]
        public async Task Create_User()
        {
            var data = new List<User>
            {
                new User { Id = 1,
                Email = "Demirci.Emirhan@outlook.com",
                Password = "test123" },
                new User { Id = 1,
                Email = "Demirci.Emirhan@outlook.com",
                Password = "test123" },
                new User { Id = 1,
                Email = "Demirci.Emirhan@outlook.com",
                Password = "test123" },
            }.AsQueryable();


            
            var dbSetMock = new Mock<DbSet<User>>();
            //needed to make use of FirstOrDefault
            dbSetMock.Setup(s => s.FindAsync(It.IsAny<User>())).ReturnsAsync(data.First());

            dbSetMock.Setup(x => x.Add(It.IsAny<User>()));

            var context = new Mock<ApplicationDbContext>();
            context.Setup(x => x.Set<User>()).Returns(dbSetMock.Object);

            var authService = new AuthService(context.Object);

            // Act
            authService.Create(_users);

            // Assert
            context.Verify(x => x.SaveChanges(), Times.Once);
        }
        public async Task delete_user()
        {
            var data = new List<User>
            {
                new User { Id = 1,
                Email = "Demirci.Emirhan@outlook.com",
                Password = "test123" },
                new User { Id = 1,
                Email = "Demirci.Emirhan@outlook.com",
                Password = "test123" },
                new User { Id = 1,
                Email = "Demirci.Emirhan@outlook.com",
                Password = "test123" },
            }.AsQueryable();



            var dbSetMock = new Mock<DbSet<User>>();
            dbSetMock.Setup(s => s.FindAsync(It.IsAny<User>())).ReturnsAsync(data.First());
            dbSetMock.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            dbSetMock.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            dbSetMock.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            dbSetMock.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
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
