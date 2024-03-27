using System.Reflection;
using ASSISTENTE.Domain.Commons;
using ASSISTENTE.Domain.Entities.Questions;
using ASSISTENTE.Domain.Entities.Resources;
using ASSISTENTE.Domain.Enums;
using ASSISTENTE.Domain.Interfaces;
using ASSISTENTE.Persistence.MSSQL.Converters;
using ASSISTENTE.Persistence.MSSQL.Seeds;
using Microsoft.EntityFrameworkCore;

namespace ASSISTENTE.Persistence.MSSQL
{
    internal class AssistenteDbContext : DbContext, IAssistenteDbContext
    {
        private readonly IUserResolver? _userResolver;
        private readonly ISystemTimeProvider? _systemTimeProvider;

        public AssistenteDbContext(DbContextOptions<AssistenteDbContext> options) : base(options)
        {
        }

        public AssistenteDbContext(
            DbContextOptions<AssistenteDbContext> options, 
            IUserResolver userResolver,
            ISystemTimeProvider systemTimeProvider) 
            : base(options)
        {
            _userResolver = userResolver ?? throw new ArgumentNullException(nameof(userResolver));
            _systemTimeProvider = systemTimeProvider ?? throw new ArgumentNullException(nameof(systemTimeProvider));
        }

        #region ENTITIES

        public DbSet<Resource> Resources { get; set; } 
        public DbSet<Question> Questions { get; set; } 

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.SeedData();

            base.OnModelCreating(modelBuilder);
        }
        
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<ResourceType>()
                .HaveConversion<ResourceTypeConverter>();
        }
        
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _userResolver!.GetUserEmail();
                        entry.Entity.Created = _systemTimeProvider!.Now();
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedBy = _userResolver!.GetUserEmail();
                        entry.Entity.Modified = _systemTimeProvider!.Now();
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Deleted;
                        break;
                    case EntityState.Detached:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Unchanged:
                        entry.State = EntityState.Unchanged;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(entry.State), entry.State, "Unknown state");
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : AuditableEntity
        {
            return base.Set<TEntity>();
        }
    }
}