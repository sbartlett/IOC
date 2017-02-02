using System;

namespace IOC
{
    public class NoImplementationRegisteredException : Exception
    {
        public NoImplementationRegisteredException(string message) : base(message) { }
    }
}