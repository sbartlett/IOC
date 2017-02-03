using System;

namespace IOC
{
    public class ContainerClosedException : InvalidOperationException
    {
        public ContainerClosedException(string message) : base(message) { }
    }


}