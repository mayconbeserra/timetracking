using Microsoft.EntityFrameworkCore;
using Visma.TimeTracking.MigrationsEventSourcing.EventStore;

namespace Visma.TimeTracking.MigrationsEventSourcing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            System.Console.WriteLine("## Starting migrations ##");

            new EventStoreDbContextMigr()
                .Database
                .Migrate();

            System.Console.WriteLine("EventStore Migrated --- OK");
        }
    }
}