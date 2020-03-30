using System;
using Shared;

namespace Identity
{
    public class User
    {
        public User(NonEmptyString firstName, NonEmptyString lastName, EmailAddress emailAddress, Guid id = default)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            EmailAddress = emailAddress ?? throw new ArgumentNullException(nameof(emailAddress));
            Id = id == default ? Guid.NewGuid() : id;
        }

        public string EmailAddress { get; private set; }

        public string FirstName { get; private set; }

        public Guid Id { get; private set; }

        public string LastName { get; private set; }
    }
}