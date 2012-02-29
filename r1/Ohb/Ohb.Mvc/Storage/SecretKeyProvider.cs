using System;
using Raven.Abstractions.Exceptions;
using Raven.Client;

namespace Ohb.Mvc.Storage
{
    public interface ISecretKeyProvider
    {
        string GetUniqueKey(IDocumentSession session);
    }

    public class SecretKeyProvider : ISecretKeyProvider
    {
        readonly ISecretKeyGenerator generator;

        public SecretKeyProvider(ISecretKeyGenerator generator)
        {
            if (generator == null) throw new ArgumentNullException("generator");
            this.generator = generator;
        }

        public string GetUniqueKey(IDocumentSession session)
        {
            if (session == null) throw new ArgumentNullException("session");

            for (;;)
            {
                var key = generator.GetNext();
                try
                {
                    StoreKey(key, session);
                    return key;
                }
                catch (ConcurrencyException)
                {
                    // already used by someone - loop and try another
                    continue;
                }
            }

        }

        static void StoreKey(string key, IDocumentSession session)
        {
            var userKey = new SecretUserKey {SecretKey = key};
            try
            {
                session.Advanced.UseOptimisticConcurrency = true;

                session.Store(userKey, 
                    String.Concat("SecretUserKeys/", key));
                    
                session.SaveChanges();
            }
            catch (ConcurrencyException)
            {
                session.Advanced.Evict(userKey);
                throw;
            }
            finally
            {
                session.Advanced.UseOptimisticConcurrency = false;
            }
        }
    }
}