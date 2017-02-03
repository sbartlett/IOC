using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IOC.InstanceResolvers;
using IOC.LifecycleResolvers;

namespace IOC
{
    public class Container
    {
        private bool _containerClosed;
        private readonly List<TypeRegistration> _typeRegistrations = new List<TypeRegistration>();

        public void Register<TType, UImplementation>()  where UImplementation : class
        {
            var resolver = new TransientInstanceResolver<UImplementation>();
            registerType(typeof(TType), typeof(UImplementation), resolver);
        }

        public void RegisterSingleton<TType, UImplementation>() where UImplementation : class
        {
            var resolver = new SingletonInstanceResolver<UImplementation>();
            registerType(typeof(TType), typeof(UImplementation), resolver);
        }

        public void RegisterInstance<TType>(TType instance) where TType : class
        {
            var type = typeof(TType);
            var resolver = new SingletonInstanceResolver<TType>();
            resolver.SetInstance(instance);
            registerType(type, type, resolver);
        }

        public T Resolve<T>()
        {
            var type = typeof(T);
            return (T)Resolve(type);
        }

        public object Resolve(Type type)
        {
            _containerClosed = true;

            return resolveType(type);
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            _containerClosed = true;

            var type = typeof(T);
            return resolveAllTypes(type).Select(x => (T)x);
        }

        private void registerType(Type type, Type implemnationType, IInstanceResolver resolver)
        {
            if (_containerClosed)
            {
                throw new ContainerClosedException("Container has been accessed! Please register all types before using container.");
            }

            if (implemnationType.IsAssignableFrom(type) && !(type == implemnationType))
            {
                throw new ObjectIncompatibleException($"{type} cannot be cast to {implemnationType}. This is an invalid registration.");
            }

            _typeRegistrations.Add(new TypeRegistration(type, resolver));
        }

        private IEnumerable<object> resolveAllTypes(Type type)
        {
            var allTypes = _typeRegistrations
                .Where(x => x.RegisteredType == type);
            if (!allTypes.Any())
            {
                throw new NoImplementationRegisteredException($"No registrations for {type} are present in the container. Please call a RegisterXXX method for the requested type.");
            }

            var objects = allTypes.Select(x => createObject(x.Resolver));

            return objects;
            
        }

        private object resolveType(Type type)
        {
            var registration = _typeRegistrations.FirstOrDefault(x => x.RegisteredType == type);
            if (registration == null)
            {
                throw new NoImplementationRegisteredException($"{type} is not registered in the container. Please call a RegisterXXX method for the requested type.");
            }

            return createObject(registration.Resolver);
        }

        private object createObject(IInstanceResolver resolver)
        {
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

            return argumentList.Select(x =>
            {
                var param = x.ParameterType;
                if (typeof(IEnumerable).IsAssignableFrom(param) && param.IsGenericType)
                {
                    var gen = param.GetGenericArguments()[0];
                    var items = resolveAllTypes(gen).ToList();
                    var constructedListType = typeof(List<>).MakeGenericType(gen);
                    var instance = (IList)Activator.CreateInstance(constructedListType);
                    foreach (var i in items)
                    {
                        instance.Add(i);
                    }

                    return instance;
                }

                return resolveType(param);
            }).ToArray();
        }
    }
}