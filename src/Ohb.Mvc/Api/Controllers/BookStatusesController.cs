using System.Collections.Generic;
using System.Linq;
using Ohb.Mvc.Api.ActionFilters;
using Ohb.Mvc.Api.Models;
using Ohb.Mvc.Storage.PreviousReads;

namespace Ohb.Mvc.Api.Controllers
{
    public class BookStatusesController : OhbApiController
    {
        // GET books/:ids/statuses e.g. books/aaa,bbb,ccc/statuses
        //
        // Should return
        // [{ GoogleVolumeId = "aaa", HasRead = false },
        //  { GoogleVolumeId = "bbb", HasRead = true },
        //  { GoogleVolumeId = "ccc", HasRead = false }] <-- might not exist but return it anyway
        [RequiresAuthCookie]
        public IEnumerable<BookStatus> Get(string volumeIds)
        {
            var requestedVolumeIds = volumeIds
                .Split(new[] {';', ','})
                .Distinct();

            var previouslyReadVolumeIds = GetPreviouslyReadVolumeIds(requestedVolumeIds);

            var results = requestedVolumeIds
                .ToDictionary(k => k, v => new BookStatus { GoogleVolumeId = v });

            // Mark any results which were found in the PreviousReads results
            foreach (var volumeId in previouslyReadVolumeIds)
            {
                if (results.ContainsKey(volumeId))
                    results[volumeId].HasRead = true;
            }

            return results.Values;
        }

        IEnumerable<string> GetPreviouslyReadVolumeIds(IEnumerable<string> requestedGoogleVolumeIds)
        {
            var idsToQuery = requestedGoogleVolumeIds
                .Select(googleVolumeId => PreviousRead.MakeId(User.Id, googleVolumeId))
                .ToList();

            var previousReads = DocumentSession
                .Load<PreviousRead>(idsToQuery)
                .Where(p => p != null);

            return previousReads.Select(p => p.GoogleVolumeId);
        }
    }
}