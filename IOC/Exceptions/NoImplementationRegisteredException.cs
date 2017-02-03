using System;

namespace IOC
{
    public class NoImplementationRegisteredException : InvalidOperationException
    {
        public NoImplementationRegisteredException(string message) : base(message) { }
    }
}