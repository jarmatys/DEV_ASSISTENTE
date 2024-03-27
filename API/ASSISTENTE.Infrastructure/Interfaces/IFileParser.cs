using ASSISTENTE.Infrastructure.ValueObjects;

namespace ASSISTENTE.Infrastructure.Interfaces;

public interface IFileParser
{
    public Result<IEnumerable<ResourceText>> GetNotes();
    public Result<IEnumerable<ResourceText>> GetCode();
}