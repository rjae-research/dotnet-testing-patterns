using System;
using Xunit;

namespace MultipleResponsibility.Tests
{
    public class ItemTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData(null)]
        public void ConstructorMustThrowExceptionWhenNameIsNullOrWhitespace(string value)
        {
            Assert.Equal("Value is required (Parameter 'name')", Assert.Throws<ArgumentException>(() => new Item(value, 1m)).Message);
        }

        [Fact]
        public void ConstructorMustThrowExceptionWhenPriceIsNegative()
        {
            Assert.Equal("Value must be non-negative (Parameter 'price')", Assert.Throws<ArgumentException>(() => new Item("Item1", -1m)).Message);
        }

        [Fact]
        public void IdMustEqualIdArgumentWhenValueIsNotDefaultGuid()
        {
            Guid id = Guid.NewGuid();
            Assert.Equal(id, new Item("Item1", 1m, id).Id);
        }

        [Fact]
        public void IdMustNotEqualDefaultGuidWhenCalled()
        {
            Assert.NotEqual(default, new Item("Item1", 1m).Id);
        }

        [Fact]
        public void NameMustEqualNameArgumentWhenCalled()
        {
            Assert.Equal("Item1", new Item("Item1", 1m).Name);
        }

        [Fact]
        public void PriceMustEqualPriceArgumentWhenCalled()
        {
            Assert.Equal(1m, new Item("Item1", 1m).Price);
        }
    }
}