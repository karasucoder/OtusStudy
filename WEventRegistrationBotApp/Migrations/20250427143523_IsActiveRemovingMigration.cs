using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEventRegistrationBotApp.Migrations
{
    /// <inheritdoc />
    public partial class IsActiveRemovingMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "WineEvents");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "WineEvents",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
