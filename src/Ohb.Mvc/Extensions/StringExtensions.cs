using HtmlAgilityPack;

// ReSharper disable CheckNamespace
namespace System
// ReSharper restore CheckNamespace
{
    public static class StringExtensions
    {
        public static string StripHtmlTags(this string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc.DocumentNode.InnerText;
        }
    }
}