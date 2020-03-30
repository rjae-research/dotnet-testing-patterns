using Microsoft.EntityFrameworkCore;

namespace UseCustomMocks
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions options) : base(options)
        {
        }

        public BookContext( /* EF */)
        {
        }

        public virtual DbSet<Book> Books { get; set; }
    }
}