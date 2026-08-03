using System;

namespace LibraryManagementSystem.Domain.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string entityName, object key)
            : base($"{entityName} with identifier '{key}' was not found.")
        {
        }
    }
}
