using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Interfaces;

public interface IFileParser
{
    public Result<IEnumerable<string>> GetNotes();
    public Result<IEnumerable<string>> GetCode();
}