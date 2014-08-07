using System;

namespace MiniLisp
{
    public abstract class LispException : Exception
    {
        protected LispException(string message = null) : base(message)
        {
        }
    }
}