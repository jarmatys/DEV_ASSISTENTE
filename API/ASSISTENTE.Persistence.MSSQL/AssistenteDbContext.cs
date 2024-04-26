using System.Reflection;
using ASSISTENTE.Domain.Commons.Interfaces;
using ASSISTENTE.Domain.Entities.Answers;
using ASSISTENTE.Domain.Entities.Questions;
using ASSISTENTE.Domain.Entities.Questions.Enums;
using ASSISTENTE.Domain.Entities.Resources;
using ASSISTENTE.Domain.Entities.Resources.Enums;
using ASSISTENTE.Domain.Interfaces;
using ASSISTENTE.Language.Identifiers;
using ASSISTENTE.Persistence.MSSQL.Converters;
using ASSISTENTE.Persistence.MSSQL.Converters.Identifiers;
using ASSISTENTE.Persistence.MSSQL.Extensions;
using ASSISTENTE.Persistence.MSSQL.Seeds;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ASSISTENTE.Persistence.MSSQL
{
    internal class AssistenteDbContext : DbContext, IAssistenteDbContext
    {
        private readonly IUserResolver? _userResolver;
        private readonly ISystemTimeProvider? _systemTimeProvider;
        private readonly IPublisher? _publisher;
        private readonly ILogger<AssistenteDbContext>? _logger;
        
        public AssistenteDbContext(DbContextOptions<AssistenteDbContext> options) : base(options)
        {
        }

        public AssistenteDbContext(
            DbContextOptions<AssistenteDbContext> options, 
            IUserResolver userResolver,
            ISystemTimeProvider systemTimeProvider,
            IPublisher publisher,
            ILogger<AssistenteDbContext> logger) 
            : base(options)
        {
            _userResolver = userResolver ?? throw new ArgumentNullException(nameof(userResolver));
            _systemTimeProvider = systemTimeProvider ?? throw new ArgumentNullException(nameof(systemTimeProvider));
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region ENTITIES

        public DbSet<Resource> Resources { get; set; } 
        public DbSet<Question> Questions { get; set; } 
        public DbSet<QuestionResource> QuestionResources { get; set; } 
        public DbSet<Answer> Answers { get; set; } 

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.SeedData();

            modelBuilder.ConfigureStrongyTypeIdentifiers();

            base.OnModelCreating(modelBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<ResourceType>()
                .HaveConversion<ResourceTypeConverter>();
            
            configurationBuilder
                .Properties<QuestionContext>()
                .HaveConversion<QuestionContextConverter>();
            
            // TODO: Prepare extension method for strongy identifiers configuration
            configurationBuilder
                .Properties<ResourceId>()
                .HaveConversion<ResourceIdConverter>();
            
            configurationBuilder
                .Properties<AnswerId>()
                .HaveConversion<AnswerIdConverter>();
            
            configurationBuilder
                .Properties<QuestionId>()
                .HaveConversion<QuestionIdConverter>();
            
            configurationBuilder
                .Properties<QuestionResourceId>()
                .HaveConversion<QuestionResourceIdConverter>();
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

            var result = await base.SaveChangesAsync(cancellationToken);

            await PublishEventsAsync(); // TODO: add in-memory outbox (outbox pattern)
            
            return result;
        }

        private async Task PublishEventsAsync()
        {
            var domainEvents = ChangeTracker
                .Entries<IEntity>()
                .Select(entry => entry.Entity)
                .SelectMany(entity =>
                {
                    var events = entity.GetEvents();
                    entity.ClearEvents();
                    return events;
                })
                .ToList();
            
            foreach (var domainEvent in domainEvents)
            {
                _logger!.LogInformation("Publishing domain event: {EventName}", domainEvent.GetType().Name);
                
                await _publisher!.Publish(domainEvent);
            }
        }
        
        public new DbSet<TEntity> Set<TEntity>() where TEntity : class, IEntity
        {
            return base.Set<TEntity>();
        }
    }
}