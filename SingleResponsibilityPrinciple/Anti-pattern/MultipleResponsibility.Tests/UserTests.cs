using System;
using Xunit;

namespace MultipleResponsibility.Tests
{
    public class UserTests
    {
        [Fact]
        public void ConstructorMustThrowExceptionWhenEmailAddressIsNull()
        {
            Assert.Equal("Value cannot be null. (Parameter 'emailAddress')", Assert.Throws<ArgumentNullException>(() => new User("User1", "Test", null)).Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData(null)]
        public void ConstructorMustThrowExceptionWhenFirstNameIsNullOrWhitespace(string value)
        {
            Assert.Equal("Value is required (Parameter 'firstName')", Assert.Throws<ArgumentException>(() => new User(value, null, null)).Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ConstructorMustThrowExceptionWhenLastNameIsNullOrEmpty(string value)
        {
            Assert.Equal("Value is required (Parameter 'lastName')", Assert.Throws<ArgumentException>(() => new User("User1", value, null)).Message);
        }

        [Fact]
        public void EmailAddressMustEqualEmailAddressArgumentWhenCalled()
        {
            Assert.Equal("user1.test@example.com", new User("User1", "Test", "user1.test@example.com").EmailAddress);
        }

        [Fact]
        public void FirstNameMustEqualFirstNameArgumentWhenCalled()
        {
            Assert.Equal("User1", new User("User1", "Test", "user1.test@example.com").FirstName);
        }

        [Fact]
        public void IdMustEqualIdArgumentWhenValueIsNotDefaultGuid()
        {
            Guid id = Guid.NewGuid();
            Assert.Equal(id, new User("User1", "Test", "user1.test@example.com", id).Id);
        }

        [Fact]
        public void IdMustNotEqualDefaultGuidWhenCalled()
        {
            Assert.NotEqual(default, new User("User1", "Test", "user1.test@example.com").Id);
        }

        [Fact]
        public void LastNameMustEqualLastNameArgumentWhenCalled()
        {
            Assert.Equal("Test", new User("User1", "Test", "user1.test@example.com").LastName);
        }
    }

    /*
     * AntiPattern: Class contains multiple responsibilities.
     *
     * EmailAddressTests contains tests for multiple classes.
     * EmailAddressTests changes when either EmailAddress or NonEmptyString changes.
     * Difficult to identity which tests are for which class.
     */
    public class EmailAddressTests
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

        [Fact]
        public void ConstructorMustReturnEmailAddressWhenValueArgumentIsValidEmailAddress()
        {
            Assert.IsType<EmailAddress>(new EmailAddress("ValidEmailAddress@example.com"));
        }

        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData(null)]
        [InlineData("Invalid Email Address")]
        [InlineData("Invalid Email Address@example.com")]
        public void ConstructorMustThrowExceptionWhenValueArgumentIsNotValidEmailAddress(string value)
        {
            /*
             * AntiPattern: Test contains multiple responsibilities.
             *
             * Separating this test into two would separate the vectors of change.
             */
            if (string.IsNullOrWhiteSpace(value))
                Assert.Equal("Value cannot be null. (Parameter 'value')", Assert.Throws<ArgumentNullException>(() => new EmailAddress(value)).Message);
            else
                Assert.Equal("The value provided is not a valid email address.", Assert.Throws<ArgumentException>(() => new EmailAddress(value)).Message);
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
        public void EmailAddressMustImplicitlyCastToEmailAddressWhenValueArgumentIsString()
        {
            Assert.IsAssignableFrom<EmailAddress>((EmailAddress) "ValidEmailAddress@example.com");
        }

        [Fact]
        public void EmailAddressMustImplicitlyCastToStringWhenValueArgumentIsEmailAddress()
        {
            Assert.IsAssignableFrom<string>((string) new EmailAddress("ValidEmailAddress@example.com"));
        }

        [Fact]
        public void EmailAddressMustImplicitlyReturnNullWhenEmailAddressArgumentIsNull()
        {
            Assert.Null((string) (EmailAddress) null);
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