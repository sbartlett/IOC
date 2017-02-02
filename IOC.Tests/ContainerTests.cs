using Xunit;

namespace IOC.Tests
{
    public class ContainerTests
    {
        [Fact]
        public void can_resolve_type()
        {
            var container = new Container();
            container.Register<IDependency, Dependency>();
            container.Register<IDependency1, Dependency1>();
            container.Register<IDependency2, Dependency2>();
            container.RegisterSingleton<IDependency3, Dependency3>();

            var res = container.Resolve<IDependency>();
            res = container.Resolve<IDependency>();
        }
    }

    public interface IDependency { }
    public interface IDependency1 { }
    public interface IDependency2 { }
    public interface IDependency3 { }

    public class Dependency : IDependency
    {
        private readonly IDependency1 _dep;
        public Dependency(IDependency1 dep)
        {
            _dep = dep;
        }
    }
    public class Dependency1 : IDependency1
    {
        private readonly IDependency2 _dep2;
        private readonly IDependency3 _dep3;

        public Dependency1(IDependency2 dep2, IDependency3 dep3)
        {
            _dep2 = dep2;
            _dep3 = dep3;
        }
    }

    public class Dependency2 : IDependency2 { }
    public class Dependency3 : IDependency3
    {
        public int Number { get; set; }

        public Dependency3()
        {
            Number++;
        }
    }

}
