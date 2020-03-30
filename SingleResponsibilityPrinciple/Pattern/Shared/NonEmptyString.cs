using System;

namespace Shared
{
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
}