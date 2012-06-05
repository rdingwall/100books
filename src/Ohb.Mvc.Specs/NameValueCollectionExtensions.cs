using System.Collections.Specialized;

// ReSharper disable CheckNamespace
namespace System.Configuration
// ReSharper restore CheckNamespace
{
    public static class NameValueCollectionExtensions
    {
        public static string TestUrl(this NameValueCollection items)
        {
            var url = items["TestUrl"];
            if (String.IsNullOrWhiteSpace(url))
                throw new ConfigurationErrorsException("The required 'TestUrl' AppSetting was null or empty.");

            return url;
        }
    }
}