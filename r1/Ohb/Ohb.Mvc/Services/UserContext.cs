using Ohb.Mvc.Storage;

namespace Ohb.Mvc.Services
{
    public interface IUserContext
    {
        IUser User { get; }
        bool IsLoggedIn { get; }
    }

    public class UserContext : IUserContext
    {
        public UserContext(IUser user)
        {
            User = user;
        }

        public IUser User { get; private set; }
        public bool IsLoggedIn { get { return User != null;  } }
    }
}