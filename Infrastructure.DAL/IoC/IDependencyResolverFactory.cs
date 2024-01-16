namespace Infrastructure
{
    public interface IDependencyResolverFactory
    {
        IDependencyResolver CreateInstance();
    }
}