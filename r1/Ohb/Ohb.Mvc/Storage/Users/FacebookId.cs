namespace Ohb.Mvc.Storage.Users
{
    public class FacebookId
    {
        public string Id { get; set; }
        public string UserId { get; set; }

        public static string MakeKey(long facebookId)
        {
            return string.Concat("FacebookIds-", facebookId.ToString());
        }
    }
}