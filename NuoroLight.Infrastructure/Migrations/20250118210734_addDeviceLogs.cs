using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NuoroLight.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addDeviceLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Sensor",
                schema: "Device",
                newName: "Sensor",
                newSchema: "Devices");

            migrationBuilder.RenameTable(
                name: "Command",
                schema: "Device",
                newName: "Command",
                newSchema: "Devices");

            migrationBuilder.CreateTable(
                name: "DeviceLog",
                schema: "Devices",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceLog", x => x.LogId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceLog",
                schema: "Devices");

            migrationBuilder.EnsureSchema(
                name: "Device");

            migrationBuilder.RenameTable(
                name: "Sensor",
                schema: "Devices",
                newName: "Sensor",
                newSchema: "Device");

            migrationBuilder.RenameTable(
                name: "Command",
                schema: "Devices",
                newName: "Command",
                newSchema: "Device");
        }
    }
}
