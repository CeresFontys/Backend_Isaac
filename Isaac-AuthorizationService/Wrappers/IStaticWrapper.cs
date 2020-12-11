using Isaac_AuthorizationService.Models;

namespace Isaac_AuthorizationService.Helpers
{
    public interface IStaticWrapper
    {
        bool StaticUserAuthenticate(User user);
    }
}