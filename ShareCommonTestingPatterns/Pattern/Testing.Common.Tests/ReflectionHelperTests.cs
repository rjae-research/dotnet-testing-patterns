using System;
using Xunit;

namespace Testing.Common.Tests
{
    public class ReflectionHelperTests
    {
        [Fact]
        public void GetFieldMustReturnFieldValueWhenFieldExists()
        {
            Assert.Equal("Goodnight Moon", ReflectionHelper.GetField<string>(new Book("Goodnight Moon"), "itsName"));
        }

        [Fact]
        public void GetFieldMustReturnFieldValueWhenFieldExistsForSpecifiedType()
        {
            Assert.Equal("Goodnight Moon", ReflectionHelper.GetField<string>(typeof(Book), new Book("Goodnight Moon"), "itsName"));
        }

        [Fact]
        public void GetFieldMustThrowExceptionWhenFieldDoesNotExist()
        {
            Assert.StartsWith("Can't find field 'itsTitle'", Assert.Throws<Exception>(() => ReflectionHelper.GetField<string>(new Book("Goodnight Moon"), "itsTitle")).Message);
        }

        [Fact]
        public void GetFieldMustThrowExceptionWhenFieldDoesNotExistForSpecifiedType()
        {
            Assert.StartsWith("Can't find field 'itsTitle'", Assert.Throws<Exception>(() => ReflectionHelper.GetField<string>(typeof(Book), new Book("Goodnight Moon"), "itsTitle")).Message);
        }

        [Fact]
        public void GetPropertyMustReturnPropertyValueWhenPropertyExists()
        {
            Assert.Equal("Goodnight Moon", ReflectionHelper.GetProperty<string>(new Book("Goodnight Moon"), "Title"));
        }

        [Fact]
        public void GetPropertyMustThrowExceptionWhenPropertyDoesNotExist()
        {
            Assert.StartsWith("Can't find property 'Name'", Assert.Throws<Exception>(() => ReflectionHelper.GetProperty<string>(new Book("Goodnight Moon"), "Name")).Message);
        }

        [Fact]
        public void InvokeConstructorMustReturnInstanceOfSpecifiedType()
        {
            Assert.Equal("Goodnight Moon", ReflectionHelper.InvokeConstructor<Book>("Goodnight Moon").Title);
        }

        [Fact]
        public void InvokeConstructorMustReturnInstanceOfSpecifiedTypeAndArguments()
        {
            Assert.Equal("Goodnight Moon", ((Book) ReflectionHelper.InvokeConstructor(typeof(Book), new[] {typeof(string)}, new[] {"Goodnight Moon"})).Title);
        }

        [Fact]
        public void InvokeMethodMustInvokeMethod()
        {
            Book book = new Book("Goodnight Moon");
            Assert.Equal(0, book.Bookmark);
            ReflectionHelper.InvokeMethod(book, "Read", 1);
            Assert.Equal(1, book.Bookmark);
        }

        [Fact]
        public void SetFieldMustSetFieldWhenFieldExists()
        {
            Book book = new Book("Goodnight Moon");
            ReflectionHelper.SetField(book, "itsName", "Hello Stars");
            Assert.Equal("Hello Stars", ReflectionHelper.GetField<string>(book, "itsName"));
        }

        [Fact]
        public void SetFieldMustSetFieldWhenFieldExistsForSpecifiedType()
        {
            Book book = new Book("Goodnight Moon");
            ReflectionHelper.SetField(typeof(Book), book, "itsName", "Hello Stars");
            Assert.Equal("Hello Stars", ReflectionHelper.GetField<string>(book, "itsName"));
        }

        [Fact]
        public void SetFieldMustThrowExceptionWhenFieldDoesNotExist()
        {
            Assert.StartsWith("Can't find field 'itsTitle'", Assert.Throws<Exception>(() => ReflectionHelper.SetField(new Book("Goodnight Moon"), "itsTitle", "Hello Stars")).Message);
        }

        [Fact]
        public void SetFieldMustThrowExceptionWhenFieldDoesNotExistForSpecifiedType()
        {
            Assert.StartsWith("Can't find field 'itsTitle'", Assert.Throws<Exception>(() => ReflectionHelper.SetField(typeof(Book), new Book("Goodnight Moon"), "itsTitle", "Hello Stars")).Message);
        }

        [Fact]
        public void SetPropertyMustSetProperty()
        {
            Book book = new Book("Goodnight Moon");
            Assert.Equal(0, book.Bookmark);
            ReflectionHelper.SetProperty(book, "Bookmark", 1);
            Assert.Equal(1, book.Bookmark);
        }

        private class Book
        {
            private string itsName;

            public Book(string name)
            {
                itsName = name;
            }

            public int Bookmark { get; private set; }

            public string Title => itsName;

            private void Read(int pages)
            {
                Bookmark += pages;
            }
        }
    }
}