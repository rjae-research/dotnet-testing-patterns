using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace UseCustomMocks.Tests
{
#pragma warning disable EF1001 // Internal EF Core API usage.
    public class MockDbSet<T> : DbSet<T>, IQueryable, IEnumerable<T> where T : class
    {
        public MockDbSet(IEnumerable<T> entities = null, Func<T, object[], bool> predicate = null)
        {
            Entities = entities == null ? new List<T>() : new List<T>(entities);
            Predicate = predicate ?? GetDefaultPredicate();
            Expression = Entities.AsQueryable().Expression;
            Provider = new AsyncQueryProvider<T>(Entities.AsQueryable().Provider);
        }

        public override EntityEntry<T> Add(T entity)
        {
            Entities.Add(entity);
            return new EntityEntry<T>(new MockEntityEntry(entity, null, new EntityType(typeof(T), new Model(), ConfigurationSource.Explicit)));
        }

        public Expression Expression { get; }

        public override T Find(params object[] keyValues)
        {
            if (Predicate == null)
                return null;
            foreach (T entity in Entities)
                if (Predicate(entity, keyValues))
                    return entity;
            return null;
        }

        public virtual IEnumerator<T> GetEnumerator()
        {
            return Entities.GetEnumerator();
        }

        public IQueryProvider Provider { get; }

        public override EntityEntry<T> Remove(T entity)
        {
            Entities.Remove(entity);
            return new EntityEntry<T>(new MockEntityEntry(entity, null, new EntityType(typeof(T), new Model(), ConfigurationSource.Explicit)));
        }

        public override void RemoveRange(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
                Remove(entity);
        }

        public override void RemoveRange(params T[] entities)
        {
            foreach (T entity in entities)
                Remove(entity);
        }

        protected List<T> Entities { get; }

        protected virtual Func<T, object[], bool> GetDefaultPredicate()
        {
            return (x, y) => ReflectionHelper.GetProperty<object>(x, "Id").Equals(y[0]);
        }

        protected Func<T, object[], bool> Predicate { get; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
#pragma warning restore EF1001 // Internal EF Core API usage.
}