using System.Reflection;
using System.Web.Mvc;

namespace Ohb.Mvc.Helpers
{
    public static class HtmlHelperExtensions
    {
        static readonly string version = 
            Assembly.GetExecutingAssembly().GetName().Version.ToString(); 

        public static string AssemblyVersion(this HtmlHelper helper)
        {
            return version;
        }

        public static string AssemblyVersion<T>(this HtmlHelper<T> helper)
        {
            return version;
        }
    }
}