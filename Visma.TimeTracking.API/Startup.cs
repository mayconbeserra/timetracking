using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore;
using SimpleInjector.Integration.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;
using Visma.TimeTracking.AppService;
using Visma.TimeTracking.Domain.Events.V1;
using Visma.TimeTracking.EventSourcing.Bus;
using Visma.TimeTracking.EventSourcing.Db;
using Visma.TimeTracking.EventSourcing.Events;
using Visma.TimeTracking.EventSourcing.Handlers;
using Visma.TimeTracking.Projections;
using Visma.TimeTracking.Projections.Contexts;

namespace Visma.TimeTracking.API
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        protected Container Container { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            Container = new Container();
            Container.Options.DefaultScopedLifestyle = new AspNetRequestLifestyle();
        }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services
                .AddOptions();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            services
                .AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(Container))
                .AddSingleton<IViewComponentActivator>(new SimpleInjectorViewComponentActivator(Container));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime serverLifetime)
        {
            SetupContainer(app);

            app.UseSwagger();
            app.UseMvc();

            app.UseSwaggerUi(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }

        private void SetupContainer(IApplicationBuilder server)
        {
            server.UseSimpleInjectorAspNetRequestScoping(Container);

            Container.Options.DefaultScopedLifestyle = new AspNetRequestLifestyle();
            Container.RegisterMvcControllers(server);
            Container.RegisterMvcViewComponents(server);

            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            var dbOptions = new DbContextOptionsBuilder<DbContext>()
                .UseNpgsql(connectionString)
                .Options;

            Container.Register(() => new DbContextOptionsBuilder<DbContext>()
                .UseNpgsql(new NpgsqlConnection(connectionString))
                .Options, Lifestyle.Scoped);
            Container.Register(() => new EventStoreDbContext(dbOptions), Lifestyle.Scoped);
            Container.Register(() => new TimerTrackingDbContext(dbOptions), Lifestyle.Scoped);

            Container.Register<IEventStore, EventStore>(Lifestyle.Scoped);
            Container.Register<IDomainRepository, DomainRepository>(Lifestyle.Scoped);
            Container.Register<IEventPublisher, EventProcessor>(Lifestyle.Scoped);
            Container.Register<IActivityService, ActivityService>(Lifestyle.Scoped);

            var handlersAssembly = typeof(EventProcessor).GetTypeInfo().Assembly;
            Container.RegisterCollection(typeof(IEventHandler<ActivityStarted>), handlersAssembly);
            Container.RegisterCollection(typeof(IEventHandler<ActivityPaused>), handlersAssembly);
            Container.RegisterCollection(typeof(IEventHandler<ActivityAdjusted>), handlersAssembly);

            Container.RegisterSingleton(server.ApplicationServices.GetService<ILoggerFactory>());
//            Container.RegisterSingleton(server.ApplicationServices.GetService<IOptions<ConsoleClientViewModel>>());
        }
    }
}