using System;
using System.Collections.Generic;

namespace Ohb.Mvc.Storage
{
    public interface IUserRepository
    {
        User GetUser(long id);
        void AddUser(User user);
    }

    public class UserRepository : IUserRepository
    {
        static readonly object syncRoot = new object();
        private readonly IDictionary<long, User> users = new Dictionary<long, User>();

        public User GetUser(long id)
        {
            if (!users.ContainsKey(id))
                return null;

            return users[id];
        }

        public void AddUser(User user)
        {
            if (user == null) throw new ArgumentNullException("user");

            lock (syncRoot)
            {
                if (users.ContainsKey(user.FacebookId))
                    return;

                users.Add(user.FacebookId, user);
            }
            
        }
    }
}