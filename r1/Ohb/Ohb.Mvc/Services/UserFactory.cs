using System;
using System.Web;
using Facebook;
using Facebook.Web;
using Ohb.Mvc.Storage;

namespace Ohb.Mvc.Services
{
    public class UserFactory : IUserFactory
    {
        private readonly IUserRepository users;

        public UserFactory(IUserRepository users)
        {
            if (users == null) throw new ArgumentNullException("users");
            this.users = users;
        }

        public User GetOrCreateUser()
        {
            var fbWebContext = new FacebookWebContext(
                FacebookApplication.Current, 
                new HttpContextWrapper(HttpContext.Current)); // or FacebookWebContext.Current;

            if (!fbWebContext.IsAuthenticated())
                return null;

            var user = users.GetUser(fbWebContext.UserId);
            if (user == null)
            {
                user = CreateUser(fbWebContext);
                users.AddUser(user);
            }

            return user;
        }

        private static User CreateUser(FacebookWebContext fbWebContext)
        {
            var fb = new FacebookWebClient();

            dynamic me = fb.Get("me");

            return new User
            {
                FacebookId = fbWebContext.UserId,
                Name = me.name,
                ProfilePictureUrl = String.Format("http://graph.facebok.com/{0}/picture", me.id)
            };
        }
    }

    public interface IUserFactory
    {
        User GetOrCreateUser();
    }
}