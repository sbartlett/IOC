namespace IOC.LifecycleResolvers
{
    public interface IInstanceResolver 
    {
        object ResolveInstance(object[] args);
    }
}