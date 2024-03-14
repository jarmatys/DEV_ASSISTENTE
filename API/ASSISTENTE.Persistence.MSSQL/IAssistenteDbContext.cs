using ASSISTENTE.Domain.Commons;
using Microsoft.EntityFrameworkCore;

namespace ASSISTENTE.Persistence.MSSQL;

public interface IAssistenteDbContext
{
    #region ENTITIES

    DbSet<Article> Articles { get; set; }

    #endregion
        
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    DbSet<TEntity> Set<TEntity>() where TEntity : AuditableEntity;
}