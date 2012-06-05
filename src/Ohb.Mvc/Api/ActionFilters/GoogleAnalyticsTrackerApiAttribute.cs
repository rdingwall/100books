using System;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using GoogleAnalyticsTracker;

namespace Ohb.Mvc.Api.ActionFilters
{
    public class GoogleAnalyticsTrackerApiAttribute : ActionFilterAttribute, IDisposable
    {
        Tracker tracker;

        public GoogleAnalyticsTrackerApiAttribute(string trackingId)
        {
            if (trackingId == null) throw new ArgumentNullException("trackingId");
            tracker = new Tracker(trackingId, "100books.api");
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var descriptor = actionContext.ActionDescriptor;
            var pageName = String.Format("{0}/{1}", descriptor.ControllerDescriptor.ControllerType.Name,
                                            descriptor.ActionName);

            tracker.TrackPageView(new HttpContextWrapper(HttpContext.Current), pageName);
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
                    if (tracker != null)
                    {
                        tracker.Dispose();
                        tracker = null;
                    }
                }

                disposed = true;
            }
        }

        ~GoogleAnalyticsTrackerApiAttribute()
        {
            Dispose(false);
        }

        bool disposed;
    }
}