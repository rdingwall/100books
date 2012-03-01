using System;
using Raven.Abstractions.Exceptions;
using Raven.Client;

namespace Ohb.Mvc.Storage.ApiTokens
{
    public interface IApiTokenFactory
    {
        ApiToken CreateApiToken(string userId, IDocumentSession session);
    }

    public class ApiTokenFactory : IApiTokenFactory
    {
        readonly ICryptoTokenGenerator generator;
        readonly IRavenUniqueInserter inserter;

        readonly static TimeSpan expiresAfter = TimeSpan.FromHours(2); // same as Facebook

        public ApiTokenFactory(ICryptoTokenGenerator generator, 
            IRavenUniqueInserter inserter)
        {
            if (generator == null) throw new ArgumentNullException("generator");
            if (inserter == null) 
                throw new ArgumentNullException("inserter");
            this.generator = generator;
            this.inserter = inserter;
        }

        public ApiToken CreateApiToken(string userId, IDocumentSession session)
        {
            if (String.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("Missing/empty parameter.", "userId");
            if (session == null) throw new ArgumentNullException("session");

            for (;;)
            {
                var token = generator.GetNext();
                var apiToken = new ApiToken {Token = token, UserId = userId };
                try
                {
                    inserter.StoreUnique(session, apiToken, t => t.Token);
                    return apiToken;
                }
                catch (ConcurrencyException)
                {
                    // already used by someone - loop and try another
                    continue;
                }
            }

        }
    }
}