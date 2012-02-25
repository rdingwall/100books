using System;
using System.Net.Http;
using Raven.Client;

namespace Ohb.Mvc
{
    public static class HttpRequestMessageExtensions
    {
        public static IDocumentSession DocumentSession(this HttpRequestMessage request)
        {
            if (!request.Properties.ContainsKey("DocumentSession"))
                throw new InvalidOperationException("No DocumentSession bound in HttpRequestMessage!");

            return request.Properties["DocumentSession"] as IDocumentSession;
        }
    }
}