using System.Web;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Registration.Lifestyle;

namespace Ohb.Mvc.Startup
{
    public static class WindsorExtensions
    {
        public static ComponentRegistration<T> PerWebRequestIfPossible<T>(this LifestyleGroup<T> lifestyleGroup)
        {
            return HttpContext.Current == null ? lifestyleGroup.Transient : lifestyleGroup.PerWebRequest;
        }
    }
}