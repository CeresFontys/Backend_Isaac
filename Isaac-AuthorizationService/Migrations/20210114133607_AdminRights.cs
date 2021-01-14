using Microsoft.EntityFrameworkCore.Migrations;

namespace Isaac_AuthorizationService.Migrations
{
    public partial class AdminRights : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsAdmin",
                table: "Users",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Users");
        }
    }
}
