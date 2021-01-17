using Isaac_AuthorizationService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Isaac_AuthorizationService.Interfaces
{
    public interface IUserService
    {
        void Create(User user);
        JwtUser Authenticate(string username, string password);
        void Delete(int id);
        IEnumerable<User> GetUsers();
        void Update(User user);
    }
    
    public interface IWhitelistService
    {
        void Create(Whitelist whitelist);
        void Delete(int id);
        IEnumerable<Whitelist> GetWhitelists();
        void Update(Whitelist whitelist);
    }

    public class Whitelist
    {
        [Key]
        public int Id { get; set; }
        public string Ip { get; set; }
        public string Name { get; set; }
    }
}
