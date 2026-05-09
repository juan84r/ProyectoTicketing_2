using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigracionInicialLimpia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Action = table.Column<string>(type: "text", nullable: false),
                    User = table.Column<string>(type: "text", nullable: false),
                    Resource = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    EventDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Venue = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sectors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EventId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sectors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sectors_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SectorId = table.Column<int>(type: "integer", nullable: false),
                    RowIdentifier = table.Column<string>(type: "text", nullable: false),
                    SeatNumber = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seats_Sectors_SectorId",
                        column: x => x.SectorId,
                        principalTable: "Sectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SeatId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ReservedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SeatNumber = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Seats_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Seats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "EventDate", "Name", "Status", "Venue" },
                values: new object[] { 1, new DateTime(2026, 12, 10, 21, 0, 0, 0, DateTimeKind.Utc), "Concierto de Rock", "Active", "Estadio Principal" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Password", "Role" },
                values: new object[] { 1, "admin@test.com", "$2a$11$faVCjgww4M15XqVz2hRIu.p3ZdAi7nhYnz3oq4ZmU5.gdvWHMU5lO", "Admin" });

            migrationBuilder.InsertData(
                table: "Sectors",
                columns: new[] { "Id", "Capacity", "EventId", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 50, 1, "Sector A", 5000m },
                    { 2, 50, 1, "Sector B", 8000m }
                });

            migrationBuilder.InsertData(
                table: "Seats",
                columns: new[] { "Id", "RowIdentifier", "SeatNumber", "SectorId", "Status", "Version" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0001-000000000001"), "A", 1, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000002"), "A", 2, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000003"), "A", 3, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000004"), "A", 4, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000005"), "A", 5, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000006"), "A", 6, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000007"), "A", 7, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000008"), "A", 8, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000009"), "A", 9, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000010"), "A", 10, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000011"), "A", 11, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000012"), "A", 12, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000013"), "A", 13, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000014"), "A", 14, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000015"), "A", 15, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000016"), "A", 16, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000017"), "A", 17, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000018"), "A", 18, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000019"), "A", 19, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000020"), "A", 20, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000021"), "A", 21, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000022"), "A", 22, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000023"), "A", 23, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000024"), "A", 24, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000025"), "A", 25, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000026"), "A", 26, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000027"), "A", 27, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000028"), "A", 28, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000029"), "A", 29, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000030"), "A", 30, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000031"), "A", 31, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000032"), "A", 32, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000033"), "A", 33, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000034"), "A", 34, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000035"), "A", 35, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000036"), "A", 36, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000037"), "A", 37, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000038"), "A", 38, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000039"), "A", 39, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000040"), "A", 40, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000041"), "A", 41, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000042"), "A", 42, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000043"), "A", 43, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000044"), "A", 44, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000045"), "A", 45, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000046"), "A", 46, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000047"), "A", 47, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000048"), "A", 48, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000049"), "A", 49, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000050"), "A", 50, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000001"), "B", 1, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000002"), "B", 2, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000003"), "B", 3, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000004"), "B", 4, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000005"), "B", 5, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000006"), "B", 6, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000007"), "B", 7, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000008"), "B", 8, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000009"), "B", 9, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000010"), "B", 10, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000011"), "B", 11, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000012"), "B", 12, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000013"), "B", 13, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000014"), "B", 14, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000015"), "B", 15, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000016"), "B", 16, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000017"), "B", 17, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000018"), "B", 18, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000019"), "B", 19, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000020"), "B", 20, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000021"), "B", 21, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000022"), "B", 22, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000023"), "B", 23, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000024"), "B", 24, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000025"), "B", 25, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000026"), "B", 26, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000027"), "B", 27, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000028"), "B", 28, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000029"), "B", 29, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000030"), "B", 30, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000031"), "B", 31, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000032"), "B", 32, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000033"), "B", 33, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000034"), "B", 34, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000035"), "B", 35, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000036"), "B", 36, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000037"), "B", 37, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000038"), "B", 38, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000039"), "B", 39, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000040"), "B", 40, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000041"), "B", 41, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000042"), "B", 42, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000043"), "B", 43, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000044"), "B", 44, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000045"), "B", 45, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000046"), "B", 46, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000047"), "B", 47, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000048"), "B", 48, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000049"), "B", 49, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000050"), "B", 50, 2, "Available", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_SeatId",
                table: "Reservations",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_SectorId_SeatNumber",
                table: "Seats",
                columns: new[] { "SectorId", "SeatNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sectors_EventId",
                table: "Sectors",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropTable(
                name: "Sectors");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
