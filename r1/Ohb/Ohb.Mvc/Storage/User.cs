using System;

namespace Ohb.Mvc.Storage
{
    public interface IUser
    {
        string ProfilePictureUrl { get; }
        string Name { get; }
        long Id { get; }
    }

    public class User : IUser
    {
        public User(long id, string name, string profilePictureUrl)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (profilePictureUrl == null) throw new ArgumentNullException("profilePictureUrl");
            Id = id;
            Name = name;
            ProfilePictureUrl = profilePictureUrl;
        }

        public string ProfilePictureUrl { get; private set; }

        public string Name { get; private set; }

        public long Id { get; private set; }
    }
}