using Isaac_AuthorizationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Isaac_AuthorizationService.Interfaces
{
    public interface IUserService
    {
        void Create(User user);
        JwtUser Authenticate(string username, string password);
        string Test();
    }
}
