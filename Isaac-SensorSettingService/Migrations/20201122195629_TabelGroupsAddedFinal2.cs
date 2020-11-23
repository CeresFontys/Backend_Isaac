using Microsoft.EntityFrameworkCore.Migrations;

namespace Isaac_SensorSettingService.Migrations
{
    public partial class TabelGroupsAddedFinal2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_Group_GroupId",
                table: "Sensors");

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "Sensors",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_Group_GroupId",
                table: "Sensors",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_Group_GroupId",
                table: "Sensors");

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "Sensors",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_Group_GroupId",
                table: "Sensors",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
