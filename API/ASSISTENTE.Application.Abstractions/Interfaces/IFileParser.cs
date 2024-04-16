using ASSISTENTE.Application.Abstractions.ValueObjects;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Application.Abstractions.Interfaces;

public interface IFileParser
{
    public Result<IEnumerable<ResourceText>> GetNotes();
    public Result<IEnumerable<ResourceText>> GetCode();
}