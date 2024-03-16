using Microsoft.EntityFrameworkCore;

namespace ASSISTENTE.Persistence.MSSQL.Utils
{
    internal class AssistenteDbContextFactory : DesignTimeDbContextFactoryBase<AssistenteDbContext>
    {
        protected override AssistenteDbContext CreateNewInstance(DbContextOptions<AssistenteDbContext> options)
        {
            return new AssistenteDbContext(options);
        }
    }
}
