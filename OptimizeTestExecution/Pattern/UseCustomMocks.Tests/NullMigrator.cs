using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UseCustomMocks.Tests
{
    public class NullMigrator : IMigrator
    {
        public string GenerateScript(string fromMigration = null, string toMigration = null, bool idempotent = false)
        {
            return string.Empty;
        }

        public void Migrate(string targetMigration = null)
        {
        }

        public Task MigrateAsync(string targetMigration = null, CancellationToken cancellationToken = new CancellationToken())
        {
            Migrate();
            return Task.CompletedTask;
        }
    }
}