using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppAvailabilityTracker.Services.AppChecker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "availability_checks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    application_id = table.Column<Guid>(type: "uuid", nullable: false),
                    app_link = table.Column<string>(type: "text", nullable: false),
                    check_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    check_result = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("availability_checks_pkey", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "availability_checks");
        }
    }
}
