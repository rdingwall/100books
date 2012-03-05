using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Ohb.Mvc.AuthCookies
{
    public interface IAuthCookieEncoder : IDisposable
    {
        bool TryDecode(string base64Encoded, out AuthCookieContext output);
        string Encode(AuthCookieContext context);
    }

    /// <summary>
    /// Similar to https://github.com/dchest/authcookie. Produces a signed
    /// cookie containing the user ID and expiration time using:
    /// 
    /// cookie = base64encode(user id|expiration time|signature)
    /// 
    /// where
    /// 
    /// signature = HMAC-SHA256(user id|expiration time|secret key)
    /// 
    /// The user ID, expiration time and signature are all percent encoded
    /// (URL encode) before being base64 encoded. In addition the user ID and
    /// expiration time are percent encoded before being hashed (so they match
    /// the unhashed values).
    /// </summary>
    public class AuthCookieEncoder : IAuthCookieEncoder
    {
        HMACSHA1 hmacSha1;
        const string DateFormat = "u";
        const char Separator = '|';
        static readonly Encoding utf8 = Encoding.UTF8;

        public AuthCookieEncoder(string secretKey)
        {
            if (String.IsNullOrEmpty(secretKey)) 
                throw new ArgumentException("Null/empty argument.", "secretKey");

            hmacSha1 = new HMACSHA1 { Key = utf8.GetBytes(secretKey) };
        }

        public bool TryDecode(string base64Encoded, out AuthCookieContext output)
        {
            if (base64Encoded == null) throw new ArgumentNullException("base64Encoded");

            output = null;

            if (String.IsNullOrEmpty(base64Encoded))
                return false;

            try
            {
                // URL-encode safe base64
                // See http://stackoverflow.com/questions/1228701/code-for-decoding-encoding-a-modified-base64-url
                var cookie = utf8.GetString(HttpServerUtility.UrlTokenDecode(base64Encoded));

                var segments = cookie.Split(Separator);
                if (segments.Length != 3)
                    return false;

                var userId = segments[0];
                var expirationTime = segments[1];
                var signature = segments[2];

                //Console.WriteLine("User ID: {0}, Expiration Time: {1}, Signature = {2}",
                //    userId, expirationTime, signature);

                var context =
                    new AuthCookieContext
                        {
                            UserId = userId,
                            ExpirationTime = DateTime.ParseExact(expirationTime, DateFormat, null),
                        };

                if (signature != Sign(context))
                {
                    //Console.WriteLine("Expected: {0}", Sign(context));
                    //Console.WriteLine("Actual: {0}", signature);
                    return false;
                }

                output = context;
                return true;
            }
            catch (FormatException e)
            {
                //Console.WriteLine(e.ToString());
                return false;
            }
        }

        public string Encode(AuthCookieContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            var userIdAndExpiry = GetUserIdAndExpirySegment(context);
            var signature = Sign(context);
            
            var cookie = String.Concat(
                userIdAndExpiry, 
                Separator,
                signature);

            // URL-encode safe base64
            // See http://stackoverflow.com/questions/1228701/code-for-decoding-encoding-a-modified-base64-url
            return HttpServerUtility.UrlTokenEncode(utf8.GetBytes(cookie));
        }

        static string GetUserIdAndExpirySegment(AuthCookieContext cookieContext)
        {
            return String.Concat(
                cookieContext.UserId,
                Separator,
                cookieContext.ExpirationTime.ToString(DateFormat));
        }

        string Sign(AuthCookieContext cookieContext)
        {
            var plainTextStr = GetUserIdAndExpirySegment(cookieContext);
            var plainText = utf8.GetBytes(plainTextStr);

            var hash = hmacSha1.ComputeHash(plainText);

            // URL-encode safe base64
            // See http://stackoverflow.com/questions/1228701/code-for-decoding-encoding-a-modified-base64-url
            return HttpServerUtility.UrlTokenEncode(hash);
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
                    if (hmacSha1 != null)
                    {
                        hmacSha1.Dispose();
                        hmacSha1 = null;
                    }
                }

                disposed = true;
            }
        }

        ~AuthCookieEncoder()
        {
            Dispose(false);
        }

        bool disposed;
    }
}