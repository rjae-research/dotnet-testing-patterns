using System;

namespace MultipleResponsibility
{
    public class Item
    {
        /*
         * AntiPattern: Class contains multiple responsibilities.
         *
         * Validation of non-empty name is not responsibility of Item class.
         * Validation of non-negative price is not responsibility of Item class.
         */
        public Item(string name, decimal price, Guid id = default)
        {
            Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Value is required", $"{nameof(name)}") : name;
            Price = price < 0m ? throw new ArgumentException("Value must be non-negative", $"{nameof(price)}") : price;
            Id = id == default ? Guid.NewGuid() : id;
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public decimal Price { get; private set; }
    }
}