using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9d350b27-845d-41a5-a99a-9baa9ce7eded", "26e08d1a-8f0f-4318-8dcc-a9f6e76376fe", "Admin", "ADMIN" },
                    { "f244e4db-c521-49d4-826b-2c9945c22478", "70658725-3022-47e5-9d85-a8b018fbbc5b", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9d350b27-845d-41a5-a99a-9baa9ce7eded");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f244e4db-c521-49d4-826b-2c9945c22478");
        }
    }
}
