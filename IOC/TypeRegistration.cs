using System;
using IOC.LifecycleResolvers;

namespace IOC
{
    public class TypeRegistration
    {
        public Type RegisteredType { get; set; }
        public IInstanceResolver Resolver { get; set; }

        public TypeRegistration(Type type, IInstanceResolver resolver)
        {
            RegisteredType = type;
            Resolver = resolver;
        }
    }
}