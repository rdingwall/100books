using System;
using Ohb.Mvc.Google;
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
            if (googleVolumeId == null) throw new ArgumentNullException("googleVolumeId");

            var book = session.Load<Book>(googleVolumeId);
            return book ?? ImportBook(session, googleVolumeId);
        }

        Book ImportBook(IDocumentSession session, string googleVolumeId)
        {
            var staticInfo = googleBooksClient.GetVolume(googleVolumeId);

            if (staticInfo == null)
                return null;

            var book = new Book
                           {
                               StaticInfo = staticInfo,
                               Id = googleVolumeId
                           };

            session.Store(book, googleVolumeId);
            session.SaveChanges();

            return book;
        }
    }
}