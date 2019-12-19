using System;

namespace MVCProjEmployees.Exceptions
{
    public class SalaryException : Exception
    {

        public SalaryException()
        {

        }

        public SalaryException(string message) : base(message)
        {

        }

        public SalaryException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
