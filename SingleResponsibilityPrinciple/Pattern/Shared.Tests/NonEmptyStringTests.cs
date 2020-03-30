using System;
using Xunit;

namespace Shared.Tests
{
    public class NonEmptyStringTests
    {
        [Fact]
        public void CompareToMustReturnGreaterThanZeroWhenComparedToLessThanValue()
        {
            Assert.True(new NonEmptyString("GHI").CompareTo(new NonEmptyString("DEF")) > 0);
        }

        [Fact]
        public void CompareToMustReturnGreaterThanZeroWhenComparedToLessThanValueAndCaseSensitivityIsFalse()
        {
            Assert.True(new NonEmptyString("GHI", true, false).CompareTo(new NonEmptyString("DEF", true, false)) > 0);
        }

        [Fact]
        public void CompareToMustReturnLessThanZeroWhenComparedToGreaterThanValue()
        {
            Assert.True(new NonEmptyString("ABC").CompareTo(new NonEmptyString("DEF")) < 0);
        }

        [Fact]
        public void CompareToMustReturnNegativeOneWhenNotComparedToStringBase()
        {
            Assert.Equal(-1, new NonEmptyString("test").CompareTo(new object()));
        }

        [Fact]
        public void ConstructorMustNotTrimValueWhenShouldTrimIsFalse()
        {
            Assert.Equal("12345 ", new NonEmptyString("12345 ", false).Value);
        }

        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData(null)]
        public void ConstructorMustThrowExceptionWhenValueIsNullOrWhitespace(string value)
        {
            Assert.Throws<ArgumentNullException>(() => new NonEmptyString(value));
        }

        [Fact]
        public void ConstructorMustTrimValueWhenShouldTrimIsTrue()
        {
            Assert.Equal("12345", new NonEmptyString("12345 ").Value);
        }

        [Fact]
        public void EqualsMustReturnFalseWhenValueIsNotStringBase()
        {
            Assert.False(new NonEmptyString("test").Equals(new object()));
        }

        [Fact]
        public void EqualsMustReturnFalseWhenValueIsNull()
        {
            Assert.False(new NonEmptyString("test").Equals(null));
        }

        [Fact]
        public void EqualsMustReturnFalseWhenValuesAreNotEqual()
        {
            Assert.NotEqual(new NonEmptyString("test"), new NonEmptyString("not test"));
        }

        [Fact]
        public void EqualsMustReturnTrueWhenValuesAreEqual()
        {
            Assert.True(new NonEmptyString("test").Equals(new NonEmptyString("test")));
        }

        [Fact]
        public void EqualsMustReturnTrueWhenValuesAreEqualCaseInsensitive()
        {
            Assert.True(new NonEmptyString("Test", true, false).Equals(new NonEmptyString("test", true, false)));
        }

        [Fact]
        public void GetHashCodeMustReturnSameValueWhenValuesAreEqual()
        {
            Assert.Equal(new NonEmptyString("test").GetHashCode(), new NonEmptyString("test").GetHashCode());
        }

        [Fact]
        public void GetHashCodeMustReturnSameValueWhenValuesAreEqualCaseInsensitive()
        {
            Assert.Equal(new NonEmptyString("test", true, false).GetHashCode(), new NonEmptyString("test", true, false).GetHashCode());
        }

        [Fact]
        public void ImplicitOperatorMustReturnNonEmptyStringWhenStringCastToNonEmptyString()
        {
            Assert.IsType<NonEmptyString>((NonEmptyString) "test");
        }

        [Fact]
        public void ImplicitOperatorMustReturnNullWhenNonEmptyStringIsNull()
        {
            Assert.Null((string) (NonEmptyString) null);
        }

        [Fact]
        public void ImplicitOperatorMustReturnNullWhenValueIsNull()
        {
            Assert.Null((string) (StringBase) null);
        }

        [Fact]
        public void ImplicitOperatorMustReturnStringWhenNonEmptyStringCastToString()
        {
            Assert.IsType<string>((string) new NonEmptyString("test"));
        }

        [Fact]
        public void ImplicitOperatorMustReturnValueWhenValueIsNotNull()
        {
            Assert.Equal("test", (StringBase) new NonEmptyString("test"));
        }
    }
}