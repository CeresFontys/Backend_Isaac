using Microsoft.EntityFrameworkCore.Migrations;

namespace Isaac_FloorService.Migrations
{
    public partial class added_basename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BaseName",
                table: "Floor",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseName",
                table: "Floor");
        }
    }
}
