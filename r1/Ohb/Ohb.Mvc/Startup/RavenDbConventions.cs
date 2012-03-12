using System;
using Raven.Client;

namespace Ohb.Mvc.Startup
{
    public class RavenDbConventions
    {
        public void Apply(IDocumentStore store)
        {
            if (store == null) throw new ArgumentNullException("store");

            // using hyphen for IDs not slash (slashes are too hard for URL routing)
            store.Conventions.IdentityPartsSeparator = "-";
        }
    }
}