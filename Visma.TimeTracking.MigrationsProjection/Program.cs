using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Visma.TimeTracking.Projections.Contexts;
using Visma.TimeTracking.Projections.Entities;

namespace Visma.TimeTracking.MigrationsProjection
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var context = new ProjectionsDbContextMigr();

            context
                .Database
                .Migrate();

            SeedData(context);

            Console.WriteLine("Projections Migrated --- OK");
        }

        private static void SeedData(TimerTrackingDbContext context)
        {
            var customer = context.Customers.FirstOrDefault(x => x.Name == "Customer Default");
            var customerId = Guid.NewGuid().ToString();
            var seedApplied = false;

            if (customer == null)
            {
                seedApplied = true;
                context.Customers.Add(new Customer
                {
                    Id = customerId,
                    Name = "Customer Default",
                    CreatedAt = DateTime.UtcNow,
                    CreatorId = "CreatorId"
                });
            }

            var project = context.Projects.FirstOrDefault(x => x.Name == "Project Default");

            if (project == null)
            {
                seedApplied = true;
                context.Projects.Add(new Project
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Project Default",
                    CreatedAt = DateTime.UtcNow,
                    CreatorId = "CreatorId",
                    CustomerId = customerId
                });
            }

            if (seedApplied) Console.WriteLine("Seed Data --> Applied");

            context.SaveChanges();
        }
    }
}