using System;
using System.Linq;
using Ohb.Mvc.Google;
using Raven.Abstractions.Exceptions;
using Raven.Client;

namespace Ohb.Mvc.Storage
{
    public interface IBookImporter
    {
        Book GetBook(IDocumentSession session, string googleVolumeId);
    }

    public class BookImporter : IBookImporter
    {
        readonly IGoogleBooksClient googleBooksClient;

        public BookImporter(IGoogleBooksClient googleBooksClient)
        {
            if (googleBooksClient == null) throw new ArgumentNullException("googleBooksClient");
            this.googleBooksClient = googleBooksClient;
        }

        public Book GetBook(IDocumentSession session, string googleVolumeId)
        {
            if (session == null) throw new ArgumentNullException("session");
            if (String.IsNullOrWhiteSpace(googleVolumeId)) 
                throw new ArgumentException("Missing/empty parameter.", "googleVolumeId");

            try
            {
                return ImportBook(session, googleVolumeId);
            }
            catch (ConcurrencyException)
            {
                // Already exists
                return session.Query<Book>().First(b => b.GoogleVolumeId == googleVolumeId);
            }
        }

        Book ImportBook(IDocumentSession session, string googleVolumeId)
        {
            var staticInfo = googleBooksClient.GetVolume(googleVolumeId);

            if (staticInfo == null)
                return null;

            try
            {
                session.Advanced.UseOptimisticConcurrency = true;

                var book = new Book
                {
                    GoogleVolumeId = googleVolumeId,
                    StaticInfo = staticInfo
                };

                session.Store(new GoogleVolumeId { VolumeId = googleVolumeId },
                    String.Concat("GoogleVolumeIds/", googleVolumeId));
                session.Store(book);
                session.SaveChanges();

                return book;
            }
            finally
            {
                session.Advanced.UseOptimisticConcurrency = false;
            }
        }
    }
}