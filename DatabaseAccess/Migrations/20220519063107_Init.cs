using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseAccess.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeatherForecasts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Cnt = table.Column<int>(type: "INTEGER", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherForecasts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebWeathers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Lon = table.Column<double>(type: "REAL", nullable: false),
                    Lat = table.Column<double>(type: "REAL", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Comment = table.Column<string>(type: "TEXT", nullable: true),
                    Temperature = table.Column<double>(type: "REAL", nullable: false),
                    WeatherDay = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebWeathers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebDailyTemps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Day = table.Column<double>(type: "REAL", nullable: false),
                    Min = table.Column<double>(type: "REAL", nullable: false),
                    Max = table.Column<double>(type: "REAL", nullable: false),
                    WebWeatherForecastId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebDailyTemps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebDailyTemps_WeatherForecasts_WebWeatherForecastId",
                        column: x => x.WebWeatherForecastId,
                        principalTable: "WeatherForecasts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WebDailyTemps_WebWeatherForecastId",
                table: "WebDailyTemps",
                column: "WebWeatherForecastId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebDailyTemps");

            migrationBuilder.DropTable(
                name: "WebWeathers");

            migrationBuilder.DropTable(
                name: "WeatherForecasts");
        }
    }
}
