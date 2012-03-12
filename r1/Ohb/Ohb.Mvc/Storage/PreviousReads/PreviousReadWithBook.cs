using System;
using Ohb.Mvc.Storage.Books;

namespace Ohb.Mvc.Storage.PreviousReads
{
    public class PreviousReadWithBook
    {
        public Book Book { get; set; }
        public DateTime MarkedByUserAt { get; set; }
    }
}