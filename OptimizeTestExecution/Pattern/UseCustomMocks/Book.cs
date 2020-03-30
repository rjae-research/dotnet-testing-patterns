using System;

namespace UseCustomMocks
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

        public string Author { get; set; }

        public Guid Id { get; set; }

        public string Title { get; set; }
    }
}