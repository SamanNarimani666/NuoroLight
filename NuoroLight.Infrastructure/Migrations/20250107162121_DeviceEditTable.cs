using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NuoroLight.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeviceEditTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Device_User_UserId",
                schema: "Devices",
                table: "Device");

            migrationBuilder.DropIndex(
                name: "IX_Device_UserId",
                schema: "Devices",
                table: "Device");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "Devices",
                table: "Device");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                schema: "Devices",
                table: "Device",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Device_UserId",
                schema: "Devices",
                table: "Device",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Device_User_UserId",
                schema: "Devices",
                table: "Device",
                column: "UserId",
                principalSchema: "Users",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
