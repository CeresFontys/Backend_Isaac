using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Isaac_AnomalyService.Migrations
{
    public partial class NewSensorErrorLogDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Errors",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    X = table.Column<int>(nullable: false),
                    Y = table.Column<int>(nullable: false),
                    Floor = table.Column<int>(nullable: false),
                    Error = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false),
                    DateTimeNext = table.Column<DateTime>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    ValueFirst = table.Column<double>(nullable: false),
                    ValueSecond = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Errors", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Errors");
        }
    }
}
