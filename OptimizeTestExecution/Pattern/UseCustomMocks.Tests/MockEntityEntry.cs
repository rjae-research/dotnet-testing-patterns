using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace UseCustomMocks.Tests
{
#pragma warning disable EF1001 // Internal EF Core API usage.
    public class MockEntityEntry : InternalEntityEntry
    {
        public MockEntityEntry(object entity, IStateManager stateManager, IEntityType entityType) : base(stateManager, entityType)
        {
            Entity = entity;
        }

        public override object Entity { get; }
    }
#pragma warning restore EF1001 // Internal EF Core API usage.
}