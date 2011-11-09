using System;

namespace Ohb.Mvc.Services
{
    public interface IUserContextFactory
    {
        IUserContext GetCurrentContext();
    }

    public class UserContextFactory : IUserContextFactory
    {
        private readonly IUserFactory userFactory;

        public UserContextFactory(IUserFactory userFactory)
        {
            if (userFactory == null) throw new ArgumentNullException("userFactory");
            this.userFactory = userFactory;
        }

        public IUserContext GetCurrentContext()
        {
            return new UserContext(userFactory.GetOrCreateUser());
        }
    }
}