using ASSISTENTE.Persistence.MSSQL.Utils;
using Microsoft.EntityFrameworkCore;

namespace ASSISTENTE.DB.Upgrade
{
    internal abstract class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Starting database upgrade...");

                var dbContextFactory = new AssistenteDbContextFactory();
                
                var context = dbContextFactory.CreateDbContext(args);

                context.Database.Migrate();

                Console.WriteLine("Database upgrade completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FAIL: {ex.Message}");
            }
        }
    }
}
