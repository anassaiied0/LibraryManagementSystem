using System;
using System.Text.RegularExpressions;

namespace LibraryManagementSystem.Application.Common
{
    public static class Guard
    {
        public static void AgainstNullOrWhiteSpace(string? value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"{paramName} cannot be null or whitespace.", paramName);
        }

        public static void AgainstInvalidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.", nameof(email));

            // simple email validation
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                if (addr.Address != email)
                    throw new ArgumentException("Invalid email format.", nameof(email));
            }
            catch
            {
                throw new ArgumentException("Invalid email format.", nameof(email));
            }
        }

        public static void AgainstNonPositive(decimal value, string paramName)
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(paramName, value, $"{paramName} must be greater than zero.");
        }

        public static void AgainstFutureDate(DateTime date, string paramName)
        {
            if (date > DateTime.Now) throw new ArgumentException($"{paramName} cannot be in the future.", paramName);
        }

        public static void AgainstOutOfRange(int value, int min, int max, string paramName)
        {
            if (value < min || value > max) throw new ArgumentOutOfRangeException(paramName, value, $"{paramName} must be between {min} and {max}.");
        }
    }
}
