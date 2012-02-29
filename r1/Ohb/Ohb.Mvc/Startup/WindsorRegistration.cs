using Bootstrap.Windsor;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Ohb.Mvc.Api;
using Ohb.Mvc.Controllers;
using Ohb.Mvc.Google;
using Ohb.Mvc.Storage;
using Raven.Client;
using Raven.Client.Document;

namespace Ohb.Mvc.Startup
{
    public class WindsorRegistration : IWindsorRegistration
    {
        public void Register(IWindsorContainer container)
        {
            container.Register(
                Component.For<HomeController>().LifeStyle.Transient,
                Component.For<AccountController>().LifeStyle.Transient,
                Component.For<ProfileController>().LifeStyle.Transient,
                Component.For<SearchController>().LifeStyle.Transient,
                Component.For<BooksController>().LifeStyle.Transient);

            container.Register(Component.For<IUserFactory>().ImplementedBy<UserFactory>(),
                               Component.For<IUserRepository>().ImplementedBy<UserRepository>(),
                               Component.For<IBookRepository>().ImplementedBy<BookRepository>(),
                               Component.For<IBookImporter>().ImplementedBy<BookImporter>(),
                               Component.For<IGoogleBooksClient>().ImplementedBy<GoogleBooksClient>()
                                   .DependsOn(new
                                                  {
                                                      apiKey = "AIzaSyDQsH0G4o3l9FjHUocTO_edha6Pv8N3NXo"
                                                  }),
                               Component.For<IDocumentStore>().UsingFactoryMethod(GetDocumentStore));

            container.Install(new ApiInstaller());
        }

        static DocumentStore GetDocumentStore()
        {
            var store = new DocumentStore {ConnectionStringName = "RavenDB"};
            store.Initialize();
            return store;
        }
    }
}