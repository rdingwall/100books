using System;

namespace Ohb.Mvc.Storage.Books
{
    public class Book
    {
        public string GoogleVolumeId { get; set; }
        public BookStaticInfo StaticInfo { get; set; }
        public string Id { get; set; }

        public override string ToString()
        {
            return string.Format("[VolumeID {0}]", GoogleVolumeId);
        }
    }
}