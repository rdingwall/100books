using System;

namespace Ohb.Mvc.Api.Models
{
    public class PreviousReadModel : BookModel
    {
        public DateTime MarkedByUserAt { get; set; }
    }
}