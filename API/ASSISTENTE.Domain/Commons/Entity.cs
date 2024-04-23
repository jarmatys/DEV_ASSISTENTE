using ASSISTENTE.Domain.Commons.Interfaces;

namespace ASSISTENTE.Domain.Commons;

public abstract class Entity : IEntity
{
    private readonly List<IDomainEvents> _domainEvents = [];

    public IReadOnlyList<IDomainEvents> GetEvents()
    {
        return _domainEvents.ToList();
    }

    public void ClearEvents()
    {
        _domainEvents.Clear();
    }

    internal void RaiseEvent(IDomainEvents domainEvents)
    {
        _domainEvents.Add(domainEvents);
    }
}