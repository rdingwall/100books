using System;
using System.Linq;
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
            if (String.IsNullOrWhiteSpace(googleVolumeId)) 
                throw new ArgumentException("Missing/empty parameter.", "googleVolumeId");

            var book = session.Query<Book>().FirstOrDefault(b => b.GoogleVolumeId == googleVolumeId);
            return book ?? ImportBook(session, googleVolumeId);
        }

        Book ImportBook(IDocumentSession session, string googleVolumeId)
        {
            var staticInfo = googleBooksClient.GetVolume(googleVolumeId);

            if (staticInfo == null)
                return null;

            var book = new Book
                           {
                               GoogleVolumeId = googleVolumeId,
                               StaticInfo = staticInfo
                           };

            session.Store(book);
            session.SaveChanges();

            return book;
        }
    }
}