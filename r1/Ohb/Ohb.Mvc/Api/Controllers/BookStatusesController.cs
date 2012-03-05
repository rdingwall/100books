using System.Collections.Generic;
using System.Linq;
using Ohb.Mvc.Api.ActionFilters;
using Ohb.Mvc.Api.Models;
using Ohb.Mvc.Storage.PreviousReads;
using Raven.Client.Linq;

namespace Ohb.Mvc.Api.Controllers
{
    public class BookStatusesController : OhbApiController
    {
        [RequiresAuthCookie]
        public IEnumerable<BookStatus> Get(string volumeIds)
        {
            var volumeIdsList = volumeIds.Split(new [] {';', ','});

            var results = volumeIdsList.Distinct().ToDictionary(k => k, v => false);

            // todo: properly implement this query with an index or something
            var previousReads = DocumentSession.Query<PreviousRead>()
                .Select(p => new { p.Book.GoogleVolumeId })
                .ToList();

            foreach (var previousRead in previousReads)
            {
                if (results.ContainsKey(previousRead.GoogleVolumeId))
                    results[previousRead.GoogleVolumeId] = true;
            }

            return results.Select(p => new BookStatus
                                           {
                                               GoogleVolumeId = p.Key,
                                               HasRead = p.Value
                                           });
        }
    }
}