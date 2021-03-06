using System;
using Ohb.Mvc.Storage.Books;

namespace Ohb.Mvc.Storage.PreviousReads
{
    public class PreviousRead
    {
        public string GoogleVolumeIdBase32 { get; set; }
        public string GoogleVolumeId { get; set; }
        public string Id { get; set; }
        public string UserId { get; set; }
        public DateTime MarkedByUserAt { get; set; }
        public string BookId { get; set; }

        public static string MakeId(string userId, string googleVolumeId)
        {
            var base32 = ConvertGoogleVolumeId.ToBase32String(googleVolumeId);
            return string.Concat("PreviousReads-", userId, "-", base32);
        }
    }
}