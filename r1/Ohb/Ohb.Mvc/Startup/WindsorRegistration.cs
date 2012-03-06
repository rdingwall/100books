using System.Reflection;
using Bootstrap.Windsor;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Ohb.Mvc.Api;
using Ohb.Mvc.AuthCookies;
using Ohb.Mvc.Google;
using Ohb.Mvc.Storage;
using Ohb.Mvc.Storage.Books;
using Ohb.Mvc.Storage.Users;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace Ohb.Mvc.Startup
{
    public class WindsorRegistration : IWindsorRegistration
    {
        public void Register(IWindsorContainer container)
        {
            container.Register(
                Component.For<IUserFactory>().ImplementedBy<UserFactory>(),
                Component.For<IRavenUniqueInserter>().ImplementedBy<RavenUniqueInserter>(),
                Component.For<IUserRepository>().ImplementedBy<UserRepository>(),
                Component.For<IBookRepository>().ImplementedBy<BookRepository>(),
                Component.For<IBookImporter>().ImplementedBy<BookImporter>(),
                Component.For<IAuthCookieEncoder>().ImplementedBy<AuthCookieEncoder>()
                .DependsOn(new { secretKey = AuthCookieSecretKey.Value }),
                Component.For<IAuthCookieFactory>().ImplementedBy<AuthCookieFactory>(),
                Component.For<IGoogleBooksClient>().ImplementedBy<GoogleBooksClient>()
                    .DependsOn(new
                                   {
                                       apiKey = "AIzaSyDQsH0G4o3l9FjHUocTO_edha6Pv8N3NXo"
                                   }),
                Component.For<IDocumentStore>().UsingFactoryMethod(GetDocumentStore));

            container.Install(new WebInstaller());
            container.Install(new ApiInstaller());
        }

        static DocumentStore GetDocumentStore()
        {
            var store = new DocumentStore { ConnectionStringName = "RavenDB" };
            store.Initialize();
            IndexCreation.CreateIndexes(Assembly.GetExecutingAssembly(), store);
            return store;
        }
    }
}