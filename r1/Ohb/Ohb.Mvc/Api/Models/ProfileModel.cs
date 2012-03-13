using System.Collections.Generic;

namespace Ohb.Mvc.Api.Models
{
    public class ProfileModel
    {
        public ProfileModel()
        {
            RecentReads = new List<PreviousReadModel>();
        }

        public string Id { get; set; }
        public string ProfileImageUrl { get; set; }
        public string DisplayName { get; set; }

        public IList<PreviousReadModel> RecentReads { get; set; }
    }
}