using System;
using System.Linq;
using Ohb.Mvc.Storage.ApiTokens;
using Raven.Client;

namespace Ohb.Mvc.Storage.Users
{
    public interface IUserRepository
    {
        User GetUser(long facebookId, IDocumentSession session);
        void AddUser(User user, IDocumentSession session);
        User GetUserByApiToken(string apiToken, IDocumentSession session);
    }

    public class UserRepository : IUserRepository
    {
        static readonly object syncRoot = new object();
        readonly IRavenUniqueInserter inserter;

        public UserRepository(IRavenUniqueInserter inserter)
        {
            if (inserter == null) throw new ArgumentNullException("inserter");
            this.inserter = inserter;
        }

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
                inserter.StoreUnique(session, user, u => u.FacebookId);
        }

        public User GetUserByApiToken(string apiToken, IDocumentSession session)
        {
            if (session == null) throw new ArgumentNullException("session");
            if (String.IsNullOrWhiteSpace(apiToken))
                throw new ArgumentException("Missing/empty parameter.", "apiToken");

            var token = session.Query<ApiToken>()
                .Customize(x => x.Include<ApiToken>(t => t.UserId))
                .FirstOrDefault(t => t.Token == apiToken);

            return session.Load<User>(token.UserId);
        }
    }
}