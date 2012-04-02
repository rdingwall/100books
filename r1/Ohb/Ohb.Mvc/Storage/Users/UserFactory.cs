using System;
using Facebook;
using Raven.Client;

namespace Ohb.Mvc.Storage.Users
{
    public interface IUserFactory
    {
        User GetOrCreateFacebookUser(IDocumentSession session, FacebookClient facebook);
    }

    public class UserFactory : IUserFactory
    {
        private readonly IUserRepository users;

        public UserFactory(IUserRepository users)
        {
            if (users == null) throw new ArgumentNullException("users");
            this.users = users;
        }

        public User GetOrCreateFacebookUser(IDocumentSession session, FacebookClient facebook)
        {
            if (session == null) throw new ArgumentNullException("session");
            if (facebook == null) throw new ArgumentNullException("facebook");

            dynamic fbUser = facebook.Get("me", new { fields = "name,id" });

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