using System;
using Xunit;

namespace UseCustomMocks.Tests
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
            Book book = new Book {Title = "Goodnight Moon", Author = "Margaret Wise Brown", Id = id};
            Assert.Equal("Goodnight Moon", book.Title);
            Assert.Equal("Margaret Wise Brown", book.Author);
            Assert.Equal(id, book.Id);
        }
    }
}