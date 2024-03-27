using ASSISTENTE.Domain.Commons;
using ASSISTENTE.Domain.Entities.Questions;
using ASSISTENTE.Domain.Entities.Resources;
using Microsoft.EntityFrameworkCore;

namespace ASSISTENTE.Persistence.MSSQL;

internal interface IAssistenteDbContext
{
    #region ENTITIES

    DbSet<Resource> Resources { get; set; } 
    DbSet<Question> Questions { get; set; } 

    #endregion
        
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    DbSet<TEntity> Set<TEntity>() where TEntity : AuditableEntity;
}