using Isaac_AuthorizationService.Data;
using Isaac_AuthorizationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Isaac_AuthorizationService.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _dbContext;
        public AuthService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public JwtUser Authenticate(string username, string password)
        {
            var jwtKey = "TryToGuessThisPassword";
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);

            if (user == null || password != user.Password)
            {
                return null;
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {

            }
        }
    }
}
