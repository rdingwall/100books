using System;
using System.Text;

namespace Ohb.Mvc.Storage.Books
{
    /// <summary>
    /// Google Volume IDs are case sensitive but Raven DB document IDs are not.
    /// So if we want to use a volume ID as part of an ID we have to make it
    /// case insensitive somehow - simplest way is simply to convert it to
    /// base64.
    /// </summary>
    public static class ConvertGoogleVolumeId
    {
        public static string ToBase64String(string volumeId)
        {
            var bytes = Encoding.ASCII.GetBytes(volumeId);
            return Convert.ToBase64String(bytes);
        }

        public static string FromBase64String(string base64)
        {
            var bytes = Convert.FromBase64String(base64);
            return Encoding.ASCII.GetString(bytes);
        }
    }
}