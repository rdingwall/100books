using System;
using System.Linq;
using Ohb.Mvc.Startup;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace Ohb.Mvc.Specs.IntegrationTests
{
    /// <summary>
    /// Test helpers for Raven DB sandbox Database management.
    /// </summary>
    public static class LiveRavenDb
    {
        /// <summary>
        /// Gets the DocumentStore for the current Raven DB Database (Tenant).
        /// </summary>
        public static IDocumentStore DocumentStore
        {
            get { return documentStore.Value; }
        }

        /// <summary>
        /// Gets the current Raven DB Database (Tenant) name.
        /// </summary>
        public static string DatabaseName
        {
            get { return "Ohb"; }
        }

        /// <summary>
        /// Opens a new DocumentSession for the current Raven DB Database 
        /// (Tenant).
        /// </summary>
        /// <returns>A new DocumentSession.</returns>
        public static IDocumentSession OpenSession()
        {
            return DocumentStore.OpenSession();
        }

        private static readonly Lazy<IDocumentStore> documentStore =
            new Lazy<IDocumentStore>(CreateStore);

        private static IDocumentStore CreateStore()
        {
            var store = new DocumentStore
                            {
                                Url = "http://localhost:8080",
                                DefaultDatabase = DatabaseName
                            };
            new RavenDbConventions().Apply(store);
            store.Initialize();
            return store;
        }

        public static void WaitForNonStaleResults<T>()
        {
            using (var session = OpenSession())
                session.Query<T>().Customize(a => a.WaitForNonStaleResults()).Any();
        }

        public static void WaitForNonStaleResults<T, TIndex>() where TIndex : AbstractIndexCreationTask, new()
        {
            using (var session = OpenSession())
                session.Query<T, TIndex>().Customize(a => a.WaitForNonStaleResults()).Any();
        }
    }
}