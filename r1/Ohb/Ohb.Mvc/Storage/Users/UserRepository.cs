using System;
using Raven.Client;

namespace Ohb.Mvc.Storage.Users
{
    public interface IUserRepository
    {
        User GetFacebookUser(long facebookId, IDocumentSession session);
        void AddUser(User user, IDocumentSession session);
        User GetUser(string userId, IDocumentSession session);
    }

    public class UserRepository : IUserRepository
    {
        public User GetFacebookUser(long facebookId, IDocumentSession session)
        {
            if (session == null) throw new ArgumentNullException("session");

            var id = FacebookId.MakeKey(facebookId);

            var facebookUser = session
                    .Include<FacebookId>(f => f.UserId)
                    .Load(id);

            if (facebookUser == null)
                return null;

             return session.Load<User>(facebookUser.UserId);
        }

        public void AddUser(User user, IDocumentSession session)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (session == null) throw new ArgumentNullException("session");

            try
            {
                session.Advanced.UseOptimisticConcurrency = true;

                session.Store(user);

                var facebookId = new FacebookId
                                     {
                                         Id = FacebookId.MakeKey(user.FacebookId),
                                         UserId = user.Id
                                     };

                session.Store(facebookId);
                session.SaveChanges();
            }
            finally
            {
                session.Advanced.UseOptimisticConcurrency = false;
            }
        }

        public User GetUser(string userId, IDocumentSession session)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            if (session == null) throw new ArgumentNullException("session");

            return session.Load<User>(userId);
        }
    }
}