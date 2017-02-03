using System;

namespace IOC
{
    public class ObjectIncompatibleException : InvalidOperationException
    {
        public ObjectIncompatibleException(string message) : base(message) { }
    }
}