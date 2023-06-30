using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CollectionFieldTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "001e472a-8735-410a-9b8d-96fe85a89011", "TEXT" },
                    { "16f60c19-4586-4029-85ee-3603981174f7", "INT" },
                    { "1f25f424-cfa9-4d3d-b147-4902c6242579", "STR" },
                    { "3f3179d0-25bd-452b-9631-36043eb1e93b", "DATE" },
                    { "adc1f2b9-8283-43c4-99c3-cb39816db869", "BOOL" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CollectionFieldTypes",
                keyColumn: "Id",
                keyValue: "001e472a-8735-410a-9b8d-96fe85a89011");

            migrationBuilder.DeleteData(
                table: "CollectionFieldTypes",
                keyColumn: "Id",
                keyValue: "16f60c19-4586-4029-85ee-3603981174f7");

            migrationBuilder.DeleteData(
                table: "CollectionFieldTypes",
                keyColumn: "Id",
                keyValue: "1f25f424-cfa9-4d3d-b147-4902c6242579");

            migrationBuilder.DeleteData(
                table: "CollectionFieldTypes",
                keyColumn: "Id",
                keyValue: "3f3179d0-25bd-452b-9631-36043eb1e93b");

            migrationBuilder.DeleteData(
                table: "CollectionFieldTypes",
                keyColumn: "Id",
                keyValue: "adc1f2b9-8283-43c4-99c3-cb39816db869");
        }
    }
}
