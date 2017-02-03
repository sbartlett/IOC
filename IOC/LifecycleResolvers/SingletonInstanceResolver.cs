using System;
using IOC.LifecycleResolvers;

namespace IOC.InstanceResolvers
{
    public class SingletonInstanceResolver<T> : IInstanceResolver where T : class 
    {
        private T _instance;

        public object ResolveInstance(object[] args)
        {
            if(_instance == null)
            {
                var type = typeof(T);
                _instance = (T)Activator.CreateInstance(type, args);
            }

            return _instance;
        }

        public void SetInstance(T instance)
        {
            _instance = instance;
        }
    }
}