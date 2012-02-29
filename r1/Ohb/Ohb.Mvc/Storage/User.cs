using System;

namespace Ohb.Mvc.Storage
{
    public class User
    {
        public string Id { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Name { get; set; }
        public long FacebookId { get; set; }
        public string SecretKey { get; set; }
    }
}