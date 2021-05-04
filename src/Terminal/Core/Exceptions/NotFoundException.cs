using System;

namespace Terminal.Core.Exceptions
{
    /// <summary>
    /// Exception if requested data not found
    /// </summary>
    public class NotFoundException : TransactorException
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
