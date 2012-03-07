using System;
using System.Runtime.Caching;

namespace Ohb.Mvc.AuthCookies
{
    public class AuthCookieCache : IAuthCookieEncoder
    {
        MemoryCache cache;
        readonly IAuthCookieEncoder encoder;

        public AuthCookieCache(IAuthCookieEncoder encoder)
        {
            if (encoder == null) throw new ArgumentNullException("encoder");
            this.encoder = encoder;
            cache = new MemoryCache(typeof(AuthCookieCache).Name);
        }

        public bool TryDecode(string base64Encoded, out AuthCookieContext output)
        {
            if (base64Encoded == null) throw new ArgumentNullException("base64Encoded");

            if (cache.Contains(base64Encoded))
            {
                output = (AuthCookieContext) cache.Get(base64Encoded);
                return true;
            }

            var result = encoder.TryDecode(base64Encoded, out output);
            cache[base64Encoded] = output;
            return result;
        }

        public string Encode(AuthCookieContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            var base64Encoded = encoder.Encode(context);
            cache[base64Encoded] = context;
            return base64Encoded;
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (cache != null)
                    {
                        cache.Dispose();
                        cache = null;
                    }
                }

                disposed = true;
            }
        }

        ~AuthCookieCache()
        {
            Dispose(false);
        }

        bool disposed;

        
    }
}