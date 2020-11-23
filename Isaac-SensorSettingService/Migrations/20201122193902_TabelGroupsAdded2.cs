using Microsoft.EntityFrameworkCore.Migrations;

namespace Isaac_SensorSettingService.Migrations
{
    public partial class TabelGroupsAdded2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_Group_GroupId",
                table: "Sensors");

            migrationBuilder.DropIndex(
                name: "IX_Sensors_GroupId",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Sensors");

            migrationBuilder.AddColumn<int>(
                name: "GroupId1",
                table: "Sensors",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_GroupId1",
                table: "Sensors",
                column: "GroupId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_Group_GroupId1",
                table: "Sensors",
                column: "GroupId1",
                principalTable: "Group",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_Group_GroupId1",
                table: "Sensors");

            migrationBuilder.DropIndex(
                name: "IX_Sensors_GroupId1",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "GroupId1",
                table: "Sensors");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Sensors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_GroupId",
                table: "Sensors",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_Group_GroupId",
                table: "Sensors",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
