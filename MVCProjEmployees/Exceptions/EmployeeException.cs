using System;

namespace MVCProjEmployees.Exceptions
{
    public class EmployeeException : Exception
    {

        public EmployeeException()
        {

        }

        public EmployeeException(string message) : base(message)
        {

        }

        public EmployeeException(string message, Exception inner) : base(message, inner)
        {

        }

    }
}
