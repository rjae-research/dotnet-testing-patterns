using System;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UseCustomMocks.Tests
{
    public class BookContextTests
    {
        /*
         * Anti-pattern: Use of mocking framework (when not required).
         *
         * Mocking frameworks use model reflection. As such they introduce significant overhead.
         * As the usages increase with the size of project, the overhead can slow development, testing,
         * and most especially the release cycle.
         */
        [Fact]
        public void AddUserMustAddUserWhenSaveChangesCalled()
        {
            BookRepository repository = new BookRepository(new MockBookContext {Books = new MockDbSet<Book>()});
            Guid id = Guid.NewGuid();
            repository.Add(new Book("Goodnight Moon", "Margaret Wise Brown", id));
            Assert.Equal("Goodnight Moon", repository.Find(id).Title);
        }

        [Fact]
        public void BooksMustReturnBooksDbSetWhenSet()
        {
            BookContext context = new BookContext(new DbContextOptions<BookContext>());
            Assert.NotNull(context.Books);
        }

        [Fact]
        public void BooksMustReturnNullWhenBooksNotSet()
        {
            BookContext context = new BookContext {Books = null};
            Assert.Null(context.Books);
        }

        private class BookRepository
        {
            public BookRepository(BookContext context)
            {
                Context = context;
            }

            public void Add(Book book)
            {
                Context.Books.Add(book);
                Context.SaveChanges();
            }

            public Book Find(Guid id)
            {
                return Context.Books.Find(id);
            }

            private BookContext Context { get; }
        }
    }
}