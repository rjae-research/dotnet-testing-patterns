using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace UseMockingFrameworks.Tests
{
    public class BookContextTests
    {
        /*
         * Anti-pattern: Use of mocking framework (when not required).
         *
         * Note that this version of AddUserMustAddUserWhenSaveChangesCalled is *10 times* slower
         * than UseCustomMocks.Tests.AddUserMustAddUserWhenSaveChangesCalled.
         *
         * Mocking frameworks use model reflection. As such they introduce significant overhead.
         * As the usages increase with the size of project, the overhead can slow development, testing,
         * and most especially the release cycle.
         */
        [Fact]
        public void AddUserMustAddUserWhenSaveChangesCalled()
        {
            List<Book> bookList = new List<Book>();
            IQueryable<Book> bookQuery = bookList.AsQueryable();
            Mock<DbSet<Book>> bookDbSet = new Mock<DbSet<Book>>();
            bookDbSet.As<IQueryable<Book>>().Setup(x => x.Provider).Returns(bookQuery.Provider);
            bookDbSet.As<IQueryable<Book>>().Setup(x => x.Expression).Returns(bookQuery.Expression);
            bookDbSet.As<IQueryable<Book>>().Setup(x => x.ElementType).Returns(bookQuery.ElementType);
            bookDbSet.As<IQueryable<Book>>().Setup(x => x.GetEnumerator()).Returns(bookQuery.GetEnumerator());
            Mock<BookContext> context = new Mock<BookContext>();
            context.Setup(x => x.Books).Returns(bookDbSet.Object);
            context.Setup(x => x.Books.Add(It.IsAny<Book>())).Callback<Book>(x => bookList.Add(x));
            Guid bookId = Guid.NewGuid();
            context.Object.Books.Add(new Book("Goodnight Moon", "Margaret Wise Brown", bookId));
            context.Object.SaveChanges();
            Assert.Contains(bookQuery, x => x.Id == bookId);
        }

        [Fact]
        public void BooksMustReturnBooksDbSetWhenSet()
        {
            FakeBookContext context = new FakeBookContext();
            Assert.NotNull(context.Books);
        }

        [Fact]
        public void ConstructorMustReturnBookContextWhenCalled()
        {
            Assert.IsAssignableFrom<BookContext>(new FakeBookContext());
        }

        [Fact]
        public void ConstructorMustReturnBookContextWhenCalledWithDbContextOptions()
        {
            Assert.IsAssignableFrom<BookContext>(new FakeBookContext(new DbContextOptions<BookContext>()));
        }

        private class FakeBookContext : BookContext
        {
            public FakeBookContext(DbContextOptions options) : base(options)
            {
            }

            public FakeBookContext()
            {
            }
        }
    }
}