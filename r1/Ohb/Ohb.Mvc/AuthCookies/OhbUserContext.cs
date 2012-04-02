using System;
using System.Web;
using Castle.Windsor;

namespace Ohb.Mvc.AuthCookies
{
    public class OhbUserContext
    {
        public string UserId { get; set; }
        public bool IsAuthenticated { get; set; }

        public static void SetContainer(IWindsorContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            
            factory = container.Resolve<IOhbUserContextFactory>();
        }

        public static OhbUserContext Current
        {
            get
            {
                return factory.CreateFromAuthCookie(new HttpContextWrapper(HttpContext.Current));
            }
        }

        static IOhbUserContextFactory factory;
    }
}