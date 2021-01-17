using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Isaac_AnomalyService.Migrations
{
    public partial class createdb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Errors",
                columns: table => new
                {
                    X = table.Column<int>(nullable: false),
                    Y = table.Column<int>(nullable: false),
                    Floor = table.Column<int>(nullable: false),
                    ValueType = table.Column<string>(nullable: false),
                    id = table.Column<string>(nullable: true),
                    Error = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false),
                    DateTimeNext = table.Column<DateTime>(nullable: false),
                    ApiValue = table.Column<double>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    ValueFirst = table.Column<double>(nullable: false),
                    ValueSecond = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Errors", x => new { x.X, x.Y, x.Floor, x.ValueType });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Errors");
        }
    }
}
