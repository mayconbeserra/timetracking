using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Visma.TimeTracking.MigrationsProjection.Migrations
{
    [DbContext(typeof(ProjectionsDbContextMigr))]
    [Migration("20170127212028_InitialDbStructure-Projections")]
    partial class InitialDbStructureProjections
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("Visma.TimerTracking.Projections.Entities.Activity", b =>
                {
                    b.Property<string>("Id");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatorId");

                    b.Property<string>("Description");

                    b.Property<DateTime?>("EndDate");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("ProjectId");

                    b.Property<DateTime>("StartDate");

                    b.Property<long>("Version")
                        .IsConcurrencyToken();

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("Visma.TimerTracking.Projections.Entities.Customer", b =>
                {
                    b.Property<string>("Id");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatorId");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("Name");

                    b.Property<long>("Version")
                        .IsConcurrencyToken();

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Visma.TimerTracking.Projections.Entities.Project", b =>
                {
                    b.Property<string>("Id");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatorId");

                    b.Property<string>("CustomerId");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("Name");

                    b.Property<long>("Version")
                        .IsConcurrencyToken();

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Visma.TimerTracking.Projections.Entities.Activity", b =>
                {
                    b.HasOne("Visma.TimerTracking.Projections.Entities.Project")
                        .WithMany("Activities")
                        .HasForeignKey("ProjectId");
                });
        }
    }
}
