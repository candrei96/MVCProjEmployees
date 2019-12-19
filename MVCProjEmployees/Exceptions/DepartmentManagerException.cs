using System;

namespace OA.Web.Exceptions
{
    public class DepartmentManagerException : Exception
    {
        public DepartmentManagerException()
        {

        }

        public DepartmentManagerException(string message) : base(message)
        {

        }

        public DepartmentManagerException(string message, Exception inner) : base(message, inner)
        {

        }

    }
}
