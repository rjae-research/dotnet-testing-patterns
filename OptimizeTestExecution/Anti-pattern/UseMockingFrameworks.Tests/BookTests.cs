using System;
using Xunit;

namespace UseMockingFrameworks.Tests
{
    public class BookTests
    {
        [Fact]
        public void ConstructorMustReturnBookWhenCalled()
        {
            Assert.IsAssignableFrom<Book>(new Book());
        }

        [Fact]
        public void ConstructorMustSetIdWhenIdEqualsDefault()
        {
            Assert.NotEqual(default, new Book("Goodnight Moon", "Margaret Wise Brown").Id);
        }

        [Fact]
        public void ConstructorMustSetPropertiesWhenCalledWithPropertyValues()
        {
            Guid id = Guid.NewGuid();
            Book book = new Book("Goodnight Moon", "Margaret Wise Brown", id);
            Assert.Equal("Goodnight Moon", book.Title);
            Assert.Equal("Margaret Wise Brown", book.Author);
            Assert.Equal(id, book.Id);
        }

        [Fact]
        public void PropertiesMustReturnExpectedValuesWhenSet()
        {
            Guid id = Guid.NewGuid();
            StubBook book = new StubBook();
            book.SetTitle("Goodnight Moon");
            book.SetAuthor("Margaret Wise Brown");
            book.SetId(id);
            Assert.Equal("Goodnight Moon", book.Title);
            Assert.Equal("Margaret Wise Brown", book.Author);
            Assert.Equal(id, book.Id);
        }

        private class StubBook : Book
        {
            public void SetAuthor(string author)
            {
                Author = author;
            }

            public void SetId(Guid id)
            {
                Id = id;
            }

            public void SetTitle(string title)
            {
                Title = title;
            }
        }
    }
}