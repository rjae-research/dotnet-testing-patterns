using System;

namespace Shared
{
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