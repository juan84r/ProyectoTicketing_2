using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    LockUntil = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LockedByUserId = table.Column<int>(type: "integer", nullable: true),
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
                values: new object[] { 1, "admin@test.com", "$2a$11$whs5JNbyQG33YGHFcwMqyOKfEz25blxlJ/lVrY9aRPbMJQFLjbFCK", "Admin" });

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
                columns: new[] { "Id", "LockUntil", "LockedByUserId", "RowIdentifier", "SeatNumber", "SectorId", "Status", "Version" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0001-000000000001"), null, null, "A", 1, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000002"), null, null, "A", 2, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000003"), null, null, "A", 3, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000004"), null, null, "A", 4, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000005"), null, null, "A", 5, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000006"), null, null, "A", 6, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000007"), null, null, "A", 7, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000008"), null, null, "A", 8, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000009"), null, null, "A", 9, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000010"), null, null, "A", 10, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000011"), null, null, "A", 11, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000012"), null, null, "A", 12, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000013"), null, null, "A", 13, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000014"), null, null, "A", 14, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000015"), null, null, "A", 15, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000016"), null, null, "A", 16, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000017"), null, null, "A", 17, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000018"), null, null, "A", 18, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000019"), null, null, "A", 19, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000020"), null, null, "A", 20, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000021"), null, null, "A", 21, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000022"), null, null, "A", 22, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000023"), null, null, "A", 23, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000024"), null, null, "A", 24, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000025"), null, null, "A", 25, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000026"), null, null, "A", 26, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000027"), null, null, "A", 27, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000028"), null, null, "A", 28, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000029"), null, null, "A", 29, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000030"), null, null, "A", 30, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000031"), null, null, "A", 31, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000032"), null, null, "A", 32, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000033"), null, null, "A", 33, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000034"), null, null, "A", 34, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000035"), null, null, "A", 35, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000036"), null, null, "A", 36, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000037"), null, null, "A", 37, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000038"), null, null, "A", 38, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000039"), null, null, "A", 39, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000040"), null, null, "A", 40, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000041"), null, null, "A", 41, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000042"), null, null, "A", 42, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000043"), null, null, "A", 43, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000044"), null, null, "A", 44, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000045"), null, null, "A", 45, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000046"), null, null, "A", 46, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000047"), null, null, "A", 47, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000048"), null, null, "A", 48, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000049"), null, null, "A", 49, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0001-000000000050"), null, null, "A", 50, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000001"), null, null, "B", 1, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000002"), null, null, "B", 2, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000003"), null, null, "B", 3, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000004"), null, null, "B", 4, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000005"), null, null, "B", 5, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000006"), null, null, "B", 6, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000007"), null, null, "B", 7, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000008"), null, null, "B", 8, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000009"), null, null, "B", 9, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000010"), null, null, "B", 10, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000011"), null, null, "B", 11, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000012"), null, null, "B", 12, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000013"), null, null, "B", 13, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000014"), null, null, "B", 14, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000015"), null, null, "B", 15, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000016"), null, null, "B", 16, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000017"), null, null, "B", 17, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000018"), null, null, "B", 18, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000019"), null, null, "B", 19, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000020"), null, null, "B", 20, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000021"), null, null, "B", 21, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000022"), null, null, "B", 22, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000023"), null, null, "B", 23, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000024"), null, null, "B", 24, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000025"), null, null, "B", 25, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000026"), null, null, "B", 26, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000027"), null, null, "B", 27, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000028"), null, null, "B", 28, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000029"), null, null, "B", 29, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000030"), null, null, "B", 30, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000031"), null, null, "B", 31, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000032"), null, null, "B", 32, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000033"), null, null, "B", 33, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000034"), null, null, "B", 34, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000035"), null, null, "B", 35, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000036"), null, null, "B", 36, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000037"), null, null, "B", 37, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000038"), null, null, "B", 38, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000039"), null, null, "B", 39, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000040"), null, null, "B", 40, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000041"), null, null, "B", 41, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000042"), null, null, "B", 42, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000043"), null, null, "B", 43, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000044"), null, null, "B", 44, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000045"), null, null, "B", 45, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000046"), null, null, "B", 46, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000047"), null, null, "B", 47, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000048"), null, null, "B", 48, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000049"), null, null, "B", 49, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0002-000000000050"), null, null, "B", 50, 2, "Available", 1 }
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
