using System;
using System.Collections.Generic;

namespace MultipleResponsibility
{
    public class Order
    {
        public Order(User user, List<Item> items = null, Guid id = default)
        {
            User = user ?? throw new ArgumentNullException($"{nameof(user)}");
            Items = items ?? new List<Item>();
            Id = id == default ? Guid.NewGuid() : id;
        }

        public Guid Id { get; private set; }

        public List<Item> Items { get; private set; }

        public User User { get; private set; }
    }
}