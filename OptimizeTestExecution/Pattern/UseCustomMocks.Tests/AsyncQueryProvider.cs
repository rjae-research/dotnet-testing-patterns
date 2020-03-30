using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace UseCustomMocks.Tests
{
    public class AsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        public AsyncQueryProvider(IQueryProvider provider)
        {
            Provider = provider;
        }

        public virtual IQueryable CreateQuery(Expression expression)
        {
            return new AsyncEnumerable<TEntity>(expression);
        }

        public virtual IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new AsyncEnumerable<TElement>(expression);
        }

        public virtual object Execute(Expression expression)
        {
            return Provider.Execute(expression);
        }

        public virtual TResult Execute<TResult>(Expression expression)
        {
            return Provider.Execute<TResult>(expression);
        }

        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = new CancellationToken())
        {
            Type resultType = typeof(TResult).GetGenericArguments()[0];
            return GetTaskFromResult<TResult>(GetResultFromExpression<TResult>(expression, resultType), resultType);
        }

        protected virtual object GetResultFromExpression<TResult>(Expression expression, Type resultType)
        {
            return typeof(IQueryProvider).GetMethod(nameof(IQueryProvider.Execute), 1, new[] {typeof(Expression)}).MakeGenericMethod(resultType).Invoke(this, new[] {expression});
        }

        protected virtual TResult GetTaskFromResult<TResult>(object result, Type resultType)
        {
            return (TResult) typeof(Task).GetMethod(nameof(Task.FromResult))?.MakeGenericMethod(resultType).Invoke(null, new[] {result});
        }

        private IQueryProvider Provider { get; }
    }
}