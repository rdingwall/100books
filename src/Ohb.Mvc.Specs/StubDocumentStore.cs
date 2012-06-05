using System;
using System.Collections.Specialized;
using System.Net;
using Raven.Client;
using Raven.Client.Connection;
using Raven.Client.Connection.Async;
using Raven.Client.Document;
using Rhino.Mocks;

namespace Ohb.Mvc.Specs
{
    public class StubDocumentStore : IDocumentStore
    {
        public StubDocumentStore()
        {
            Session = MockRepository.GenerateMock<IDocumentSession>();
        }

        public IDocumentSession Session { get; set; }

        public void Dispose()
        {
        }

        public bool WasDisposed
        {
            get { throw new NotImplementedException(); }
        }

        public event EventHandler AfterDispose;
        public IDisposable AggressivelyCacheFor(TimeSpan cahceDuration)
        {
            throw new NotImplementedException();
        }

        public IDisposable DisableAggressiveCaching()
        {
            throw new NotImplementedException();
        }

        public IDocumentStore Initialize()
        {
            throw new NotImplementedException();
        }

        public IAsyncDocumentSession OpenAsyncSession()
        {
            throw new NotImplementedException();
        }

        public IAsyncDocumentSession OpenAsyncSession(string database)
        {
            throw new NotImplementedException();
        }

        public IDocumentSession OpenSession()
        {
            return Session;
        }

        public IDocumentSession OpenSession(string database)
        {
            throw new NotImplementedException();
        }

        public IDocumentSession OpenSession(string database, ICredentials credentialsForSession)
        {
            throw new NotImplementedException();
        }

        public IDocumentSession OpenSession(ICredentials credentialsForSession)
        {
            throw new NotImplementedException();
        }

        public Guid? GetLastWrittenEtag()
        {
            throw new NotImplementedException();
        }

        public NameValueCollection SharedOperationsHeaders
        {
            get { throw new NotImplementedException(); }
        }

        public HttpJsonRequestFactory JsonRequestFactory
        {
            get { throw new NotImplementedException(); }
        }

        public string Identifier
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public IAsyncDatabaseCommands AsyncDatabaseCommands
        {
            get { throw new NotImplementedException(); }
        }

        public IDatabaseCommands DatabaseCommands
        {
            get { throw new NotImplementedException(); }
        }

        public DocumentConvention Conventions
        {
            get { throw new NotImplementedException(); }
        }

        public string Url
        {
            get { throw new NotImplementedException(); }
        }
    }
}