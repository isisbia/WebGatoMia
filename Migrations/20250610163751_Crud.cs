using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebGatoMia.Migrations
{
    /// <inheritdoc />
    public partial class Crud : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_UserType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_UserType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DateRegistration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_User_tb_UserType_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "tb_UserType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "tb_UserType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Gerente" },
                    { 3, "Padrão" },
                    { 4, "Responsável" },
                    { 5, "Psicólogo" },
                    { 6, "Advogado" }
                });

            migrationBuilder.InsertData(
                table: "tb_User",
                columns: new[] { "Id", "DateRegistration", "Email", "IsActive", "Name", "PasswordHash", "Phone", "UserTypeId" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2025, 6, 10, 16, 37, 50, 591, DateTimeKind.Utc).AddTicks(1790), "maria@gatomia.com", true, "Maria Silva", "hashedpassword1", "(11) 99999-9999", 1 },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2025, 6, 10, 16, 37, 50, 591, DateTimeKind.Utc).AddTicks(1811), "joao@gatomia.com", true, "João Santos", "hashedpassword2", "(11) 88888-8888", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_User_Email",
                table: "tb_User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_User_UserTypeId",
                table: "tb_User",
                column: "UserTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_User");

            migrationBuilder.DropTable(
                name: "tb_UserType");
        }
    }
}
