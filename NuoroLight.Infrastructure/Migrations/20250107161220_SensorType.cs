using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NuoroLight.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SensorType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SensorType",
                schema: "Device",
                table: "Sensor",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SensorType",
                schema: "Device",
                table: "Sensor");
        }
    }
}
