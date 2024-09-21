using System.Reflection;
using ASSISTENTE.Domain.Common.Interfaces;
using ASSISTENTE.Domain.Entities.Answers;
using ASSISTENTE.Domain.Entities.QuestionCodes;
using ASSISTENTE.Domain.Entities.QuestionCodes.Enums;
using ASSISTENTE.Domain.Entities.QuestionNotes;
using ASSISTENTE.Domain.Entities.QuestionNotes.Enums;
using ASSISTENTE.Domain.Entities.Questions;
using ASSISTENTE.Domain.Entities.Questions.Enums;
using ASSISTENTE.Domain.Entities.Resources;
using ASSISTENTE.Domain.Interfaces;
using ASSISTENTE.Language.Enums;
using ASSISTENTE.Persistence.Configuration.Converters;
using ASSISTENTE.Persistence.Configuration.Extensions;
using ASSISTENTE.Persistence.Configuration.Seeds;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ASSISTENTE.Persistence.Configuration
{
    public class AssistenteDbContext : DbContext, IAssistenteDbContext
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
        public DbSet<QuestionResource> QuestionResources { get; set; } 
        public DbSet<QuestionFile> QuestionFiles { get; set; } 
        public DbSet<Answer> Answers { get; set; } 
        public DbSet<QuestionCode> QuestionCodes { get; set; }
        public DbSet<QuestionNote> QuestionNotes { get; set; }

        #endregion
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.SeedData();

            modelBuilder.ConfigureStronglyIdentifiers();

            base.OnModelCreating(modelBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.ConfigureEnum<ResourceType>();
            configurationBuilder.ConfigureEnum<QuestionContext>();
            
            configurationBuilder.ConfigureEnum<QuestionStates>();
            configurationBuilder.ConfigureEnum<QuestionCodeStates>();
            configurationBuilder.ConfigureEnum<QuestionNoteStates>();
            
            configurationBuilder
                .Properties<DateTime>()
                .HaveConversion<DateTimeConverter>();
            
            configurationBuilder.ConfigureStronglyIdentifiers();
        }
        
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
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
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

   
        public new DbSet<TEntity> Set<TEntity>() where TEntity : class, IEntity
        {
            return base.Set<TEntity>();
        }
    }
}