using Isaac_AuthorizationService.Models;
using Isaac_AuthorizationService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Isaac_AuthorizationService.Helpers
{
    public class StaticWrapper : IStaticWrapper
    {
        AuthService auth = new AuthService();
        public void StaticUserAdd(User user)
        {
            auth.Create(user);
        }
    }
}
