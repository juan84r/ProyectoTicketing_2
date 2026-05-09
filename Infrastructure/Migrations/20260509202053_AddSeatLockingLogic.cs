using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSeatLockingLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Reservations");

            migrationBuilder.AddColumn<DateTime>(
                name: "LockUntil",
                table: "Seats",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LockedByUserId",
                table: "Seats",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000001"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000002"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000003"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000004"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000005"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000006"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000007"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000008"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000009"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000010"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000011"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000012"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000013"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000014"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000015"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000016"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000017"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000018"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000019"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000020"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000021"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000022"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000023"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000024"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000025"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000026"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000027"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000028"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000029"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000030"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000031"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000032"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000033"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000034"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000035"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000036"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000037"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000038"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000039"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000040"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000041"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000042"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000043"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000044"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000045"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000046"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000047"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000048"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000049"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0001-000000000050"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000001"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000002"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000003"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000004"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000005"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000006"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000007"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000008"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000009"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000010"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000011"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000012"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000013"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000014"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000015"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000016"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000017"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000018"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000019"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000020"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000021"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000022"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000023"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000024"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000025"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000026"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000027"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000028"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000029"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000030"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000031"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000032"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000033"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000034"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000035"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000036"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000037"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000038"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000039"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000040"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000041"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000042"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000043"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000044"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000045"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000046"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000047"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000048"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000049"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000050"),
                columns: new[] { "LockUntil", "LockedByUserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$QPLqKvyy.sbo8oNVWJv/seAPJaqVqlD6FF9K5OqSaCMbxZnUzLj5O");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LockUntil",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "LockedByUserId",
                table: "Seats");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Reservations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$faVCjgww4M15XqVz2hRIu.p3ZdAi7nhYnz3oq4ZmU5.gdvWHMU5lO");
        }
    }
}
