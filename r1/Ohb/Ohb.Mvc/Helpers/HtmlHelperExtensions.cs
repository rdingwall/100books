using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Ohb.Mvc.Helpers
{
    public static class HtmlHelperExtensions
    {
        static readonly string version = 
            Assembly.GetExecutingAssembly().GetName().Version.ToString();

        // Using AssemblyConfiguration = git commit hash
        static readonly string commit =
            Assembly.GetExecutingAssembly()
                .GetCustomAttributes(typeof (AssemblyConfigurationAttribute), true)
                .Cast<AssemblyConfigurationAttribute>().Single().Configuration;

        public static string AssemblyVersion(this HtmlHelper helper)
        {
            return version;
        }

        public static string AssemblyVersion<T>(this HtmlHelper<T> helper)
        {
            return version;
        }

        public static string GitCommit(this HtmlHelper helper)
        {
            return commit;
        }

        public static string GitCommit<T>(this HtmlHelper<T> helper)
        {
            return commit;
        }
    }
}