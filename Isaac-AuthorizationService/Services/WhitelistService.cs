using System.Collections.Generic;
using System.Linq;
using Isaac_AuthorizationService.Data;
using Isaac_AuthorizationService.Interfaces;

namespace Isaac_AuthorizationService.Services
{
    public class WhitelistService : IWhitelistService
    {
        private readonly ApplicationDbContext _dbContext;

        public WhitelistService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(Whitelist whitelist)
        {
            _dbContext.Add(whitelist);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var userToBeDeleted = _dbContext.Whitelists.Find(id);
            if (userToBeDeleted == null) return;
            
            _dbContext.Whitelists.Remove(userToBeDeleted);
            _dbContext.SaveChanges();
        }

        public IEnumerable<Whitelist> GetWhitelists()
        {
            return _dbContext.Whitelists.AsEnumerable();
        }

        public void Update(Whitelist whitelist)
        {
            _dbContext.Update(whitelist);
            _dbContext.SaveChanges();
        }
    }
}