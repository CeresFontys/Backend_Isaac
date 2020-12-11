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
        public bool StaticUserAuthenticate(User user)
        {
            var ewa = UserTestData(user.Email, user.Password);
            if (ewa == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UserTestData(string email, string password)
        {
            var _email = "Demirci.Emirhan@outlook.com";
            var _password = "test1234";

            if (_email == email && _password == password)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
