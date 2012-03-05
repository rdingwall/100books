using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Ohb.Mvc.Authentication
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
        readonly string secretKey;
        HMACSHA1 hmacSha1;
        const string DateFormat = "u";
        const char Separator = '&'; // won't be percent-encoded
        static readonly Encoding ascii = Encoding.ASCII;

        public AuthCookieEncoder(string secretKey)
        {
            if (String.IsNullOrEmpty(secretKey)) 
                throw new ArgumentException("Null/empty argument.", "secretKey");

            this.secretKey = secretKey;
            hmacSha1 = new HMACSHA1();
        }

        public bool TryDecode(string base64Encoded, out AuthCookieContext output)
        {
            if (base64Encoded == null) throw new ArgumentNullException("base64Encoded");

            output = null;

            if (String.IsNullOrEmpty(base64Encoded))
                return false;

            try
            {
                var cookie = ascii.GetString(Convert.FromBase64String(base64Encoded));

                var segments = cookie.Split(Separator);
                if (segments.Length != 3)
                    return false;

                var userId = HttpUtility.UrlDecode(segments[0]);
                var expirationTime = HttpUtility.UrlDecode(segments[1]);
                var signature = HttpUtility.UrlDecode(segments[2]);

                var context =
                    new AuthCookieContext
                        {
                            UserId = userId,
                            ExpirationTime = DateTime.ParseExact(expirationTime, DateFormat, null),
                        };

                if (signature != Sign(context))
                    return false;

                output = context;
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public string Encode(AuthCookieContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            var userIdAndExpiry = GetUserAndExpirySegment(context);
            var signature = Sign(context);
            
            var cookie = String.Concat(
                HttpUtility.UrlPathEncode(userIdAndExpiry), 
                Separator,
                HttpUtility.UrlPathEncode(signature));

            return Convert.ToBase64String(ascii.GetBytes(cookie));
        }

        static string GetUserAndExpirySegment(AuthCookieContext cookieContext)
        {
            return String.Concat(
                cookieContext.UserId,
                Separator,
                cookieContext.ExpirationTime.ToString(DateFormat));
        }

        string Sign(AuthCookieContext cookieContext)
        {
            var plainTextStr = String.Concat(
                GetUserAndExpirySegment(cookieContext),
                Separator,
                secretKey);

            var plainText = ascii.GetBytes(plainTextStr);

            var hash = hmacSha1.ComputeHash(plainText);

            return Convert.ToBase64String(hash);
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