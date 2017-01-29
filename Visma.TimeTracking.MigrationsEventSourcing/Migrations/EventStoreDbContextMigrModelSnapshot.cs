using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Visma.TimeTracking.MigrationsEventSourcing.EventStore;

namespace Visma.TimeTracking.MigrationsEventSourcing.Migrations
{
    [DbContext(typeof(EventStoreDbContextMigr))]
    partial class EventStoreDbContextMigrModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("Visma.TimerTracking.EventSourcing.Models.Event", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("CommitId")
                        .IsRequired();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatorId");

                    b.Property<string>("Payload")
                        .IsRequired();

                    b.Property<string>("StreamId")
                        .IsRequired();

                    b.Property<string>("Type")
                        .IsRequired();

                    b.Property<long>("Version");

                    b.HasKey("Id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Visma.TimerTracking.EventSourcing.Models.Stream", b =>
                {
                    b.Property<string>("Id");

                    b.Property<long>("Version")
                        .IsConcurrencyToken();

                    b.HasKey("Id");

                    b.ToTable("Streams");
                });
        }
    }
}
