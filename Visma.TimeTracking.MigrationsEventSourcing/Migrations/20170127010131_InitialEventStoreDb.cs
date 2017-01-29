using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Visma.TimeTracking.MigrationsEventSourcing.Migrations
{
    public partial class InitialEventStoreDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CommitId = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<string>(nullable: true),
                    Payload = table.Column<string>(nullable: false),
                    StreamId = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    Version = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Streams",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Version = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Streams", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Streams");
        }
    }
}
