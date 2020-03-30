using System;
using Xunit;

namespace Identity.Tests
{
    public class UserTests
    {
        [Fact]
        public void ConstructorMustThrowExceptionWhenEmailAddressIsNull()
        {
            Assert.Equal("Value cannot be null. (Parameter 'emailAddress')", Assert.Throws<ArgumentNullException>(() => new User("User1", "Test", null)).Message);
        }

        [Fact]
        public void ConstructorMustThrowExceptionWhenFirstNameIsNull()
        {
            Assert.Equal("Value cannot be null. (Parameter 'firstName')", Assert.Throws<ArgumentNullException>(() => new User(null, null, null)).Message);
        }

        [Fact]
        public void ConstructorMustThrowExceptionWhenLastNameIsNull()
        {
            Assert.Equal("Value cannot be null. (Parameter 'lastName')", Assert.Throws<ArgumentNullException>(() => new User("User1", null, null)).Message);
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
}