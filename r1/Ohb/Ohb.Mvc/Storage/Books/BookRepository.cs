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
        public Book Get(string googleVolumeId, IDocumentSession session)
        {
            if (session == null) throw new ArgumentNullException("session");
            if (String.IsNullOrWhiteSpace(googleVolumeId))
                throw new ArgumentException("Missing/empty parameter.", "googleVolumeId");
            
            return session.Query<Book>().FirstOrDefault(b => b.GoogleVolumeId == googleVolumeId);
        }

        public void Add(Book book, IDocumentSession session)
        {
            if (book == null) throw new ArgumentNullException("book");
            if (session == null) throw new ArgumentNullException("session");

            try
            {
                session.Advanced.UseOptimisticConcurrency = true;

                session.Store(new UniqueGoogleVolumeId {VolumeId = book.GoogleVolumeId},
                              String.Concat("GoogleVolumeIds/", book.GoogleVolumeId));

                session.Store(book);
                session.SaveChanges();
            }
            catch (ConcurrencyException)
            {
                // Already added, do nothing
            }
            finally
            {
                session.Advanced.UseOptimisticConcurrency = false;
            }
        }
    }
}