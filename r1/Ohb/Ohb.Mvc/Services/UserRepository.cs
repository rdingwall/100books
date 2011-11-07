using System;
using System.Collections.Generic;
using Facebook.Web;

namespace Ohb.Mvc.Services
{
    public interface IUserRepository
    {
        IUser GetUser(long id);
        void AddUser(IUser user);
    }

    public class UserRepository : IUserRepository
    {
        private readonly IDictionary<long, IUser> users = new Dictionary<long, IUser>();

        public IUser GetUser(long id)
        {
            if (!users.ContainsKey(id))
                return null;

            return users[id];
        }

        public void AddUser(IUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            users.Add(user.Id, user);
        }
    }
}