using ASSISTENTE.Common.HealthCheck.Core;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace ASSISTENTE.Persistence.Configuration.HealthChecks;

internal class DatabaseHealthCheck(IAssistenteDbContext dbContext) : CheckBase
{
    protected override async Task<Result> Check()
    {
        await dbContext.Resources.OrderBy(r => r.Created).FirstOrDefaultAsync();
    
        return Result.Success();
    }
}