using Ohb.Mvc.Api.Models;

namespace Ohb.Mvc.Storage
{
    public class Book
    {
        public string Id { get; set; }
        public BookStaticInfo StaticInfo { get; set; }
    }
}