using System;
using Facebook;
using Raven.Client;

namespace Ohb.Mvc.Storage.Users
{
    public interface IUserFactory
    {
        User GetOrCreateFacebookUser(IDocumentSession session, string accessToken);
    }

    public class UserFactory : IUserFactory
    {
        private readonly IUserRepository users;

        public UserFactory(IUserRepository users)
        {
            if (users == null) throw new ArgumentNullException("users");
            this.users = users;
        }

        public User GetOrCreateFacebookUser(IDocumentSession session, string accessToken)
        {
            if (session == null) throw new ArgumentNullException("session");
            if (String.IsNullOrWhiteSpace(accessToken))
                throw new ArgumentException("Null or empty parameter.", "accessToken");

            var fbClient = new FacebookClient(accessToken);
            dynamic fbUser = fbClient.Get("me", new { fields = "name,id" });

            var user = users.GetFacebookUser(fbUser.id, session);
            if (user == null)
            {
                user = CreateUser(fbUser);
                users.AddUser(user, session);
            }

            return user;
        }

        private static User CreateUser(dynamic fbUser)
        {
            return new User
            {
                FacebookId = fbUser.id,
                DisplayName = fbUser.name,
                ProfileImageUrl = String.Format("http://graph.facebok.com/{0}/picture", fbUser.id)
            };
        }
    }
}