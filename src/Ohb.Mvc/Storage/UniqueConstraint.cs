using System;

namespace Ohb.Mvc.Storage
{
    public class UniqueConstraint
    {
        public string Type { get; set; }
        public string Key { get; set; }
        public string Id { get; set; }

        public static string MakeKey<T>(object key)
        {
            return String.Format("UniqueConstraints/{0}/{1}", typeof(T).Name, key);
        }
    }
}