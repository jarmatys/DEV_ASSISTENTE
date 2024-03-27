namespace ASSISTENTE.Infrastructure.Interfaces;

public interface IMaintenanceService
{
    public Task<Result> InitAsync();
    public Task<Result> ResetAsync();
}