using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UseCustomMocks.Tests
{
    public class AsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        public AsyncEnumerator(IEnumerator<T> enumerator)
        {
            Enumerator = enumerator ?? throw new ArgumentNullException(nameof(enumerator));
        }

        public T Current => Enumerator.Current;

        public virtual ValueTask DisposeAsync()
        {
            return new ValueTask(Task.CompletedTask);
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(Task.FromResult(Enumerator.MoveNext()));
        }

        private IEnumerator<T> Enumerator { get; }
    }
}