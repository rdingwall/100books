using System;
using Ohb.Mvc.Api.Models;
using Ohb.Mvc.Google;
using Raven.Client;

namespace Ohb.Mvc.Storage
{
    public interface IBookImporter
    {
        BookStaticInfo GetBook(IDocumentSession session, string googleVolumeId);
    }

    public class BookImporter : IBookImporter
    {
        readonly IGoogleBooksClient googleBooksClient;

        public BookImporter(IGoogleBooksClient googleBooksClient)
        {
            if (googleBooksClient == null) throw new ArgumentNullException("googleBooksClient");
            this.googleBooksClient = googleBooksClient;
        }

        public BookStaticInfo GetBook(IDocumentSession session, string googleVolumeId)
        {
            if (session == null) throw new ArgumentNullException("session");
            if (googleVolumeId == null) throw new ArgumentNullException("googleVolumeId");

            var book = session.Load<Book>(googleVolumeId);
            return book == null ? ImportBook(session, googleVolumeId) : book.StaticInfo;
        }

        BookStaticInfo ImportBook(IDocumentSession session, string googleVolumeId)
        {
            var staticInfo = googleBooksClient.GetVolume(googleVolumeId);

            var book = new Book
                           {
                               StaticInfo = staticInfo,
                               Id = googleVolumeId
                           };

            session.Store(book);
            session.SaveChanges();

            return staticInfo;
        }
    }
}