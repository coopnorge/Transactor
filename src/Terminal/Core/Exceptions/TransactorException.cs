using System;

namespace Terminal.Core.Exceptions
{
    /// <summary>
    /// Core application exception
    /// </summary>
    public class TransactorException : Exception
    {
        public TransactorException()
        {
        }

        public TransactorException(string message) : base(message)
        {
        }

        public TransactorException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
