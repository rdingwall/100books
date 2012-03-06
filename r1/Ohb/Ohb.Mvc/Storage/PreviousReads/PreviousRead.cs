using System;

namespace Ohb.Mvc.Storage.PreviousReads
{
    public class PreviousRead
    {
        public string GoogleVolumeId { get; set; }
        public string Id { get; set; }
        public string UserId { get; set; }
        public DateTime MarkedByUserAt { get; set; }
        public string BookId { get; set; }

        public static string MakeId(string userId, string googleVolumeId)
        {
            return string.Concat("PreviousReads/", userId, "-", googleVolumeId);
        }
    }
}