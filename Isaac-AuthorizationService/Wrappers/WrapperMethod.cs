using Isaac_AuthorizationService.Data;
using Isaac_AuthorizationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Isaac_AuthorizationService.Helpers
{
    public class WrapperMethod
    {
        private readonly ApplicationDbContext _dbContext;
        IStaticWrapper _wrapper;

        public WrapperMethod(IStaticWrapper wrapper)
        {
            _wrapper = wrapper;
        }
        public WrapperMethod(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User AddStaticUser(User user)
        {
            _wrapper.StaticUserAdd(user);
            return user;
        }
    }
}
