using Microsoft.EntityFrameworkCore;

namespace Visma.TimeTracking.MigrationsProjection
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new ProjectionsDbContextMigr()
                .Database
                .Migrate();

            System.Console.WriteLine("Projections Migrated --- OK");
        }
    }
}