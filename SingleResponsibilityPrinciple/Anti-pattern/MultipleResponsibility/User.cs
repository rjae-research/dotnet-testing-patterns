/*
 * AntiPattern: File contains multiple responsibilities (classes).
 *
 * File changes even when User class does not.
 * Makes finding non-User classes time consuming.
 * Encourages coupling of classes.
 */

using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace MultipleResponsibility
{
    /*
     * AntiPattern: Class contains multiple responsibilities.
     *
     * Validation of non-empty strings is not responsibility of User class.
     */
    public class User
    {
        public User(string firstName, string lastName, EmailAddress emailAddress, Guid id = default)
        {
            FirstName = string.IsNullOrWhiteSpace(firstName) ? throw new ArgumentException("Value is required", $"{nameof(firstName)}") : firstName;
            /*
             * Bug: LastName not also fixed when FirstName fixed to validate non-whitespace.
             *
             * Refactoring name validation to another class would centralize the behavior and drastically reduce risk of bugs.
             */
            LastName = string.IsNullOrEmpty(lastName) ? throw new ArgumentException("Value is required", $"{nameof(lastName)}") : lastName;
            EmailAddress = emailAddress ?? throw new ArgumentNullException($"{nameof(emailAddress)}");
            Id = id == default ? Guid.NewGuid() : id;
        }

        public string EmailAddress { get; private set; }

        public string FirstName { get; private set; }

        public Guid Id { get; private set; }

        public string LastName { get; private set; }
    }

    public class EmailAddress : NonEmptyString
    {
        public EmailAddress(string value) : base(value, true, false)
        {
            try
            {
                if (Regex.IsMatch(value, @"\s"))
                    throw new ArgumentException("The value provided is not a valid email address.");
                new MailAddress(value);
            }
            catch
            {
                throw new ArgumentException("The value provided is not a valid email address.");
            }
        }

        public static implicit operator EmailAddress(string value)
        {
            return new EmailAddress(value);
        }

        public static implicit operator string(EmailAddress value)
        {
            return value?.Value;
        }
    }

    public class NonEmptyString : StringBase
    {
        public NonEmptyString(string value, bool shouldTrim = true, bool isCaseSensitive = true) : base(value, shouldTrim, isCaseSensitive)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));
        }

        public static implicit operator NonEmptyString(string value)
        {
            return new NonEmptyString(value);
        }

        public static implicit operator string(NonEmptyString value)
        {
            return value?.Value;
        }
    }

    public abstract class StringBase : IComparable<StringBase>, IComparable, IEquatable<StringBase>
    {
        protected StringBase(string value, bool shouldTrim = true, bool isCaseSensitive = true)
        {
            Value = shouldTrim ? value?.Trim() : value;
            IsCaseSensitive = isCaseSensitive;
        }

        public virtual int CompareTo(object other)
        {
            return CompareTo(other as StringBase);
        }

        public virtual int CompareTo(StringBase other)
        {
            return other == null ? -1 : string.Compare(Value, other.Value, !IsCaseSensitive && !other.IsCaseSensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
        }

        public virtual bool Equals(StringBase other)
        {
            return other != null && string.Equals(Value, other.Value, !IsCaseSensitive && !other.IsCaseSensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
        }

        public override bool Equals(object other)
        {
            return Equals(other as StringBase);
        }

        public override int GetHashCode()
        {
            return IsCaseSensitive ? Value.GetHashCode() : Value.ToLowerInvariant().GetHashCode();
        }

        public static implicit operator string(StringBase value)
        {
            return value?.ToString();
        }

        public override string ToString()
        {
            return Value;
        }

        public virtual string Value { get; }

        protected virtual bool IsCaseSensitive { get; }
    }
}