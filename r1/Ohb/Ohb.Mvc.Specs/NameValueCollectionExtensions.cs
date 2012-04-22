using System.Collections.Specialized;

// ReSharper disable CheckNamespace
namespace System.Configuration
// ReSharper restore CheckNamespace
{
    public static class NameValueCollectionExtensions
    {
        public static string IntegrationTestUrl(this NameValueCollection items)
        {
            var url = items["IntegrationTestUrl"];
            if (String.IsNullOrWhiteSpace(url))
                throw new ConfigurationErrorsException("The required 'IntegrationTestUrl' AppSetting was null or empty.");

            return url;
        }
    }
}