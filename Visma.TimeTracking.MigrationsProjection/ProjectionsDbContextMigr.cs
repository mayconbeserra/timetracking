using System;
using Microsoft.EntityFrameworkCore;
using Visma.TimeTracking.Projections.Contexts;

namespace Visma.TimeTracking.MigrationsProjection
{
    public class ProjectionsDbContextMigr : TimerTrackingDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
                                   ?? "User ID=postgres;Password=sql;Host=localhost;Port=5432;Database=timertracking;Pooling=true;";

            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}