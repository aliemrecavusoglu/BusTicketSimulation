using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusTicketSimulation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserRoleToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"UPDATE ""Users"" SET ""Role"" = '1' WHERE ""Role"" = 'User' OR ""Role"" IS NULL;");
            migrationBuilder.Sql(@"ALTER TABLE ""Users"" ALTER COLUMN ""Role"" TYPE integer USING (""Role""::integer);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
