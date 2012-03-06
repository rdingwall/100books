using System;
using Ohb.Mvc.Storage.Books;

namespace Ohb.Mvc.Storage.PreviousReads
{
    public class PreviousRead
    {
        public Book Book { get; set; }
        public string Id { get; set; }
        public string UserId { get; set; }
    }
}