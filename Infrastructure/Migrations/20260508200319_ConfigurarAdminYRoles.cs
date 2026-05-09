using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConfigurarAdminYRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Seats_SeatNumber",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Seats_SectorId",
                table: "Seats");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Concierto de Rock Inicial");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Role",
                value: "Admin");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_SectorId_SeatNumber",
                table: "Seats",
                columns: new[] { "SectorId", "SeatNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Seats_SectorId_SeatNumber",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Concierto de Rock");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_SeatNumber",
                table: "Seats",
                column: "SeatNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seats_SectorId",
                table: "Seats",
                column: "SectorId");
        }
    }
}
