using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace UseCustomMocks.Tests
{
    public class DatabaseFacadeStub : DatabaseFacade, IInfrastructure<IServiceProvider>
    {
        public DatabaseFacadeStub(DbContext context, IServiceProvider provider) : base(context)
        {
            Provider = provider;
        }

        IServiceProvider IInfrastructure<IServiceProvider>.Instance => Provider;

        private IServiceProvider Provider { get; }
    }
}