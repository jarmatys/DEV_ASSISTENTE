using ASSISTENTE.Infrastructure.ValueObjects;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Interfaces;

public interface IFileParser
{
    public Result<IEnumerable<ResourceText>> GetNotes();
    public Result<IEnumerable<ResourceText>> GetCode();
}