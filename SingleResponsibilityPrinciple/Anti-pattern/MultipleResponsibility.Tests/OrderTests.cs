using System;
using System.Collections.Generic;
using Xunit;

namespace MultipleResponsibility.Tests
{
    public class OrderTests
    {
        [Fact]
        public void ConstructorMustThrowExceptionWhenUserArgumentIsNull()
        {
            Assert.Equal("Value cannot be null. (Parameter 'user')", Assert.Throws<ArgumentNullException>(() => new Order(null)).Message);
        }

        [Fact]
        public void IdMustEqualIdArgumentWhenValueIsNotDefaultGuid()
        {
            Guid id = Guid.NewGuid();
            User user = new User("User1", "Test", "user1.test@example.com");
            List<Item> items = new List<Item>(new[] {new Item("Item1", 1m)});
            Assert.Equal(id, new Order(user, items, id).Id);
        }

        [Fact]
        public void IdMustNotEqualDefaultGuidWhenCalled()
        {
            User user = new User("User1", "Test", "user1.test@example.com");
            List<Item> items = new List<Item>(new[] {new Item("Item1", 1m)});
            Assert.NotEqual(default, new Order(user, items).Id);
        }

        [Fact]
        public void ItemsMustEqualEmptyListWhenItemsArgumentIsNull()
        {
            User user = new User("User1", "Test", "user1.test@example.com");
            Assert.Empty(new Order(user).Items);
        }

        [Fact]
        public void ItemsMustEqualItemsArgumentWhenItemsArgumentIsNotNull()
        {
            User user = new User("User1", "Test", "user1.test@example.com");
            Item item = new Item("Item1", 1m);
            Assert.Contains(new Order(user, new List<Item>(new[] {item})).Items, x => x.Id == item.Id);
        }

        [Fact]
        public void UserMustEqualUserArgumentWhenUserArgumentIsNotNull()
        {
            User user = new User("User1", "Test", "user1.test@example.com");
            Assert.Equal(user.Id, new Order(user).User.Id);
        }
    }
}