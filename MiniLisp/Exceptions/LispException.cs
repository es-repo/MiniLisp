using System;

namespace MiniLisp.Exceptions
{
    public abstract class LispException : Exception
    {
        protected LispException(string message = null) : base(message)
        {
        }
    }
}