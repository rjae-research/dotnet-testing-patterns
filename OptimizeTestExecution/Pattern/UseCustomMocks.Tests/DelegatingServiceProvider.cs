using System;

namespace UseCustomMocks.Tests
{
    public class DelegatingServiceProvider : IServiceProvider, IDisposable
    {
        public DelegatingServiceProvider(Func<Type, object> getServiceFunction, Action disposeAction)
        {
            GetServiceFunction = getServiceFunction;
            DisposeAction = disposeAction;
        }

        public void Dispose()
        {
            DisposeAction();
        }

        public object GetService(Type serviceType)
        {
            return GetServiceFunction(serviceType);
        }

        private Action DisposeAction { get; }

        private Func<Type, object> GetServiceFunction { get; }
    }
}