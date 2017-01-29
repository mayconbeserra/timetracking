namespace Visma.TimeTracking.EventSourcing
{
    public interface IOriginator
    {
        IMemento CreateMemento();

        void SetMemento(IMemento memento);
    }
}