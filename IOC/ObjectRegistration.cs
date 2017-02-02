using System;

namespace IOC
{
    public class ObjectRegistration
    {
        public Type RegisteredType { get; set; }
        public object Instance { get; set; }

        public ObjectRegistration(Type type, object instance)
        {
            RegisteredType = type;
            Instance = instance;
        }
    }
}