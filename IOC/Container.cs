using System;
using System.Collections.Generic;
using System.Linq;
using IOC.InstanceResolvers;
using IOC.LifecycleResolvers;

namespace IOC
{
    public class Container
    {
        private readonly List<TypeRegistration> _typeRegistrations = new List<TypeRegistration>();
        private readonly List<ObjectRegistration> _objectRegistrations = new List<ObjectRegistration>();

        public void Register<TType, UImplementation>()  where UImplementation : class
        {
            var resolver = new TransientInstanceResolver<UImplementation>();
            _typeRegistrations.Add(new TypeRegistration(typeof(TType), resolver));
        }

        public void RegisterSingleton<TType, UImplementation>()  where UImplementation : class
        {
            var resolver = new SingletonInstanceResolver<UImplementation>();
            _typeRegistrations.Add(new TypeRegistration(typeof(TType), resolver));
        }

        public void RegisterInstance(object instance)
        {
            _objectRegistrations.Add(new ObjectRegistration(instance.GetType(), instance));
        }

        public T Resolve<T>()
        {
            var type = typeof(T);
            return (T)resolveType(type);
        }

        private object resolveType(Type type)
        {
            var registration = _typeRegistrations.FirstOrDefault();
            if (registration == null)
            {
                throw new NoImplementationRegisteredException($"{type} is not registered in the container. Please call a RegisterXXX method for the requested type.");
            }

            var resolver = registration.Resolver;
            var typeToBuild = resolver.GetType().GetGenericArguments()[0];
            var arguments = buildConstructorParameters(typeToBuild);
            return resolver.ResolveInstance(arguments);
        }

        private object[] buildConstructorParameters(Type typeToBuild)
        {
            var argumentList = typeToBuild
                .GetConstructors()
                .Select(x => x.GetParameters())
                .OrderBy(x => x.Length)
                .First();

            return argumentList.Select(x => resolveType(x.ParameterType)).ToArray();
        }
    }
}