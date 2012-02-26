using Ohb.Mvc.Api.Models;

namespace Ohb.Mvc.Storage
{
    public class Book
    {
        public string GoogleVolumeId { get; set; }
        public BookStaticInfo StaticInfo { get; set; }

        public override string ToString()
        {
            return string.Format("[VolumeID {0}]", GoogleVolumeId);
        }
    }
}