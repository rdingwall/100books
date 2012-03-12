using System;

namespace Ohb.Mvc.Api.Models
{
    public class PreviousReadModel
    {
        public BookModel Book { get; set; }
        public DateTime MarkedByUserAt { get; set; }
    }
}