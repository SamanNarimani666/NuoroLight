using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NuoroLight.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditCommandTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CommandText",
                schema: "Device",
                table: "Command",
                newName: "Value");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Device",
                table: "Command",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Device",
                table: "Command");

            migrationBuilder.RenameColumn(
                name: "Value",
                schema: "Device",
                table: "Command",
                newName: "CommandText");
        }
    }
}
