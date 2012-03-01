using System;
using System.Linq;
using Raven.Client;

namespace Ohb.Mvc.Storage.Users
{
    public interface IUserRepository
    {
        User GetUser(long facebookId, IDocumentSession session);
        void AddUser(User user, IDocumentSession session);
    }

    public class UserRepository : IUserRepository
    {
        static readonly object syncRoot = new object();

        public User GetUser(long facebookId, IDocumentSession session)
        {
            if (session == null) throw new ArgumentNullException("session");

            return session.Query<User>().FirstOrDefault(u => u.FacebookId == facebookId);
        }

        public void AddUser(User user, IDocumentSession session)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (session == null) throw new ArgumentNullException("session");

            lock (syncRoot)
            {
                try
                {
                    // Unique constraint ala http://old.ravendb.net/faq/unique-constraints
                    session.Advanced.UseOptimisticConcurrency = true;

                    session.Store(new FacebookUserId { UserId = user.FacebookId },
                        String.Format("FacebookUserIds/{0}", user.FacebookId));

                    session.Store(user);

                    session.SaveChanges();
                }
                finally
                {
                    session.Advanced.UseOptimisticConcurrency = false;
                }
            }
        }
    }
}