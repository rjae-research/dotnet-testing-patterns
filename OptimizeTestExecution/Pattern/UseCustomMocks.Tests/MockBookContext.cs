using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UseCustomMocks.Tests
{
    public class MockBookContext : BookContext
    {
        public MockBookContext() : this(new DbContextOptions<BookContext>())
        {
        }

        public MockBookContext(DbContextOptions options) : base(options)
        {
            Dictionary<Type, object> services = new Dictionary<Type, object> {{typeof(IMigrator), new NullMigrator()}};
            Database = new DatabaseFacadeStub(this, new DelegatingServiceProvider(type => services[type], () => { }));
        }

        public override DatabaseFacade Database { get; }

        public override int SaveChanges()
        {
            return 0;
        }
    }
}