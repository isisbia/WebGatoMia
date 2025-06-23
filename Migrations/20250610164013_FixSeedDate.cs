using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebGatoMia.Migrations
{
    /// <inheritdoc />
    public partial class FixSeedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "tb_User",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "DateRegistration",
                value: new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "tb_User",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "DateRegistration",
                value: new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "tb_User",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "DateRegistration",
                value: new DateTime(2025, 6, 10, 16, 37, 50, 591, DateTimeKind.Utc).AddTicks(1790));

            migrationBuilder.UpdateData(
                table: "tb_User",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "DateRegistration",
                value: new DateTime(2025, 6, 10, 16, 37, 50, 591, DateTimeKind.Utc).AddTicks(1811));
        }
    }
}
