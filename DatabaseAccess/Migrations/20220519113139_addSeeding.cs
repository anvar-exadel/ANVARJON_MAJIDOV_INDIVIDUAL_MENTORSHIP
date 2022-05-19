using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseAccess.Migrations
{
    public partial class addSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "UserName", "UserRole" },
                values: new object[] { 1, "Vladimir", 1 });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "UserName", "UserRole" },
                values: new object[] { 2, "Anvar", 1 });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "UserName", "UserRole" },
                values: new object[] { 3, "Jim", 0 });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "UserName", "UserRole" },
                values: new object[] { 4, "John", 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
