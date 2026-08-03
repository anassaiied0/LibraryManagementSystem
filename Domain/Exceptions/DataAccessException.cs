using System;

namespace LibraryManagementSystem.Domain.Exceptions
{
    public class DataAccessException : AppException
    {
        public DataAccessException(string message) : base(message) { }
        public DataAccessException(string message, Exception inner) : base(message, inner) { }
    }
}
