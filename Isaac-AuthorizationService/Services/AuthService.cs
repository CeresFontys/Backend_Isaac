using Isaac_AuthorizationService.Data;
using Isaac_AuthorizationService.Interfaces;
using Isaac_AuthorizationService.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace Isaac_AuthorizationService.Services
{
    public class AuthService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        public AuthService()
        {
        }

        public AuthService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(User user)
        {
            var dbUser = _dbContext.Users.FirstOrDefault(x => x.Username == user.Username);
            if (dbUser == null)
            {
                //Hashing the password
                user.Password = BC.HashPassword(user.Password);
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new AlreadyExistsException("User already exists");
            }
        }

        public JwtUser Authenticate(string username, string password)
        {
            var jwtKey = "TryToGuessThisPassword";
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
            //bool verified = BC.Verify(password, user.Password);
            //Check if the user exists and verifies the hashed password
            if (user == null || !BC.Verify(password, user.Password))
            {
                return null;
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //Making claims for the Jwt token. So if I decode the token I could find these data
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Username", user.Username)
                }),
                //How long the token is valid
                Expires = DateTime.UtcNow.AddDays(1),
                //Creating a securityToken with the "HmacSha256" algorithm
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    SecurityAlgorithms.HmacSha256)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);

            var jwtUser = new JwtUser();
            jwtUser.Token = token;
            jwtUser.User = user;
            return jwtUser;
        }
    }
}
