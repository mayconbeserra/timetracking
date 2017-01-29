namespace Visma.TimeTracking.EventSourcing
{
    public interface IMemento
    {
        string Id { get; }
        long Version { get; }
    }
}