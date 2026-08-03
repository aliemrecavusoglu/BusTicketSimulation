using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusTicketSimulation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPassengerInfoToSoldSeats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "SoldSeats",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "SoldSeats",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "SoldSeats",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TcIdentity",
                table: "SoldSeats",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "SoldSeats");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "SoldSeats");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "SoldSeats");

            migrationBuilder.DropColumn(
                name: "TcIdentity",
                table: "SoldSeats");
        }
    }
}
