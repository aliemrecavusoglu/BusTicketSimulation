using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusTicketSimulation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPnrToSoldSeats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PnrNumber",
                table: "SoldSeats",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PnrNumber",
                table: "SoldSeats");
        }
    }
}
