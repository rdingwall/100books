using System.Linq;
using System.Reflection;

namespace Ohb.Mvc.Models
{
    public class AppVersionModel
    {
        static AppVersionModel()
        {
            var assembly = Assembly.GetExecutingAssembly();
            Current = new AppVersionModel
                          {
                              AppVersion = assembly.GetName().Version.ToString(),
                              GitCommit = assembly
                                  .GetCustomAttributes(typeof(AssemblyConfigurationAttribute), true)
                                  .Cast<AssemblyConfigurationAttribute>().Single().Configuration
                          };
        }

        public string AppVersion { get; set; }
        public string GitCommit { get; set; }

        public static AppVersionModel Current { get; set; }

    }
}