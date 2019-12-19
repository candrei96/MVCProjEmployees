using System;

namespace OA.Web.Exceptions
{
    public class DepartmentEmployeeException : Exception
    {
        public DepartmentEmployeeException()
        {

        }

        public DepartmentEmployeeException(string message) : base(message)
        {

        }

        public DepartmentEmployeeException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
