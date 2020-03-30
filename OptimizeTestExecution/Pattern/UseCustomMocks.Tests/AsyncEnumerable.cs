using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace UseCustomMocks.Tests
{
    public class AsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>
    {
        public AsyncEnumerable(Expression expression) : base(expression)
        {
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
        {
            return new AsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }
    }
}