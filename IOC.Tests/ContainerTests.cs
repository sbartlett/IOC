using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Shouldly;

namespace IOC.Tests
{
    public class ContainerTests
    {
        [Fact]
        public void can_resolve_transient_type_without_generic()
        {
            var container = new Container();
            container.Register(typeof(Dependency2));

            var obj = container.Resolve<Dependency2>();
            var obj2 = container.Resolve<Dependency2>();

            obj.ShouldNotBeSameAs(obj2);
        }

        [Fact]
        public void can_resolve_transient_type()
        {
            var container = new Container();
            container.Register<IDependency2, Dependency2>();

            var obj = container.Resolve<IDependency2>();
            var obj2 = container.Resolve<IDependency2>();

            obj.ShouldNotBeSameAs(obj2);
        }

        [Fact]
        public void can_resolve_singleton_type()
        {
            var container = new Container();
            container.RegisterSingleton<IDependency3, Dependency3>();
            var obj = container.Resolve<IDependency3>();
            var obj2 = container.Resolve<IDependency3>();

            obj.ShouldBeSameAs(obj2);
        }

        [Fact]
        public void can_resolve_multiple_of_type()
        {
            var container = new Container();
            container.Register<IDependency2, Dependency2>();
            container.Register<IDependency2, Dependency2Again>();

            var res = container.ResolveAll<IDependency2>();
            res.Count().ShouldBe(2);
        }

        [Fact]
        public void can_resolve_all_nested_constructor_arguments()
        {
            var container = new Container();
            container.Register<IServiceWithDependencies, ServiceWithDependencies>();
            container.Register<IDependencyWithDependencies, DependencyWithDependencies>();
            container.Register<IDependency2, Dependency2>();
            container.Register<IDependency3, Dependency3>();

            var res = container.Resolve<IServiceWithDependencies>();
            res.DependencyObject.ShouldNotBeNull();
            res.DependencyObject2.ShouldNotBeNull();
            res.DependencyObject2.DepencyObject.ShouldNotBeNull();
        }

        [Fact]
        public void can_resolve_lists_in_constructor_arguments()
        {
            var container = new Container();
            container.Register<IDependency1, Dependency1>();
            container.Register<IDependency2, Dependency2>();
            container.Register<IDependency2, Dependency2Again>();
            container.Register<IDependency2, Dependency2AgainAgain>();
            container.Register<IDependency3, Dependency3>();

            var res = container.Resolve<IDependency1>();
            res.DependencyObject.ShouldNotBeNull();
            res.DependencyObject.Count().ShouldBe(3);
            res.DependencyObject2.ShouldNotBeNull();
        }

        [Fact]
        public void can_resolve_provided_implementation()
        {
            var container = new Container();
            var glove = new Glove
            {
                Fingers = 1,
                IsMitten = true
            };

            container.RegisterInstance(glove);
            var res = container.Resolve<Glove>();
            res.ShouldBeSameAs(glove);
        }

        [Fact]
        public void throws_NoImplementationRegisteredException_when_no_registration()
        {
            var container = new Container();
            Should.Throw<NoImplementationRegisteredException>(() => container.Resolve<IDependency1>());
        }

        [Fact]
        public void throws_ContainerClosedException_when_registration_after_container_accessed()
        {
            var container = new Container();
            container.Register<IDependency3, Dependency3>();
            container.Resolve<IDependency3>();

            Should.Throw<ContainerClosedException>(() => container.Register<IDependency2, Dependency2>());
        }

        [Fact]
        public void throws_InvalidCastException_when_registration_after_container_accessed()
        {
            var container = new Container();
            container.Register<IDependency3, Dependency2>();

            Should.Throw<InvalidCastException>(() => container.Resolve<IDependency3>());
        }
    }

    public interface IServiceWithDependencies
    {
        IDependency3 DependencyObject { get; set; }
        IDependencyWithDependencies DependencyObject2 { get; set; }
    }
    public interface IDependency1
    {
        IEnumerable<IDependency2> DependencyObject { get; set; }
        IDependency3 DependencyObject2 { get; set; }
    }
    public interface IDependency2 { }
    public interface IDependency3 { }
    public interface IDependencyWithDependencies
    {
        IDependency2 DepencyObject { get; set; }
    }

    public class ServiceWithDependencies : IServiceWithDependencies
    {
        public IDependency3 DependencyObject { get; set; }
        public IDependencyWithDependencies DependencyObject2 { get; set; }

        public ServiceWithDependencies(IDependency3 dep3, IDependencyWithDependencies dep4)
        {
            DependencyObject = dep3;
            DependencyObject2 = dep4;
        }
    }

    public class DependencyWithDependencies : IDependencyWithDependencies
    {
        public IDependency2 DepencyObject { get; set; }

        public DependencyWithDependencies(IDependency2 dep)
        {
            DepencyObject = dep;
        }
    }
    public class Dependency1 : IDependency1
    {
        public IEnumerable<IDependency2> DependencyObject { get; set; }
        public IDependency3 DependencyObject2 { get; set; }

        public Dependency1(IEnumerable<IDependency2> dep2, IDependency3 dep3)
        {
            DependencyObject = dep2;
            DependencyObject2 = dep3;
        }
    }
    public class Dependency2 : IDependency2 { }
    public class Dependency2Again : IDependency2 { }
    public class Dependency2AgainAgain : IDependency2 { }
    public class Dependency3 : IDependency3
    {
        public int Number { get; set; }

        public Dependency3()
        {
            Number++;
        }
    }

    public class Glove
    {
        public int Fingers { get; set; }
        public bool IsMitten { get; set; }
    }
}
