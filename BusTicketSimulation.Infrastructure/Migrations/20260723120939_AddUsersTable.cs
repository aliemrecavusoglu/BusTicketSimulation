using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusTicketSimulation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "SoldSeats",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "bytea", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "bytea", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    CreadDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SoldSeats_UserId",
                table: "SoldSeats",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SoldSeats_Users_UserId",
                table: "SoldSeats",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoldSeats_Users_UserId",
                table: "SoldSeats");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_SoldSeats_UserId",
                table: "SoldSeats");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SoldSeats");
        }
    }
}
