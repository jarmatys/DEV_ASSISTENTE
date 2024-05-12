using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ASSISTENTE.Persistence.Configuration
{
    public class AssistenteDbContextFactory : DesignTimeDbContextFactoryBase<AssistenteDbContext>
    {
        protected override AssistenteDbContext CreateNewInstance(DbContextOptions<AssistenteDbContext> options)
        {
            return new AssistenteDbContext(options);
        }
    }

    public abstract class DesignTimeDbContextFactoryBase<TContext> : IDesignTimeDbContextFactory<TContext>
        where TContext : DbContext
    {
        public TContext CreateDbContext(string[]? args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TContext>();

            // TODO: Add DB switcher
            // optionsBuilder.UseSqlServer();
            optionsBuilder.UseNpgsql();

            return CreateNewInstance(optionsBuilder.Options);
        }

        protected abstract TContext CreateNewInstance(DbContextOptions<TContext> options);
    }
}