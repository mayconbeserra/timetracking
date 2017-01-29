namespace Visma.TimeTracking.AppService
{
    public abstract class ServiceBase
    {
        protected IDomainRepository Repository { get; }

        protected ServiceBase(IDomainRepository repository)
        {
            Repository = repository;
        }
    }
}