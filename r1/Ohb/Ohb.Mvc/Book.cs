using System;

namespace Ohb.Mvc
{
    public interface IBook
    {
        string Asin { get; }
        string Title { get; }
        string Author { get; }
        string CoverImageUrl { get; }
    }

    class Book : IBook
    {
        public Book(string asin, string title, string author, string coverImageUrl)
        {
            if (asin == null) throw new ArgumentNullException("asin");
            if (title == null) throw new ArgumentNullException("title");
            if (author == null) throw new ArgumentNullException("author");
            if (coverImageUrl == null) throw new ArgumentNullException("coverImageUrl");
            Asin = asin;
            Title = title;
            Author = author;
            CoverImageUrl = coverImageUrl;
        }

        public string Asin { get; private set; }
        public string Title { get; private set; }
        public string Author { get; private set; }
        public string CoverImageUrl { get; private set; }

        public override string ToString()
        {
            return String.Format("{0} - {1} ({2})", Title, Author, Asin);
        }
    }
}