using System;
using System.IO;
using System.ServiceProcess;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Extensions;

namespace Ohb.Mvc.Specs.IntegrationTests.Storage
{
    public static class TestRavenDb
    {
        public const string RavenDbDir = @"C:\RavenDB";
        public static string TenantName = GetNewTenantName();

        static string GetNewTenantName()
        {
            return "OhbTests-" + DateTime.Now.Ticks;
        }

        static Lazy<IDocumentStore> documentStore = new Lazy<IDocumentStore>(CreateStore);

        static IDocumentStore CreateStore()
        {
            var store = new DocumentStore {Url = "http://localhost:8080"};
            store.Initialize();
            store.DatabaseCommands.EnsureDatabaseExists(TenantName);
            return store;
        }

        public static IDocumentStore DocumentStore
        {
            get { return documentStore.Value; }
        }

        public static IDocumentSession OpenSession()
        {
            return DocumentStore.OpenSession(TenantName);
        }

        public static void UseNewTenant()
        {
            if (documentStore.IsValueCreated)
                documentStore.Value.Dispose();

            TenantName = GetNewTenantName();

            Console.WriteLine("Using RavenDB database name = {0}", TenantName);

            documentStore = new Lazy<IDocumentStore>(CreateStore);
        }
    }
}