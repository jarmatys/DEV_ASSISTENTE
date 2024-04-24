namespace ASSISTENTE.Domain.Commons.Interfaces;

public interface IEntity
{
    IReadOnlyList<IDomainEvents> GetEvents();
    void ClearEvents();
}