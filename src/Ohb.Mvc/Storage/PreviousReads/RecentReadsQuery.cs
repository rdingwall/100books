using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client;
using Raven.Client.Linq;

namespace Ohb.Mvc.Storage.PreviousReads
{
    public interface IRecentReadsQuery
    {
        List<PreviousReadWithBook> Get(IDocumentSession session, string userId);
    }

    public class RecentReadsQuery : IRecentReadsQuery
    {
        public List<PreviousReadWithBook> Get(IDocumentSession session, string userId)
        {
            if (session == null) throw new ArgumentNullException("session");
            if (userId == null) throw new ArgumentNullException("userId");

            return session
                .Query<PreviousRead, PreviousReadsWithBook>()
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.MarkedByUserAt)
                .Take(100)
                .As<PreviousReadWithBook>()
                .ToList();
        }
    }
}