using Microsoft.EntityFrameworkCore.Migrations;

namespace Isaac_AuthorizationService.Migrations
{
    public partial class PasswordToLong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
