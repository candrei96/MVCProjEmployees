using System;

namespace MVCProjEmployees.Exceptions
{
    public class TitleException : Exception
    {

        public TitleException()
        {

        }

        public TitleException(string message) : base(message)
        {

        }

        public TitleException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
