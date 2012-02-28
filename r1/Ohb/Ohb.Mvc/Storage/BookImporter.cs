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
        readonly IBookRepository books;

        public BookImporter(IGoogleBooksClient googleBooksClient, IBookRepository books)
        {
            if (googleBooksClient == null) throw new ArgumentNullException("googleBooksClient");
            if (books == null) throw new ArgumentNullException("books");
            this.googleBooksClient = googleBooksClient;
            this.books = books;
        }

        public Book GetBook(IDocumentSession session, string googleVolumeId)
        {
            if (session == null) throw new ArgumentNullException("session");
            if (String.IsNullOrWhiteSpace(googleVolumeId)) 
                throw new ArgumentException("Missing/empty parameter.", "googleVolumeId");

            
            return books.Get(googleVolumeId, session) ?? ImportBook(session, googleVolumeId);
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

            books.Add(book, session);

            return book;
        }
    }
}