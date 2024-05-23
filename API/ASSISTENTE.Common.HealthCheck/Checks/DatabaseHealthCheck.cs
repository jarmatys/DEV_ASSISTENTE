using ASSISTENTE.Persistence.Configuration;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace ASSISTENTE.Common.HealthCheck.Checks;

internal class DatabaseHealthCheck(IAssistenteDbContext dbContext) : CheckBase("Database")
{
    protected override async Task<Result> Check()
    {
        await dbContext.Resources.OrderBy(r => r.Created).FirstOrDefaultAsync();
    
        return Result.Success();
    }
}