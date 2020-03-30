using System;

namespace UseMockingFrameworks
{
    public class Book
    {
        public Book( /* EF */)
        {
        }

        public Book(string title, string author, Guid id = default)
        {
            Title = title;
            Author = author;
            Id = id == default ? Guid.NewGuid() : id;
        }

        public string Author { get; protected set; }

        public Guid Id { get; protected set; }

        public string Title { get; protected set; }
    }
}