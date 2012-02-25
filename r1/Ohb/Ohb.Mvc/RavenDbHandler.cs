using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Castle.Windsor;
using Ohb.Mvc.Api.Controllers;
using Raven.Client;

namespace Ohb.Mvc
{
    //public class Foo : DefaultHttpControllerFactory
    //{
    //    public Foo(HttpConfiguration configuration) : base(configuration)
    //    {
    //    }

    //    public override IHttpController CreateController(HttpControllerContext controllerContext, string controllerName)
    //    {
    //        var controller = base.CreateController(controllerContext, controllerName);

    //        var c = controller as OhbApiController;
    //        if (c != null)
    //            c.DocumentSession = controllerContext.Request.Properties["DocumentSession"];

    //        return controller;
    //    }
    //}

    public class RavenDbHandler : DelegatingHandler
    {
        readonly IWindsorContainer container;

        public RavenDbHandler(IWindsorContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            this.container = container;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var documentSession = container.Resolve<IDocumentStore>().OpenSession();
            request.Properties.Add("DocumentSession", documentSession);

            return base.SendAsync(request, cancellationToken)
                .ContinueWith(result =>
                                  {
                                      if (result.Result.IsSuccessStatusCode)
                                          documentSession.SaveChanges();

                                      documentSession.Dispose();

                                      return result.Result;
                                  });
        }

        //public override void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //    var controller = filterContext.Controller as OhbApiController;

        //    if (controller == null)
        //        return;

        //    var session = controller.DocumentSession;
        //    session.SaveChanges();
        //    session.Dispose();
        //}
    }
}