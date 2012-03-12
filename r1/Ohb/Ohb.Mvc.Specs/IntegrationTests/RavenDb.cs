using System;
using System.Linq;
using System.Reflection;
using Ohb.Mvc.Startup;
using Raven.Client;
using Raven.Client.Document;

namespace Ohb.Mvc.Specs.IntegrationTests
{
    /// <summary>
    /// Test helpers for Raven DB sandbox Database management.
    /// </summary>
    public static class RavenDb
    {
        static string databaseName = GenerateNextDatabaseName();

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
            get { return databaseName; }
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

        /// <summary>
        /// Initializes DocumentStore using an empty Database (Tenant) in 
        /// Raven DB. Any subsequent calls to DocumentStore and OpenSession()
        /// will use this new Database.
        /// </summary>
        public static void SpinUpNewDatabase()
        {
            if (documentStore.IsValueCreated)
                documentStore.Value.Dispose();

            databaseName = GenerateNextDatabaseName();

            Console.WriteLine("Using RavenDB database name = {0}", databaseName);

            documentStore = new Lazy<IDocumentStore>(CreateStore);
        }

        private static Lazy<IDocumentStore> documentStore =
            new Lazy<IDocumentStore>(CreateStore);
        
        private static string GenerateNextDatabaseName()
        {
            // Using test assembly name + test timestamp for unique DB name.
            // For shared Raven DB servers, you could also use the machine
            // name, current logged in user etc.
            return String.Format("{0}-{1}",
                                 Assembly.GetExecutingAssembly().GetName().Name,
                                 DateTime.Now.Ticks);
        }

        private static IDocumentStore CreateStore()
        {
            var store = new DocumentStore
                            {
                                Url = "http://localhost:8080",
                                DefaultDatabase = databaseName
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
    }
}