using Microsoft.EntityFrameworkCore.Migrations;

namespace Isaac_SensorSettingService.Migrations
{
    public partial class uiIndexField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "uiIndex",
                table: "Sensors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "uiIndex",
                table: "Group",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "uiIndex",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "uiIndex",
                table: "Group");
        }
    }
}
