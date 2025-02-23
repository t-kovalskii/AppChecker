using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppAvailabilityTracker.Services.AppStorage.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SchemaChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:store_type", "GooglePlay,AppStore");

            migrationBuilder.AddColumn<string>(
                name: "store",
                table: "applications",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "store",
                table: "applications");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:store_type", "GooglePlay,AppStore");
        }
    }
}
