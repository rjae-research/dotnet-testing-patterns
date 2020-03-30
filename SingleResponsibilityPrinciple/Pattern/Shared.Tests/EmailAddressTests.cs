using System;
using Xunit;

namespace Shared.Tests
{
    public class EmailAddressTests
    {
        [Fact]
        public void ConstructorMustReturnEmailAddressWhenValueIsValidEmailAddress()
        {
            Assert.IsType<EmailAddress>(new EmailAddress("ValidEmailAddress@example.com"));
        }

        [Theory]
        [InlineData("Invalid Email Address")]
        [InlineData("Invalid Email Address@example.com")]
        public void ConstructorMustThrowExceptionWhenValueArgumentIsNotValidEmailAddress(string value)
        {
            Assert.Equal("The value provided is not a valid email address.", Assert.Throws<ArgumentException>(() => new EmailAddress(value)).Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData(null)]
        public void ConstructorMustThrowExceptionWhenValueArgumentIsNullOrWhitespace(string value)
        {
            Assert.Equal("Value cannot be null. (Parameter 'value')", Assert.Throws<ArgumentNullException>(() => new EmailAddress(value)).Message);
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
    }
}