using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Shared
{
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
}