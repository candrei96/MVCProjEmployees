using System;

namespace MVCProjEmployees.Exceptions
{
    public class DepartmentException : Exception
    {

        public DepartmentException()
        {

        }

        public DepartmentException(string message) : base(message)
        {

        }

        public DepartmentException(string message, Exception inner) : base(message, inner)
        {

        }

    }
}
