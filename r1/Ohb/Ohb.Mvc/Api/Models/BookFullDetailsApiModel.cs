using Ohb.Mvc.Models;

namespace Ohb.Mvc.Api.Models
{
    public class BookInfo
    {
        public BookStaticInfo StaticInfo { get; set; }

        public bool HasPreviouslyRead { get; set; }
    }
}