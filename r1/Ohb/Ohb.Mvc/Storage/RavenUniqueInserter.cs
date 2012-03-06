using System;
using System.Linq.Expressions;
using Raven.Abstractions.Exceptions;
using Raven.Client;

namespace Ohb.Mvc.Storage
{
public interface IRavenUniqueInserter
{
    void StoreUnique<T, TUnique>(
        IDocumentSession session, T entity,
        Expression<Func<T, TUnique>> keyProperty);
}

public class RavenUniqueInserter : IRavenUniqueInserter
{
    public void StoreUnique<T, TUnique>(IDocumentSession session, T entity, 
                                    Expression<Func<T, TUnique>> keyProperty)
    {
        if (session == null) throw new ArgumentNullException("session");
        if (keyProperty == null) throw new ArgumentNullException("keyProperty");
        if (entity == null) throw new ArgumentNullException("entity");

        var key = keyProperty.Compile().Invoke(entity).ToString();
        var type = typeof (T).Name;
        var id = String.Format("UniqueConstraints/{0}/{1}", type, key);

        var constraint = new UniqueConstraint
                                {
                                    Id = id,
                                    Type = type,
                                    Key = key
                                };

        DoStore(session, entity, constraint);
    }

    static void DoStore<T>(IDocumentSession session, T entity, 
        UniqueConstraint constraint)
    {
        var previousSetting = session.Advanced.UseOptimisticConcurrency;

        try
        {
            session.Advanced.UseOptimisticConcurrency = true;
            session.Store(constraint);
            session.Store(entity);
            session.SaveChanges();
        }
        catch (ConcurrencyException)
        {
            // rollback changes so we can keep using the session
            session.Advanced.Evict(entity);
            session.Advanced.Evict(constraint);
            throw;
        }
        finally
        {
            session.Advanced.UseOptimisticConcurrency = previousSetting;
        }
    }
}
}