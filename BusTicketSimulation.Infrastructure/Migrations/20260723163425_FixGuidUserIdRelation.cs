using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusTicketSimulation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixGuidUserIdRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoldSeats_Users_UserId",
                table: "SoldSeats");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "SoldSeats",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SoldSeats_Users_UserId",
                table: "SoldSeats",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoldSeats_Users_UserId",
                table: "SoldSeats");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "SoldSeats",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_SoldSeats_Users_UserId",
                table: "SoldSeats",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
