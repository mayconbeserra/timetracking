using System;
using System.Reflection;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SimpleInjector;
using SimpleInjector.Extensions.LifetimeScoping;
using Visma.TimeTracking.Domain.Events.V1;
using Visma.TimeTracking.EventSourcing.Bus;
using Visma.TimeTracking.EventSourcing.Db;
using Visma.TimeTracking.EventSourcing.Events;
using Visma.TimeTracking.EventSourcing.Handlers;
using Visma.TimeTracking.Projections;
using Visma.TimeTracking.Projections.Contexts;
using Visma.TimeTracking.Projections.Entities;

namespace Visma.TimeTracking.Tests.Repositories
{
    public abstract class RepositoryBase
    {
        protected TimerTrackingDbContext TimerTrackingDbContext => Container.GetInstance<TimerTrackingDbContext>();

        protected RepositoryBase()
        {
            Container = new Container();
            Container.Options.DefaultScopedLifestyle = new LifetimeScopeLifestyle();

            SetupSqlite();
            var handlersAssembly = typeof(EventProcessor).GetTypeInfo().Assembly;
            Container.RegisterCollection(typeof(IEventHandler<ActivityStarted>), handlersAssembly);
            Container.RegisterCollection(typeof(IEventHandler<ActivityPaused>), handlersAssembly);
            Container.RegisterCollection(typeof(IEventHandler<ActivityAdjusted>), handlersAssembly);
            Container.Register<IEventStore, EventStore>(Lifestyle.Singleton);
            Container.Register<IEventPublisher, EventProcessor>(Lifestyle.Singleton);
            Container.Register<IDomainRepository, DomainRepository>(Lifestyle.Singleton);
        }

        protected Container Container { get; }

        protected void SetupSqlite()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);
            connection.Open();

            var connectionStringBuilder2 = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connectionString2 = connectionStringBuilder2.ToString();
            var connection2 = new SqliteConnection(connectionString2);
            connection2.Open();

            var dbOptionsProjections = new DbContextOptionsBuilder<DbContext>()
                .UseSqlite(connection, x => x.MigrationsAssembly("Visma.TimeTracking.MigrationsProjection"))
                .Options;

            var dbOptionsEventStore = new DbContextOptionsBuilder<DbContext>()
                .UseSqlite(connection2, x => x.MigrationsAssembly("Visma.TimeTracking.MigrationsEventSourcing"))
                .Options;

            var eventStoreDb = new EventStoreDbContext(dbOptionsEventStore);
            var projectionsDb = new TimerTrackingDbContext(dbOptionsProjections);

            EnsureDatabaseCreated(projectionsDb);
            EnsureDatabaseCreated(eventStoreDb);

            Container.Register(() => eventStoreDb, Lifestyle.Singleton);
            Container.Register(() => projectionsDb, Lifestyle.Singleton);
        }

        protected void EnsureDatabaseCreated(DbContext context)
        {
            context.Database.OpenConnection();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Database.Migrate();
        }

        protected string CreateProject()
        {
            var id = Guid.NewGuid().ToString();

            TimerTrackingDbContext.Projects.Add(new Project
            {
                Id = id,
                Name = "Test",
                CreatedAt = DateTime.UtcNow,
                CreatorId = "1"
            });
            TimerTrackingDbContext.SaveChanges();

            return id;
        }
    }
}