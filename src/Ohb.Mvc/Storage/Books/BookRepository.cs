using System;
using System.Linq;
using Raven.Abstractions.Exceptions;
using Raven.Client;

namespace Ohb.Mvc.Storage.Books
{
    public interface IBookRepository
    {
        Book Get(string googleVolumeId, IDocumentSession session);
        void Add(Book book, IDocumentSession session);
    }

    public class BookRepository : IBookRepository
    {
        readonly IRavenUniqueInserter inserter;

        public BookRepository(IRavenUniqueInserter inserter)
        {
            if (inserter == null) throw new ArgumentNullException("inserter");
            this.inserter = inserter;
        }

        public Book Get(string googleVolumeId, IDocumentSession session)
        {
            if (session == null) throw new ArgumentNullException("session");
            if (String.IsNullOrWhiteSpace(googleVolumeId))
                throw new ArgumentException("Missing/empty parameter.", "googleVolumeId");

            var base32 = ConvertGoogleVolumeId.ToBase32String(googleVolumeId);

            return session.Query<Book>()
                .FirstOrDefault(b => b.GoogleVolumeIdBase32 == base32);
        }

        public void Add(Book book, IDocumentSession session)
        {
            if (book == null) throw new ArgumentNullException("book");
            if (session == null) throw new ArgumentNullException("session");

            try
            {
                inserter.StoreUnique(session, book, b => b.GoogleVolumeIdBase32);
            }
            catch (ConcurrencyException)
            {
                // Already added, do nothing
            }
        }
    }
}