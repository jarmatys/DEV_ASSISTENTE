using ASSISTENTE.Domain.Commons;
using ASSISTENTE.Domain.Entities.Resources;
using Microsoft.EntityFrameworkCore;

namespace ASSISTENTE.Persistence.MSSQL;

internal interface IAssistenteDbContext
{
    #region ENTITIES

    DbSet<Resource> Resources { get; set; } 

    #endregion
        
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    DbSet<TEntity> Set<TEntity>() where TEntity : AuditableEntity;
}