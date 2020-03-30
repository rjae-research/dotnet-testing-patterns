using Microsoft.EntityFrameworkCore;

namespace UseMockingFrameworks
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions options) : base(options)
        {
        }

        protected BookContext()
        {
        }

        public virtual DbSet<Book> Books { get; set; }
    }
}