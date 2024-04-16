using CSharpFunctionalExtensions;

namespace ASSISTENTE.Application.Abstractions.Interfaces;

public interface IMaintenanceService
{
    public Task<Result> InitAsync();
    public Task<Result> ResetAsync();
}