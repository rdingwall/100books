using System;
using System.Text;

namespace Ohb.Mvc.Storage.Books
{
    /// <summary>
    /// Google Volume IDs are case sensitive but Raven DB document IDs are not.
    /// So if we want to use a volume ID as part of an ID we have to make it
    /// case insensitive somehow - simplest way is simply to convert it to
    /// base32. Explained here: 
    /// http://richarddingwall.name/2012/04/02/turning-a-case-sensitive-string-into-a-non-case-sensitive-string/
    /// </summary>
    public static class ConvertGoogleVolumeId
    {
        public static string ToBase32String(string volumeId)
        {
            var bytes = Encoding.ASCII.GetBytes(volumeId);
            return Base32.ToBase32String(bytes);
        }

        public static string FromBase32String(string base32)
        {
            var bytes = Base32.FromBase32String(base32);
            return Encoding.ASCII.GetString(bytes);
        }
    }
}