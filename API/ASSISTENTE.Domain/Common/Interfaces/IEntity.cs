namespace ASSISTENTE.Domain.Common.Interfaces;

public interface IEntity
{
    IReadOnlyList<IDomainEvents> GetEvents();
    void ClearEvents();
}