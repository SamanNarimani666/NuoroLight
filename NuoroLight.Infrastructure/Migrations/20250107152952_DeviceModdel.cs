using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NuoroLight.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeviceModdel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                schema: "Users",
                table: "User");

            migrationBuilder.EnsureSchema(
                name: "Device");

            migrationBuilder.EnsureSchema(
                name: "Devices");

            migrationBuilder.CreateTable(
                name: "Device",
                schema: "Devices",
                columns: table => new
                {
                    DeviceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MAC = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Device", x => x.DeviceId);
                    table.ForeignKey(
                        name: "FK_Device_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Users",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sensor",
                schema: "Device",
                columns: table => new
                {
                    SensorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    DeviceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensor", x => x.SensorId);
                    table.ForeignKey(
                        name: "FK_Sensor_Device_DeviceId",
                        column: x => x.DeviceId,
                        principalSchema: "Devices",
                        principalTable: "Device",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Command",
                schema: "Device",
                columns: table => new
                {
                    CommandId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Display = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommandText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SensorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Command", x => x.CommandId);
                    table.ForeignKey(
                        name: "FK_Command_Sensor_SensorId",
                        column: x => x.SensorId,
                        principalSchema: "Device",
                        principalTable: "Sensor",
                        principalColumn: "SensorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Command_SensorId",
                schema: "Device",
                table: "Command",
                column: "SensorId");

            migrationBuilder.CreateIndex(
                name: "IX_Device_UserId",
                schema: "Devices",
                table: "Device",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensor_DeviceId",
                schema: "Device",
                table: "Sensor",
                column: "DeviceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Command",
                schema: "Device");

            migrationBuilder.DropTable(
                name: "Sensor",
                schema: "Device");

            migrationBuilder.DropTable(
                name: "Device",
                schema: "Devices");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                schema: "Users",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
