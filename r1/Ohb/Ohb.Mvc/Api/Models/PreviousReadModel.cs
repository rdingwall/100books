using System;
using Ohb.Mvc.Storage.Books;

namespace Ohb.Mvc.Api.Models
{
    public class PreviousReadModel
    {
        public Book Book { get; set; }
        public DateTime MarkedByUserAt { get; set; }
    }
}