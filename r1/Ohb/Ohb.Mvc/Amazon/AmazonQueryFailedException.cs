using System;

namespace Ohb.Mvc.Amazon
{
    public class AmazonQueryFailedException : Exception
    {
        public AmazonQueryFailedException(string message) : base(message)
        {
        }
    }
}